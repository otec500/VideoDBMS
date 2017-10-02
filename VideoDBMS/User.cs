using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoDBMS
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public int Status { get; set; }

        public User()
        {

        }
        public User(User xUser)
        {
            Id = xUser.Id;
            Name = xUser.Name;
            Password = xUser.Password;
            BirthDate = xUser.BirthDate;
            Status = xUser.Status;
        }

    }
}
