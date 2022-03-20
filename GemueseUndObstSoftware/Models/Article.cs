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
        private int _articleNumber; //this is only used as a backing field, which is required for MVVM
        public int ArticleNumber
        {
            get { return _articleNumber; }
            set { SetProperty(ref _articleNumber, value); }
        }
        private string _articleDescription;//this is only used as a backing field, which is required for MVVM
        public string ArticleDescription
        {
            get { return _articleDescription; }
            set { SetProperty(ref _articleDescription, value); }
        }
        private decimal _price; //this is only used as a backing field, which is required for MVVM
        public decimal Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }
        private decimal _storageQuantity; //this is only used as a backing field, which is required for MVVM
        public decimal StorageQuantity
        {
            get { return _storageQuantity; }
            set { SetProperty(ref _storageQuantity, value); }
        }
        private QuantityUnit _quantityUnit; //this is only used as a backing field, which is required for MVVM
        public QuantityUnit QuantityUnit
        {
            get { return _quantityUnit; }
            set { SetProperty(ref _quantityUnit, value); }
        }

        public Article()
        {

        }

        public Article(int articleNumber, string articleDescription, QuantityUnit quantityUnit, decimal price)
        {
            ArticleNumber = articleNumber;
            ArticleDescription = articleDescription;
            QuantityUnit = quantityUnit;
            Price = price;
        }
        public override string ToString()
        {
            return $"{ArticleNumber.ToString()}î{ArticleDescription}î{Price.ToString()}î{StorageQuantity.ToString()}î{QuantityUnit.ToString()}";
        }
    }
}
