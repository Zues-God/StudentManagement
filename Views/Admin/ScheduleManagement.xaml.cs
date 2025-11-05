using StudentManagement.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StudentManagement.Views.Admin
{
    public partial class ScheduleManagement : Window
    {
        private readonly AppDbContext _context = new AppDbContext();

        public ScheduleManagement()
        {
            InitializeComponent();
            this.Loaded += ScheduleManagement_Loaded;
        }

        private void ScheduleManagement_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCourses();
            LoadSlots();        
            LoadSchedules();
        }

        // 🧩 Load danh sách môn học
        private void LoadCourses()
        {
            var courses = _context.Courses.ToList();
            cbCourse.ItemsSource = courses;
            cbCourse.DisplayMemberPath = "Name";
            cbCourse.SelectedValuePath = "Id";
        }

        // Load danh sách Slot (hiển thị giờ học)
        private void LoadSlots()
        {
            var slots = _context.Slots
                .Select(s => new
                {
                    s.Id,
                    SlotDisplay = $"Slot {s.SlotNumber}"
                })
                .ToList();

            cbSlot.ItemsSource = slots;
            cbSlot.DisplayMemberPath = "SlotDisplay";
            cbSlot.SelectedValuePath = "Id";
            if (cbSlot.Items.Count > 0) cbSlot.SelectedIndex = 0;
        }


        // Load danh sách Schedule
        private void LoadSchedules()
        {
            var schedules = from s in _context.Schedules
                            join c in _context.Courses on s.CourseId equals c.Id
                            join sl in _context.Slots on s.SlotId equals sl.Id
                            select new
                            {
                                s.Id,
                                s.CourseId, 
                                Course = c.Name,
                                s.DayOfWeek,
                                s.SlotId,   
                                Slot = $"Slot {sl.SlotNumber}",
                                s.Room,
                                s.Notes
                            };

            dgSchedules.ItemsSource = schedules.ToList();
        }


        // Thêm mới Schedule
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbCourse.SelectedValue == null)
                {
                    MessageBox.Show("Please select a course.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cbSlot.SelectedValue == null)
                {
                    MessageBox.Show("Please select a slot.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //  Lấy dữ liệu đang chọn
                int courseId = (int)cbCourse.SelectedValue;
                string dayOfWeek = ((ComboBoxItem)cbDayOfWeek.SelectedItem).Content.ToString();
                int slotId = (int)cbSlot.SelectedValue;

                // Kiểm tra trùng (Course + DayOfWeek + Slot)
                bool exists = _context.Schedules.Any(s =>
                    s.CourseId == courseId &&
                    s.DayOfWeek == dayOfWeek &&
                    s.SlotId == slotId);

                if (exists)
                {
                    MessageBox.Show("⚠️ This schedule already exists for the same Course, Day, and Slot.",
                                    "Duplicate Schedule", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Nếu không trùng -> thêm mới
                var schedule = new Schedule
                {
                    CourseId = courseId,
                    DayOfWeek = dayOfWeek,
                    SlotId = slotId,
                    Room = txtRoom.Text,
                    Notes = txtNotes.Text,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Schedules.Add(schedule);
                _context.SaveChanges();

                MessageBox.Show("✅ Schedule added successfully!",
                                "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadSchedules();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error adding schedule:\n\n{ex.InnerException?.Message ?? ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Chỉnh sửa Schedule
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgSchedules.SelectedItem == null)
            {
                MessageBox.Show("Please select a schedule to edit.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                dynamic selected = dgSchedules.SelectedItem;
                int scheduleId = selected.Id;
                var schedule = _context.Schedules.Find(scheduleId);

                if (schedule != null)
                {
                    schedule.CourseId = (int)cbCourse.SelectedValue;
                    schedule.DayOfWeek = ((ComboBoxItem)cbDayOfWeek.SelectedItem).Content.ToString();
                    schedule.SlotId = (int)cbSlot.SelectedValue;
                    schedule.Room = txtRoom.Text;
                    schedule.Notes = txtNotes.Text;
                    schedule.UpdatedAt = DateTime.Now;

                    _context.SaveChanges();
                    MessageBox.Show("✅ Schedule updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadSchedules();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error editing schedule:\n\n{ex.InnerException?.Message ?? ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xóa Schedule
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgSchedules.SelectedItem == null)
            {
                MessageBox.Show("Please select a schedule to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            dynamic selected = dgSchedules.SelectedItem;
            int scheduleId = selected.Id;

            var confirm = MessageBox.Show($"Are you sure you want to delete schedule ID {scheduleId}?",
                                          "Confirm Delete",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);

            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    var schedule = _context.Schedules.Find(scheduleId);
                    if (schedule != null)
                    {
                        _context.Schedules.Remove(schedule);
                        _context.SaveChanges();
                        MessageBox.Show("🗑️ Schedule deleted successfully!", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadSchedules();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Error deleting schedule:\n\n{ex.InnerException?.Message ?? ex.Message}",
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void dgSchedules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgSchedules.SelectedItem == null)
                return;

            try
            {
                dynamic selected = dgSchedules.SelectedItem;

                // ✅ Cập nhật thông tin lên form
                // Course
                cbCourse.SelectedItem = cbCourse.Items.Cast<dynamic>()
                    .FirstOrDefault(c => c.Id == selected.CourseId);

                // Day of Week
                foreach (ComboBoxItem item in cbDayOfWeek.Items)
                {
                    if (item.Content.ToString() == selected.DayOfWeek)
                    {
                        cbDayOfWeek.SelectedItem = item;
                        break;
                    }
                }

                // Slot
                cbSlot.SelectedItem = cbSlot.Items.Cast<dynamic>()
                    .FirstOrDefault(s => s.Id == selected.SlotId);

                // Room & Notes
                txtRoom.Text = selected.Room;
                txtNotes.Text = selected.Notes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying schedule detail:\n\n{ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtNotes_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
