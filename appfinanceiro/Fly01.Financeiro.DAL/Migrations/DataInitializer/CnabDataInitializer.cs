using System;
using System.Linq;
using Fly01.Financeiro.API.Models.DAL;

namespace Fly01.Financeiro.DAL.Migrations.DataInitializer
{
    public class CnabDataInitializer
    {
        public void Initialize(AppDataContext context)
        {
            var cnabs = context.Cnab.AsEnumerable().Where(x => x.PessoaId == null).ToList();

            if (cnabs != null)
            {
                foreach (var item in cnabs)
                {
                    var idPessoa = context.ContasReceber.FirstOrDefault(x => x.Id == item.ContaReceberId).PessoaId;
                    item.PessoaId = new Guid(idPessoa.ToString());
                }
            }

            context.SaveChanges();            
        }
    }
}