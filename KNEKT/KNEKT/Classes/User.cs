using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KNEKT.Classes
{
                                                                                            /// <summary>
                                                                                            /// Used to store users in the User management interface
                                                                                            /// </summary>
    class User
    {
        private string _UserName;
        private string _FirstName;
        private string _LastName;
        private string _Password;
        private int _UserLevel;

        public User(string username, string firstname, string lastname, string password, int userlevel)
        {
            Username = username;
            FirstName = firstname;
            LastName = lastname;
            Password = password;
            UserLevel = userlevel;
        }

        public string Username
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
            }
        }

        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                _LastName = value;
            }
        }

        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
            }
        }

        public int UserLevel
        {
            get
            {
                return _UserLevel;
            }
            set
            {
                _UserLevel = value;
            }
        }

    }
}
