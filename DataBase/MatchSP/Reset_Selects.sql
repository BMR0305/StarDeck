SELECT * FROM Users;
SELECT * FROM Turn;
SELECT * FROM CardsLeft;
SELECT * FROM CardPlayed;
SELECT * FROM Partida;
SELECT * FROM PlayerTurn_Card;

SELECT * FROM Deck;
SELECT * FROM Deck_Card;

SELECT * FROM Planet
WHERE p_description = 'Hola'

UPDATE Users
SET u_status = 'A';

UPDATE Partida
SET p_status = 'T';

DELETE FROM CardsLeft
DELETE FROM PlayerTurn_Card
DELETE FROM CardPlayed

DELETE FROM Turn
DELETE FROM Partida

