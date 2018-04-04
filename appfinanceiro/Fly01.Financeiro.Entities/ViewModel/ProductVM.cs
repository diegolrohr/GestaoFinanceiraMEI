using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Helpers;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class ProductVM : DomainBaseVM
    {      
        //Informe a descrição do produto.
        [JsonProperty("description")]
        [Display(Name = "Descricao")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Description { get; set; }

        //Informe a descrição auxiliar do produto. (máximo de 400 caracteres)
        [JsonProperty("auxDescription")]
        [Display(Name = "Auxiliar")]
        [StringLength(400, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string AuxDescription { get; set; }

        //Neste local devera ser definido o tipo do produto: 1 - Material de Consumo; 2 - Materia Prima; 3 - Produto Acabado; 4 - Produto Intermediário; 5 - Outros; 6 - Servico Intermediario; 7 - Servico
        [JsonProperty("type")]
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Type { get; set; }

        //Informe o grupo de produto.
        [JsonProperty("groupId")]
        [Display(Name = "Grupo Produto")]
        [StringLength(4, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string GroupId { get; set; }

        //Informe se o Produto está Ativo: 1=Sim 2=Não
        [JsonProperty("active")]
        [Display(Name = "Ativo")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Active { get; set; }

        //Unidade de medida indicada em todos os movimentos, desde compras até vendas.
        [JsonProperty("measure")]
        [Display(Name = "Unidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Measure { get; set; }

        //Fator de Conversão da 1ª UM para a 2ª UM. Todas as Rotinas de Entrada, Saída e Movimentação interna possuem campos para a digitação nas 2 unidades de Medida. Se um Fator de Conversão for cadastrado, somente um deles precisa ser digitado, o sistema calcula a outra unidade de medida com base neste Fator de Conversão e preenche o outro campo automaticamente. Se nenhum Fator se Conversão for atribuído os 2 campos deverão ser preenchidos manualmente.
        [JsonProperty("measureFactor")]
        [Display(Name = "Fator Conv.")]
        public double? MeasureFactor { get; set; }

        //Multiplica ou Divide o Fator de Conversão para se calcular a 1ª Unidade de Medida.
        [JsonProperty("conversionType")]
        [Display(Name = "Tipo Conv.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string ConversionType { get; set; }

        //Local padrão para armazenamento do produto. Esta informação auxilia no controle da saída ou entrada de produtos.
        [JsonProperty("defaultWarehouse")]
        [Display(Name = "Armazem")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DefaultWarehouse { get; set; }

        //Esse campo possibilita ao usuário o controle de qualidade na entrada dos produtos. Valores permitidos: 1=Sugere, 2=Não Controla e 3=Obrigatório
        [JsonProperty("qualityControl")]
        [Display(Name = "Contr.Qualidad")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string QualityControl { get; set; }

        //Código NCM do padrão para controle de importação.
        [JsonProperty("ncm")]
        [Display(Name = "N.C.M.")]
        [StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Ncm { get; set; }

        //Exceção do NCM/TIPI
        [JsonProperty("ncmException")]
        [Display(Name = "Ex Do Ncm")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string NcmException { get; set; }

        //Lote econômico do produto. Quantidade padrão a ser comprada de uma só vez ou a ser produzida em uma só operação, de modo que se incorra no custo mínimo e obtenha-se utilidades máximas.
        [JsonProperty("economicLot")]
        [Display(Name = "Lote Econ.")]
        public double? EconomicLot { get; set; }

        //Quantidade padrão inferior ao lote econômicos a ser considerada para Produção, de modo que se incorra no custo mínimo e obtenha-se utilidades máximas.
        [JsonProperty("minimalLot")]
        [Display(Name = "Lote Minimo")]
        public double? MinimalLot { get; set; }

        //Alíquota de ICMS aplicada sobre o produto para vendas dentro do estado no caso de uma tributação específica. Caso não informado, será utilizado a aliquota de acordo com o Estado.
        [JsonProperty("icmsRate")]
        [Display(Name = "Aliq.Icms")]
        public double? IcmsRate { get; set; }

        //Percentual que define o lucro para o calculo de ICMS solidário de entrada.
        [JsonProperty("mvaShopping")]
        [Display(Name = "Solid.Entr.")]
        public double? MvaShopping { get; set; }

        //Margem de lucro para cálculo do ICMS Solidário ou Retido.
        [JsonProperty("mvaSelling")]
        [Display(Name = "Solid.Saida")]
        public double? MvaSelling { get; set; }

        //Percentual de IPI a ser aplicado sobre o produto, de acordo com a posição do IPI.
        [JsonProperty("ipiRate")]
        [Display(Name = "Aliq.Ipi")]
        public double? IpiRate { get; set; }

        //Informa ao sistema que esse produto se refere a serviço, utilizando a aliquota para cálculo de ISS.
        [JsonProperty("issRate")]
        [Display(Name = "Aliq.Iss")]
        public double? IssRate { get; set; }

        //O preenchimento deste campo é importante para o cálculo da Contribuição ao Programa de Integração Social (PIS). Para que isto ocorra, seu conteúdo deve estar definido como (Sim).
        [JsonProperty("hasPis")]
        [Display(Name = "Calcula Pis")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string HasPis { get; set; }

        //O preenchimento deste campo é importante para o cálculo da Contribuição para o Financiamento da Seguridade Social (Cofins). Para que isto ocorra, seu conteúdo deve estar definido como (Sim).
        [JsonProperty("hasCofins")]
        [Display(Name = "Calc.Cofins")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string HasCofins { get; set; }

        //O preenchimento deste campo é importante para o cálculo da Contribuição Social sobre Lucro Líquido (CSLL). Para que isto ocorra, seu conteúdo deve ser definido como (Sim).
        [JsonProperty("hasCsll")]
        [Display(Name = "Calcula Csll")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string HasCsll { get; set; }

        //Codigo de servico. Campo utilizado para NFe.
        [JsonProperty("issCode")]
        [Display(Name = "Cod.Serv.Iss")]
        [StringLength(9, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string IssCode { get; set; }

        //Indica se o produto utiliza controle de lote ou não. L = Controle de Lote, N = Não controla lote.
        [JsonProperty("trail")]
        [Display(Name = "Rastro")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Trail { get; set; }

        //Código da Tributação Municipal
        [JsonProperty("municipalTributationCode")]
        [Display(Name = "Trib Mun")]
        [StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string MunicipalTributationCode { get; set; }

        //Alíquota estadual
        [JsonProperty("estimatedStateRate")]
        [Display(Name = "Alíq. De Tributos Estaduais")]
        public double? EstimatedStateRate { get; set; }

        //Alíquota municipal
        [JsonProperty("estimatedMunicipalRate")]
        [Display(Name = "Alíq. De Tributos Municipais")]
        public double? EstimatedMunicipalRate { get; set; }

        //Alíquota nacional
        [JsonProperty("estimatedTaxesRate")]
        [Display(Name = "Alíq. De Tributos Federais")]
        public double? EstimatedTaxesRate { get; set; }

        //Informar o código de origem da mercadoria conforme a tabela A da Situação Tributária. Tabela A - Origem da Mercadoria ou Serviço: 0-Nacional; 1-Estrangeira-Import direta; 2-Estrangeira-Adq no mercado interno; 3-Nacional, mercadoria ou bem com Conteúdo de Importação superior a 40%; 4-Nacional, cuja produção tenha sido feita em conformidade com os processos produtivos básicos de que tratam o Decreto-Lei nº 288/67, e as Leis nºs 8.248/91, 8.387/91, 10.176/01 e 11.4; 5-Nacional, mercadoria ou bem com Conteúdo de Importação inferior ou igual a 40%; 6-Estrangeira-Importação direta, sem similar nacional, constante em lista de Resolução CAMEX; 7-Estrangeira - Adquirida no mercado int sem similar nacional, constante em lista Resolução CAMEX; 8-Nacional- Mercadoria ou bem com Conteúdo de Importação superior a 70%;
        [JsonProperty("origin")]
        [Display(Name = "Origem")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Origin { get; set; }

        //Código da FCI, retornado após validação do FCI, segundo especificações do SINIEF nº 19/2012.
        [JsonProperty("fciCode")]
        [Display(Name = "Código Fci")]
        [StringLength(36, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string FciCode { get; set; }

        //Valor do conteúdo de importação, referente ao valor das matérias primas  importadas ponderada pela quantidade usada na produção, calculado segundo especificações do SINIEF nº 19/2012
        [JsonProperty("fciImportValue")]
        [Display(Name = "Val. Cont. Importado (Produção)")]
        public double? FciImportValue { get; set; }

        //Valor de venda apurado na geração da última FCI.
        [JsonProperty("fciCalculatedSaleValue")]
        [Display(Name = "Valor De Venda Apurado")]
        public double? FciCalculatedSaleValue { get; set; }

        //Segunda Unidade de Medida utilizada para se referir ao Produto. Todas as rotinas de Entrada, Saída e Movimentação Interna permitem a entrada de dados nas 2 unidades de medida.
        [JsonProperty("measure2")]
        [Display(Name = "Seg.Un.Med.")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Measure2 { get; set; }

        //Informe o ultimo preço de compra de produto.
        [JsonProperty("lastPurchasePrice")]
        [Display(Name = "Ult.Preco")]
        public double? LastPurchasePrice { get; set; }

        //Data da ultima compra do produto.
        [Display(Name = "Ultima Compra")]
        [JsonIgnore]        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LastPurchaseDate { get; set; }

        //tratar formato de data diferente na API
        [JsonProperty("lastPurchaseDate")]
        public string LastPurchaseDateRest
        {
            get
            {
                return LastPurchaseDate.HasValue ? LastPurchaseDate.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                LastPurchaseDate = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }


        //Letra de referência do NCM.
        [JsonProperty("ncmLetter")]
        [Display(Name = "Letra Ncm")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string NcmLetter { get; set; }

        //Custo de reposição do produto informada ou calculada pelo sistema através da rotina 'Custo de Reposição' do módulo estoque.
        [JsonProperty("replacementCost")]
        [Display(Name = "Custo Rep.")]
        public double? ReplacementCost { get; set; }

        //Data de referência do custo de reposição. Esta data é gravada pela rotina 'Custo de Reposição' do módulo estoque com base no último preço de compra do produto.
        [Display(Name = "Dt.Ref.Custo")]
        [JsonIgnore]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateReplacementCost { get; set; }

        [JsonProperty("dateReplacementCost")]
        public string DateReplacementCostRest
        {
            get
            {
                return DateReplacementCost.HasValue ? DateReplacementCost.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                DateReplacementCost = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }


        //Moeda do custo de reposição.
        [JsonProperty("currencyReplacementCost")]
        [Display(Name = "Moeda Custo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CurrencyReplacementCost { get; set; }

        //Data do último calculo do custo de reposição gravada pela rotina 'Custo de Reposição'.
        [Display(Name = "Ult.Calc.Custo")]
        [JsonIgnore]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DtLastCalcReplacementCost { get; set; }
        
        [JsonProperty("dtLastCalcReplacementCost")]
        public string DtLastCalcReplacementCostRest
        {
            get
            {
                return DtLastCalcReplacementCost.HasValue ? DtLastCalcReplacementCost.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                DtLastCalcReplacementCost = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

        //Tipo de entrada padrão que será utilizado no processo de compra do produto.
        [JsonProperty("entryType")]
        [Display(Name = "Tipo Entrada")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string EntryType { get; set; }

        //Tipo de saída padrão que será utilizado no processo de venda do produto.
        [JsonProperty("outputType")]
        [Display(Name = "Tipo Saida")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string OutputType { get; set; }

        //Categoria financeira padrão que será utilizado no processo de compra do produto.
        [JsonProperty("defaultFinancialPurchaseCategory")]
        [Display(Name = "Cat.Fin.Ent.")]
        [StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DefaultFinancialPurchaseCategory { get; set; }

        //Categoria financeira padrão que será utilizado no processo de venda do produto.
        [JsonProperty("defaultFinancialSaleCategory")]
        [Display(Name = "Cat.Fin.Sai.")]
        [StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DefaultFinancialSaleCategory { get; set; }

        //Indica se o produto geralmente é comprado de outros estados. Esta informação será utilizada no cálculo do custo de reposição na rotina 'Custo de  Reposição' do módulo estoque.
        [JsonProperty("outOfState")]
        [Display(Name = "Fora Estado")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string OutOfState { get; set; }

        //Informe o código de barras.
        [JsonProperty("barCode")]
        [Display(Name = "Cod.Barras")]
        [StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string BarCode { get; set; }

        //Informe o segundo código de barras.
        [JsonProperty("barCode2")]
        [Display(Name = "Cod.Barras2")]
        [StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string BarCode2 { get; set; }

        //Informe o peso bruto.
        [JsonProperty("grossWeight")]
        [Display(Name = "Peso Bruto")]
        public double? GrossWeight { get; set; }

        //Informe o peso líquido.
        [JsonProperty("netWeight")]
        [Display(Name = "Peso Liquido")]
        public double? NetWeight { get; set; }

        //Informe o código da situação tributária.
        [JsonProperty("codeTaxSituation")]
        [Display(Name = "C.S.T.")]
        [StringLength(5, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CodeTaxSituation { get; set; }

        //Informe a classificação fiscal.
        [JsonProperty("taxClassification")]
        [Display(Name = "Clas.Fiscal")]
        [StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string TaxClassification { get; set; }

        //Indica se o produto pertence a Grade ou não.
        [JsonProperty("grid")]
        [Display(Name = "Grade")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Grid { get; set; }

        //Percentual de comissão sobre o produto.
        [JsonProperty("percentageComissionProduct")]
        [Display(Name = "%Comissao")]
        public double? PercentageComissionProduct { get; set; }

        //Armazena o código cnae
        [JsonProperty("cnaeCode")]
        [Display(Name = "Cnae")]
        [StringLength(9, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CnaeCode { get; set; }

        //Informe o código da Conta Analítica.
        [JsonProperty("analyticalAccountCode")]
        [Display(Name = "Conta Analitica")]
        [StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string AnalyticalAccountCode { get; set; }

        //Código de produto da ANP
        [JsonProperty("anpCode")]
        [Display(Name = "Código Anp")]
        [StringLength(9, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string AnpCode { get; set; }

        //
        [JsonProperty("recalcType")]
        [Display(Name = "Tp.Recalc.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string RecalcType { get; set; }

        //Codigo do produto referencial para importação do arquivo XML e emissão da DANFE.
        [JsonProperty("referenceProductCode")]
        [Display(Name = "Cod. Referencia")]
        [StringLength(60, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string ReferenceProductCode { get; set; }

        //Endereçamento de produto
        [JsonProperty("productAddress")]
        [Display(Name = "Endereçamento De Produto")]
        [StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string ProductAddress { get; set; }

        //Classe de enquadramento do selo de IPI
        [JsonProperty("ipiSealClass")]
        [Display(Name = "Classe Enq. Ipi")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string IpiSealClass { get; set; }

        //Código do selo de IPI
        [JsonProperty("ipiSealCode")]
        [Display(Name = "Código Do Selo De Controle")]
        [StringLength(60, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string IpiSealCode { get; set; }

        //Observações comerciais do produto
        [JsonProperty("commercialObservations")]
        [Display(Name = "Observações Comerciais")]
        public string CommercialObservations { get; set; }

        //Departamento
        [JsonProperty("mainDepartment")]
        [Display(Name = "Departamento")]
        [StringLength(9, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string MainDepartment { get; set; }

        //Ponto de Pedido é quantidade mínima pré-estabelecida em estoque para utilização na geração das Solicitações de Compra. Na criação das Ordens de Produção é utilizado na geração das Solicitações de Compras necessárias para atender a demanda da produção. Ponto de Pedido normalmente é menos que o Lote minímo ou Lote Econômico.
        [JsonProperty("applicationPoint")]
        [Display(Name = "Ponto Pedido")]
        public double? ApplicationPoint { get; set; }

        //Prazo de entrega do produto. É o numero de dias , meses ou anos que o fornecedor ou a fábrica necessita para entregar o produto, a partir do recebimento de seu pedido.
        [JsonProperty("deliveryTime")]
        [Display(Name = "Entrega")]
        public int? DeliveryTime { get; set; }

        //Tipo de prazo de entrega. Informa se o prazo será em horas(1), dias(2), semanas(3), meses(4), ou ano(5). Este campo deve estar de acordo com o campo Prazo de Entrega.
        [JsonProperty("deliveryTimeType")]
        [Display(Name = "Tipo Prazo")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DeliveryTimeType { get; set; }

        //Define o tratamento dos decimais para a explosão de estrutura da OP. 1=Normal, 2=Arredonda.
        [JsonProperty("decimalType")]
        [Display(Name = "Tipo Dec.Op")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DecimalType { get; set; }

        //Grupo de tributação para tratamento em exceção fiscal.
        [JsonProperty("tributationGroup")]
        [Display(Name = "Grupo Trib.")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string TributationGroup { get; set; }

        //Percentual a ser aplicado para cálculo  do PIS quando a alíquota for diferente daquele que estiver informada no parâmetro MV_TXPIS.
        [JsonProperty("percentagePis")]
        [Display(Name = "Perc. Pis")]
        public double? PercentagePis { get; set; }

        //Percentual a ser aplicado para cálculo  do COFINS, quando a alíquota for diferente da que estiver informada no parâmetro MV_TXCOFIN.
        [JsonProperty("percentageCofins")]
        [Display(Name = "Perc. Cofins")]
        public double? PercentageCofins { get; set; }

        //Percentual a ser aplicado para cálculo do INSS. Este percentual sobrescreve o parâmetro MV_ALIQINS.
        [JsonProperty("percentageInss")]
        [Display(Name = "Perc. Inss")]
        public double? PercentageInss { get; set; }

        //Percentual a ser aplicado para cálculo do IRRF. Este percentual sobrescreve o parâmetro MV_ALIQIRR.
        [JsonProperty("percentageIrff")]
        [Display(Name = "Perc. Irrf")]
        public double? PercentageIrff { get; set; }

        //Percentual a ser aplicado para cálculo do CSLL. Este percentual sobrescreve o parâmetro MV_ALIQCSL.
        [JsonProperty("percentageCsll")]
        [Display(Name = "Perc. Csll")]
        public double? PercentageCsll { get; set; }

        //Redução da base de cálculo do PIS. Informe o percentual de redução a ser aplicado à base de cálculo do imposto.
        [JsonProperty("redPis")]
        [Display(Name = "%Red. Pis")]
        public double? RedPis { get; set; }

        //Redução da base do COFINS. Informe o percentual a ser considerado em relação à base de cálculo.
        [JsonProperty("redConfis")]
        [Display(Name = "%Red. Cofins")]
        public double? RedConfis { get; set; }

        //Produto Importado: <S>im: Nas vendas para Zona Franca não será aplicado desconto.<N>ão: Nas vendas para Zona Franca será aplicado Desconto.
        [JsonProperty("importedProduct")]
        [Display(Name = "Imp. Z. Franca")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string ImportedProduct { get; set; }

        //Valor unitario do IPI de Pauta.
        [JsonProperty("valueIpiRuling")]
        [Display(Name = "Ipi De Pauta")]
        public double? ValueIpiRuling { get; set; }

        //Valor unitário do COFINS de pauta.
        [JsonProperty("valueCofinsRuling")]
        [Display(Name = "Cofins De Pauta")]
        public double? ValueCofinsRuling { get; set; }

        //Valor unitário do PIS de pauta.
        [JsonProperty("valuePisRuling")]
        [Display(Name = "Pis De Pauta")]
        public double? ValuePisRuling { get; set; }

        //Valor do conteúdo de importação para revenda
        [JsonProperty("importationContent")]
        [Display(Name = "Conteúdo De Importação De Mp")]
        public double? ImportationContent { get; set; }

        //Redução da base do INSS. Informe o percentual a ser considerado em relação à base de cálculo.
        [JsonProperty("redInss")]
        [Display(Name = "%Red. Inss")]
        public double? RedInss { get; set; }

        //Alíquota de redução para IR
        [JsonProperty("percRedIrrf")]
        [Display(Name = "Perc. Red. Irrf")]
        public double? PercRedIrrf { get; set; }

        //Alíquota de redução para CSLL
        [JsonProperty("percRedCsll")]
        [Display(Name = "Perc. Red. Csll")]
        public double? PercRedCsll { get; set; }

        //Informe o preço de venda do produto.
        [JsonProperty("salePrice1")]
        [Display(Name = "Padrao")]
        public double? SalePrice1 { get; set; }

        //Informe o preço de venda do produto na tabela 2.
        [JsonProperty("salePrice2")]
        [Display(Name = "Promocional")]
        public double? SalePrice2 { get; set; }

        //Informe o preço de venda do produto na tabela 3.
        [JsonProperty("salePrice3")]
        [Display(Name = "Preferencial")]
        public double? SalePrice3 { get; set; }

        //Informe o item da Tabela Variável
        [JsonProperty("table1")]
        [Display(Name = "Tabela Variável 1")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Table1 { get; set; }

        //Informe o item da Tabela Variável
        [JsonProperty("table2")]
        [Display(Name = "Tabela Variável 2")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Table2 { get; set; }

        //Informe o item da Tabela Variável
        [JsonProperty("table3")]
        [Display(Name = "Tabela Variável 3")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Table3 { get; set; }

        //Informe o item da Tabela Variável
        [JsonProperty("table4")]
        [Display(Name = "Tabela Variável 4")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Table4 { get; set; }

        //Código de Enquadramento Legal do IPI - Nota Técnica 2015/002
        [JsonProperty("frameIPIId")]
        [Display(Name = "Enquadramento Legal Do Ipi")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string FrameIPIId { get; set; }

        //Código Especificador da Substituição Tributária – CEST, que identifica a mercadoria sujeita aos regimes de substituição tributária e de antecipação do recolhimento do imposto.
        [JsonProperty("CEST")]
        [Display(Name = "C.E.S.T.")]
        [StringLength(9, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CEST { get; set; }

        //Informa se o registro está integrado à mobilidade.
        [JsonProperty("mobile")]
        [Display(Name = "Mobilidade")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Mobile { get; set; }

        //Informa se o registro está ativo no serviço de mobilidade
        [JsonProperty("mobileActive")]
        [Display(Name = "Ativo Em Mobilidade")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string MobileActive { get; set; }

        //Se o registro faz parte da integracao generica.
        [JsonProperty("pdv")]
        [Display(Name = "Integrar Com Pdv")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Pdv { get; set; }

        //Se o registro está ativo na integracao generica
        [JsonProperty("pdvActive")]
        [Display(Name = "Ativo Integracao Pdv")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string PdvActive { get; set; }

        //Indica se o registro será integrado ao CIASHOP
        [JsonProperty("virtualStore")]
        [Display(Name = "Integrar Com Loja Virtual")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string VirtualStore { get; set; }

        //Indica se o registro está ativo no CIASHOP
        [JsonProperty("virtualStoreActive")]
        [Display(Name = "Ativo Na Loja Virtual")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string VirtualStoreActive { get; set; }
    }
}
