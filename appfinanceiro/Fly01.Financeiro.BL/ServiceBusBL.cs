using System;
using System.Reflection;
using Fly01.Core.Base;
using Fly01.Core.ServiceBus;
using System.Threading.Tasks;
using System.Collections.Generic;
using Fly01.Core;
using Newtonsoft.Json;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.BL
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
            RabbitConfig.RoutingKey = "ContaPagar";
            var unitOfWorkAssembly = Type.GetType(Assembly.GetExecutingAssembly().GetName().Name + ".UnitOfWork");
            var domainAssembly = Assembly.Load("Fly01.Core.Entities").GetType("Fly01.Core.Entities.Domains.Commons." + RabbitConfig.RoutingKey);
            dynamic entidade = unitOfWorkAssembly.GetProperty(RabbitConfig.RoutingKey + "BL")?.GetGetMethod(false);

            exceptions = new Dictionary<string, Exception>();

            using (var unitOfWork = new UnitOfWork(new ContextInitialize() { AppUser = RabbitConfig.AppUser, PlataformaUrl = RabbitConfig.PlataformaUrl }))
            {
                foreach (var item in MessageType.Resolve<dynamic>(Message))
                {
                    try
                    {
                        var data = JsonConvert.DeserializeObject(item.ToString(), domainAssembly);

                        entidade?.Invoke(unitOfWork, null).PersistMessage(data, HTTPMethod);
                        await unitOfWork.Save();
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(item, ex);
                        continue;
                    }
                }
            }
        }
    }
}