CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE SCHEMA IF NOT EXISTS identity;
CREATE SCHEMA IF NOT EXISTS users;
CREATE SCHEMA IF NOT EXISTS notification;

CREATE TABLE IF NOT EXISTS identity.app_users
(
    id UUID PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    email VARCHAR(250) NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,
    role_name VARCHAR(100) NOT NULL,
    created_utc TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS users.user_profiles
(
    id UUID PRIMARY KEY,
    identity_user_id UUID NULL,
    name VARCHAR(200) NOT NULL,
    role VARCHAR(100) NOT NULL,
    email VARCHAR(250) NOT NULL,
    status VARCHAR(50) NOT NULL,
    team VARCHAR(100) NOT NULL,
    score INT NOT NULL,
    created_utc TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS notification.notifications
(
    id UUID PRIMARY KEY,
    message TEXT NOT NULL,
    is_read BOOLEAN NOT NULL DEFAULT FALSE,
    created_utc TIMESTAMP NOT NULL DEFAULT NOW()
);

INSERT INTO identity.app_users(id, name, email, password_hash, role_name)
VALUES
(gen_random_uuid(), 'Prashanth Reddy', 'prashanth@enterprise.com', 'Password@123', 'Admin')
ON CONFLICT(email) DO NOTHING;

INSERT INTO users.user_profiles(id, name, role, email, status, team, score)
VALUES
(gen_random_uuid(), 'Prashanth Reddy', 'Lead Engineer', 'prashanth@enterprise.com', 'Active', 'Commerce', 92),
(gen_random_uuid(), 'Ananya Rao', 'Frontend Engineer', 'ananya@enterprise.com', 'Active', 'Commerce', 86),
(gen_random_uuid(), 'Vikram Sen', 'Backend Engineer', 'vikram@enterprise.com', 'Inactive', 'Platform', 74),
(gen_random_uuid(), 'Meera Iyer', 'QA Engineer', 'meera@enterprise.com', 'Active', 'QA', 81),
(gen_random_uuid(), 'Kiran Das', 'DevOps Engineer', 'kiran@enterprise.com', 'Active', 'Cloud', 88)
ON CONFLICT DO NOTHING;

INSERT INTO notification.notifications(id, message)
VALUES
(gen_random_uuid(), 'Sprint dashboard refreshed'),
(gen_random_uuid(), 'New user onboarding pending')
ON CONFLICT DO NOTHING;
