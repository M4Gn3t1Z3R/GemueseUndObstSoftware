using GemueseUndObstSoftware.Enums;
using GemueseUndObstSoftware.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemueseUndObstSoftware.Models
{
    public class Article : ViewModelBase
    {
        private int _articleNumber;
        public int ArticleNumber
        {
            get { return _articleNumber; }
            set { SetProperty(ref _articleNumber, value); }
        }
        private string _articleDescription;
        public string ArticleDescription
        {
            get { return _articleDescription; }
            set { SetProperty(ref _articleDescription, value); }
        }
        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }
        private decimal _storageQuantity;
        public decimal StorageQuantity
        {
            get { return _storageQuantity; }
            set { SetProperty(ref _storageQuantity, value); }
        }
        private QuantityUnit _quantityUnit;
        public QuantityUnit QuantityUnit
        {
            get { return _quantityUnit; }
            set { SetProperty(ref _quantityUnit, value); }
        }
        private bool _selectedForAction;
        public bool SelectedForAction
        {
            get { return _selectedForAction; }
            set { SetProperty(ref _selectedForAction, value); }
        }

        public bool IsValid
        {
            get { return !string.IsNullOrWhiteSpace(ArticleDescription) && Price != 0 && ArticleNumber >= 0; }
        }
        public Article()
        {
            SelectedForAction = false;

        }
        public override string ToString()
        {
            return $"{ArticleNumber.ToString()}î{ArticleDescription}î{Price.ToString()}î{StorageQuantity.ToString()}î{QuantityUnit.ToString()}";
        }
    }
}
