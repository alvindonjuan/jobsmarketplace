# jobsmarketplace
Jobs Marketplace - Demo Project

This project requires PostgreSQL and Redis running locally via Docker.

**Getting Started**

1. Run PostgreSQL via Docker

docker run -d `
  --name jobsmarketplace-postgres `
  -e POSTGRES_USER=postgres_username `
  -e POSTGRES_PASSWORD=postgres_password `
  -e POSTGRES_DB=jobsmarketplace `
  -p 5432:5432 `
  -v pgdata:/var/lib/postgresql/data `
  postgres:16

2. Run Redis via Docker

docker run -d `
  --name jobsmarketplace-redis `
  -p 6379:6379 `
  redis:7

3. Setup the Database

From the Solution, navigate to: db/Database Scripts

Execute the scripts in order:
	a. Create Schema.sql
	b. Seed.sql

4. Update Connection Strings

From the Solution, open appsettings.json
	a. Edit the DefaultConnection and ReadConnection based on the credential used in running the postgres container
