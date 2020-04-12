using System;
using System.Linq;
using LanguageExt;

namespace TestClient.Console
{
    public enum ClientAction { None, Quit, Send, JoinGame, Help }

    public sealed class Command
    {
        public ClientAction ClientAction { get; set; }

        public Option<string> Option1 { get; set; }

        private Command(ClientAction action)
        {
            this.ClientAction = action;
        }

        private Command(ClientAction action, string option1)
            :this(action)
        {
            Option1 = option1;
        }

        public static Command For(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new Command(ClientAction.None);
            }

            var parts = input.Split(':', StringSplitOptions.RemoveEmptyEntries);

            string actionStr = parts
                .DefaultIfEmpty("h")
                .First();

            return actionStr switch
            {
                "q" => new Command(ClientAction.Quit),
                "j" => new Command(ClientAction.JoinGame),
                "s" => new Command(ClientAction.Send, GetOption(1)),
                "h" => new Command(ClientAction.Help),
                _ => new Command(ClientAction.None)
            };

            string GetOption(int index)
            {
                if (parts.Length < index+1) // option plus action
                {
                    return null;
                }

                return parts[index];
            }
        }
    }
}