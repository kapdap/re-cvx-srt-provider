﻿using SRTPluginProviderRECVX.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace SRTPluginProviderRECVX
{
    public class GameEmulator : IDisposable
    {
        public const string RPCS3 = "rpcs3";
        public const string PCSX2 = "pcsx2";
        public const string PCSX2QT = "pcsx2-qt";
        public const string PCSX264QT = "pcsx2-qtx64";
        public const string PCSX264QTAV = "pcsx2-qtx64-avx2";
        public const string PCSX264WX = "pcsx2x64";
        public const string PCSX264WXAV = "pcsx2x64-avx2";
        public const string Dolphin = "dolphin";

        private Dolphin.Memory.Access.Dolphin _dolphin;

        private static List<string> _emulatorList;
        public static List<string> EmulatorList
        {
            get
            {
                if (_emulatorList == null)
                {
                    _emulatorList = new List<string>();

                    foreach (FieldInfo field in typeof(GameEmulator).GetFields())
                        if (field.IsLiteral && !field.IsInitOnly)
                            _emulatorList.Add((string)field.GetValue(null));
                }

                return _emulatorList;
            }
        }

        public Process Process { get; private set; }

        public IntPtr VirtualMemoryPointer { get; private set; }
        public IntPtr ProductPointer { get; private set; }
        public int ProductLength { get; private set; }
        public bool IsBigEndian { get; private set; }

        public GameEmulator(Process process)
        {
            if ((Process = process) == null)
                return;

            if (Process.HasExited)
                return;

            if (Process.ProcessName.ToLower() == Dolphin)
            {
                UpdateVirtualMemoryPointer();
                ProductLength = 6;
                IsBigEndian = true;
            }
            else if (Process.ProcessName.ToLower().StartsWith(PCSX2))
            {
                UpdateVirtualMemoryPointer();
                ProductLength = 11;
                IsBigEndian = false;
            }
            else // RPCS3
            {
                UpdateVirtualMemoryPointer();
                ProductLength = 9;
                IsBigEndian = true;
            }
        }

        public void UpdateVirtualMemoryPointer()
        {
            if (Process == null)
                return;

            if (Process.HasExited)
                return;

            if (Process.ProcessName.ToLower() == Dolphin)
            {
                IntPtr pointer = IntPtr.Zero;

                _dolphin = _dolphin ?? new Dolphin.Memory.Access.Dolphin(Process);
                _dolphin.TryGetBaseAddress(out pointer);

                VirtualMemoryPointer = pointer;
            }
            else if (Process.ProcessName.ToLower().StartsWith(PCSX2))
            {
                if (Process.ProcessName.ToLower() == PCSX2) // PCSX2 1.6 and earlier
                {
                    VirtualMemoryPointer = new IntPtr(0x20000000);
                }
                else // PCSX2 1.7+
                {
                    // https://forums.pcsx2.net/Thread-PCSX2-1-7-Cheat-Engine-Script-Compatibility
                    IntPtr process = NativeWrappers.LoadLibrary(Process.MainModule.FileName);
                    IntPtr address = NativeWrappers.GetProcAddress(process, "EEmem");

                    VirtualMemoryPointer = (IntPtr)Process.ReadValue<long>(address);

                    NativeWrappers.FreeLibrary(process);
                }
            }
            else // RPCS3
            {
                VirtualMemoryPointer = new IntPtr(0x300000000);
            }
        }

        public IntPtr FindGameWindowHandle(string filter = null)
        {
            if (Process != null)
            {
                List<IntPtr> handles = WindowHelper.EnumerateProcessWindowHandles(Process.Id);

                foreach (IntPtr handle in handles)
                {
                    if (Process.ProcessName.ToLower() == Dolphin)
                    {
                        string title = WindowHelper.GetWindowTitle(handle);

                        if ((!String.IsNullOrEmpty(filter) && title.Contains(filter)) || title.StartsWith("Dolphin 5.0 |"))
                            return handle;
                    }
                    // https://forums.pcsx2.net/Thread-can-someone-help-PCSX2-s-ClassName
                    // How to return the PCSX2 game window handle (Post #4)
                    // 1. Find all parent window handles having the "wxWindowClassNR" class name.
                    // 2. Compare the leftmost window text of them with a string "GSdx".
                    else if (Process.ProcessName.ToLower().StartsWith(PCSX2))
                    {
                        string title = WindowHelper.GetWindowTitle(handle);

                        if ((!String.IsNullOrEmpty(filter) && title.Contains(filter)) ||
                            title.Contains("GS:") ||
                            title.Contains("BioHazard") ||
                            title.Contains("Resident Evil"))
                            return handle;
                    }
                    else if (!WindowHelper.GetClassName(handle).EndsWith("QWindowIcon")) // RPCS3
                    {
                        string title = WindowHelper.GetWindowTitle(handle);

                        if ((!String.IsNullOrEmpty(filter) && title.Contains(filter)) || title.StartsWith("FPS"))
                            return handle;
                    }
                }
            }

            return IntPtr.Zero;
        }

        public static GameEmulator DetectEmulator()
        {
            foreach (string name in EmulatorList)
            {
                Process[] processes = System.Diagnostics.Process.GetProcessesByName(name);

                if (processes.Length <= 0)
                    continue;

                return new GameEmulator(processes[0]);
            }

            return null;
        }

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Process?.Dispose();
                    Process = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}