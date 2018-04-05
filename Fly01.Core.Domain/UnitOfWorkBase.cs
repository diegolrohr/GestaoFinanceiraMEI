using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Fly01.Core.Domain
{
    public abstract class UnitOfWorkBase : IDisposable
    {
        protected abstract IEnumerable<DbEntityEntry> ContextChangeTrackerEntries();
        protected abstract Task ContextSaveChanges();
        protected abstract void ContextDispose();

        public async Task Save()
        {
            await ContextSaveChanges();
        }
        public void RejectChanges()
        {
            foreach (DbEntityEntry entry in ContextChangeTrackerEntries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        {
                            entry.CurrentValues.SetValues(entry.OriginalValues);
                            entry.State = EntityState.Unchanged;
                            break;
                        }
                    case EntityState.Deleted:
                        {
                            entry.State = EntityState.Unchanged;
                            break;
                        }
                    case EntityState.Added:
                        {
                            entry.State = EntityState.Detached;
                            break;
                        }
                }
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    ContextDispose();
                }
            }
            _disposed = true;
        }
        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public TBL GetGenericBL<TBL>()
        {
            try
            {
                return (TBL)GetType().GetProperty(typeof(TBL).Name).GetValue(this);
            }
            catch (Exception)
            {
                throw new Exception(string.Format("A classe {0} não está definida no {1}", typeof(TBL).Name, this.GetType().Name));
            }
        }
    }
}