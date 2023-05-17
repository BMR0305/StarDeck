ALTER TABLE Deck
ADD CONSTRAINT PK_Deck
PRIMARY KEY (Deck_ID,Player_ID,Card_ID,d_name);

ALTER TABLE Deck
ADD CONSTRAINT FK_UserDeck
FOREIGN KEY (Player_ID) REFERENCES Users(ID);

ALTER TABLE Deck
ADD CONSTRAINT FK_CardDeck
FOREIGN KEY (Card_ID) REFERENCES Cards(ID);