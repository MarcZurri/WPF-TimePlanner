namespace ZTimePlanner.PoC
{
    internal class DayTimeEvent
    {
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

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DayTimeEvent(DateTime start, DateTime end, string title, string description)
        {
            Start = start;
            End = end;
            Title = title;
            Description = description;
        }

        #region PlannerTemplate Properties

        public string Header
        {
            get { return this.Title; }
        }

        public string Footer
        {
            get { return $"{this.Start:HH:mm} - {this.End:HH:mm}"; }
        }

        public double StartHour
        {
            get { return this.Start.TimeOfDay.TotalHours; }
        }

        public double EndHour
        {
            get { return this.End.TimeOfDay.TotalHours; }
        }

        #endregion
    }
}
