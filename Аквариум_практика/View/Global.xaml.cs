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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Аквариум_практика.Clients;





/// <summary>
/// Логика взаимодействия для Global.xaml
/// </summary>
namespace Аквариум_практика.View
    {
        public partial class Global : Window
        {
   
        private int _userId;
        

        public Global(int userId)
            {
                InitializeComponent();
              
                _userId = userId;
        }

            private void FishButton_Click(object sender, RoutedEventArgs e)
            {
            Fish fishPage = new Fish(_userId);
            fishPage.Show();
            this.Hide();
        }

        private void CleaningButton_Click(object sender, RoutedEventArgs e)
        {
            Cleaning cleaningPage = new Cleaning(_userId);
            cleaningPage.Show();
            this.Hide();
        }
        private void AquariumButton_Click(object sender, RoutedEventArgs e)
        {
            Aquarium aquariumPage = new Aquarium(_userId);
            aquariumPage.Show();
            this.Hide();
        }
        private void PlantsButton_Click(object sender, RoutedEventArgs e)
        {
            Plants plantsPage = new Plants(_userId);
            plantsPage.Show();
            this.Hide();
        }
        private void FoodMedicineButton_Click(object sender, RoutedEventArgs e)
        {
            FoodMedicine foodmedPage = new FoodMedicine(_userId);
            foodmedPage.Show();
            this.Hide();
        }
        private void EquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            Equipments equipmentPage = new Equipments(_userId);
            equipmentPage.Show();
            this.Hide();
        }
        private void UserIconButton_Click(object sender, RoutedEventArgs e)
        {

            Profil profilePage = new Profil(_userId); 
            profilePage.Show();
            this.Hide();
        }

       
    }
}
