using Project.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.ViewModel.System.Users
{
    public class RoleAssignRequest
    {
        public Guid Id { get; set; }
        public IList<SelectItem> Roles { get; set; } = new List<SelectItem>();
    }
}