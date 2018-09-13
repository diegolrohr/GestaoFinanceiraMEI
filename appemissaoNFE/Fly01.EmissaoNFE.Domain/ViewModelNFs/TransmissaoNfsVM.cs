using Fly01.Core.Entities.Domains;
using Fly01.EmissaoNFE.Domain.Entities.NFs;
using System.Collections.Generic;

namespace Fly01.EmissaoNFE.Domain.ViewModelNfs
{
    public class TransmissaoNFSVM : PlataformaBase
    {
        public Identificacao Identificacao { get; set; }

        public Atividade Atividade { get; set; }

        public Prestador Prestador { get; set; }

        public Prestacao Prestacao { get; set; }

        public Tomador Tomador { get; set; }

        public List<Servico> Servicos { get; set; }

        public Valores Valores { get; set; }

        public InformacoesComplementares InformacoesComplementares { get; set; }
    }
}
