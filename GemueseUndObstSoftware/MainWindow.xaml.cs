using GemueseUndObstSoftware.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GemueseUndObstSoftware
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        private void Focushandling_KeyDown(object sender, KeyEventArgs e)
        {
            if(sender is Control)
            {
                var x = sender as Control;
                if(e.Key == Key.Up)
                {
                    switch (x.Name)
                    {
                        case var value when value == BookingQuantityTextBox.Name:

                            break;
                        case var value when value == ArticleCreationCheckBox.Name:
                            BookingQuantityTextBox.Focus();
                            break;
                        case var value when value == ArticleCreationNumber.Name:
                            BookingQuantityTextBox.Focus();
                            break;
                        case var value when value == ArticleCreationDescription.Name:
                            ArticleCreationNumber.Focus();
                            break;
                        case var value when value == ArticleCreationPrice.Name:
                            ArticleCreationDescription.Focus();
                            break;
                        case var value when value == ArticleCreationQuantityUnit.Name:
                            ArticleCreationPrice.Focus();
                            break;
                    }
                    e.Handled = true;
                }
                else if(e.Key == Key.Down)
                {
                    switch (x.Name)
                    {
                        case var value when value == BookingQuantityTextBox.Name:
                            ArticleCreationNumber.Focus();
                            break;
                        case var value when value == ArticleCreationCheckBox.Name:
                            ArticleCreationNumber.Focus();
                            break;
                        case var value when value == ArticleCreationNumber.Name:
                            ArticleCreationDescription.Focus();
                            break;
                        case var value when value == ArticleCreationDescription.Name:
                            ArticleCreationPrice.Focus();
                            break;
                        case var value when value == ArticleCreationPrice.Name:
                            ArticleCreationQuantityUnit.Focus();
                            break;
                        case var value when value == ArticleCreationQuantityUnit.Name:

                            break;
                    }
                    e.Handled = true;
                }
                else if(e.Key == Key.Enter)
                {
                    switch (x.Name)
                    {
                        case var value when value == ArticleCreationCheckBox.Name:
                            ArticleCreationCheckBox.IsChecked = !ArticleCreationCheckBox.IsChecked;
                            break;
                        case var value when value == ArticleCreationNumber.Name:
                            ArticleCreationSubmitButton.Command.Execute(null);
                            break;
                        case var value when value == ArticleCreationDescription.Name:
                            ArticleCreationSubmitButton.Command.Execute(null);
                            break;
                        case var value when value == ArticleCreationPrice.Name:
                            ArticleCreationSubmitButton.Command.Execute(null);
                            break;
                        case var value when value == ArticleCreationQuantityUnit.Name:
                            ArticleCreationSubmitButton.Command.Execute(null);
                            break;
                    }
                    e.Handled = true;
                }
            }
        }
    }
}
