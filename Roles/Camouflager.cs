using Hazel;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using static TownOfHost.Translator;

namespace TownOfHost
{
    public static class Camouflager
    {
        static int Id = 2500;

        static CustomOption CamouflagerCamouflageCoolDown;
        static CustomOption CamouflagerCamouflageDuration;
        public static void SetupCustomOption()
        {
            Options.SetupRoleOptions(Id, CustomRoles.Camouflager);
            CamouflagerCamouflageCoolDown = CustomOption.Create(Id + 10, Color.white, "Camouflager Camouflage CoolDown", 2.5f, 2.5f, 60f, 2.5f, Options.CustomRoleSpawnChances[CustomRoles.Camouflager]);
            CamouflagerCamouflageDuration = CustomOption.Create(Id + 11, Color.white, "Camouflager Camouflage Duration", 2.5f, 2.5f, 60f, 2.5f, Options.CustomRoleSpawnChances[CustomRoles.Camouflager]);
        }
        public static void ShapeShiftState(PlayerControl pc, bool shapeshifting)
        {
            Logger.info($"Camouflager ShapeShift");
            if (pc == null || pc.Data.IsDead || !shapeshifting) return;
            foreach (PlayerControl target in PlayerControl.AllPlayerControls)
                target.RpcShapeshift(PlayerControl.LocalPlayer, false);
        }
    }
}