using SRTPluginProviderRECVX.Enumerations;
using SRTPluginProviderRECVX.Models;

namespace SRTPluginProviderRECVX
{
    public interface IGameMemoryRECVX
    {
        public string GameName { get; }

        public EmulatorEntry Emulator { get; }
        public GameVersion Version { get; }
        public TimeEntry IGT { get; }
        public PlayerEntry Player { get; }
        public EnemyEntry[] Enemy { get; }
        public RoomEntry Room { get; }
        public RankEntry Rank { get; }
        public DifficultyEnumeration Difficulty { get; set; }
        public string DifficultyName { get; }
    }
}