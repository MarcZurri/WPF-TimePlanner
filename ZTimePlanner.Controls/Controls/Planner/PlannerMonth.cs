using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace ZTimePlanner.Controls.Controls.Planner
{
    internal class PlannerMonth : PlannerBase
    {
        protected override double MinColumnWidth => 100;
        protected override double RowHeight => 80;
        protected override RowDefinition? RowDefinition => new RowDefinition() { Height = new GridLength(1, GridUnitType.Star), MinHeight = this.RowHeight };
        protected override double HeaderRowHeight => 30;
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

        protected override UIElement GetContentCellBackground(int columnIndex, int rowIndex)
        {
            Border border = new Border()
            {
                Background = System.Windows.Media.Brushes.Transparent,
                BorderBrush = System.Windows.Media.Brushes.DarkGray,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };

            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            border.Child = grid;

            DateTime date = this.CurrentPeriodStartDatePrinted.AddDays(rowIndex * 7 + columnIndex);
            string text = date.Day == 1 ? $"{date.Day} {CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[date.Month - 1]}" : date.Day.ToString();
            TextBlock textBlock = new TextBlock()
            {
                Text = text,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(2)
            };
            grid.Children.Add(textBlock);
            Grid.SetRow(textBlock, 0);

            return border;
        }
    }
}
