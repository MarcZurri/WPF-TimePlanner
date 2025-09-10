using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        protected abstract double HeaderRowHeight { get; }
        protected virtual RowDefinition? RowDefinition => null;

        protected abstract int NumberOfColumns { get; }
        protected abstract int NumberOfRows { get; }

        private List<Border> ColumnHeaders { get; set; } = new List<Border>();
        private List<Border> RowHeaders { get; set; } = new List<Border>();

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty RowHeaderTemplateProperty =
            DependencyProperty.Register("RowHeaderTemplate", typeof(DataTemplate), typeof(PlannerBase), new PropertyMetadata(null, RowHeaderTemplateChanged));

        private static void RowHeaderTemplateChanged(DependencyObject selfItem, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.NewValue != null && eventArgs.NewValue != eventArgs.OldValue)
                ((PlannerBase)selfItem).RefreshRowHeaders();
        }

        public DataTemplate RowHeaderTemplate
        {
            get { return (DataTemplate)GetValue(RowHeaderTemplateProperty); }
            set { SetValue(RowHeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty RowHeaderItemsSourceProperty =
            DependencyProperty.Register("RowHeaderItemsSource", typeof(IEnumerable<object>), typeof(PlannerBase), new PropertyMetadata(null, RowHeaderItemsSourceChanged));

        private static void RowHeaderItemsSourceChanged(DependencyObject selfItem, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.NewValue != null && eventArgs.NewValue != eventArgs.OldValue)
                ((PlannerBase)selfItem).RefreshRowHeaders();
        }

        public IEnumerable<object> RowHeaderItemsSource
        {
            get { return (IEnumerable<object>)GetValue(RowHeaderItemsSourceProperty); }
            set { SetValue(RowHeaderItemsSourceProperty, value); }
        }

        #endregion

        internal PlannerBase()
        {
            Binding plannerRowHeaderTemplateBinding = new Binding("RowHeaderTemplate") { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Planner), 1) };
            this.SetBinding(RowHeaderTemplateProperty, plannerRowHeaderTemplateBinding);

            Binding plannerRowHeaderItemsSourceBinding = new Binding("RowHeaderItemsSource") { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Planner), 1) };
            this.SetBinding(RowHeaderItemsSourceProperty, plannerRowHeaderItemsSourceBinding);
        }

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
            this.Children?.Clear();

            this.AddColumns();
            this.AddRows();
            this.AddBackgroundRectangles();
        }

        private void AddColumns()
        {
            this.ColumnDefinitions.Clear();

            if (this.ShowRowsHeader)
                this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Star), MinWidth = this.MinColumnWidth, MaxWidth = 200 });

            for (int i = 0; i < this.NumberOfColumns; i++)
            {
                var column = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star), MinWidth = this.MinColumnWidth };
                this.ColumnDefinitions.Add(column);
            }
        }

        private void AddRows()
        {
            this.RowDefinitions.Clear();

            if (this.ShowColumnsHeader)
                this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(this.HeaderRowHeight) });

            for (int i = 0; i < this.NumberOfRows; i++)
            {
                var row = this.RowDefinition ?? new RowDefinition() { Height = new GridLength(this.RowHeight) };
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
                    var uiElement = this.GetContentCellBackground(columnIndex, rowIndex);
                    uiElement.SetValue(FrameworkElement.NameProperty, cellName);

                    this.AddCell(uiElement, columnIndex + addingColumnsIndex, rowIndex + addingRowsIndex);
                }
            }
        }

        protected virtual UIElement GetContentCellBackground(int columnIndex, int rowIndex)
        {
            return new Rectangle()
            {
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = System.Windows.Media.Brushes.DarkGray,
                StrokeThickness = 1,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
        }

        private void AddHeaderCells()
        {
            if (this.ShowColumnsHeader)
            {
                int addingColumnsIndex = this.ShowRowsHeader ? 1 : 0;
                this.ColumnHeaders.Clear();

                for (int columnIndex = 0; columnIndex < this.NumberOfColumns; columnIndex++)
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
                    this.AddCell(border, columnIndex + addingColumnsIndex, 0);
                }
            }

            if (this.ShowRowsHeader)
            {
                int addingRowsIndex = this.ShowColumnsHeader ? 1 : 0;
                this.RowHeaders.Clear();

                for (int rowIndex = 0; rowIndex < this.NumberOfRows; rowIndex++)
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
                    this.AddCell(border, 0, rowIndex + addingRowsIndex);
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

        private void RefreshRowHeaders()
        {
            foreach (var item in RowHeaders)
            {
                int position = int.Parse(item.Name.Split('_')[1]);
                item.Child = this.GetRowHeaderContent(position);
            }
        }
    }
}
