﻿using Fly01.Core.BL;
using Fly01.Core.Helpers;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using System.Linq;

namespace Fly01.EmissaoNFE.BL
{
    public class ChaveBL : PlataformaBaseBL<ChaveVM>
    {
        protected EmpresaBL EmpresaBL;
        protected EntidadeBL EntidadeBL;
        protected EstadoBL EstadoBL;

        public ChaveBL(AppDataContextBase context, EmpresaBL empresaBL, EntidadeBL entidadeBL, EstadoBL estadoBL) : base(context)
        {
            EmpresaBL = empresaBL;
            EntidadeBL = entidadeBL;
            EstadoBL = estadoBL;
        }
        
        public ChaveRetornoVM ChaveNFe(ChaveVM entity)
        {
            var chave = GeraChave(entity.CodigoUF.ToString(),
                                    entity.Emissao.Year.ToString(), 
                                    entity.Emissao.Month.ToString(),
                                    entity.Cnpj,
                                    entity.Modelo.ToString(),
                                    entity.Serie.ToString(),
                                    entity.NumeroNF.ToString(),
                                    ((int)entity.TipoDocumentoFiscal).ToString(),
                                    entity.CodigoNF.ToString()
                                    );

            if(chave == null)
            {
                entity.Notification.Errors.Add(new Error("Erro ao gerar a chave da NFe"));
                throw new BusinessException(entity.Notification.Get());
            }
            else
            {
                return new ChaveRetornoVM { Chave = chave.Replace("NFe", "") };
            }
        }

        public string GeraChave(string UF, string ano, string mes, string cnpj, string modelo, string serie, string numeroNota, string tipoNota, string codigoNF)
        {
            var retorno = UF + ano[2] + ano[3] + mes.PadLeft(2, '0') + cnpj.PadLeft(14, '0') + modelo.PadLeft(2, '0') +
                            serie.PadLeft(3, '0') + numeroNota.PadLeft(9, '0') + tipoNota + codigoNF.PadLeft(8, '0');
            int dv = 0;

            if (retorno.Length != 43)
                return null;
            else
            {
                var peso = 2;
                var total = 0;
                for (int x = retorno.Length - 1; x >= 0; x--)
                {
                    int valor = 0;
                    Int32.TryParse(retorno[x].ToString(), out valor);

                    total = total + valor * peso;
                    peso++;

                    if (peso == 10)
                        peso = 2;
                }

                dv = 11 - (total % 11);

                if (dv > 9)
                    dv = 0;

            }
            
            return "NFe" + retorno + dv.ToString();
        }

        public override void ValidaModel(ChaveVM entity)
        {
            #region Obrigatoriedades
            entity.Fail(entity.CodigoUF == 0, CodigoUFRequired);
            entity.Fail(string.IsNullOrEmpty(entity.Emissao.ToString()), EmissaoRequired);
            entity.Fail(entity.Modelo == 0, ModeloRequired);
            entity.Fail(entity.Serie == 0, SerieRequired);
            entity.Fail(string.IsNullOrEmpty(entity.TipoDocumentoFiscal.ToString()), TipoRequired);
            entity.Fail(entity.NumeroNF == 0, NumeroRequired);
            entity.Fail(entity.CodigoNF == 0, CodigoRequired);
            entity.Fail(string.IsNullOrEmpty(entity.Cnpj), CnpjRequired);
            #endregion

            #region Validações
            var tipoNota = EnumHelper.GetDataEnumValues(typeof(TipoNota));
            
            entity.Fail(entity.CodigoUF != 0 && (EstadoBL.All.Where(e => e.CodigoIbge == entity.CodigoUF.ToString()).FirstOrDefault() != null ? false : true), CodigoUFNotValid);
            entity.Fail(entity.Modelo != 0 && entity.Modelo > 99, ModeloNotValid);
            entity.Fail(entity.Serie != 0 && entity.Serie > 999, SerieNotValid);
            entity.Fail(!string.IsNullOrEmpty(entity.TipoDocumentoFiscal.ToString()) && !tipoNota.Any(x => x.Value == ((int)entity.TipoDocumentoFiscal).ToString()), TipoNotValid);
            entity.Fail(entity.NumeroNF != 0 && entity.NumeroNF.ToString().Length > 9, NumeroNotValid);
            entity.Fail(entity.CodigoNF != 0 && entity.CodigoNF.ToString().Length > 8, CodigoNotValid);
            entity.Fail(!string.IsNullOrEmpty(entity.Cnpj) && !EmpresaBL.ValidaCNPJ(entity.Cnpj), CnpjNotValid);
            #endregion

            base.ValidaModel(entity);
        }

        public static Error CodigoUFRequired = new Error("Código da UF é um dado obrigatório", "CodigoUF");
        public static Error EmissaoRequired = new Error("Data de emissão é um dado obrigatório", "Emissao");
        public static Error ModeloRequired = new Error("Modelo é um dado obrigatório", "Modelo");
        public static Error SerieRequired = new Error("Série é um dado obrigatório", "Serie");
        public static Error TipoRequired = new Error("Tipo do documento é um dado obrigatório", "TipoDocumentoFiscal");
        public static Error NumeroRequired = new Error("Número da nota é um dado obrigatório", "NumeroNF");
        public static Error CodigoRequired = new Error("Código da nota é um dado obrigatório", "CodigoNF");
        public static Error CnpjRequired = new Error("CNPJ do emitente é um dado obrigatório", "Cnpj");

        public static Error CodigoUFNotValid = new Error("O código da UF é inválido", "CodigoUF");
        public static Error ModeloNotValid = new Error("Modelo inválido", "Modelo");
        public static Error SerieNotValid = new Error("A série deve ser entre 1 e 999", "Serie");
        public static Error TipoNotValid = new Error("A série deve ser entre 1 e 999", "TipoDocumentoFiscal");
        public static Error NumeroNotValid = new Error("O número da nota deve ter no máximo 9 dígitos", "NumeroNF");
        public static Error CodigoNotValid = new Error("O código da nota deve ter no máximo 8 dígitos", "CodigoNF");
        public static Error CnpjNotValid = new Error("CNPJ inválido", "Cnpj");
    }
}
