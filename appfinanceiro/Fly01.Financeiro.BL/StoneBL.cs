using Fly01.Core.BL;
using Fly01.Core.Entities.Domains;
using Fly01.Core.Notifications;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Fly01.Financeiro.BL
{
    public class StoneBL
    {
        private Notification Notification { get; set; }

        public StoneBL(AppDataContextBase context)
        {
            Notification = new Notification();
            Notification.Errors = new List<Error>();
        }

        public void ValidaAutenticacaoStone(AutenticacaoStoneVM entity)
        {
            CleanErros();
            Fail(string.IsNullOrEmpty(entity.Password), new Error("Informe a senha.", "senha"));
            Fail(string.IsNullOrEmpty(entity.Email), new Error("Informe o e-mail.", "email"));
            
            const string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            Fail(!string.IsNullOrEmpty(entity.Email) && (!Regex.IsMatch(entity.Email ?? "", pattern)), new Error("Informe um e-mail válido.", "email"));
            ThrowErros();
        }

        public void ValidaToken(StoneAutenticacaoVM entity)
        {
            CleanErros();
            FailToken(entity.Token);
            ThrowErros();
        }

        public void ValidaAntecipacaoSimular(StoneAntecipacaoSimularVM entity)
        {
            CleanErros();
            FailToken(entity.Token);
            Fail(entity.Valor <= 0, new Error("Informe um valor válido.", "valor"));
            ThrowErros();
        }

        public void ValidaAntecipacaoEfetivar(StoneAntecipacaoEfetivarPostVM entity)
        {
            CleanErros();
            FailToken(entity.Token);
            Fail(entity.Valor <= 0, new Error("Valor inválido.", "valor"));
            Fail(entity.StoneBancoId <= 0, new Error("Informe o banco para efetivar.", "stoneBancoId"));
            ThrowErros();
        }

        public void ValidaAntecipacaoConfiguracao(StoneTokenBaseVM entity)
        {
            CleanErros();
            FailToken(entity.Token);
            ThrowErros();
        }

        public void ValidaAntecipacaoConsultar(StoneTokenBaseVM entity)
        {
            CleanErros();
            FailToken(entity.Token);
            ThrowErros();
        }

        public void ValidaAntecipacaoDadosBancarios(StoneTokenBaseVM entity)
        {
            CleanErros();
            FailToken(entity.Token);
            ThrowErros();
        }

        public void FailToken(string token)
        {
            Fail(string.IsNullOrEmpty(token), new Error("Informe o token.", "token"));
        }

        #region Validation codes
        private void Fail(bool condition, Error error)
        {
            if (condition)
                Notification.Errors.Add(error);
        }

        private void CleanErros()
        {
            Notification.Errors.Clear();
        }

        private void ThrowErros()
        {
            if (Notification.HasErrors)
            {
                throw new BusinessException(Notification.Get());
            }
        }
        #endregion
    }
}