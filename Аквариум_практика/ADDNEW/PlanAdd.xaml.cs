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
    public partial class PlanAdd : Window
    {
        private int _userId;

        public PlanAdd()
        {
            InitializeComponent();
            FillFishComboBox();
        }
        private void FillFishComboBox()
        {
            string connectionString = "Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT IdAquaFish, Name FROM Aquarium_Fish";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            IdAquaFishComboBox.Items.Add(new ComboBoxItem
                            {
                                Content = reader["Name"].ToString(),
                                Tag = reader["IdAquaFish"]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при заполнении ComboBox: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False";
            try
            {              
                string type = TypeTextBox.Text;
                int leaflength;
                if (!int.TryParse(LeafLengthTextBox.Text, out leaflength))
                {
                    MessageBox.Show("Введите корректную длину листа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                int idAquaFish = (int)((ComboBoxItem)IdAquaFishComboBox.SelectedItem).Tag;

                PlantsForAquariums newPlant = new PlantsForAquariums()
                {
                    Type = type,
                    LeafLength = leaflength,
                    IdAquaFish = idAquaFish
                };

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Plants_For_Aquariums(Type, LeafLength, IdAquaFish) " +
                        "VALUES (@Type, @LeafLength, @IdAquaFish)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Type", newPlant.Type);
                        command.Parameters.AddWithValue("@LeafLength", newPlant.LeafLength);
                        command.Parameters.AddWithValue("@IdAquaFish", newPlant.IdAquaFish);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Данные успешно добавлены в базу данных.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Plants plantsPage = new Plants(_userId);
                plantsPage.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Plants plantsPage = new Plants(_userId);
            plantsPage.Show();
            this.Hide();
        }
    }
}
