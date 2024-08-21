
DO
$$
BEGIN
    IF NOT EXISTS (
        SELECT FROM pg_database
        WHERE datname = 'message_app'
    ) THEN
        EXECUTE 'CREATE DATABASE message_app';
    END IF;
END
$$;

\c message_app;

CREATE TABLE IF NOT EXISTS messages (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    order_number BIGINT NOT NULL,
    text VARCHAR(128) NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_messages_created_at ON messages(created_at);

INSERT INTO messages (order_number, text, created_at)
VALUES (1, 'Hello World!', '1969-10-29 10:00:00+00');
