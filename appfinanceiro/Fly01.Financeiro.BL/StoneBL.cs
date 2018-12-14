using Fly01.Core.BL;
using Fly01.Core.Entities.Domains;
using Fly01.Core.Notifications;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System.Collections.Generic;

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
            Fail(string.IsNullOrEmpty(entity.Email), new Error("Informe o e-mail.", "email"));
            Fail(string.IsNullOrEmpty(entity.Password), new Error("Informe a senha.", "senha"));
            ThrowErros();
        }

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
    }
}