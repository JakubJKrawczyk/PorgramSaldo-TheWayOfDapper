using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace ProgramSaldo_TheWayOfDapper.Controller
{
    internal class MainWindowController
    {
        internal void Rectangle_MouseDown(MainWindow w, object sender, MouseButtonEventArgs e)
        {
            if (w.IsSideMenuOpen)
            {

                w.SideMenu.Width = new System.Windows.GridLength(0);

                // Arrow Transformation

                RotateTransform rotate = new RotateTransform(0);
                rotate.CenterX = 40;
                rotate.CenterY = 30;
                w.MenuButton.RenderTransform = rotate;

                //


                //Hide Buttons
                w.Button1.Visibility = System.Windows.Visibility.Hidden;
                w.Button2.Visibility = System.Windows.Visibility.Hidden;
                w.Button3.Visibility = System.Windows.Visibility.Hidden;
                w.MenuButtons.Visibility = System.Windows.Visibility.Hidden;
                //
            }
            else
            {
                w.SideMenu.Width = new System.Windows.GridLength(300);

                // Arrow Transformation

                RotateTransform rotate = new RotateTransform(180);
                rotate.CenterX = 40;
                rotate.CenterY = 30;
                w.MenuButton.RenderTransform = rotate;

                //


                //Hide Buttons
                w.Button1.Visibility = System.Windows.Visibility.Visible;
                w.Button2.Visibility = System.Windows.Visibility.Visible;
                w.Button3.Visibility = System.Windows.Visibility.Visible;
                w.MenuButtons.Visibility = System.Windows.Visibility.Visible;
                //
            }

            w.IsSideMenuOpen = w.IsSideMenuOpen == true ? false : true;
        }

        internal void GenerateTableAndLoadData(DateTime date, string tableName)
        {

        }
    }
}
