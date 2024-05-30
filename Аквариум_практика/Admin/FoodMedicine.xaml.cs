using Google.Apis.Admin.Directory.directory_v1.Data;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
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
using System.Windows.Shapes;
using Аквариум_практика.ADDNEW;
using Аквариум_практика.Edit;
using Аквариум_практика.View;

namespace Аквариум_практика.Clients
{
    /// <summary>
    /// Логика взаимодействия для FeedAndMedicines.xaml
    /// </summary>
    public partial class FoodMedicine : Window
    {
        private int _userId;

        public ObservableCollection<FeedAndMedicines> FeedAndMedicinesData { get; set; } = new ObservableCollection<FeedAndMedicines>();
        public ObservableCollection<FeedAndMedicines> FilteredFeedAndMedicinesData { get; set; } = new ObservableCollection<FeedAndMedicines>();

        public FoodMedicine(int userId)
        {
            InitializeComponent();
            LoadFeedAndMedicinesData();
            FilteredFeedAndMedicinesData = new ObservableCollection<FeedAndMedicines>(FeedAndMedicinesData);
            DataContext = this;
            _userId = userId;
        }

        private void LoadFeedAndMedicinesData()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT [IdFeedAndMedicines], [NameFeed], [TypeFeed], [NameMedicine], [TypeMedicine] FROM [Aquarium].[dbo].[Feed_And_Medicines]", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        FeedAndMedicinesData.Add(new FeedAndMedicines
                        {
                            IdFeedAndMedicines = reader.GetInt32(0),
                            NameFeed = reader.IsDBNull(1) ? "" : reader.GetString(1),
                            TypeFeed = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            NameMedicine = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            TypeMedicine = reader.IsDBNull(4) ? "" : reader.GetString(4)
                        });
                    }
                }
            }
        }
        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSearchPlaceholder.Visibility = Visibility.Collapsed;
        }
        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                txtSearchPlaceholder.Visibility = Visibility.Visible;
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Global globalWindow = new Global(_userId);
            this.Hide();
            globalWindow.Show();
        }

        private void SearchTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string searchText = (sender as TextBox).Text.ToLower();
         

            if (string.IsNullOrEmpty(searchText))
            {
                FilteredFeedAndMedicinesData.Clear();
                foreach (var item in FeedAndMedicinesData)
                {
                    FilteredFeedAndMedicinesData.Add(item);
                }
            }
            else
            {
                FeedAndMedicinesData.Clear();
                foreach (FeedAndMedicines item in FeedAndMedicinesData)
                {
                    if (item.NameFeed.ToLower().Contains(searchText) ||
                        item.TypeFeed.ToLower().Contains(searchText) ||
                        item.NameMedicine.ToLower().Contains(searchText) ||
                        item.TypeMedicine.ToLower().Contains(searchText))
                    {
                        FilteredFeedAndMedicinesData.Add(item);
                    }
                }
            }
        }

        private void ExportAllToXlsButtonClick(object sender, RoutedEventArgs e)
        {
            if (dataGrid.Items.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Установка LicenseContext
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Запрос пользовательского ввода для названия файла
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Excel файл (*.xlsx)|*.xlsx";
            saveFileDialog.DefaultExt = ".xlsx";
            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string fileName = saveFileDialog.FileName;

                // Создание нового пустого файла Excel
                FileInfo newFile = new FileInfo(fileName);

                // Запись данных из DataGrid в Excel файл
                using (ExcelPackage excelPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                    // Запись названий столбцов
                    for (int j = 0; j < dataGrid.Columns.Count; j++)
                    {
                        worksheet.Cells[1, j + 1].Value = dataGrid.Columns[j].Header;
                    }

                    // Запись данных из DataGrid в Excel
                    for (int i = 0; i < dataGrid.Items.Count; i++)
                    {
                        for (int j = 0; j < dataGrid.Columns.Count; j++)
                        {
                            var cellValue = ((TextBlock)dataGrid.Columns[j].GetCellContent(dataGrid.Items[i])).Text;
                            worksheet.Cells[i + 2, j + 1].Value = cellValue;
                        }
                    }
                    excelPackage.Save();
                }
                MessageBox.Show("Данные успешно экспортированы в файл " + fileName, "Экспорт завершен", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите запись для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            FeedAndMedicines selectedFoddMed = (FeedAndMedicines)dataGrid.SelectedItem;

            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection("Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM [Aquarium].[dbo].[Feed_And_Medicines] WHERE [IdFeedAndMedicines] = @IdFeedAndMedicines", connection))
                    {
                        command.Parameters.AddWithValue("@IdFeedAndMedicines", selectedFoddMed.IdFeedAndMedicines);
                        command.ExecuteNonQuery();
                    }
                }

                FeedAndMedicinesData.Remove(selectedFoddMed);
                FilteredFeedAndMedicinesData.Remove(selectedFoddMed);

                dataGrid.SelectedItem = null;
            }
        }


        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите растение для редактирования.");
                return;
            }
            FeedAndMedicines selectedMedFeed = (FeedAndMedicines)dataGrid.SelectedItem;

            if (selectedMedFeed != null)
            {
                EditFeedMedDialog editFeedMedWindow = new EditFeedMedDialog(selectedMedFeed);
                editFeedMedWindow.ShowDialog();

            }
        }


        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            AddFeedAndMedicinesDialog addfeedmedWindow = new AddFeedAndMedicinesDialog();
            addfeedmedWindow.Show();
            Close();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                EditButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
            }
            else
            {
                EditButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
            }
        }
    }
}