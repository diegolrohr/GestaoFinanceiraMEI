using System;

namespace Fly01.OrdemServico.ViewModel
{
    public class ImprimirOrdemServicoVM
    {
        public string Id { get; internal set; }
        public string ClienteNome { get; internal set; }
        public DateTime? DataEmissao { get; internal set; }
        public DateTime? DataEntrega { get; internal set; }
        public TimeSpan? HoraEntrega { get; internal set; }
        public string Status { get; internal set; }
        public string Numero { get; internal set; }
        public string Descricao { get; internal set; }
        public string ItemId { get; internal set; }
        public string ItemNome { get; internal set; }
        public double ItemQtd { get; internal set; }
        public double ItemValor { get; internal set; }
        public double ItemDesconto { get; internal set; }
        public double ItemTotal { get; internal set; }
        public double Total { get; internal set; }
        public string ItemObservacao { get; internal set; }
        public string ItemTipo { get; internal set; }
        public string ClienteCPF { get; internal set; }
        public string ClienteEndereco { get; internal set; }
        public string ClienteEmail { get; internal set; }
        public string ClienteTelefone { get; internal set; }
        public string ClienteCelular { get; internal set; }
        public string ObjManutId { get; internal set; }
        public string ObjManutNome { get; internal set; }
        public string ObjManutQtd { get; internal set; }
        public double ObjManutValor { get; internal set; }
        public double ObjManutDesconto { get; internal set; }
        public double ObjManutTotal { get; internal set; }
        public string ObjManutTipo { get; internal set; }
        public string Pais { get; internal set; }
        public string ClienteNumero { get; internal set; }
        public string Bairro { get; internal set; }
        public string Cidade { get; internal set; }
        public string CEP { get; internal set; }
        public string Estado { get; internal set; }
        public string Complemento { get; internal set; }
        public TimeSpan? Duracao { get; internal set; }
    }
}