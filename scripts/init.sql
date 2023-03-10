CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE "PharmacyVisited" (
	"Id" uuid DEFAULT uuid_generate_v1(),
	"PharmacyName" VARCHAR(100) NOT NULL,
	"SupportsPriorAuth" BOOLEAN NOT NULL
);

CREATE TABLE "PharmacyNearby" (
	"Id" uuid DEFAULT uuid_generate_v1(),
	"PharmacyName" VARCHAR(100) NOT NULL,
	"SupportsPriorAuth" BOOLEAN NOT NULL,
	"Distance" INT NOT NULL,
	"Copay" INT NOT NULL
);

ALTER TABLE	"PharmacyVisited" ADD CONSTRAINT "PharmacyVisited_Id_PK" PRIMARY KEY ("Id");
ALTER TABLE	"PharmacyNearby" ADD CONSTRAINT "PharmacyNearby_Id_PK" PRIMARY KEY ("Id");

ALTER USER postgres WITH ENCRYPTED PASSWORD 'postgres';
CREATE USER circulo WITH ENCRYPTED PASSWORD 'vPa7tuE*=zKfRb5_';

ALTER DATABASE "Circulo" OWNER TO circulo;

REVOKE CONNECT ON DATABASE "Circulo" FROM PUBLIC;

GRANT CONNECT ON DATABASE "Circulo" TO circulo;

GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA PUBLIC TO circulo;