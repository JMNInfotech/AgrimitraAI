-- V010: carts, orders, shipments, invoices, payments
create table carts (
    id uuid primary key default gen_random_uuid(),
    user_id uuid not null references users(id),
    status text not null default 'active' check (status in ('active','checked_out','abandoned')),
    like tpl.audit including defaults
);
create unique index ux_carts_user_active on carts (user_id) where status = 'active';

create table cart_items (
    id uuid primary key default gen_random_uuid(),
    cart_id uuid not null references carts(id) on delete cascade,
    product_id uuid not null references products(id),
    quantity int not null check (quantity > 0),
    unit_price_snapshot numeric(12,2) not null check (unit_price_snapshot >= 0),
    like tpl.audit including defaults,
    unique (cart_id, product_id)
);
create index ix_cart_items_product on cart_items (product_id);

create table orders (
    id uuid primary key default gen_random_uuid(),
    order_number text not null unique,
    buyer_user_id uuid not null references users(id),
    farmer_profile_id uuid references farmer_profiles(id),
    organization_id uuid references organizations(id),
    seller_type text not null check (seller_type in ('nursery','shop')),
    nursery_id uuid references nursery_profiles(id),
    shop_id uuid references shop_profiles(id),
    status text not null default 'pending' check (status in ('pending','confirmed','processing','shipped','delivered','cancelled','refunded')),
    payment_status text not null default 'PENDING' check (payment_status in ('PENDING','PROCESSING','SUCCESS','FAILED','REFUNDED','PARTIALLY_REFUNDED')),
    fulfillment text not null default 'delivery' check (fulfillment in ('delivery','pickup')),
    delivery_address_id uuid references addresses(id),
    delivery_address_snapshot jsonb,
    subtotal numeric(14,2) not null check (subtotal >= 0),
    tax_amount numeric(14,2) not null default 0 check (tax_amount >= 0),
    shipping_amount numeric(14,2) not null default 0 check (shipping_amount >= 0),
    discount_amount numeric(14,2) not null default 0 check (discount_amount >= 0),
    total_amount numeric(14,2) generated always as (subtotal + tax_amount + shipping_amount - discount_amount) stored,
    currency char(3) not null default 'INR',
    placed_at timestamptz not null default now(),
    cancelled_at timestamptz,
    cancellation_reason text,
    notes text,
    idempotency_key text unique,
    like tpl.audit_sd including defaults,
    constraint ck_orders_seller check (
        (seller_type = 'nursery' and nursery_id is not null and shop_id is null) or
        (seller_type = 'shop' and shop_id is not null and nursery_id is null)),
    check (discount_amount <= subtotal + tax_amount + shipping_amount)
);
create index ix_orders_buyer on orders (buyer_user_id, placed_at desc) where not is_deleted;
create index ix_orders_farmer on orders (farmer_profile_id);
create index ix_orders_nursery_status on orders (nursery_id, status, placed_at desc);
create index ix_orders_shop_status on orders (shop_id, status, placed_at desc);
create index ix_orders_status on orders (status, placed_at desc);
create index ix_orders_delivery_address on orders (delivery_address_id);
alter table expenses add constraint fk_expenses_order foreign key (order_id) references orders(id);
create index ix_expenses_order on expenses (order_id);

create table order_items (
    id uuid primary key default gen_random_uuid(),
    order_id uuid not null references orders(id) on delete cascade,
    product_id uuid not null references products(id),
    nursery_batch_id uuid references nursery_batches(id),
    product_batch_id uuid references product_batches(id),
    title_snapshot text not null,
    quantity int not null check (quantity > 0),
    unit_price numeric(12,2) not null check (unit_price >= 0),
    tax_percent numeric(5,2) not null default 0 check (tax_percent between 0 and 100),
    line_total numeric(14,2) generated always as (round(quantity * unit_price * (1 + tax_percent / 100), 2)) stored,
    like tpl.audit including defaults
);
create index ix_order_items_order on order_items (order_id);
create index ix_order_items_product on order_items (product_id);
create index ix_order_items_nursery_batch on order_items (nursery_batch_id);
create index ix_order_items_product_batch on order_items (product_batch_id);
alter table reviews add constraint fk_reviews_order_item foreign key (order_item_id) references order_items(id);
create index ix_reviews_order_item on reviews (order_item_id);

