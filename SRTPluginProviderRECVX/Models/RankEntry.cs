using SRTPluginProviderRECVX.Enumerations;

namespace SRTPluginProviderRECVX.Models
{
    public class RankEntry : BaseNotifyModel
    {
        private int _time;
        public int Time
        {
            get => _time;
            set => SetField(ref _time, value);
        }

        private int _saves;
        public int Saves
        {
            get => _saves;
            set => SetField(ref _saves, value);
        }

        private int _retry;
        public int Retry
        {
            get => _retry;
            set => SetField(ref _retry, value);
        }

        private int _fas;
        public int FAS
        {
            get => _fas;
            set => SetField(ref _fas, value);
        }

        private int _map;
        public int Map
        {
            get => _map;
            set => SetField(ref _map, value);
        }

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

        private bool _rocket;
        public bool Rocket
        {
            get => _rocket;
            set => SetField(ref _rocket, value);
        }

        private int _score;
        public int Score
        {
            get => _score;
            set => SetField(ref _score, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        // TODO: Battle Mode
        public void UpdateScore(DifficultyEnumeration difficulty)
        {
            Score = Time + Saves + Retry + FAS + Map + Steve + Rodrigo;

            if (difficulty != DifficultyEnumeration.Normal)
                Name = "F";
            if (Score < 1000)
                Name = "E";
            else if (Score < 4999)
                Name = "D";
            else if (Score < 6999)
                Name = "C";
            else if (Score < 9999 || Rocket)
                Name = "B";
            else
                Name = "S/A";
        }

        public static int PlayTimeScore(int timer)
        {
            if (timer < 972060)
                return 8250;
            else if(timer < 1080060)
                return 7550;
            else if (timer < 1188060)
                return 7000;
            else if (timer < 1317660)
                return 6450;
            else if (timer < 1620060)
                return 5500;
            else if (timer < 3240060)
                return 5000;
            else if (timer < 4320060)
                return 2500;
            else
                return 2000;
        }

        public static int HealItemUseScore(int p1) =>
            p1 == 0 ? 1800 : 0;

        public static int SaveCountScore(int p1) =>
            p1 != 0 ? (p1 + -1) * -50 + -1000 : 0;

        public static int RetryCountScore(int p1) =>
            p1 != 0 ? (p1 + -1) * -50 + -1000 : 0;

        public static int RodrigoEventScore(int p1) =>
            CheckFlag(p1, 96) == 0 ? -1000 : 250;

        public static int SteveEventScore(int p1) =>
            CheckFlag(p1, 155) != 0 ? 400 : CheckFlag(p1, 156) != 0 ? -1000 : 0;

        public static bool RocketUsed(int p1) =>
            CheckFlag(p1, 75) != 0;

        public static int CheckFlag(int p1, int p2) =>
            (int)(0x80000000 >> (p2 & 31) & p1);

        public static int MapScore(int[] p1)
        {
            return FlagCheck(p1[0], 6) != 0 &&
                   FlagCheck(p1[0], 7) != 0 &&
                   FlagCheck(p1[1], 93) != 0 &&
                   FlagCheck(p1[1], 94) != 0 &&
                   FlagCheck(p1[2], 143) != 0 ? 100 : 0;
        }

        public static int FlagCheck(int p1, int p2, int p3 = 0)
        {
            int r1 = p1 << (p2 & 31);
            int r2 = r1 < 0 ? 1 : 0;

            return p3 ^ r2;
        }
    }
}