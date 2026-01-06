using DemoAttempt3.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DemoAttempt3
{
    /// <summary>
    /// Interaction logic for ItemUserControl.xaml
    /// </summary>
    public partial class ItemUserControl : UserControl
    {
        public ItemUserControl(Models.Good good, bool IsAdmin)
        {
            InitializeComponent();
            LoadGood(good);
        }
        
        private void LoadGood(Models.Good good)
        {
            if (good != null)
            {
                CategoryLabel.Content = $"{good.GoodName} | {good.Category}";
                Description.Content = $"Описание товара: {good.Description}";
                Fabric.Content = $"Производитель: {good.Fabric}";
                Supplier.Content = $"Производитель: {good.Supplier}";
                OldPrice.Text = good.Price.ToString();

                if (good.Discount > 0 && good.Discount <= 100)
                {
                    OldPrice.TextDecorations = TextDecorations.Strikethrough;
                    OldPrice.Foreground = Brushes.Red;
                    NewPrice.Text = (good.Price - good.Price * ((double)good.Discount / 100)).ToString();
                    if (good.Discount > 15)
                    {
                        DiscountBorder.Background = (Brush)Application.Current.Resources["Discount"];
                    }
                }
                else
                {
                    NewPrice.Visibility = Visibility.Collapsed;
                }

                UnitOfMeasure.Content = $"Единица измерения: {good.UnitOfMeasure}";
                Count.Content = $"Количество на складе: {good.Count}";
                Discount.Content = good.Discount.ToString();
                if (!(good.Photo is null))
                {
                    string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pictures", good.Photo);
                   
                    if (File.Exists(path))
                    {
                        BitmapImage bmp = new BitmapImage();
                        bmp.BeginInit();
                        bmp.UriSource = new Uri(path);
                        bmp.CacheOption = BitmapCacheOption.OnLoad;
                        bmp.EndInit();
                        PhotoBox.Source = bmp;
                    }
                }
            }
        }
    }
}
