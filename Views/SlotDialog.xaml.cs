using System;
using System.Linq;
using System.Windows;
using StudentManagement.Models;

namespace StudentManagement.Views
{
    public partial class SlotDialog : Window
    {
        public Slot? Slot { get; private set; }

        public SlotDialog(Slot? slot = null)
        {
            InitializeComponent();
            Slot = slot ?? new Slot();
            
            if (slot != null && slot.Id > 0)
            {
                NameTextBox.Text = slot.Name;
                
                var startHour = slot.StartTime.Hours;
                var startMinute = slot.StartTime.Minutes;
                var endHour = slot.EndTime.Hours;
                var endMinute = slot.EndTime.Minutes;

                StartHourComboBox.SelectedItem = StartHourComboBox.Items
                    .Cast<System.Windows.Controls.ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == startHour.ToString("00"));
                StartMinuteComboBox.SelectedItem = StartMinuteComboBox.Items
                    .Cast<System.Windows.Controls.ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == startMinute.ToString("00"));
                EndHourComboBox.SelectedItem = EndHourComboBox.Items
                    .Cast<System.Windows.Controls.ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == endHour.ToString("00"));
                EndMinuteComboBox.SelectedItem = EndMinuteComboBox.Items
                    .Cast<System.Windows.Controls.ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == endMinute.ToString("00"));
            }
            else
            {
                StartHourComboBox.SelectedIndex = 0;
                StartMinuteComboBox.SelectedIndex = 0;
                EndHourComboBox.SelectedIndex = 1;
                EndMinuteComboBox.SelectedIndex = 0;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                StartHourComboBox.SelectedItem == null ||
                StartMinuteComboBox.SelectedItem == null ||
                EndHourComboBox.SelectedItem == null ||
                EndMinuteComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var startHour = int.Parse(((System.Windows.Controls.ComboBoxItem)StartHourComboBox.SelectedItem).Content.ToString()!);
            var startMinute = int.Parse(((System.Windows.Controls.ComboBoxItem)StartMinuteComboBox.SelectedItem).Content.ToString()!);
            var endHour = int.Parse(((System.Windows.Controls.ComboBoxItem)EndHourComboBox.SelectedItem).Content.ToString()!);
            var endMinute = int.Parse(((System.Windows.Controls.ComboBoxItem)EndMinuteComboBox.SelectedItem).Content.ToString()!);

            Slot!.Name = NameTextBox.Text;
            Slot.StartTime = new TimeSpan(startHour, startMinute, 0);
            Slot.EndTime = new TimeSpan(endHour, endMinute, 0);

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

