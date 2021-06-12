using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeeTalk.Models
{
    public class UserRolesMultiModel
    {
        public UserRolesViewModel UserRolesViewModel { get; set; }
        public IEnumerable<UserRolesViewModel> UserRolesViewModelList { get; set; }
    }
}
