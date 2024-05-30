using Google.Apis.Admin.Directory.directory_v1.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using Аквариум_практика.Clients;

namespace Аквариум_практика.ADDNEW
{
    /// <summary>
    /// Логика взаимодействия для AddFishDialog.xaml
    /// </summary>
    public partial class AddFishDialog : Window
    {
        private int _userId;

        public AddFishDialog()
        {
            InitializeComponent();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False";

            string name = txtName.Text;
            string waterTemperature = txtwaterTemperature.Text;
            string pHOfThewater = txtpHOfThewater.Text;
            int maximumSize = 0;

            // Проверка, что поле Volume содержит корректное целочисленное значение
            if (!int.TryParse(txtmaximumSize.Text, out maximumSize))
            {
                MessageBox.Show("Пожалуйста, введите корректное значение для объема.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string requiredVolumeAquarium = txtrequiredVolumeAquarium.Text;
            string theNeedShelters = txttheNeedShelters.Text;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Aquarium_Fish (Name, WaterTemperature, pHOfTheWater, MaximumSize, RequiredVolumeAquarium,TheNeedShelters ) " +
                        "VALUES (@Name, @WaterTemperature, @pHOfTheWater, @MaximumSize, @RequiredVolumeAquarium, @TheNeedShelters)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@WaterTemperature", waterTemperature);
                        command.Parameters.AddWithValue("@pHOfTheWater", pHOfThewater);
                        command.Parameters.AddWithValue("@MaximumSize", maximumSize);
                        command.Parameters.AddWithValue("@RequiredVolumeAquarium", requiredVolumeAquarium);
                        command.Parameters.AddWithValue("@TheNeedShelters", theNeedShelters);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Данные успешно добавлены в таблицу.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                // Дополнительные действия после успешного добавления данных

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Fish fishPage = new Fish(_userId);
            fishPage.Show();
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Fish fishPage = new Fish(_userId);
            fishPage.Show();
            this.Hide();
        }
    }

}

