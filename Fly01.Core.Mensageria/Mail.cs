﻿using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Fly01.Core.Mensageria
{
    public class Mail
    {
        public static void Send(string nomeRemetente, string emailDestinatario, string tituloEmail, string corpoEmail, Stream anexo, string tipoAnexo = ".pdf")
        {
            var emailsDestintario = emailDestinatario.Split(';');
            if (emailsDestintario.Length > 1)
            {
                foreach (var item in emailsDestintario)
                {
                    SendEmail(item, nomeRemetente, tituloEmail, corpoEmail, anexo, tipoAnexo);
                }
            }
            else
                SendEmail(emailDestinatario, nomeRemetente, tituloEmail, corpoEmail, anexo, tipoAnexo);
        }

        public static void SendNoAttachment(string nomeRemetente, string emailDestinatario, string tituloEmail, string corpoEmail)
        {
            var emailsDestintario = emailDestinatario.Split(';');
            if (emailsDestintario.Length > 1)
            {
                foreach (var item in emailsDestintario)
                {
                    SendEmailNoAttachment(emailDestinatario, nomeRemetente, tituloEmail, corpoEmail);
                }
            }
            else
                SendEmailNoAttachment(emailDestinatario, nomeRemetente, tituloEmail, corpoEmail);
        }

        private static void SendEmailNoAttachment(string emailDestinatario, string nomeRemetente, string tituloEmail, string corpoEmail)
        {

            var from = new MailAddress(ConfigurationManager.AppSettings["EmailRemetente"], nomeRemetente);
            var to = new MailAddress(emailDestinatario);
            var message = new MailMessage(from, to)
            {
                Subject = tituloEmail,
                Body = corpoEmail,
                IsBodyHtml = true
            };

            try
            {
                SmtpClient client = ConfigSmtpClient();

                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void SendEmail(string emailDestinatario, string nomeRemetente, string tituloEmail, string corpoEmail, Stream anexo, string tipoAnexo)
        {

            var from = new MailAddress(ConfigurationManager.AppSettings["EmailRemetente"], nomeRemetente);
            var to = new MailAddress(emailDestinatario);
            var message = new MailMessage(from, to)
            {
                Subject = tituloEmail,
                Body = corpoEmail,
                IsBodyHtml = true
            };

            if (anexo.Length > 0)
            {
                if (tipoAnexo != ".pdf")
                    message.Attachments.Add(new Attachment(anexo, $"{tituloEmail}" + tipoAnexo));
                else
                    message.Attachments.Add(new Attachment(anexo, $"{tituloEmail}" + tipoAnexo, System.Net.Mime.MediaTypeNames.Application.Pdf));
            }


            try
            {
                SmtpClient client = ConfigSmtpClient();

                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendMultipleAttach(string emailDestinatario, string nomeRemetente, string tituloEmail, string corpoEmail, Stream[] anexos, string[] tiposAnexos)
        {

            var from = new MailAddress(ConfigurationManager.AppSettings["EmailRemetente"], nomeRemetente);
            var to = new MailAddress(emailDestinatario);
            var message = new MailMessage(from, to)
            {
                Subject = tituloEmail,
                Body = corpoEmail,
                IsBodyHtml = true
            };

            if (anexos.Length > 0)
            {
                int i = 0;
                foreach (Stream anexo in anexos)
                {
                    if (tiposAnexos[i] == "application/pdf")
                        message.Attachments.Add(new Attachment(anexo, $"{tituloEmail}.pdf", System.Net.Mime.MediaTypeNames.Application.Pdf));
                    else
                        message.Attachments.Add(new Attachment(anexo, $"{tituloEmail}" + tiposAnexos[i]));
                    i++;
                }
            }


            try
            {
                SmtpClient client = ConfigSmtpClient();

                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static SmtpClient ConfigSmtpClient()
        {
            return new SmtpClient
            {
                Host = ConfigurationManager.AppSettings["EmailHost"],
                Port = int.Parse(ConfigurationManager.AppSettings["EmailPort"]),
                EnableSsl = bool.Parse(ConfigurationManager.AppSettings["EmailEnableSsl"]),
                UseDefaultCredentials = bool.Parse(ConfigurationManager.AppSettings["EmailUseDefaultCredentials"]),
                Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailCredentialsUserName"], ConfigurationManager.AppSettings["EmailCredentialsPassword"])
            };
        }

        public static string FormataMensagem(string htmlContent, string tituloEmail, string mensagemPrincipal)
        {
            return FormatTextMail(htmlContent, tituloEmail, mensagemPrincipal, "", "");
        }

        public static string FormataMensagem(string htmlContent, string tituloEmail, string mensagemPrincipal, string emailEmpresa)
        {
            return FormatTextMail(htmlContent, tituloEmail, mensagemPrincipal, "", emailEmpresa);
        }
        public static string FormataMensagem(string htmlContent, string tituloEmail, string mensagemPrincipal, string mensagemComplemento, string emailEmpresa, string simplesNacional, string impostoRenda, string csll, string cofins, string pisPasep, string ipi, string iss, string fcp, string inss)
        {
            return FormatTextMailParametroTributario(htmlContent, tituloEmail, mensagemPrincipal, mensagemComplemento, emailEmpresa, simplesNacional, impostoRenda, csll, cofins, pisPasep, ipi, iss, fcp, inss);
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

        public static string FormatTextMailParametroTributario(string htmlContent, string tituloEmail, string mensagemPrincipal, string mensagemComplemento, string emailEmpresa, string simplesNacional,
             string impostoRenda, string csll, string cofins, string pisPasep, string ipi, string iss, string fcp, string inss)
        {
            return new StringBuilder(htmlContent
                .Replace("{TITULO_EMAIL}", HttpUtility.HtmlEncode(tituloEmail))
                .Replace("{MENSAGEM_1}", HttpUtility.HtmlEncode(mensagemPrincipal))
                .Replace("{MENSAGEM_2}", HttpUtility.HtmlEncode(mensagemComplemento))
                .Replace("{EMAIL_EMPRESA}", HttpUtility.HtmlEncode(emailEmpresa))
                .Replace("{ICMS}", HttpUtility.HtmlEncode(simplesNacional))
                .Replace("{FCP}", HttpUtility.HtmlEncode(fcp))
                .Replace("{PIS/PASEP}", HttpUtility.HtmlEncode(pisPasep))
                .Replace("{COFINS}", HttpUtility.HtmlEncode(cofins))
                .Replace("{ISS}", HttpUtility.HtmlEncode(iss))
                .Replace("{CSLL}", HttpUtility.HtmlEncode(csll))
                .Replace("{INSS}", HttpUtility.HtmlEncode(inss))
                .Replace("{IR}", HttpUtility.HtmlEncode(impostoRenda))
            ).ToString();
        }
    }
}
