call psql_database
psql -h localhost -d postgres -U postgres -p 5432 -a -f unit_test.sql