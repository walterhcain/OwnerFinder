using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using Rocket.Core.Logging;

namespace walterhcain.CheckOwner
{
    public class Find : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }

        public string Name
        {
            get { return "find"; }
        }

        public string Help
        {
            get { return "Finds coordinates of player"; }
        }

        public string Syntax
        {
            get { return "/find"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }
        public List<string> Permissions
        {
            get
            {
                return new List<string>
                {
                  "cain.find"
                };
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller is UnturnedPlayer)
            {
                UnturnedPlayer player = (UnturnedPlayer)caller;
                if (command.Length == 0)
                {
                    UnturnedChat.Say(caller, player.Position.x + " " + player.Position.y + " " + player.Position.z);
                }
                else if (command.Length == 1)
                {
                    UnturnedPlayer target = UnturnedPlayer.FromName(command[0]);
                    UnturnedChat.Say(player, "The target is located at:");
                    UnturnedChat.Say(player, target.Position.x + " " + target.Position.y + " " + target.Position.z);
                }
                else
                {
                    UnturnedChat.Say(player, "Improper Parameters", UnityEngine.Color.red);
                }
            }
            else
            {
                Logger.Log("This command can only be called by a player.");
            }

        }
    }
}