create table order_status_history (
    id uuid primary key default gen_random_uuid(),
    order_id uuid not null references orders(id) on delete cascade,
    from_status text,
    to_status text not null,
    changed_by uuid references users(id),
    reason text,
    created_at timestamptz not null default now()
);
create index ix_order_status_history_order on order_status_history (order_id, created_at);

create table nursery_orders (
    id uuid primary key default gen_random_uuid(),
    order_id uuid not null unique references orders(id),
    nursery_id uuid not null references nursery_profiles(id),
    ready_on date,
    pickup_slot_start timestamptz,
    pickup_slot_end timestamptz,
    status text not null default 'received' check (status in ('received','preparing','ready','handed_over','cancelled')),
    notes text,
    like tpl.audit including defaults,
    check (pickup_slot_end is null or pickup_slot_start is null or pickup_slot_end > pickup_slot_start)
);
create index ix_nursery_orders_nursery on nursery_orders (nursery_id, status);

create table shipments (
    id uuid primary key default gen_random_uuid(),
    order_id uuid not null references orders(id),
    carrier text,
    tracking_number text,
    tracking_url text,
    status text not null default 'preparing' check (status in ('preparing','in_transit','out_for_delivery','delivered','failed','returned')),
    shipped_at timestamptz,
    estimated_delivery_on date,
    like tpl.audit including defaults
);
create index ix_shipments_order on shipments (order_id);
create index ix_shipments_tracking on shipments (tracking_number) where tracking_number is not null;

create table deliveries (
    id uuid primary key default gen_random_uuid(),
    shipment_id uuid not null unique references shipments(id),
    status text not null default 'pending' check (status in ('pending','delivered','failed','returned')),
    delivered_at timestamptz,
    received_by text,
    proof_file_id uuid references file_objects(id),
    failure_reason text,
    like tpl.audit including defaults,
    check (status <> 'delivered' or delivered_at is not null)
);

-- Payments: common service used by consultation, lab, orders and ads
create table payments (
    id uuid primary key default gen_random_uuid(),
    payment_number text not null unique,
    payer_user_id uuid not null references users(id),
    purpose text not null check (purpose in ('consultation','lab_booking','order','advertisement')),
    consultation_id uuid references consultations(id),
    lab_booking_id uuid references lab_bookings(id),
    order_id uuid references orders(id),
    ad_billing_id uuid,
    amount numeric(14,2) not null check (amount > 0),
    refunded_amount numeric(14,2) not null default 0 check (refunded_amount >= 0),
    currency char(3) not null default 'INR',
    gateway text not null,
    gateway_order_id text,
    method text,
    status text not null default 'PENDING' check (status in ('PENDING','PROCESSING','SUCCESS','FAILED','REFUNDED','PARTIALLY_REFUNDED')),
    paid_at timestamptz,
    verified_at timestamptz,
    failure_reason text,
    idempotency_key text unique,
    like tpl.audit including defaults,
    constraint ck_payments_one_subject check (
        (consultation_id is not null)::int + (lab_booking_id is not null)::int + (order_id is not null)::int + (ad_billing_id is not null)::int = 1),
    constraint ck_payments_subject_matches check (
        (purpose = 'consultation' and consultation_id is not null) or (purpose = 'lab_booking' and lab_booking_id is not null) or
        (purpose = 'order' and order_id is not null) or (purpose = 'advertisement' and ad_billing_id is not null)),
    check (refunded_amount <= amount),
    check (status not in ('SUCCESS','PARTIALLY_REFUNDED','REFUNDED') or verified_at is not null),
    unique (gateway, gateway_order_id)
);
create index ix_payments_payer on payments (payer_user_id, created_at desc);
create index ix_payments_status on payments (status, created_at) where status in ('PENDING','PROCESSING');
create index ix_payments_consultation on payments (consultation_id);
create index ix_payments_lab_booking on payments (lab_booking_id);
create index ix_payments_order on payments (order_id);
alter table consultations add constraint fk_consultations_payment foreign key (payment_id) references payments(id);
alter table lab_bookings add constraint fk_lab_bookings_payment foreign key (payment_id) references payments(id);
create index ix_consultations_payment on consultations (payment_id);
create index ix_lab_bookings_payment on lab_bookings (payment_id);

