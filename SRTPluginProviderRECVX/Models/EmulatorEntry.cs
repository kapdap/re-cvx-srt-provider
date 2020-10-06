using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SRTPluginProviderRECVX.Models
{
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public class EmulatorEntry : BaseNotifyModel, IEquatable<EmulatorEntry>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get => String.Format("{0} ({1})", ProcessName, Id);
        }

        private int _id;
        public int Id
        {
            get => _id;
            set => SetField(ref _id, value);
        }

        private string _processName;
        public string ProcessName
        {
            get => _processName;
            set => SetField(ref _processName, value);
        }

        private IntPtr _gameWindowHandle;
        public IntPtr GameWindowHandle
        {
            get => _gameWindowHandle;
            set => SetField(ref _gameWindowHandle, value);
        }

        private string _gameWindowTitleFilter;
        public string GameWindowTitleFilter
        {
            get => _gameWindowTitleFilter;
            set => SetField(ref _gameWindowTitleFilter, value);
        }

        private bool _detectGameWindowHandle;
        public bool DetectGameWindowHandle
        {
            get => _detectGameWindowHandle;
            set => SetField(ref _detectGameWindowHandle, value);
        }

        public override bool Equals(object obj) =>
            Equals(obj as EmulatorEntry);

        public bool Equals(EmulatorEntry other) =>
            other != null &&
            Id == other.Id &&
            ProcessName == other.ProcessName;

        public override int GetHashCode()
        {
            int hashCode = -1107992127;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ProcessName);
            return hashCode;
        }
    }
}