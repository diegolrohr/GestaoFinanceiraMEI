using Fly01.Core;
using Fly01.Core.BL;
using Fly01.Core.Defaults;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.Core.Rest;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Fly01.Compras.BL
{
    public class NotaFiscalCartaCorrecaoBL : PlataformaBaseBL<NotaFiscalCartaCorrecao>
    {
        protected NotaFiscalBL NotaFiscalBL { get; set; }
        protected TotalTributacaoBL TotalTributacaoBL { get; set; }
        protected CertificadoDigitalBL CertificadoDigitalBL { get; set; }

        public NotaFiscalCartaCorrecaoBL(AppDataContextBase context, NotaFiscalBL notaFiscalBL, TotalTributacaoBL totalTributacaoBL, CertificadoDigitalBL certificadoDigitalBL) : base(context)
        {
            NotaFiscalBL = notaFiscalBL;
            TotalTributacaoBL = totalTributacaoBL;
            CertificadoDigitalBL = certificadoDigitalBL;
        }

        public IQueryable<NotaFiscalCartaCorrecao> Everything => repository.AllIncluding(y => y.NotaFiscal).Where(x => x.Ativo);

        public override void ValidaModel(NotaFiscalCartaCorrecao entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.MensagemCorrecao), new Error("Informe a mensagem de correção", "mensagemCorrecao"));
            entity.Fail(All.AsNoTracking().Where(x => x.NotaFiscalId == entity.NotaFiscalId && x.Id != entity.Id && x.Status == StatusCartaCorrecao.Transmitida).Any(), new Error("Já existe um carta de correção em transmissão, aguarde o retorno SEFAZ para emitir um novo evento. Atualize o status.","status"));        

            entity.Fail(!string.IsNullOrEmpty(entity.MensagemCorrecao) && entity.MensagemCorrecao.Length > 1000,
                new Error("O SEFAZ permite até 1000 caracteres somando todas as cartas de correção. A soma possui: " + entity.MensagemCorrecao.Length.ToString() + " caracteres."));

            base.ValidaModel(entity);
        }

        public override void Insert(NotaFiscalCartaCorrecao entity)
        {
            entity.Data = DateTime.Now;
            var max = 0;
            var cceValidasAnterioes = All.AsNoTracking().Where(x => (x.NotaFiscalId == entity.NotaFiscalId) && (x.Id != entity.Id) && (x.Status == StatusCartaCorrecao.RegistradoEVinculado || x.Status == StatusCartaCorrecao.RegistradoENaoVinculado));

            if (cceValidasAnterioes != null && cceValidasAnterioes.Any())
            {
                max = cceValidasAnterioes.Max(x => x.Numero);

                var ultimaMensagem = cceValidasAnterioes.Where(x => x.Numero == max).FirstOrDefault().MensagemCorrecao;

                if (!string.IsNullOrEmpty(entity.MensagemCorrecao))
                    entity.MensagemCorrecao = entity.MensagemCorrecao + " " + ultimaMensagem;
                else
                    entity.MensagemCorrecao = ultimaMensagem;
            }

            entity.Fail(cceValidasAnterioes != null && cceValidasAnterioes.Count() >= 20, new Error("O SEFAZ permite no máximo 20 cartas de correções válidas e registradas por nota fiscal."));

            ValidaModel(entity);

            if (entity.IsValid())
            {
                TransmitirCartaCorrecao(entity);
            }
            base.Insert(entity);
        }

        public override void Update(NotaFiscalCartaCorrecao entity)
        {
            //ver questão sobre falha na transmissão e casos de rejeição, em principio criar nova cc-e
            entity.Fail(entity.Status == StatusCartaCorrecao.Transmitida, new Error("Não é possível retransmitir uma carta de correção."));
        }

        public override void Delete(NotaFiscalCartaCorrecao entityToDelete)
        {
            var status = entityToDelete.Status;
            entityToDelete.Fail(status == StatusCartaCorrecao.Transmitida || status == StatusCartaCorrecao.RegistradoEVinculado || status == StatusCartaCorrecao.RegistradoENaoVinculado, new Error("Não é possível deletar Carta de Correção com status Transmitida ou Registrada e Vinculada/Não Vinculada.", "status"));
            if (entityToDelete.IsValid())
            {
                base.Delete(entityToDelete);
            }
            else
            {
                throw new BusinessException(entityToDelete.Notification.Get());
            }
        }

        public void TransmitirCartaCorrecao(NotaFiscalCartaCorrecao entity)
        {
            try
            {
                if (!TotalTributacaoBL.ConfiguracaoTSSOK())
                {
                    throw new BusinessException("Configuração inválida para comunicação com TSS");
                }
                else
                {
                    var parametros = TotalTributacaoBL.GetParametrosTributarios();
                    if (parametros == null)
                    {
                        throw new BusinessException("Acesse o menu Configurações > Parâmetros Tributários e salve as configurações para a transmissão");
                    }

                    var header = new Dictionary<string, string>()
                    {
                        { "AppUser", AppUser },
                        { "PlataformaUrl", PlataformaUrl }
                    };

                    var entidade = CertificadoDigitalBL.GetEntidade();
                    var notafiscal = NotaFiscalBL.All.AsNoTracking().Where(x => x.Id == entity.NotaFiscalId).FirstOrDefault();

                    var cartaCorrecao = new CartaCorrecaoVM()
                    {
                        Homologacao = entidade.Homologacao,
                        Producao = entidade.Producao,
                        EntidadeAmbiente = entidade.EntidadeAmbiente,
                        Correcao = entity.MensagemCorrecao,
                        SefazChaveAcesso = notafiscal.SefazId
                    };

                    entity.Mensagem = null;
                    var response = RestHelper.ExecutePostRequest<CartaCorrecaoRetornoVM>(AppDefaults.UrlEmissaoNfeApi, "cartacorrecao", JsonConvert.SerializeObject(cartaCorrecao, JsonSerializerSetting.Edit), null, header);

                    entity.Status = StatusCartaCorrecao.Transmitida;
                    entity.IdRetorno = response.IdEvento;
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}