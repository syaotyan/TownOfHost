using Hazel;

namespace TownOfHost
{
    static class Jackal
    {
        public static int AliveJackalCount()
        {
            if (!CustomRoles.Jackal.isEnable()) return 0;
            var count = 0;
            foreach (var pc in PlayerControl.AllPlayerControls) if (pc.isJackal() && !pc.Data.IsDead && pc.Data.Disconnected) count++;
            return count;
        }
    }
}