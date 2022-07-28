using ProgramSaldo_TheWayOfDapper.Controller;
using System;
using System.Collections.Generic;
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

namespace ProgramSaldo_TheWayOfDapper.Okna_Główne
{
    /// <summary>
    /// Logika interakcji dla klasy Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private LoginController _controller;
        public Button Buttonlogin { get; set; }
        public Login()
        {
            
        InitializeComponent();
           _controller = new LoginController();
            Buttonlogin = ButtonLogin;
            
        }

        private void LoginIn(object sender, RoutedEventArgs e)
        {
            if(_controller.Login(TextBoxLogin, TextBoxPassword))
            {
                MainWindow.IsLogged = true;
                MainWindow w = new MainWindow();
                w.Show();
                this.Hide();
            }
            else
            {
                LoginErrorTextBlock.Text = $"Zły login lub hasło -> Login: {TextBoxLogin.Text} Hasło: {TextBoxPassword.Text} ";
            };
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (MainWindow.IsLogged)
            {
                return;
            }
            else App.Current.Shutdown();
        }
    }
}
