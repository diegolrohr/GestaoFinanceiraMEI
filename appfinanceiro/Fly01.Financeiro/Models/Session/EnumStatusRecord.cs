using System;

namespace Fly01.Financeiro.Models.Session
{
    [Serializable]
    public enum EnumStatusRecord
    {
        srNone = 0,
        srInsert = 1,
        srUpdate = 2,
        srDelete = 3
    }
}