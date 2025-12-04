using MiniDashboard.App.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MiniDashboard.App.Services
{
    public interface IDialogService
    {
        /// <summary>
        /// Shows a modal dialog for a specific View and ViewModel.
        /// </summary>
        /// <typeparam name="TView">The Window type to display.</typeparam>
        /// <typeparam name="TViewModel">The ViewModel type to set as DataContext.</typeparam>
        /// <param name="viewModel">The ViewModel instance.</param>
        /// <returns>True if the dialog was accepted (DialogResult == true), otherwise false.</returns>
        bool ShowDialog<TView, TViewModel>(TViewModel viewModel,double Height)
            where TView : Window, new()
            where TViewModel : class;

        /// <summary>
        /// Shows a reusable CustomDialog with your predefined styles.
        /// Supports specifying the owner window (useful for nested modals).
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="message">Message to display</param>
        /// <param name="type">Dialog type (Information, Warning, Error, Question, Custom)</param>
        /// <param name="owner">Optional owner window. If null, MainWindow is used.</param>
        /// <returns>Dialog result (true/false/null)</returns>
        bool? ShowCustomDialog(string title, string message, DialogType type = DialogType.Information, Window owner = null);
    }
}
