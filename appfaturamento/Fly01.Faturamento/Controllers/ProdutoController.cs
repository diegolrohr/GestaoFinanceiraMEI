using Fly01.uiJS.Classes.Helpers;
using System.Collections.Generic;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Presentation;

namespace Fly01.Faturamento.Controllers
{
    public class ProdutoController : ProdutoBaseController<ProdutoVM>
    {
        public ProdutoController() 
            : base(ResourceHash.FaturamentoCadastrosGrupoProdutos) { }

        public override List<TooltipUI> GetHelpers()
        {
            return new List<TooltipUI> {
                new TooltipUI
                {
                    Id = "enquadramentoLegalIPIId",
                    Tooltip = new HelperUITooltip()
                    {
                        Text = "Informe o enquadramento legal do IPI, se utilizar este produto com um grupo tributário que calcula IPI ao emitir notas fiscais."
                    }
                },
                new TooltipUI
                {
                    Id = "codigoBarras",
                    Tooltip = new HelperUITooltip()
                    {
                        Text = "Informe códigos GTIN (8, 12, 13, 14), de acordo com o NCM e CEST. Para produtos que não possuem código de barras, informe o literal “SEM GTIN”, se utilizar este produto para emitir notas fiscais."
                    }
                }
            };
        }
    }
}