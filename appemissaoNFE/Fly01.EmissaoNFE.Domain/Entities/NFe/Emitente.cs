﻿using Fly01.EmissaoNFE.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class Emitente
    {
        [Required]
        [StringLength(14, ErrorMessage = "CNPJ inválido")]
        /// <summary>
        /// informar o CNPJ do emitente, sem formatação ou máscara
        /// </summary>
        [XmlElement(ElementName = "CNPJ")]
        public string Cnpj { get; set; }

        [Required]
        [StringLength(11, ErrorMessage = "CPF inválido")]
        /// <summary>
        /// informar o CPF do emitente, sem formatação ou máscara, utilizado apenas quando o fisco emite a nota fiscal
        /// </summary>
        [XmlElement(ElementName = "CPF")]
        public string Cpf { get; set; }

        [Required]
        [MaxLength(60)]
        /// <summary>
        /// informar a razão social do emitente
        /// </summary>
        [XmlElement(ElementName = "xNome")]
        public string Nome { get; set; }
        
        [MaxLength(60)]
        /// <summary>
        /// informar o nome fantasia do emitente, pode ser omitido
        /// </summary>
        [XmlElement(ElementName = "xFant")]
        public string NomeFantasia { get; set; }

        [Required]
        [XmlElement(ElementName = "enderEmit")]
        public Endereco Endereco { get; set; }

        [Required]
        [MaxLength(14)]
        /// <summary>
        /// informar a IE do emitente, sem formatação ou máscara
        /// </summary>
        [XmlElement(ElementName = "IE")]
        public string InscricaoEstadual { get; set; }

        [Required]
        /// <summary>
        /// informar o Código de Regime Tributário - CRT, valores válidos: 1 - Simples Nacional; 2 - Simples Nacional - excesso de sublimite de receita bruta; 3 - Regime Normal 
        /// </summary>
        [XmlElement(ElementName = "CRT")]
        public CRT CRT { get; set; }
    }
}
