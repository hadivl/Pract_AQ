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
    /// Логика взаимодействия для EditEquipmentsWindows.xaml
    /// </summary>
    public partial class EditEquipmentsDialog : Window
    {
        public int IdEquipment { get; set; }
        public string Lighting { get; set; }
        public string TemperatureControl { get; set; }
        public string WaterPurification { get; set; }
        public string AdditionalEquipment { get; set; }
        public int? IdAquariumClass { get; set; }
        public string TypeConstruction { get; set; }

        private Equipment _equipment;


        public EditEquipmentsDialog(Equipment equipments)
        {
            InitializeComponent();

            if (equipments != null)
            {
                _equipment = equipments;
                IdEquipment = equipments.IdEquipment;
                Lighting = equipments.Lighting;
                TemperatureControl = equipments.TemperatureControl;
                WaterPurification = equipments.WaterPurification;
                AdditionalEquipment = equipments.AdditionalEquipment;
                IdAquariumClass = equipments.IdAquariumClass ?? 0; ;
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
            txtLightingr.Text = Lighting;
            txtTemperatureControl.Text = TemperatureControl;
            txtWaterPurification.Text = WaterPurification;
            txtAdditionalEquipment.Text = AdditionalEquipment;

            cbAquarium.IsEnabled = false;
            cbAquarium.ItemsSource = null;

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
            Lighting = txtLightingr.Text;
            TemperatureControl = txtTemperatureControl.Text;
            WaterPurification = txtWaterPurification.Text;
            AdditionalEquipment = txtAdditionalEquipment.Text;

            try
            {
                string connectionString = "Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE [Aquarium].[dbo].[Equipment] SET [Lighting] = @Lighting, [TemperatureControl] = @TemperatureControl, [WaterPurification] = @WaterPurification," +
                        " [AdditionalEquipment] = @AdditionalEquipment, [IdAquariumClass] = @IdAquariumClass  WHERE [IdEquipment] = @IdEquipment";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdEquipment", IdEquipment);
                        command.Parameters.AddWithValue("@Lighting", Lighting);
                        command.Parameters.AddWithValue("@TemperatureControl", TemperatureControl);
                        command.Parameters.AddWithValue("@WaterPurification", WaterPurification);
                        command.Parameters.AddWithValue("@AdditionalEquipment", AdditionalEquipment);
                        command.Parameters.AddWithValue("@IdAquariumClass", IdAquariumClass);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                _equipment.Lighting = Lighting;
                _equipment.TemperatureControl = TemperatureControl;
                _equipment.WaterPurification = WaterPurification;
                _equipment.AdditionalEquipment = AdditionalEquipment;
                _equipment.IdAquariumClass = IdAquariumClass;
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