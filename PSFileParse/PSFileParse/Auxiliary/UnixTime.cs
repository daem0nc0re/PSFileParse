using System;

namespace PSFileParse.Auxiliary
{
    public sealed class UnixTime
    {
        public UInt32 Value { get; }
        public DateTime UTC { get; }
        public DateTime LocalTime { get; }


        internal UnixTime(UInt32 value)
        {
            var unix_epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Value = value;
            UTC = unix_epoch.AddSeconds((Double)this.Value);
            LocalTime = UTC.ToLocalTime();
        }


        public override String ToString()
        {
            var dayofweek_str = this.UTC.DayOfWeek.ToString();
            return String.Format("{0}/{1}/{2} ({3}.) {4}:{5}:{6} UTC",
                this.UTC.Year.ToString("D4"),
                this.UTC.Month.ToString("D2"),
                this.UTC.Day.ToString("D2"),
                String.Join(String.Empty, dayofweek_str[0], dayofweek_str[1], dayofweek_str[2]),
                this.UTC.Hour.ToString("D2"),
                this.UTC.Minute.ToString("D2"),
                this.UTC.Second.ToString("D2"));
        }
    }
}
