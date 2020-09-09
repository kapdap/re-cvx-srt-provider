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
                    OnPropertyChanged("CharacterFirstName");
                }
            }
        }

        private InventoryEntry _equipment;
        public InventoryEntry Equipment
        {
            get => _equipment;
            set
            {
                if (_equipment != value)
                {
                    _equipment = value;
                    OnPropertyChanged();
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
            => CurrentHP >= 0;

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
        private bool _rodrigo;
        public bool Rodrigo
        {
            get => _rodrigo;
            set
            {
                if (_rodrigo != value)
                {
                    _rodrigo = value;
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
                    OnPropertyChanged("Rank");
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

        public string CharacterFirstName
        {
            get
            {
                switch (Character)
                {
                    case CharacterEnumeration.Chris:
                        return "Chris";
                    case CharacterEnumeration.Steve:
                        return "Steve";
                    case CharacterEnumeration.Wesker:
                        return "Albert";
                    default:
                        return "Claire";
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
        public string Rank
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