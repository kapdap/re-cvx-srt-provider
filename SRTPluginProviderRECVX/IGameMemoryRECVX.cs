using SRTPluginProviderRECVX.Enumerations;
using SRTPluginProviderRECVX.Models;
using System.Collections.Generic;

namespace SRTPluginProviderRECVX
{
    public interface IGameMemoryRECVX
    {
        public GameVersion Version { get; set; }
        public TimeEntry IGT { get; set; }
        public PlayerEntry Player { get; set; }
        public List<EnemyEntry> Enemy { get; set; }
        public RoomEntry Room { get; set; }
        public DifficultyEnumeration Difficulty { get; set; }
        public string DifficultyName { get; }
    }
}