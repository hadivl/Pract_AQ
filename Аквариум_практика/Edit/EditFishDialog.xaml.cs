using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel;
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

namespace Аквариум_практика.Edit
{
    /// <summary>
    /// Логика взаимодействия для EditFishDialog.xaml
    /// </summary>
    public partial class EditFishDialog : Window
    {
        public int IdAquaFish { get; set; }
        public string Name { get; set; }
        public int MaximumSize { get; set; }
        public int WaterTemperature { get; set; }
        public string pHOfTheWater { get; set; }
        public int RequiredVolumeAquarium { get; set; }
        public string TheNeedShelters { get; set; }

        private AquariumFish _fish;
        public EditFishDialog(AquariumFish fish)
        {
            InitializeComponent();
            if (fish != null)
            {
                _fish = fish;
                IdAquaFish = fish.IdAquaFish;
                Name = fish.Name;
                MaximumSize = fish.MaximumSize;
                WaterTemperature = fish.WaterTemperature;
                pHOfTheWater = fish.pHOfTheWater;
                RequiredVolumeAquarium = fish.RequiredVolumeAquarium;
                TheNeedShelters = fish.TheNeedShelters;
                UpdateFields();
            }
            else
            {
                MessageBox.Show("Ошибка: Неверные данные об аквариуме.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
        private void UpdateFields()
        {
            txtName.Text = Name;
            txtmaximumSize.Text = MaximumSize.ToString();
            txtwaterTemperature.Text = WaterTemperature.ToString();
            txtpHOfThewater.Text = pHOfTheWater;
            txtrequiredVolumeAquarium.Text = RequiredVolumeAquarium.ToString();
            txttheNeedShelters.Text = TheNeedShelters;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Name = txtName.Text;
            if (!int.TryParse(txtmaximumSize.Text, out int maximumSize))
            {
                MessageBox.Show("Некорректное значение для 'Максимального размера'.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MaximumSize = maximumSize;
            if (!int.TryParse(txtwaterTemperature.Text, out int waterTemperature))
            {
                MessageBox.Show("Некорректное значение для 'Максимального размера'.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            WaterTemperature = waterTemperature;
            pHOfTheWater = txtpHOfThewater.Text;
            if (!int.TryParse(txtrequiredVolumeAquarium.Text, out int requiredVolumeAquarium))
            {
                MessageBox.Show("Некорректное значение для 'Максимального размера'.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            RequiredVolumeAquarium = requiredVolumeAquarium;
            TheNeedShelters = txttheNeedShelters.Text;

            try
            {
                string connectionString = "Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE [Aquarium].[dbo].[Aquarium_Fish] SET [Name] = @Name, [MaximumSize] = @MaximumSize," +
                        " [WaterTemperature] = @WaterTemperature, [pHOfTheWater] = @pHOfTheWater, [RequiredVolumeAquarium] = @RequiredVolumeAquarium, [TheNeedShelters] = @TheNeedShelters WHERE [IdAquaFish] = @IdAquaFish";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", Name);
                        command.Parameters.AddWithValue("@MaximumSize", MaximumSize);
                        command.Parameters.AddWithValue("@WaterTemperature", WaterTemperature);
                        command.Parameters.AddWithValue("@pHOfTheWater", pHOfTheWater);
                        command.Parameters.AddWithValue("@RequiredVolumeAquarium", RequiredVolumeAquarium);
                        command.Parameters.AddWithValue("@TheNeedShelters", TheNeedShelters);
                        command.Parameters.AddWithValue("@IdAquaFish", IdAquaFish);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                _fish.Name = Name;
                _fish.MaximumSize = MaximumSize;
                _fish.WaterTemperature = WaterTemperature;
                _fish.pHOfTheWater = pHOfTheWater;
                _fish.RequiredVolumeAquarium = RequiredVolumeAquarium;
                _fish.TheNeedShelters = TheNeedShelters;
                UpdateFields();
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
