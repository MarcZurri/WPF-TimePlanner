namespace ZTimePlanner.Controls.Abstractions
{
    public abstract class Event
    {
        public abstract DateTime EventStart { get; set; }
        public abstract DateTime EventEnd { get; set; }

        public int StartDayPosition { get; set; }
        public int EndDayPosition { get; set; }
        public double StartHourPosition { get; set; }
        public double EndHourPosition { get; set; }

        public int StartHourPositionAbsoloute
        {
            get { return StartHourPosition < 0 ? 0 : (int)StartHourPosition; }
        }

        public int EndHourPositionAbsoloute
        {
            get { return EndHourPosition < 0 ? 0 : Math.Round(EndHourPosition) > (int)EndHourPosition ? (int)EndHourPosition + 1 : (int)EndHourPosition; }
        }
    }
}
