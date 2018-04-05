using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Domain;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System;
using System.Text.RegularExpressions;

namespace Fly01.EmissaoNFE.BL
{
    public class EmailBL : PlataformaBaseBL<EmailVM>
    {
        protected EntidadeBL EntidadeBL;
        public EmailBL(AppDataContextBase context, EntidadeBL entidadeBL) : base(context)
        {
            EntidadeBL = entidadeBL;
        }

        public override void ValidaModel(EmailVM entity)
        {
            EntidadeBL.ValidaModel(entity);

            Regex mail = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");

            entity.Fail(!mail.IsMatch(entity.Email), EmailInvalido);
            entity.Fail(!mail.IsMatch(entity.EmailAdicional), EmailAdicionalInvalido);

            if (entity.Autenticacao)
            {
                entity.Fail(string.IsNullOrEmpty(entity.Login), LoginRequerido);
                entity.Fail(entity.Senha == null || entity.Senha.Length == 0, SenhaRequerida);                
            }            

            base.ValidaModel(entity);
        }

        public static Error EmailInvalido = new Error("E-mail inválido.", "Email");
        public static Error EmailAdicionalInvalido = new Error("E-mail adicional inválido.", "EmailAdicional");
        public static Error LoginRequerido = new Error("Login é um campo obrigatório.", "Login");
        public static Error SenhaRequerida = new Error("Senha é um campo obrigatório.", "Senha");
    }
}
