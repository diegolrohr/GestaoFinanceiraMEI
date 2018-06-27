using System;
using Fly01.Core;
using Fly01.Core.Base;
using System.Reflection;
using Fly01.Core.ServiceBus;
using System.Threading.Tasks;
using Fly01.Core.Entities.Domains;
using System.Collections.Generic;

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

            //var domainAssembly = Type.GetType("Fly01.Core.Entities.Domains.Commons." + RabbitConfig.RoutingKey);

            var exceptions = new List<Exception>();
            using (var unitOfWork = new UnitOfWork(new ContextInitialize() { AppUser = RabbitConfig.AppUser, PlataformaUrl = RabbitConfig.PlataformaUrl }))
            {
                foreach (var item in MessageType.Resolve<dynamic>(Message))
                {
                    try
                    {
                        entidade?.Invoke(unitOfWork, null).PersistMessage(item, HTTPMethod);
                        await unitOfWork.Save();
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                        continue;
                    }
                }
            }
        }
    }
}