using SRTPluginBase;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace SRTPluginProviderRECVX
{
    public class SRTPluginProviderRECVX : IPluginProvider
    {
        private GameEmulator Emulator;
        private GameMemoryRECVXScanner MemoryScanner;
        private Stopwatch Stopwatch;
        private IPluginHostDelegates HostDelegates;
        public IPluginInfo Info => new PluginInfo();

        public bool GameRunning
        {
            get
            {
                if (MemoryScanner != null && !MemoryScanner.ProcessRunning)
                {
                    Emulator = GetEmulator();
                    if (Emulator != null)
                        MemoryScanner.Initialize(Emulator); // Re-initialize and attempt to continue.
                }

                return MemoryScanner != null && MemoryScanner.ProcessRunning;
            }
        }

        public int Startup(IPluginHostDelegates hostDelegates)
        {
            HostDelegates = hostDelegates;
            Emulator = GetEmulator();
            MemoryScanner = new GameMemoryRECVXScanner(Emulator);
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
            return 0;
        }

        public int Shutdown()
        {
            MemoryScanner?.Dispose();
            MemoryScanner = null;
            Stopwatch?.Stop();
            Stopwatch = null;
            return 0;
        }

        public object PullData()
        {
            try
            {
                if (!GameRunning) // Not running? Bail out!
                    return null;

                if (Stopwatch.ElapsedMilliseconds >= 2000L)
                {
                    MemoryScanner.UpdatePointerAddresses();
                    Stopwatch.Restart();
                }

                return MemoryScanner.Refresh();
            }
            catch (Win32Exception ex)
            {
                // Only show the error if its not ERROR_PARTIAL_COPY.
                // ERROR_PARTIAL_COPY is typically an issue with reading as the
                // program exits or reading right as the pointers are changing
                // (i.e. switching back to main menu).
                if (ex.NativeErrorCode != 0x0000012B)
                    HostDelegates.ExceptionMessage(ex);

                return null;
            }
            catch (Exception ex)
            {
                HostDelegates.ExceptionMessage(ex);
                return null;
            }
        }

        private GameEmulator GetEmulator()
            => GameEmulator.DetectEmulator();
    }
}