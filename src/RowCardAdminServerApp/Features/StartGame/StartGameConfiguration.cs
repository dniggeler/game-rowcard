using System.ComponentModel.DataAnnotations;

namespace RowCardAdminServerApp.Features.StartGame
{
    public class StartGameConfiguration
    {
        [Required]
        public string Token { get; set; }

        [Range(typeof(int), "1", "4", ErrorMessage = "Wert {0} muss zwischen {1} und {2} sein")]
        public int NumberOfPlayers { get; set; }

        [Range(typeof(int), "1", "4", ErrorMessage = "Wert {0} muss zwischen {1} und {2} sein")]
        public int NumberOfRealPlayers { get; set; }
    }
}
