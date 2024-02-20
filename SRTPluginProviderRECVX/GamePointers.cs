﻿using System;

namespace SRTPluginProviderRECVX
{
    public class GamePointers
    {
        public GameVersion Version { get; } = new GameVersion();

        public IntPtr Time { get; set; }
        public IntPtr Room { get; set; }
        public IntPtr Status { get; set; }
        public IntPtr Health { get; set; }
        public IntPtr Character { get; set; }
        public IntPtr Inventory { get; set; }
        public IntPtr Difficulty { get; set; }
        public IntPtr Enemy { get; set; }
        public IntPtr EnemyCount { get; set; }
        public IntPtr Saves { get; set; }
        public IntPtr Retry { get; set; }
        public IntPtr FAS { get; set; }
        public IntPtr Map { get; set; }
        public IntPtr Steve { get; set; }
        public IntPtr Rodrigo { get; set; }
        public IntPtr Rocket { get; set; }
        public IntPtr RDX { get; set; }
    }
}