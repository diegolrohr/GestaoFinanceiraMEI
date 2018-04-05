﻿using Fly01.Financeiro.API.Models.DAL;
using Fly01.Financeiro.Domain.Entities;
using Fly01.Financeiro.Domain.Enums;
using Fly01.Core.BL;
using Fly01.Core.Helpers;
using Fly01.Core.Notifications;
using System;
using System.Linq;

namespace Fly01.Financeiro.BL
{
    public class ContaReceberBL : PlataformaBaseBL<ContaReceber>
    {
        private CondicaoParcelamentoBL condicaoParcelamentoBL;

        public ContaReceberBL(AppDataContext context, CondicaoParcelamentoBL condicaoParcelamentoBL) : base(context)
        {
            this.condicaoParcelamentoBL = condicaoParcelamentoBL;
            MustConsumeMessageServiceBus = true;
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
                        base.Insert(itemContaReceberRepeticao);
                    }
                }
            }
        }

        public override void Update(ContaReceber entity)
        {
            var ContaReceberDb = All.FirstOrDefault(x => x.Id == entity.Id);

            entity.Fail(ContaReceberDb.CondicaoParcelamentoId != entity.CondicaoParcelamentoId, AlteracaoCondicaoParcelamento);
            entity.Fail((ContaReceberDb.Repetir != entity.Repetir) || (ContaReceberDb.TipoPeriodicidade != entity.TipoPeriodicidade) ||
                (ContaReceberDb.NumeroRepeticoes != entity.NumeroRepeticoes), AlteracaoConfiguracaoRecorrencia);

            base.Update(entity);
        }

        public static Error RepeticoesInvalidas = new Error("Número de repetições inválido. Somente até 48 Meses (4 Anos / 208 Semanas).");
        public static Error AlteracaoCondicaoParcelamento = new Error("Não é permitido alterar a condição de parcelamento.");
        public static Error AlteracaoConfiguracaoRecorrencia = new Error("Não é permitido alterar as configurações de recorrência.");

        public static Error TipoPeriodicidadeInvalida = new Error("Periodicidade inválida", "tipoPeriodicidade");
        public static Error NumeroRepeticoesInvalido = new Error("Número de repetições inválido", "numeroRepeticoes");
    }
}
