using UnityEngine;

namespace TownOfHost
{
    public static class Camouflager
    {
        static int Id = 2500;

        public static CustomOption CamouflagerCamouflageCoolDown;
        public static CustomOption CamouflagerCamouflageDuration;
        public static void SetupCustomOption()
        {
            Options.SetupRoleOptions(Id, CustomRoles.Camouflager);
            CamouflagerCamouflageCoolDown = CustomOption.Create(Id + 10, Color.white, "Camouflager Camouflage CoolDown", 2.5f, 2.5f, 60f, 2.5f, Options.CustomRoleSpawnChances[CustomRoles.Camouflager]);
            CamouflagerCamouflageDuration = CustomOption.Create(Id + 11, Color.white, "Camouflager Camouflage Duration", 2.5f, 2.5f, 60f, 2.5f, Options.CustomRoleSpawnChances[CustomRoles.Camouflager]);
        }
        public static void ShapeShiftState(PlayerControl pc, bool shapeshifting)
        {
            if (shapeshifting)
            {
                Logger.info($"Camouflager ShapeShift");
                if (pc == null || pc.Data.IsDead) return;
                foreach (PlayerControl target in PlayerControl.AllPlayerControls)
                    target.RpcShapeshift(PlayerControl.LocalPlayer, true);//誰がカモフラージュしたか分からなくさせるために、全員にアニメーションを再生
                return;
            }
            else
            {
                Logger.info($"Camouflager Revert ShapeShift");
                if (pc == null || pc.Data.IsDead) return;
                foreach (PlayerControl target in PlayerControl.AllPlayerControls)
                    target.RpcRevertShapeshift(true);
                return;
            }
        }
    }
}