use StarDeck;

CREATE TABLE Users(
	ID VARCHAR(15) NOT NULL PRIMARY KEY,
	email VARCHAR(100),
	nickname VARCHAR(30),
	u_name VARCHAR(30),
	birthday DATE,
	nacionality VARCHAR(50),
	u_password VARCHAR(8),
	u_status VARCHAR(10),
	avatar VARCHAR(1000),
	ranking INT,
	coins INT,
	UNIQUE(email)
);