using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SRTPluginProviderRECVX.Models
{
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public class ProcessEntry : BaseNotifyModel, IEquatable<ProcessEntry>
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
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _processName;
        public string ProcessName
        {
            get => _processName;
            set
            {
                if (_processName != value)
                {
                    _processName = value;
                    OnPropertyChanged();
                }
            }
        }

        public override bool Equals(object obj) => 
            Equals(obj as ProcessEntry);

        public bool Equals(ProcessEntry other) => 
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