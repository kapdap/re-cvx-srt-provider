using SRTPluginProviderRECVX.Enumerations;
using System;
using System.Diagnostics;

namespace SRTPluginProviderRECVX.Models
{
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public class EnemyEntry
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                if (IsAlive)
                    return String.Format("{0} / {1} ({2:P1})", CurrentHP, MaximumHP, Percentage);
                else
                    return "DEAD / DEAD (0%)";
            }
        }

        public string DebugMessage => 
            $"{Slot}:{Damage}:{CurrentHP}:{MaximumHP}:{Convert.ToInt32(HasMaxHP)}:{Convert.ToInt32(IsAlive)}:{Action}:{Status}:{Model}:{Convert.ToInt32(Type)}";

        public string HealthMessage => 
            HasMaxHP ? $"{DisplayHP} {Percentage:P1}" : $"{DisplayHP}";

        private int _currentHP;
        public int CurrentHP
        {
            get => _currentHP;
            set
            {
                _currentHP = value;

                if (_maximumHP == 0)
                    _maximumHP = _currentHP; // Does not set HasMaxHP
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
                _maximumHP = value;
                HasMaxHP = true;
            }
        }

        public bool HasMaxHP { get; private set; }
        public bool IsAlive { get; set; }
        public float Percentage
            => (IsAlive && DisplayHP > 0) ? (float)DisplayHP / (float)MaximumHP : 0f;

        public int Damage { get; set; }

        public int Slot { get; set; }
        public byte Action { get; set; }
        public byte Status { get; set; }
        public byte Model { get; set; }
        public bool Active => Action > 0 && Action < 4;

        public EnemyEnumeration Type { get; set; }

        public string TypeName
        {
            get
            {
                return String.Empty;
            }
        }

        public EnemyEntry(EnemyEnumeration type) =>
            Type = type;
    }
}