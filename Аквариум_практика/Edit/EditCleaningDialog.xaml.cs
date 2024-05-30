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
    /// Логика взаимодействия для EditCleaningDialog.xaml
    /// </summary>
    public partial class EditCleaningDialog : Window
    {
        public int IdCleaningEquipment { get; set; }
        public string PumpingOutWater { get; set; }
        public string WaterInlet { get; set; }
        public string EquipPlantingFish { get; set; }
        public string Cleaning { get; set; }
        public string EquipCatchingFish { get; set; }
        public int IdAquariumClass { get; set; }
        public string TypeConstruction { get; set; }

        private AquariumCleaningEquipment _cleaning;

        public EditCleaningDialog(AquariumCleaningEquipment cleaning)
        {
            InitializeComponent();
    
            if (cleaning != null)
            {
                _cleaning = cleaning;
                IdCleaningEquipment = cleaning.IdCleaningEquipment;
                PumpingOutWater = cleaning.PumpingOutWater;
                WaterInlet = cleaning.WaterInlet;
                EquipPlantingFish = cleaning.EquipPlantingFish;
                Cleaning = cleaning.Cleaning;
                EquipCatchingFish = cleaning.EquipCatchingFish;
                IdAquariumClass = cleaning.IdAquariumClass ?? 0; ;
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
            txtPumpingOutWater.Text = PumpingOutWater;
            txtWaterInlet.Text = WaterInlet;
            txtEquipPlantingFish.Text = EquipPlantingFish;
            txtCleaning.Text = Cleaning;
            txtEquipCatchingFish.Text = EquipCatchingFish;

            cbAquarium.IsEnabled = false;
            cbAquarium.ItemsSource = null; // Очищаем список элементов

            foreach (AquariumClassification aquarium in cbAquarium.Items)
            {
                if (aquarium.TypeConstruction == TypeConstruction)
                {
                    cbAquarium.SelectedItem = aquarium;
                    break;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            PumpingOutWater = txtPumpingOutWater.Text;
            WaterInlet = txtWaterInlet.Text;
            EquipPlantingFish = txtEquipPlantingFish.Text;
            Cleaning = txtCleaning.Text;
            EquipCatchingFish = txtEquipCatchingFish.Text;

            try
            {
                string connectionString = "Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE [Aquarium].[dbo].[Aquarium_Cleaning_Equipment] SET [PumpingOutWater] = @PumpingOutWater, [WaterInlet] = @WaterInlet, [EquipPlantingFish] = @EquipPlantingFish," +
                        " [Cleaning] = @Cleaning, [EquipCatchingFish] = @EquipCatchingFish, [IdAquariumClass] = @IdAquariumClass  WHERE [IdCleaningEquipment] = @IdCleaningEquipment";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdCleaningEquipment", IdCleaningEquipment);
                        command.Parameters.AddWithValue("@PumpingOutWater", PumpingOutWater);
                        command.Parameters.AddWithValue("@WaterInlet", WaterInlet);
                        command.Parameters.AddWithValue("@EquipPlantingFish", EquipPlantingFish);
                        command.Parameters.AddWithValue("@Cleaning", Cleaning);
                        command.Parameters.AddWithValue("@EquipCatchingFish", EquipCatchingFish);
                        command.Parameters.AddWithValue("@IdAquariumClass", IdAquariumClass);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                _cleaning.PumpingOutWater = PumpingOutWater;
                _cleaning.WaterInlet = WaterInlet;
                _cleaning.EquipPlantingFish = EquipPlantingFish;
                _cleaning.Cleaning = Cleaning;
                _cleaning.EquipCatchingFish = EquipCatchingFish;
                _cleaning.IdAquariumClass = IdAquariumClass;
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