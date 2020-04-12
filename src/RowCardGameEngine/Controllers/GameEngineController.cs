using System.Threading.Tasks;
using AzulGameEngine.ChatHub;
using AzulGameEngine.Game;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzulGameEngine.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api")]
    public class GameEngineController : ControllerBase
    {
        private readonly GameEngine gameEngine;
        private readonly IChatClient chat;

        public GameEngineController(
            GameEngine gameEngine,
            IChatClient chat)
        {
            this.gameEngine = gameEngine;
            this.chat = chat;
        }

        [HttpPost("game")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult CreateGame()
        {
            return gameEngine.Create()
                .Match<ActionResult>(
                Right: t => Created(HttpContext.Request.Path.Value, t.GameId),
                Left: err => BadRequest(err));
        }

        [HttpGet("players")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Get()
        {
            return Ok(gameEngine.GetPlayers());
        }

        [HttpPost("players/{name}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Create(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("invalid player name");
            }

            Either<string, long> command = gameEngine.AddPlayer(name);

            ActionResult response = command
                    .Match<ActionResult>(
                        Right: playerId => Created(HttpContext.Request.Path.Value, playerId),
                        Left: err => BadRequest(err));

            if (command.IsRight)
            {
                await this.chat.SendNewPlayerMessage(name);
            }

            return response;
        }
    }
}