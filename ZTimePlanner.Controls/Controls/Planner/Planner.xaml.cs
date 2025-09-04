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
            DependencyProperty.Register("PlannerType", typeof(PlannerTypes), typeof(Planner), new PropertyMetadata(PlannerTypes.Week, PlannerTypeModified));

        private static void PlannerTypeModified(DependencyObject selfItem, DependencyPropertyChangedEventArgs eventArgs)
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
            DependencyProperty.Register("FocusDay", typeof(DateTime), typeof(Planner), new PropertyMetadata(DateTime.Now, FocusDayModified));

        private static void FocusDayModified(DependencyObject selfItem, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (((Planner)selfItem).CurrentPlanner != null)
                ((Planner)selfItem).CurrentPlanner?.SetFocusDay(((Planner)selfItem).FocusDay);
        }

        public DateTime FocusDay
        {
            get { return (DateTime)GetValue(FocusDayProperty); }
            set { SetValue(FocusDayProperty, value); }
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
