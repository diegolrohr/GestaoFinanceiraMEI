using System;
using System.IO;
using System.Reflection;

namespace Fly01.Compras.Helpers
{
    public static class EmailFilesHelper
    {
        public static Lazy<string> GetTemplate(string resourceName)
        {
            return new Lazy<string>(() => GetResource(resourceName));
        }

        public static string GetResource(string templatePath)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            foreach (var resourceName in currentAssembly.GetManifestResourceNames())
            {
                if (resourceName.ToLower().EndsWith(templatePath.ToLower()))
                {
                    using (var stream = currentAssembly.GetManifestResourceStream(resourceName))
                    {
                        using (var reader = new StreamReader(stream))
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