-- D001: DEV ONLY sample geography (India / Maharashtra subset). No real personal data.
insert into countries (iso2, iso3, name, phone_code) values ('IN','IND','India','+91') on conflict (iso2) do nothing;
insert into states (country_id, code, name, name_local)
select c.id, s.code, s.name, s.local::jsonb from countries c, (values
 ('MH','Maharashtra','{"mr":"महाराष्ट्र","hi":"महाराष्ट्र"}'),('GJ','Gujarat','{"hi":"गुजरात"}'),
 ('KA','Karnataka','{"hi":"कर्नाटक"}'),('MP','Madhya Pradesh','{"hi":"मध्य प्रदेश"}'),('PB','Punjab','{"hi":"पंजाब"}')) as s(code,name,local)
where c.iso2='IN' on conflict (country_id, code) do nothing;
insert into districts (state_id, code, name, name_local, centroid)
select s.id, d.code, d.name, d.local::jsonb, st_setsrid(st_makepoint(d.lon, d.lat),4326)::geography
from states s join (values
 ('PUNE','Pune','{"mr":"पुणे","hi":"पुणे"}',73.8567,18.5204),('NASHIK','Nashik','{"mr":"नाशिक","hi":"नाशिक"}',73.7898,19.9975),
 ('SANGLI','Sangli','{"mr":"सांगली","hi":"सांगली"}',74.5815,16.8524),('SOLAPUR','Solapur','{"mr":"सोलापूर","hi":"सोलापुर"}',75.9064,17.6599)
) as d(code,name,local,lon,lat) on true where s.code='MH' on conflict (state_id, code) do nothing;
insert into talukas (district_id, code, name, name_local)
select d.id, t.code, t.name, t.local::jsonb from districts d join (values
 ('PUNE','HAVELI','Haveli','{"mr":"हवेली"}'),('PUNE','BARAMATI','Baramati','{"mr":"बारामती"}'),
 ('NASHIK','NIPHAD','Niphad','{"mr":"निफाड"}'),('NASHIK','DINDORI','Dindori','{"mr":"दिंडोरी"}'),
 ('SANGLI','TASGAON','Tasgaon','{"mr":"तासगाव"}')) as t(dist,code,name,local) on t.dist=d.code
on conflict (district_id, code) do nothing;
insert into villages (taluka_id, code, name, name_local, pincode)
select t.id, v.code, v.name, v.local::jsonb, v.pin from talukas t join (values
 ('NIPHAD','PIMPALGAON','Pimpalgaon Baswant','{"mr":"पिंपळगाव बसवंत"}','422209'),
 ('NIPHAD','OZAR','Ozar','{"mr":"ओझर"}','422206'),
 ('BARAMATI','MALEGAON','Malegaon Bk','{"mr":"माळेगाव बुद्रुक"}','413115'),
 ('TASGAON','MANERAJURI','Manerajuri','{"mr":"मणेराजुरी"}','416311')) as v(taluka,code,name,local,pin) on v.taluka=t.code
on conflict (taluka_id, code) do nothing;
