using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Permission
    {
        public Permission()
        {
            RolePermissions = new HashSet<RolePermission>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
