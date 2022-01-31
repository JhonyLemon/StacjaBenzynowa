using Caliburn.Micro;
using StacjaBenzynowaMVVM.Helpers.Classes;
using StacjaBenzynowaMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.ViewModels
{
    class SaleListViewModel : Screen
    {
        private Sale _sale;
        private ObservableCollection<Sale> _sales;
        private ObservableCollection<SaleDetails> _saleDetails;
        private IEventAggregator _eventAggregator;

        public SaleListViewModel(IEventAggregator eventAggregator)
        {
            UpdateList();
            _eventAggregator = eventAggregator;
        }

        public void UpdateList()
        {
            _sales = DatabaseDataHelper.GetSales();
        }

        public void UpdateDetailsList(Sale sale)
        {
            _saleDetails = DatabaseDataHelper.GetSaleDetails(sale.ID_ZAMOWNIENIA);
        }

        public Sale Sale
        {
            get { return _sale; }
            set 
            {
                _sale = value;
                UpdateDetailsList(Sale);
                NotifyOfPropertyChange(() => Sale);
                NotifyOfPropertyChange(() => SaleDetails);
            }
        }

        public ObservableCollection<Sale> Sales
        {
            get { return _sales; }
            set
            {
                _sales = value;
                NotifyOfPropertyChange(() => Sales);
            }
        }
        public ObservableCollection<SaleDetails> SaleDetails
        {
            get { return _saleDetails; }
            set
            {
                _saleDetails = value;
                NotifyOfPropertyChange(() => SaleDetails);
            }
        }
    }
}
