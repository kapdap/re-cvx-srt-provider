using SRTPluginProviderRECVX.Enumerations;
using System;
using System.Diagnostics;

namespace SRTPluginProviderRECVX.Models
{
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public class PlayerEntry : BaseNotifyModel
    {
        public enum PlayerState
        {
            Dead,
            Fine,
            CautionYellow,
            CautionOrange,
            Danger,
            Gassed,
            Poisoned
        }

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

        public string HealthMessage =>
            $"{DisplayHP} ({MaximumHP})";

        private CharacterEnumeration _character = CharacterEnumeration.Claire;
        public CharacterEnumeration Character
        {
            get => _character;
            set => SetField(ref _character, value, "Character", "CharacterName", "CharacterFirstName");
        }

        public InventoryEntry Equipment { get; } = new InventoryEntry(0);

        private InventoryEntry[] _inventory = new InventoryEntry[11];
        public InventoryEntry[] Inventory
        {
            get
            {
                if (_inventory[0] == null)
                {
                    for (int i = 0; i < _inventory.Length; i++)
                        _inventory[i] = new InventoryEntry(i);
                    OnPropertyChanged();
                }

                return _inventory;
            }
        }

        private int _maximumHP;
        public int MaximumHP
        {
            get => _maximumHP;
            set => SetField(ref _maximumHP, value, "MaximumHP", "MaxHP", "Percentage", "HealthMessage");
        }
        
        public int MaxHP { get => _maximumHP; }

        private int _currentHP;
        public int CurrentHP
        {
            get => _currentHP;
            set => SetField(ref _currentHP, value, 
                "CurrentHP", 
                "HealthMessage", 
                "IsAlive",
                "IsFine",
                "IsCautionYellow",
                "IsCautionOrange",
                "IsDanger",
                "DisplayHP",
                "Percentage",
                "HealthState",
                "StatusName",
                "StatusMessage");
        }

        public int DisplayHP
            => Math.Max(CurrentHP, 0);

        public float Percentage
            => IsAlive ? (float)DisplayHP / MaximumHP : 0f;

        private byte _status;
        public byte Status
        {
            get => _status;
            set => SetField(ref _status, value, "Status", "IsPoison", "IsGassed", "HealthState", "CurrentHealthState", "StatusName", "StatusMessage");
        }

        public bool IsPoison
            => (Status & 0x08) != 0;

        public bool IsGassed
            => (Status & 0x20) != 0;

        public bool IsAlive
            => CurrentHP >= 0;

		// Deprecated
        public bool IsFine
            => CurrentHP >= 120;

		// Deprecated
        public bool IsCautionYellow
            => CurrentHP < 120 && CurrentHP >= 60;

		// Deprecated
        public bool IsCautionOrange
            => CurrentHP < 60 && CurrentHP >= 30;

		// Deprecated
        public bool IsDanger
            => CurrentHP < 30;

        public PlayerState HealthState
        {
            get =>
                !IsAlive ? PlayerState.Dead :
                IsGassed ? PlayerState.Gassed :
                IsPoison ? PlayerState.Poisoned :
                Percentage >= 0.60f ? PlayerState.Fine :
                Percentage >= 0.30f ? PlayerState.CautionYellow :
                Percentage >= 0.15f ? PlayerState.CautionOrange :
                PlayerState.Danger;
        }
        
        public string CurrentHealthState => HealthState.ToString();

        // S rank requirments
        private int _retry;
        public int Retry
        {
            get => _retry;
            set => SetField(ref _retry, value);
        }

        private int _saves;
        public int Saves
        {
            get => _saves;
            set => SetField(ref _saves, value);
        }

        private int _fas;
        public int FAS
        {
            get => _fas;
            set => SetField(ref _fas, value);
        }

        public int[] Map { get; } = new int[3];

        private int _steve;
        public int Steve
        {
            get => _steve;
            set => SetField(ref _steve, value);
        }

        private int _rodrigo;
        public int Rodrigo
        {
            get => _rodrigo;
            set => SetField(ref _rodrigo, value);
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
                return HealthState.ToString();
            }
        }

        public string StatusMessage
        {
            get
            {
                return StatusName.StartsWith("Caution") ? "Caution" : StatusName;
            }
        }
    }
}
