using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM.Application.DTOs
{
    public class AuthenticatedUserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}
