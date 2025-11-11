using System.Windows;
using System.Linq;
using System.Threading.Tasks;
using StudentManagement.Models;
using StudentManagement.Services;

namespace StudentManagement.Views
{
    public partial class ScheduleDialog : Window
    {
        public Schedule? Schedule { get; private set; }

        public ScheduleDialog(Schedule? schedule = null)
        {
            InitializeComponent();
            Schedule = schedule ?? new Schedule();
            Loaded += ScheduleDialog_Loaded;
        }

        private async void ScheduleDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var dbService = new DatabaseService();
            var courseService = new CourseService(dbService);
            var slotService = new SlotService(dbService);

            var courses = await courseService.GetAllCoursesAsync();
            var slots = await slotService.GetAllSlotsAsync();

            CourseComboBox.ItemsSource = courses;
            SlotComboBox.ItemsSource = slots;

            if (Schedule != null && Schedule.Id > 0)
            {
                CourseComboBox.SelectedItem = courses.FirstOrDefault(c => c.Id == Schedule.CourseId);
                SlotComboBox.SelectedItem = slots.FirstOrDefault(s => s.Id == Schedule.SlotId);
                RoomTextBox.Text = Schedule.Room ?? "";
                DayOfWeekComboBox.SelectedItem = DayOfWeekComboBox.Items
                    .Cast<System.Windows.Controls.ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == Schedule.DayOfWeek);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CourseComboBox.SelectedItem is Course selectedCourse &&
                SlotComboBox.SelectedItem is Slot selectedSlot &&
                DayOfWeekComboBox.SelectedItem != null)
            {
                Schedule!.CourseId = selectedCourse.Id;
                Schedule.SlotId = selectedSlot.Id;
                Schedule.DayOfWeek = ((System.Windows.Controls.ComboBoxItem)DayOfWeekComboBox.SelectedItem).Content.ToString()!;
                Schedule.Room = string.IsNullOrWhiteSpace(RoomTextBox.Text) ? null : RoomTextBox.Text;
                
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

