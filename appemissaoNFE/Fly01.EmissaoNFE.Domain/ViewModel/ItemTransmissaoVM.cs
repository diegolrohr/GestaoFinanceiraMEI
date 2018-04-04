using Fly01.EmissaoNFE.Domain.Entities.NFe;
using System.Collections.Generic;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class ItemTransmissaoVM
    {
        /// <summary>
        /// ID da nota
        /// </summary>
        public string NotaId { get; set; }

        /// <summary>
        /// Versão de NFe utilizada pelo cliente
        /// </summary>
        public string Versao { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        public Identificador Identificador { get; set; }

        /// <summary>
        /// Emitente
        /// </summary>
        public Emitente Emitente { get; set; }

        /// <summary>
        /// Destinatário
        /// </summary>
        public Destinatario Destinatario { get; set; }

        /// <summary>
        /// Detalhes (Produto/Imposto) 
        /// </summary>
        public List<Detalhe> Detalhes { get; set; }

        /// <summary>
        /// Totais
        /// </summary>
        public Total Total { get; set; }

        /// <summary>
        /// Transporte
        /// </summary>
        public Transporte Transporte { get; set; }

        /// <summary>
        /// Cobranca
        /// </summary>
        public Cobranca Cobranca { get; set; }

        /// <summary>
        /// Informações Adicionais
        /// </summary>
        public InformacoesAdicionais InformacoesAdicionais { get; set; }
    }
}
