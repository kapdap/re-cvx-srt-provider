using SRTPluginProviderRECVX.Enumerations;
using SRTPluginProviderRECVX.Models;
using System;

namespace SRTPluginProviderRECVX
{
    public class GameMemoryRECVX : BaseNotifyModel, IGameMemoryRECVX
    {
        public ProcessEntry Process { get; } = new ProcessEntry();
        public GameVersion Version { get; } = new GameVersion();
        public TimeEntry IGT { get; } = new TimeEntry();
        public RoomEntry Room { get; } = new RoomEntry();
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

        public DifficultyEnumeration _difficulty = DifficultyEnumeration.Normal;
        public DifficultyEnumeration Difficulty
        {
            get => _difficulty;
            set
            {
                if (_difficulty != value)
                {
                    _difficulty = value;
                    _difficultyName = GetDifficultyName();

                    OnPropertyChanged();
                    OnPropertyChanged("DifficultyName");
                }
            }
        }

        public string _difficultyName;
        public string DifficultyName
        {
            get => _difficultyName != String.Empty ? _difficultyName : GetDifficultyName();
            set
            {
                if (_difficultyName != value)
                {
                    _difficultyName = value;
                    OnPropertyChanged();
                }
            }
        }

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