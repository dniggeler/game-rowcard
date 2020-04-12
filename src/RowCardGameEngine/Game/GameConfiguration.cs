using LanguageExt;

namespace AzulGameEngine.Game
{
    public static class GameConfiguration
    {
        public static int MaxPlayers = 4;
        public static int MinPlayers = 2;

        public static Option<int> NumberOfTileBowls(int players) => players switch
        {
            2 => 5,
            3 => 7,
            4 => 9,
            _ => Option<int>.None
        };
    }
}