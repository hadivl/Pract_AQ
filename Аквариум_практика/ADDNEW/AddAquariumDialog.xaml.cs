using Google.Apis.Admin.Directory.directory_v1.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class AddAquariumDialog : Window
    {
      
        private int _userId;

        public AddAquariumDialog()
        {
            InitializeComponent();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False";

            string typeConstruction= txtTypeConstruction.Text;
           
                if (Regex.IsMatch(typeConstruction, @"^\d+$"))
                {
                    MessageBox.Show("Поле 'Тип конструктора' не должно содержать цифры. Пожалуйста, введите корректные данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            

            string location = txtLocation.Text;
            if (Regex.IsMatch(location, @"^\d+$"))
            {
                MessageBox.Show("Поле 'Местоположение' не должно содержать цифры. Пожалуйста, введите корректные данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string scope = txtScope.Text;
            if (Regex.IsMatch(scope, @"^\d+$"))
            {
                MessageBox.Show("Поле 'Тип размера' не должно содержать цифры. Пожалуйста, введите корректные данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string shape = txtShape.Text;
            if (Regex.IsMatch(shape, @"^\d+$"))
            {
                MessageBox.Show("Поле 'Форма' не должно содержать цифры. Пожалуйста, введите корректные данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int volume = 0;
            if (!int.TryParse(txtVolume.Text, out volume))
            {
                MessageBox.Show("Пожалуйста, введите корректное значение для объема.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } 

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Aquarium_Classification (TypeConstruction, Location, Scope, Shape, Volume) VALUES (@TypeConstruction, @Location, @Scope, @Shape, @Volume)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TypeConstruction", typeConstruction);
                        command.Parameters.AddWithValue("@Location", location);
                        command.Parameters.AddWithValue("@Scope", scope);
                        command.Parameters.AddWithValue("@Shape", shape);
                        command.Parameters.AddWithValue("@Volume", volume);

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

            Aquarium aquariumPage = new Aquarium(_userId);
            aquariumPage.Show();
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Aquarium aquariumPage = new Aquarium(_userId);
            aquariumPage.Show();
            this.Hide();
        }
    }

}

