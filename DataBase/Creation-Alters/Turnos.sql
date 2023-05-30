CREATE TABLE Turn(

	Turn_ID VARCHAR(15) NOT NULL PRIMARY KEY,
	Game_ID VARCHAR(15) NOT NULL,
	Active_Player VARCHAR(15) NOT NULL

);

ALTER TABLE Turn
ADD CONSTRAINT FK_Turn
FOREIGN KEY (Game_ID) REFERENCES Partida(ID);

ALTER TABLE Turn
ADD CONSTRAINT FK_TurnPlayer
FOREIGN KEY (Active_Player) REFERENCES Users(ID);

CREATE TABLE PlayerTurn_Card(

	PlayerID VARCHAR(15) NOT NULL,
	TurnID VARCHAR(15) NOT NULL,
	Card1_ID VARCHAR(15),
	Card2_ID VARCHAR(15),
	Card3_ID VARCHAR(15),
	Card4_ID VARCHAR(15),
	Card5_ID VARCHAR(15)

);

ALTER TABLE PlayerTurn_Card
ADD CONSTRAINT PK_PTC
PRIMARY KEY (PlayerID,TurnID);

ALTER TABLE PlayerTurn_Card
ADD CONSTRAINT FK_Player_TC
FOREIGN KEY (PlayerID) REFERENCES Turn(Turn_ID);

ALTER TABLE PlayerTurn_Card
ADD CONSTRAINT FK_PC_Turn
FOREIGN KEY (TurnID) REFERENCES Cards(ID);

ALTER TABLE PlayerTurn_Card
ADD CONSTRAINT FK_PC1
FOREIGN KEY (Card1_ID) REFERENCES Cards(ID);

ALTER TABLE PlayerTurn_Card
ADD CONSTRAINT FK_PC2
FOREIGN KEY (Card2_ID) REFERENCES Cards(ID);

ALTER TABLE PlayerTurn_Card
ADD CONSTRAINT FK_PC3
FOREIGN KEY (Card3_ID) REFERENCES Cards(ID);

ALTER TABLE PlayerTurn_Card
ADD CONSTRAINT FK_PC4
FOREIGN KEY (Card4_ID) REFERENCES Cards(ID);

ALTER TABLE PlayerTurn_Card
ADD CONSTRAINT FK_PC5
FOREIGN KEY (Card5_ID) REFERENCES Cards(ID);