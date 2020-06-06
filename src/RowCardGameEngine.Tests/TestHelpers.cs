using RowCardGameEngine.Game;
using RowCardGameEngine.Game.Models;


namespace RowCardGameEngine.Tests
{
    public static class TestHelpers
    {
        public const string PlayerNameA = "a";
        public const string PlayerNameB = "b";

        public static (long StartingPlayer, long NotStartingPlayer) SetupWithTwoPlayer(
            this IGameEngine gameEngine, Card startingCard)
        {
            long startingPlayerId = gameEngine.AddPlayer(PlayerNameA).IfLeft(0);
            long notStartingPlayerId = gameEngine.AddPlayer(PlayerNameB).IfLeft(0);

            var result = from id in gameEngine.Setup()
                from p in gameEngine.SetStartingPlayer(startingPlayerId)
                from c in gameEngine.SetStartCard(startingPlayerId, startingCard)
                select id;

            return (startingPlayerId, notStartingPlayerId);
        }
    }
}