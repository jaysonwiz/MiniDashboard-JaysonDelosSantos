using MiniDashboard.App.Enums;
using MiniDashboard.App.ViewModels.Dialogs;
using MiniDashboard.App.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace MiniDashboard.App.Services
{
    public class DialogService : IDialogService
    {
        private const double OverlayOpacity = 0.5;
        private const int FadeDurationMs = 150;

        private void ShowOverlay(Grid overlay)
        {
            if (overlay == null) return;

            // Clear any previous animation
            overlay.BeginAnimation(UIElement.OpacityProperty, null);
            overlay.Visibility = Visibility.Visible;
            overlay.Opacity = 0;

            var fadeIn = new DoubleAnimation(OverlayOpacity, TimeSpan.FromMilliseconds(FadeDurationMs));
            overlay.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        private void HideOverlay(Grid overlay)
        {
            if (overlay == null) return;

            overlay.BeginAnimation(UIElement.OpacityProperty, null);

            var fadeOut = new DoubleAnimation(0, TimeSpan.FromMilliseconds(FadeDurationMs));
            fadeOut.Completed += (s, e) => overlay.Visibility = Visibility.Collapsed;
            overlay.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        public bool ShowDialog<TView, TViewModel>(TViewModel viewModel,double Height)
           where TView : Window, new()
           where TViewModel : class
        {
            var owner = Application.Current.MainWindow;
            var overlay = (Grid)owner.FindName("Overlay");

            var window = new TView
            {
                Height = Height,
                DataContext = viewModel,
                Owner = owner,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            try
            {
                ShowOverlay(overlay);

                bool? result = window.ShowDialog();
                return result == true;
            }
            finally
            {
                HideOverlay(overlay);
            }
        }

        public bool? ShowCustomDialog(string title, string message, DialogType type = DialogType.Information, Window owner = null)
        {
            var dialogOwner = owner ?? Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            if (dialogOwner == null) return null;

            var overlay = (Grid)dialogOwner.FindName("Overlay");

            var dialogVm = new CustomDialogViewModel
            {
                Title = title,
                Message = message,
                Type = type
            };

            var dialog = new CustomDialog(dialogVm)
            {
                Owner = dialogOwner,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };


            try
            {
                ShowOverlay(overlay);

                // Show dialog (blocking)
                dialog.ShowDialog();
                bool? result = dialog.DialogResultValue;

                // Hide overlay instantly after dialog closes
                if (overlay != null)
                {
                    overlay.Visibility = Visibility.Collapsed;
                    overlay.Opacity = 0;
                }
                return result;
            }
            finally
            {
                HideOverlay(overlay);
            }


        }
    }
}
