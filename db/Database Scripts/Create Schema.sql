CREATE EXTENSION IF NOT EXISTS pgcrypto;
CREATE EXTENSION IF NOT EXISTS citext;
CREATE EXTENSION IF NOT EXISTS pg_trgm;

CREATE TABLE customers (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    first_name CITEXT NOT NULL,
    last_name CITEXT NOT NULL,
    full_name TEXT GENERATED ALWAYS AS (
        first_name || ' ' || last_name
    ) STORED,
    created_at TIMESTAMPTZ  DEFAULT NOW(),
	updated_at TIMESTAMPTZ  DEFAULT NOW()
);

CREATE TABLE contractors (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name CITEXT NOT NULL,
    rating DECIMAL(3,2) NOT NULL CHECK (rating BETWEEN 0 AND 5),
    created_at TIMESTAMPTZ  DEFAULT NOW(),
	updated_at TIMESTAMPTZ  DEFAULT NOW()
	
);

CREATE TABLE jobs 
(
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    customer_id UUID NOT NULL REFERENCES customers(id),
    title CITEXT NOT NULL,
    description TEXT NOT NULL,
	budget DECIMAL(12,2) NOT NULL CHECK (budget > 0),
    status SMALLINT NOT NULL DEFAULT 0,
		-- 0 = Open 
		-- 1 = InProgress
		-- 2 = Completed
		-- 3 = Cancelled
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
	start_date TIMESTAMPTZ  NULL,
	end_date  TIMESTAMPTZ  NULL,
    due_date TIMESTAMPTZ  NULL,
	search_vector tsvector
        GENERATED ALWAYS AS (
            to_tsvector('english',
                coalesce(title, '') || ' ' ||
                coalesce(description, '')
            )
        ) STORED,
    CONSTRAINT chk_budget_valid CHECK (budget > 0)
);


CREATE TABLE job_offers (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    job_id UUID NOT NULL REFERENCES jobs(id),
    contractor_id UUID NOT NULL REFERENCES contractors(id),
    offered_price DECIMAL(12,2) NOT NULL,
	is_accepted BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT NOW(),
	updated_at TIMESTAMP NOT NULL DEFAULT NOW(),

    CONSTRAINT chk_offered_price_valid CHECK (offered_price > 0)
);

CREATE INDEX idx_customers_full_name_trgm ON customers USING gin (full_name gin_trgm_ops);
CREATE INDEX idx_contractors_name_trgm ON contractors USING gin (name gin_trgm_ops);


--Full Text Search 	
CREATE INDEX idx_jobs_searchvector_statusopen ON jobs USING GIN (search_vector) WHERE status = 0;
	
--Browse Search / Pagination
CREATE INDEX idx_jobs_createdat_statusopen ON jobs (created_at DESC) WHERE status = 0;

-- Foreign keys
CREATE INDEX idx_jobs_customerid ON jobs(customer_id);
CREATE INDEX idx_joboffers_jobid ON job_offers(job_id);
CREATE INDEX idx_joboffers_contractorid ON job_offers(contractor_id);
	


