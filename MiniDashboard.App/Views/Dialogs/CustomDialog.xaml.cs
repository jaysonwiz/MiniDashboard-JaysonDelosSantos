using MiniDashboard.App.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MiniDashboard.App.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for CustomDialog.xaml
    /// </summary>
    public partial class CustomDialog : Window
    {
        public bool? DialogResultValue { get; private set; } = null;

        public CustomDialog(CustomDialogViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            this.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    this.DragMove();
            };
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) { DialogResultValue = true; Close(); }
        private void CancelButton_Click(object sender, RoutedEventArgs e) { DialogResultValue = false; Close(); }
        private void YesButton_Click(object sender, RoutedEventArgs e) { DialogResultValue = true; Close(); }
        private void NoButton_Click(object sender, RoutedEventArgs e) { DialogResultValue = false; Close(); }
    }
}
