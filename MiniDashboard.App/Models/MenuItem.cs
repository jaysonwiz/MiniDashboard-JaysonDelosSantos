
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MiniDashboard.App.Models
{

    public partial class MenuItem : INotifyPropertyChanged
    {
        private string _title = "";
        private string _viewName = "";
        private bool _isChecked;
        //private ImageSource? _icon;

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string ViewName
        {
            get => _viewName;
            set { _viewName = value; OnPropertyChanged(); }
        }

        public bool IsChecked
        {
            get => _isChecked;
            set { _isChecked = value; OnPropertyChanged(); }
        }

        // Use ImageSource instead of Image
        //public ImageSource? Icon
        //{
        //    get => _icon;
        //    set { _icon = value; OnPropertyChanged(); }
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
