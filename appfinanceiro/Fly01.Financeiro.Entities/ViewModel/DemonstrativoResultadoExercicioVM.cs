using System;
using Fly01.Core.Entities.ViewModels.Commons;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class DemonstrativoResultadoExercicioVM : DomainBaseVM
    {
        public double ReceitasPrevistas { get; set; }
        public double DespesasPrevistas { get; set; }
        public double TotalPrevisto { get; set; }
        public double? ReceitasRealizadas { get; set; }
        public double? DespesasRealizadas { get; set; }
        public double? TotalRealizado { get; set; }
        public double? ReceitasTotais { get; set; }
        public double? DespesasTotais { get; set; }
        public double? Total { get; set; }
        //public List<MovimentacaoPorCategoriaVM> MovimentacoesPorCategoria { get; set; }
    }

    //public class MovimentacaoPorCategoriaVM
    //{
    //    public Guid CategoriaId { get; set; }
    //    public string Categoria { get; set; }
    //    public Guid? CategoriaPaiId { get; set; }
    //    public string Codigo { get; set; }
    //    public double Previsto { get; set; }
    //    public double? Realizado { get; set; }
    //    public double Soma { get; set; }
    //    //public string Classe { get; set; }
    //    public string TipoCarteira { get; set; }
    //    public string TipoContaFinanceira { get; set; }
    //}
}