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

namespace ProgramSaldo_TheWayOfDapper.Additional_windows
{
    /// <summary>
    /// Logika interakcji dla klasy MyAccount.xaml
    /// </summary>
    public partial class MyAccount : Window
    {
        //TODO: Zaimplementować zmiany hasła dla użytkownika.

        public MyAccount()
        {
            InitializeComponent();
            login.Text = MainWindow._login;

        }

      
        private void Window_Closed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
