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
    /// Logika interakcji dla klasy Users.xaml
    /// </summary>
    public partial class Users : Window
    {
        private Window _adduser;
        public Users()
        {
            InitializeComponent();
            _adduser = new Users___Additional_Windows.AddUser();
        }

       

        private void Window_Closed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();

        }

        private void Add_User(object sender, RoutedEventArgs e)
        {
            if (_adduser.IsVisible)
            {
                _adduser.Focus();
            }
            else
            {
                _adduser.Show();
            }

            
        }

        private void Del_User(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
