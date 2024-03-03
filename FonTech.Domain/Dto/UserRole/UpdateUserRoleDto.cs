using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonTech.Domain.Dto.UserRole
{
    public class UpdateUserRoleDto
    {
        public string Login { get; set; }
        public long FromRoleId { get; set; }
        public long ToRoleId { get; set;}
    }
}
