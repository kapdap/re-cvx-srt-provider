using System;
using System.Diagnostics;

namespace SRTPluginProviderRECVX.Models
{
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public class RoomEntry : BaseNotifyModel, IEquatable<RoomEntry>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                if (IsLoaded)
                    return String.Format("{0}", Id);
                else
                    return "Not Loaded";
            }
        }

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();

					Name = GetName();
                }
            }
        }

        private bool _isLoaded;
        public bool IsLoaded
        {
            get => _isLoaded;
            set
            {
                if (_isLoaded != value)
                {
                    _isLoaded = value;
                    OnPropertyChanged();
                }
            }
		}

		private string _name;
		public string Name
		{
			get => _name;
			set
			{
				if (_name != value)
				{
					_name = value;
					OnPropertyChanged();
				}
			}
		}

		private string GetName()
        {
            switch (Id)
            {
				case 0x0000:
					return "Prison B1 Cells";
				case 0x0001:
					return "Prison B1 Entry";
				case 0x0002:
					return "Prison 1F Graveyard";
				case 0x0003:
					return "Prison 1F Yard";
				case 0x0004:
					return "Prison 1F Computer Room";
				case 0x0005:
					return "Prison 1F Barracks Yard";
				case 0x0006:
					return "Prison 1F Barracks";
				case 0x0007:
					return "Prison 1F Bunkhouse";
				case 0x0008:
					return "Prison 1F Execution Yard";
				case 0x0009:
					return "Prison 1F Security Corridor";
				case 0x000A:
					return "Prison Iron Bridge";
				case 0x000B:
					return "Prison 1F Clinic";
				case 0x000C:
					return "Prison 1F Cremetorium";
				case 0x000D:
					return "Prison 1F Secret Corridor";
				case 0x000E:
					return "Prison 1F Torture Chamber";
				case 0x000F:
					return "Prison 1F Statue Room";
				case 0x0010:
					return "Prison 1F Backyard";
				case 0x0100:
					return "Palace Courtyard";
				case 0x0102:
					return "Palace Entrance Hall";
				case 0x0103:
					return "Palace 1F Bathroom";
				case 0x0104:
					return "Palace 1F Portrait Room";
				case 0x0105:
					return "Palace 1F Display Room";
				case 0x0106:
					return "Palace 1F Umbrella Meeting Room";
				case 0x0107:
					return "Palace 1F Corridor";
				case 0x0108:
					return "Palace 2F Secretarys Office";
				case 0x0109:
					return "Palace 2F Alfreds Office";
				case 0x010A:
					return "Palace 2F Residence Bridge";
				case 0x010B:
					return "Palace 2F Casino Lounge";
				case 0x010C:
					return "Palace Passage";
				case 0x010D:
					return "Palace B1 Dock";
				case 0x010E:
					return "Palace B1 Submarine";
				case 0x0200:
					return "Private Residence Courtyard";
				case 0x0201:
					return "Private Residence Entrance Hall";
				case 0x0202:
					return "Private Residence 1F Dining Room";
				case 0x0203:
					return "Private Residence 2F Bedroom Corridor";
				case 0x0204:
					return "Private Residence 2F Alfreds Bedroom";
				case 0x0205:
					return "Private Residence 2F Alexias Bedroom";
				case 0x0206:
					return "Private Residence 3F Play Room";
				case 0x0207:
					return "Private Residence 4F Private Study";
				case 0x0300:
					return "Military Training Facility Garage";
				case 0x0301:
					return "Military Training Facility Back Courtyard";
				case 0x0302:
					return "Military Training Facility 1F Tank Yard";
				case 0x0303:
					return "Military Training Facility 1F Elevator Corridor";
				case 0x0304:
					return "Military Training Facility 1F Diorama Room";
				case 0x0305:
					return "Military Training Facility 1F Lockers";
				case 0x0306:
					return "Military Training Facility 1F Swimming Pool";
				case 0x0307:
					return "Military Training Facility 1F Front Office";
				case 0x0308:
					return "Military Training Facility Lobby";
				case 0x0309:
					return "Military Training Facility 1F Excercise Yard";
				case 0x030A:
					return "Military Training Facility Warehouse";
				case 0x030B:
					return "Military Training Facility 2F Warehouse Managers Room";
				case 0x030C:
					return "Military Training Facility 2F Command Center";
				case 0x030D:
					return "Military Training Facility 2F Emergency Door";
				case 0x030F:
					return "Military Training Facility 2F Warehouse Corridor";
				case 0x0310:
					return "Military Training Facility B1 Basement Storehouse";
				case 0x0311:
					return "Military Training Facility B1 Boiler Room";
				case 0x0314:
					return "Military Training Facility B1 Weapon Repair Shop";
				case 0x0315:
					return "Military Training Facility B1 Iron Walkway";
				case 0x0316:
					return "Military Training Facility Elevator";
				case 0x0317:
					return "Military Training Facility Cargo Elevator";
				case 0x0318:
					return "Military Training Facility Jet Hangar Corridor";
				case 0x0400:
					return "Airport Check-in";
				case 0x0401:
					return "Airport Bridge";
				case 0x0402:
					return "Airport Cargo Bay";
				case 0x0403:
					return "Airport Entry";
				case 0x0404:
					return "Airport Cargo Transport";
				case 0x0405:
					return "Airport Submarine";
				case 0x0500:
					return "Transport Plane Cockpit";
				case 0x0501:
					return "Transport Plane Cargo Hold";
				case 0x0600:
					return "Antartic Transport Terminal B1 Silo";
				case 0x0601:
					return "Antartic Transport Terminal B1 Workmans Bunkhouse";
				case 0x0602:
					return "Antartic Transport Terminal B1 Tool Storeroom";
				case 0x0603:
					return "Antartic Transport Terminal B2 Frozen Corridor";
				case 0x0604:
					return "Antartic Transport Terminal B2 Office";
				case 0x0605:
					return "Antartic Transport Terminal Sorting Room";
				case 0x0606:
					return "Antartic Transport Terminal Mining Room";
				case 0x0607:
					return "Antartic Transport Terminal Power Room";
				case 0x0608:
					return "Antartic Transport Terminal B2 Weapon Room";
				case 0x0609:
					return "Antartic Transport Terminal B2 B.O.W. Room";
				case 0x060A:
					return "Antartic Transport Terminal 1F Helipad";
				case 0x0700:
					return "Military Training Facility 1F Tank Yard";
				case 0x0701:
					return "Military Training Facility Elevator";
				case 0x0702:
					return "Military Training Facility 1F Diorama Room";
				case 0x0703:
					return "Military Training Facility 2F Command Center";
				case 0x0704:
					return "Military Training Facility B1 Weapon Repair Shop";
				case 0x0705:
					return "Military Training Facility B1 Chemical Storage Room";
				case 0x0706:
					return "Military Training Facility Iron Walkway";
				case 0x0707:
					return "Military Training Facility Specimen Lab";
				case 0x0708:
					return "Military Training Facility B2 Jet Hangar Corridor";
				case 0x0709:
					return "Military Training Facility B2 Captins Room";
				case 0x070A:
					return "Military Training Facility B2 Water Pool";
				case 0x070B:
					return "Military Training Facility B1 Turn Table Room";
				case 0x070C:
					return "Military Training Facility Garage";
				case 0x070D:
					return "Military Training Facility 1F Excercise Yard";
				case 0x070E:
					return "Military Training Facility Back Courtyard";
				case 0x070F:
					return "Military Training Facility 1F Front Office";
				case 0x0710:
					return "Military Training Facility 1F Lobby";
				case 0x0711:
					return "Military Training Facility 2F Lobby";
				case 0x0712:
					return "Military Training Facility 2F Warehouse Corridor";
				case 0x0713:
					return "Military Training Facility 1F Elevator Corridor";
				case 0x0714:
					return "Military Training Facility B1 Basement Storehouse";
				case 0x0715:
					return "Military Training Facility B1 Boiler Room";
				case 0x0716:
					return "Military Training Facility B3 Cave";
				case 0x0717:
					return "Military Training Facility B3 Tomb";
				case 0x0719:
					return "Military Training Facility B1 Underground Passage";
				case 0x0800:
					return "Airport Check-in";
				case 0x0801:
					return "Airport Bridge";
				case 0x0802:
					return "Airport Cargo Bay";
				case 0x0804:
					return "Airport Cargo Transport";
				case 0x0805:
					return "Airport Entry";
				case 0x0900:
					return "Antartic Transport Terminal B1 Silo";
				case 0x0901:
					return "Antartic Transport Terminal B1 Workmans Bunkhouse";
				case 0x0902:
					return "Antartic Transport Terminal B1 Tool Storeroom";
				case 0x0903:
					return "Antartic Transport Terminal B2 Frozen Corridor";
				case 0x0904:
					return "Antartic Transport Terminal B2 Office";
				case 0x0905:
					return "Antartic Transport Terminal B1 Sorting Room";
				case 0x0906:
					return "Antartic Transport Terminal B1 Mining Room";
				case 0x0907:
					return "Antartic Transport Terminal B1 Power Room";
				case 0x0908:
					return "Antartic Transport Terminal B2 Weapon Room";
				case 0x0909:
					return "Antartic Transport Terminal B1 Jet Hanger";
				case 0x090A:
					return "Antartic Transport Terminal B1 Corridor";
				case 0x090B:
					return "Antartic Transport Terminal B1 Water Tank Room";
				case 0x090C:
					return "Antartic Transport Terminal B5 Icy Corridor";
				case 0x090D:
					return "Antartic Transport Terminal B5 Mansion Courtyard";
				case 0x090E:
					return "Antartic Transport Terminal B5 High Voltage Room";
				case 0x090F:
					return "Antartic Transport Terminal B5 Main Hall";
				case 0x0910:
					return "Antartic Transport Terminal B5 Art Room";
				case 0x0911:
					return "Antartic Transport Terminal B6 Ant Farm";
				case 0x0912:
					return "Antartic Transport Terminal B6 Alexias Lab";
				case 0x0913:
					return "Antartic Transport Terminal B6 Alexias Study";
				case 0x0914:
					return "Antartic Transport Terminal B4 Bedroom Corridor";
				case 0x0915:
					return "Antartic Transport Terminal B4 Study";
				case 0x0916:
					return "Antartic Transport Terminal B4 T-Veronica Lab";
				case 0x0917:
					return "Antartic Transport Terminal B4 Alfreds Bedroom";
				case 0x0918:
					return "Antartic Transport Terminal B4 Alexias Bedroom";
				case 0x0919:
					return "Antartic Transport Terminal B3 Conference Room";
				case 0x091A:
					return "Antartic Transport Terminal B4 Sitting Room";
				case 0x091B:
					return "Antartic Transport Terminal B4 L Corridor";
				case 0x091C:
					return "Antartic Transport Terminal B4 Hall of Armors";
				case 0x091D:
					return "Antartic Transport Terminal B4 Detention Center";
				case 0x091E:
					return "Antartic Transport Terminal B4 Upper Ant Farm";
				case 0x091F:
					return "Antartic Transport Terminal B4 Reactor Core";
				case 0x0920:
					return "Antartic Transport Terminal B5 Main Hall Destroyed";
				case 0x0922:
					return "Antartic Transport Terminal End Cinematic 1";
				case 0x0923:
					return "Antartic Transport Terminal End Cinematic 2";
				case 0x0924:
					return "Antartic Transport Terminal End Cinematic 3";
				case 0x0925:
					return "Antartic Transport Terminal End Cinematic 4";
				case 0x0534:
					return "Battle Time Room 1";
				case 0x0535:
					return "Battle Time Room 2";
				case 0x0536:
					return "Battle Time Room 3";
				case 0x0537:
					return "Battle Time Room 4";
				case 0x053A:
					return "Battle Time Room 5";
				case 0x053B:
					return "Battle Time Room 6";
				case 0x053C:
					return "Battle Time Room 7";
				case 0x053F:
					return "Battle Time Room 8";
				case 0x0540:
					return "Battle Time Room 9";
				case 0x0541:
					return "Battle Time Room 10";
				case 0x0542:
					return "Battle Time Room 11";
				case 0x0543:
					return "Battle Time Room 12";
				case 0x0544:
					return "Battle Time Room 13";
				case 0x0545:
					return "Battle Time Room 14";
				case 0x0546:
					return "Battle Time Room 15";
				case 0x0547:
					return "Battle Time Room 16";
				case 0x0550:
					return "Battle Time Boss 17";
				default:
					return "No Room Loaded";
			}
        }

        public override bool Equals(object obj) => 
            Equals(obj as RoomEntry);

        public bool Equals(RoomEntry other) => 
            other != null &&
            Id == other.Id &&
            IsLoaded == other.IsLoaded;

        public override int GetHashCode()
        {
            int hashCode = -659867440;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + IsLoaded.GetHashCode();
            return hashCode;
        }
    }
}