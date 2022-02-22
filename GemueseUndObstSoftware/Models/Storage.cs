using GemueseUndObstSoftware.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GemueseUndObstSoftware.Models
{
    public class Storage : GemueseUndObstSoftware.MVVM.ViewModelBase
    {
        private ObservableCollection<Article> _articles = new ObservableCollection<Article>();
        public ObservableCollection<Article> Articles
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
                Articles.Where(a => a.ArticleNumber == articleNumber).Single().StorageQuantity += quantity;
            }
            catch(Exception e)
            {
                //the result was != 1
                throw e;
            }
        }
        public void BookIn(decimal quantity, int articleNumber)
        {
            try
            {
                Articles.Where(a => a.ArticleNumber == articleNumber).Single().StorageQuantity -= quantity;
            }
            catch(Exception e)
            {
                //the result was != 1
                throw e;
            }
        }
        public void CreateArticle(Article article)
        {
            CreateArticle(article.ArticleNumber, article.ArticleDescription, article.QuantityUnit, article.Price);
        }
        public void CreateArticle(int articleNumber, string articleDesctiption, QuantityUnit quantityUnit, decimal price)
        {
            try
            {
                if(Articles.Where(a => a.ArticleNumber == articleNumber).Count() == 0)
                {
                    Article newArticle = new Article();
                    newArticle.ArticleNumber = articleNumber;
                    newArticle.ArticleDescription = articleDesctiption;
                    newArticle.Price = price;
                    newArticle.QuantityUnit = quantityUnit;
                    newArticle.StorageQuantity = 0;
                    Articles.Add(newArticle);
                    Articles = new ObservableCollection<Article>(Articles.OrderBy(a => a.ArticleNumber));
                }
                else
                {
                    //articleNumber already exists
                }
            }
            catch(Exception e)
            {
                //various exceptions
                throw e;
            }
        }
        public void ChangePrice(int articleNumber, decimal newPrice)
        {
            try
            {
                Articles.Where(a => a.ArticleNumber == articleNumber).Single().Price = newPrice;
            }
            catch(Exception e)
            {
                //the result was != 1
                throw e;
            }
        }
        public void DeleteArticle(int articleNumber)
        {
            try
            {
                Articles.Remove(Articles.Where(a => a.ArticleNumber == articleNumber).Single());
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
                return Articles.Where(a => a.ArticleNumber == articleNumber).Single().StorageQuantity;
            }
            catch(Exception e)
            {
                //the result was != 1
                throw e;
            }
        }
    }
}
