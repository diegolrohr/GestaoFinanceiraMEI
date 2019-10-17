using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;
using Fly01.Core.Helpers;
using System;
using System.IO;
using System.Linq;
using Fly01.Core.Notifications;

namespace Fly01.Financeiro.BL
{
    public class ConciliacaoBancariaBL : EmpresaBaseBL<ConciliacaoBancaria>
    {
        protected ContaBancariaBL contaBancariaBL { get; set; }
        protected ConciliacaoBancariaItemBL conciliacaoBancariaItemBL { get; set; }

        public ConciliacaoBancariaBL(AppDataContext context, ConciliacaoBancariaItemBL ConciliacaoBancariaItemBL, ContaBancariaBL ContaBancariaBL) : base(context)
        {
            this.conciliacaoBancariaItemBL = ConciliacaoBancariaItemBL;
            this.contaBancariaBL = ContaBancariaBL;
        }

        
        private void ValidaModel(ConciliacaoBancaria entity, string bancoDoArquivo)
        {
            if (All.Any(x => x.ContaBancariaId == entity.ContaBancariaId & x.Id != entity.Id))
                throw new BusinessException("Já existe uma conciliação bancária com a mesma conta bancária");

            var firstOrDefault = contaBancariaBL.All.Where(x => x.Id == entity.ContaBancariaId).Select(y => y.Banco).FirstOrDefault();

            if (firstOrDefault == null)
                throw new BusinessException("Não existem contas bancárias cadastradas");

            var bancoCodigo = firstOrDefault.Codigo;

            //ex: 341 no arquivo vem 0341
            int bancoDoArquivoInt = 0;
            int bancoCodigoInt = 0;

            Int32.TryParse(bancoDoArquivo, out bancoDoArquivoInt);
            Int32.TryParse(bancoCodigo, out bancoCodigoInt);

            if (bancoCodigoInt != bancoDoArquivoInt)
                throw new BusinessException("Banco da conta bancária selecionada é diferente do banco do arquivo");
        }

        protected string SalvarEmArquivoTemporario(string arquivoBase64)
        {
            string tempPath = Path.GetTempFileName();
            string arquivoDecode = Base64Helper.DecodificaBase64(arquivoBase64);

            File.WriteAllText(tempPath, arquivoDecode);

            return tempPath;
        }

        public override void Insert(ConciliacaoBancaria entity)
        {
            try
            {
                var tempPath = SalvarEmArquivoTemporario(entity.Arquivo);
                var arquivoOFX = new ArquivoOFX(tempPath);

                var bancoId = arquivoOFX.GetBancoId();
                var lancamentos = arquivoOFX.GetLancamentos();

                ValidaModel(entity, bancoId);

                //gero a Guid para já vincular os itens
                entity.Id = Guid.NewGuid();
                conciliacaoBancariaItemBL.SalvarConciliacaoBancariaItensExtrato(entity.Id, lancamentos);

                base.Insert(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public override void Update(ConciliacaoBancaria entity)
        {
            try
            {
                if (entity.Arquivo != null)
                {
                    var tempPath = SalvarEmArquivoTemporario(entity.Arquivo);
                    var arquivoOFX = new ArquivoOFX(tempPath);

                    var bancoId = arquivoOFX.GetBancoId();
                    var lancamentos = arquivoOFX.GetLancamentos();

                    ValidaModel(entity, bancoId);
                    conciliacaoBancariaItemBL.SalvarConciliacaoBancariaItensExtrato(entity.Id, lancamentos);
                }

                base.Update(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        //public override void Delete(ConciliacaoBancaria entityToDelete)
        //{
        //    throw new BusinessException("Não é possível deletar uma conciliação bancária, somente seus lançamentos");
        //}
    }
}