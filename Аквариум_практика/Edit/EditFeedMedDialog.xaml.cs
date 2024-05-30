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

namespace Аквариум_практика.Edit
{
    /// <summary>
    /// Логика взаимодействия для EditFeedMedDialog.xaml
    /// </summary>
    public partial class EditFeedMedDialog : Window
    {
        public int IdFeedAndMedicines { get; set; }
        public string NameFeed { get; set; }
        public string TypeFeed { get; set; }
        public string NameMedicine { get; set; }
        public string TypeMedicine { get; set; }

        private FeedAndMedicines _feedmed;

        public EditFeedMedDialog(FeedAndMedicines feedmed)
        {
            InitializeComponent();
            if (feedmed != null)
            {
                _feedmed = feedmed;
                NameFeed = feedmed.NameFeed;
                TypeFeed = feedmed.TypeFeed;
                NameMedicine = feedmed.NameMedicine;
                TypeMedicine = feedmed.TypeMedicine;
                UpdateFields();
            }
            else
            {
                MessageBox.Show("Ошибка: Неверные данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
        private void UpdateFields()
        {
            NameFeedTextBox.Text = NameFeed;
            TypeFeedTextBox.Text = TypeFeed;
            NameMedTextBox.Text = NameMedicine;
            TypeMedTextBox.Text = TypeMedicine;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            NameFeed = NameFeedTextBox.Text;
            TypeFeed = TypeFeedTextBox.Text;
            NameMedicine = NameMedTextBox.Text;
            TypeMedicine = TypeMedTextBox.Text;

            try
            {
                string connectionString = "Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE [Aquarium].[dbo].[Feed_And_Medicines] SET [NameFeed] = @NameFeed," +
                        " [TypeFeed] = @TypeFeed, [NameMedicine] = @NameMedicine, [TypeMedicine]=@TypeMedicine" +
                        "  WHERE [IdFeedAndMedicines] = @IdFeedAndMedicines";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdFeedAndMedicines", IdFeedAndMedicines);
                        command.Parameters.AddWithValue("@NameFeed", NameFeed);
                        command.Parameters.AddWithValue("@TypeFeed", TypeFeed);
                        command.Parameters.AddWithValue("@NameMedicine", NameMedicine);
                        command.Parameters.AddWithValue("@TypeMedicine", TypeMedicine);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                _feedmed.NameFeed = NameFeed;
                _feedmed.TypeFeed = TypeFeed;
                _feedmed.NameMedicine = NameMedicine;
                _feedmed.TypeFeed = TypeFeed;

                UpdateFields();
                this.DialogResult = true;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547) 
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
