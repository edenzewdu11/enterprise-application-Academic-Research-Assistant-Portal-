-- Initialize ARAP Database
-- This script creates additional databases for Keycloak

-- Create Keycloak database
CREATE DATABASE keycloak_db;
GRANT ALL PRIVILEGES ON DATABASE keycloak_db TO postgres;


\c arap_db

-- Create schemas for each Bounded Context
CREATE SCHEMA IF NOT EXISTS research_proposal;
CREATE SCHEMA IF NOT EXISTS document_review;
CREATE SCHEMA IF NOT EXISTS progress_tracking;
CREATE SCHEMA IF NOT EXISTS academic_integrity;
CREATE SCHEMA IF NOT EXISTS notifications;
CREATE SCHEMA IF NOT EXISTS outbox;

-- Grant permissions
GRANT ALL PRIVILEGES ON SCHEMA research_proposal TO postgres;
GRANT ALL PRIVILEGES ON SCHEMA document_review TO postgres;
GRANT ALL PRIVILEGES ON SCHEMA progress_tracking TO postgres;
GRANT ALL PRIVILEGES ON SCHEMA academic_integrity TO postgres;
GRANT ALL PRIVILEGES ON SCHEMA notifications TO postgres;
GRANT ALL PRIVILEGES ON SCHEMA outbox TO postgres;

-- Create extensions
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- Success message
SELECT 'ARAP Database initialized successfully!' AS message;
