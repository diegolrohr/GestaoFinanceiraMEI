﻿using Fly01.Core.Entities.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoLayoutNFSE
    {
        [Subtitle("DSFNET", "DSFNET")]
        DSFNET = 1,

        [Subtitle("ABRASFWebIss", "ABRASF - WebIss")]
        ABRASFWebIss = 2,

        [Subtitle("Egoverne", "Egoverne")]
        Egoverne = 3,

        [Subtitle("ISSWEB", "ISSWEB")]
        ISSWEB = 4,

        [Subtitle("ABRASFPublica", "ABRASF - Pública")]
        ABRASFPublica = 5,

        [Subtitle("XML", "XML")]
        XML = 6
    }
}