using System;
using Fly01.Core.Base;
using System.Reflection;
using Fly01.Core.ServiceBus;
using System.Threading.Tasks;
using System.Collections.Generic;
using Fly01.Core;
using Newtonsoft.Json;

namespace Fly01.Faturamento.BL
{
    public class ServiceBusBL : Consumer
    {
        const string domainAssemblyName = "Fly01.Core.Entities";

        public ServiceBusBL()
        {
            SetupEnvironment.Create();

            base.Consume();
        }

        protected override async Task PersistMessage()
        {
            RabbitConfig.RoutingKey = "FormaPagamento"; //hardcode para testes
            var unitOfWorkAssembly = Type.GetType(Assembly.GetExecutingAssembly().GetName().Name + ".UnitOfWork");
            var domainAssembly = Assembly.Load(domainAssemblyName).GetType($"{domainAssemblyName}.Domains.Commons.{RabbitConfig.RoutingKey}");
            dynamic entidade = unitOfWorkAssembly.GetProperty(RabbitConfig.RoutingKey + "BL")?.GetGetMethod(false);

            exceptions = new List<KeyValuePair<string, object>>();

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
                        exceptions.Add(new KeyValuePair<string, object>(item.ToString(), ex));
                        continue;
                    }
                }
            }
        }
    }
}