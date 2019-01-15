using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class MinhaConta
    {
        public string Descricao { get; set; }

        public string DataEmissao { get; set; }

        public string NFE { get; set; }

        public string Numero { get; set; }

        public double Valor { get; set; }

        public string Prefixo{ get; set; }

        public string Vencimento { get; set; }

        public string Situacao { get; set; }

        public string UrlBoleto { get; set; }

        public string UrlNfe { get; set; }

        public string Empresa { get; set; }

        public string Filial { get; set; }

        public string Parcela { get; set; }
    }
}

