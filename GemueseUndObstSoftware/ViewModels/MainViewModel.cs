using GemueseUndObstSoftware.Enums;
using GemueseUndObstSoftware.Models;
using GemueseUndObstSoftware.MVVM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GemueseUndObstSoftware.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private static readonly string DataLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GemueseUndObstApplicationData");
        private static readonly string ArticleDataLocation = Path.Combine(DataLocation, "Articles");

        private Storage _storage;
        public Storage Storage
        {
            get { return _storage; }
            set { SetProperty(ref _storage, value); }
        }

        public MainViewModel()
        {
            InitializeCommands();
            Load();
        }
        public RelayCommand<object> saveCommand;
        public RelayCommand<object> loadCommand;
        public RelayCommand<object> newArticleCommand;
        public void InitializeCommands()
        {
            saveCommand = new RelayCommand<object>(c => Save());
            loadCommand = new RelayCommand<object>(c => Load());
            newArticleCommand = new RelayCommand<object>(c => CreateNewArticle());
        }

        public void CreateNewArticle()
        {

        }

        #region Save and Load
        public void Save()
        {
            foreach(Article article in Storage.Articles)
            {
                SaveArticle(article);
            }
        }

        public bool SaveArticle(Article article)
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

        public void Load()
        {
            Regex filter = new Regex(@"^(?<ArticleNumber>[^î]+)[î](?<ArticleName>[^î]+)[î](?<ArticleDescription>[^î]+)[î](?<Price>[^î]+)[î](?<StorageQuantity>[^î]+)[î](?<QuantityUnit>[^î]+)$");
            string[] articleFiles = Directory.GetFiles(ArticleDataLocation);
            foreach (string articleFile in articleFiles)
            {
                Article loadedArticle = new Article();
                
                if (int.TryParse(articleFile.Substring(articleFile.LastIndexOf('\\'), articleFile.LastIndexOf('.')-1), out int articleNumber)){
                    string fileContent = File.ReadAllText(articleFile).Replace("\\r\\n", "");
                    Match filterResult = filter.Match(fileContent);
                    foreach(var property in typeof(Article).GetProperties())
                    {
                        try
                        {
                            if (filterResult.Groups[property.Name] != null && property.PropertyType.IsAssignableFrom(typeof(string)))
                            {
                                property.SetValue(loadedArticle, filterResult.Groups[property.Name].Value.Trim());
                            }
                            else
                            {
                                switch (property.GetType())
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
                        catch(Exception e)
                        {
                            Debug.WriteLine("Exception occured on loading articles: " + e);
                        }
                    }
                    Storage.Articles.Add(loadedArticle);
                }
            }
            Storage.Articles = new System.Collections.ObjectModel.ObservableCollection<Article>(Storage.Articles.OrderBy(a => a.ArticleNumber));
        }
        #endregion Save and Load
    }
}
