
namespace TownOfHost
{
    static class Jackal
    {
        public static int AliveJackalCount()
        {
            if (!CustomRoles.Jackal.IsEnable()) return 0;
            var count = 0;
            foreach (var pc in PlayerControl.AllPlayerControls) if (pc.Is(CustomRoles.Jackal) && !pc.Data.IsDead && pc.Data.Disconnected) count++;
            return count;
        }
    }
}