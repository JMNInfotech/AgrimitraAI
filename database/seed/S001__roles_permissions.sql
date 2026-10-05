-- S001: roles, permissions, role-permission grants (idempotent)
insert into roles (name, description, is_system, requires_mfa) values
 ('Farmer','Farmer using the platform',true,false),
 ('Consultant','Agriculture consultant',true,false),
 ('Nursery','Nursery owner/operator',true,false),
 ('Laboratory','Laboratory operator',true,false),
 ('PesticideShop','Pesticide / agriculture product shop',true,false),
 ('Advertiser','Advertiser',true,false),
 ('Brand','Agriculture brand',true,false),
 ('Admin','Platform administrator',true,true),
 ('SuperAdmin','Unrestricted administrator',true,true),
 ('SupportAgent','Customer support agent',true,false),
 ('AIDataAdmin','AI / dataset administrator',true,true),
 ('ContentManager','Content and knowledge manager',true,false),
 ('FinanceAdmin','Finance administrator',true,true)
on conflict (name) do nothing;

create temp table _perm (code text primary key, description text) on commit drop;
insert into _perm values
 ('auth.session.manage','Manage own sessions and devices'),
 ('users.profile.read','Read own user profile'),('users.profile.write','Update own user profile'),
 ('users.user.read','Read any user'),('users.user.write','Update any user'),('users.user.suspend','Suspend or unlock users'),
 ('users.role.read','Read roles'),('users.role.write','Create/edit roles and grants'),
 ('users.permission.read','Read permissions'),('users.permission.write','Edit role permissions'),
 ('farmer.profile.read','Read own farmer profile'),('farmer.profile.write','Update own farmer profile'),
 ('farmer.profile.read_all','Read any farmer profile'),('farmer.govid.reveal','Reveal government ID reference (audited)'),
 ('farm.land.read','Read own farms and lands'),('farm.land.write','Create/update own farms and lands'),
 ('farm.land.delete','Delete own lands'),('farm.land.read_all','Read any land'),
 ('farm.document.read','Read own land documents'),('farm.document.write','Upload land documents'),
 ('crop.master.read','Read crop master data'),('crop.master.write','Manage crop master data'),
 ('crop.cycle.read','Read own crop cycles'),('crop.cycle.write','Manage own crop cycles'),('crop.cycle.read_all','Read any crop cycle'),
 ('calendar.activity.read','Read own calendar activities'),('calendar.activity.read_assigned','Read activities of linked farmers'),
 ('calendar.activity.read_all','Read any activity'),
 ('calendar.activity.create','Create activities'),('calendar.activity.edit','Edit activities'),
 ('calendar.activity.reschedule','Reschedule activities'),('calendar.activity.cancel','Cancel activities'),
 ('calendar.activity.complete','Complete activities'),('calendar.activity.skip','Skip activities'),
 ('calendar.plan.read','Read crop calendar plans'),('calendar.proposal.read','Read schedule change proposals'),
 ('calendar.proposal.decide','Approve/reject schedule change proposals'),('calendar.reminder.manage','Manage reminder settings'),
 ('careplan.read','Read own crop care plans'),('careplan.read_assigned','Read care plans of linked farmers'),('careplan.read_all','Read any care plan'),
 ('careplan.create','Create crop care plans'),('careplan.edit','Edit crop care plans'),('careplan.publish','Publish crop care plans'),
 ('prescription.read','Read own prescriptions'),('prescription.read_assigned','Read prescriptions written by self'),('prescription.read_all','Read any prescription'),
 ('prescription.create','Create prescriptions'),('prescription.edit','Edit draft prescriptions'),('prescription.issue','Issue prescriptions'),
 ('consultant.profile.read','Browse consultant profiles'),('consultant.profile.write','Manage own consultant profile'),
 ('consultant.verify','Verify consultants'),('consultant.availability.manage','Manage availability'),
 ('consultation.request.create','Request a consultation'),('consultation.request.decide','Accept/reject consultation requests'),
 ('consultation.read','Read own consultations'),('consultation.read_all','Read any consultation'),('consultation.manage','Run consultations'),
 ('diary.read','Read own diary'),('diary.write','Write diary entries'),
 ('expense.read','Read own expenses'),('expense.write','Manage own expenses'),
 ('income.read','Read own income'),('income.write','Manage own income'),
 ('production.read','Read own production'),('production.write','Manage own production'),
 ('machinery.read','Read own machinery'),('machinery.write','Manage own machinery'),
 ('ai.copilot.use','Use the AI copilot'),('ai.scan.create','Submit crop scans'),('ai.scan.read','Read own scans'),
 ('ai.recommendation.read','Read AI recommendations'),('ai.feedback.create','Rate AI results'),('ai.feedback.confirm','Confirm or correct AI diagnoses'),
 ('ai.model.read','Read AI model registry'),('ai.model.manage','Manage AI models and versions'),
 ('ai.dataset.read','Read datasets'),('ai.dataset.manage','Manage datasets and labels'),('ai.dataset.approve','Approve dataset versions'),
 ('ai.candidate.approve','Approve training candidates'),
 ('ai.knowledge.read','Read knowledge base'),('ai.knowledge.manage','Manage knowledge documents'),('ai.knowledge.approve','Approve knowledge documents'),
 ('weather.read','Read weather'),('weather.alert.manage','Manage weather alerts'),
 ('soil.report.read','Read own soil reports'),('soil.report.write','Manage soil reports'),
 ('nursery.profile.manage','Manage own nursery profile'),('nursery.product.manage','Manage nursery products'),
 ('nursery.batch.manage','Manage nursery batches'),('nursery.order.manage','Manage nursery orders'),
 ('lab.profile.manage','Manage own laboratory profile'),('lab.service.manage','Manage laboratory services'),
 ('lab.booking.create','Book lab tests'),('lab.booking.read','Read own lab bookings'),('lab.booking.manage','Manage lab bookings'),
 ('lab.sample.manage','Manage lab samples'),('lab.result.enter','Enter lab results'),('lab.result.approve','Approve lab results'),('lab.report.issue','Issue lab reports'),
 ('shop.profile.manage','Manage own shop profile'),('shop.product.manage','Manage shop products'),
 ('shop.inventory.manage','Manage shop inventory'),('shop.order.manage','Manage shop orders'),
 ('marketplace.product.read','Browse marketplace'),('marketplace.product.moderate','Moderate marketplace products'),
 ('cart.manage','Manage cart'),('order.create','Place orders'),('order.read','Read own orders'),('order.read_all','Read any order'),('order.manage','Manage orders'),
 ('payment.create','Initiate payments'),('payment.read','Read own payments'),('payment.read_all','Read any payment'),
 ('payment.refund.request','Request a refund'),('payment.refund.approve','Approve refunds'),('payment.reconcile','Reconcile payments'),
 ('invoice.read','Read own invoices'),('invoice.read_all','Read any invoice'),
 ('chat.room.read','Read own chat rooms'),('chat.message.send','Send chat messages'),('chat.moderate','Moderate chat'),
 ('notification.read','Read notifications'),('notification.preferences.manage','Manage notification preferences'),
 ('notification.template.manage','Manage notification templates'),('notification.broadcast','Send broadcast notifications'),
 ('ad.campaign.read','Read own campaigns'),('ad.campaign.manage','Manage own campaigns'),('ad.campaign.approve','Approve campaigns'),
 ('ad.campaign.read_all','Read any campaign'),('ad.creative.manage','Manage creatives'),('ad.analytics.read','Read aggregated ad analytics'),
 ('ad.billing.read','Read ad billing'),('ad.billing.manage','Manage ad billing'),('ad.placement.manage','Manage ad placements'),
 ('support.ticket.create','Create tickets'),('support.ticket.read','Read own tickets'),('support.ticket.manage','Work tickets'),
 ('support.ticket.read_all','Read any ticket'),('support.complaint.create','File complaints'),('support.complaint.manage','Manage complaints'),
 ('review.create','Create reviews'),('review.moderate','Moderate reviews'),('review.respond','Respond to reviews'),
 ('report.generate','Generate own reports'),('report.admin','Generate platform reports'),
 ('admin.dashboard.read','Read admin dashboard'),('admin.config.manage','Manage system configuration'),
 ('admin.audit.read','Read audit logs'),('admin.content.manage','Manage content'),('admin.geography.manage','Manage geographic data'),
 ('admin.system.read','Read system health');

