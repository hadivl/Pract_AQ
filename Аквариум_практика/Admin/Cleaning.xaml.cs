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
using Аквариум_практика.ADDNEW;
using System.Data;
using System;
using Microsoft.Win32;
using System.Collections.Generic;
using Google.Apis.Admin.Directory.directory_v1.Data;

namespace Аквариум_практика.Clients
{
    /// <summary>
    /// Логика взаимодействия для Cleaning.xaml
    /// </summary>
    public partial class Cleaning : Window
    {
        private AquariumCleaningEquipment selectedCleaning;
        private int _userId;

        public ObservableCollection<AquariumCleaningEquipment> CleaningData { get; set; } = new ObservableCollection<AquariumCleaningEquipment>();
        public ObservableCollection<AquariumCleaningEquipment> FiltereCleaningData { get; set; } = new ObservableCollection<AquariumCleaningEquipment>();

        public Cleaning(int userId)
        {
            InitializeComponent();
            LoadCleaningData();
            CopyCleaningData();

            DataContext = this;
            _userId = userId;
        }

        private void LoadCleaningData()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT [IdCleaningEquipment], [PumpingOutWater], [WaterInlet], [EquipPlantingFish], [Cleaning], [EquipCatchingFish], IdAquariumClass FROM [Aquarium].[dbo].[Aquarium_Cleaning_Equipment]", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        CleaningData.Add(new AquariumCleaningEquipment
                        {
                            IdCleaningEquipment = reader.GetInt32(0),
                            PumpingOutWater = reader.GetString(1),
                            WaterInlet = reader.GetString(2),
                            EquipPlantingFish = reader.GetString(3),
                            Cleaning = reader.GetString(4),
                            EquipCatchingFish = reader.GetString(5),
                            IdAquariumClass = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6)


                        });
                    }
                }
            }
        }

        private void CopyCleaningData()
        {
            foreach (var item in CleaningData)
            {
                FiltereCleaningData.Add(item);
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
                FiltereCleaningData.Clear();
                foreach (var item in CleaningData)
                {
                    FiltereCleaningData.Add(item);
                }
            }
            else
            {
                FiltereCleaningData.Clear();
                foreach (var item in CleaningData)
                {
                    if (item.PumpingOutWater.ToLower().Contains(searchText) ||
                        item.WaterInlet.ToLower().Contains(searchText) ||
                        item.EquipPlantingFish.ToLower().Contains(searchText) ||
                        item.Cleaning.ToLower().Contains(searchText) ||
                        item.EquipCatchingFish.ToString().ToLower().Contains(searchText) ||
                        item.IdAquariumClass.ToString().ToLower().Contains(searchText))
                    {
                        FiltereCleaningData.Add(item);
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

            AquariumCleaningEquipment selectedCleaning = (AquariumCleaningEquipment)dataGrid.SelectedItem;

            var selectedCleaningItem = (AquariumCleaningEquipment)dataGrid.SelectedItem;

            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection("Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM [Aquarium_Cleaning_Equipment] WHERE [IdCleaningEquipment] = @IdCleaningEquipment", connection))
                    {
                        command.Parameters.AddWithValue("@IdCleaningEquipment", selectedCleaningItem.IdCleaningEquipment);
                        command.ExecuteNonQuery();
                    }
                }

                CleaningData.Remove(selectedCleaningItem);
                FiltereCleaningData.Remove(selectedCleaningItem);
                dataGrid.SelectedItem = null;
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
                MessageBox.Show("Пожалуйста, выберите поле для редактирования.");
                return;
            }
            AquariumCleaningEquipment selectedCleaning = (AquariumCleaningEquipment)dataGrid.SelectedItem;

            if (selectedCleaning != null)
            {
                EditCleaningDialog editCleaningWindow = new EditCleaningDialog(selectedCleaning);
                editCleaningWindow.ShowDialog();

            }
        }


        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            AddCleaningDialog addcleaningWindow = new AddCleaningDialog();
            addcleaningWindow.Show();
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