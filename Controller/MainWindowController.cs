using DapperDynamic;
using DapperDynamic.queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProgramSaldo_TheWayOfDapper.Controller
{
    internal class MainWindowController
    {

        DatabaseManager _manager = DatabaseManager.DBManagerInstance;

        Array[] _dataBehind;

        

        public MainWindowController()
        {
            
        }
        internal void Rectangle_MouseDown(MainWindow w, object sender, MouseButtonEventArgs e)
        {
            Button[] listOfButtons = { w.Button1, w.Button2, w.Button3 };
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
               

                foreach (Button button in listOfButtons)
                {
                    button.Visibility = System.Windows.Visibility.Hidden;
                }
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


                //Show Buttons
            

                foreach (Button button in listOfButtons)
                {
                    button.Visibility = System.Windows.Visibility.Visible;
                }
                w.MenuButtons.Visibility = System.Windows.Visibility.Visible;
                //
            }

            w.IsSideMenuOpen = w.IsSideMenuOpen == true ? false : true;
        }

        internal void GenerateTableAndLoadData(Window mainWindow, DateTime date )
        {
            //TODO: Zaimplementować działanie funkcji generującej tabelę danych do okna głównego ( Zadania do rozdrobniena )
        }
    }
}
