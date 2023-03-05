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

    //TODO: Zaimplementować logikę dodawania i usuwania kolumn z tabeli.
    /// <summary>
    /// Logika interakcji dla klasy Columns.xaml
    /// </summary>
    public partial class Columns : Window
    {
        public Columns()
        {
            InitializeComponent();
        }

      

        private void Window_Closed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void DellColumn_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddColumn_Btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
