namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class InutilizarNFVM : EntidadeVM
    {
        public int EmpresaCodigoUF { get; set; }
        public string EmpresaCnpj { get; set; }
        public int ModeloDocumentoFiscal { get; set; }
        public int Serie { get; set; }
        public int Numero { get; set; }
    }
}