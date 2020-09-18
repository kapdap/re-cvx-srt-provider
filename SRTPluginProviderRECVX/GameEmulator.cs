using SRTPluginProviderRECVX.Utilities;
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

            if (Process.ProcessName == PCSX2)
            {
                VirtualMemoryPointer = new IntPtr(0x20000000);
                ProductPointer = IntPtr.Add(VirtualMemoryPointer, 0x00015B90);
                ProductLength = 11;
                IsBigEndian = false;
            }
            else // RPCS3
            {
                VirtualMemoryPointer = new IntPtr(0x300000000);
                ProductPointer = IntPtr.Add(VirtualMemoryPointer, 0x20010251);
                ProductLength = 9;
                IsBigEndian = true;
            }
        }

        public IntPtr FindGameWindowHandle(string windowFilter = null)
        {
            if (Process != null)
            {
                List<IntPtr> windowHandles = WindowHelper.EnumerateProcessWindowHandles(Process.Id);

                foreach (IntPtr windowHandle in windowHandles)
                {
                    // https://forums.pcsx2.net/Thread-can-someone-help-PCSX2-s-ClassName
                    // How to return the PCSX2 game window handle (Post #4)
                    // 1. Find all parent window handles having the "wxWindowClassNR" class name.
                    // 2. Compare the leftmost window text of them with a string "GSdx".
                    if (Process.ProcessName == PCSX2)
                    {
                        string windowTitle = WindowHelper.GetWindowTitle(windowHandle);

                        if ((!String.IsNullOrEmpty(windowFilter) && windowTitle.Contains(windowFilter)) || windowTitle.Contains("GSdx"))
                            return windowHandle;
                    }
                    else // RPCS3
                    {
                        if (WindowHelper.GetClassName(windowHandle) != "Qt5QWindowIcon")
                            continue;

                        string windowTitle = WindowHelper.GetWindowTitle(windowHandle);

                        if ((!String.IsNullOrEmpty(windowFilter) && windowTitle.Contains(windowFilter)) || windowTitle.StartsWith("FPS"))
                            return windowHandle;
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