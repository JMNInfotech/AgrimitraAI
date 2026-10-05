-- Database smoke/constraint tests. Runs in a transaction and rolls back. Usage: psql "$DATABASE_URL" -v ON_ERROR_STOP=1 -f database/tests/smoke.sql
begin;
do $$
declare
    u uuid; cu uuid; fp uuid; cp uuid; f uuid; l uuid; cy uuid; act uuid; occ uuid; pay uuid; ord uuid; rx uuid; v int;
    crop uuid := (select id from crops where code='grapes');
    ok boolean;
    procedure_expect_fail text;
begin
    -- seeds
    assert (select count(*) from roles) = 13, 'roles seeded';
    assert (select count(*) from role_permissions rp join roles r on r.id=rp.role_id where r.name='SuperAdmin') = (select count(*) from permissions), 'SuperAdmin has all permissions';
    assert not exists (select 1 from role_permissions rp join roles r on r.id=rp.role_id join permissions p on p.id=rp.permission_id
                       where r.name='Advertiser' and p.code in ('farmer.profile.read_all','calendar.activity.read_all')), 'advertiser cannot read farmer data';
    assert not exists (select 1 from role_permissions rp join roles r on r.id=rp.role_id join permissions p on p.id=rp.permission_id
                       where r.name='Farmer' and p.code = 'calendar.activity.read_assigned'), 'farmer scoped';

    insert into users (mobile_number, preferred_language, status) values ('+919876543210','mr','active') returning id into u;
    insert into users (email, preferred_language, status) values ('consultant@example.com','en','active') returning id into cu;
    -- identifier rule
    begin insert into users (preferred_language) values ('en'); raise exception 'should fail'; exception when check_violation then null; end;
    -- unique mobile
    begin insert into users (mobile_number) values ('+919876543210'); raise exception 'should fail'; exception when unique_violation then null; end;

    insert into farmer_profiles (user_id, farmer_code, full_name) values (u,'F-TEST-1','Test Farmer') returning id into fp;
    insert into consultant_profiles (user_id, display_name) values (cu,'Dr Test') returning id into cp;
    insert into farms (farmer_profile_id, name) values (fp,'My Grapes Farm') returning id into f;
    insert into lands (farm_id, farmer_profile_id, name, area_value, area_unit, location)
      values (f, fp, 'Plot 1', 2, 'acre', st_setsrid(st_makepoint(73.9,20.0),4326)::geography) returning id into l;
    assert (select area_sq_meters from lands where id=l) = 8093.71, 'area conversion';

    insert into land_boundaries (land_id, boundary) values (l, st_geogfromtext('SRID=4326;POLYGON((73.9 20.0,73.901 20.0,73.901 20.001,73.9 20.001,73.9 20.0))'));
    begin -- self-intersecting polygon
      insert into land_boundaries (land_id, boundary, version, is_current) values (l, st_geogfromtext('SRID=4326;POLYGON((0 0,1 1,1 0,0 1,0 0))'), 2, false);
      raise exception 'should fail';
    exception when check_violation then null; end;
    begin insert into land_boundaries (land_id, boundary, version) values (l, st_geogfromtext('SRID=4326;POLYGON((73.9 20.0,73.902 20.0,73.902 20.002,73.9 20.002,73.9 20.0))'), 3);
          raise exception 'two current boundaries'; exception when unique_violation then null; end;

    insert into crop_cycles (land_id, farmer_profile_id, crop_id, planting_date) values (l, fp, crop, '2026-06-01') returning id into cy;

    -- consultant activity needs consultant + assigner
    begin insert into crop_calendar_activities (farmer_profile_id, land_id, origin, activity_type, title, start_date)
          values (fp, l, 'consultant','irrigation','x','2026-10-15'); raise exception 'should fail';
    exception when check_violation then null; end;
    insert into crop_calendar_activities (farmer_profile_id, land_id, crop_cycle_id, consultant_id, assigned_by_user_id, origin, activity_type, title, start_date, start_time)
      values (fp, l, cy, cp, cu, 'consultant','irrigation','Irrigation','2026-10-15','07:00') returning id into act;
    assert (select is_locked_by_consultant from crop_calendar_activities where id=act), 'consultant lock flag';
    insert into crop_activity_occurrences (activity_id, seq_no, scheduled_start_at) values (act,0,'2026-10-15 01:30+00') returning id into occ;
    begin insert into crop_activity_occurrences (activity_id, seq_no, scheduled_start_at) values (act,0,'2026-10-15 01:30+00'); raise exception 'dup'; exception when unique_violation then null; end;

    insert into crop_activity_history (activity_id, occurrence_id, change_type, actor_user_id, actor_role) values (act, occ, 'created', cu, 'Consultant');
    begin update crop_activity_history set reason='x'; raise exception 'should fail'; exception when restrict_violation then null; end;
    insert into audit_logs (user_id, action, entity_type) values (u,'test','x');
    begin delete from audit_logs; raise exception 'should fail'; exception when restrict_violation then null; end;

    -- updated_at trigger
    update farms set created_at = now() - interval '1 day' where id=f;
    update farms set name='My Grapes Farm 2' where id=f;
    assert (select updated_at > created_at from farms where id=f), 'updated_at trigger';

    -- address hierarchy derives parents from village
    declare a uuid; st text; begin
      insert into addresses (country_id, village_id) select s.country_id, v.id from villages v join talukas t on t.id=v.taluka_id join districts d on d.id=t.district_id join states s on s.id=d.state_id where v.code='OZAR' returning id into a;
      select s.code into st from addresses ad join states s on s.id=ad.state_id where ad.id=a;
      assert st='MH', 'address hierarchy filled';
    end;

    -- prescription immutability
    insert into prescriptions (prescription_number, consultant_id, farmer_profile_id, diagnosis, status, issued_at)
      values ('RX-1', cp, fp, 'Downy mildew','issued', now()) returning id into rx;
    begin update prescriptions set diagnosis='changed' where id=rx; raise exception 'should fail'; exception when restrict_violation then null; end;
    begin insert into prescription_items (prescription_id, line_no, treatment) values (rx,1,'x'); raise exception 'should fail'; exception when restrict_violation then null; end;

    -- payments + refund trigger
    insert into orders (order_number, buyer_user_id, seller_type, nursery_id, subtotal) 
      select 'ORD-1', u, 'shop', null, 100 where false;
    insert into shop_profiles (user_id, shop_name, owner_name) values (cu,'Shop','Owner');
    insert into orders (order_number, buyer_user_id, seller_type, shop_id, subtotal, tax_amount)
      select 'ORD-1', u, 'shop', id, 1000, 50 from shop_profiles limit 1 returning id into ord;
    assert (select total_amount from orders where id=ord) = 1050, 'order total generated';
    insert into payments (payment_number, payer_user_id, purpose, order_id, amount, gateway, status, verified_at)
      values ('PAY-1', u, 'order', ord, 1050, 'razorpay','SUCCESS', now()) returning id into pay;
    insert into refunds (payment_id, amount, reason, status, approved_by, processed_at) values (pay, 50, 'partial','processed', cu, now());
    assert (select status from payments where id=pay) = 'PARTIALLY_REFUNDED', 'partial refund status';
    begin insert into refunds (payment_id, amount, reason, status, approved_by, processed_at) values (pay, 2000, 'too much','processed', cu, now());
          raise exception 'should fail'; exception when check_violation then null; end;
    insert into refunds (payment_id, amount, reason, status, approved_by, processed_at) values (pay, 1000, 'rest','processed', cu, now());
    assert (select status from payments where id=pay) = 'REFUNDED', 'full refund status';
    begin insert into payments (payment_number, payer_user_id, purpose, order_id, consultation_id, amount, gateway) values ('PAY-2', u,'order', ord, null, 10,'x');
          insert into payments (payment_number, payer_user_id, purpose, amount, gateway) values ('PAY-3', u,'order', 10,'x'); raise exception 'should fail';
    exception when check_violation then null; end;

    -- ad creative must carry a sponsored label
    declare adv uuid; camp uuid; begin
      insert into advertiser_profiles (user_id, company_name) values (cu,'Brand Co');
      insert into advertisers (advertiser_profile_id, display_name) select id,'Brand Co' from advertiser_profiles limit 1 returning id into adv;
      insert into ad_campaigns (advertiser_id, name, pricing_model, bid_amount, total_budget, start_at, end_at) values (adv,'c','cpc',2,1000,now(),now()+interval '10 days') returning id into camp;
      begin insert into ad_creatives (campaign_id, ad_type, title, sponsored_label) values (camp,'banner','t','  '); raise exception 'should fail'; exception when check_violation then null; end;
      begin update ad_campaigns set spent_amount = 5000 where id=camp; raise exception 'should fail'; exception when check_violation then null; end;
    end;

    -- partitions exist and route rows
    assert (select count(*) from pg_inherits where inhparent='audit_logs'::regclass) >= 12, 'audit partitions';
    -- AI: one production version per model/env
    declare m uuid; v1 uuid; begin
      insert into ai_models (code,name,task,crop_id) values ('grape_disease','Grape Disease','disease_detection',crop) returning id into m;
      insert into ai_model_versions (model_id, version, status, approved_at) values (m,'1.0.0','production', now());
      begin insert into ai_model_versions (model_id, version, status, approved_at) values (m,'1.1.0','production', now()); raise exception 'should fail'; exception when unique_violation then null; end;
    end;
    -- vector column accepts embeddings
    raise notice 'all database smoke tests passed';
end $$;
rollback;
