using MiniDashboard.App.Commands;
using MiniDashboard.App.Models;
using MiniDashboard.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace MiniDashboard.App.Components.Navigation
{
    public partial class NavBarViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _main;

        public NavBarViewModel(MainViewModel main)
        {
            _main = main;
            MenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem { Title = "Dashboard", ViewName = "DashboardView", IsChecked = true },
                new MenuItem { Title = "Product", ViewName = "ProductView", IsChecked = false },
            };

            NavigateCommand = new RelayCommand<string>(Navigate);
        }


        public ICommand NavigateCommand { get; set; }


        private void Navigate(string viewName)
        {
            // Update checked state
            foreach (var item in MenuItems)
            {
                item.IsChecked = item.ViewName == viewName;
            }

            // Call MainViewModel to update content
            _main.NavigateTo(viewName);
        }

        #region INotifyPropertyChanged Implementation
        public ObservableCollection<MenuItem> MenuItems { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion
    }
}
