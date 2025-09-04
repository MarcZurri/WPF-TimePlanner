using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace ZTimePlanner.Controls.Controls.Planner
{
    internal abstract class PlannerBase : Grid
    {
        #region Properties

        protected bool ShowColumnsHeader { get; set; } = true;
        protected bool ShowRowsHeader { get; set; } = true;

        protected DateTime FocusDate { get; private set; }
        protected DateTime CurrentPeriodStartDate { get; set; }


        protected abstract double MinColumnWidth { get; }
        protected abstract double RowHeight { get; }

        protected abstract int NumberOfColumns { get; }
        protected abstract int NumberOfRows { get; }

        private List<Border> ColumnHeaders { get; set; } = new List<Border>();
        private List<Border> RowHeaders { get; set; } = new List<Border>();

        #endregion

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.CreateStructure();
        }

        protected abstract UIElement GetRowHeaderContent(int position);
        protected abstract UIElement GetColumnHeaderContent(int position);
        protected abstract void CalculateCurrentPeriodStartDate();

        internal void SetFocusDay(DateTime focusDay)
        {
            this.FocusDate = focusDay;
            this.CalculateCurrentPeriodStartDate();
            this.RefreshCurrentPeriodHeaders();
        }

        protected void CreateStructure()
        {
            this.AddColumns();
            this.AddRows();
            this.AddBackgroundRectangles();
        }

        private void AddColumns()
        {
            if (this.ShowColumnsHeader)
                this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Star), MinWidth = this.MinColumnWidth, MaxWidth = 200 });

            for (int i = 0; i < this.NumberOfColumns; i++)
            {
                var column = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star), MinWidth = this.MinColumnWidth };
                this.ColumnDefinitions.Add(column);
            }
        }

        private void AddRows()
        {
            if (this.ShowRowsHeader)
                this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(this.RowHeight) });

            for (int i = 0; i < this.NumberOfRows; i++)
            {
                var row = new RowDefinition() { Height = new GridLength(this.RowHeight) };
                this.RowDefinitions.Add(row);
            }
        }

        private void AddBackgroundRectangles()
        {
            this.AddHeaderCells();

            int addingColumnsIndex = this.ShowRowsHeader ? 1 : 0;
            int addingRowsIndex = this.ShowColumnsHeader ? 1 : 0;

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
            if (this.ShowColumnsHeader)
            {
                this.ColumnHeaders.Clear();
                for (int columnIndex = 1; columnIndex < this.NumberOfColumns + 1; columnIndex++)
                {
                    string cellName = $"columnHeader_{columnIndex}";
                    var content = this.GetColumnHeaderContent(columnIndex);
                    var border = new Border()
                    {
                        Name = cellName,
                        Background = System.Windows.Media.Brushes.LightGray,
                        BorderBrush = System.Windows.Media.Brushes.DarkGray,
                        BorderThickness = new Thickness(1),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Child = content
                    };

                    this.ColumnHeaders.Add(border);
                    this.AddCell(border, columnIndex, 0);
                }
            }

            if (this.ShowRowsHeader)
            {
                this.RowHeaders.Clear();
                for (int rowIndex = 1; rowIndex < this.NumberOfRows + 1; rowIndex++)
                {
                    string cellName = $"rowHeader_{rowIndex}";
                    var content = this.GetRowHeaderContent(rowIndex);
                    var border = new Border()
                    {
                        Name = cellName,
                        Background = System.Windows.Media.Brushes.LightGray,
                        BorderBrush = System.Windows.Media.Brushes.DarkGray,
                        BorderThickness = new Thickness(1),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Child = content
                    };

                    this.RowHeaders.Add(border);
                    this.AddCell(border, 0, rowIndex);
                }
            }
        }

        private void AddCell(UIElement cellElement, int columnIndex, int rowIndex, int columnSpan = 1, int rowSpan = 1)
        {
            Grid.SetColumn(cellElement, columnIndex);
            Grid.SetRow(cellElement, rowIndex);
            Grid.SetColumnSpan(cellElement, columnSpan);
            Grid.SetRowSpan(cellElement, rowSpan);
            this.Children.Add(cellElement);
        }

        private void RefreshCurrentPeriodHeaders()
        {
            foreach (var item in ColumnHeaders)
            {
                int position = int.Parse(item.Name.Split('_')[1]);
                item.Child = this.GetColumnHeaderContent(position);
            }
        }
    }
}
