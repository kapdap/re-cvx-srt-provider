using SRTPluginProviderRECVX.Enumerations;
using SRTPluginProviderRECVX.Models;
using System;
using System.Collections.Generic;

namespace SRTPluginProviderRECVX
{
    public class GameVersion : BaseNotifyModel, IEquatable<GameVersion>
    {
        public const string SLPM_65022 = "SLPM_650.22";
        public const string SLUS_20184 = "SLUS_201.84";
        public const string SLES_50306 = "SLES_503.06";
        public const string NPJB00135 = "NPJB00135";
        public const string NPUB30467 = "NPUB30467";
        public const string NPEB00553 = "NPEB00553";
        public const string GCDJ08 = "GCDJ08";
        public const string GCDE08 = "GCDE08";
        public const string GCDP08 = "GCDP08";

        private string _code;
        public string Code
        {
            get => _code;
            private set => SetField(ref _code, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            private set => SetField(ref _name, value);
        }

        private CountryEnumeration _country;
        public CountryEnumeration Country
        {
            get => _country;
            private set => SetField(ref _country, value);
        }

        private ConsoleEnumeration _console;
        public ConsoleEnumeration Console
        {
            get => _console;
            private set => SetField(ref _console, value);
        }

        private bool _supported;
        public bool Supported
        {
            get => _supported;
            private set => SetField(ref _supported, value);
        }

        public static bool IsValid(string code) => 
            code == GCDJ08 ||
            code == GCDE08 ||
            code == GCDP08 ||
            code == SLPM_65022 ||
            code == SLUS_20184 ||
            code == SLES_50306 ||
            code == NPJB00135 ||
            code == NPUB30467 ||
            code == NPEB00553;

        public GameVersion() => Update();

        public GameVersion(string code) => Update(code);

        public void Update(string code = null)
        {
            if (String.IsNullOrEmpty(code))
                code = "None";

            if (Code == code)
                return;

            Code = code;
            Supported = true;

            switch (Code)
            {
                case GCDJ08:
                    Name = "BioHazard: Code: Veronica Kanzenban";
                    Country = CountryEnumeration.JP;
                    Console = ConsoleEnumeration.GC;
                    break;

                case GCDE08:
                    Name = "Resident Evil: Code: Veronica X";
                    Country = CountryEnumeration.US;
                    Console = ConsoleEnumeration.GC;
                    break;

                case GCDP08:
                    Name = "Resident Evil: Code: Veronica X";
                    Country = CountryEnumeration.EU;
                    Console = ConsoleEnumeration.GC;
                    break;

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

        public override bool Equals(object obj) => 
            Equals(obj as GameVersion);

        public bool Equals(GameVersion other) => 
            other != null &&
            Code == other.Code;

        public override int GetHashCode() => 
            -434485196 + EqualityComparer<string>.Default.GetHashCode(Code);
    }
}