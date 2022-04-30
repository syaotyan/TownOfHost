using Hazel;

namespace TownOfHost
{
    static class Jackal
    {
        public static int AliveJackalCount()
        {
            if (CustomRoles.Jackal.isEnable()) return 0;
            var count = 0;
            foreach (var pc in PlayerControl.AllPlayerControls) if (!pc.Data.IsDead && pc.isJackal() && pc.Data.Disconnected) count++;
            return count;
        }
    }
}