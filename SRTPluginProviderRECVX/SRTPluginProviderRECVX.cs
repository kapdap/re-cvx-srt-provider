using SRTPluginBase;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace SRTPluginProviderRECVX
{
    public class SRTPluginProviderRECVX : IPluginProvider
    {
        private GameMemoryRECVXScanner _memoryScanner;
        private Stopwatch _stopwatch;
        private IPluginHostDelegates _hostDelegates;
        public IPluginInfo Info => new PluginInfo();

        public bool GameRunning
        {
            get
            {
                if (_memoryScanner != null && !_memoryScanner.ProcessRunning)
                    _memoryScanner.Initialize(GetEmulator());

                if (_memoryScanner == null || !_memoryScanner.ProcessRunning)
                    return false;

                _memoryScanner.UpdateGameVersion();

                return _memoryScanner.Memory.Version.Supported;
            }
        }

        public int Startup(IPluginHostDelegates hostDelegates)
        {
            try
            {
                _hostDelegates = hostDelegates;
                _memoryScanner = new GameMemoryRECVXScanner(GetEmulator());
                _stopwatch = new Stopwatch();
                _stopwatch.Start();
                return 0;
            }
            catch (Exception ex)
            {
                _hostDelegates.ExceptionMessage(ex);
                return 1;
            }
        }

        public int Shutdown()
        {
            try
            {
                _memoryScanner?.Dispose();
                _memoryScanner = null;
                _stopwatch?.Stop();
                _stopwatch = null;
                return 0;
            }
            catch (Exception ex)
            {
                _hostDelegates.ExceptionMessage(ex);
                return 1;
            }
        }

        public object PullData()
        {
            try
            {
                if (!GameRunning) // Not running? Bail out!
                    return null;

                if (_stopwatch.ElapsedMilliseconds >= 2000L)
                {
                    _memoryScanner.Update();
                    _stopwatch.Restart();
                }

                return _memoryScanner.Refresh();
            }
            catch (Win32Exception ex)
            {
                // Only show the error if its not ERROR_PARTIAL_COPY.
                // ERROR_PARTIAL_COPY is typically an issue with reading as the
                // program exits or reading right as the pointers are changing
                // (i.e. switching back to main menu).
                if (ex.NativeErrorCode != 0x0000012B)
                    _hostDelegates.ExceptionMessage(ex);
                return null;
            }
            catch (Exception ex)
            {
                _hostDelegates.ExceptionMessage(ex);
                return null;
            }
        }

        private GameEmulator GetEmulator()
            => GameEmulator.DetectEmulator();
    }
}