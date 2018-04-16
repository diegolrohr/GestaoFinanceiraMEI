namespace Fly01.Financeiro.Models.ViewModel
{
    public class ReciboContaFinanceiraVM
    {
        public string Id { get; set; }
        public string Conteudo { get; set; }
        public string DescricaoTitulo { get; set; }
        public string ValorTitulo { get; set; }
        public string DataAtual { get; set; }
        public string Assinatura { get; set; }
        public string DescricaoJuros { get; set; }
        public string ValorJuros { get; set; }
        public string DescricaoDesconto { get; set; }
        public string ValorDesconto { get; set; }
        public string DescricaoTituloTotal { get; set; }
        public string ValorTituloTotal { get; set; }
        public string Observacao { get; set; }
        public string Numero { get; set; }
    }
}