insert into permissions (code, module, description)
select code, split_part(code, '.', 1), description from _perm
on conflict (code) do update set description = excluded.description;

-- Role grants: regex patterns on permission codes; "except" removes matches.
create temp table _grant (role_name text, pattern text, is_except boolean default false) on commit drop;
insert into _grant (role_name, pattern, is_except) values
 -- SuperAdmin: everything
 ('SuperAdmin','.*',false),
 -- Admin: everything except identity/role administration, config, refund approval, ID reveal
 ('Admin','.*',false),
 ('Admin','^(users\.role\.write|users\.permission\.write|admin\.config\.manage|payment\.refund\.approve|farmer\.govid\.reveal)$',true),
 -- Farmer
 ('Farmer','^(auth\.session\.manage|users\.profile\.(read|write)|farmer\.profile\.(read|write)|farm\.(land\.(read|write|delete)|document\.(read|write))|crop\.master\.read|crop\.cycle\.(read|write)|calendar\.(activity\.(read|create|edit|reschedule|cancel|complete|skip)|plan\.read|proposal\.(read|decide)|reminder\.manage)|careplan\.read|prescription\.read|consultant\.profile\.read|consultation\.(request\.create|read)|diary\..*|expense\..*|income\..*|production\..*|machinery\..*|ai\.(copilot\.use|scan\..*|recommendation\.read|feedback\.create|knowledge\.read)|weather\.read|soil\.report\..*|marketplace\.product\.read|cart\.manage|order\.(create|read)|payment\.(create|read|refund\.request)|invoice\.read|lab\.booking\.(create|read)|chat\.(room\.read|message\.send)|notification\.(read|preferences\.manage)|support\.(ticket\.(create|read)|complaint\.create)|review\.create|report\.generate)$',false),
 -- Consultant
 ('Consultant','^(auth\.session\.manage|users\.profile\.(read|write)|consultant\.(profile\.(read|write)|availability\.manage)|consultation\.(request\.decide|read|manage)|calendar\.(activity\.(read_assigned|create|edit|reschedule|cancel)|plan\.read|proposal\.(read|decide))|careplan\.(read_assigned|create|edit|publish)|prescription\.(read_assigned|create|edit|issue)|crop\.master\.read|ai\.(feedback\.confirm|recommendation\.read|knowledge\.read|scan\.read)|weather\.read|soil\.report\.read|chat\.(room\.read|message\.send)|notification\.(read|preferences\.manage)|payment\.read|invoice\.read|review\.respond|support\.ticket\.(create|read)|report\.generate|ad\.analytics\.read)$',false),
 -- Nursery
 ('Nursery','^(auth\.session\.manage|users\.profile\.(read|write)|nursery\..*|crop\.master\.read|marketplace\.product\.read|order\.(read|manage)|payment\.read|invoice\.read|chat\.(room\.read|message\.send)|notification\.(read|preferences\.manage)|review\.respond|support\.ticket\.(create|read)|report\.generate)$',false),
 -- Laboratory
 ('Laboratory','^(auth\.session\.manage|users\.profile\.(read|write)|lab\.(profile\.manage|service\.manage|booking\.manage|sample\.manage|result\.enter|result\.approve|report\.issue)|crop\.master\.read|soil\.report\.write|payment\.read|invoice\.read|chat\.(room\.read|message\.send)|notification\.(read|preferences\.manage)|review\.respond|support\.ticket\.(create|read)|report\.generate)$',false),
 -- PesticideShop
 ('PesticideShop','^(auth\.session\.manage|users\.profile\.(read|write)|shop\..*|crop\.master\.read|marketplace\.product\.read|order\.(read|manage)|payment\.read|invoice\.read|chat\.(room\.read|message\.send)|notification\.(read|preferences\.manage)|review\.respond|support\.ticket\.(create|read)|report\.generate|ad\.(campaign\.(read|manage)|creative\.manage|analytics\.read))$',false),
 -- Advertiser / Brand
 ('Advertiser','^(auth\.session\.manage|users\.profile\.(read|write)|ad\.(campaign\.(read|manage)|creative\.manage|analytics\.read|billing\.read)|payment\.(create|read)|invoice\.read|notification\.(read|preferences\.manage)|support\.ticket\.(create|read)|report\.generate)$',false),
 ('Brand','^(auth\.session\.manage|users\.profile\.(read|write)|ad\.(campaign\.(read|manage)|creative\.manage|analytics\.read|billing\.read)|payment\.(create|read)|invoice\.read|notification\.(read|preferences\.manage)|support\.ticket\.(create|read)|report\.generate|marketplace\.product\.read)$',false),
 -- SupportAgent
 ('SupportAgent','^(auth\.session\.manage|users\.profile\.(read|write)|users\.user\.read|support\..*|order\.read_all|payment\.read_all|invoice\.read_all|consultation\.read_all|chat\.moderate|notification\.read|admin\.dashboard\.read|review\.moderate)$',false),
 -- AIDataAdmin
 ('AIDataAdmin','^(auth\.session\.manage|users\.profile\.(read|write)|ai\..*|crop\.master\.(read|write)|weather\.read|notification\.read|admin\.dashboard\.read|admin\.system\.read)$',false),
 -- ContentManager
 ('ContentManager','^(auth\.session\.manage|users\.profile\.(read|write)|admin\.content\.manage|admin\.geography\.manage|crop\.master\.(read|write)|ai\.knowledge\..*|notification\.(read|template\.manage|broadcast)|ad\.campaign\.(read_all|approve)|marketplace\.product\.moderate|review\.moderate|admin\.dashboard\.read)$',false),
 -- FinanceAdmin
 ('FinanceAdmin','^(auth\.session\.manage|users\.profile\.(read|write)|payment\..*|invoice\.read_all|order\.read_all|ad\.(billing\.(read|manage)|campaign\.read_all)|report\.(generate|admin)|admin\.dashboard\.read|admin\.audit\.read)$',false);

insert into role_permissions (role_id, permission_id)
select r.id, p.id
  from roles r
  join _grant g on g.role_name = r.name and not g.is_except
  join permissions p on p.code ~ g.pattern
 where not exists (select 1 from _grant x where x.role_name = r.name and x.is_except and p.code ~ x.pattern)
on conflict do nothing;
