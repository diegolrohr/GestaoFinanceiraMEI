using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fly01.Financeiro.Controllers.Base;
using Fly01.Financeiro.ViewModel;
using Fly01.Core;
using Fly01.Core.Helpers;

namespace Fly01.Financeiro.Controllers
{
    public class HistoricoCNABController : BaseController<CNABHistoryVM>
    {
        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            Dictionary<string, string> customFilters = base.GetQueryStringDefaultGridLoad();
            customFilters.AddParam("flow", "2");

            return customFilters;
        }

        public override Func<CNABHistoryVM, object> GetDisplayData()
        {
            return x => new
            {
                x.Id, x.PersonName,
                IssueDate = x.IssueDate.HasValue ? ((DateTime)x.IssueDate).ToString("dd/MM/yyyy") : string.Empty,
                DueDate = x.DueDate.HasValue ? ((DateTime)x.DueDate).ToString("dd/MM/yyyy") : string.Empty,
                Value = x.Value.ToString("C", AppDefaults.CultureInfoDefault), x.MovementId, x.MovementDescription, x.OcurrenceId, x.OcurrenceDescription
            };
        }

        public override ContentResult List()
        {
            return EmConstrucao(Url.Action("Index", "CNAB"));
        }

        public override ContentResult Form()
        {
            throw new NotImplementedException();
        }
    }
}