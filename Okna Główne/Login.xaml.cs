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
                MainWindow._login = TextBoxLogin.Text;
                MainWindow._password = TextBoxPassword.Text;
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

        private void TextBoxLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            if(TextBoxLogin.Text == "Login") TextBoxLogin.Text = "";
        }

        private void TextBoxLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if(TextBoxLogin.Text == "")
            {
                TextBoxLogin.Text = "Login";
            }
        }

        private void TextBoxPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxPassword.Text == "Password") TextBoxPassword.Text = "";
        }

        private void TextBoxPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxPassword.Text == "")
            {
                TextBoxPassword.Text = "Password";
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                LoginIn(this, new RoutedEventArgs()) ;
            }
        }
    }
}
