using Fly01.Core.Entities.Domains.Commons;
using Fly01.EmissaoNFE.Domain;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fly01.EmissaoNFE.DAL.Migrations.DataInitializer
{
    public class CestDataInitializer
    {
        public void Initialize(AppDataContext context, List<Estado> ufs)
        {
            if (!context.TabelaIcms.Any())
            {
                var listIcmss = new List<TabelaIcms>();
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                if (baseDirectory.Contains(@"\Debug\"))
                {
                    baseDirectory = baseDirectory.Replace(@"bin\Debug\", "");
                }
                else
                {
                    baseDirectory = baseDirectory.Replace(@"bin\", "");
                }

                using (TextFieldParser parser = new TextFieldParser(string.Concat(baseDirectory, @"ICMS.csv")))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(";");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();

                        Guid ufOrig = ufs.Where(e => e.Sigla.ToUpper() == fields[0].ToUpper()).Select(x => x.Id).FirstOrDefault();
                        Guid ufDest = ufs.Where(e => e.Sigla.ToUpper() == fields[1].ToUpper()).Select(x => x.Id).FirstOrDefault();
                        double aliquotaIcms = double.Parse(fields[2]);

                        listIcmss.Add(new TabelaIcms() { Id = Guid.NewGuid(), DataInclusao = DateTime.Now, UsuarioInclusao = "SEED", Ativo = true, EstadoOrigemId = ufOrig, SiglaOrigem = fields[0].ToUpper(), EstadoDestinoId = ufDest, SiglaDestino = fields[1].ToUpper(), IcmsAliquota = aliquotaIcms });
                    }
                }

                context.TabelaIcms.AddRange(listIcmss);
                context.SaveChanges();
            }            
        }
    }
}
