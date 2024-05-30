using Аквариум_практика.Edit;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using Аквариум_практика.View;
using Аквариум_практика.ADDNEW;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.IO;

//using Microsoft.Office.Interop.Excel;
using System.Data;
using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using Google.Apis.Admin.Directory.directory_v1.Data;

namespace Аквариум_практика.Clients
{
    /// <summary>
    /// Логика взаимодействия для Plants.xaml
    /// </summary>
    public partial class Plants : Window
    {
        public ObservableCollection<PlantsForAquariums> PlantsAquariumsData { get; set; } = new ObservableCollection<PlantsForAquariums>();
        public ObservableCollection<PlantsForAquariums> FilteredPlantsAquariumsData { get; set; } = new ObservableCollection<PlantsForAquariums>();
        private PlantsForAquariums selectedPlant;
        private Button editButton;
        private Button deleteButton;
        private int _userId;

        public Plants(int userId)
        {
            InitializeComponent();
            LoadPlantsAquariumsData();
            FilteredPlantsAquariumsData = new ObservableCollection<PlantsForAquariums>(PlantsAquariumsData);
            DataContext = this;
            _userId = userId;

            this.Loaded += (s, e) =>
            {
                editButton = (Button)FindName("EditButton");
                deleteButton = (Button)FindName("DeleteButton");
            };
        }
        private void LoadPlantsAquariumsData()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT [IdPlantsAquariums], [Type], [LeafLength], [IdAquaFish] FROM [Aquarium].[dbo].[Plants_For_Aquariums]", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        PlantsAquariumsData.Add(new PlantsForAquariums
                        {
                            IdPlantsAquariums = reader.GetInt32(0),
                            Type = reader.IsDBNull(1) ? "" : reader.GetString(1),
                            LeafLength = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                            IdAquaFish = reader.IsDBNull(3) ? 0 : reader.GetInt32(3)
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
            FilteredPlantsAquariumsData.Clear();

            if (string.IsNullOrEmpty(searchText))
            {
                FilteredPlantsAquariumsData = new ObservableCollection<PlantsForAquariums>(PlantsAquariumsData);
            }
            else
            {
                foreach (PlantsForAquariums item in PlantsAquariumsData)
                {
                    if (item.Type.ToLower().Contains(searchText) ||
                        item.LeafLength.ToString().Contains(searchText) ||
                        item.IdAquaFish.ToString().Contains(searchText))
                    {
                        FilteredPlantsAquariumsData.Add(item);
                    }
                }
            }
        }
        private void PlantsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                if (dataGrid.SelectedItem is PlantsForAquariums selectedPlant)
                {
                    if (EditButton != null && deleteButton != null)
                    {
                        EditButton.IsEnabled = true;
                        deleteButton.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Кнопки не найдены.");
                    }
                }
                else
                {
                    MessageBox.Show("Выбранный элемент не является растением.");
                    EditButton.IsEnabled = false;
                    deleteButton.IsEnabled = false;
                }
            }
            else
            {
                EditButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            PlanAdd planaddWindow = new PlanAdd();
            planaddWindow.Show();
            Close();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите растение для редактирования.");
                return;
            }
            PlantsForAquariums selectedPlan = (PlantsForAquariums)dataGrid.SelectedItem;

            if (selectedPlan != null)
            {
                EditPlantDialog editPlanWindow = new EditPlantDialog(selectedPlan);
                editPlanWindow.ShowDialog();

            }
        }
        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите запись для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            PlantsForAquariums selectedPlant = (PlantsForAquariums)dataGrid.SelectedItem;

            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection("Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM [Aquarium].[dbo].[Plants_For_Aquariums] WHERE [IdPlantsAquariums] = @IdPlantsAquariums", connection))
                    {
                        command.Parameters.AddWithValue("@IdPlantsAquariums", selectedPlant.IdPlantsAquariums);
                        command.ExecuteNonQuery();
                    }
                }

                PlantsAquariumsData.Remove(selectedPlant);
                FilteredPlantsAquariumsData.Remove(selectedPlant);

                dataGrid.SelectedItem = null;
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
        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите растение для редактирования.");
                return;
            }
            PlantsForAquariums selectedPlan = (PlantsForAquariums)dataGrid.SelectedItem;

            if (selectedPlan != null)
            {
                EditPlantDialog editPlanWindows= new EditPlantDialog(selectedPlan);
                editPlanWindows.ShowDialog();

            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            PlanAdd addplanWindow = new PlanAdd();
            addplanWindow.Show();
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
