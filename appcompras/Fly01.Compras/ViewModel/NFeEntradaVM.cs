﻿using Newtonsoft.Json;
using System;

namespace Fly01.Compras.ViewModel
{
    [Serializable]
    public class NFeEntradaVM : NotaFiscalEntradaVM
    {
        [JsonProperty("totalImpostosProdutos")]
        public double TotalImpostosProdutos { get; set; }

        [JsonProperty("totalImpostosProdutosNaoAgrega")]
        public double TotalImpostosProdutosNaoAgrega { get; set; }
    }
}