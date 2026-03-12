using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace walterhcain.CheckOwner
{
    class OwnerCheckerCommand : IRocketCommand
    {
        RaycastHit hit;
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }

        public string Name
        {
            get { return "checkowner"; }
        }

        public string Help
        {
            get { return "Check the owner of a certain object"; }
        }

        public string Syntax
        {
            get { return "/checkowner"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (Physics.Raycast(player.Player.look.aim.position, player.Player.look.aim.forward, out hit, 10, RayMasks.BARRICADE_INTERACT))
            {
                byte x;
                byte y;

                ushort plant;
                ushort index;

                BarricadeRegion r;
                StructureRegion s;

                Transform transform = hit.transform;
                InteractableVehicle vehicle = transform.gameObject.GetComponent<InteractableVehicle>();

                if (transform.GetComponent<InteractableDoorHinge>() != null)
                {
                    transform = transform.parent.parent;
                }
                BarricadeDrop bd = BarricadeManager.FindBarricadeByRootTransform(transform);
                StructureDrop sd = StructureManager.FindStructureByRootTransform(transform);
                if(bd != null)
                {
                    BarricadeData bdata = bd.GetServersideData();
                    Library.TellInfo(caller, (CSteamID)bdata.owner, (CSteamID)bdata.group);
                }
                else if (sd != null)
                {
                    StructureData sdata = sd.GetServersideData();

                    Library.TellInfo(caller, (CSteamID)sdata.owner, (CSteamID)sdata.group);
                }
                else if (vehicle != null)
                {
                    if (vehicle.lockedOwner != CSteamID.Nil)
                    {
                        Library.TellInfo(caller, vehicle.lockedOwner, vehicle.lockedGroup);
                        return;
                    }
                    UnturnedChat.Say(caller, "Vehicle does not have an owner.");
                }
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>
                {
                  "cain.checkowner"
                };
            }
        }
    }
}

