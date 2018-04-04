using Fly01.Estoque.SOAManager;
using System;
using System.Collections.Generic;

namespace Fly01.Estoque.Models.ViewModel
{
    [Serializable]
    public class SOADataSessionVM
    {
        public SOADataSessionVM()
        {
            DataList = new List<SOAData>();
        }

        public List<SOAData> DataList { get; set; }

        public string CurrentState { get; set; }
        public string ServiceExecutionId { get; set; }
    }
}