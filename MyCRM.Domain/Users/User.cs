using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM.Domain.Users
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
    }
}
