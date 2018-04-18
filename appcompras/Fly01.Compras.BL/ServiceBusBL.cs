using System;
using Fly01.Core.Base;
using System.Reflection;
using Fly01.Core.ServiceBus;
using System.Threading.Tasks;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.BL
{
    public class ServiceBusBL : Consumer
    {
        public ServiceBusBL()
        {
            SetupEnvironment.Create();
            
            base.Consume();
        }

        protected override async Task PersistMessage()
        {
            var unitOfWorkAssembly = Type.GetType(Assembly.GetExecutingAssembly().GetName().Name + ".UnitOfWork");
            dynamic entidade = unitOfWorkAssembly.GetProperty(RabbitConfig.RoutingKey + "BL")?.GetGetMethod(false);

            using (var unitOfWork = new UnitOfWork(new ContextInitialize() { AppUser = RabbitConfig.AppUser, PlataformaUrl = RabbitConfig.PlataformaUrl }))
            {
                entidade?.Invoke(unitOfWork, null).PersistMessage(Message, HTTPMethod);

                await unitOfWork.Save();
            }
        }
    }
}