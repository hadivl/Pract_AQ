using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Аквариум_практика.View
{
    public partial class Profil : Window
    {
        private int _userId;

        public Profil(int userId)
        {
            InitializeComponent();
            _userId = userId;
            Task.Run(async () =>
            {
                try
                {
                    var user = await GetUserFromDatabaseAsync(_userId);
                    if (user != null)
                    {
                        ShowUserDetails(user);
                    }
                    else
                    {
                        ShowErrorMessage("Пользователь не найден.");
                    }
                }
                catch (SqlException ex)
                {
                    ShowErrorMessage($"Ошибка подключения к базе данных: {ex.Message}");
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Произошла ошибка: {ex.Message}");
                }
            });
        }
        private async Task<Users> GetUserFromDatabaseAsync(int userId)
        {
            string connectionString = "Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM Users WHERE id_User = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new Users
                            {
                                LoginUser = reader.GetString(1),
                                NameUser = reader.GetString(2),
                                Password = reader.GetString(3)

                            };
                        }
                    }
                }
            }
            return null;
        }

        private void ShowUserDetails(Users user)
        {
            Dispatcher.Invoke(() =>
            {
                DataContext = user;
                Console.WriteLine($"Имя пользователя: {user.NameUser}");
                Console.WriteLine($"Логин пользователя: {user.LoginUser}");
            });
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Global globalWindow = new Global(_userId);
            globalWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Register registrationWindow = new Register();
            registrationWindow.Show();
            this.Close();
        }
    }
}
