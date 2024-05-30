using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Аквариум_практика.View
{
    public partial class Login : Window
    {
        private const string ConnectionString = "Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False";

        public Login()
        {
            InitializeComponent();
        }

        private bool ValidateInput(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.");
                return false;
            }
            return true;
        }

        private int AuthenticateUser(string username, string password)
        {
            string query = "SELECT id_User FROM Users WHERE LoginUser = @Username AND Password = @Password";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    object result = command.ExecuteScalar();
                    return result == null ? -1 : Convert.ToInt32(result);
                }
            }
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            string username = txtLogin.Text;
            string password = txtPassword.Password;

            if (!ValidateInput(username, password))
            {
                return;
            }

            try
            {
                int userId = AuthenticateUser(username, password);
                if (userId != -1)
                {
                    MessageBox.Show("Авторизация успешна.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    Profil profilWindow = new Profil(userId);
                    this.Hide();
                    profilWindow.Show();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Button_Click_Register(object sender, RoutedEventArgs e)
        {
            Register registerWindow = new Register();
            this.Hide();
            registerWindow.Show();
        }
    }
}
