using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace ZTimePlanner.Controls.Controls.Planner
{
    internal class PlannerMonth : PlannerBase
    {
        protected override double MinColumnWidth => 100;
        protected override double RowHeight => 80;
        protected override int NumberOfColumns => 7;
        protected override int NumberOfRows => this.numberOfRows;

        public DateTime CurrentPeriodStartDatePrinted { get; private set; }

        private int numberOfRows = 6;

        internal PlannerMonth()
        {
            this.ShowRowsHeader = false;
        }

        protected override void CalculateCurrentPeriodStartDate()
        {
            this.CurrentPeriodStartDate = this.FocusDate.AddDays(-(int)this.FocusDate.Day + 1);

            this.CurrentPeriodStartDatePrinted = this.CurrentPeriodStartDate.AddDays(-(int)this.CurrentPeriodStartDate.DayOfWeek);

            if (CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek != DayOfWeek.Sunday)
                this.CurrentPeriodStartDatePrinted = this.CurrentPeriodStartDatePrinted.AddDays(1);

            this.SetNumberOfRows();
        }

        private void SetNumberOfRows()
        {
            var endDate = this.CurrentPeriodStartDate.AddMonths(1).AddDays(-1);

            int daysDiff = (endDate - this.CurrentPeriodStartDatePrinted).Days + 1;

            int numberOfWeeks = daysDiff / 7;
            numberOfWeeks = (daysDiff % 7) > 0 ? numberOfWeeks + 1 : numberOfWeeks;

            this.numberOfRows = numberOfWeeks;
        }

        protected override UIElement GetRowHeaderContent(int position)
        {
            return null;
        }

        protected override UIElement GetColumnHeaderContent(int position)
        {
            var date = this.CurrentPeriodStartDatePrinted.AddDays(position);
            string dayName = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)date.DayOfWeek];
            var dayNameTextBlock = new TextBlock()
            {
                Text = dayName,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            return dayNameTextBlock;
        }
    }
}
