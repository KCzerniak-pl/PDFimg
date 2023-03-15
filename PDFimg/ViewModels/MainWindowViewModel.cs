using PDFimg.Views;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Input;

namespace PDFimg.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        // Button for minimize app.
        private ICommand? _minimizeAppCommand;
        public ICommand MinimizeAppCommand { get => _minimizeAppCommand ?? (_minimizeAppCommand = new DelegateCommand(() => SystemCommands.MinimizeWindow(Application.Current.MainWindow))); }

        // Button for close app.
        private ICommand? _closeAppCommand;
        public ICommand CloseAppCommand { get => _closeAppCommand ?? (_closeAppCommand = new DelegateCommand(() => Application.Current.Shutdown())); }

        // Active view.
        private object _currentView = null!;
        public object CurrentView
        {
            get { return _currentView; }
            set { SetProperty(ref _currentView, value); }
        }

        // Marker for selected option in the menu.
        private Thickness _slidebarMarkMargin;
        public Thickness SlidebarMarkMargin
        {
            get { return _slidebarMarkMargin; }
            set { SetProperty(ref _slidebarMarkMargin, value); }
        }

        // Selecting an item in the menu.
        public int SidebarSelectedIndex
        {
            set
            {
                SlidebarMarkMargin = new Thickness(0, 65 * value, 0, 0);
            }
        }
    }
}
