use LPI

ALTER TABLE LEAD add  dentalNeed SMALLINT not null default 0;
ALTER TABLE LEAD add  dentalCareIsFor SMALLINT not null default 0;
ALTER TABLE LEAD add  preferredAppointmentTime SMALLINT not null default 0;
ALTER TABLE LEAD add  insurancePlanBudget SMALLINT not null default 0;

ALTER TABLE LEAD add  firstName VARCHAR(50) not null default '';
ALTER TABLE LEAD add  lastName VARCHAR(50) not null default '';
ALTER TABLE LEAD add  email VARCHAR(50) not null default '';

go
