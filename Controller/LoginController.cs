using DapperDynamic;
using ProgramSaldo_TheWayOfDapper.Okna_Główne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProgramSaldo_TheWayOfDapper.Controller
{

    
    internal class LoginController
    {
        DatabaseManager? _manager;


        public LoginController()
        {
            DatabaseManager.Initialize("Saldo", "Kasia!@#", "saldo", "localhost", "3306");
            _manager = DatabaseManager.Instance;


            // if (!_manager._isTableExists("users", _manager.DataBaseName))
            // {
            //     _manager.CreateUsersTable();
            // }

        }

        public bool Login(System.Windows.Controls.TextBox login, System.Windows.Controls.TextBox password )
        {
            return _manager.ProcessLogin(login.Text, password.Text);
        }
    }
}
