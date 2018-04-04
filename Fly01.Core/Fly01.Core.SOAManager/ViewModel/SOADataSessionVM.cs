using System;
using System.Collections.Generic;
using Fly01.Core.SOAManager.SOAManager;

namespace Fly01.Core.SOAManager.ViewModel
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
