using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Fly01.Core;
using Fly01.Core.Helpers;

namespace Fly01.Financeiro.Models.Tasks.Base
{
    public abstract class TaskExportItemBase
    {
        public Guid Id { get; set; }
        public int TotalRecords { get; set; }
        public int TotalRecordsProcess { get; set; }
        public MemoryStream Stream { get; set; }
        public int Percent { get; set; }

        public TaskExportItemBase(Guid id)
        {
            Id = id;
        }

        public int GetPercent()
        {
            int percent = 0;

            if (TotalRecords > 0)
                percent = (int)Math.Round((double)(100 * TotalRecordsProcess) / TotalRecords);
            else if (TotalRecords == -1 && TotalRecordsProcess == -1) //filtro não foi atendido (zero registros na pesquisa)
                percent = 100;

            return percent;
        }

        public Dictionary<string, string> ProcessQueryString(string queryString)
        {
            Dictionary<string, string> queryStringResult =
                Regex.Matches(queryString, "([^?=&]+)(=([^&]*))?").Cast<Match>().ToDictionary(x => x.Groups[1].Value, x => x.Groups[3].Value);

            // Altera limite de registros para 50 (Máximo da API)
            queryStringResult.AddParam("max", AppDefaults.MaxRecordsPerPageAPI.ToString());

            // Altera página sempre para a primeira
            queryStringResult.AddParam("page", "1");

            return queryStringResult;
        }

        public abstract void Process(string queryStringFilter, string accessToken);
    }
}