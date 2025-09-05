using System.Windows;
using System.Windows.Controls;
using ZTimePlanner.Controls.Controls.Planner.Factory;
using ZTimePlanner.Controls.Models;

namespace ZTimePlanner.Controls.Controls.Planner
{
    /// <summary>
    /// Interaction logic for Planner.xaml
    /// </summary>
    public partial class Planner : UserControl
    {
        #region Public Properties

        public static readonly DependencyProperty PlannerTypeProperty =
            DependencyProperty.Register("PlannerType", typeof(PlannerTypes), typeof(Planner), new PropertyMetadata(PlannerTypes.Week, PlannerTypeChanged));

        private static void PlannerTypeChanged(DependencyObject selfItem, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.NewValue != null && eventArgs.NewValue != eventArgs.OldValue)
                ((Planner)selfItem).CreatePlanner();
        }

        public PlannerTypes PlannerType
        {
            get { return (PlannerTypes)GetValue(PlannerTypeProperty); }
            set { SetValue(PlannerTypeProperty, value); }
        }

        public static readonly DependencyProperty FocusDayProperty =
            DependencyProperty.Register("FocusDay", typeof(DateTime), typeof(Planner), new PropertyMetadata(DateTime.Now, FocusDayChanged));

        private static void FocusDayChanged(DependencyObject selfItem, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (((Planner)selfItem).CurrentPlanner != null)
                ((Planner)selfItem).CurrentPlanner?.SetFocusDay(((Planner)selfItem).FocusDay);
        }

        public DateTime FocusDay
        {
            get { return (DateTime)GetValue(FocusDayProperty); }
            set { SetValue(FocusDayProperty, value); }
        }

        public static readonly DependencyProperty DisableInnerScrollProperty =
            DependencyProperty.Register("DisableInnerScroll", typeof(bool), typeof(Planner), new PropertyMetadata(false, DisableInnerScrollChanged));

        private static void DisableInnerScrollChanged(DependencyObject selfItem, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (((Planner)selfItem).scrollPlanner != null)
            {
                ((Planner)selfItem).scrollPlanner.VerticalScrollBarVisibility = (bool)eventArgs.NewValue ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto;
                ((Planner)selfItem).scrollPlanner.HorizontalScrollBarVisibility = (bool)eventArgs.NewValue ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto;
            }
        }

        public bool DisableInnerScroll
        {
            get { return (bool)GetValue(DisableInnerScrollProperty); }
            set { SetValue(DisableInnerScrollProperty, value); }
        }

        #endregion

        #region PlannerBase Dependency Properties

        public static readonly DependencyProperty RowHeaderTemplateProperty =
            DependencyProperty.Register("RowHeaderTemplate", typeof(DataTemplate), typeof(Planner), new PropertyMetadata(null));

        public DataTemplate RowHeaderTemplate
        {
            get { return (DataTemplate)GetValue(RowHeaderTemplateProperty); }
            set { SetValue(RowHeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty RowHeaderItemsSourceProperty =
            DependencyProperty.Register("RowHeaderItemsSource", typeof(IEnumerable<object>), typeof(Planner), new PropertyMetadata(null));

        public IEnumerable<object> RowHeaderItemsSource
        {
            get { return (IEnumerable<object>)GetValue(RowHeaderItemsSourceProperty); }
            set { SetValue(RowHeaderItemsSourceProperty, value); }
        }

        #endregion

        #region Private Properties

        private PlannerBase? CurrentPlanner { get; set; }

        #endregion

        public Planner()
        {
            InitializeComponent();
            this.Loaded += Planner_Loaded;
        }

        private void Planner_Loaded(object sender, RoutedEventArgs e)
        {
            this.CreatePlanner();
        }

        private void CreatePlanner()
        {
            this.CurrentPlanner = PlannerFactory.CreatePlanner(this.PlannerType);

            if (this.CurrentPlanner != null)
                this.CurrentPlanner.SetFocusDay(this.FocusDay);

            this.scrollPlanner.Content = this.CurrentPlanner;
        }
    }
}
