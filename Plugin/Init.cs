using System;
using System.IO;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;

namespace walterhcain.CheckOwner
{
    public class Init : RocketPlugin<WCOCConfig>
    {
        public static Init Instance;

        public string directory = System.IO.Directory.GetCurrentDirectory() + "/..";


        protected override void Load()
        {
            Instance = this;
            if (File.Exists(directory + "/Barricade-Library.txt"))
            {
                Logger.Log(directory + "/Barricade-Library.txt Already exists, loopholing...");
            }
            else
            {
                File.CreateText(directory + "/Barricade-Library.txt");
            }
            if (File.Exists(directory + "/Harvest-Library.txt"))
            {
                Logger.Log(directory + "/Harvest-Library.txt Already exists, loopholing...");
            }
            else
            {
                File.CreateText(directory + "/Barricade-Library.txt");
            }
            BarricadeManager.onBarricadeSpawned += CainTrigger;
            InteractableFarm.OnHarvestRequested_Global += CainHarvest;
            Rocket.Core.Logging.Logger.Log("Cain's Owner Checker has been successfully loaded");

        }

        

        private void CainTrigger(BarricadeRegion br, BarricadeDrop bd)
        {
            if (Configuration.Instance.recordBarricades)
            {
                BarricadeData bdrop = bd.GetServersideData();
                if (bd.asset.type != EItemType.FARM) {
                    UnturnedPlayer player = UnturnedPlayer.FromCSteamID((CSteamID)bdrop.owner);
                    Logger.Log(player.CharacterName + " placed a " + bd.asset.name + " at: " + bdrop.point.x.ToString() + ", " + bdrop.point.y.ToString() + ", " + bdrop.point.z.ToString());
                    using (StreamWriter w = File.AppendText(directory + "/Barricade-Library.txt"))
                    {
                        w.WriteLine(player.CharacterName + " placed a " + bd.asset.name + " at: " + bdrop.point.x.ToString() + ", " + bdrop.point.y.ToString() + ", " + bdrop.point.z.ToString());
                        w.Close();
                    }
                }

            }
        }

        private void CainHarvest(InteractableFarm harvestable, SteamPlayer instigatorPlayer, ref bool shouldAllow)
        {
            if (shouldAllow)
            {
                if (Configuration.Instance.recordHarvest)
                {

                    if (harvestable.IsFullyGrown)
                    {
                        
                        BarricadeData bd = BarricadeManager.FindBarricadeByRootTransform(harvestable.transform).GetServersideData();
                        if (Configuration.Instance.TrackedHarvestables.Contains(bd.barricade.asset.id))
                        {
                            if ((CSteamID)bd.owner != UnturnedPlayer.FromSteamPlayer(instigatorPlayer).CSteamID)
                            {
                                if ((CSteamID)bd.group != UnturnedPlayer.FromSteamPlayer(instigatorPlayer).SteamGroupID)
                                {
                                    Logger.Log("CHECK HARVEST LIBRARY FILE");
                                    using (StreamWriter w = File.AppendText(directory + "/Harvest-Library.txt"))
                                    {
                                        w.WriteLine("*****" + UnturnedPlayer.FromSteamPlayer(instigatorPlayer).CharacterName + " harvested a " + bd.barricade.asset.name + " at: " + bd.point.x.ToString() + ", " + bd.point.y.ToString() + ", " + bd.point.z.ToString() + ". It belongs to " + bd.owner.ToString());
                                        w.Close();
                                    }
                                }
                                else
                                {
                                    using (StreamWriter w = File.AppendText(directory + "/Harvest-Library.txt"))
                                    {
                                        w.WriteLine("%%%%%" + UnturnedPlayer.FromSteamPlayer(instigatorPlayer).CharacterName + " harvested a " + bd.barricade.asset.name + " at: " + bd.point.x.ToString() + ", " + bd.point.y.ToString() + ", " + bd.point.z.ToString() + "Same group");
                                        w.Close();
                                    }
                                }

                            }
                            else
                            {
                                using (StreamWriter w = File.AppendText(directory + "/Harvest-Library.txt"))
                                {
                                    w.WriteLine(UnturnedPlayer.FromSteamPlayer(instigatorPlayer).CharacterName + " harvested a " + bd.barricade.asset.name + " at: " + bd.point.x.ToString() + ", " + bd.point.y.ToString() + ", " + bd.point.z.ToString());
                                    w.Close();
                                }
                            }
                        }
                    }
                }
            }
        }




    }
}