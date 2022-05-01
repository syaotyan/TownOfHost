using InnerNet;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TownOfHost
{
    public static class BotManager
    {
        public static List<PlayerControl> Bots = new List<PlayerControl>();
        public static PlayerControl Spawn(string name = "", int id = -1)
        {
            PlayerControl bot = UnityEngine.Object.Instantiate(AmongUsClient.Instance.PlayerPrefab);
            bot.PlayerId = 15;
            GameData.Instance.AddPlayer(bot);
            AmongUsClient.Instance.Spawn(bot, -2, SpawnFlags.None);
            bot.transform.position = new Vector2(999f,999f);
            bot.NetTransform.enabled = true;
            GameData.Instance.RpcSetTasks(bot.PlayerId, new byte[0]);


            bot.RpcSetColor((byte)PlayerControl.LocalPlayer.CurrentOutfit.ColorId);
            bot.RpcSetName(PlayerControl.LocalPlayer.name);
            bot.RpcSetPet(PlayerControl.LocalPlayer.CurrentOutfit.PetId);
            bot.RpcSetSkin(PlayerControl.LocalPlayer.CurrentOutfit.SkinId);
            bot.RpcSetNamePlate(PlayerControl.LocalPlayer.CurrentOutfit.NamePlateId);

            Bots.Add(bot);
            return bot;
        }
        public static void BotDespawn(this PlayerControl player)
        {
            Bots.RemoveAll(x => player.PlayerId == x.PlayerId);
            player.Despawn();
        }
        public static void AllDespawn()
        {
            foreach (PlayerControl bot in Bots)
            {
                bot.BotDespawn();
            }
            Bots = new List<PlayerControl>();
        }
    }
}