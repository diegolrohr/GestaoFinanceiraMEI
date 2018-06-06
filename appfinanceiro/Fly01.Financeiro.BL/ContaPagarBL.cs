using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Notifications;
using Fly01.Financeiro.API.Models.DAL;
using System;
using System.Data.Entity;
using System.Linq;

namespace Fly01.Financeiro.BL
{
    public class ContaPagarBL : PlataformaBaseBL<ContaPagar>
    {
        private CondicaoParcelamentoBL condicaoParcelamentoBL;
        private ContaFinanceiraBaixaBL contaFinanceiraBaixaBL;
        private PessoaBL pessoaBL;

        public ContaPagarBL(AppDataContext context, CondicaoParcelamentoBL condicaoParcelamentoBL, ContaFinanceiraBaixaBL contaFinanceiraBaixaBL, PessoaBL pessoaBL) : base(context)
        {
            this.condicaoParcelamentoBL = condicaoParcelamentoBL;
            this.contaFinanceiraBaixaBL = contaFinanceiraBaixaBL;
            this.pessoaBL = pessoaBL;
            MustConsumeMessageServiceBus = true;
        }

        public virtual IQueryable<ContaPagar> Everything => repository.All.Where(x => x.PlataformaId == PlataformaUrl);

        public override void ValidaModel(ContaPagar entity)
        {
            entity.Fail(entity.Numero < 1, NumeroContaInvalido);
            entity.Fail(Everything.Any(x => x.Numero == entity.Numero && x.Id != entity.Id), NumeroContaDuplicada);

            //Validacao Plataforma
            entity.Fail(entity.CategoriaId == default(Guid), CategoriaNaoInformada);
            entity.Fail(entity.CondicaoParcelamentoId == default(Guid), CondicaoParcelamentoNaoInformada);
            entity.Fail(entity.PessoaId == default(Guid) && string.IsNullOrEmpty(entity.NomePessoa), PessoaNaoInformada);
            entity.Fail(entity.DataEmissao == default(DateTime) || entity.DataEmissao.GetType() != typeof(DateTime), DataEmissaoInvalida);
            entity.Fail(entity.DataVencimento == default(DateTime) || entity.DataVencimento.GetType() != typeof(DateTime), DataVencimentoInvalida);
            entity.Fail(string.IsNullOrEmpty(entity.Descricao), DescricaoInvalida);
            entity.Fail(entity.FormaPagamentoId == default(Guid), FormaPagamentoNaoInformada);
            entity.Fail(entity.TipoContaFinanceira != TipoContaFinanceira.ContaPagar, TipoContaFinanceiraInvalida);

            base.ValidaModel(entity);
        }

