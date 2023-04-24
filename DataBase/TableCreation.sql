use StarDeck;

CREATE TABLE Users(
	ID VARCHAR(15) NOT NULL PRIMARY KEY,
	email VARCHAR(100),
	nickname VARCHAR(30),
	u_name VARCHAR(30),
	birthday DATE,
	nationality VARCHAR(50),
	u_password VARCHAR(8),
	u_status VARCHAR(10),
	avatar VARCHAR(MAX),
	ranking INT,
	coins INT,
	UNIQUE(email)
);

CREATE TABLE Cards(
	ID VARCHAR(15) NOT NULL PRIMARY KEY,
	c_name VARCHAR(30),
	battle_pts INT,
	energy INT,
	c_image VARCHAR(MAX),
	c_type VARCHAR(5),
	race VARCHAR(20),
	c_status VARCHAR(20),
	c_description VARCHAR(1000)
);

CREATE TABLE User_card(
	user_key VARCHAR(15) NOT NULL,
	card_key VARCHAR(15) NOT NULL
);

CREATE TABLE Race(
	r_name VARCHAR(20) NOT NULL PRIMARY KEY
);