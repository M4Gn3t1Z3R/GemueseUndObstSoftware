using GemueseUndObstSoftware.Models;
using GemueseUndObstSoftware.MVVM;
using GemueseUndObstSoftware.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemueseUndObstSoftware.ViewModels
{
    class ArticleCreationViewModel: ViewModelBase
    {
        private Article _article;
        public Article Article
        {
            get { return _article; }
            set { SetProperty(ref _article, value); }
        }

        private List<QuantityUnit> _quantityUnits;
        public List<QuantityUnit> QuantityUnits
        {
            get { return _quantityUnits; }
            set { SetProperty(ref _quantityUnits, value); }
        }

        public ArticleCreationViewModel()
        {
            //QuantityUnits = Enum.GetValues(typeof(QuantityUnit));
        }
    }
}
