use StarDeck;

CREATE TABLE Planet(

	ID VARCHAR(15) NOT NULL PRIMARY KEY,
	p_name VARCHAR(35),
	p_image VARCHAR(MAX),
	p_description VARCHAR(1000),
	p_effect VARCHAR(500),
	p_type VARCHAR(30), --Raro, Basico, Popular
	p_status VARCHAR(10), --Activo (default), Desactivado
	UNIQUE(p_name)

);

CREATE TABLE Deck(

	Deck_ID VARCHAR(15) NOT NULL,
	Player_ID VARCHAR(15) NOT NULL,
	Card_ID VARCHAR(15) NOT NULL,
	d_name VARCHAR(50) NOT NULL

);