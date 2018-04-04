using System;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Fly01.Core.Messages
{
    public class Mail
    {
        public static void Send(string nomeRemetente, string emailDestinatario, string tituloEmail, string corpoEmail, FileStreamResult anexo)
        {
            var from = new MailAddress(ConfigurationManager.AppSettings["EmailRemetente"], nomeRemetente);
            var to = new MailAddress(emailDestinatario);
            var message = new MailMessage(from, to);

            message.Subject = tituloEmail;
            message.Body = corpoEmail;
            message.IsBodyHtml = true;

            if (anexo.FileStream.Length > 0)
                message.Attachments.Add(new Attachment(anexo.FileStream, $"{tituloEmail}.pdf"));

            try
            {
                var client = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["EmailHost"],
                    Port = int.Parse(ConfigurationManager.AppSettings["EmailPort"]),
                    EnableSsl = bool.Parse(ConfigurationManager.AppSettings["EmailEnableSsl"]),
                    UseDefaultCredentials = bool.Parse(ConfigurationManager.AppSettings["EmailUseDefaultCredentials"]),
                    Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailCredentialsUserName"], ConfigurationManager.AppSettings["EmailCredentialsPassword"])
                };

                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string FormataMensagem(string htmlContent, string tituloEmail, string mensagemPrincipal)
        {
            return FormatTextMail(htmlContent, tituloEmail, mensagemPrincipal, "", "");
        }

        public static string FormataMensagem(string htmlContent, string tituloEmail, string mensagemPrincipal, string emailEmpresa)
        {
            return FormatTextMail(htmlContent, tituloEmail, mensagemPrincipal, "", emailEmpresa);
        }

        public static string FormatTextMail(string htmlContent, string tituloEmail, string mensagemPrincipal, string mensagemComplemento, string emailEmpresa)
        {
            return new StringBuilder(htmlContent
                .Replace("{TITULO_EMAIL}", HttpUtility.HtmlEncode(tituloEmail))
                .Replace("{MENSAGEM_1}", HttpUtility.HtmlEncode(mensagemPrincipal))
                .Replace("{MENSAGEM_2}", HttpUtility.HtmlEncode(mensagemComplemento))
                .Replace("{EMAIL_EMPRESA}", HttpUtility.HtmlEncode(emailEmpresa))
            ).ToString();
        }
    }
}
