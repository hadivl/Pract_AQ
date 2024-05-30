using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using Аквариум_практика.View;
using Аквариум_практика.Clients;

namespace Аквариум_практика.Edit
{
    public partial class EditPlantDialog : Window
    {
        public int IdPlantsAquariums { get; set; }
        public string Type { get; set; }
        public int LeafLength { get; set; }
        public int IdAquaFish { get; set; }

        private PlantsForAquariums _plants;

        public EditPlantDialog(PlantsForAquariums plant)
        {
            InitializeComponent();

            if (plant != null)
            {
                _plants = plant;
                IdPlantsAquariums = plant.IdPlantsAquariums;
                Type = plant.Type;
                LeafLength = plant.LeafLength;
                IdAquaFish = plant.IdAquaFish;
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
            TypeTextBox.Text = Type;
            LeafLengthTextBox.Text = LeafLength.ToString();


            cbAquaFish.IsEnabled = false;
            cbAquaFish.ItemsSource = null; // Очищаем список элементов

            foreach (AquariumFish fish in cbAquaFish.Items)
            {
                if (fish.Name == Name)
                {
                    cbAquaFish.SelectedItem = fish;
                    break;
                }
            }
        }
            private void SaveButton_Click(object sender, RoutedEventArgs e)
            {

            Type = TypeTextBox.Text;

            int.TryParse(LeafLengthTextBox.Text, out int leafLength);
            LeafLength = leafLength;

            try
            {
                string connectionString = "Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE [Aquarium].[dbo].[Plants_For_Aquariums] SET [Type] = @Type, [LeafLength] = @LeafLength, [IdAquaFish] = @IdAquaFish  WHERE [IdPlantsAquariums] = @IdPlantsAquariums";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdPlantsAquariums", IdPlantsAquariums);
                        command.Parameters.AddWithValue("@Type", Type);
                        command.Parameters.AddWithValue("@LeafLength", LeafLength);
                        command.Parameters.AddWithValue("@IdAquaFish", IdAquaFish);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                _plants.Type = Type;
                _plants.LeafLength = LeafLength;
                _plants.IdAquaFish = IdAquaFish;

                UpdateFields();
                this.DialogResult = true;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547) // Проверяем код ошибки на конфликт внешнего ключа
                {
                    MessageBox.Show("Ошибка: конфликт внешнего ключа при обновлении данных. Данные могут быть обновлены частично.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Произошла ошибка при обновлении данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
