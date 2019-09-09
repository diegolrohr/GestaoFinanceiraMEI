using System;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Financeiro.ViewModel
{
    [Serializable]
    public class DemonstrativoResultadoExercicioVM : EmpresaBaseVM
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
    }
}