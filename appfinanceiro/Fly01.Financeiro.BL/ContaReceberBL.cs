using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.BL;
using Fly01.Core.Helpers;
using Fly01.Core.Notifications;
using System;
using System.Linq;
using Fly01.Core.Entities.Domains.Enum;
using System.Data.Entity;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.BL
{
    public class ContaReceberBL : PlataformaBaseBL<ContaReceber>
    {
        private CondicaoParcelamentoBL condicaoParcelamentoBL;
        private ContaFinanceiraBaixaBL contaFinanceiraBaixaBL;

        public ContaReceberBL(AppDataContext context, CondicaoParcelamentoBL condicaoParcelamentoBL, ContaFinanceiraBaixaBL contaFinanceiraBaixaBL)
            : base(context)
        {
            MustConsumeMessageServiceBus = true;

            this.condicaoParcelamentoBL = condicaoParcelamentoBL;
            this.contaFinanceiraBaixaBL = contaFinanceiraBaixaBL;
        }

        public virtual IQueryable<ContaReceber> Everything => repository.All.Where(x => x.PlataformaId == PlataformaUrl);

        public override void ValidaModel(ContaReceber entity)
        {
            entity.Fail(entity.Numero < 1, new Error("Número da conta inválido", "numero"));
            entity.Fail(Everything.Any(x => x.Numero == entity.Numero && x.Id != entity.Id), new Error("Número da conta duplicado", "numero"));

            base.ValidaModel(entity);
        }

        public override void Insert(ContaReceber entity)
        {
            const int limiteSemanal = 208;
            const int limiteMensal = 48;
            const int limiteAnual = 4;

            entity.PlataformaId = PlataformaUrl;
            entity.UsuarioInclusao = AppUser;

            entity.Fail(entity.Repetir && entity.TipoPeriodicidade == TipoPeriodicidade.Nenhuma, TipoPeriodicidadeInvalida);
            entity.Fail(entity.Repetir && !entity.NumeroRepeticoes.HasValue, NumeroRepeticoesInvalido);

            entity.Fail((entity.Repetir && entity.TipoPeriodicidade != TipoPeriodicidade.Nenhuma && entity.NumeroRepeticoes.HasValue) &&
                ((entity.TipoPeriodicidade == TipoPeriodicidade.Semanal && !(entity.NumeroRepeticoes.Value > 0 && entity.NumeroRepeticoes.Value <= limiteSemanal)) ||
                (entity.TipoPeriodicidade == TipoPeriodicidade.Mensal && !(entity.NumeroRepeticoes.Value > 0 && entity.NumeroRepeticoes.Value <= limiteMensal)) ||
                (entity.TipoPeriodicidade == TipoPeriodicidade.Anual && !(entity.NumeroRepeticoes.Value > 0 && entity.NumeroRepeticoes.Value <= limiteAnual)))
            , RepeticoesInvalidas);

            //na nova Transação já é informado o id e o status
            if (entity.Id == default(Guid))
                entity.StatusContaBancaria = StatusContaBancaria.EmAberto;

            var max = Everything.Any(x => x.Id != entity.Id) ? Everything.Max(x => x.Numero) : 0;

            max = (max == 1 && !Everything.Any(x => x.Id != entity.Id && x.Ativo && x.Numero == 1)) ? 0 : max;

            var condicoesParcelamento = condicaoParcelamentoBL.GetPrestacoes(entity.CondicaoParcelamentoId, entity.DataVencimento, entity.ValorPrevisto);
            Guid contaFinanceiraPrincipal = entity.Id == default(Guid) ? Guid.NewGuid() : entity.Id;
            for (int iParcela = 0; iParcela < condicoesParcelamento.Count(); iParcela++)
            {
                var parcela = condicoesParcelamento[iParcela];

                var itemContaReceber = new ContaReceber();
                entity.CopyProperties<ContaReceber>(itemContaReceber);

                // CopyProperties não copia as notificações
                itemContaReceber.Notification.Errors.AddRange(entity.Notification.Errors);

                itemContaReceber.DataVencimento = parcela.DataVencimento;
                itemContaReceber.DescricaoParcela = parcela.DescricaoParcela;
                itemContaReceber.ValorPrevisto = parcela.Valor;

                itemContaReceber.Id = iParcela == default(int) ? contaFinanceiraPrincipal : default(Guid);

                itemContaReceber.Numero = ++max;

                base.Insert(itemContaReceber);

                if (entity.Repetir && entity.TipoPeriodicidade != TipoPeriodicidade.Nenhuma)
                {
                    for (int iRepeticao = 1; iRepeticao <= entity.NumeroRepeticoes; iRepeticao++)
                    {
                        var itemContaReceberRepeticao = new ContaReceber();
                        itemContaReceber.CopyProperties<ContaReceber>(itemContaReceberRepeticao);

                        // CopyProperties não copia as notificações
                        itemContaReceberRepeticao.Notification.Errors.AddRange(itemContaReceber.Notification.Errors);

                        itemContaReceberRepeticao.Id = default(Guid);
                        itemContaReceberRepeticao.ContaFinanceiraRepeticaoPaiId = contaFinanceiraPrincipal;

                        switch (entity.TipoPeriodicidade)
                        {
                            case TipoPeriodicidade.Semanal:
                                itemContaReceberRepeticao.DataVencimento = itemContaReceberRepeticao.DataVencimento.AddDays(iRepeticao * 7);
                                break;
                            case TipoPeriodicidade.Mensal:
                                itemContaReceberRepeticao.DataVencimento = itemContaReceberRepeticao.DataVencimento.AddMonths(iRepeticao);
                                break;
                            case TipoPeriodicidade.Anual:
                                itemContaReceberRepeticao.DataVencimento = itemContaReceberRepeticao.DataVencimento.AddYears(iRepeticao);
                                break;
                        }

                        itemContaReceberRepeticao.Numero = ++max;

                        base.Insert(itemContaReceberRepeticao);
                    }
                }
            }
        }

        public override void Update(ContaReceber entity)
        {
            var contaReceberDb = All.AsNoTracking().FirstOrDefault(x => x.Id == entity.Id);

            entity.Fail(contaReceberDb.CondicaoParcelamentoId != entity.CondicaoParcelamentoId, AlteracaoCondicaoParcelamento);
            entity.Fail((contaReceberDb.Repetir != entity.Repetir) || (contaReceberDb.TipoPeriodicidade != entity.TipoPeriodicidade) ||
                (contaReceberDb.NumeroRepeticoes != entity.NumeroRepeticoes), AlteracaoConfiguracaoRecorrencia);

            entity.Numero = contaReceberDb.Numero;

            base.Update(entity);
        }

        public override void Delete(ContaReceber entityToDelete)
        {
            contaFinanceiraBaixaBL.All.Where(x => x.ContaFinanceiraId == entityToDelete.Id).ToList()
                .ForEach(itemBaixa => { contaFinanceiraBaixaBL.Delete(itemBaixa); });

            base.Delete(entityToDelete);
        }

        public static Error RepeticoesInvalidas = new Error("Número de repetições inválido. Somente até 48 Meses (4 Anos / 208 Semanas).");
        public static Error AlteracaoCondicaoParcelamento = new Error("Não é permitido alterar a condição de parcelamento.");
        public static Error AlteracaoConfiguracaoRecorrencia = new Error("Não é permitido alterar as configurações de recorrência.");

        public static Error TipoPeriodicidadeInvalida = new Error("Periodicidade inválida", "tipoPeriodicidade");
        public static Error NumeroRepeticoesInvalido = new Error("Número de repetições inválido", "numeroRepeticoes");
    }
}
