-- V011: real-time chat and notifications
create table chat_rooms (
    id uuid primary key default gen_random_uuid(),
    room_type text not null check (room_type in ('consultation','direct','order','lab_booking','support')),
    consultation_id uuid unique references consultations(id),
    order_id uuid references orders(id),
    lab_booking_id uuid references lab_bookings(id),
    ticket_id uuid,
    title text,
    is_archived boolean not null default false,
    last_message_at timestamptz,
    like tpl.audit including defaults,
    check ((consultation_id is not null)::int + (order_id is not null)::int + (lab_booking_id is not null)::int + (ticket_id is not null)::int <= 1)
);
create index ix_chat_rooms_order on chat_rooms (order_id);
create index ix_chat_rooms_lab_booking on chat_rooms (lab_booking_id);
create index ix_chat_rooms_ticket on chat_rooms (ticket_id);
create index ix_chat_rooms_last on chat_rooms (last_message_at desc);
alter table consultations add constraint fk_consultations_chat_room foreign key (chat_room_id) references chat_rooms(id);
create index ix_consultations_chat_room on consultations (chat_room_id);

create table chat_participants (
    room_id uuid not null references chat_rooms(id) on delete cascade,
    user_id uuid not null references users(id),
    participant_role text not null default 'member' check (participant_role in ('owner','member','agent','observer')),
    joined_at timestamptz not null default now(),
    left_at timestamptz,
    is_muted boolean not null default false,
    last_read_at timestamptz,
    primary key (room_id, user_id)
);
create index ix_chat_participants_user on chat_participants (user_id) where left_at is null;

create table messages (
    id uuid primary key default gen_random_uuid(),
    room_id uuid not null references chat_rooms(id) on delete cascade,
    sender_user_id uuid not null references users(id),
    message_type text not null default 'text' check (message_type in ('text','image','video','pdf','document','audio','voice','system')),
    body text,
    reply_to_id uuid references messages(id),
    client_message_id uuid,
    edited_at timestamptz,
    search_vector tsvector generated always as (to_tsvector('simple', coalesce(body,''))) stored,
    like tpl.audit_sd including defaults,
    check (body is not null or message_type not in ('text','system')),
    unique (sender_user_id, client_message_id)
);
create index ix_messages_room_time on messages (room_id, created_at desc) where not is_deleted;
create index ix_messages_sender on messages (sender_user_id);
create index ix_messages_reply on messages (reply_to_id);
create index ix_messages_search on messages using gin (search_vector);

create table message_attachments (
    id uuid primary key default gen_random_uuid(),
    message_id uuid not null references messages(id) on delete cascade,
    file_object_id uuid not null references file_objects(id),
    attachment_kind text not null check (attachment_kind in ('image','video','pdf','document','audio','voice')),
    thumbnail_file_id uuid references file_objects(id),
    duration_seconds numeric(10,2) check (duration_seconds is null or duration_seconds >= 0),
    waveform jsonb,
    like tpl.audit including defaults
);
create index ix_message_attachments_message on message_attachments (message_id);
create index ix_message_attachments_file on message_attachments (file_object_id);

create table message_read_receipts (
    message_id uuid not null references messages(id) on delete cascade,
    user_id uuid not null references users(id),
    delivered_at timestamptz,
    read_at timestamptz,
    primary key (message_id, user_id),
    check (delivered_at is not null or read_at is not null)
);
create index ix_receipts_user on message_read_receipts (user_id, read_at);

-- Notifications
create table notification_templates (
    id uuid primary key default gen_random_uuid(),
    code text not null,
    channel text not null check (channel in ('in_app','push','email','sms','whatsapp')),
    language_code text not null references languages(code),
    subject text,
    body text not null,
    provider_template_id text,
    is_active boolean not null default true,
    like tpl.audit including defaults,
    unique (code, channel, language_code)
);

create table notifications (
    id uuid primary key default gen_random_uuid(),
    user_id uuid not null references users(id) on delete cascade,
    event_type text not null check (event_type in ('login','booking','payment','order','lab_report','prescription','consultant_message','crop_activity','activity_reminder','missed_activity','ai_alert','weather','advertisement','farm_reminder','schedule_change','care_plan','system')),
    title text not null,
    body text not null,
    language_code text references languages(code),
    template_code text,
    data jsonb not null default '{}',
    related_entity_type text,
    related_entity_id uuid,
    priority text not null default 'normal' check (priority in ('low','normal','high')),
    read_at timestamptz,
    expires_at timestamptz,
    dedupe_key text,
    like tpl.audit including defaults
);
create index ix_notifications_user_unread on notifications (user_id, created_at desc) where read_at is null;
create index ix_notifications_user_time on notifications (user_id, created_at desc);
create index ix_notifications_entity on notifications (related_entity_type, related_entity_id);
create unique index ux_notifications_dedupe on notifications (user_id, dedupe_key) where dedupe_key is not null;

create table notification_preferences (
    id uuid primary key default gen_random_uuid(),
    user_id uuid not null references users(id) on delete cascade,
    event_type text not null,
    channel text not null check (channel in ('in_app','push','email','sms','whatsapp')),
    is_enabled boolean not null default true,
    quiet_hours_start time,
    quiet_hours_end time,
    like tpl.audit including defaults,
    unique (user_id, event_type, channel)
);

create table notification_delivery_logs (
    id uuid primary key default gen_random_uuid(),
    notification_id uuid not null references notifications(id) on delete cascade,
    channel text not null check (channel in ('in_app','push','email','sms','whatsapp')),
    status text not null check (status in ('queued','sent','delivered','failed','skipped')),
    provider text,
    provider_message_id text,
    attempt int not null default 1 check (attempt > 0),
    error_code text,
    error_message text,
    sent_at timestamptz,
    created_at timestamptz not null default now()
);
create index ix_delivery_logs_notification on notification_delivery_logs (notification_id);
create index ix_delivery_logs_failed on notification_delivery_logs (created_at) where status = 'failed';
