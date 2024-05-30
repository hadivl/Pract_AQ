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
    /// Логика взаимодействия для AddEquipmentsDialog.xaml
    /// </summary>
    public partial class AddEquipmentsDialog : Window
    {
        private int _userId;

        public AddEquipmentsDialog()
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
                string lightingr = txtLightingr.Text;
                string temperatureControl = txtTemperatureControl.Text;
                string equipPlantingFish = txtWaterPurification.Text;
                string additional = txtAdditionalEquipment.Text;

                int idAquariumClass = (int)((ComboBoxItem)cmbAquariumClass.SelectedItem).Tag;

                Equipment newEquipment = new Equipment()
                {
                    Lighting = lightingr,
                    TemperatureControl = temperatureControl,
                    WaterPurification = equipPlantingFish,
                    AdditionalEquipment = additional,
                    IdAquariumClass = idAquariumClass
                };

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Equipment(Lighting,TemperatureControl,WaterPurification,AdditionalEquipment,IdAquariumClass) " +
                        "VALUES (@Lighting,@TemperatureControl,@WaterPurification, @AdditionalEquipment, @IdAquariumClass)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Lighting", newEquipment.Lighting);
                        command.Parameters.AddWithValue("@TemperatureControl", newEquipment.TemperatureControl);
                        command.Parameters.AddWithValue("@WaterPurification", newEquipment.WaterPurification);
                        command.Parameters.AddWithValue("@AdditionalEquipment", newEquipment.AdditionalEquipment);
                        command.Parameters.AddWithValue("@IdAquariumClass", newEquipment.IdAquariumClass);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Данные успешно добавлены в базу данных.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Equipments equipmentsgPage = new Equipments(_userId);
                equipmentsgPage.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Equipments equipmentsgPage = new Equipments(_userId);
            equipmentsgPage.Show();
            this.Hide();
        }
    }
}