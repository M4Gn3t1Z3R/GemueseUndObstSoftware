using GemueseUndObstSoftware.Enums;
using GemueseUndObstSoftware.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemueseUndObstSoftware.Models
{
    public class ArticleDisplay : ViewModelBase
    {
        private Article _article;
        public Article Article
        {
            get { return _article; }
            set { SetProperty(ref _article, value); }
        }

        private bool _selectedForAction;
        public bool SelectedForAction
        {
            get { return _selectedForAction; }
            set { SetProperty(ref _selectedForAction, value); }
        }

        public ArticleDisplay()
        {
            SelectedForAction = false;
        }

        public ArticleDisplay(Article article)
        {
            this.Article = article;
            SelectedForAction = false;
        }

        public static List<ArticleDisplay> GetAsArticleDisplays(List<Article> articles)
        {
            List<ArticleDisplay> returner = new List<ArticleDisplay>();
            foreach (Article article in articles)
            {
                returner.Add(new ArticleDisplay(article));
            }
            return returner;
        }

    }
}
