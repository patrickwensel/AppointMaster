use LPI

ALTER TABLE LEAD add  original tinyint not null default 0;

ALTER TABLE LEAD add  alterPrimaryPhone varchar(15)

ALTER TABLE LEAD add  alterName varchar(50)
ALTER TABLE LEAD add  alterFirstName varchar(50)

ALTER TABLE CAMPAIGN add  visible tinyint;


