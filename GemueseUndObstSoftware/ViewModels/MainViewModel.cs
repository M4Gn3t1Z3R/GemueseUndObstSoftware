using GemueseUndObstSoftware.Enums;
using GemueseUndObstSoftware.Models;
using GemueseUndObstSoftware.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace GemueseUndObstSoftware.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private static readonly string DataLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GemueseUndObstApplicationData");
        private static readonly string ArticleDataLocation = Path.Combine(DataLocation, "Articles");

        private Storage _storage = new Storage();
        public Storage Storage
        {
            get { return _storage; }
            set { SetProperty(ref _storage, value); }
        }

        private Article _article = new Article();
        public Article Article
        {
            get { return _article; }
            set { SetProperty(ref _article, value); }
        }

        private ObservableCollection<QuantityUnit> _quantityUnits;
        public ObservableCollection<QuantityUnit> QuantityUnits
        {
            get { return _quantityUnits; }
            set { SetProperty(ref _quantityUnits, value); }
        }

        private ObservableCollection<Article> _filteredArticles;
        public ObservableCollection<Article> FilteredArticles
        {
            get { return _filteredArticles; }
            set { SetProperty(ref _filteredArticles, value); }
        }

        private decimal _quantity;
        public decimal Quantity
        {
            get { return _quantity; }
            set { SetProperty(ref _quantity, value); }
        }

        private bool _isArticleCreationEnabled;
        public bool IsArticleCreationEnabled
        {
            get { return _isArticleCreationEnabled; }
            set { SetProperty(ref _isArticleCreationEnabled, value); }
        }

        private string _searchQuery;
        public string SearchQuery
        {
            get { return _searchQuery; }
            set 
            {
                SetProperty(ref _searchQuery, value);
                if (string.IsNullOrWhiteSpace(SearchQuery))
                {
                    FilteredArticles = new ObservableCollection<Article>(Storage.Articles.OrderBy(a => a.ArticleNumber));
                }
                else
                {
                    FilteredArticles = new ObservableCollection<Article>(Storage.Articles.Where(a => typeof(Article).GetProperties().Where(p => p.GetValue(a).ToString().Contains(SearchQuery)).Any()).OrderBy(a => a.ArticleNumber));
                    if(!FilteredArticles.Where(a => a.SelectedForAction).Any() && Storage.Articles.Where(a => a.SelectedForAction).Any())
                    {
                        //we do this to prevent editing articles that are not currently visible
                        Storage.Articles.Where(a => a.SelectedForAction).First().SelectedForAction = false;
                    }
                }
            }
        }

        private decimal _newPrice;
        public decimal NewPrice
        {
            get { return _newPrice; }
            set { SetProperty(ref _newPrice, value); }
        }

        public MainViewModel()
        {
            InitializeCommands();
            QuantityUnits = new ObservableCollection<QuantityUnit>((IEnumerable<QuantityUnit>)Enum.GetValues(typeof(QuantityUnit)));
            Storage = new Storage();
            Load();
        }
        public RelayCommand<object> SaveCommand { get; set; }
        public RelayCommand<object> LoadCommand { get; set; }
        public RelayCommand<object> BookInCommand { get; set; }
        public RelayCommand<object> BookOutCommand { get; set; }
        public RelayCommand<object> SaveArticleCommand { get; set; }
        public RelayCommand<object> DeleteArticleCommand { get; set; }
        public RelayCommand<object> ChangePriceCommand { get; set; }
        public void InitializeCommands()
        {
            SaveCommand = new RelayCommand<object>(c => SaveAll());
            LoadCommand = new RelayCommand<object>(c => Load());
            BookInCommand = new RelayCommand<object>((object par) => { BookInExecute(Storage.Articles.Where(a => a.SelectedForAction).First()); }, c => Storage.Articles.Where(a => a.SelectedForAction).Any());
            BookOutCommand = new RelayCommand<object>((object par) => { BookOutExecute(Storage.Articles.Where(a => a.SelectedForAction).First()); }, c => Storage.Articles.Where(a => a.SelectedForAction).Any());
            ChangePriceCommand = new RelayCommand<object>((object par) => { ChangePriceExecute(); }, c => Storage.Articles.Where(a => a.SelectedForAction).Any());
            SaveArticleCommand = new RelayCommand<object>((object par) => { CreateNewArticleExecute(); }, c => Article.IsValid);
            DeleteArticleCommand = new RelayCommand<object>((object par) => { DeleteArticleExecute(); }, c => Storage.Articles.Where(a => a.SelectedForAction).Any());
        }

        private void BookInExecute(Article article)
        {
            Storage.BookIn(Quantity, article.ArticleNumber);
            Quantity = 0m;
        }

        private void BookOutExecute(Article article)
        {
            if(article.StorageQuantity >= Quantity || MessageBox.Show("Not enough in stock, book the remaining quantity instead?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Storage.BookOut(Quantity, article.ArticleNumber);
                Quantity = 0m;
            }
        }

        private void ChangePriceExecute()
        {
            Article article = Storage.Articles.Where(a => a.SelectedForAction).First();
            Storage.ChangePrice(article.ArticleNumber, NewPrice);
        }

        private void CreateNewArticleExecute()
        {
            if(Article.ArticleNumber <= 0 || Storage.Articles.Where(a => a.ArticleNumber == Article.ArticleNumber).Any())
            {
                //when we create a new article and the chosen articlenumber is already taken, we generate a new one
                for(int i = (Article.ArticleNumber > 0 ? Article.ArticleNumber : 1); i< int.MaxValue; i++)
                {
                    if(!Storage.Articles.Where(a => a.ArticleNumber == i).Any())
                    {
                        Article.ArticleNumber = i;
                        break;
                    }
                }
            }
            Storage.CreateArticle(Article);
            Article = new Article();
        }

        private void DeleteArticleExecute()
        {
            if(MessageBox.Show("Are you sure you want to delete this article?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Article article = Storage.Articles.Where(a => a.SelectedForAction).First();
                Storage.DeleteArticle(article.ArticleNumber);
                File.Delete(Path.Combine(ArticleDataLocation, article.ArticleNumber.ToString() + ".article"));
            }
        }

        #region Save and Load
        private void SaveAll()
        {
            List<Article> saveIssueList = new List<Article>();
            foreach(Article article in Storage.Articles)
            {
                if (!SaveArticle(article))
                {
                    saveIssueList.Add(article);
                }
            }
            string errorList = "";
            foreach(Article article in saveIssueList)
            {
                errorList += (string.IsNullOrWhiteSpace(errorList) ? "" : "; " ) + article.ArticleNumber.ToString();
            }
            if (!string.IsNullOrWhiteSpace(errorList))
            {
                MessageBox.Show("Errors occurred while trying to save the articles " + errorList, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool SaveArticle(Article article)
        {
            try
            {
                File.WriteAllText(Path.Combine(ArticleDataLocation, article.ArticleNumber.ToString() + ".article"), article.ToString(), Encoding.Default);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Load()
        {
            if (Directory.Exists(ArticleDataLocation))
            {
                Storage = new Storage();
                Regex filter = new Regex(@"^(?<ArticleNumber>[^î]+)[î](?<ArticleDescription>[^î]+)[î](?<Price>[^î]+)[î](?<StorageQuantity>[^î]+)[î](?<QuantityUnit>[^î]+)$");
                string[] articleFiles = Directory.GetFiles(ArticleDataLocation);
                foreach (string articleFile in articleFiles)
                {
                    Article loadedArticle = new Article();

                    if (int.TryParse(articleFile.Substring(articleFile.LastIndexOf('\\')+1, articleFile.LastIndexOf('.') - articleFile.LastIndexOf('\\') -1), out int articleNumber))
                    {
                        string fileContent = File.ReadAllText(articleFile, Encoding.GetEncoding(1252)).Replace("\\r\\n", "");
                        Match filterResult = filter.Match(fileContent);
                        foreach (var property in typeof(Article).GetProperties())
                        {
                            try
                            {
                                if (filterResult.Groups[property.Name] != null && property.PropertyType.IsAssignableFrom(typeof(string)))
                                {
                                    property.SetValue(loadedArticle, filterResult.Groups[property.Name].Value.Trim());
                                }
                                else
                                {
                                    switch (property.PropertyType)
                                    {
                                        case var value when value == typeof(decimal):
                                            property.SetValue(loadedArticle, decimal.Parse(filterResult.Groups[property.Name].Value));
                                            break;
                                        case var value when value == typeof(QuantityUnit):
                                            property.SetValue(loadedArticle, Enum.Parse(typeof(QuantityUnit), filterResult.Groups[property.Name].Value));
                                            break;
                                        case var value when value == typeof(int):
                                            property.SetValue(loadedArticle, int.Parse(filterResult.Groups[property.Name].Value));
                                            break;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine("Exception occured on loading articles: " + e);
                            }
                        }
                        Storage.Articles.Add(loadedArticle);
                    }
                }
                Storage.Articles = new ObservableCollection<Article>(Storage.Articles.OrderBy(a => a.ArticleNumber));
                SearchQuery = "";//we set this to trigger the search and initialize the filtered list
                return true;
            }
            Directory.CreateDirectory(ArticleDataLocation);
            return false;
        }
        #endregion Save and Load
    }
}
