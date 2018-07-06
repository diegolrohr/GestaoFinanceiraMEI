using System.Linq;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Entities.NFe.IBPT;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core;

namespace Fly01.EmissaoNFE.BL
{
    public class IbptNcmBL : DomainBaseBL<IbptNcm>
    {
        protected EstadoBL EstadoBL { get; set; }

        public IbptNcmBL(AppDataContextBase context, EstadoBL estadoBL) : base(context)
        {
            EstadoBL = estadoBL;
        }

        public void CalculaImpostoIBPT(TransmissaoVM entity)
        {
            foreach (var nota in entity.Item)
            {
                if ((nota.Identificador.FinalidadeEmissaoNFe == TipoFinalidadeEmissaoNFe.Normal)
                    && (nota.Identificador.TipoDocumentoFiscal == TipoNota.Saida) && (nota.Identificador.ConsumidorFinal == 1))
                {
                    var empresaUF = EstadoBL.All.Where(x => x.CodigoIbge == nota.Identificador.CodigoUF.ToString()).FirstOrDefault().Sigla;
                    double impostoFederal = 0;
                    double impostoEstadual = 0;
                    double impostoMunicipal = 0;

                    foreach (var detalhe in nota.Detalhes)
                    {

                        if (detalhe.Produto.NCM != null && detalhe.Produto.TipoProduto == TipoProduto.ProdutoFinal)
                        {
                            var codigoFilter = detalhe.Produto.NCM[0] == '0'
                                ? detalhe.Produto.NCM.Substring(1)
                                : detalhe.Produto.NCM;

                            var ibpt = All.FirstOrDefault(x => x.Codigo == codigoFilter && x.UF == empresaUF);
                            if (ibpt != null)
                            {

                                impostoFederal += (
                                    ((detalhe.Produto.Quantidade * detalhe.Produto.ValorUnitario)
                                      - (detalhe.Produto.ValorDesconto.HasValue ? detalhe.Produto.ValorDesconto.Value : 0))
                                    * (ibpt.ImpostoNacional / 100.00));

                                impostoEstadual += (
                                    ((detalhe.Produto.Quantidade * detalhe.Produto.ValorUnitario)
                                      - (detalhe.Produto.ValorDesconto.HasValue ? detalhe.Produto.ValorDesconto.Value : 0))
                                     * (ibpt.ImpostoEstadual / 100.00));

                                impostoMunicipal += (
                                    ((detalhe.Produto.Quantidade * detalhe.Produto.ValorUnitario)
                                      - (detalhe.Produto.ValorDesconto.HasValue ? detalhe.Produto.ValorDesconto.Value : 0))
                                     * (ibpt.ImpostoMunicipal / 100.00)); 
                            }
                        }
                    }
                    //percentual 

                    var percImpostoFederal = "("+ ((impostoFederal * 100) / nota.Total.ICMSTotal.ValorTotalNF) + "%)";
                    var percImpostoEstadual = "(" + ((impostoEstadual * 100) / nota.Total.ICMSTotal.ValorTotalNF) +"%)";
                    var percImpostoMunicipal = "(" + ((impostoMunicipal * 100) / nota.Total.ICMSTotal.ValorTotalNF) +"%)";
                    var InformacoesIBPT = " | " + "Valor aproximado do(s) Tributo(s): "
                        + impostoFederal.ToString("C", AppDefaults.CultureInfoDefault) + percImpostoFederal + " Federal, " 
                        + impostoEstadual.ToString("C", AppDefaults.CultureInfoDefault) + percImpostoEstadual + " Estadual, " 
                        + impostoMunicipal.ToString("C", AppDefaults.CultureInfoDefault) + percImpostoMunicipal
                        + " Municipal. Fonte: IBPT";

                    if (nota.InformacoesAdicionais != null)
                    {
                        nota.InformacoesAdicionais.InformacoesComplementares = nota.InformacoesAdicionais.InformacoesComplementares != null ? nota.InformacoesAdicionais.InformacoesComplementares += InformacoesIBPT : InformacoesIBPT;
                    }
                }

            }
        }

    }
}
