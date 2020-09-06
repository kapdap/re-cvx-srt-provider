using SRTPluginProviderRECVX.Enumerations;

namespace SRTPluginProviderRECVX
{
    public class GameVersion
    {
        public const string SLPM_65022 = "SLPM_650.22";
        public const string SLUS_20184 = "SLUS_201.84";
        public const string SLES_50306 = "SLES_503.06";
        public const string NPJB00135 = "NPJB00135";
        public const string NPUB30467 = "NPUB30467";
        public const string NPEB00553 = "NPEB00553";

        public string Code { get; private set; }
        public string Name { get; private set; }
        public CountryEnumeration Country { get; private set; }
        public ConsoleEnumeration Console { get; private set; }
        public bool Supported { get; private set; }

        public GameVersion() => Update();

        public GameVersion(string code) => Update(code);

        public void Update(string code = null)
        {
            if (Code == code)
                return;

            Code = code;
            Supported = true;

            switch (Code)
            {
                case SLPM_65022:
                    Name = "BioHazard: Code: Veronica Kanzenban";
                    Country = CountryEnumeration.JP;
                    Console = ConsoleEnumeration.PS2;
                    break;

                case SLUS_20184:
                    Name = "Resident Evil: Code: Veronica X";
                    Country = CountryEnumeration.US;
                    Console = ConsoleEnumeration.PS2;
                    break;

                case SLES_50306:
                    Name = "Resident Evil: Code: Veronica X";
                    Country = CountryEnumeration.EU;
                    Console = ConsoleEnumeration.PS2;
                    break;

                case NPJB00135:
                    Name = "BioHazard: Code: Veronica Kanzenban";
                    Country = CountryEnumeration.JP;
                    Console = ConsoleEnumeration.PS3;
                    break;

                case NPUB30467:
                    Name = "Resident Evil: Code: Veronica X HD";
                    Country = CountryEnumeration.US;
                    Console = ConsoleEnumeration.PS3;
                    break;

                case NPEB00553:
                    Name = "Resident Evil: Code: Veronica X";
                    Country = CountryEnumeration.EU;
                    Console = ConsoleEnumeration.PS3;
                    break;

                default:
                    Name = "Unsupported Game";
                    Country = CountryEnumeration.None;
                    Console = ConsoleEnumeration.None;
                    Supported = false;
                    break;
            }
        }
    }
}