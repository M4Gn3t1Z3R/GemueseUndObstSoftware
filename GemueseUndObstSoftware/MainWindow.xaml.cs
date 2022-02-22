using GemueseUndObstSoftware.ViewModels;
using GemueseUndObstSoftware.Models;
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
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        private void Focushandling_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is Control)
            {
                var x = sender as Control;
                if (e.Key == Key.Up)
                {
                    e.Handled = true;
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
                        default:
                            e.Handled = false;
                            break;
                    }
                }
                else if (e.Key == Key.Down)
                {
                    e.Handled = true;
                    switch (x.Name)
                    {
                        case var value when value == BookingQuantityTextBox.Name:
                            ArticleCreationCheckBox.Focus();
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
                        default:
                            e.Handled = false;
                            break;
                    }
                }
                else if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    switch (x.Name)
                    {
                        case var value when value == BookingQuantityTextBox.Name:
                            if(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift) || Keyboard.IsKeyDown(Key.LeftAlt))
                            {
                                BookOutButton.Focus();
                                if (BookOutButton.Command.CanExecute(null))
                                    BookOutButton.Command.Execute(null);
                            }
                            else
                            {
                                BookInButton.Focus();
                                if (BookInButton.Command.CanExecute(null))
                                    BookInButton.Command.Execute(null);
                            }
                            BookingQuantityTextBox.Focus();
                            break;
                        case var value when value == ArticleCreationCheckBox.Name:
                            ArticleCreationCheckBox.IsChecked = !ArticleCreationCheckBox.IsChecked;
                            break;
                        case var value when value == ArticleCreationNumber.Name:
                            if (ArticleCreationSubmitButton.Command.CanExecute(null))
                            {
                                ArticleCreationSubmitButton.Command.Execute(null);
                                ArticleCreationNumber.Focus();
                            }
                            break;
                        case var value when value == ArticleCreationDescription.Name:
                            if (ArticleCreationSubmitButton.Command.CanExecute(null))
                            {
                                ArticleCreationSubmitButton.Command.Execute(null);
                                ArticleCreationNumber.Focus();
                            }
                            break;
                        case var value when value == ArticleCreationPrice.Name:
                            //we set the focus here because we have onLostFocus as changedetection
                            ArticleCreationNumber.Focus();
                            if (ArticleCreationSubmitButton.Command.CanExecute(null))
                            {
                                ArticleCreationSubmitButton.Command.Execute(null);
                                ArticleCreationNumber.Focus();
                            }
                            break;
                        case var value when value == ArticleCreationQuantityUnit.Name:
                            if (ArticleCreationSubmitButton.Command.CanExecute(null))
                            {
                                ArticleCreationSubmitButton.Command.Execute(null);
                                ArticleCreationNumber.Focus();
                            }
                            break;
                        case var value when value == NewPriceTextBox.Name:
                            BookingQuantityTextBox.Focus();
                            if (ChangePriceButton.Command.CanExecute(null))
                            {
                                ChangePriceButton.Command.Execute(null);
                            }
                            break;
                        case var value when value == ArticleSelectionList.Name:
                            var lView = x as ListView;
                            (lView.SelectedItem as Article).SelectedForAction = true;
                            break;
                        default:
                            e.Handled = false;
                            break;
                    }
                }
                else if(e.Key == Key.Tab)
                {
                    e.Handled = true;
                    switch (x.Name)
                    {
                        case var value when value == ArticleSelectionList.Name:
                            BookingQuantityTextBox.Focus();
                            break;
                        case var value when value == BookingQuantityTextBox.Name:
                            ArticleSelectionList.Focus();
                            break;
                        default:
                            e.Handled = false;
                            break;
                    }
                }
            }
        }
    }
}
