using GemueseUndObstSoftware.Enums;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace GemueseUndObstSoftware.Models
{
    public class Storage : GemueseUndObstSoftware.MVVM.ViewModelBase
    {
        private ObservableCollection<Article> _articles = new ObservableCollection<Article>(); //this is only used as a backing field, which is required for MVVM
        public ObservableCollection<Article> ArticleStock
        {
            get { return _articles; }
            set { SetProperty(ref _articles, value); }
        }

        public Storage()
        {

        }

        public void BookOut(decimal quantity, int articleNumber)
        {
            try
            {
                Article article = ArticleStock.Where(a => a.ArticleNumber == articleNumber).Single();
                if (article.StorageQuantity > quantity)
                {
                    article.StorageQuantity -= quantity;
                }
                else
                {
                    article.StorageQuantity = 0;
                }
            }
            catch (Exception e)
            {
                //the result was != 1
                throw e;
            }
        }
        public void BookIn(decimal quantity, int articleNumber)
        {
            try
            {
                ArticleStock.Where(a => a.ArticleNumber == articleNumber).Single().StorageQuantity += quantity;
            }
            catch (Exception e)
            {
                //the result was != 1
                throw e;
            }
        }
        public void CreateArticle(int articleNumber, string articleDesctiption, QuantityUnit quantityUnit, decimal price)
        {
            try
            {
                if (ArticleStock.Where(a => a.ArticleNumber == articleNumber).Count() == 0)
                {
                    Article newArticle = new Article();
                    newArticle.ArticleNumber = articleNumber;
                    newArticle.ArticleDescription = articleDesctiption;
                    newArticle.Price = price;
                    newArticle.QuantityUnit = quantityUnit;
                    newArticle.StorageQuantity = 0;
                    ArticleStock.Add(newArticle);
                    ArticleStock = new ObservableCollection<Article>(ArticleStock.OrderBy(a => a.ArticleNumber));
                }
                else
                {
                    //articleNumber already exists, which should not be possible
                }
            }
            catch (Exception e)
            {
                //various exceptions
                throw e;
            }
        }
        public void ChangePrice(int articleNumber, decimal newPrice)
        {
            try
            {
                ArticleStock.Where(a => a.ArticleNumber == articleNumber).Single().Price = newPrice;
            }
            catch (Exception e)
            {
                //the result was != 1
                throw e;
            }
        }
        public void DeleteArticle(int articleNumber)
        {
            try
            {
                ArticleStock.Remove(ArticleStock.Where(a => a.ArticleNumber == articleNumber).Single());
            }
            catch (Exception e)
            {
                //the result was != 1
                throw e;
            }
        }


        public decimal GetAmountOfArticle(int articleNumber)
        {
            try
            {
                return ArticleStock.Where(a => a.ArticleNumber == articleNumber).Single().StorageQuantity;
            }
            catch (Exception e)
            {
                //the result was != 1
                throw e;
            }
        }
    }
}
