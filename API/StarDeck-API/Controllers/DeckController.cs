﻿using Microsoft.AspNetCore.Mvc;
using StarDeck_API.Models;
using Newtonsoft.Json;
using StarDeck_API.Support_Components;
using Microsoft.EntityFrameworkCore;

namespace StarDeck_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DeckController : ControllerBase
    {
        private KeyGen KeyGenerator = KeyGen.GetInstance();

        //DB context
        private readonly DBContext context;
        public DeckController(DBContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("post")]
        public dynamic PostDeck([FromBody] Deck_Aux d)
        {
            try
            {
                string Message = Deck_DB.GetInstance().PostDeck(d,context);
                return Message;
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("get/{id}")]
        public dynamic GetDeck(string id)
        {
            try
            {
                string deck = Deck_DB.GetInstance().GetDeck(context, id);
                return Ok(deck);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("getPlayerDecks/{email}")]
        public dynamic GetPlayerDecks(string email)
        {
            try
            {
                string decks = Deck_DB.GetInstance().GetPlayerDecks(context, email);
                return decks;
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
