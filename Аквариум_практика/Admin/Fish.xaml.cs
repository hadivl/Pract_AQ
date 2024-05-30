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
    /// Логика взаимодействия для Fish.xaml
    /// </summary>
    public partial class Fish : Window
    {
        private int _userId;

        public ObservableCollection<AquariumFish> FishData { get; set; } = new ObservableCollection<AquariumFish>();
        public ObservableCollection<AquariumFish> FilteredFishtData { get; set; } = new ObservableCollection<AquariumFish>();

        public Fish(int userId)
        {
            InitializeComponent();
            LoadFishData();
            FilteredFishtData = new ObservableCollection<AquariumFish>(FishData);
            DataContext = this;
            _userId = userId;
        }

        private void LoadFishData()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT [IdAquaFish], [Name], [MaximumSize], [WaterTemperature], [pHOfTheWater], [RequiredVolumeAquarium], [TheNeedShelters] FROM [Aquarium].[dbo].[Aquarium_Fish]", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        FishData.Add(new AquariumFish
                        {
                            IdAquaFish = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            MaximumSize = reader.GetInt32(2),
                            WaterTemperature = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                            pHOfTheWater = reader.IsDBNull(4) ? string.Empty : reader.GetString(4), // Прочитать как string, обрабатывая возможность null значения
                            RequiredVolumeAquarium = reader.GetInt32(5),
                            TheNeedShelters = reader.GetString(6)
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
                FilteredFishtData.Clear();
                foreach (var item in FishData)
                {
                    FilteredFishtData.Add(item);
                }
            }
            else
            {
                FilteredFishtData.Clear();
                foreach (AquariumFish item in FishData)
                {
                    if (item.Name.ToLower().Contains(searchText) ||
                        item.MaximumSize.ToString().Contains(searchText) ||
                        item.WaterTemperature.ToString().Contains(searchText) ||
                        item.pHOfTheWater.ToString().Contains(searchText) ||
                        item.RequiredVolumeAquarium.ToString().Contains(searchText))
                    {
                        FilteredFishtData.Add(item);
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

            AquariumFish selectedFish = (AquariumFish)dataGrid.SelectedItem;

            string errorMessage;
            bool hasForeignKeyConflict = CheckForForeignKeyConflict(selectedFish.IdAquaFish, out errorMessage);

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
                        SqlCommand deleteRelatedRecordsCommand = new SqlCommand("DELETE FROM [Aquarium].[dbo].[Aquarium_Fish] WHERE [IdAquaFish] = @IdAquaFish", connection);
                        deleteRelatedRecordsCommand.Parameters.AddWithValue("@IdAquaFish", selectedFish.IdAquaFish);
                        deleteRelatedRecordsCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand command = new SqlCommand("DELETE FROM [Aquarium].[dbo].[Aquarium_Fish] WHERE [IdAquaFish] = @IdAquaFish", connection))
                    {
                        command.Parameters.AddWithValue("@IdAquaFish", selectedFish.IdAquaFish);
                        command.ExecuteNonQuery();
                    }

                    if (hasForeignKeyConflict)
                    {
                        SqlCommand enableFKCommand = new SqlCommand("ALTER TABLE dbo.Aqua_Class_Plants_Fish CHECK CONSTRAINT FK_Aqua_Class_Plants_Fish_Aquarium_Classification", connection);
                        enableFKCommand.ExecuteNonQuery();
                    }

                    FishData.Remove(selectedFish);
                    FilteredFishtData.Remove(selectedFish);

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
            AquariumFish selectedFish = (AquariumFish)dataGrid.SelectedItem;

            if (selectedFish != null)
            {
                EditFishDialog editFishWindow = new EditFishDialog(selectedFish);
                editFishWindow.ShowDialog();

            }
        }


        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            AddFishDialog addfishWindow = new AddFishDialog();
            addfishWindow.Show();
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