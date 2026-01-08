using DemoAttempt3.Data;
using DemoAttempt3.Models;
using System.Collections.Generic;
using System.Windows;

namespace DemoAttempt3
{
    /// <summary>
    /// Interaction logic for AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        private Good CurrentGood;
        private int Id;

        public AddEditWindow(Good good, bool isEditing)
        {
            InitializeComponent();
            CurrentGood = good;
            if (isEditing) 
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

            fabrics.Insert(0, new ComboBoxItems { Id = 0, Name = "Не выбрано" });
            categories.Insert(0, new ComboBoxItems { Id = 0, Name = "Не выбрано" });

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
                DiscountBox.Text = CurrentGood.Count.ToString();
                SupplierBox.Text = CurrentGood.Supplier;
                UnitOfMeasureBox.Text = CurrentGood.UnitOfMeasure;
                PhotoLabel.Content = $"Фото ({CurrentGood.Photo})";
                PriceBox.Text = CurrentGood.Price.ToString();
                Id = CurrentGood.Id;
                IdLabel.Content = $"ID: {Id}";

                LoadComboBoxes();
            }
            AddEditButton.Content = "Редактировать";
        }

        private void InitAddWindow()
        {
            this.Title = "Добавление товара";
            LoadComboBoxes();
            Id = DbHelper.GetMaxId() + 1;
            IdLabel.Content = $"New ID: {Id}";
        }
    }
}
