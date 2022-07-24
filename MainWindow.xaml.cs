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
        public MainWindow()
        {
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
    }
}
