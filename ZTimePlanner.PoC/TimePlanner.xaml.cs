using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using ZTimePlanner.PoC.PlannerElements;

namespace ZTimePlanner.PoC
{
    /// <summary>
    /// Interaction logic for TimePlanner.xaml
    /// </summary>
    public partial class TimePlanner : UserControl
    {
        private readonly double MinColumnWidth = 100;
        private readonly double RowHeight = 50;

        private bool HasColumnsHeader { get; set; } = true;
        private bool HasRowsHeader { get; set; } = true;

        private int NumberOfColumns { get; set; } = 5;
        private int NumberOfRows { get; set; } = 24;

        public TimePlanner()
        {
            InitializeComponent();
            this.CreateStructure();

            this.Loaded += TimePlanner_Loaded;
        }

        private void TimePlanner_Loaded(object sender, RoutedEventArgs e)
        {
            var timeEvents = this.GenerateFakeItems();
            int addingColumnsIndex = this.HasRowsHeader ? 1 : 0;
            int addingRowsIndex = this.HasColumnsHeader ? 1 : 0;

            foreach (var timeEvent in timeEvents)
            {
                var item = (timeEvent.Item as DayTimeEvent);
                if (item.StartHourPosition % 1 > 0)
                {
                    timeEvent.Margin = new Thickness(timeEvent.Margin.Left, (RowHeight / 2) + timeEvent.Margin.Top, timeEvent.Margin.Right, timeEvent.Margin.Bottom);
                }

                if (item.EndHourPosition % 1 > 0)
                {
                    timeEvent.Margin = new Thickness(timeEvent.Margin.Left, timeEvent.Margin.Top, timeEvent.Margin.Right, (RowHeight / 2) + timeEvent.Margin.Bottom);
                }

                this.AddCell(timeEvent, item.StartDayPosition + addingColumnsIndex, (int)item.StartHourPosition + addingRowsIndex, 1, (int)(item.EndHourPosition - item.StartHourPosition));
            }
        }

        private void CreateStructure()
        {

            this.AddColumns();
            this.AddRows();
            this.AddBackgroundRectangles();
        }

