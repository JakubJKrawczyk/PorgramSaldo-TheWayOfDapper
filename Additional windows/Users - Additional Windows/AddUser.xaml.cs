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

namespace ProgramSaldo_TheWayOfDapper.Additional_windows.Users___Additional_Windows
{
    /// <summary>
    /// Logika interakcji dla klasy AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        private Controller.AddUserController controller;
        private bool _isVisible = true;

        private ImageBrush _openedEyeImage;
        private ImageBrush _closedEyeImage;
        public AddUser()
        {
            InitializeComponent();

            controller = new Controller.AddUserController();


            //images

            //Open eye
            _openedEyeImage = new ImageBrush();
            Image image = new Image();
            image.Source = new BitmapImage(
                new Uri(
                   "C:/Users/JJK/Desktop/ProgramSaldo-TheWayOfDapper/Additional windows/Users - Additional Windows/eye-opened.png"));
            _openedEyeImage.ImageSource = image.Source;

            // Closed eye

            image.Source = new BitmapImage(
                new Uri(
                   "C:/Users/JJK/Desktop/ProgramSaldo-TheWayOfDapper/Additional windows/Users - Additional Windows/eye-closed.png"));
            _closedEyeImage = new ImageBrush
            {
                ImageSource = image.Source
            };


            ChangePasswordVisibility(this,new RoutedEventArgs());


        }


        ///<summary>
        ///Event zmiany jawności wyświetlania hasła. Użycie tej funkcji przełącza widoczność hasła w oknie dodawania nowego użytkownika z ***** na widok jawny
        ///</summary>
        ///<param name="e">Argumenty eventu</param>
        ///<param name="sender">obiekt wywołujący event. przeważnie "this"</param>
        private void ChangePasswordVisibility(object sender, RoutedEventArgs e) 
        {
            if (_isVisible)
            {
                ShowPasswordBtn.Background = _openedEyeImage;

                UnmaskPasswordBox.Visibility = Visibility.Hidden;
                UnmaskRepeatPasswordBox.Visibility = Visibility.Hidden;

                PasswordBox.Password = UnmaskPasswordBox.Text;
                RepeatPasswordBox.Password = UnmaskRepeatPasswordBox.Text;
                PasswordBox.Visibility = Visibility.Visible;
                RepeatPasswordBox.Visibility = Visibility.Visible;
            }
            else
            {
                ShowPasswordBtn.Background = _closedEyeImage;


                PasswordBox.Visibility = Visibility.Hidden;
                RepeatPasswordBox.Visibility = Visibility.Hidden;

                UnmaskPasswordBox.Text = PasswordBox.Password;
                UnmaskRepeatPasswordBox.Text = RepeatPasswordBox.Password;

                UnmaskPasswordBox.Visibility = Visibility.Visible;
                UnmaskRepeatPasswordBox.Visibility = Visibility.Visible;
            }

            _isVisible = _isVisible == true ? false : true;

        }
        /// <summary>
        /// Funkcja dodająca uprawnienia użytkownikowi podczas jego tworzenia.
        /// </summary>
        /// <param name="sender">Obiekt wywołujący event</param>
        /// <param name="e">Parametry wywołania</param>
        private void AddPrivilige(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// Fukcja usuwająca uprawnienia użytkownika podczas jego tworzenia
        /// </summary>
        /// <param name="sender">Obiekt wywołujący event</param>
        /// <param name="e">Parametry wywołania</param>
        private void DeletePrivilige(object sender, RoutedEventArgs e)
        {

        }
        //TODO: Zaimplementować obsługę controllera AddUserController
    }
      

    
    
}
