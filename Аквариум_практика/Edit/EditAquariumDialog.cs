using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Аквариум_практика.Clients;
using Аквариум_практика.View;

namespace Аквариум_практика.Edit
{
    public partial class EditAquariumDialog : Window
    {
        public int IdAquariumClass { get; set; }
        public string TypeConstruction { get; set; }
        public string Location { get; set; }
        public string Scope { get; set; }
        public string Shape { get; set; }
        public int Volume { get; set; }
        private AquariumClassification _aquarium;
        public EditAquariumDialog(AquariumClassification aquarium)
        {
            InitializeComponent();
            if (aquarium != null)
            {
                _aquarium = aquarium;
                TypeConstruction = aquarium.TypeConstruction;
                Location = aquarium.Location;
                Scope = aquarium.Scope;
                Shape = aquarium.Shape;
                Volume = aquarium.Volume;
                IdAquariumClass = aquarium.IdAquariumClass;
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
            txtTypeConstruction.Text = TypeConstruction;
            txtLocation.Text = Location;
            txtScope.Text = Scope;
            txtShape.Text = Shape;
            txtVolume.Text = Volume.ToString();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            TypeConstruction = txtTypeConstruction.Text;
            Location = txtLocation.Text;
            Scope = txtScope.Text;
            Shape = txtShape.Text;
            if (!int.TryParse(txtVolume.Text, out int volume))
            {
                MessageBox.Show("Некорректное значение для 'Объем'.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Volume = volume;
            try
            {
                string connectionString = "Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE [Aquarium].[dbo].[Aquarium_Classification] SET [TypeConstruction] = @TypeConstruction, [Location] = @Location, [Scope] = @Scope, [Shape] = @Shape, [Volume] = @Volume WHERE [IdAquariumClass] = @IdAquariumClass";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TypeConstruction", TypeConstruction);
                        command.Parameters.AddWithValue("@Location", Location);
                        command.Parameters.AddWithValue("@Scope", Scope);
                        command.Parameters.AddWithValue("@Shape", Shape);
                        command.Parameters.AddWithValue("@Volume", Volume);
                        command.Parameters.AddWithValue("@IdAquariumClass", IdAquariumClass);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                _aquarium.TypeConstruction = TypeConstruction;
                _aquarium.Location = Location;
                _aquarium.Scope = Scope;
                _aquarium.Shape = Shape;
                _aquarium.Volume = Volume;
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