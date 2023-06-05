using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarDeck_API.DB_Calls;
using StarDeck_API.Models;
using StarDeck_API.Logic_Files;

namespace StarDeck_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly DBContext context;
        public MatchController(DBContext context)
        {
            this.context = context;
            Match_DB.GetInstance.SetContext(context);
        }

        [HttpPost]
        [Route("NewTurn")]
        public dynamic NewTurn(Turn turn)
        {
            Match_DB.GetInstance.SetContext(this.context);
            CardsUsers_DB.GetInstance().SetContext(this.context);
            try
            {

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetHand/{gameID}/{email}")]
        public dynamic GetHand(string gameID, string email)
        {
            Match_DB.GetInstance.SetContext(context);
            CardsUsers_DB.GetInstance().SetContext(context);
            try
            {
                string output = Match_Logic.GetInstance.GetHand(gameID, email);
                return output;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("TakeCard/{email}")]
        public dynamic TakeCard(string email)
        {
            CardsUsers_DB.GetInstance().SetContext(context);
            try
            {
                string output = Match_Logic.GetInstance.TakeCard(email);
                return output;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("EndTurn/{gameID}/{email}")]
        public dynamic EndTurn([FromBody] List<CardPlayed> cardsPlayed, string gameID, string email)
        {
            Match_DB.GetInstance.SetContext(context);
            CardsUsers_DB.GetInstance().SetContext(context);
            try
            {
                string output = Match_Logic.GetInstance.EndTurn(cardsPlayed, gameID, email);
                return output;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetCardsPlayed/{gameID}/{turnID}/{email}")]
        public dynamic GetCardsPlayed(string gameID, string turnID, string email)
        {
            CardsUsers_DB.GetInstance().SetContext(context);
            try
            {
                string output = Match_Logic.GetInstance.GetCardsPlayed(gameID, turnID, email);
                return output;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("EndGame/{gameID}")]
        public dynamic EndGame(string gameID)
        {
            try
            {
                string output = Match_Logic.GetInstance.EndGame(gameID);
                return output;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetGameTurn/{gameID}")]
        public dynamic GetGameTurn(string gameID)
        {
            try
            {
                string output = Match_Logic.GetInstance.GetGameTurn(gameID);
                return output;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetActivePlayer/{gameID}")]
        public dynamic GetActivePlayer(string gameID)
        {
            try
            {
                string output = Match_Logic.GetInstance.GetTurnActivePlayer(gameID);
                return output;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
