
using MiniDashboard.App.Components.Navigation;
using MiniDashboard.App.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
namespace MiniDashboard.App.ViewModels
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        private readonly IServiceProvider _serviceProvider;

        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged();
                }
            }
        }

        public NavBarViewModel NavBarVm { get; }

        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            NavBarVm = new NavBarViewModel(this);
            CurrentView = _serviceProvider.GetRequiredService<DashboardView>();

        }
        public void NavigateTo(string viewName)
        {
            CurrentView = viewName switch
            {
                "DashboardView" => _serviceProvider.GetRequiredService<DashboardView>(),
                "ProductView" => _serviceProvider.GetRequiredService<ProductView>(),              
                _ => _serviceProvider.GetRequiredService<DashboardView>()
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
