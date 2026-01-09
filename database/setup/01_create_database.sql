-- ARAP Database Setup Script

SELECT 'CREATE DATABASE arap_db'
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'arap_db')\gexec


\c arap_db

-- Create schemas for each bounded context
CREATE SCHEMA IF NOT EXISTS research_proposal;
CREATE SCHEMA IF NOT EXISTS document_review;
CREATE SCHEMA IF NOT EXISTS progress_tracking;
CREATE SCHEMA IF NOT EXISTS academic_integrity;
CREATE SCHEMA IF NOT EXISTS notifications;

-- Grant all privileges to postgres user (already owner, but being explicit)
GRANT ALL PRIVILEGES ON SCHEMA research_proposal TO postgres;
GRANT ALL PRIVILEGES ON SCHEMA document_review TO postgres;
GRANT ALL PRIVILEGES ON SCHEMA progress_tracking TO postgres;
GRANT ALL PRIVILEGES ON SCHEMA academic_integrity TO postgres;
GRANT ALL PRIVILEGES ON SCHEMA notifications TO postgres;

-- Set default privileges for future tables
ALTER DEFAULT PRIVILEGES IN SCHEMA research_proposal GRANT ALL ON TABLES TO postgres;
ALTER DEFAULT PRIVILEGES IN SCHEMA document_review GRANT ALL ON TABLES TO postgres;
ALTER DEFAULT PRIVILEGES IN SCHEMA progress_tracking GRANT ALL ON TABLES TO postgres;
ALTER DEFAULT PRIVILEGES IN SCHEMA academic_integrity GRANT ALL ON TABLES TO postgres;
ALTER DEFAULT PRIVILEGES IN SCHEMA notifications GRANT ALL ON TABLES TO postgres;

-- Set default privileges for future sequences
ALTER DEFAULT PRIVILEGES IN SCHEMA research_proposal GRANT ALL ON SEQUENCES TO postgres;
ALTER DEFAULT PRIVILEGES IN SCHEMA document_review GRANT ALL ON SEQUENCES TO postgres;
ALTER DEFAULT PRIVILEGES IN SCHEMA progress_tracking GRANT ALL ON SEQUENCES TO postgres;
ALTER DEFAULT PRIVILEGES IN SCHEMA academic_integrity GRANT ALL ON SEQUENCES TO postgres;
ALTER DEFAULT PRIVILEGES IN SCHEMA notifications GRANT ALL ON SEQUENCES TO postgres;

-- Verify schemas were created
SELECT schema_name 
FROM information_schema.schemata 
WHERE schema_name IN ('research_proposal', 'document_review', 'progress_tracking', 'academic_integrity', 'notifications')
ORDER BY schema_name;

-- Success message
SELECT 'Database and schemas created successfully!' AS status;
