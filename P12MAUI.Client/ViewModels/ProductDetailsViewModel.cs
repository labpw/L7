using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using P04WeatherForecastAPI.Client.ViewModels;
using P06Shop.Shared.MessageBox;
using P06Shop.Shared.Services.ProductService;
using P06Shop.Shared.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P12MAUI.Client.ViewModels
{
    [QueryProperty(nameof(Product), nameof(Product))]
    [QueryProperty(nameof(ProductsViewModel), nameof(ProductsViewModel))]
    public partial class ProductDetailsViewModel : ObservableObject
    {
        private readonly IProductService _productService;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IGeolocation _geolocation;
        private readonly IMap _map;
        private ProductsViewModel _productViewModel;

        public ProductDetailsViewModel(IProductService productService, IMessageDialogService messageDialogService, IGeolocation geolocation, IMap map)
        {
            _map = map;
            _productService = productService;
            _messageDialogService = messageDialogService;
            _geolocation = geolocation;

            
        }




        [RelayCommand]
        public async Task GetLocation()
        {
            var location = await _geolocation.GetLastKnownLocationAsync();

            try
            {
                await _map.OpenAsync(52.23564245654427, 21.0112328246909, new MapLaunchOptions
                {
                    Name = "ALX",
                    NavigationMode = NavigationMode.None
                });
            }
            catch (Exception)
            {
                 _messageDialogService.ShowMessage("Error opening map");
            }
            
        }

        public ProductsViewModel ProductsViewModel
        {
            get
            {
                return _productViewModel;
            }
            set
            {
                _productViewModel = value;
            }
        }


        [ObservableProperty]
        Product product;

        public async Task DeleteProduct()
        {
            await _productService.DeleteProductAsync(product.Id);
            await _productViewModel.GetProducts();
        }

        public async Task CreateProduct()
        {
            var newProduct = new Product()
            {
                Title = product.Title,
                Description = product.Description,
                Barcode = product.Barcode,
                Price = product.Price,
                ReleaseDate = product.ReleaseDate,
            };

            var result = await _productService.CreateProductAsync(newProduct);
            if (result.Success)
                await _productViewModel.GetProducts();
            else
                _messageDialogService.ShowMessage(result.Message);
        }

        public async Task UpdateProduct()
        {
            var productToUpdate = new Product()
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Barcode = product.Barcode,
                Price = product.Price,
                ReleaseDate = product.ReleaseDate,
            };

            await _productService.UpdateProductAsync(productToUpdate);
            await _productViewModel.GetProducts();
        }


        [RelayCommand]
        public async Task Save()
        {
            if (product.Id == 0)
            {
                CreateProduct();
                await Shell.Current.GoToAsync("../", true);

            }
            else
            {
                UpdateProduct();
                await Shell.Current.GoToAsync("../", true);
            }

        }

        [RelayCommand]
        public async Task Delete()
        {
            DeleteProduct();
            await Shell.Current.GoToAsync("../", true);
        }
    }
}
