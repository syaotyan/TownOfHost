using System.Collections.Generic;

namespace TownOfHost
{
    public static class Camouflague
    {
        /*public static Dictionary<byte, (int, string, string, string, string)> AllPlayerSkin = new(); //Key : PlayerId, Value : (1: color, 2: hat, 3: skin, 4:visor, 5: pet)
        public static void SaveSkin()
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                var color = player.CurrentOutfit.ColorId;
                var hat = player.CurrentOutfit.HatId;
                var skin = player.CurrentOutfit.SkinId;
                var visor = player.CurrentOutfit.VisorId;
                var pet = player.CurrentOutfit.PetId;
                AllPlayerSkin[player.PlayerId] = (color, hat, skin, visor, pet);
            }
        }*/
        public static void Cause()
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                player.RpcSetName("");
                player.RpcSetColorV2(15); //グレー
                player.RpcSetHatV2("");
                player.RpcSetSkinV2("");
                player.RpcSetVisorV2("");
                player.RpcSetPetV2("");
            }
        }
        public static void Revert()
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                player.RpcSetName(player.Data.DefaultOutfit.PlayerName);
                player.RpcSetColorV2(player.Data.DefaultOutfit.ColorId);
                player.RpcSetHatV2(player.Data.DefaultOutfit.HatId);
                player.RpcSetSkinV2(player.Data.DefaultOutfit.SkinId);
                player.RpcSetVisorV2(player.Data.DefaultOutfit.VisorId);
                player.RpcSetPetV2(player.Data.DefaultOutfit.PetId);
                Utils.NotifyRoles();
            }
        }
    }
}