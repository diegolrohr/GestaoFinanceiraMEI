using System;
using System.Collections.Generic;

namespace Fly01.Core.Entities.ViewModels
{
    [Serializable]
    public class SystemValueVM
    {
        public string Name { get; set; }
        public List<KeyValueVM> Values { get; set; }
    }
}
