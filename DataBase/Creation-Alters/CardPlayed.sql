CREATE TABLE CardPlayed(

	GameID VARCHAR(15) NOT NULL,
	CardID VARCHAR(15) NOT NULL,
	PlayerID VARCHAR(15) NOT NULL,
	Planet VARCHAR(15) NOT NULL,
	Turn VARCHAR(15) NOT NULL

);

ALTER TABLE CardPlayed
ADD CONSTRAINT PK_CardPlayed
PRIMARY KEY (GameID, CardID, PlayerID);

ALTER TABLE CardPlayed
ADD CONSTRAINT FK_CPGame
FOREIGN KEY (GameID) REFERENCES Partida(ID);

ALTER TABLE CardPlayed
ADD CONSTRAINT FK_CPCard
FOREIGN KEY (CardID) REFERENCES Cards(ID);

ALTER TABLE CardPlayed
ADD CONSTRAINT FK_CPPlayer
FOREIGN KEY (PlayerID) REFERENCES Users(ID);

ALTER TABLE CardPlayed
ADD CONSTRAINT FK_CPPlanet
FOREIGN KEY (Planet) REFERENCES Planet(ID);

ALTER TABLE CardPlayed
ADD CONSTRAINT FK_CPTurn
FOREIGN KEY (Turn) REFERENCES Turn(Turn_ID);