using ProgramSaldo_TheWayOfDapper.Controller;
using ProgramSaldo_TheWayOfDapper.Okna_Główne;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProgramSaldo_TheWayOfDapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool IsSideMenuOpen ;
        public static bool IsLogged { get; set; } = false;
        private MainWindowController _controller; 
        private Dictionary<string, Window> _addWin = new Dictionary<string, Window>();
        internal static string _login;
        internal static string _password;
        internal static DateTime _date;

        public MainWindow()
        {
            _addWin["myAccount"] = new Additional_windows.MyAccount();
            _addWin["columns"] = new Additional_windows.Columns();
            _addWin["users"] = new Additional_windows.Users();

            _date = DateTime.Now;

            IsSideMenuOpen = false;
            InitializeComponent();
            _controller = new MainWindowController();
            if (!IsLogged)
            {
            Login w = new Login();
            w.Show();
            this.Hide();

            }
            
        }

        

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => _controller.Rectangle_MouseDown(this, sender, e);

        private void MWindow_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if (!_addWin["myAccount"].IsVisible) {
                _addWin["myAccount"].Show();
            }
            else
            {
                _addWin["myAccount"].Focus();
            }
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            if (!_addWin["columns"].IsVisible)
            {
                _addWin["columns"].Show();
            }
            else
            {
                _addWin["columns"].Focus();
            }
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            if (!_addWin["users"].IsVisible)
            {
                _addWin["users"].Show();
            }
            else
            {
                _addWin["users"].Focus();
            }
        }
    }
}
