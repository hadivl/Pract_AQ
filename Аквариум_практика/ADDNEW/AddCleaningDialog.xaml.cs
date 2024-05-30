using Google.Apis.Admin.Directory.directory_v1.Data;
using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class AddCleaningDialog : Window
    {
        private int _userId;

        public AddCleaningDialog()
        {
            InitializeComponent();
            LoadAquariumClassifications();
        }

        private void LoadAquariumClassifications()
        {
            string connectionString = "Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT IdAquariumClass, TypeConstruction FROM Aquarium_Classification";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ComboBoxItem item = new ComboBoxItem
                            {
                                Content = reader["TypeConstruction"].ToString(),
                                Tag = reader["IdAquariumClass"]
                            };
                            cmbAquariumClass.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False";

            try
            {
                string pumpingOutWater = txtPumpingOutWater.Text;
                string waterInlet = txtWaterInlet.Text;
                string equipPlantingFish = txtEquipPlantingFish.Text;
                string cleaning = txtCleaning.Text;
                string equipCatchingFish = txtEquipCatchingFish.Text;

                int idAquariumClass = (int)((ComboBoxItem)cmbAquariumClass.SelectedItem).Tag;

                AquariumCleaningEquipment newCleaning = new AquariumCleaningEquipment()
                {
                    PumpingOutWater = pumpingOutWater,
                    WaterInlet = waterInlet,
                    EquipPlantingFish = equipPlantingFish,
                    Cleaning = cleaning,
                    EquipCatchingFish = equipCatchingFish,
                    IdAquariumClass = idAquariumClass
                };

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Aquarium_Cleaning_Equipment(PumpingOutWater,WaterInlet,EquipPlantingFish,Cleaning,EquipCatchingFish,IdAquariumClass) " +
                        "VALUES (@PumpingOutWater,@WaterInlet,@EquipPlantingFish, @Cleaning, @EquipCatchingFish, @IdAquariumClass)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PumpingOutWater", newCleaning.PumpingOutWater);
                        command.Parameters.AddWithValue("@WaterInlet", newCleaning.WaterInlet);
                        command.Parameters.AddWithValue("@EquipPlantingFish", newCleaning.EquipPlantingFish);
                        command.Parameters.AddWithValue("@Cleaning", newCleaning.Cleaning);
                        command.Parameters.AddWithValue("@EquipCatchingFish", newCleaning.EquipCatchingFish);
                        command.Parameters.AddWithValue("@IdAquariumClass", newCleaning.IdAquariumClass);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Данные успешно добавлены в базу данных.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Cleaning cleaningPage = new Cleaning(_userId);
                cleaningPage.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Cleaning cleaningPage = new Cleaning(_userId);
            cleaningPage.Show();
            this.Hide();
        }
    }
}