        public override void Insert(ContaPagar entity)
        {
            entity.PlataformaId = PlataformaUrl;
            entity.UsuarioInclusao = AppUser;

            var repetir = RepeticaoValida(entity);

            //ContaFinanceira.Número
            var max = Everything.Any(x => x.Id != entity.Id) ? Everything.Max(x => x.Numero) : 0;
            max = (max == 1 && !Everything.Any(x => x.Id != entity.Id && x.Ativo && x.Numero == 1)) ? 0 : max;

            //na nova Transação e quando status nao definido
            if (entity.StatusContaBancaria == default(StatusContaBancaria))
                entity.StatusContaBancaria = StatusContaBancaria.EmAberto;

            //Se Cliente não informado, busca pelo nome ou Insere
            if (entity.PessoaId == default(Guid) && !string.IsNullOrEmpty(entity.NomePessoa))
                entity.PessoaId = pessoaBL.BuscaPessoaNome(entity.NomePessoa, false, true);

            //post bemacash ignorando condicao parcelamento
            if (entity.DescricaoParcela != null)
            {
                entity.Id = Guid.NewGuid();
                entity.Numero = ++max;

                base.Insert(entity);
            }

            if (string.IsNullOrEmpty(entity.DescricaoParcela))
            {
                var condicoesParcelamento = condicaoParcelamentoBL
                                                .GetPrestacoes(entity.CondicaoParcelamentoId,
                                                               entity.DataVencimento,
                                                               entity.ValorPrevisto);
                var contaFinanceiraPrincipal = entity.Id == default(Guid) ? Guid.NewGuid() : entity.Id;
                for (int iParcela = 0; iParcela < condicoesParcelamento.Count; iParcela++)
                {
                    var parcela = condicoesParcelamento[iParcela];
                    var itemContaPagar = new ContaPagar();
                    entity.CopyProperties<ContaPagar>(itemContaPagar);
                    itemContaPagar.Notification.Errors.AddRange(entity.Notification.Errors); // CopyProperties não copia as notificações
                    itemContaPagar.DataVencimento = parcela.DataVencimento;
                    itemContaPagar.DescricaoParcela = parcela.DescricaoParcela;
                    itemContaPagar.ValorPrevisto = parcela.Valor;
                    itemContaPagar.ValorPago = entity.StatusContaBancaria == StatusContaBancaria.Pago
                                                    ? parcela.Valor
                                                    : entity.ValorPago;

                    if (iParcela == default(int))
                    {
                        itemContaPagar.Id = contaFinanceiraPrincipal;
                    }
                    else
                    {
                        itemContaPagar.Id = Guid.NewGuid();
                        if (repetir)
                            itemContaPagar.ContaFinanceiraRepeticaoPaiId = contaFinanceiraPrincipal;
                    }

                    itemContaPagar.Numero = ++max;

                    base.Insert(itemContaPagar);

                    //Se status "pago", gerar ContaFinanceiraBaixa
                    if (entity.StatusContaBancaria == StatusContaBancaria.Pago)
                        contaFinanceiraBaixaBL.GeraContaFinanceiraBaixa(itemContaPagar.DataVencimento,
                                                                        itemContaPagar.Id,
                                                                        itemContaPagar.ValorPrevisto,
                                                                        TipoContaFinanceira.ContaPagar,
                                                                        entity.Descricao);

                    if (repetir)
                    {
                        for (int iRepeticao = 1; iRepeticao <= entity.NumeroRepeticoes; iRepeticao++)
                        {
                            var itemContaPagarRepeticao = new ContaPagar();
                            itemContaPagar.CopyProperties<ContaPagar>(itemContaPagarRepeticao);
                            itemContaPagarRepeticao.Notification.Errors.AddRange(itemContaPagar.Notification.Errors); // CopyProperties não copia as notificações
                            itemContaPagarRepeticao.Id = default(Guid);
                            itemContaPagarRepeticao.ContaFinanceiraRepeticaoPaiId = contaFinanceiraPrincipal;

                            switch (entity.TipoPeriodicidade)
                            {
                                case TipoPeriodicidade.Semanal:
                                    itemContaPagarRepeticao.DataVencimento = itemContaPagarRepeticao.DataVencimento.AddDays(iRepeticao * 7);
                                    break;
                                case TipoPeriodicidade.Mensal:
                                    itemContaPagarRepeticao.DataVencimento = itemContaPagarRepeticao.DataVencimento.AddMonths(iRepeticao);
                                    break;
                                case TipoPeriodicidade.Anual:
                                    itemContaPagarRepeticao.DataVencimento = itemContaPagarRepeticao.DataVencimento.AddYears(iRepeticao);
                                    break;
                            }

                            itemContaPagarRepeticao.Numero = ++max;
                            base.Insert(itemContaPagarRepeticao);
                        }
                    }
                }
            }
        }

