using DemoAttempt3.Data;
using DemoAttempt3.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Animation;

namespace DemoAttempt3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Good> Goods = null;
        private ICollectionView goodView;
        private User CurrentUser = null;
        public MainWindow(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            ShowRoleComponents();
            Goods = DbHelper.GetGoods();
            goodView = CollectionViewSource.GetDefaultView(Goods);
            goodView.Filter = FilterGoods;
            LoadGoods();
        }

        private bool FilterGoods(object obj)
        {
            if (!(obj is Good good))
            {
                return false; 
            }

            string search = SearchBox?.Text.ToLower();

            bool searchMatch = string.IsNullOrWhiteSpace(search) ||
                good.Category?.ToLower().Contains(search) == true ||
                good.Description?.ToLower().Contains(search) == true ||
                good.Fabric?.ToLower().Contains(search) == true ||
                good.Supplier?.ToLower().Contains(search) == true ||
                good.GoodName?.ToLower().Contains(search) == true ||
                good.Article?.ToLower().Contains(search) == true;

            //bool providerMatch = true;

            //if(FilterBox.Sele)

            return searchMatch;
        }

        private void LoadGoods()
        {
            var isAdmin = false;

            if (CurrentUser != null && CurrentUser.Role == "Администратор")
            {
                isAdmin = true;
            }

            ItemsPanel.Children.Clear();

            foreach (Good good in goodView)
            {
                var item = new ItemUserControl(good, isAdmin);
                item.Edited += RefreshGoods;
                ItemsPanel.Children.Add(item);
            }
        }

        private void RefreshGoods()
        {
            Goods = DbHelper.GetGoods();

            var isAdmin = false;

            if (CurrentUser != null && CurrentUser.Role == "Администратор")
            {
                isAdmin = true;
            }

            ItemsPanel.Children.Clear();

            foreach (Good good in goodView)
            {
                var item = new ItemUserControl(good, isAdmin);
                item.Edited += RefreshGoods;
                ItemsPanel.Children.Add(item);
            }
        }

        private void ShowRoleComponents()
        {
            if (CurrentUser != null)
            {
                FullNameLabel.Content = CurrentUser.Name;

                if (CurrentUser.Role == "Администратор")
                {
                    this.Title = "Главное окно (Администратор)";

                }
                else if (CurrentUser.Role == "Менеджер")
                {
                    this.Title = "Главное окно (Менеджер)";
                    AddButton.Visibility = Visibility.Collapsed;
                }
                else if (CurrentUser.Role == "Авторизированный клиент")
                {
                    this.Title = "Главное окно (Авторизированный клиент)";
                    AddButton.Visibility = Visibility.Collapsed;
                    SearchBox.Visibility = Visibility.Collapsed;
                    FilterBox.Visibility = Visibility.Collapsed;
                    SortBox.Visibility = Visibility.Collapsed;
                }

                BackButton.Content = "Выйти";
            }
            else
            {
                FullNameLabel.Visibility = Visibility.Collapsed;
                this.Title = "Главное окно (Гость)";
                AddButton.Visibility = Visibility.Collapsed;
                SearchBox.Visibility = Visibility.Collapsed;
                FilterBox.Visibility = Visibility.Collapsed;
                SortBox.Visibility = Visibility.Collapsed;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditWindow wnd = new AddEditWindow(null, false)
            {
                Owner = App.Current.MainWindow
            };
            wnd.Closed += (s, args) => RefreshGoods();
            wnd.ShowDialog();
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if(goodView == null)
            {
                return;
            }

            goodView.Refresh();
            LoadGoods();
        }
    }
}
