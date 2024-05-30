using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using Аквариум_практика.View;
using Аквариум_практика.ADDNEW;
using Аквариум_практика.Edit;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.IO;
using OfficeOpenXml;
using Аквариум_практика;
using System.Data;
using System;
using Microsoft.Win32;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Google.Apis.Admin.Directory.directory_v1.Data;


namespace Аквариум_практика.Clients
{
    public partial class Aquarium : Window
    {
        private int _userId;
        public ObservableCollection<AquariumClassification> AquariumClassificationData { get; set; } = new ObservableCollection<AquariumClassification>();
        public ObservableCollection<AquariumClassification> FilteredAquariumData { get; set; } = new ObservableCollection<AquariumClassification>();
        public Aquarium(int userId)
        {
            InitializeComponent();
            LoadAquariumData();
            FilteredAquariumData = new ObservableCollection<AquariumClassification>(AquariumClassificationData);
            DataContext = this;
            _userId = userId;

            if (CheckAdminRights(userId))
            {
                EditButton.Visibility = Visibility.Collapsed;
                DeleteButton.Visibility = Visibility.Collapsed;
                AddButton.Visibility = Visibility.Collapsed;
            }
        }
        private bool CheckAdminRights(int userId)
        {
            if (userId == 4) 
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void LoadAquariumData()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT [IdAquariumClass], [TypeConstruction], [Location], [Scope], [Shape], [Volume] FROM [Aquarium].[dbo].[Aquarium_Classification]", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        AquariumClassificationData.Add(new AquariumClassification
                        {
                            IdAquariumClass = reader.GetInt32(0),
                            TypeConstruction = reader.GetString(1),
                            Location = reader.GetString(2),
                            Scope = reader.GetString(3),
                            Shape = reader.GetString(4),
                            Volume = reader.GetInt32(5)
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
        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                FilteredAquariumData.Clear();
                foreach (var item in AquariumClassificationData)
                {
                    FilteredAquariumData.Add(item);
                }
            }
            else
            {
                FilteredAquariumData.Clear();
                foreach (var item in AquariumClassificationData)
                {
                    if (item.TypeConstruction.ToLower().Contains(searchText) ||
                        item.Location.ToLower().Contains(searchText) ||
                        item.Scope.ToLower().Contains(searchText) ||
                        item.Shape.ToLower().Contains(searchText) ||
                        item.Volume.ToString().ToLower().Contains(searchText))
                    {
                        FilteredAquariumData.Add(item);
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

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Запрос пользовательского ввода для названия файла
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Excel файл (*.xlsx)|*.xlsx";
            saveFileDialog.DefaultExt = ".xlsx";
            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string fileName = saveFileDialog.FileName;

                FileInfo newFile = new FileInfo(fileName);

                using (ExcelPackage excelPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                    for (int j = 0; j < dataGrid.Columns.Count; j++)
                    {
                        worksheet.Cells[1, j + 1].Value = dataGrid.Columns[j].Header;
                    }

                    for (int i = 0; i < dataGrid.Items.Count; i++)
                    {
                        for (int j = 0; j < dataGrid.Columns.Count; j++)
                        {
                            var cellValue = ((TextBlock)dataGrid.Columns[j].GetCellContent(dataGrid.Items[i])).Text;

                            // Проверка, является ли значение числом и форматирование как число
                            if (double.TryParse(cellValue, out double number))
                            {
                                worksheet.Cells[i + 2, j + 1].Value = number;
                            }
                            else
                            {
                                worksheet.Cells[i + 2, j + 1].Value = cellValue;
                            }
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

            AquariumClassification selectedAquarium = (AquariumClassification)dataGrid.SelectedItem;

            string errorMessage;
            bool hasForeignKeyConflict = CheckForForeignKeyConflict(selectedAquarium.IdAquariumClass, out errorMessage);

            string message = hasForeignKeyConflict ?
                $"Вы действительно хотите удалить запись с ограничением внешнего ключа. Это может привести к {errorMessage}" :
                "Вы уверены, что хотите удалить запись?";

            if (MessageBox.Show(message, "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection("Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
                {
                    connection.Open();

                    if (hasForeignKeyConflict)
                    {
                        SqlCommand disableFKCommand = new SqlCommand("ALTER TABLE dbo.Aqua_Class_Plants_Fish NOCHECK CONSTRAINT FK_Aqua_Class_Plants_Fish_Aquarium_Classification", connection);
                        disableFKCommand.ExecuteNonQuery();
                    }

                    if (hasForeignKeyConflict)
                    {
                        // Здесь можно добавить код для удаления или обновления связанных записей в таблице Aquarium_Cleaning_Equipment
                        SqlCommand deleteRelatedRecordsCommand = new SqlCommand("DELETE FROM [Aquarium].[dbo].[Aquarium_Cleaning_Equipment] WHERE [IdAquariumClass] = @IdAquariumClass", connection);
                        deleteRelatedRecordsCommand.Parameters.AddWithValue("@IdAquariumClass", selectedAquarium.IdAquariumClass);
                        deleteRelatedRecordsCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand command = new SqlCommand("DELETE FROM [Aquarium].[dbo].[Aquarium_Classification] WHERE [IdAquariumClass] = @IdAquariumClass", connection))
                    {
                        command.Parameters.AddWithValue("@IdAquariumClass", selectedAquarium.IdAquariumClass);
                        command.ExecuteNonQuery();
                    }

                    if (hasForeignKeyConflict)
                    {
                        SqlCommand enableFKCommand = new SqlCommand("ALTER TABLE dbo.Aqua_Class_Plants_Fish CHECK CONSTRAINT FK_Aqua_Class_Plants_Fish_Aquarium_Classification", connection);
                        enableFKCommand.ExecuteNonQuery();
                    }

                    AquariumClassificationData.Remove(selectedAquarium);
                    FilteredAquariumData.Remove(selectedAquarium);

                    dataGrid.SelectedItem = null;
                }
            }
        }
        private bool CheckForForeignKeyConflict(int id, out string errorMessage)
        {
            errorMessage = "утери данных, нарушению целостности данных или ошибки в приложении"; 
            return true;
        }
        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите растение для редактирования.");
                return;
            }
            AquariumClassification selectedAquarium = (AquariumClassification)dataGrid.SelectedItem;

            if (selectedAquarium != null)
            {
                EditAquariumDialog editAquariumWindow = new EditAquariumDialog(selectedAquarium);
                editAquariumWindow.ShowDialog();

            }
        }
        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            AddAquariumDialog addaquariumWindow = new AddAquariumDialog();
            addaquariumWindow.Show();
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