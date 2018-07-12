using System;
using Fly01.Core.Base;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Configuration;
using Fly01.Core.Entities.Domains.NoSQL;
using Fly01.Core.Helpers;

namespace Fly01.Core.ServiceBus
{
    public class ServiceBusBase : Consumer
    {
        private Type AssemblyBL { get; set; }

        public ServiceBusBase(Type assemblyBL)
        {
            SetupEnvironment.Create();
            AssemblyBL = assemblyBL;

            base.Consume();
        }

        protected override async Task PersistMessage()
        {
            var domainAssembly = Assembly.Load("Fly01.Core.Entities").GetType($"Fly01.Core.Entities.Domains.Commons.{RabbitConfig.RoutingKey}");
            var uow = AssemblyBL.GetConstructor(new Type[1] { typeof(ContextInitialize) }).Invoke(new object[] { new ContextInitialize() { AppUser = RabbitConfig.AppUser, PlataformaUrl = RabbitConfig.PlataformaUrl } });
            dynamic entidade = AssemblyBL.GetProperty(RabbitConfig.RoutingKey + "BL")?.GetGetMethod(false)?.Invoke(uow, null);
            exceptions = new List<KeyValuePair<string, object>>();
            var lotSize = 50;
            var content = MessageType.Resolve<dynamic>(Message);
            dynamic data;

            var i = 0;
            while (true)
            {
                data = JsonConvert.DeserializeObject(content[i].ToString(), domainAssembly);

                try
                {
                    entidade.PersistMessage(data, HTTPMethod);

                    if (content.Count == 1 || lotSize == 1) //insert individual
                        await (Task)AssemblyBL.GetMethod("Save").Invoke(uow, new object[] { });
                    else if ((i >= lotSize && i % lotSize == 0)) //insert em lote
                        await (Task)AssemblyBL.GetMethod("Save").Invoke(uow, new object[] { });
                    else if (content.Count - i <= lotSize && i == content.Count - 1) //insert do restante do pacote
                        await (Task)AssemblyBL.GetMethod("Save").Invoke(uow, new object[] { });

                    i++;
                }
                catch (Exception ex)
                {
                    exceptions.Add(new KeyValuePair<string, object>(content[i].ToString(), ex));

                    continue;
                }

                if (i == content.Count)
                    return;
            }

            //for (var i = 0; i < content.Count; i++)
            //{
            //    data = JsonConvert.DeserializeObject(content[i].ToString(), domainAssembly);

            //    try
            //    {
            //        entidade.PersistMessage(data, HTTPMethod);

            //        if (content.Count == 1 || lotSize == 1) //insert individual
            //            await (Task)AssemblyBL.GetMethod("Save").Invoke(uow, new object[] { });
            //        else if ((i >= lotSize && i % lotSize == 0)) //insert em lote
            //            await (Task)AssemblyBL.GetMethod("Save").Invoke(uow, new object[] { });
            //        else if (content.Count - i <= lotSize && i == content.Count - 1) //insert do restante do pacote
            //            await (Task)AssemblyBL.GetMethod("Save").Invoke(uow, new object[] { });

            //        erroRegistrado = false;
            //    }
            //    catch (Exception ex)
            //    {
            //        if (!erroRegistrado)
            //        {
            //            i = i - lotSize > 0 ? i - lotSize : -1;
            //            lotSize = 1;
            //            erroRegistrado = true;
            //            data = string.Empty;
            //            //entidade = null;
            //            //entidade = AssemblyBL.GetProperty(RabbitConfig.RoutingKey + "BL")?.GetGetMethod(false)?.Invoke(uow, null);
            //        }
            //        else
            //            exceptions.Add(new KeyValuePair<string, object>(content[i].ToString(), ex));

            //        continue;
            //    }
            //}
        }
    }
}