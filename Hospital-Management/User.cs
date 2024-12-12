using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Management
{
    class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public User(string username, string password, string role)
        {
            Username = username;
            Password = password;
            Role = role;
        }

        static public List<User> users = new List<User>{
            new User("admin", "admin", "Administrator" ),
            new User("nurse", "nurse", "Nurse"),
            new User("doctor", "doctor", "Doctor" ),
            new User("receptionist", "receptionist", "Receptionist")
        };
    }
}
