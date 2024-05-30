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
    /// Логика взаимодействия для Equipment.xaml
    /// </summary>
    public partial class Equipments : Window
    {
        private int _userId;

        public ObservableCollection<Equipment> EquipmentData { get; set; } = new ObservableCollection<Equipment>();
        public ObservableCollection<Equipment> FilteredEquipmentData { get; set; } = new ObservableCollection<Equipment>();

        public Equipments(int userId)
        {
            InitializeComponent();
            LoadEquipmentData();
            FilteredEquipmentData = new ObservableCollection<Equipment>(EquipmentData);
            DataContext = this;
            _userId = userId;
        }
        private void LoadEquipmentData()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT [IdEquipment], [Lighting], [TemperatureControl], [WaterPurification], [AdditionalEquipment], [IdAquariumClass] FROM [Aquarium].[dbo].[Equipment]", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        EquipmentData.Add(new Equipment
                        {
                            IdEquipment = reader.GetInt32(0),
                            Lighting = reader.IsDBNull(1) ? "" : reader.GetString(1),
                            TemperatureControl = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            WaterPurification = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            AdditionalEquipment = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            IdAquariumClass = reader.IsDBNull(5) ? 0 : reader.GetInt32(5)
                        });
                    }
                }
            }
        }

        private void CopyEquipmentsData()
        {
            foreach (var item in EquipmentData)
            {
                FilteredEquipmentData.Add(item);
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
            string searchText = txtSearch.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                FilteredEquipmentData.Clear();
                foreach (var item in EquipmentData)
                {
                    FilteredEquipmentData.Add(item);
                }
            }
            else
            {
                FilteredEquipmentData.Clear();
                foreach (Equipment item in EquipmentData)
                {
                    if (item.Lighting.ToLower().Contains(searchText) ||
                        item.TemperatureControl.ToLower().Contains(searchText) ||
                        item.WaterPurification.ToLower().Contains(searchText) ||
                        item.AdditionalEquipment.ToLower().Contains(searchText) ||
                        item.IdAquariumClass.ToString().Contains(searchText))
                    {
                        FilteredEquipmentData.Add(item);
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

            Equipment selectedEquipment = (Equipment)dataGrid.SelectedItem; // Предполагается, что используется класс Equipment для хранения данных

            var selectedEquipmentItem = (Equipment)dataGrid.SelectedItem;

            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection("Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM [Equipment] WHERE [IdEquipment] = @IdEquipment", connection))
                    {
                        command.Parameters.AddWithValue("@IdEquipment", selectedEquipmentItem.IdEquipment);
                        command.ExecuteNonQuery();
                    }
                }

                // Удаление элемента из коллекции данных
                EquipmentData.Remove(selectedEquipmentItem);
                FilteredEquipmentData.Remove(selectedEquipmentItem);

                // Обновление отображения данных в DataGrid
                dataGrid.Items.Refresh(); // Обновление всех элементов в DataGrid
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
                MessageBox.Show("Пожалуйста, выберите растение для редактирования.");
                return;
            }
            Equipment selecteEquipments = (Equipment)dataGrid.SelectedItem;

            if (selecteEquipments != null)
            {
                EditEquipmentsDialog editEqmWindow = new EditEquipmentsDialog(selecteEquipments);
                editEqmWindow.ShowDialog();

            }
        }


        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            AddEquipmentsDialog addEquipmentsWindow = new AddEquipmentsDialog();
            addEquipmentsWindow.Show();
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