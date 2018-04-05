using Fly01.Core.Notifications;
using Fly01.Financeiro.Domain.Enums;
using System;

namespace Fly01.Financeiro.BL
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
