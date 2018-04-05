//using Fly01.Financeiro.API.Models.DAL;
//using Fly01.Core.BL;
//using Fly01.Core.ValueObjects;
//using System;
//using System.Linq;

//namespace Fly01.Financeiro.BL
//{
//    public class DemonstrativoResultadoExercicioBL : PlataformaBaseBL<DemonstrativoResultadoExercicio>
//    {
//        protected CondicaoParcelamentoBL CondicaoParcelamentoBL;
//        protected ContaPagarBL ContaPagarBL;
//        protected ContaReceberBL ContaReceberBL;
//        protected CategoriaBL CategoriaBL;

//        public DemonstrativoResultadoExercicioBL(AppDataContext context,
//                                                 ContaPagarBL contaPagarBL,
//                                                 ContaReceberBL contaReceberBL,
//                                                 CategoriaBL categoriaBL)
//            : base(context)
//        {
//            ContaPagarBL = contaPagarBL;
//            ContaReceberBL = contaReceberBL;
//            CategoriaBL = categoriaBL;
//        }

//        public DemonstrativoResultadoExercicio Get(DateTime dataInicial, DateTime dataFinal)
//        {
//            var contasPagar = ContaPagarBL.AllIncluding(c => c.Categoria)
//                .Where(x => ((x.DataVencimento >= dataInicial && x.DataVencimento <= dataFinal)))
//                .ToList();

//            var contasReceber = ContaReceberBL.AllIncluding(c => c.Categoria)
//                .Where(x => ((x.DataVencimento >= dataInicial && x.DataVencimento <= dataFinal)))
//                .ToList();

//            var categoria = CategoriaBL.All.OrderBy(x => x.Descricao).ToList();

//            try
//            {
//                var dre = new DemonstrativoResultadoExercicio(contasReceber, contasPagar, categoria)
//                {
//                    PlataformaId = PlataformaUrl,
//                    UsuarioInclusao = AppUser
//                };

//                return dre;
//            }
//            catch (Exception e)
//            {
//                throw new BusinessException(e.Message);
//            }
//        }
//    }
//}