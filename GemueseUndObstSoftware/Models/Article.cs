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
        private string _articleName;
        public string ArticleName 
        { 
            get { return _articleName; }
            set { SetProperty(ref _articleName, value); }
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
        public Article()
        {

        }
        public override string ToString()
        {
            return $"{ArticleNumber.ToString()}î{ArticleName}î{ArticleDescription}î{Price.ToString()}î{StorageQuantity.ToString()}î{QuantityUnit.ToString()}";
        }
    }
}
