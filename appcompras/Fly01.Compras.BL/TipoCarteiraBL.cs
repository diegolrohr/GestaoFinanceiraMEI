using Fly01.Compras.Domain.Enums;
using Fly01.Core.ValueObjects;
using System;

namespace Fly01.Compras.BL
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