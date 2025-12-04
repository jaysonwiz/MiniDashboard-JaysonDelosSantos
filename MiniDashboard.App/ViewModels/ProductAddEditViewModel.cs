using Entities;
using MiniDashboard.App.Commands;
using MiniDashboard.App.Services;
using MiniDashboard.App.Views;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MiniDashboard.App.ViewModels
{
    public partial class ProductAddEditViewModel : INotifyPropertyChanged
    {
        private readonly IDialogService _dialogService;
        private readonly HttpClient _client;

        public ICommand SaveProductCommand { get; }
        public ICommand CloseProductCommand { get; }
        public ProductAddEditViewModel(HttpClient client, IDialogService dialogService)
        {
            _client = client;
            _dialogService = dialogService;

            CloseProductCommand = new RelayCommand(() => CloseProduct());
            SaveProductCommand = new RelayCommand(async () => await SaveProduct());
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private string _productName;
        public string ProductName
        {
            get => _productName;
            set
            {
                if (_productName != value)
                {
                    _productName = value;
                    OnPropertyChanged();
                }
            }

        }

        private string _pageTitle;
        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                if (_pageTitle != value)
                {
                    _pageTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _hasError;
        public bool HasError
        {
            get => _hasError;
            set
            {
                if (_hasError != value)
                {
                    _hasError = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        private ProductAddRequest _productAddRequest;
        public ProductAddRequest ProductAddRequest
        {
            get => _productAddRequest;
            set
            {
                if (_productAddRequest != value)
                {
                    _productAddRequest = value;
                    OnPropertyChanged();
                }
            }
        }

        private  bool _isEdit;
        public bool IsEdit
        {
            get => _isEdit;
            set
            {
                if (_isEdit != value)
                {
                    _isEdit = value;
                    OnPropertyChanged();
                }
            }
        }

        private ProductUpdateRequest _productUpdateRequest;
        public ProductUpdateRequest ProductUpdateRequest
        {
            get => _productUpdateRequest;
            set
            {
                if (_productUpdateRequest != value)
                {
                    _productUpdateRequest = value;
                    OnPropertyChanged();
                }
            }
        }
        private ProductResponse _productResponse;
        public ProductResponse ProductResponse
        {
            get => _productResponse;
            set
            {
                if (_productResponse != value)
                {
                    _productResponse = value;
                    OnPropertyChanged();
                }
            }
        }
        private async Task SaveProduct()
        {
            var window = Application.Current.Windows.OfType<ProductAddEditView>().FirstOrDefault();
            try
            {
                if (!IsEdit)
                {
                    ProductAddRequest request = new ProductAddRequest();
                    request.ProductName = ProductName;

                    var response = await _client.PostAsJsonAsync("/Products/Create", request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        ProductResponse = await response.Content.ReadFromJsonAsync<ProductResponse>();
                        window.DialogResult = true;
                        CloseProduct();
                    }
                    else
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        ErrorMessage = JsonDocument.Parse(json).RootElement.GetProperty("error").GetString();
                        HasError = true;    
                       
                        window.DialogResult = false;
                        CloseProduct();
                    }
                }
                else
                {
                    ProductUpdateRequest request = new ProductUpdateRequest();
                    request.ProductID = ProductResponse.ProductID;
                    request.ProductName = ProductName;

                    var response = await _client.PutAsJsonAsync($"/Products/Update/{request.ProductID}", request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        ProductResponse = await response.Content.ReadFromJsonAsync<ProductResponse>();
                        window.DialogResult = true;
                        CloseProduct();
                    }
                    else
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        ErrorMessage = JsonDocument.Parse(json).RootElement.GetProperty("error").GetString();
                        HasError = true;

                        window.DialogResult = false;
                        CloseProduct();
                    }
                }
                
            }
            catch (Exception ex)
            {
                _dialogService.ShowCustomDialog("Error Saving Product", ex.Message, Enums.DialogType.Error, window);
            }
        }
        private void CloseProduct()
        {
            var window = Application.Current.Windows.OfType<ProductAddEditView>().FirstOrDefault();
            window?.Close();
        }

    }
}
