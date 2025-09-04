using System.Windows;

namespace ZTimePlanner.ControlsTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlannerTypeChecked(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as System.Windows.Controls.RadioButton;
            if (this.planner != null && radioButton != null && radioButton.Tag != null)
            {
                if (Enum.TryParse(radioButton.Tag.ToString(), out ZTimePlanner.Controls.Models.PlannerTypes plannerType))
                    this.planner.PlannerType = plannerType;
            }
        }
    }
}