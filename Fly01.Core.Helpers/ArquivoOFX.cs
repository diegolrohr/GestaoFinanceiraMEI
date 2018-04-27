using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using System;
using System.Globalization;

namespace Fly01.Core.Helpers
{
    public class OFXLancamento
    {
        public DateTime Data { get; set; }
        public double Valor { get; set; }
        public string Descricao { get; set; }
        public string MD5 { get; set; }
    }

    public class ArquivoOFX
    {
        private XElement _XElement { get; set; }
        private List<OFXLancamento> _lancamentos { get; set; }

        public ArquivoOFX(string caminhoArquivoOFX)
        {
            if (string.IsNullOrEmpty(caminhoArquivoOFX))
            {
                throw new Exception("Informe o caminho do arquivo OFX");
            }

            try
            {
                this._XElement = ToXElement(caminhoArquivoOFX);
                ProcessXElementToLancamentos();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ProcessXElementToLancamentos()
        {
            //vem formato 1000.15
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            provider.NumberGroupSizes = new int[] { 3 };

            _lancamentos = (
                from c in _XElement.Descendants("STMTTRN")
                select new OFXLancamento()
                {
                    Valor = Convert.ToDouble(c.Element("TRNAMT").Value, provider),
                    Data = DateTime.ParseExact(c.Element("DTPOSTED").Value.Substring(0, 8), "yyyyMMdd", null),
                    Descricao = c.Element("MEMO").Value,
                    MD5 = Base64Helper.CalculaMD5Hash(c.ToString())
                }).ToList();
        }

        public string GetBancoId()
        {
            var bankId = _XElement.Element("BANKID");
            return bankId != null ? bankId.Value : "";
        }

        public List<OFXLancamento> GetLancamentos()
        {
            return _lancamentos;
        }

        public List<OFXLancamento> GetLancamentosDebito()
        {
            return _lancamentos.Where(x => x.Valor < 0).ToList();
        }

        public List<OFXLancamento> GetLancamentosCredito()
        {
            return _lancamentos.Where(x => x.Valor >= 0).ToList();
        }

        private XElement ToXElement(string caminhoArquivoOFX)
        {

            //https://www.codeproject.com/Articles/14386/Class-to-transform-ofx-Microsoft-Money-file-into-D
            var tags = from line in File.ReadAllLines(caminhoArquivoOFX)
                       where
                       line.Contains("<BANKID>") ||
                       line.Contains("<STMTTRN>") ||
                       line.Contains("<TRNTYPE>") ||
                       line.Contains("<DTPOSTED>") ||
                       line.Contains("<TRNAMT>") ||
                       line.Contains("<FITID>") ||
                       line.Contains("<CHECKNUM>") ||
                       line.Contains("<MEMO>")
                       select line;

            XElement el = new XElement("root");
            XElement son = null;
            foreach (var l in tags)
            {
                if (l.IndexOf("<BANKID>") != -1)
                {
                    son = new XElement("BANKID");
                    son.Value = GetTagValue(l);
                    el.Add(son);
                    continue;
                }
                else
                {
                    if (l.IndexOf("<STMTTRN>") != -1)
                    {
                        son = new XElement("STMTTRN");
                        el.Add(son);
                        continue;
                    }

                    var tagName = GetTagName(l);
                    var elSon = new XElement(tagName);
                    elSon.Value = GetTagValue(l);
                    son.Add(elSon);
                }
            }
            return el;
        }

        private string GetTagName(string line)
        {
            var pos_init = line.IndexOf("<") + 1;
            var pos_end = line.IndexOf(">");

            return line.Substring(pos_init, pos_end - pos_init);
        }

        private string GetTagValue(string line)
        {
            var pos_init = line.IndexOf(">") + 1;
            var subStr = line.Substring(pos_init).Trim();
            var len = subStr.IndexOf("<");

            var retValue = len > 0 ? line.Substring(pos_init, len).Trim() : subStr;

            if (retValue.IndexOf("[") != -1)
                retValue = retValue.Substring(0, 8);

            return retValue;
        }
    }

}