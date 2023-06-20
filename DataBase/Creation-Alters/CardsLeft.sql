CREATE TABLE CardsLeft(

	Player_ID VARCHAR(15) NOT NULL,
	Card_ID VARCHAR(15) NOT NULL

);

ALTER TABLE CardsLeft
ADD CONSTRAINT PK_CLeft
PRIMARY KEY (Player_ID, Card_ID)

ALTER TABLE CardsLeft
ADD CONSTRAINT FK_CLeft1
FOREIGN KEY (Player_ID) REFERENCES Users(ID);

ALTER TABLE CardsLeft
ADD CONSTRAINT FK_CLeft2
FOREIGN KEY (Card_ID) REFERENCES Cards(ID);