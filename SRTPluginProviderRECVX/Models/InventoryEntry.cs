using SRTPluginProviderRECVX.Enumerations;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SRTPluginProviderRECVX.Models
{
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public class InventoryEntry : BaseNotifyModel, IEquatable<InventoryEntry>
    {
        /// <summary>
        /// Debugger display message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                if (!IsEmpty)
                    return string.Format("[#{0}] Slot {1} Item {2} Quantity {3} Infinite {4}", Index, Slot, Id, Quantity, IsInfinite);
                else
                    return string.Format("[#{0}] Empty Slot", Index);
            }
        }

        public int Index { get; private set; }

        private int _slot;
        public int Slot
        {
            get => _slot;
            private set => SetField(ref _slot, value);
        }

        private int _slotSize;
        public int SlotSize
        {
            get => _slotSize;
            private set => SetField(ref _slotSize, value);
        }

        private int _slotRow;
        public int SlotRow
        {
            get => _slotRow;
            private set => SetField(ref _slotRow, value);
        }

        private int _slotColumn;
        public int SlotColumn
        {
            get => _slotColumn;
            private set => SetField(ref _slotColumn, value);
        }

        private byte[] _data = new byte[4];
        public byte[] Data
        {
            get => _data != null ? _data : new byte[4];
            private set
            {
                if (value == null || value.Length < 4)
                    value = new byte[4];

                for (int i = 0; i < _data.Length; ++i)
                    if (_data[i] != value[i])
                    {
                        _data[i] = value[i];
                        OnPropertyChanged();
                        break;
                    }
            }
        }

        private byte _id;
        public byte Id
        {
            get => _id;
            private set => SetField(ref _id, value);
        }

        private ItemEnumeration _type = ItemEnumeration.None;
        public ItemEnumeration Type
        {
            get => _type;
            private set => SetField(ref _type, value);
        }

        private string _name;
        public string Name
        {
            get
            {
                if (String.IsNullOrEmpty(_name))
                    SetField(ref _name, GetItemName());
                return _name;
            }
            private set => SetField(ref _name, value);
        }

        private ItemStatusEnumeration _ammoType = ItemStatusEnumeration.Normal;
        public ItemStatusEnumeration AmmoType
        {
            get => _ammoType;
            private set => SetField(ref _ammoType, value);
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            private set => SetField(ref _quantity, value);
        }

        private bool _hasQuantity;
        public bool HasQuantity
        {
            get => _hasQuantity;
            private set => SetField(ref _hasQuantity, value);
        }

        private bool _isInfinite;
        public bool IsInfinite
        {
            get => _isInfinite;
            private set => SetField(ref _isInfinite, value);
        }

        private bool _isFlame;
        public bool IsFlame
        {
            get => _isFlame;
            private set => SetField(ref _isFlame, value);
        }

        private bool _isAcid;
        public bool IsAcid
        {
            get => _isAcid;
            private set => SetField(ref _isAcid, value);
        }

        private bool _isBOW;
        public bool IsBOW
        {
            get => _isBOW;
            private set => SetField(ref _isBOW, value);
        }

        private bool _isEquipped;
        public bool IsEquipped
        {
            get => _isEquipped;
            private set => SetField(ref _isEquipped, value);
        }

        private bool _isEmpty = true;
        public bool IsEmpty
        {
            get => _isEmpty;
            private set => SetField(ref _isEmpty, value);
        }

        public InventoryEntry(int index) =>
            Index = index;

        public void Update(byte[] data, int slot = 0, bool isEquipped = false)
        {
            if (data == null || data.Length < 4)
                data = new byte[4];

            if (data.Equals(Data))
                return;

            Data = data;

            Id = Data[2];

            Type = GetItemType();
            IsEmpty = Type == ItemEnumeration.None;

            Quantity = BitConverter.ToInt16(Data, 0);
            HasQuantity = GetHasQuantity();

            Slot = slot;
            SlotRow = Slot / 2;
            SlotColumn = Slot % 2;
            SlotSize = GetSlotSize();

            IsEquipped = isEquipped;

            IsInfinite = (Data[3] & (byte)ItemStatusEnumeration.Infinite) != 0;
            IsFlame = (Data[3] & (byte)ItemStatusEnumeration.Flame) != 0 || Type == ItemEnumeration.FlameRounds || Type == ItemEnumeration.GunPowderArrow;
            IsAcid = (Data[3] & (byte)ItemStatusEnumeration.Acid) != 0 || Type == ItemEnumeration.AcidRounds;
            IsBOW = (Data[3] & (byte)ItemStatusEnumeration.BOW) != 0 || Type == ItemEnumeration.BOWGasRounds;

            AmmoType = GetAmmoType();
            Name = GetItemName();

            SendUpdateEntryEvent();
        }

        private string GetItemName()
        {
            switch(Type)
            {
                case ItemEnumeration.None:
                    return "None";
                case ItemEnumeration.RocketLauncher:
                    return "Rocket Launcher (Bazooka)";
                case ItemEnumeration.AssaultRifle:
                    return "Assault Rifle (AK-47)";
                case ItemEnumeration.SniperRifle:
                    return "Sniper Rifle (MR7)";
                case ItemEnumeration.Shotgun:
                    return "Shotgun (SPAS 12)";
                case ItemEnumeration.HandgunGlock17:
                    return "Handgun (Glock 17)";
                case ItemEnumeration.GrenadeLauncher:
                    return "Grenade Launcher (M79)";
                case ItemEnumeration.BowGun:
                    return "Bow Gun";
                case ItemEnumeration.CombatKnife:
                    return "Combat Knife";
                case ItemEnumeration.Handgun:
                    return "Handgun (M93R)";
                case ItemEnumeration.CustomHandgun:
                    return "Custom Handgun (M93R Burst)";
                case ItemEnumeration.LinearLauncher:
                    return "Linear Launcher";
                case ItemEnumeration.HandgunBullets:
                    return "Handgun Bullets";
                case ItemEnumeration.MagnumBullets:
                    return "Magnum Bullets";
                case ItemEnumeration.ShotgunShells:
                    return "Shotgun Shells";
                case ItemEnumeration.GrenadeRounds:
                    return "Grenade Rounds";
                case ItemEnumeration.AcidRounds:
                    return "Acid Rounds";
                case ItemEnumeration.FlameRounds:
                    return "Flame Rounds";
                case ItemEnumeration.BowGunArrows:
                    return "Bow Gun Arrows";
                case ItemEnumeration.M93RPart:
                    return "M93R Part";
                case ItemEnumeration.FAidSpray:
                    return "F. Aid Spray";
                case ItemEnumeration.GreenHerb:
                    return "Green Herb";
                case ItemEnumeration.RedHerb:
                    return "Red Herb";
                case ItemEnumeration.BlueHerb:
                    return "Blue Herb";
                case ItemEnumeration.MixedHerb2Green:
                    return "Mixed Herb (2 Green)";
                case ItemEnumeration.MixedHerbRedGreen:
                    return "Mixed Herb (Red & Green)";
                case ItemEnumeration.MixedHerbBlueGreen:
                    return "Mixed Herb (Blue & Green)";
                case ItemEnumeration.MixedHerb2GreenBlue:
                    return "Mixed Herb (2 Green & Blue)";
                case ItemEnumeration.MixedHerb3Green:
                    return "Mixed Herb (3 Green)";
                case ItemEnumeration.MixedHerbGreenBlueRed:
                    return "Mixed Herb (Green, Blue & Red)";
                case ItemEnumeration.MagnumBulletsInsideCase:
                    return "Magnum Bullets (Inside Case)";
                case ItemEnumeration.InkRibbon:
                    return "Ink Ribbon";
                case ItemEnumeration.Magnum:
                    return "Magnum (Colt Python)";
                case ItemEnumeration.GoldLugers:
                    return "Gold Lugers";
                case ItemEnumeration.SubMachineGun:
                    return "Sub Machine Gun (Ingram)";
                case ItemEnumeration.BowGunPowder:
                    return "Bow Gun Powder";
                case ItemEnumeration.GunPowderArrow:
                    return "Gun Powder Arrow";
                case ItemEnumeration.BOWGasRounds:
                    return "BOW Gas Rounds";
                case ItemEnumeration.MGunBullets:
                    return "M. Gun Bullets (Ingram)";
                case ItemEnumeration.GasMask:
                    return "Gas Mask";
                case ItemEnumeration.RifleBullets:
                    return "Rifle Bullets (MR7)";
                case ItemEnumeration.DuraluminCaseUnused:
                    return "Duralumin Case (Unused)";
                case ItemEnumeration.ARifleBullets:
                    return "A. Rifle Bullets";
                case ItemEnumeration.AlexandersPierce:
                    return "Alexander's Pierce";
                case ItemEnumeration.AlexandersJewel:
                    return "Alexander's Jewel";
                case ItemEnumeration.AlfredsRing:
                    return "Alfred's Ring";
                case ItemEnumeration.AlfredsJewel:
                    return "Alfred's Jewel";
                case ItemEnumeration.PrisonersDiary:
                    return "Prisoner's Diary";
                case ItemEnumeration.DirectorsMemo:
                    return "Director's Memo";
                case ItemEnumeration.Instructions:
                    return "Instructions";
                case ItemEnumeration.Lockpick:
                    return "Lockpick";
                case ItemEnumeration.GlassEye:
                    return "Glass Eye";
                case ItemEnumeration.PianoRoll:
                    return "Piano Roll";
                case ItemEnumeration.SteeringWheel:
                    return "Steering Wheel";
                case ItemEnumeration.CraneKey:
                    return "Crane Key";
                case ItemEnumeration.Lighter:
                    return "Lighter";
                case ItemEnumeration.EaglePlate:
                    return "Eagle Plate";
                case ItemEnumeration.SidePack:
                    return "Side Pack";
                case ItemEnumeration.MapRoll:
                    return "Map (Roll)";
                case ItemEnumeration.HawkEmblem:
                    return "Hawk Emblem";
                case ItemEnumeration.QueenAntObject:
                    return "Queen Ant Object";
                case ItemEnumeration.KingAntObject:
                    return "King Ant Object";
                case ItemEnumeration.BiohazardCard:
                    return "Biohazard Card";
                case ItemEnumeration.DuraluminCaseM93RParts:
                    return "Duralumin Case (M93R Parts)";
                case ItemEnumeration.Detonator:
                    return "Detonator";
                case ItemEnumeration.ControlLever:
                    return "Control Lever";
                case ItemEnumeration.GoldDragonfly:
                    return "Gold Dragonfly";
                case ItemEnumeration.SilverKey:
                    return "Silver Key";
                case ItemEnumeration.GoldKey:
                    return "Gold Key";
                case ItemEnumeration.ArmyProof:
                    return "Army Proof";
                case ItemEnumeration.NavyProof:
                    return "Navy Proof";
                case ItemEnumeration.AirForceProof:
                    return "Air Force Proof";
                case ItemEnumeration.KeyWithTag:
                    return "Key With Tag";
                case ItemEnumeration.IDCard:
                    return "ID Card";
                case ItemEnumeration.Map:
                    return "Map";
                case ItemEnumeration.AirportKey:
                    return "Airport Key";
                case ItemEnumeration.EmblemCard:
                    return "Emblem Card";
                case ItemEnumeration.SkeletonPicture:
                    return "Skeleton Picture";
                case ItemEnumeration.MusicBoxPlate:
                    return "Music Box Plate";
                case ItemEnumeration.GoldDragonflyNoWings:
                    return "Gold Dragonfly (No Wings)";
                case ItemEnumeration.Album:
                    return "Album";
                case ItemEnumeration.Halberd:
                    return "Halberd";
                case ItemEnumeration.Extinguisher:
                    return "Extinguisher";
                case ItemEnumeration.Briefcase:
                    return "Briefcase";
                case ItemEnumeration.PadlockKey:
                    return "Padlock Key";
                case ItemEnumeration.TG01:
                    return "TG-01";
                case ItemEnumeration.SpAlloyEmblem:
                    return "Sp. Alloy Emblem";
                case ItemEnumeration.ValveHandle:
                    return "Valve Handle";
                case ItemEnumeration.OctaValveHandle:
                    return "Octa Valve Handle";
                case ItemEnumeration.MachineRoomKey:
                    return "Machine Room Key";
                case ItemEnumeration.MiningRoomKey:
                    return "Mining Room Key";
                case ItemEnumeration.BarCodeSticker:
                    return "Bar Code Sticker";
                case ItemEnumeration.SterileRoomKey:
                    return "Sterile Room Key";
                case ItemEnumeration.DoorKnob:
                    return "Door Knob";
                case ItemEnumeration.BatteryPack:
                    return "Battery Pack";
                case ItemEnumeration.HemostaticWire:
                    return "Hemostatic (Wire)";
                case ItemEnumeration.TurnTableKey:
                    return "Turn Table Key";
                case ItemEnumeration.ChemStorageKey:
                    return "Chem. Storage Key";
                case ItemEnumeration.ClementAlpha:
                    return "Clement Alpha";
                case ItemEnumeration.ClementSigma:
                    return "Clement Sigma";
                case ItemEnumeration.TankObject:
                    return "Tank Object";
                case ItemEnumeration.SpAlloyEmblemUnused:
                    return "Sp. Alloy Emblem (Unused)";
                case ItemEnumeration.AlfredsMemo:
                    return "Alfred's Memo";
                case ItemEnumeration.RustedSword:
                    return "Rusted Sword";
                case ItemEnumeration.Hemostatic:
                    return "Hemostatic";
                case ItemEnumeration.SecurityCard:
                    return "Security Card";
                case ItemEnumeration.SecurityFile:
                    return "Security File";
                case ItemEnumeration.AlexiasChoker:
                    return "Alexia's Choker";
                case ItemEnumeration.AlexiasJewel:
                    return "Alexia's Jewel";
                case ItemEnumeration.QueenAntRelief:
                    return "Queen Ant Relief";
                case ItemEnumeration.KingAntRelief:
                    return "King Ant Relief";
                case ItemEnumeration.RedJewel:
                    return "Red Jewel";
                case ItemEnumeration.BlueJewel:
                    return "Blue Jewel";
                case ItemEnumeration.Socket:
                    return "Socket";
                case ItemEnumeration.SqValveHandle:
                    return "Sq. Valve Handle";
                case ItemEnumeration.Serum:
                    return "Serum";
                case ItemEnumeration.EarthenwareVase:
                    return "Earthenware Vase";
                case ItemEnumeration.PaperWeight:
                    return "Paper Weight";
                case ItemEnumeration.SilverDragonflyNoWings:
                    return "Silver Dragonfly (No Wings)";
                case ItemEnumeration.SilverDragonfly:
                    return "Silver Dragonfly";
                case ItemEnumeration.WingObject:
                    return "Wing Object";
                case ItemEnumeration.Crystal:
                    return "Crystal";
                case ItemEnumeration.GoldDragonfly1Wing:
                    return "Gold Dragonfly (1 Wing)";
                case ItemEnumeration.GoldDragonfly2Wings:
                    return "Gold Dragonfly (2 Wings)";
                case ItemEnumeration.GoldDragonfly3Wings:
                    return "Gold Dragonfly (3 Wings)";
                case ItemEnumeration.File:
                    return "File";
                case ItemEnumeration.PlantPot:
                    return "Plant Pot";
                case ItemEnumeration.PictureB:
                    return "Picture B";
                case ItemEnumeration.DuraluminCaseBowGunPowder:
                    return "Duralumin Case (Bow Gun Powder)";
                case ItemEnumeration.DuraluminCaseMagnumRounds:
                    return "Duralumin Case (Magnum Rounds)";
                case ItemEnumeration.BowGunPowderUnused:
                    return "Bow Gun Powder (Unused)";
                case ItemEnumeration.EnhancedHandgun:
                    return "Enhanced Handgun (Modified Glock 17)";
                case ItemEnumeration.Memo:
                    return "Memo";
                case ItemEnumeration.BoardClip:
                    return "Board Clip";
                case ItemEnumeration.Card:
                    return "Card";
                case ItemEnumeration.NewspaperClip:
                    return "Newspaper Clip";
                case ItemEnumeration.LugerReplica:
                    return "Luger Replica";
                case ItemEnumeration.QueenAntReliefComplete:
                    return "Queen Ant Relief (Complete)";
                case ItemEnumeration.FamilyPicture:
                    return "Family Picture";
                case ItemEnumeration.FileFolders:
                    return "File Folders";
                case ItemEnumeration.RemoteController:
                    return "Remote Controller";
                case ItemEnumeration.QuestionA:
                    return "? A";
                case ItemEnumeration.M1P:
                    return "M-100P";
                case ItemEnumeration.CalicoBullets:
                    return "Calico Bullets (M-100P)";
                case ItemEnumeration.ClementMixture:
                    return "Clement Mixture";
                case ItemEnumeration.PlayingManual:
                    return "Playing Manual";
                case ItemEnumeration.QuestionB:
                    return "? B";
                case ItemEnumeration.QuestionC:
                    return "? C";
                case ItemEnumeration.QuestionD:
                    return "? D";
                case ItemEnumeration.EmptyExtinguisher:
                    return "Empty Extinguisher";
                case ItemEnumeration.SquareSocket:
                    return "Square Socket";
                case ItemEnumeration.QuestionE:
                    return "? E";
                case ItemEnumeration.CrestKeyS:
                    return "Crest Key S";
                case ItemEnumeration.CrestKeyG:
                    return "Crest Key G";
                case ItemEnumeration.Unknown:
                default:
                    return "Unknown";
            }
        }

        private bool GetHasQuantity()
        {
            switch (Type)
            {
                case ItemEnumeration.RocketLauncher:
                case ItemEnumeration.AssaultRifle:
                case ItemEnumeration.SniperRifle:
                case ItemEnumeration.Shotgun:
                case ItemEnumeration.HandgunGlock17:
                case ItemEnumeration.GrenadeLauncher:
                case ItemEnumeration.BowGun:
                case ItemEnumeration.Handgun:
                case ItemEnumeration.CustomHandgun:
                case ItemEnumeration.LinearLauncher:
                case ItemEnumeration.HandgunBullets:
                case ItemEnumeration.MagnumBullets:
                case ItemEnumeration.MagnumBulletsInsideCase:
                case ItemEnumeration.ShotgunShells:
                case ItemEnumeration.GrenadeRounds:
                case ItemEnumeration.AcidRounds:
                case ItemEnumeration.FlameRounds:
                case ItemEnumeration.BowGunArrows:
                case ItemEnumeration.InkRibbon:
                case ItemEnumeration.Magnum:
                case ItemEnumeration.GoldLugers:
                case ItemEnumeration.SubMachineGun:
                case ItemEnumeration.BowGunPowder:
                case ItemEnumeration.GunPowderArrow:
                case ItemEnumeration.BOWGasRounds:
                case ItemEnumeration.MGunBullets:
                case ItemEnumeration.RifleBullets:
                case ItemEnumeration.ARifleBullets:
                case ItemEnumeration.CalicoBullets:
                case ItemEnumeration.WingObject:
                case ItemEnumeration.M1P:
                case ItemEnumeration.BowGunPowderUnused:
                case ItemEnumeration.EnhancedHandgun:
                case ItemEnumeration.CrestKeyS:
                case ItemEnumeration.CrestKeyG:
                    return true;
                default:
                    return false;
            }
        }

        private int GetSlotSize()
        {
            switch (Type)
            {
                case ItemEnumeration.RocketLauncher:
                case ItemEnumeration.AssaultRifle:
                case ItemEnumeration.SniperRifle:
                case ItemEnumeration.GoldLugers:
                case ItemEnumeration.SubMachineGun:
                case ItemEnumeration.M1P:
                    return 2;
                default:
                    return 1;
            }
        }

        private ItemEnumeration GetItemType()
        {
            if (Enum.IsDefined(typeof(ItemEnumeration), (ItemEnumeration)Id))
                return (ItemEnumeration)Id;
            return ItemEnumeration.Unknown;
        }

        private ItemStatusEnumeration GetAmmoType()
        {
            if (IsFlame)
                return ItemStatusEnumeration.Flame;
            else if (IsAcid)
                return ItemStatusEnumeration.Acid;
            else if (IsBOW)
                return ItemStatusEnumeration.BOW;
            return ItemStatusEnumeration.Normal;
        }

        public override bool Equals(object obj) => 
            Equals(obj as InventoryEntry);

        public bool Equals(InventoryEntry other) => 
            other != null &&
            Index == other.Index &&
            EqualityComparer<byte[]>.Default.Equals(Data, other.Data);

        public override int GetHashCode()
        {
            int hashCode = -2005450498;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<byte[]>.Default.GetHashCode(Data);
            return hashCode;
        }
    }
}