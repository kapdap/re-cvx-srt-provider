using SRTPluginProviderRECVX.Enumerations;
using SRTPluginProviderRECVX.Models;

namespace SRTPluginProviderRECVX
{
    public interface IGameMemoryRECVX
    {
        public GameVersion Version { get; }
        public TimeEntry IGT { get; }
        public PlayerEntry Player { get; }
        public EnemyEntry[] Enemy { get; }
        public RoomEntry Room { get; }
        public DifficultyEnumeration Difficulty { get; set; }
        public string DifficultyName { get; }
    }
}