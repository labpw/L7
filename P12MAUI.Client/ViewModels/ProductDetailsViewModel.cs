using CommunityToolkit.Mvvm.ComponentModel;
using P06Shop.Shared.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P12MAUI.Client.ViewModels
{
    [QueryProperty(nameof(Product), nameof(Product))]
    public partial class  ProductDetailsViewModel : ObservableObject
    {

        [ObservableProperty]
        Product product;

    }
}
