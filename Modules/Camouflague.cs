using System.Collections.Generic;

namespace TownOfHost
{
    public static class Camouflague
    {
        public static bool IsActive = false;
        public static void Cause()
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                player.RpcSetCamouflague();
                IsActive = true;
                Utils.NotifyRoles();
            }
        }
        public static void Revert()
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                player.RpcRevertSkins();
                IsActive = false;
                Utils.NotifyRoles();
            }
        }
        public static void RpcSetCamouflague(this PlayerControl player)
        {
            if (!AmongUsClient.Instance.AmHost) return;

            int colorId = Main.AllPlayerSkin[player.PlayerId].Item1;


            var sender = CustomRpcSender.Create(name: "RpcSetCamouflague");

            player.SetColor(15); //グレー
            sender.AutoStartRpc(player.NetId, (byte)RpcCalls.SetColor);
            sender.Write(15);
            sender.EndRpc();

            player.SetHat("", colorId);
            sender.AutoStartRpc(player.NetId, (byte)RpcCalls.SetHat);
            sender.Write("");
            sender.EndRpc();

            player.SetSkin("", colorId);
            sender.AutoStartRpc(player.NetId, (byte)RpcCalls.SetSkin);
            sender.Write("");
            sender.EndRpc();

            player.SetVisor("");
            sender.AutoStartRpc(player.NetId, (byte)RpcCalls.SetVisor);
            sender.Write("");
            sender.EndRpc();

            player.SetPet("");
            sender.AutoStartRpc(player.NetId, (byte)RpcCalls.SetPet);
            sender.Write("");
            sender.EndRpc();

            sender.SendMessage();
        }

        public static void RpcRevertSkins(this PlayerControl player)
        {
            if (!AmongUsClient.Instance.AmHost) return;

            int colorId = Main.AllPlayerSkin[player.PlayerId].Item1;
            string hatId = Main.AllPlayerSkin[player.PlayerId].Item2;
            string skinId = Main.AllPlayerSkin[player.PlayerId].Item3;
            string visorId = Main.AllPlayerSkin[player.PlayerId].Item4;
            string petId = Main.AllPlayerSkin[player.PlayerId].Item5;


            var sender = CustomRpcSender.Create(name: "RpcRevertSkins");

            player.SetColor(colorId); //グレー
            sender.AutoStartRpc(player.NetId, (byte)RpcCalls.SetColor);
            sender.Write(colorId);
            sender.EndRpc();

            player.SetHat(hatId, colorId);
            sender.AutoStartRpc(player.NetId, (byte)RpcCalls.SetHat);
            sender.Write(hatId);
            sender.EndRpc();

            player.SetSkin(skinId, colorId);
            sender.AutoStartRpc(player.NetId, (byte)RpcCalls.SetSkin);
            sender.Write(skinId);
            sender.EndRpc();

            player.SetVisor(visorId);
            sender.AutoStartRpc(player.NetId, (byte)RpcCalls.SetVisor);
            sender.Write(visorId);
            sender.EndRpc();

            player.SetPet(petId);
            sender.AutoStartRpc(player.NetId, (byte)RpcCalls.SetPet);
            sender.Write(petId);
            sender.EndRpc();

            sender.SendMessage();
        }
    }
}