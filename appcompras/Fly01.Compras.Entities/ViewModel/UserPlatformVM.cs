using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly01.Compras.Entities.ViewModel
{
    public class UserPlatformVM
    {
        public int Count { get; set; }
        public UserPlatformItemsVM[] Items { get; set; }
    }

    public class UserPlatformItemsVM
    {
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
