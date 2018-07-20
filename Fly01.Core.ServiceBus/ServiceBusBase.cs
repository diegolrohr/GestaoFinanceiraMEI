using System;
using Fly01.Core.Base;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Fly01.Core.ServiceBus
{
    public class ServiceBusBase : Consumer
    {
        private dynamic data;
        private dynamic entidade;
        private Object unitOfWork;
        private Type AssemblyBL { get; set; }

        public ServiceBusBase(Type assemblyBL)
        {
            SetupEnvironment.Create();

            if (AssemblyBL == null)
                AssemblyBL = assemblyBL;
        }

        protected override async Task DeliverMessage()
        {
            var domainAssembly = Assembly.Load("Fly01.Core.Entities").GetType($"Fly01.Core.Entities.Domains.Commons.{RabbitConfig.RoutingKey}");
            exceptions = new List<KeyValuePair<string, object>>();
            unitOfWork = AssemblyBL.GetConstructor(new Type[1] { typeof(ContextInitialize) }).Invoke(new object[] { new ContextInitialize() { AppUser = RabbitConfig.AppUser, PlataformaUrl = RabbitConfig.PlataformaUrl } });
            entidade = AssemblyBL.GetProperty($"{RabbitConfig.RoutingKey}BL")?.GetGetMethod(false)?.Invoke(unitOfWork, null);

            foreach (var item in MessageType.Resolve<dynamic>(Message))
            {
                try
                {
                    data = JsonConvert.DeserializeObject(item.ToString(), domainAssembly);

                    entidade.PersistMessage(data, HTTPMethod);
                   
                    await (Task)AssemblyBL.GetMethod("Save").Invoke(unitOfWork, new object[] { });
                }
                catch (Exception exErr)
                {
                    exceptions.Add(new KeyValuePair<string, object>(item.ToString(), exErr));
                    continue;
                }
            }
        }
    }
}