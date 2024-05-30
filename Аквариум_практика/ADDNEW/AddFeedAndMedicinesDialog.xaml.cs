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
    /// Логика взаимодействия для AddFeedAndMedicinesDialog.xaml
    /// </summary>
    public partial class AddFeedAndMedicinesDialog : Window
    {
        private int _userId;

        public AddFeedAndMedicinesDialog()
        {
            InitializeComponent();
           
        } 
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False";

            try
            {
                string nameFeed = txtNameFood.Text;
                string typeFeed = txtTypeFood.Text;
                string nameMed = txtNameMed.Text;
                string typeMed = txtTypeMed.Text;

                FeedAndMedicines newFeedMed = new FeedAndMedicines()
                {
                    NameFeed = nameFeed,
                    TypeFeed = typeFeed,
                    NameMedicine = nameMed,
                    TypeMedicine = typeMed
                };

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Feed_And_Medicines(NameFeed,TypeFeed,NameMedicine,TypeMedicine) " +
                        "VALUES (@NameFeed,@TypeFeed,@NameMedicine, @TypeMedicine)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NameFeed", newFeedMed.NameFeed);
                        command.Parameters.AddWithValue("@TypeFeed", newFeedMed.TypeFeed);
                        command.Parameters.AddWithValue("@NameMedicine", newFeedMed.NameMedicine);
                        command.Parameters.AddWithValue("@TypeMedicine", newFeedMed.TypeMedicine);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Данные успешно добавлены в базу данных.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
               
                FoodMedicine foodmedPage = new FoodMedicine(_userId);
                foodmedPage.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            FoodMedicine foodmedPage = new FoodMedicine(_userId);
            foodmedPage.Show();
            this.Hide();
        }
    }
}