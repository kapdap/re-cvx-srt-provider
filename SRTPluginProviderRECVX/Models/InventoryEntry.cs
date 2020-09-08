using SRTPluginProviderRECVX.Enumerations;
using System;
using System.Diagnostics;

namespace SRTPluginProviderRECVX.Models
{
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public class InventoryEntry
    {
        /// <summary>
        /// Debugger display message.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                if (!IsEmptySlot)
                    return string.Format("[#{0}] Item {1} Quantity {2} Infinite {3}", Slot, ItemID, Quantity, IsInfinite);
                else
                    return string.Format("[#{0}] Empty Slot", Slot);
            }
        }

        public int Slot { get; private set; }
        public byte[] Data { get; private set; }

        public ItemPositionEnumeration Position { get; private set; }

        public ItemEnumeration ItemID { get; private set; }
        public int Quantity { get; private set; }

        public bool IsInfinite { get; private set; }
        public bool IsFlame { get; private set; }
        public bool IsAcid { get; private set; }
        public bool IsBOW { get; private set; }

        public bool IsEquipped { get; private set; }
        public bool IsEmptySlot => ItemID == ItemEnumeration.None;

        public InventoryEntry(int slot) =>
            Slot = slot;

        public InventoryEntry(int slot, byte[] data, bool isEquipped = false)
        {
            Slot = slot;
            Data = data;

            Position = (ItemPositionEnumeration)Slot;

            if (Data.Length < 4)
                return;

            Quantity = BitConverter.ToInt16(Data, 0);
            ItemID = (ItemEnumeration)Data[2];

            IsInfinite = (Data?[3] & (byte)ItemStatusEnumeration.Infinite) != 0;
            IsFlame = (Data?[3] & (byte)ItemStatusEnumeration.Flame) != 0;
            IsAcid = (Data?[3] & (byte)ItemStatusEnumeration.Acid) != 0;
            IsBOW = (Data?[3] & (byte)ItemStatusEnumeration.BOW) != 0;

            IsEquipped = isEquipped;
        }

        public ItemStatusEnumeration GetAmmoType()
        {
            if (ItemID == ItemEnumeration.FlameRounds || ItemID == ItemEnumeration.GunPowderArrow)
                return ItemStatusEnumeration.Flame;
            else if (ItemID == ItemEnumeration.AcidRounds)
                return ItemStatusEnumeration.Acid;
            else if (ItemID == ItemEnumeration.BOWGasRounds)
                return ItemStatusEnumeration.BOW;
            else if (IsFlame)
                return ItemStatusEnumeration.Flame;
            else if (IsAcid)
                return ItemStatusEnumeration.Acid;
            else if (IsBOW)
                return ItemStatusEnumeration.BOW;
            return ItemStatusEnumeration.Normal;
        }
    }
}