        public override void Update(ContaPagar entity)
        {
            var contaPagarDb = All.AsNoTracking().FirstOrDefault(x => x.Id == entity.Id);

            entity.Fail(contaPagarDb.CondicaoParcelamentoId != entity.CondicaoParcelamentoId, AlteracaoCondicaoParcelamento);
            entity.Fail((contaPagarDb.Repetir != entity.Repetir) || (contaPagarDb.TipoPeriodicidade != entity.TipoPeriodicidade) ||
                (contaPagarDb.NumeroRepeticoes != entity.NumeroRepeticoes), AlteracaoConfiguracaoRecorrencia);

            entity.Numero = contaPagarDb.Numero;

            base.Update(entity);
        }

        public override void Delete(ContaPagar entityToDelete)
        {
            contaFinanceiraBaixaBL.All.Where(x => x.ContaFinanceiraId == entityToDelete.Id).OrderBy(x => x.DataInclusao).ToList()
                .ForEach(itemBaixa => { contaFinanceiraBaixaBL.Delete(itemBaixa); });

            base.Delete(entityToDelete);
        }

        private static bool RepeticaoValida(ContaFinanceira entity)
        {
            if (entity.Repetir)
            {
                const int limiteSemanal = 208;
                const int limiteMensal = 48;
                const int limiteAnual = 4;

                entity.Fail(entity.TipoPeriodicidade == TipoPeriodicidade.Nenhuma, TipoPeriodicidadeInvalida);
                entity.Fail(!entity.NumeroRepeticoes.HasValue, NumeroRepeticoesInvalido);

                entity.Fail((entity.TipoPeriodicidade != TipoPeriodicidade.Nenhuma && entity.NumeroRepeticoes.HasValue) &&
                            ((entity.TipoPeriodicidade == TipoPeriodicidade.Semanal && !(entity.NumeroRepeticoes.Value > 0 && entity.NumeroRepeticoes.Value <= limiteSemanal)) ||
                             (entity.TipoPeriodicidade == TipoPeriodicidade.Mensal && !(entity.NumeroRepeticoes.Value > 0 && entity.NumeroRepeticoes.Value <= limiteMensal)) ||
                             (entity.TipoPeriodicidade == TipoPeriodicidade.Anual && !(entity.NumeroRepeticoes.Value > 0 && entity.NumeroRepeticoes.Value <= limiteAnual)))
                    , RepeticoesInvalidas);

                return entity.Repetir && entity.IsValid();
            }

            return false;
        }

        public static Error RepeticoesInvalidas = new Error("Número de repetições inválido. Somente até 48 Meses (4 Anos / 208 Semanas).");
        public static Error AlteracaoCondicaoParcelamento = new Error("Não é permitido alterar a condição de parcelamento.");
        public static Error AlteracaoConfiguracaoRecorrencia = new Error("Não é permitido alterar as configurações de recorrência.");
        public static Error TipoPeriodicidadeInvalida = new Error("Periodicidade inválida", "tipoPeriodicidade");
        public static Error NumeroRepeticoesInvalido = new Error("Número de repetições inválido", "numeroRepeticoes");
        public static Error NumeroContaInvalido = new Error("Número da conta inválido", "numero");
        public static Error NumeroContaDuplicada = new Error("Número da conta duplicado", "numero");

        public static Error CategoriaNaoInformada = new Error("Categoria Financeira não Informada", "categoriaId");
        public static Error CondicaoParcelamentoNaoInformada = new Error("Condição Parcelamento não Informada", "condicaoParcelamentoId");
        public static Error PessoaNaoInformada = new Error("Pessoa não Informada", "pessoaId");
        public static Error DataEmissaoInvalida = new Error("Data de Emissão não informada ou inválida", "dataEmissao");
        public static Error DataVencimentoInvalida = new Error("Data de Vencimento não informada ou inválida", "dataVencimento");
        public static Error DescricaoInvalida = new Error("Descrição não informada", "descricao");
        public static Error FormaPagamentoNaoInformada = new Error("Forma de Pagamento não Informada", "formaPagamentoId");
        public static Error TipoContaFinanceiraInvalida = new Error("Tipo Conta Financeira deve ser Conta a Pagar", "tipoContaFinanceira");
    }
}