        private void AddColumns()
        {
            if (this.HasColumnsHeader)
                this.timePlanner.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Star), MinWidth = this.MinColumnWidth, MaxWidth = 200 });

            for (int i = 0; i < this.NumberOfColumns; i++)
            {
                var column = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star), MinWidth = this.MinColumnWidth };
                this.timePlanner.ColumnDefinitions.Add(column);
            }
        }

        private void AddRows()
        {
            if (this.HasRowsHeader)
                this.timePlanner.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(this.RowHeight) });

            for (int i = 0; i < this.NumberOfRows; i++)
            {
                var row = new RowDefinition() { Height = new GridLength(this.RowHeight) };
                this.timePlanner.RowDefinitions.Add(row);
            }
        }

        private void AddBackgroundRectangles()
        {
            this.AddHeaderCells();

            int addingColumnsIndex = this.HasRowsHeader ? 1 : 0;
            int addingRowsIndex = this.HasColumnsHeader ? 1 : 0;

            for (int columnIndex = 0; columnIndex < this.NumberOfColumns; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < this.NumberOfRows; rowIndex++)
                {
                    string cellName = $"Cell_{columnIndex}_{rowIndex}";
                    var rectangle = new Rectangle()
                    {
                        Name = cellName,
                        Fill = System.Windows.Media.Brushes.Transparent,
                        Stroke = System.Windows.Media.Brushes.DarkGray,
                        StrokeThickness = 1,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                    };

                    this.AddCell(rectangle, columnIndex + addingColumnsIndex, rowIndex + addingRowsIndex);
                }
            }
        }

        private void AddHeaderCells()
        {
            if (this.HasColumnsHeader)
            {
                for (int columnIndex = 1; columnIndex < this.NumberOfColumns + 1; columnIndex++)
                {
                    string cellName = $"columnHeader_{columnIndex}";
                    var rectangle = new Rectangle()
                    {
                        Name = cellName,
                        Fill = System.Windows.Media.Brushes.LightGray,
                        Opacity = 0.5,
                        Stroke = System.Windows.Media.Brushes.DarkGray,
                        StrokeThickness = 1,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                    };

                    this.AddCell(rectangle, columnIndex, 0);
                }
            }

            if (this.HasRowsHeader)
            {
                for (int rowIndex = 1; rowIndex < this.NumberOfRows + 1; rowIndex++)
                {
                    string cellName = $"rowHeader_{rowIndex}";
                    var rectangle = new Rectangle()
                    {
                        Name = cellName,
                        Fill = System.Windows.Media.Brushes.LightGray,
                        Opacity = 0.5,
                        Stroke = System.Windows.Media.Brushes.DarkGray,
                        StrokeThickness = 1,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                    };

                    this.AddCell(rectangle, 0, rowIndex);
                }
            }
        }

        private void AddContentCells()
        {
        }

        private void AddCell(UIElement cellElement, int columnIndex, int rowIndex, int columnSpan = 1, int rowSpan = 1)
        {
            Grid.SetColumn(cellElement, columnIndex);
            Grid.SetRow(cellElement, rowIndex);
            Grid.SetColumnSpan(cellElement, columnSpan);
            Grid.SetRowSpan(cellElement, rowSpan);
            this.timePlanner.Children.Add(cellElement);
        }

        private IEnumerable<PlannerItemControl> GenerateFakeItems()
        {
            List<Tuple<DateTime, DateTime>> dates = new List<Tuple<DateTime, DateTime>>()
            {
                new Tuple<DateTime, DateTime>(new DateTime(2025, 8, 15, 9, 0, 0), new DateTime(2025, 8, 15, 11, 30, 0)),
                new Tuple<DateTime, DateTime>(new DateTime(2025, 8, 15, 13, 0, 0), new DateTime(2025, 8, 15, 14, 0, 0)),
                new Tuple<DateTime, DateTime>(new DateTime(2025, 8, 16, 6, 30, 0), new DateTime(2025, 8, 16, 7, 30, 0)),
                new Tuple<DateTime, DateTime>(new DateTime(2025, 8, 17, 10, 00, 0), new DateTime(2025, 8, 17, 11, 00, 0)),
                new Tuple<DateTime, DateTime>(new DateTime(2025, 8, 17, 12, 00, 0), new DateTime(2025, 8, 17, 13, 30, 0)),
                new Tuple<DateTime, DateTime>(new DateTime(2025, 8, 18, 9, 00, 0), new DateTime(2025, 8, 18, 10, 00, 0)),
                new Tuple<DateTime, DateTime>(new DateTime(2025, 8, 18, 9, 00, 0), new DateTime(2025, 8, 18, 10, 00, 0)),
            };

            int firstDayPosition = dates.Min(d => d.Item1.Day);

            IEnumerable<PlannerItemControl> items = dates.Select(d => this.CreatePlannerItem(new DayTimeEvent(d.Item1, d.Item2, "Meeting", "Meeting description")
            {
                StartDayPosition = d.Item1.Day - firstDayPosition,
                EndDayPosition = d.Item2.Day - firstDayPosition,
                StartHourPosition = d.Item1.TimeOfDay.TotalHours,
                EndHourPosition = d.Item2.TimeOfDay.Minutes == 0 ? d.Item2.TimeOfDay.TotalHours : d.Item2.TimeOfDay.TotalHours + 1
            }));

            return items;
        }

        private PlannerItemControl CreatePlannerItem(DayTimeEvent dayTimeEvent)
        {
            return new PlannerItemControl()
            {
                Background = System.Windows.Media.Brushes.LightCyan,
                BorderBrush = System.Windows.Media.Brushes.DarkCyan,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(2),
                ChildControl = new PlannerEvent(),
                Item = dayTimeEvent
            };
        }
    }
}
