using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using P04WeatherForecastAPI.Client.Models;
using P06Shop.Shared.MessageBox;
using P06Shop.Shared.Services.ProductService;
using P06Shop.Shared.Shop;
using P12MAUI.Client;
using P12MAUI.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace P04WeatherForecastAPI.Client.ViewModels
{
   
 public partial class ProductsViewModel : ObservableObject
    {
        private readonly IProductService _productService;
        private readonly ProductDetailsView _productDetailsView;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IConnectivity _connectivity;
        public ObservableCollection<Product> Products { get; set; }

        [ObservableProperty]
        private Product selectedProduct;

        public ProductsViewModel(IProductService productService, ProductDetailsView productDetailsView, IMessageDialogService messageDialogService,
            IConnectivity connectivity)
        {
            _messageDialogService = messageDialogService;
            _productDetailsView = productDetailsView;
            _productService = productService;
            _connectivity = connectivity; // set the _connectivity field
            Products = new ObservableCollection<Product>();
            GetProducts();
        }

        public async Task GetProducts()
        {
            Products.Clear();
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                _messageDialogService.ShowMessage("Internet not available!");
                return;
            }

            var productsResult = await _productService.GetProductsAsync();
            if (productsResult.Success)
            {
                foreach (var p in productsResult.Data)
                {
                    Products.Add(p);
                }
            }
            else
            {
                _messageDialogService.ShowMessage(productsResult.Message);
            }
        }

        [RelayCommand]
        public async Task ShowDetails(Product product)
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                _messageDialogService.ShowMessage("Internet not available!");
                return;
            }

            await Shell.Current.GoToAsync(nameof(ProductDetailsView), true, new Dictionary<string, object>
            {
                {"Product", product },
                {nameof(ProductsViewModel), this }
            });
            SelectedProduct = product;
        }

        [RelayCommand]
        public async Task New()
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                _messageDialogService.ShowMessage("Internet not available!");
                return;
            }

            SelectedProduct = new Product();
            await Shell.Current.GoToAsync(nameof(ProductDetailsView), true, new Dictionary<string, object>
            {
                {"Product", SelectedProduct },
                {nameof(ProductsViewModel), this }
            });
        }

    }
}
