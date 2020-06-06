namespace RowCardGameEngine.Game
{
    public class GameManager
    {
        private readonly IGameEngine gameEngine;

        public GameManager(IGameEngine gameEngine)
        {
            this.gameEngine = gameEngine;
        }

        public IGameEngine GetEngine(long gameEngineId)
        {
            if (gameEngineId > 0)
            {
                return gameEngine;
            }

            return null;
        }
    }
}