using System;
using Fly01.Core.Base;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Fly01.Core.Defaults;

namespace Fly01.Core.ServiceBus
{
    //public class ServiceBusBase : Consumer
    //{
    //    private dynamic data;
    //    private dynamic entidade;
    //    private Object unitOfWork;
    //    private Type AssemblyBL { get; set; }

    //    public ServiceBusBase(Type assemblyBL)
    //    {
    //        SetupEnvironment.Create();
    //        if (AssemblyBL == null)
    //            AssemblyBL = assemblyBL;
    //    }

    //    protected override async Task DeliverMessage(string message, RabbitConfig.EnHttpVerb httpMethod, string routingKey, string appId, string plataformaUrl, string appUser)
    //    {
    //        var domainAssembly = Assembly.Load("Fly01.Core.Entities").GetType($"Fly01.Core.Entities.Domains.Commons.{routingKey}");
    //        exceptions = new List<KeyValuePair<string, object>>();

    //        foreach (var item in MessageType.Resolve<dynamic>(message))
    //        {
    //            try
    //            {
    //                unitOfWork = AssemblyBL.GetConstructor(new Type[1] { typeof(ContextInitialize) }).Invoke(new object[] { new ContextInitialize() { AppUser = appUser, PlataformaUrl = plataformaUrl } });
    //                entidade = AssemblyBL.GetProperty($"{routingKey}BL")?.GetGetMethod(false)?.Invoke(unitOfWork, null);
    //                data = JsonConvert.DeserializeObject<dynamic>(item.ToString());
                    
    //                entidade.PersistMessage(data, httpMethod, appId.ToLower() == "bemacash");

    //                await (Task)AssemblyBL.GetMethod("Save").Invoke(unitOfWork, new object[] { });
    //            }
    //            catch (Exception exErr)
    //            {
    //                data = null;
    //                exceptions.Add(new KeyValuePair<string, object>(item.ToString(), exErr));
    //                continue;
    //            }
    //        }
    //    }
    //}
}