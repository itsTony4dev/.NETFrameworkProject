using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CsharpProject
{
    public static class User
    {
        public static string Name { get; set; }

        public static string LastName { get; set; }
        public static int userID { get; set; }

        private static bool _isAdmin = false;
        public static bool isAdmin
        {
            get
            {
                return _isAdmin;
            }
            set
            {
                _isAdmin = value;

            }
        }

        public static bool _isLogedIn = false;
        public static bool isLogedIn
        {
            get
            {
                return _isLogedIn;
            }
            set
            {
                _isLogedIn = value;
            }
        }
       
    }
}
