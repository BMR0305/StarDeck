﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StarDeck_API.Models;

namespace StarDeck_API.DB_Calls
{
    public class Match_DB
    {
        private static Match_DB instance = null;
        private DBContext context;
        public static Match_DB GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Match_DB();
                }
                return instance;
            }
        }

        public void InsertDeckToCardsLeft(string playerID, string deckID)
        {
            try
            {
                context.Database.ExecuteSqlRaw("EXEC InsertDeckCardsLeft @deckID = {0}, @playerID = {1}", deckID, playerID);
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to insert deck to cards left: " + e.Message);
            }
        }

        public void InsertTurn(Turn turn)
        {
            try
            {
                context.turn.Add(turn);
                context.SaveChanges();
            } 
            catch (SqlException e)
            {
                throw new Exception("Failed to insert turn: " + e.Message);
            }
        }

        public void SetContext(DBContext context)
        {
            this.context = context;
        }

        private Match_DB() { }
    }
}
