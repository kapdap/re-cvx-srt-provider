using SRTPluginProviderRECVX.Enumerations;
using System;
using System.Diagnostics;

namespace SRTPluginProviderRECVX.Models
{
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public class PlayerEntry : BaseNotifyModel
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                if (IsAlive)
                    return String.Format("{0} {1} / {2} ({3:P1})", CharacterName, CurrentHP, MaximumHP, Percentage);
                else
                    return String.Format("{0} DEAD / DEAD (0%)", CharacterName);
            }
        }

        private CharacterEnumeration _character;
        public CharacterEnumeration Character
        {
            get => _character;
            set
            {
                if (_character != value)
                {
                    _character = value;
                    OnPropertyChanged();
                    OnPropertyChanged("CharacterName");
                }
            }
        }

        private InventoryEntry[] _inventory;
        public InventoryEntry[] Inventory
        {
            get => _inventory;
            set
            {
                if (_inventory != value)
                {
                    _inventory = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _maximumHP;
        public int MaximumHP
        {
            get => _maximumHP;
            set
            {
                if (_maximumHP != value)
                {
                    _maximumHP = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _currentHP;
        public int CurrentHP
        {
            get => _currentHP;
            set
            {
                if (_currentHP != value)
                {
                    _currentHP = value;
                    OnPropertyChanged();
                    OnPropertyChanged("DisplayHP");
                    OnPropertyChanged("Percentage");
                }
            }
        }

        public int DisplayHP
            => Math.Max(CurrentHP, 0);

        public bool IsAlive
            => CurrentHP > 0;

        public float Percentage
            => IsAlive ? (float)DisplayHP / (float)MaximumHP : 0f;

        private byte _status;
        public byte Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                    OnPropertyChanged("Poison");
                    OnPropertyChanged("Gassed");
                    OnPropertyChanged("StatusName");
                }
            }
        }

        public bool Poison
            => (Status & 0x08) != 0;

        public bool Gassed
            => (Status & 0x20) != 0;

        // S rank requirments
        private int _retry;
        public int Retry
        {
            get => _retry;
            set
            {
                if (_retry != value)
                {
                    _retry = value;
                    OnPropertyChanged();
                    UpdateScore();
                }
            }
        }

        private int _saves;
        public int Saves
        {
            get => _saves;
            set
            {
                if (_saves != value)
                {
                    _saves = value;
                    OnPropertyChanged();
                    UpdateScore();
                }
            }
        }

        private int _health;
        public int Health
        {
            get => _health;
            set
            {
                if (_health != value)
                {
                    _health = value;
                    OnPropertyChanged();
                    UpdateScore();
                }
            }
        }

        // ToDo
        private int _map;
        public int Map
        {
            get => _map;
            set
            {
                if (_map != value)
                {
                    _map = value;
                    OnPropertyChanged();
                    UpdateScore();
                }
            }
        }

        // ToDo
        private bool _steve;
        public bool Steve
        {
            get => _steve;
            set
            {
                if (_steve != value)
                {
                    _steve = value;
                    OnPropertyChanged();
                    UpdateScore();
                }
            }
        }

        // ToDo
        private bool _rodorigo;
        public bool Rodorigo
        {
            get => _rodorigo;
            set
            {
                if (_rodorigo != value)
                {
                    _rodorigo = value;
                    OnPropertyChanged();
                    UpdateScore();
                }
            }
        }

        // ToDo
        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                if (_score != value)
                {
                    _score = value;
                    OnPropertyChanged();
                    OnPropertyChanged("ScoreName");
                }
            }
        }

        public string CharacterName
        {
            get
            {
                switch (Character)
                {
                    case CharacterEnumeration.Chris:
                        return "Chris Redfield";
                    case CharacterEnumeration.Steve:
                        return "Steve Burnside";
                    case CharacterEnumeration.Wesker:
                        return "Albert Wesker";
                    default:
                        return "Claire Redfield";
                }
            }
        }

        public string StatusName
        {
            get
            {
                if (Gassed)
                    return "Gassed";
                else if (Poison)
                    return "Poison";
                else
                    return "Normal";
            }
        }

        // ToDo
        public string ScoreName
        {
            get
            {
                return String.Empty;
            }
        }

        // ToDo
        public void UpdateScore()
        {
            Score = 0;
        }
    }
}