using System;

namespace Fly01.Faturamento.Entities.Helpers
{
    public class APIEnumAttribute : Attribute
    {
        public string EnumContext { get; set; }

        public APIEnumAttribute(string APIPropertyName)
        {
            EnumContext = "Fly01.Faturamento.Domain." + APIPropertyName;
        }
    }
}