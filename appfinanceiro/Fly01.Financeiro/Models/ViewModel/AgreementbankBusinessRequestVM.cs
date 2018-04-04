using System;
using Fly01.Financeiro.Entities.ViewModel;
using Fly01.Financeiro.Models.Session;

namespace Fly01.Financeiro.Models.ViewModel
{
    [Serializable]
    public class AgreementbankBusinessRequestVM : AgreementbankBusinessVM
    {
        public int ItemIdSession { get; set; }
        public EnumStatusRecord StatusRecord { get; set; }
    }
}