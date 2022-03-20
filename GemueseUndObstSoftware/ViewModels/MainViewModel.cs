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
    public class MainViewModel : ViewModelBase
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

        private ObservableCollection<ArticleDisplay> _filteredArticles;
        public ObservableCollection<ArticleDisplay> FilteredArticles
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
                    FilteredArticles = new ObservableCollection<ArticleDisplay>(ArticleDisplay.GetAsArticleDisplays(Storage.ArticleStock.OrderBy(a => a.ArticleNumber).ToList()));
                }
                else
                {
                    FilteredArticles = new ObservableCollection<ArticleDisplay>(ArticleDisplay.GetAsArticleDisplays(Storage.ArticleStock.Where(a => typeof(Article).GetProperties().Where(p => p.GetValue(a).ToString().Contains(SearchQuery)).Any()).OrderBy(a => a.ArticleNumber).ToList()));
                }
            }
        }

        private bool _autoSave = true;
        public bool AutoSave
        {
            get { return _autoSave; }
            set { SetProperty(ref _autoSave, value); }
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
            BookInCommand = new RelayCommand<object>((object par) => { BookInExecute(); }, c => this.FilteredArticles.Where(a => a.SelectedForAction).Any());
            BookOutCommand = new RelayCommand<object>((object par) => { BookOutExecute(); }, c => this.FilteredArticles.Where(a => a.SelectedForAction).Any());
            ChangePriceCommand = new RelayCommand<object>((object par) => { ChangePriceExecute(); }, c => this.FilteredArticles.Where(a => a.SelectedForAction).Any());
            SaveArticleCommand = new RelayCommand<object>((object par) => { CreateNewArticleExecute(); }, c => IsArticleValid(Article));
            DeleteArticleCommand = new RelayCommand<object>((object par) => { DeleteArticleExecute(); }, c => this.FilteredArticles.Where(a => a.SelectedForAction).Any());
        }

        public bool IsArticleValid(Article article)
        {
            return !string.IsNullOrWhiteSpace(article.ArticleDescription) && article.Price != 0 && article.ArticleNumber >= 0;
        }

        private void BookInExecute()
        {
            Article article = this.FilteredArticles.Where(a => a.SelectedForAction).First().Article;
            Storage.BookIn(Quantity, article.ArticleNumber);
            Quantity = 0m;
            if (AutoSave)
                SaveArticle(article);
            SearchQuery = SearchQuery;
        }

        private void BookOutExecute()
        {
            Article article = this.FilteredArticles.Where(a => a.SelectedForAction).First().Article;
            if (article.StorageQuantity >= Quantity || MessageBox.Show("Not enough in stock, book the remaining quantity instead?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Storage.BookOut(Quantity, article.ArticleNumber);
                Quantity = 0m;
                if (AutoSave)
                    SaveArticle(article);
                SearchQuery = SearchQuery;
            }
        }

        private void ChangePriceExecute()
        {
            Article article = this.FilteredArticles.Where(a => a.SelectedForAction).First().Article;
            Storage.ChangePrice(article.ArticleNumber, NewPrice);
            if (AutoSave)
                SaveArticle(article);
            SearchQuery = SearchQuery;
        }

        private void CreateNewArticleExecute()
        {
            if (Article.ArticleNumber <= 0 || Storage.ArticleStock.Where(a => a.ArticleNumber == Article.ArticleNumber).Any())
            {
                //when we create a new article and the chosen articlenumber is already taken, we generate a new one
                for (int i = (Article.ArticleNumber > 0 ? Article.ArticleNumber : 1); i < int.MaxValue; i++)
                {
                    if (!Storage.ArticleStock.Where(a => a.ArticleNumber == i).Any())
                    {
                        Article.ArticleNumber = i;
                        break;
                    }
                }
            }
            Storage.CreateArticle(Article.ArticleNumber, Article.ArticleDescription, Article.QuantityUnit, Article.Price);
            if (AutoSave)
                SaveArticle(Article);
            Article = new Article();
            SearchQuery = SearchQuery;
        }

        private void DeleteArticleExecute()
        {
            if (MessageBox.Show("Are you sure you want to delete this article?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Article article = this.FilteredArticles.Where(a => a.SelectedForAction).First().Article;
                Storage.DeleteArticle(article.ArticleNumber);
                File.Delete(Path.Combine(ArticleDataLocation, article.ArticleNumber.ToString() + ".article"));
                SearchQuery = SearchQuery;
            }
        }

        #region Save and Load
        private void SaveAll()
        {
            List<Article> saveIssueList = new List<Article>();
            foreach (Article article in Storage.ArticleStock)
            {
                if (!SaveArticle(article))
                {
                    saveIssueList.Add(article);
                }
            }
            string errorList = "";
            foreach (Article article in saveIssueList)
            {
                errorList += (string.IsNullOrWhiteSpace(errorList) ? "" : "; ") + article.ArticleNumber.ToString();
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
            catch (Exception)
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

                    if (int.TryParse(articleFile.Substring(articleFile.LastIndexOf('\\') + 1, articleFile.LastIndexOf('.') - articleFile.LastIndexOf('\\') - 1), out int articleNumber))
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
                        Storage.ArticleStock.Add(loadedArticle);
                    }
                }
                Storage.ArticleStock = new ObservableCollection<Article>(Storage.ArticleStock.OrderBy(a => a.ArticleNumber));
                SearchQuery = "";//we set this to trigger the search and initialize the filtered list
                return true;
            }
            Directory.CreateDirectory(ArticleDataLocation);
            return false;
        }
        #endregion Save and Load
    }
}
