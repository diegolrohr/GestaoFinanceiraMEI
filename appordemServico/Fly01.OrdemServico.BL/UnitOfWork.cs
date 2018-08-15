using Fly01.Core.Base;
using Fly01.Core.Entities.Domains;
using Fly01.OrdemServico.API.Models.DAL;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Fly01.OrdemServico.BL
{
    public class UnitOfWork : UnitOfWorkBase
    {
        protected override IEnumerable<DbEntityEntry> ContextChangeTrackerEntries()
        {
            return Context.ChangeTracker.Entries();
        }

        protected override async Task ContextSaveChanges()
        {
            await Context.SaveChanges();
        }

        protected override void ContextDispose()
        {
            Context.Dispose();
        }

        public AppDataContext Context;
        public UnitOfWork(ContextInitialize initialize)
        {
            Context = new AppDataContext(initialize);
        }

        #region BLS
        //exemplo private EstadoBL estadoBL;
        //public EstadoBL EstadoBL => estadoBL ?? (estadoBL = new EstadoBL(Context));        

        #endregion
    }
}