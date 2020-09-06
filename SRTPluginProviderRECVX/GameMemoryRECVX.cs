using SRTPluginProviderRECVX.Enumerations;
using SRTPluginProviderRECVX.Models;
using System.Collections.Generic;

namespace SRTPluginProviderRECVX
{
    public class GameMemoryRECVX : BaseNotifyModel, IGameMemoryRECVX
    {
        public GameVersion _version = new GameVersion();
        public GameVersion Version
        {
            get => _version;
            set
            {
                if (_version.Code != value.Code)
                {
                    _version = value;
                    OnPropertyChanged();
                }
            }
        }

        public TimeEntry IGT { get; set; } = new TimeEntry();
        public RoomEntry Room { get; set; } = new RoomEntry();
        public PlayerEntry Player { get; set; } = new PlayerEntry();

        public List<EnemyEntry> _enemy = new List<EnemyEntry>();
        public List<EnemyEntry> Enemy
        {
            get => _enemy;
            set
            {
                _enemy = value;
                OnPropertyChanged();
            }
        }

        public DifficultyEnumeration _difficulty;
        public DifficultyEnumeration Difficulty
        {
            get => _difficulty;
            set
            {
                if (_difficulty != value)
                {
                    _difficulty = value;
                    OnPropertyChanged();
                    OnPropertyChanged("DifficultyName");
                }
            }
        }

        public string DifficultyName
        {
            get
            {
                switch (Difficulty)
                {
                    case DifficultyEnumeration.Easy:
                        return "Easy";
                    case DifficultyEnumeration.VeryEasy:
                        return "Very Easy";
                    default:
                        return "Normal";
                }
            }
        }
    }
}