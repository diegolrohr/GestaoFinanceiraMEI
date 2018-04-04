using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fly01.Financeiro.Models.ViewModel;

namespace Fly01.Financeiro.Models.Session
{
    [Serializable]
    public class SessionContaFinanceiraConvenio
    {
        private const string SessionKey = "_SESSION_CONTA_FINANCEIRA_CONVENIO_FLY01_FINANCEIRO";

        public static SessionContaFinanceiraConvenio Current
        {
            get
            {
                if (HttpContext.Current.Session[SessionKey] == null)
                {
                    HttpContext.Current.Session[SessionKey] = new SessionContaFinanceiraConvenio();
                }

                return (SessionContaFinanceiraConvenio)HttpContext.Current.Session[SessionKey];
            }
        }

        private List<AgreementbankBusinessRequestVM> _dataItemsList;
        public List<AgreementbankBusinessRequestVM> DataItems
        {
            get { return _dataItemsList ?? (_dataItemsList = new List<AgreementbankBusinessRequestVM>()); }
            set
            {
                _dataItemsList = value;
                if (value == null)
                    HttpContext.Current.Session[SessionKey] = null;
            }
        }

        public AgreementbankBusinessRequestVM GetItem(int item)
        {
            return DataItems.FirstOrDefault(x => x.ItemIdSession == item);
        }

        public int GetIndexItem(int item)
        {
            return DataItems.IndexOf(DataItems.FirstOrDefault(x => x.ItemIdSession == item));
        }

        public void InsertOrUpdateItem(AgreementbankBusinessRequestVM newItem)
        {
            AgreementbankBusinessRequestVM findItem = GetItem(newItem.ItemIdSession);
            if (findItem == null)
            {
                int maxItemIdSession = DataItems.Count == 0 ? 1 : DataItems.Max(x => x.ItemIdSession) + 1;
                newItem.StatusRecord = EnumStatusRecord.srInsert;
                newItem.Id = default(Guid);
                newItem.ItemIdSession = maxItemIdSession;
                DataItems.Add(newItem);
            }
            else
            {
                int indexOldElement = GetIndexItem(newItem.ItemIdSession);
                if (indexOldElement != -1)
                {
                    //newItem.StatusRecord = newItem.Id.Length > 0 ? EnumStatusRecord.srUpdate : EnumStatusRecord.srInsert;
                    newItem.StatusRecord = !(newItem.Id == default(Guid))  ? EnumStatusRecord.srUpdate : EnumStatusRecord.srInsert;
                    DataItems[indexOldElement] = newItem;
                }
            }
        }

        public void DeleteItem(int item)
        {
            int indexElement = GetIndexItem(item);
            if (indexElement != -1)
            {
                DataItems[indexElement].StatusRecord = EnumStatusRecord.srDelete;
            }
        }
    }
}