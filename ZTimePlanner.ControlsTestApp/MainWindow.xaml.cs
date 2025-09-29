using System.Windows;
using ZTimePlanner.Controls.Models;

namespace ZTimePlanner.ControlsTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PlannerTypes? PlannerType
        {
            get { return this.plannerType; }
            set
            {
                if (value != null)
                    this.plannerType = (PlannerTypes)value;
            }
        }

        private PlannerTypes plannerType = PlannerTypes.WorkWeek;

        public List<string> RowHeaders = new List<string>()
        {
        "1","2","3","4","5","6","7","8","9","10",
        };

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            //this.planner.RowHeaderItemsSource = this.RowHeaders;
        }

        private void PlannerTypeChecked(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as System.Windows.Controls.RadioButton;
            if (this.planner != null && this.PlannerType != null && this.planner.PlannerType != this.PlannerType)
                this.planner.PlannerType = this.PlannerType.Value;
        }
    }
}