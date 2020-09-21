using SRTPluginProviderRECVX.Enumerations;
using SRTPluginProviderRECVX.Models;

namespace SRTPluginProviderRECVX
{
    public class GameMemoryRECVX : BaseNotifyModel, IGameMemoryRECVX
    {
        public EmulatorEntry Emulator { get; } = new EmulatorEntry();
        public GameVersion Version { get; } = new GameVersion();
        public TimeEntry IGT { get; } = new TimeEntry();
        public PlayerEntry Player { get; } = new PlayerEntry();
        private EnemyEntry[] _enemy = new EnemyEntry[8];
        public EnemyEntry[] Enemy
        {
            get
            {
                if (_enemy[0] == null)
                {
                    for (int i = 0; i < _enemy.Length; i++)
                        _enemy[i] = new EnemyEntry(i);
                    OnPropertyChanged();
                }

                return _enemy;
            }
        }
        public RoomEntry Room { get; } = new RoomEntry();
        public RankEntry Rank { get; } = new RankEntry();

        public DifficultyEnumeration _difficulty = DifficultyEnumeration.Normal;
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

        public string DifficultyName =>
            GetDifficultyName();

        private string GetDifficultyName()
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