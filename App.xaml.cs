using System.Windows;
using StudentManagement.Services;

namespace StudentManagement
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Handle unhandled exceptions
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            // Initialize default slots on startup
            try
            {
                var dbService = new DatabaseService();
                var slotService = new SlotService(dbService);
                await slotService.InitializeDefaultSlotsAsync();
            }
            catch
            {
                // Ignore errors during slot initialization
            }
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"An error occurred: {e.Exception.Message}\n\n{e.Exception.StackTrace}", 
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}