create table payment_transactions (
    id uuid primary key default gen_random_uuid(),
    payment_id uuid not null references payments(id),
    kind text not null check (kind in ('authorize','capture','charge','refund','webhook')),
    gateway_transaction_id text,
    amount numeric(14,2) not null check (amount >= 0),
    status text not null check (status in ('PENDING','PROCESSING','SUCCESS','FAILED')),
    signature_verified boolean,
    raw_response jsonb,
    created_at timestamptz not null default now()
);
create index ix_payment_transactions_payment on payment_transactions (payment_id, created_at);
create unique index ux_payment_transactions_gateway on payment_transactions (payment_id, kind, gateway_transaction_id) where gateway_transaction_id is not null;

create table refunds (
    id uuid primary key default gen_random_uuid(),
    payment_id uuid not null references payments(id),
    amount numeric(14,2) not null check (amount > 0),
    reason text not null,
    status text not null default 'requested' check (status in ('requested','approved','processing','processed','rejected','failed')),
    requested_by uuid references users(id),
    approved_by uuid references users(id),
    approved_at timestamptz,
    processed_at timestamptz,
    gateway_refund_id text,
    like tpl.audit including defaults,
    check (status not in ('approved','processing','processed') or approved_by is not null),
    check (status <> 'processed' or processed_at is not null)
);
create index ix_refunds_payment on refunds (payment_id);
create index ix_refunds_status on refunds (status, created_at) where status in ('requested','approved','processing');

-- Keeps payments.refunded_amount / status consistent with processed refunds
create or replace function sync_payment_refunds() returns trigger language plpgsql as $$
declare v_total numeric(14,2); v_amount numeric(14,2); v_pid uuid := coalesce(new.payment_id, old.payment_id);
begin
    select coalesce(sum(amount), 0) into v_total from refunds where payment_id = v_pid and status = 'processed';
    select amount into v_amount from payments where id = v_pid for update;
    if v_total > v_amount then
        raise exception 'Refunds exceed payment amount' using errcode = 'check_violation';
    end if;
    update payments set refunded_amount = v_total,
        status = case when v_total = 0 then status when v_total = v_amount then 'REFUNDED' else 'PARTIALLY_REFUNDED' end
     where id = v_pid;
    return null;
end $$;
create trigger trg_refunds_sync after insert or update of status, amount on refunds
    for each row execute function sync_payment_refunds();

create table payment_logs (
    id uuid not null default gen_random_uuid(),
    payment_id uuid references payments(id),
    event text not null,
    level text not null default 'info' check (level in ('debug','info','warning','error')),
    payload jsonb,
    correlation_id text,
    created_at timestamptz not null default now(),
    primary key (id, created_at)
) partition by range (created_at);
create table payment_logs_default partition of payment_logs default;
create index ix_payment_logs_payment on payment_logs (payment_id, created_at desc);

create table invoices (
    id uuid primary key default gen_random_uuid(),
    invoice_number text not null unique,
    invoice_type text not null check (invoice_type in ('order','lab_booking','consultation','advertisement')),
    order_id uuid references orders(id),
    lab_booking_id uuid references lab_bookings(id),
    consultation_id uuid references consultations(id),
    ad_billing_id uuid,
    payment_id uuid references payments(id),
    bill_to_user_id uuid not null references users(id),
    issuer_name text not null,
    issuer_gstin text,
    subtotal numeric(14,2) not null check (subtotal >= 0),
    tax_amount numeric(14,2) not null default 0 check (tax_amount >= 0),
    total_amount numeric(14,2) generated always as (subtotal + tax_amount) stored,
    currency char(3) not null default 'INR',
    status text not null default 'issued' check (status in ('draft','issued','paid','void')),
    issued_at timestamptz not null default now(),
    pdf_file_object_id uuid references file_objects(id),
    like tpl.audit including defaults,
    check ((order_id is not null)::int + (lab_booking_id is not null)::int + (consultation_id is not null)::int + (ad_billing_id is not null)::int = 1)
);
create index ix_invoices_user on invoices (bill_to_user_id, issued_at desc);
create index ix_invoices_order on invoices (order_id);
create index ix_invoices_lab_booking on invoices (lab_booking_id);
create index ix_invoices_consultation on invoices (consultation_id);
create index ix_invoices_payment on invoices (payment_id);

create table invoice_lines (
    id uuid primary key default gen_random_uuid(),
    invoice_id uuid not null references invoices(id) on delete cascade,
    line_no int not null check (line_no > 0),
    description text not null,
    quantity numeric(12,3) not null check (quantity > 0),
    unit_price numeric(12,2) not null check (unit_price >= 0),
    tax_percent numeric(5,2) not null default 0 check (tax_percent between 0 and 100),
    unique (invoice_id, line_no)
);
