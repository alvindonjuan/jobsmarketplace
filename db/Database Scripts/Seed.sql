INSERT INTO public.customers
(id, first_name, last_name, created_at, updated_at)
select 
gen_random_uuid()
, upper(substr(md5(random()::text), 1, 25))
, upper(substr(md5(random()::text), 1, 25))
, now() + (floor(random() * 100) + 1 || ' days')::interval
, now() + (floor(random() * 100) + 2 || ' days')::interval
from generate_series(1,1e7) n;


INSERT INTO public.contractors
(id, "name", rating, created_at, updated_at)
select 
gen_random_uuid()
, upper(substr(md5(random()::text), 1, 25))
, floor(random() * 5) + 1
, now() + (floor(random() * 100) + 1 || ' days')::interval
, now() + (floor(random() * 100) + 2 || ' days')::interval
from generate_series(1,1e5) n;


INSERT INTO public.jobs
(id, customer_id, title, description, budget, status, created_at, updated_at, start_date, due_date)
SELECT
gen_random_uuid()
, c.id
,  case abs(hashtext(c.id::text)) % 3
	when 0 then 'Frontend'
	when 1 then 'Backend'
	else 'DevOps'
   end
,  case abs(hashtext(c.id::text)) % 3
	when 0 then 'Frontend Engineer'
	when 1 then 'Backend Engineer'
	else 'DevOps Engineer'
   end
, (floor(random() * 1000) + 1) * 100
, abs(hashtext(c.id::text)) % 4
, now() + (floor(random() * 100) + 1 || ' days')::interval
, now() + (floor(random() * 100) + 2 || ' days')::interval
, case abs(hashtext(c.id::text)) % 4
	when 0 then null
	when 1 then c.created_at  + (2 || ' days')::interval
	when 2 then c.created_at  + (2 || ' days')::interval
	else null
   end
, case abs(hashtext(c.id::text)) % 4
	when 0 then null
	when 1 then null
	when 2 then c.updated_at  + (4 || ' days')::interval
	else null
   end
from public.customers c