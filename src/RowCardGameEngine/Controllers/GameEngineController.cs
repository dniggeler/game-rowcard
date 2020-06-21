using System.Threading.Tasks;
using AzulGameEngine.ChatHub;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RowCardGameEngine.Controllers.Models;
using RowCardGameEngine.Game;
using RowCardGameEngine.Game.Models;


namespace RowCardGameEngine.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api")]
    public class GameEngineController : ControllerBase
    {
        private const long GameEngineId = 1;
        private readonly GameManager gameManager;
        private readonly IChatClient chat;

        public GameEngineController(
            GameManager gameManager,
            IChatClient chat)
        {
            this.gameManager = gameManager;
            this.chat = chat;
        }

        [HttpPost("game")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult CreateGame()
        {
            return gameManager.GetEngine(GameEngineId)
                .Setup()
                .Match<ActionResult>(
                Right: id => Created(HttpContext.Request.Path.Value, id),
                Left: BadRequest);
        }

        [HttpGet("players")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Get()
        {
            var players = gameManager.GetEngine(GameEngineId).GetPlayers();

            return Ok(players);
        }

        [HttpPost("players/{name}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Create(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("invalid player name");
            }

            Either<string, long> command = gameManager.GetEngine(GameEngineId).AddPlayer(name);

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

        [HttpPost("game/setStartingPlayer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult SetStartingPlayer(long playerId)
        {
            return
                gameManager.GetEngine(GameEngineId)
                    .SetStartingPlayer(playerId)
                    .Match<ActionResult>(
                            Right: _ => Ok(),
                            Left: BadRequest);
        }

        [HttpPost("game/setStartCard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult SetStartCard([FromBody]PlayCardRequest request)
        {
            if (request == null)
            {
                return BadRequest(nameof(request));
            }

            return
                gameManager.GetEngine(GameEngineId)
                    .SetStartCard(request.PlayerId, new Card(request.Suit, request.Rank))
                    .Match<ActionResult>(
                        Right: _ => Ok(),
                        Left: BadRequest);
        }

        [HttpPost("game/playCard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult PlayCard([FromBody] PlayCardRequest request)
        {
            if (request == null)
            {
                return BadRequest(nameof(request));
            }

            return
                gameManager.GetEngine(GameEngineId)
                    .PlayCard(request.PlayerId, new Card(request.Suit, request.Rank))
                    .Match<ActionResult>(
                        Right: _ => Ok(),
                        Left: BadRequest);
        }

        [HttpGet("game/history")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetHistory()
        {
            return Ok(gameManager.GetEngine(GameEngineId).GetActionHistory());
        }

        [HttpPost("game/newGame")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult NewGame()
        {
            var result = gameManager.GetEngine(GameEngineId).NewGame();
            return result.Match(
                Right: id => Ok(id),
                Left: Ok);
        }

        [HttpPost("game/reset")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult Reset()
        {
            var result = gameManager.GetEngine(GameEngineId).Reset();
            return result.Match(
                Right: id => Ok(id),
                Left: Ok);
        }
    }
}