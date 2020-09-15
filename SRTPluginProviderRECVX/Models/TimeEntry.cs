using System;
using System.Diagnostics;
using System.Globalization;

namespace SRTPluginProviderRECVX.Models
{
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public class TimeEntry : BaseNotifyModel
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get => FormattedString;
        }

        private const string TIMESPAN_STRING_FORMAT = @"hh\:mm\:ss";

        private int _runningTimer;
        public int RunningTimer
        {
            get => _runningTimer;
            set
            {
                _runningTimer = value;
                OnPropertyChanged();

                Calculated = _runningTimer / 60;
            }
        }

        private int _calculated;
        public int Calculated
        {
            get => _calculated;
            private set
            {
                if (_calculated != value)
                {
                    _calculated = value;
                    OnPropertyChanged();
                    OnPropertyChanged("TimeSpan");
                    OnPropertyChanged("FormattedString");
                }
            }
        }

        public TimeSpan TimeSpan
        {
            get
            {
                if (Calculated <= TimeSpan.MaxValue.Ticks)
                    return new TimeSpan(0, 0, Calculated);
                else
                    return new TimeSpan();
            }
        }

        public string FormattedString
            => TimeSpan.ToString(TIMESPAN_STRING_FORMAT, CultureInfo.InvariantCulture);
    }
}