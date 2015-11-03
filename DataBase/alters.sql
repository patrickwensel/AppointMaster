ALTER TABLE lead  ADD birthday 	DATETIME	NOT NULL default '1900/1/1';


ALTER TABLE patient ADD 	created			DATETIME	NOT NULL default '2011/1/1';


ALTER TABLE dentalprocedure ADD 	ID			VARCHAR(12)	NOT NULL default '';