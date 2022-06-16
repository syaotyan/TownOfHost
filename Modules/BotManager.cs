using InnerNet;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TownOfHost
{
    public static class BotManager
    {
        public static List<PlayerControl> Bots = new();
        public static PlayerControl Spawn(string name = "")
        {
            byte id = 0;
            foreach (PlayerControl p in PlayerControl.AllPlayerControls)
            {
                if (p.PlayerId > id)
                    {
                        id = p.PlayerId;
                    }
            }
            var bot = UnityEngine.Object.Instantiate(AmongUsClient.Instance.PlayerPrefab);
            id++;
            bot.PlayerId = id;
            GameData.Instance.AddPlayer(bot);
            AmongUsClient.Instance.Spawn(bot, -2, SpawnFlags.None);
            bot.transform.position = new Vector3(999f,999f,999f);
            bot.NetTransform.enabled = true;
            GameData.Instance.RpcSetTasks(bot.PlayerId, new byte[0]);

            bot.RpcSetName(name);
            bot.RpcSetColor(1);
            bot.RpcSetHat("hat_NoHat");
            bot.RpcSetPet("peet_EmptyPet");
            bot.RpcSetVisor("visor_EmptyVisor");
            bot.RpcSetNamePlate("nameplate_NoPlate");
            bot.RpcSetSkin("skin_None");
            GameData.Instance.RpcSetTasks(bot.PlayerId, new byte[0]);

            Bots.Add(bot);
            return bot;
        }
        public static void BotDespawn(this PlayerControl bot)
        {
            Bots.RemoveAll(x => bot.PlayerId == x.PlayerId);
            AmongUsClient.Instance.Despawn(bot);
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