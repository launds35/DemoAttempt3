using DemoAttempt3.Data;
using DemoAttempt3.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DemoAttempt3
{
    /// <summary>
    /// Interaction logic for AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        private Good CurrentGood;
        private int CurrentId;
        private string NewPhoto;
        private JpegBitmapEncoder Encoder = null;
        private string TargetPath;
        private bool IsEditing;

        public AddEditWindow(Good good, bool isEditing)
        {
            InitializeComponent();
            CurrentGood = good;
            IsEditing = isEditing;
            if (IsEditing) 
            {
                InitEditWindow();
            }
            else
            {
                InitAddWindow();
            }
        }

        private void LoadComboBoxes()
        {
            List<ComboBoxItems> fabrics = DbHelper.GetFabrics();
            List<ComboBoxItems> categories = DbHelper.GetCategories();

            ComboBoxItems itemForLists = new ComboBoxItems { Id = 0, Name = "Не выбрано" };
            
            fabrics.Insert(0, itemForLists);
            categories.Insert(0, itemForLists);

            FabricComboBox.ItemsSource = fabrics;
            CategoryComboBox.ItemsSource = categories;

            FabricComboBox.SelectedValuePath = "Id";
            FabricComboBox.DisplayMemberPath = "Name";
            CategoryComboBox.SelectedValuePath = "Id";
            CategoryComboBox.DisplayMemberPath = "Name";

            if(CurrentGood != null)
            {
                for (int i = 0; i < fabrics.Count; i++)
                {
                    if (fabrics[i].Name == CurrentGood.Fabric)
                    {
                        FabricComboBox.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < categories.Count; i++)
                {
                    if (categories[i].Name == CurrentGood.Category)
                    {
                        CategoryComboBox.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                FabricComboBox.SelectedIndex = 0;
                CategoryComboBox.SelectedIndex = 0;
            }
        }

        private void InitEditWindow()
        {
            this.Title = "Редактирование товара";
            if (CurrentGood != null) 
            {
                ArticleBox.Text = CurrentGood.Article;
                DescriptionBox.Text = CurrentGood.Description;
                GoodNameBox.Text = CurrentGood.GoodName;
                CountBox.Text = CurrentGood.Count.ToString();
                DiscountBox.Text = CurrentGood.Discount.ToString();
                SupplierBox.Text = CurrentGood.Supplier;
                UnitOfMeasureBox.Text = CurrentGood.UnitOfMeasure;
                PhotoLabel.Content = $"Фото ({CurrentGood.Photo})";
                PriceBox.Text = CurrentGood.Price.ToString();
                CurrentId = CurrentGood.Id;
                IdLabel.Content = $"ID: {CurrentId}";

                LoadComboBoxes();
            }
            AddEditButton.Content = "Редактировать";
            NewPhoto = CurrentGood.Photo;
        }

        private void InitAddWindow()
        {
            this.Title = "Добавление товара";
            LoadComboBoxes();
            CurrentId = DbHelper.GetMaxId() + 1;
            IdLabel.Content = $"New ID: {CurrentId}";
        }

        private void ChoosePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Выберите фото";
                openFileDialog.Filter = "Изображения (*.jpg;*.png)|*.jpg;*.png";
                openFileDialog.Multiselect = false;

                if(openFileDialog.ShowDialog() != true)
                {
                    return;
                }

                string sourcePath = openFileDialog.FileName;
                string pictureDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pictures");
                Directory.CreateDirectory(pictureDir);

                string fileName = $"{CurrentId}.jpg";
                TargetPath = Path.Combine(pictureDir, fileName);

                BitmapImage bitmapImage = new BitmapImage();

                using (FileStream fs = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
                {
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(TargetPath);
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.DecodePixelWidth = 300;
                    bitmapImage.DecodePixelHeight = 200;
                    bitmapImage.EndInit();
                }
                Encoder = new JpegBitmapEncoder();
                Encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            }
            catch 
            {
                
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Подтвердить удаление товара?", "Удаление товара", 
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                
            }
        }

        private void AddEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (FabricComboBox.SelectedIndex == 0)
            {
                MessageBox.Show("Ошибка, не выбран производитель!", $"{this.Title}: Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CategoryComboBox.SelectedIndex == 0)
            {
                MessageBox.Show("Ошибка, не выбрана категория товара!", $"{this.Title}: Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Good newGood = new Good
            {
                Id = CurrentId,
                Article = ArticleBox.Text,
                GoodName = GoodNameBox.Text,
                UnitOfMeasure = UnitOfMeasureBox.Text,
                Price = Convert.ToDouble(PriceBox.Text),
                Supplier = SupplierBox.Text,
                IdFabric = FabricComboBox.SelectedIndex,
                IdCategory = FabricComboBox.SelectedIndex,
                Discount = Convert.ToInt32(DiscountBox.Text),
                Count = Convert.ToInt32(CountBox.Text),
                Description = DescriptionBox.Text,
                Photo = NewPhoto
            };

            if (Encoder != null)
            {
                using (FileStream fs = new FileStream(TargetPath, FileMode.Create, FileAccess.Write))
                {
                    Encoder.Save(fs);
                }
            }

            try
            {
                if (IsEditing)
                {
                    if (DbHelper.UpdateGood(newGood))
                    {
                        MessageBox.Show("Успешное редактирования товара!", $"Успех!",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    if (DbHelper.AddGood(newGood))
                    {
                        MessageBox.Show("Успешное добавление товара!", $"Успех!",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка!{ex}", $"Ошибка!",
                                        MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void PriceBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if(!char.IsDigit(e.Text, 0) && e.Text != ".")
            {
                e.Handled = true;
            }
            TextBox textBox = sender as TextBox;
            if (e.Text == "." && textBox.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void DiscountBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
    }
}
