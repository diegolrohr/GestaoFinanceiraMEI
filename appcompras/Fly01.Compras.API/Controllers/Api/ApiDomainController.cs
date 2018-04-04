using System.Linq;
using Fly01.Compras.BL;
using Fly01.Core.Api.BL;
using Fly01.Core.Api.Domain;
using System.Threading.Tasks;
using Fly01.Core.Controllers.API;

namespace Fly01.Compras.API.Controllers.Api
{
    public class ApiDomainController<TEntity, TBL> : ApiDomainBaseController<TEntity>
        where TEntity : DomainBase, new()
        where TBL : DomainBaseBL<TEntity>
    {
        protected bool isServiceBusRoute { get; set; }

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
    }
}