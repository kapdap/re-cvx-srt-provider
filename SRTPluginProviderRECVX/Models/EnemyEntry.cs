using SRTPluginProviderRECVX.Enumerations;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SRTPluginProviderRECVX.Models
{
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public class EnemyEntry : BaseNotifyModel, IEquatable<EnemyEntry>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                if (IsAlive)
                    return String.Format("[#{0}] {1} / {2} ({3:P1}) {4}", Index, CurrentHP, MaximumHP, Percentage, TypeName);
                if (IsEmpty)
                    return String.Format("[#{0}] EMPTY / EMPTY (0%)", Index);
                else
                    return String.Format("[#{0}] DEAD / DEAD (0%) {1}", Index, TypeName);
            }
        }

        public string DebugMessage =>
            $"{Slot}:{Damage}:{CurrentHP}:{MaximumHP}:{Convert.ToInt32(HasMaxHP)}:{Convert.ToInt32(IsEmpty)}:{Convert.ToInt32(IsAlive)}:{Action}:{Status}:{Model}:{Convert.ToInt32(Type)}";

        public string HealthMessage =>
            HasMaxHP ? $"{DisplayHP} {Percentage:P1}" : $"{DisplayHP}";

        public int Index { get; private set; }

        private bool _isEmpty = true;
        public bool IsEmpty
        {
            get => _isEmpty;
            set
            {
                if (_isEmpty != value)
                {
                    _isEmpty = value;
                    OnPropertyChanged();
                }
            }
        }

        private RoomEntry _room;
        public RoomEntry Room
        {
            get => _room;
            set
            {
                if (_room == null)
                {
                    _room = value;
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
                bool hasChanged = false;

                if (_currentHP != value)
                {
                    _currentHP = value;
                    OnPropertyChanged();
                    OnPropertyChanged("DisplayHP");
                    hasChanged = true;
                }

                if (!_hasMaxHP)
                {
                    _maximumHP = _currentHP; // Does not set HasMaxHP
                    OnPropertyChanged("MaximumHP");
                    hasChanged = true;
                }

                if (hasChanged)
                {
                    OnPropertyChanged("Percentage");
                    OnPropertyChanged("HealthMessage");
                    OnPropertyChanged("DebugMessage");
                }
            }
        }

        public int DisplayHP
            => Math.Max(CurrentHP, 0);

        private int _maximumHP;
        public int MaximumHP
        {
            get => _maximumHP;
            set
            {
                bool hasChanged = false;

                if (_maximumHP != value)
                {
                    _maximumHP = value;
                    OnPropertyChanged();
                    hasChanged = true;
                }

                if (!_hasMaxHP)
                {
                    _hasMaxHP = true;
                    OnPropertyChanged("HasMaxHP");
                    hasChanged = true;
                }

                if (hasChanged)
                {
                    OnPropertyChanged("Percentage");
                    OnPropertyChanged("HealthMessage");
                    OnPropertyChanged("DebugMessage");
                }
            }
        }

        private bool _hasMaxHP;
        public bool HasMaxHP
        {
            get => _hasMaxHP;
            set
            {
                if (_hasMaxHP != value)
                {
                    _hasMaxHP = true;
                    OnPropertyChanged("HasMaxHP");
                    OnPropertyChanged("HealthMessage");
                    OnPropertyChanged("DebugMessage");
                }
            }
        }

        private bool _isAlive;
        public bool IsAlive
        {
            get => _isAlive;
            set
            {
                if (_isAlive != value)
                {
                    _isAlive = value;
                    OnPropertyChanged();
                    OnPropertyChanged("Percentage");
                    OnPropertyChanged("HealthMessage");
                    OnPropertyChanged("DebugMessage");
                }
            }
        }

        public float Percentage
            => IsAlive && CurrentHP > 0 ? (float)DisplayHP / (float)MaximumHP : 0f;

        private int _damage;
        public int Damage
        {
            get => _damage;
            set
            {
                if (_damage != value)
                {
                    _damage = value;
                    OnPropertyChanged();
                    OnPropertyChanged("HealthMessage");
                    OnPropertyChanged("DebugMessage");
                }
            }
        }

        private int _slot;
        public int Slot
        {
            get => _slot;
            set
            {
                if (_slot != value)
                {
                    _slot = value;
                    OnPropertyChanged();
                    OnPropertyChanged("HealthMessage");
                    OnPropertyChanged("DebugMessage");
                }
            }
        }

        private byte _action;
        public byte Action
        {
            get => _action;
            set
            {
                if (_action != value)
                {
                    _action = value;
                    OnPropertyChanged();
                    OnPropertyChanged("Active");
                    OnPropertyChanged("HealthMessage");
                    OnPropertyChanged("DebugMessage");
                }
            }
        }

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
                    OnPropertyChanged("HealthMessage");
                    OnPropertyChanged("DebugMessage");
                }
            }
        }

        private byte _model;
        public byte Model
        {
            get => _model;
            set
            {
                if (_model != value)
                {
                    _model = value;
                    OnPropertyChanged();
                    OnPropertyChanged("HealthMessage");
                    OnPropertyChanged("DebugMessage");
                }
            }
        }

        public bool Active => 
            Action > 0 && Action < 4;

        private EnemyEnumeration _type = EnemyEnumeration.None;
        public EnemyEnumeration Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    _typeName = GetTypeName();
                    _isBoss = GetIsBoss();
                    _hasMaxHP = false;

                    OnPropertyChanged();
                    OnPropertyChanged("TypeName");
                    OnPropertyChanged("IsBoss");
                    OnPropertyChanged("MaximumHP");
                    OnPropertyChanged("HealthMessage");
                    OnPropertyChanged("DebugMessage");
                }
            }
        }

        private string _typeName;
        public string TypeName
        {
            get => !String.IsNullOrEmpty(_typeName) ? _typeName : GetTypeName();
            set
            {
                if (_typeName != value)
                {
                    _typeName = value;
                    OnPropertyChanged();
                    OnPropertyChanged("HealthMessage");
                    OnPropertyChanged("DebugMessage");
                }
            }
        }

        private bool _isBoss;
        public bool IsBoss
        {
            get => _isBoss;
            set
            {
                if (_isBoss != value)
                {
                    _isBoss = value;
                    OnPropertyChanged();
                    OnPropertyChanged("HealthMessage");
                    OnPropertyChanged("DebugMessage");
                }
            }
        }

        public EnemyEntry(int index) =>
            Index = index;

        public void Clear()
        {
            if (IsEmpty) return;

            _isAlive = false;
            _currentHP = 0;
            _maximumHP = 0;
            _hasMaxHP = false;
            _damage = 0;
            _slot = 0;
            _action = 0;
            _status = 0;
            _model = 0;
            _type = EnemyEnumeration.None;

            IsEmpty = true;

            OnPropertyChanged("IsAlive");
            OnPropertyChanged("CurrentHP");
            OnPropertyChanged("DisplayHP");
            OnPropertyChanged("MaximumHP");
            OnPropertyChanged("HasMaxHP");
            OnPropertyChanged("Percentage");
            OnPropertyChanged("Damage");
            OnPropertyChanged("Slot");
            OnPropertyChanged("Action");
            OnPropertyChanged("Status");
            OnPropertyChanged("Model");
            OnPropertyChanged("Active");
            OnPropertyChanged("Type");
            OnPropertyChanged("TypeName");
            OnPropertyChanged("IsBoss");
            OnPropertyChanged("HealthMessage");
            OnPropertyChanged("DebugMessage");

            OnPropertyChanged("ClearEntry");
        }

        public void SendUpdateEntryEvent() =>
            OnPropertyChanged("UpdateEntry");

        private string GetTypeName()
        {
            switch (Type)
            {
                case EnemyEnumeration.Zombie:
                    return "Zombie";
                case EnemyEnumeration.GlupWorm:
                    return "Glup Worm";
                case EnemyEnumeration.BlackWidow:
                    return "Black Widow";
                case EnemyEnumeration.ZombieDog:
                    return "Zombie Dog";
                case EnemyEnumeration.Hunter:
                    return "Hunter";
                case EnemyEnumeration.Moth:
                    return "Moth";
                case EnemyEnumeration.Bat:
                    return "Bat";
                case EnemyEnumeration.Bandersnatch:
                    return "Bandersnatch";
                case EnemyEnumeration.AlexiaAshford:
                    return "Alexia Ashford";
                case EnemyEnumeration.AlexiaAshfordB:
                    return "Alexia Ashford Second Stage";
                case EnemyEnumeration.AlexiaAshfordC:
                    return "Alexia Ashford Final Stage";
                case EnemyEnumeration.Nosferatu:
                    return "Nosferatu";
                case EnemyEnumeration.MutatedSteve:
                    return "Mutated Steve";
                case EnemyEnumeration.Tyrant:
                    return "Tyrant";
                case EnemyEnumeration.AlbinoidInfant:
                    return "Albinoid Infant";
                case EnemyEnumeration.AlbinoidAdult:
                    return "Albinoid Adult";
                case EnemyEnumeration.GiantBlackWidow:
                    return "Giant Black Widow";
                case EnemyEnumeration.AnatomistZombie:
                    return "Anatomist Zombie";
                case EnemyEnumeration.Tenticle:
                    return "Tenticle";
                case EnemyEnumeration.AlexiaBaby:
                    return "Alexia Baby";
                case EnemyEnumeration.Unknown:
                    return "Unknown";
                case EnemyEnumeration.None:
                default:
                    return "None";
            }
        }

        private bool GetIsBoss()
        {
            switch (_type)
            {
                case EnemyEnumeration.GlupWorm:
                case EnemyEnumeration.AlexiaAshford:
                case EnemyEnumeration.AlexiaAshfordB:
                case EnemyEnumeration.AlexiaAshfordC:
                case EnemyEnumeration.Nosferatu:
                case EnemyEnumeration.MutatedSteve:
                case EnemyEnumeration.Tyrant:
                case EnemyEnumeration.AlbinoidAdult:
                case EnemyEnumeration.GiantBlackWidow:
                    return true;
                default:
                    return false;
            }
        }

        public override bool Equals(object obj) => 
            Equals(obj as EnemyEntry);

        public bool Equals(EnemyEntry other) =>
            other != null &&
            Index == other.Index &&
            Slot == other.Slot &&
            EqualityComparer<RoomEntry>.Default.Equals(Room, other.Room);

        public override int GetHashCode()
        {
            int hashCode = -90915250;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + Slot.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<RoomEntry>.Default.GetHashCode(Room);
            return hashCode;
        }
    }
}