using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Permission
    {
        public Permission()
        {
            RolePermissions = new HashSet<RolePermission>();
            IsActive = true;
        }

        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive {  get; set; } 

        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
