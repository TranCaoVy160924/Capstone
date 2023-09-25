using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities;

public class Role : IdentityRole<Guid>
{
    //public Guid roleID { get; set; }
    // public string roleName { get; set; }
    [Column(TypeName = "varchar(350)")]
    public string Description { get; set; }
    public bool isDeactive { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }
}
