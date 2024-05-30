using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Аквариум_практика.View;

namespace Аквариум_практика
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            /* Открываем окно регистрации Register.xaml при запуске приложения */
            Register registerWindow = new Register();
            registerWindow.Show();


        }
    }
}