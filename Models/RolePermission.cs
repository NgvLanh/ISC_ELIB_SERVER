using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class RolePermission
    {
        public long Id { get; set; }
        public long PermissionId { get; set; }
        public long RoleId { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual Permission Permission { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
