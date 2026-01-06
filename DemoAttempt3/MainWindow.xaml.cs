using DemoAttempt3.Data;
using DemoAttempt3.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

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

            LoadGoods();
        }

        private void LoadGoods()
        {
            var isAdmin = false;
            if (CurrentUser != null && CurrentUser.Role == "Администратор")
            {
                isAdmin = true;
            }
            ItemsPanel.Children.Clear();
            foreach (Good good in Goods)
            {
                var item = new ItemUserControl(good, isAdmin);
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
    }
}
