using System;
using Fly01.Core.Base;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace Fly01.Core.ServiceBus
{
    public class ServiceBusBase : Consumer
    {
        private dynamic data;
        private dynamic entidade;
        private Object unitOfWork;
        private static Object thisLock = new Object();
        private Type AssemblyBL { get; set; }
        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public ServiceBusBase(Type assemblyBL)
        {
            SetupEnvironment.Create();
            AssemblyBL = assemblyBL;

            base.Consume();
        }

        protected override async Task PersistMessage()
        {
            var domainAssembly = Assembly.Load("Fly01.Core.Entities").GetType($"Fly01.Core.Entities.Domains.Commons.{RabbitConfig.RoutingKey}");
            var content = MessageType.Resolve<dynamic>(Message);
            exceptions = new List<KeyValuePair<string, object>>();
            unitOfWork = AssemblyBL.GetConstructor(new Type[1] { typeof(ContextInitialize) }).Invoke(new object[] { new ContextInitialize() { AppUser = RabbitConfig.AppUser, PlataformaUrl = RabbitConfig.PlataformaUrl } });
            entidade = AssemblyBL.GetProperty(RabbitConfig.RoutingKey + "BL")?.GetGetMethod(false)?.Invoke(unitOfWork, null);

            foreach (var item in content)
            {
                try
                {
                    data = JsonConvert.DeserializeObject(item.ToString(), domainAssembly);

                    entidade.PersistMessage(data, HTTPMethod);

                    //await semaphoreSlim.WaitAsync();
                    await (Task)AssemblyBL.GetMethod("Save").Invoke(unitOfWork, new object[] { });
                }
                catch (Exception exErr)
                {
                    exceptions.Add(new KeyValuePair<string, object>(data, exErr));
                    continue;
                }
                //finally
                //{
                //    semaphoreSlim.Release();
                //}
            }
        }
    }
}