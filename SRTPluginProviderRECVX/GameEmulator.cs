using SRTPluginProviderRECVX.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace SRTPluginProviderRECVX
{
    public class GameEmulator
    {
        public const string RPCS3 = "rpcs3";
        public const string PCSX2 = "pcsx2";

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

        public IntPtr FindGameWindowHandle()
        {
            List<IntPtr> windowHandles = WindowHelper.EnumerateProcessWindowHandles(Process.Id);
            foreach (IntPtr windowHandle in windowHandles)
            {
                // TODO: Let user change window title filter

                // https://forums.pcsx2.net/Thread-can-someone-help-PCSX2-s-ClassName
                // How to return the PCSX2 game window handle (Post #4)
                // 1. Find all parent window handles having the "wxWindowClassNR" class name.
                // 2. Compare the leftmost window text of them with a string "GSdx".
                if (Process.ProcessName == PCSX2)
                {
                    string title = WindowHelper.GetTitle(windowHandle);

                    if (title.Contains("GSdx"))
                        return windowHandle;
                }
                else // RPCS3
                {
                    if (WindowHelper.GetClassName(windowHandle) != "Qt5QWindowIcon")
                        continue;

                    string windowTitle = WindowHelper.GetTitle(windowHandle);

                    if (windowTitle.StartsWith("FPS") || windowTitle.EndsWith("| SRT"))
                        return windowHandle;
                }
            }

            return IntPtr.Zero;
        }

        public static List<string> GetEmulatorList()
        {
            List<string> list = new List<string>();

            foreach (FieldInfo field in typeof(GameEmulator).GetFields())
                if (field.IsLiteral && !field.IsInitOnly)
                    list.Add((string)field.GetValue(null));

            return list;
        }

        public static GameEmulator DetectEmulator()
        {
            foreach (string name in GetEmulatorList())
            {
                Process[] processes = System.Diagnostics.Process.GetProcessesByName(name);

                if (processes.Length <= 0)
                    continue;

                return new GameEmulator(processes[0]);
            }

            return null;
        }
    }
}