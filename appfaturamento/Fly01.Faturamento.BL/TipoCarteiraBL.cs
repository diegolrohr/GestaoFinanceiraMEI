using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.Faturamento.Domain.Enums;
using System;

namespace Fly01.Faturamento.BL
{
    public static class TipoCarteiraBL
    {
        public static void ValidaTipoCarteira(TipoCarteira tipoCarteira)
        {
            if (!Enum.IsDefined(typeof(TipoCarteira), tipoCarteira))
                throw new BusinessException("Não foi possível inserir este registro. Tipo carteira inválido.");
        }
    }
}
