
using HarmonyLib;
using Hazel;

namespace TownOfHost
{
    //勝利判定処理
    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CheckEndCriteria))]
    class CheckGameEndPatch
    {
        public static bool Prefix(ShipStatus __instance)
        {
            if (!GameData.Instance) return false;
            if (DestroyableSingleton<TutorialManager>.InstanceExists) return true;
            var statistics = new PlayerStatistics(__instance);
            if (Options.NoGameEnd.GetBool()) return false;

            if (CheckAndEndGameForJester(__instance)) return false;
            if (CheckAndEndGameForTerrorist(__instance)) return false;
            if (CheckAndEndGameForExecutioner(__instance)) return false;
            if (CheckAndEndGameForArsonist(__instance)) return false;
            if (CheckAndEndGameForJackal(__instance, statistics)) return false;
            if (Main.currentWinner == CustomWinner.Default)
            {
                if (Options.CurrentGameMode == CustomGameMode.HideAndSeek)
                {
                    if (CheckAndEndGameForHideAndSeek(__instance, statistics)) return false;
                    if (CheckAndEndGameForTroll(__instance)) return false;
                    if (CheckAndEndGameForTaskWin(__instance)) return false;
                }
                else
                {
                    if (CheckAndEndGameForTaskWin(__instance)) return false;
                    if (CheckAndEndGameForSabotageWin(__instance)) return false;
                    if (CheckAndEndGameForImpostorWin(__instance, statistics)) return false;
                    if (CheckAndEndGameForCrewmateWin(__instance, statistics)) return false;
                }
            }
            return false;
        }

        private static bool CheckAndEndGameForSabotageWin(ShipStatus __instance)
        {
            if (__instance.Systems == null) return false;
            ISystemType systemType = __instance.Systems.ContainsKey(SystemTypes.LifeSupp) ? __instance.Systems[SystemTypes.LifeSupp] : null;
            if (systemType != null)
            {
                LifeSuppSystemType lifeSuppSystemType = systemType.TryCast<LifeSuppSystemType>();
                if (lifeSuppSystemType != null && lifeSuppSystemType.Countdown < 0f)
                {
                    EndGameForSabotage(__instance);
                    lifeSuppSystemType.Countdown = 10000f;
                    return true;
                }
            }
            ISystemType systemType2 = __instance.Systems.ContainsKey(SystemTypes.Reactor) ? __instance.Systems[SystemTypes.Reactor] : null;
            if (systemType2 == null)
            {
                systemType2 = __instance.Systems.ContainsKey(SystemTypes.Laboratory) ? __instance.Systems[SystemTypes.Laboratory] : null;
            }
            if (systemType2 != null)
            {
                ICriticalSabotage criticalSystem = systemType2.TryCast<ICriticalSabotage>();
                if (criticalSystem != null && criticalSystem.Countdown < 0f)
                {
                    EndGameForSabotage(__instance);
                    criticalSystem.ClearSabotage();
                    return true;
                }
            }
            return false;
        }

        private static bool CheckAndEndGameForTaskWin(ShipStatus __instance)
        {
            if (GameData.Instance.TotalTasks <= GameData.Instance.CompletedTasks)
            {
                __instance.enabled = false;
                ResetRoleAndEndGame(GameOverReason.HumansByTask, false);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForImpostorWin(ShipStatus __instance, PlayerStatistics statistics)
        {
            if (statistics.TeamImpostorsAlive >= statistics.TotalAlive - statistics.TeamImpostorsAlive && statistics.TeamJackalsAlive == 0)
            {
                if (Jackal.AliveJackalCount() != 0) return false;
                __instance.enabled = false;
                var endReason = TempData.LastDeathReason switch
                {
                    DeathReason.Exile => GameOverReason.ImpostorByVote,
                    DeathReason.Kill => GameOverReason.ImpostorByKill,
                    _ => GameOverReason.ImpostorByVote,
                };
                ResetRoleAndEndGame(endReason, false);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForCrewmateWin(ShipStatus __instance, PlayerStatistics statistics)
        {
            if (statistics.TeamImpostorsAlive == 0)
            {
                if (Jackal.AliveJackalCount() != 0) return false;
                __instance.enabled = false;
                ResetRoleAndEndGame(GameOverReason.HumansByVote, false);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForHideAndSeek(ShipStatus __instance, PlayerStatistics statistics)
        {
            if (statistics.TotalAlive - statistics.TeamImpostorsAlive == 0)
            {
                __instance.enabled = false;
                ResetRoleAndEndGame(GameOverReason.ImpostorByKill, false);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForTroll(ShipStatus __instance)
        {
            foreach (var pc in PlayerControl.AllPlayerControls)
            {
                var hasRole = Main.AllPlayerCustomRoles.TryGetValue(pc.PlayerId, out var role);
                if (!hasRole) return false;
                if (role == CustomRoles.HASTroll && pc.Data.IsDead)
                {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.TrollWin, Hazel.SendOption.Reliable, -1);
                    writer.Write(pc.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPC.TrollWin(pc.PlayerId);
                    __instance.enabled = false;
                    ResetRoleAndEndGame(GameOverReason.ImpostorByKill, false);
                    return true;
                }
            }
            return false;
        }

        private static bool CheckAndEndGameForJester(ShipStatus __instance)
        {
            if (Main.currentWinner == CustomWinner.Jester && Main.CustomWinTrigger)
            {
                __instance.enabled = false;
                ResetRoleAndEndGame(GameOverReason.ImpostorByKill, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForTerrorist(ShipStatus __instance)
        {
            if (Main.currentWinner == CustomWinner.Terrorist && Main.CustomWinTrigger)
            {
                __instance.enabled = false;
                ResetRoleAndEndGame(GameOverReason.ImpostorByKill, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForExecutioner(ShipStatus __instance)
        {
            if (Main.currentWinner == CustomWinner.Executioner && Main.CustomWinTrigger)
            {
                __instance.enabled = false;
                ResetRoleAndEndGame(GameOverReason.ImpostorByKill, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForArsonist(ShipStatus __instance)
        {
            if (Main.currentWinner == CustomWinner.Arsonist && Main.CustomWinTrigger)
            {
                __instance.enabled = false;
                ResetRoleAndEndGame(GameOverReason.ImpostorByKill, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForJackal(ShipStatus __instance, PlayerStatistics statistics)
        {
            if (Main.currentWinner == CustomWinner.Jackal && Main.CustomWinTrigger)
            {
                __instance.enabled = false;
                ResetRoleAndEndGame(GameOverReason.ImpostorByKill, false);
                return true;
            }
            else if (statistics.TeamJackalsAlive >= statistics.TotalAlive - statistics.TeamJackalsAlive && statistics.TeamImpostorsAlive == 0)
            {
                RPC.JackalWin();
                return true;
            }
            return false;
        }


        private static void EndGameForSabotage(ShipStatus __instance)
        {
            __instance.enabled = false;
            ResetRoleAndEndGame(GameOverReason.ImpostorBySabotage, false);
            return;
        }
        private static void ResetRoleAndEndGame(GameOverReason reason, bool showAd)
        {
            foreach (var pc in PlayerControl.AllPlayerControls)
            {
                var LoseImpostorRole = Main.AliveImpostorCount == 0 ? pc.Is(RoleType.Impostor) : pc.Is(CustomRoles.Egoist);
                if (pc.Is(CustomRoles.Sheriff) || (!(Main.currentWinner == CustomWinner.Arsonist) && pc.Is(CustomRoles.Arsonist)) ||  (!(Main.currentWinner == CustomWinner.Jackal) && pc.Is(CustomRoles.Jackal)) || LoseImpostorRole)
                {
                    pc.RpcSetRole(RoleTypes.GuardianAngel);
                }
            }
            new LateTask(() =>
            {
                ShipStatus.RpcEndGame(reason, showAd);
            }, 0.5f, "EndGameTask");
        }
        //プレイヤー統計
        internal class PlayerStatistics
        {
            public int TeamImpostorsAlive { get; set; }
            public int TeamJackalsAlive { get; set; }
            public int TotalAlive { get; set; }

            public PlayerStatistics(ShipStatus __instance)
            {
                GetPlayerCounts();
            }

            private void GetPlayerCounts()
            {
                int numImpostorsAlive = 0;
                int numJackalsAlive = 0;
                int numTotalAlive = 0;

                for (int i = 0; i < GameData.Instance.PlayerCount; i++)
                {
                    GameData.PlayerInfo playerInfo = GameData.Instance.AllPlayers[i];
                    var hasHideAndSeekRole = Main.AllPlayerCustomRoles.TryGetValue((byte)i, out var role);
                    if (!playerInfo.Disconnected)
                    {
                        if (!playerInfo.IsDead)
                        {
                            if (Options.CurrentGameMode != CustomGameMode.HideAndSeek || !hasHideAndSeekRole)
                            {
                                if (playerInfo.PlayerId < 15)
                                {
                                numTotalAlive++;//HideAndSeek以外
                                }
                            }
                            else
                            {
                                //HideAndSeek中
                                if (role == CustomRoles.Crewmate && playerInfo.PlayerId < 15) numTotalAlive++;
                            }
                            if (role == CustomRoles.Jackal && playerInfo.PlayerId < 15)
                            {
                                numJackalsAlive++;
                            }
                                if (role is not CustomRoles.HASFox and not CustomRoles.HASTroll) numTotalAlive++;

                            if (playerInfo.Role.TeamType == RoleTeamTypes.Impostor &&(
                            playerInfo.GetCustomRole() != CustomRoles.Sheriff || playerInfo.GetCustomRole() != CustomRoles.Arsonist || playerInfo.GetCustomRole() != CustomRoles.Jackal))
                            {
                                if (playerInfo.PlayerId < 15)
                                {
                                    numImpostorsAlive++;
                                }
                            }
                        }
                    }
                }

                TeamImpostorsAlive = numImpostorsAlive;
                TeamJackalsAlive = numJackalsAlive;
                TotalAlive = numTotalAlive;
            }
        }
    }
}