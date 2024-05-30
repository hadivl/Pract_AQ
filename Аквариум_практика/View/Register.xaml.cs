using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace Аквариум_практика.View
{
    public partial class Register : Window
    {
        private const string ConnectionString = "Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False";

        public Register()
        {
            InitializeComponent();
        }

        private bool ValidateUsername(string username)
        {
            if (username.Length <= 5)
            {
                MessageBox.Show("Логин должен содержать более 5 символов.");
                return false;
            }

            return true;
        }

        private bool ValidatePassword(string password)
        {
            if (password.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать минимум 6 символов.");
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                MessageBox.Show("Пароль должен содержать минимум одну прописную букву.");
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                MessageBox.Show("Пароль должен содержать минимум одну цифру.");
                return false;
            }

            if (!password.Any(c => "!@#$%^".Contains(c)))
            {
                MessageBox.Show("Пароль должен содержать минимум один символ из набора: ! @ # $ % ^.");
                return false;
            }

            return true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string fullName = txtFullName.Text;
            string password = txtPassword.Password;
            string repeatPassword = txtRepeatPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(repeatPassword))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password != repeatPassword)
            {
                MessageBox.Show("Пароли не совпадают. Пожалуйста, введите одинаковые пароли.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!ValidatePassword(password))
            {
                return;
            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                int lastUserId = GetLastUserId(connection);

                // Увеличение последнего ID на 1
                int userId = lastUserId + 1;

                string query = "INSERT INTO Users (id_User, LoginUser, NameUser, Password) VALUES (@UserId, @Username, @FullName, @Password)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@FullName", fullName);
                    command.Parameters.AddWithValue("@Password", password);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Пользователь успешно зарегистрирован.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Login loginWindow = new Login();
                        this.Hide();
                        loginWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Произошла ошибка при регистрации пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private int GetLastUserId(SqlConnection connection)
        {
            string query = "SELECT MAX(id_User) FROM Users";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                object result = command.ExecuteScalar();
                if (result == DBNull.Value)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(result);
                }
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            this.Hide();
            loginWindow.Show();
        }

        private void txtUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
