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
                if (SetField(ref _runningTimer, value))
                    Calculated = _runningTimer / 60;
            }
        }

        private int _calculated;
        public int Calculated
        {
            get => _calculated;
            private set => SetField(ref _calculated, value, "Calculated", "TimeSpan", "FormattedString");
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