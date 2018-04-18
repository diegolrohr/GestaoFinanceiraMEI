using System.Linq;
using System.Threading.Tasks;
using Fly01.Estoque.BL;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains;
using Fly01.Core.BL;

namespace Fly01.Estoque.API.Controllers.Api
{
    public class ApiPlataformaController<TEntity, TBL> : ApiPlataformaBaseController<TEntity>
        where TEntity : PlataformaBase, new()
        where TBL : PlataformaBaseBL<TEntity>
    {
        private UnitOfWork _unitOfWork;

        protected UnitOfWork UnitOfWork
        {
            get { return _unitOfWork ?? (_unitOfWork = new UnitOfWork(ContextInitialize)); }
            set { _unitOfWork = value; }
        }

        protected override IQueryable<TEntity> All()
        {
            return UnitOfWork.GetGenericBL<TBL>().All;
        }

        protected override void Update(TEntity entity)
        {
            UnitOfWork.GetGenericBL<TBL>().Update(entity);
        }

        protected override bool Exists(object primaryKey)
        {
            return UnitOfWork.GetGenericBL<TBL>().Exists(primaryKey);
        }

        protected override void UnitDispose(bool disposing)
        {
            UnitOfWork?.Dispose();
        }

        protected async override Task UnitSave()
        {
            await UnitOfWork.Save();
        }

        protected override void Insert(TEntity entity)
        {
            UnitOfWork.GetGenericBL<TBL>().Insert(entity);
        }

        protected override void Delete(TEntity primaryKey)
        {
            UnitOfWork.GetGenericBL<TBL>().Delete(primaryKey);
        }

        protected override TEntity Find(object id)
        {
            return UnitOfWork.GetGenericBL<TBL>().Find(id);
        }
    }
}