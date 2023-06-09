﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Newtonsoft.Json;
using StarDeck_API.Models;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

namespace StarDeck_API.DB_Calls
{
    /*
     * Singleton class that manages DB access for the Cards & Users tables.
     */
    public class CardsUsers_DB
    {
        private DBContext context;

        private static CardsUsers_DB instance = null;

        /*
         * Method that allows to get an instance of the class.
         */
        public static CardsUsers_DB GetInstance()
        {
            if (instance == null)
            {
                instance = new CardsUsers_DB();
            }
            return instance;
        }

        /*
         * Method that saves a new user in the DB.
         * Params u - user to save.
         * Return "Saved" if it succeded, exception if it didn't.
         */
        public dynamic PostUser(Users u)
        {
            try
            {
                context.users.Add(u);
                context.SaveChanges();
                return "Saved";
            }
            catch (SqlException ex)
            {
                return ex;
            }
        }

        /*
         * Method that allows to validate user's credentials to access the game.
         * Params: mail - email to validate, password - user's password to validate.
         */
        public string ValidateUser(string mail, string password)
        {
            var errorMessage = new SqlParameter("@error_message", SqlDbType.NVarChar, -1);
            errorMessage.Direction = ParameterDirection.Output;
            var users = context.Database.ExecuteSqlRaw("EXEC ValidateUser @mail, @password, @error_message OUTPUT",
                                                    new SqlParameter("@mail", mail),
                                                    new SqlParameter("@password", password),
                                                    errorMessage);
            return errorMessage.Value.ToString();
        }

        /*
         * Method that gets the user's information from the DB according to the email provided.
         * Params: email - email of the user to get the information.
         * Return: user's information or exception.
         */
        public dynamic GetUser(string email)
        {
            try
            {
                var userInfo = context.users.FromSqlRaw("EXEC GetPlayer @email = {0}", email).ToList();
                if (userInfo.Count == 0)
                {
                    throw new Exception("User not found");
                }
                return userInfo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /*
         * Method that gets the user's information from the DB according to the ID provided.
         * Params: ID - ID of the user to get the information.
         * Return: user's information or exception.
         */
        public dynamic GetUserByID(string ID)
        {
            try
            {
                var userInfo = context.users.FromSqlRaw("EXEC GetUserWithID @ID = {0}", ID).ToList();
                if (userInfo.Count == 0)
                {
                    throw new Exception("User not found");
                }
                return userInfo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /*
         * Method that allows to get all the users from the DB.
         * Return: List of users.
         */
        public List<Users> GetAllUsers()
        {
            List<Users> users = context.users.ToList();
            return users;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public dynamic PostCard(Card card)
        {
            try
            {
                context.cards.Add(card);
                context.SaveChanges();
                return "Saved";
            }
            catch (SqlException ex)
            {
                return ex;
            }
        }

        /*
         * Method that allows to get a random number of cards with different types from the procedure on the DB.
         * Params: num - number of cards to get, types - list of types of cards to get.
         * Return: List of cards.
         */
        public List<Card> GetRandomCardsSP(int num, string Types_String)
        {
            List<Card> RandomCards = new List<Card>();
            var cards = context.cards.FromSqlRaw("EXEC GetRandomCards @num = {0}, @ctypeList = {1}", num, Types_String).ToList();
            RandomCards.AddRange(cards);
            return RandomCards;
        }

        /*
         * Method that allows to check if a user has cards.
         * Params: email - email of the user to check if it has cards.
         * Return: Count of cards.
         */
        public int HasCardsSP(string email)
        {
            SqlParameter count = new SqlParameter("@card_count", SqlDbType.Int);
            count.Direction = ParameterDirection.Output;
            SqlParameter user_email = new SqlParameter("@email", email);
            context.Database.ExecuteSqlRaw("EXEC HasCards @email, @card_count OUTPUT", user_email, count);
            int card_count = Convert.ToInt32(count.Value);
            return card_count;
        }

        /*
         * Method that allows to get the user's cards from the DB.
         * Params: email - email of the user to get the cards.
         * Return: List of cards.
         */
        public dynamic GetUserCards(string email)
        {
            try
            {
                List<Card> cards = context.cards.FromSqlRaw("EXEC GetCards @email = {0}", email).ToList();
                return cards;
            }
            catch (SqlException ex)
            {
                return ex;
            }
        }

        /*
         * Method that allows to get all the cards from the DB.
         * Return: List of all cards.
         */
        public dynamic GetAllCards()
        {
            try
            {
                List<Card> cards = context.cards.ToList();
                return cards;
            }
            catch (SqlException ex)
            {
                return ex;
            }
        }

        /*
         * Method that allows to get a card from the DB.
         * Params: cardID - ID of the card to get.
         * Return: Card.
         */
        public Card GetCard(string cardID)
        {
            Card card = context.cards.FromSqlRaw("EXEC GetCard @cardID = {0}", cardID).ToList()[0];
            if (card == null)
            {
                throw new Exception("Card not found");
            }
            return card;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*
         * Method that saves the user's cards in the DB.
         * Param u_c - user_card structure with the user's and the card's id.
         * No return, exception if it fails.
         */
        public dynamic PostUserCard(User_Card u_c)
        {
            try
            {
                context.user_card.Add(u_c);
                context.SaveChanges();
                return true;
            }
            catch (SqlException ex)
            {
                return ex;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*
         * Method that sets the context of the DB to get access to it.
         */
        public void SetContext(DBContext context)
        {
            this.context = context;
        }

        /*
         * Private constructor to avoid multiple instances of the class.
         */
        private CardsUsers_DB() { }
    }
}
