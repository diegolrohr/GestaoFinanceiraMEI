using System;

namespace Fly01.OrdemServico.ViewModel
{
    public class ImprimirOrdemServicoVM
    {
        public string Id { get; internal set; }
        public string ClienteNome { get; internal set; }
        public object DataEmissao { get; internal set; }
        public string DataEntrega { get; internal set; }
        public string Status { get; internal set; }
        public string Numero { get; internal set; }
        public string Observacao { get; internal set; }
        public Guid ItemId { get; internal set; }
        public string ItemNome { get; internal set; }
        public double ItemQtd { get; internal set; }
        public double ItemValor { get; internal set; }
        public double ItemDesconto { get; internal set; }
        public double ItemTotal { get; internal set; }
        public double Total { get; internal set; }
    }
}