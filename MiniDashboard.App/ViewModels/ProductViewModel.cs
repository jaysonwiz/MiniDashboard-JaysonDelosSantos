using Entities;
using MiniDashboard.App.Commands;
using MiniDashboard.App.Enums;
using MiniDashboard.App.Services;
using MiniDashboard.App.Views;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MiniDashboard.App.ViewModels
{
    public partial class ProductViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _client;
        private readonly IDialogService _dialogService;

        public ICommand SearchProductCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }


        public ProductViewModel(HttpClient client,IDialogService dialogService)   
        {
            _client = client;
            _dialogService = dialogService;

            SearchProductCommand = new RelayCommand(async () => await SearchProductAsync());
            AddProductCommand = new RelayCommand(() => AddProduct());
            EditProductCommand = new RelayCommand(() => EditProduct());
            DeleteProductCommand = new RelayCommand(async () => await DeleteProductAsync());

            // Load tenants on ViewModel initialization
            _ = LoadProductsAsync();
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }


        private ObservableCollection<ProductResponse> _products;
        public ObservableCollection<ProductResponse> Products
        {
            get => _products;
            set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged();
                }
            }
        }
        private ProductResponse _selectedProduct;
        public ProductResponse SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool CanEditOrDelete() => SelectedProduct != null;

        private async Task SearchProductAsync()
        {
            try
            {
                IsLoading = true;
                await Task.Delay(2000);
                var response = await _client.GetAsync("/Products/Search?query=" + SearchText);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var ProductsResponse = await response.Content.ReadFromJsonAsync<List<ProductResponse>>();
                    Products = new ObservableCollection<ProductResponse>(ProductsResponse);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowCustomDialog(
                   "Error",
                   $"Error loading products: {ex.Message}",
                   DialogType.Error, Application.Current.MainWindow);
            }
            finally
            {
                IsLoading = false;
            }
            
        }
        private async Task LoadProductsAsync()
        {
            try
            {
                IsLoading = true;

                await Task.Delay(5000);
                var response = await _client.GetAsync("/Products/GetAll");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var ProductsResponse = await response.Content.ReadFromJsonAsync<List<ProductResponse>>();
                    Products = new ObservableCollection<ProductResponse>(ProductsResponse);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowCustomDialog(
                   "Error",
                   $"Error loading products: {ex.Message}",
                   DialogType.Error, Application.Current.MainWindow);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AddProduct()
        {
            var addViewModel = new ProductAddEditViewModel(_client, _dialogService);
            addViewModel.PageTitle = "Add Product Dialog";


            // Show the modal dialog
            if (_dialogService.ShowDialog<ProductAddEditView, ProductAddEditViewModel>(addViewModel,210))
            {
                Products.Add(addViewModel.ProductResponse);

                _dialogService.ShowCustomDialog(
                    "Success",
                    $"Product '{addViewModel.ProductName}' has been added successfully!",
                    DialogType.Information, Application.Current.MainWindow );
            }
            else
            {
                if (addViewModel.HasError)
                {
                    _dialogService.ShowCustomDialog(
                        "Error",
                        $"{addViewModel.ErrorMessage}",
                        DialogType.Error, Application.Current.MainWindow);
                }
            }
        }
        private void EditProduct()
        {
            var editViewModel = new ProductAddEditViewModel(_client, _dialogService);
            editViewModel.PageTitle = "Edit Product Dialog";
            editViewModel.ProductName = SelectedProduct.ProductName;
            editViewModel.ProductResponse = SelectedProduct;
            editViewModel.IsEdit = true;

            // Show the modal dialog
            if (_dialogService.ShowDialog<ProductAddEditView, ProductAddEditViewModel>(editViewModel, 210))
            {
                var index = Products.IndexOf(SelectedProduct);
                if (index >= 0)
                    Products[index] = editViewModel.ProductResponse;

                _dialogService.ShowCustomDialog(
                "Success",
                $"Product '{editViewModel.ProductName}' has been modified successfully!",
                DialogType.Information, Application.Current.MainWindow
            );
            }
            else
            {
                if (editViewModel.HasError)
                {
                    _dialogService.ShowCustomDialog(
                        "Error",
                        $"{editViewModel.ErrorMessage}",
                        DialogType.Error, Application.Current.MainWindow);
                }
            }
        }


        private async Task DeleteProductAsync()
        {
            if (SelectedProduct == null) return;
            string productName = SelectedProduct.ProductName;
            var confirm = _dialogService.ShowCustomDialog("Delete Product?", $"Are you sure you want to delete {SelectedProduct.ProductName}?", DialogType.Question, Application.Current.MainWindow);

            if (confirm != true) return;

            var response = await _client.DeleteAsync($"/Products/Delete/{SelectedProduct.ProductID}");
           
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var deleteResult = await response.Content.ReadFromJsonAsync<bool>();
                if(deleteResult)
                {
                    Products.Remove(SelectedProduct);
                    _dialogService.ShowCustomDialog(
                        "Success",
                        $"Product '{productName}' has been deleted successfully!",
                        DialogType.Information, Application.Current.MainWindow);
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
