using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels.Presentation.Commons;
using System;

namespace Fly01.Financeiro.ViewModel
{
    public class CnabVM : DomainBaseVM
    {
        public int NumeroBoleto { get; set; }
        public double ValorBoleto { get; set; }
        public PessoaVM Pessoa { get; set; }
        public string Status { get; set; }
        public BancoVM Banco { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
    }
}