using System.Collections.ObjectModel;
using System.Linq;
using AppName.Core;

namespace AppName
{
    public class OrderConfirmationViewModel : ObservableObject
    {
        private int _quantity = 1;
        private bool _isGift;
        private FlowProductData _product;
        private ShippingOptionData _optionSelected;
        private string _paymentMethod, _shippingAddress;

        public OrderConfirmationViewModel(FlowProductData product)
        {
            LoadData();

            if (product != null)
            {
                Product = product;
            }

            if (Product != null)
            {
                Quantity = Product.SelectedQuantity;
            }
        }

        public FlowProductData Product
        { 
            get { return _product; }
            set { SetProperty(ref _product, value); }
        }

        public ObservableCollection<ShippingOptionData> ShippingOptions { get; } = new ObservableCollection<ShippingOptionData>();

        public double Subtotal => Product != null ? Product.Price * Quantity : 0;

        public double Total => Subtotal + OptionSelected?.Price ?? 0;

        public string ShippingAddress
        { 
            get { return _shippingAddress; }
            set { SetProperty(ref _shippingAddress, value); }
        }
        
        public string PaymentMethod
        { 
            get { return _paymentMethod; }
            set { SetProperty(ref _paymentMethod, value); }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (SetProperty(ref _quantity, value))
                {
                    NotifyPropertyChanged(nameof(Subtotal));
                }
            }
        }

        public bool IsGift
        {
            get { return _isGift; }
            set { SetProperty(ref _isGift, value); }
        }

        public ShippingOptionData OptionSelected
        {
            get { return _optionSelected; }
            set 
            {
                if (SetProperty(ref _optionSelected, value))
                {
                    NotifyPropertyChanged(nameof(Total));
                }
            }
        }

        private void LoadData()
        {
            ShippingOptions.Clear();

            // This viewmodel is reused in the last page of the flow as they share most of the data
            JsonHelper.Instance.LoadViewModel(this, pageName: "CheckoutPage.xaml", source: "EcommerceFlow.json");

            OptionSelected = ShippingOptions.FirstOrDefault();
        }
    }

    public class ShippingOptionData
    {
        public string Option { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
