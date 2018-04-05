using Fly01.Core.Notifications;

namespace Fly01.Core.Helpers
{
    public static class ConversorHelper
    {
        /// <summary>
        /// Função que converte tipos inteiros e textos passados como 1 ou 0 para true e false respectivamente.
        /// </summary>
        /// <param name="value">Valor boolean, int ou string</param>
        /// <returns>Boolean</returns>
        public static bool ToBool(object value)
        {
            var type = value.GetType().Name;
            switch (type)
            {
                case "String":
                    switch (value as string)
                    {
                        case "1": return true;
                        case "0": return false;
                        case "true": return true;
                        case "false": return false;
                        case "verdadeiro": return true;
                        case "falso": return false;
                        default: throw new BusinessException("Valor inválido.");
                    }
                case "Int32":
                    switch ((int)value)
                    {
                        case 1: return true;
                        case 0: return false;
                        default: throw new BusinessException("Valor inválido.");
                    }
                case "Boolean":
                    return (bool)value;
                default:
                    throw new BusinessException("Valor ou tipo inválido.");
            }
        }
    }
}