using System;
using System.IO;
using System.Reflection;

namespace Fly01.Financeiro.API.Models
{
    internal static class EmailFilesHelper
    {
        internal static Lazy<String> NotificaEmailContasFinanceira = new Lazy<string>(() =>
        {
            return GetResource("NotificaContasFinanceira.index.html");
        });

        private static string GetResource(string templatePath)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            foreach (var resourceName in currentAssembly.GetManifestResourceNames())
            {
                if (resourceName.ToLower().EndsWith(templatePath.ToLower()))
                {
                    using (Stream stream = currentAssembly.GetManifestResourceStream(resourceName))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }

            throw new FileNotFoundException("Não encontrou o resource (" + templatePath + ")");
        }
    }
}