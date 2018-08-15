﻿using System;
using System.Reflection;
using Fly01.Core.ServiceBus;

namespace Fly01.OrdemServico.BL
{
    public class ServiceBusBL : ServiceBusBase
    {
        public ServiceBusBL() : base(Type.GetType($"{Assembly.GetExecutingAssembly().GetName().Name}.UnitOfWork")) { }
    }
}