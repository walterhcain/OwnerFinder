using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace walterhcain.CheckOwner
{
    public class WCOCConfig : IRocketPluginConfiguration
    {
        public bool recordBarricades;
        public bool recordHarvest;

        public List<ushort> TrackedHarvestables;

        [XmlElement("usePlayerInfoLib")]
        public bool usePlayerInfoLib;

        [XmlElement("SayPlayerID")]
        public bool SayPlayerID;

        [XmlElement("SayPlayerCharacterName")]
        public bool SayPlayerCharacterName;

        [XmlElement("SayPlayerSteamName")]
        public bool SayPlayerSteamName;

        [XmlElement("SayGroupID")]
        public bool SayGroupID;

        [XmlElement("SayGroupName")]
        public bool SayGroupName;

        public void LoadDefaults()
        {
            recordBarricades = true;
            usePlayerInfoLib = false;
            SayPlayerID = true;
            SayPlayerCharacterName = true;
            SayPlayerSteamName = true;
            SayGroupID = true;
            SayGroupName = true;
            recordHarvest = true;
            TrackedHarvestables = new List<ushort>() { 56797, 56798 };
        }
    }
}