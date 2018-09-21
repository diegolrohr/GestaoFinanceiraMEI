namespace Fly01.EmissaoNFE.Domain.ViewModelNFS
{
    public class TransmissaoNFSRetornoVM
    {
        /// <summary>
        /// ID da nota
        /// </summary>
        public string NotaId { get; set; }
        public SchemaXMLNFSRetornoVM Error { get; set; }
        public string XML { get; set; }
    }
}
