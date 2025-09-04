using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace ZTimePlanner.Controls.Controls.Planner
{
    internal class PlannerWorkWeek : PlannerBase
    {
        protected override double MinColumnWidth => 100;
        protected override double RowHeight => 50;
        protected override int NumberOfColumns => 5;
        protected override int NumberOfRows => 24;

        internal PlannerWorkWeek()
        {
        }

        protected override void CalculateCurrentPeriodStartDate()
        {
            this.CurrentPeriodStartDate = this.FocusDate.AddDays(-(int)this.FocusDate.DayOfWeek).AddDays(1);
        }

        protected override UIElement GetRowHeaderContent(int position)
        {
            return new TextBlock()
            {
                Text = $"{position - 1:00}:00",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        protected override UIElement GetColumnHeaderContent(int position)
        {
            var date = this.CurrentPeriodStartDate.AddDays(position - 1);
            string dayName = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)date.DayOfWeek];
            var dayNameTextBlock = new TextBlock()
            {
                Text = dayName,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            var dateTextBlock = new TextBlock()
            {
                Text = date.ToString("dd.MM"),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            var stackPanel = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            stackPanel.Children.Add(dayNameTextBlock);
            stackPanel.Children.Add(dateTextBlock);
            return stackPanel;
        }

    }
}
