using System;
using System.Collections.Generic;
using System.Text;

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

        private int _score;
        public int Score
        {
            get => _score;
            set => SetField(ref _score, value);
        }

        private int _name;
        public int Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        public void UpdateScore()
        {
            Score = Time + Saves + Retry + FAS + Map + Steve + Rodrigo;

            if (Score < 1000)
                Name = 4;
            else if (Score < 4999)
                Name = 3;
            else if (Score < 6999)
                Name = 2;
            else if (Score < 9999)
                Name = 1;
            else
                Name = 0;
        }

        public static int PlayTimeScore(int timer, int difficulty, int character)
        {
            if (difficulty > 0)
                return 0;

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

        public static int SteveEventScore(int p1)
        {
            int iScore = 400;

            if (CheckFlag(p1, 155) == 0)
            {
                iScore = -1000;

                if (CheckFlag(p1, 156) == 0)
                    iScore = 0;
            }

            return iScore;
        }

        public static uint CheckFlag(int p1, uint p2)
        {
            uint r0 = p2 & 31;
            uint r1 = p2 >> 5;
            uint r2 = r1 * 4;
            uint r3 = (uint)p1 + r2;
            uint r4 = r0 & r3;
            int r5 = 0x8000 >> (int)r4;

            return (uint)r5;
        }

        public static int MapScore(int[] p1)
        {
            bool bClear = FlagCheck((uint)p1[0], 6) != 0 &&
                          FlagCheck((uint)p1[0], 7) != 0 &&
                          FlagCheck((uint)p1[1], 93) != 0 &&
                          FlagCheck((uint)p1[1], 94) != 0 &&
                          FlagCheck((uint)p1[2], 143) != 0;
            return bClear ? 100 : 0;
        }

        public static int FlagCheck(uint v1, uint a1, uint a2 = 0)
        {
            uint a0 = a1 & 0x1f;
            uint v0 = a2 & 0xff;
            int v2 = (int)(v1 << (int)a0);
            int b1 = v2 < 0 ? 1 : 0;
            int b2 = (int)(b1 ^ v0);

            return b2;
        }
    }
}