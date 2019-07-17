using System;
using System.Collections.Generic;
using System.Text;
using Fly01.Financeiro.ViewModel;
using Fly01.Core;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Financeiro.Models.Utils
{
    public static class EmailBodyHelper
    {
        public static string GetEmailReportContasPagar(string userName, List<ContaPagarVM> accountsPayable)
        {
            string subject = "Bemacash Financeiro - Resumo Financeiro do dia";

            StringBuilder body = new StringBuilder();

            body.Append("<!doctype html>");
            body.Append("<html>");
            body.Append("<head>");
            body.Append("<meta charset=\"UTF-8\">");
            body.Append("<title>Resumo Financeiro do Dia</title>");
            body.Append("</head>");
            body.Append("<body>");
            body.Append("<table width=\"625px\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-family:Tahoma, Verdana, Segoe, sans-serif; font-size:12px;\">");
            body.Append("<tbody>");
            body.Append("<tr>");
            body.Append("<td bgcolor=\"#ff700f\">&nbsp;</td>");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td>");
            body.Append("<table>");
            body.Append("<tr>");
            body.Append("<td width=\"380px\">");
            body.Append("<img style=\"display:block; padding:20px; border:0;\" src=\"https://totvss1saudehd.blob.core.windows.net/img-mkt/financeiro-email-report-contas-pagar/logo_fly01.gif\" alt=\"Fly01\"></td>");
            body.Append("<td bgcolor=\"#ffffff\"><span href=\"#\" style=\"color:#ff700f; text-decoration:none; font-weight: normal; text-align:right; font-size:18px;\">Resumo Financeiro do Dia</span></td>");
            body.Append("</tr>");
            body.Append("</table>");
            body.Append("</td>");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td><img src=\"https://totvss1saudehd.blob.core.windows.net/img-mkt/financeiro-email-report-contas-pagar/sombra.gif\" alt=\"sombra\"></td>");
            body.Append("</tr>");
            body.Append("<tr style=\"color:#ff700f; text-decoration:none; font-family:tahoma, arial; font-size:12px;\"> ");
            body.Append("<td style=\"padding-top:20px; padding-left:20px;\">");
            body.AppendFormat("<p> Olá, {0} </p>", userName);
            body.AppendFormat("<p style=\"color:#ff700f; ptext-decoration:none; font-family:tahoma, arial; font-size:12px;\">Esse é o resumo financeiro de sua empresa do dia {0}.", DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"));
            body.Append("</p>");
            body.Append("<p style=\"font-size:16px; padding-top:20px; padding-bottom:px; color:#8a8886; text-decoration:none;\">CONTAS A PAGAR ");
            body.Append("<strong style=\"color:#456bd5; font-size:12px; font-weight:normal;\">");
            body.Append("<a href=\"http://financeiro.bemacash.com.br/ContaPagar\" style=\"text-decoration:none;\">Ver todos</a> </strong></p></td>");
            body.Append("<tr>");
            body.Append("<td>");
            body.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"color:#8a8886; text-decoration:none; font-family:tahoma, arial; font-size:16px; align:left; font-weight: normal;\">");
            body.Append("<tbody>");
            body.Append("<tr>");
            body.Append("<th bgcolor=\"#e9e9e9\" width=\"312,5\" scope=\"col\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" style=\"color:#8a8886; font-size:12px; text-decoration:none; font-weight: normal; font-family:tahoma, arial; font-size:11px; padding:10px; \">Descrição</th>");
            body.Append("<th bgcolor=\"#e9e9e9\" width=\"312,5\" scope=\"col\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" style=\"color:#8a8886; font-size:12px; text-decoration:none; font-weight: normal; font-family:tahoma, arial; font-size:11px; align:left; padding:10px;\">Valor</th>");
            body.Append("</tr>");

            foreach (var item in accountsPayable)
            {
                body.Append("<tr>");
                body.AppendFormat("<td style=\"font-size:11px; padding-left:10px; padding-top:5px; padding-bottom:5px;;\">{0}</td>", String.IsNullOrWhiteSpace(item.Descricao) ? item.Categoria.Descricao : item.Descricao );
                body.AppendFormat("<td style=\"font-size:11px; padding-left:10px; padding-top:5px; padding-bottom:5px;;\">{0}<br></td>", item.ValorPrevisto.ToString("C", AppDefaults.CultureInfoDefault));
                body.Append("</tr>");                
            }

            body.Append("</tbody>");
            body.Append("</table>");
            body.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"color:#8a8886; text-decoration:none; font-family:tahoma, arial; font-size:16px; align:left; font-weight: normal;\">");
            body.Append("<tbody>");
            body.Append("<tr> ");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td><img src=\"https://totvss1saudehd.blob.core.windows.net/img-mkt/financeiro-email-report-contas-pagar/imagem_rodape_01.gif\" alt=\"imagem_rodape_01\" style=\"display:block; border:0; padding:50px 0 0 0;\"></td>");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td>");
            body.Append("<table width=\"0\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            body.Append("<tr>");
            body.Append("<td width=\"208\"><img src=\"https://totvss1saudehd.blob.core.windows.net/img-mkt/financeiro-email-report-contas-pagar/imagem_rodape_02.gif\" alt=\"imagem_rodape_02\" style=\"display:block; border:0;\"></td>");
            body.Append("<td width=\"209\" align=\"center\" bgcolor=\"#ff700f\" style=\"border-radius:3px; border:1px solid #d1d1d1;\"><a href=\"http://financeiro.bemacash.com.br/Dashboard\" style=\"color:white; text-decoration:none; font-weight: normal; font-size:12px;\">Acessar meu Dashboard</a></td>");
            body.Append("<td width=\"208\"></td>");
            body.Append("</tr>");
            body.Append("</table>");
            body.Append("</td>");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td><img src=\"https://totvss1saudehd.blob.core.windows.net/img-mkt/financeiro-email-report-contas-pagar/imagem_rodape_03.gif\" alt=\"imagem_rodape_03\" style=\"display:block; border:0;\"></td>");
            body.Append("</tr>");
            body.Append("<tr>");
            body.Append("<td>");
            body.Append("<table cellpadding=\"0\" cellspacing=\"0\">");
            body.Append("<tr>");
            body.Append("<td width=\"357\" bgcolor=\"#c12f0c\"><img style=\"display:block; border:0;\" src=\"https://totvss1saudehd.blob.core.windows.net/img-mkt/financeiro-email-report-contas-pagar/imagem_rodape_04.gif\" alt=\"imagem_rodape_04\"></td>");
            body.Append("<td cellpadding=\"0\" cellspacing=\"0\"><a href=\"https://www.facebook.com/Fly01oficial/\"><img src=\"https://totvss1saudehd.blob.core.windows.net/img-mkt/financeiro-email-report-contas-pagar/logo_facebook.gif\" alt=\"Facebook\" style=\"display:block; border:0;\"></a></td>");
            body.Append("<td cellpadding=\"0\" cellspacing=\"0\" width=\"30\" height=\"0\" bgcolor=\"#c12f0c\"></td>");
            body.Append("<td cellpadding=\"0\" cellspacing=\"0\"><a href=\"https://www.youtube.com/channel/UCOc0hB1_mExx763A53r_HQw\"><img src=\"https://totvss1saudehd.blob.core.windows.net/img-mkt/financeiro-email-report-contas-pagar/logo_youtube.gif\" alt=\"Youtube\" style=\"display:block; border:0;\"></a></td>");
            body.Append("<td cellpadding=\"0\" cellspacing=\"0\" width=\"30\" height=\"0\" bgcolor=\"#c12f0c\"></td>");
            body.Append("<td cellpadding=\"0\" cellspacing=\"0\"><a href=\"https://twitter.com/totvs\"><img src=\"https://totvss1saudehd.blob.core.windows.net/img-mkt/financeiro-email-report-contas-pagar/logo_twitter.gif\" alt=\"Twitter\" style=\"display:block; border:0;\"></a></td>");
            body.Append("<td cellpadding=\"0\" cellspacing=\"0\" width=\"116\" height=\"0\" bgcolor=\"#c12f0c\"></td>");
            body.Append("</tr>");
            body.Append("</table>");
            body.Append("</td>");
            body.Append("<tr>");
            body.Append("<td bgcolor=\"#ff700f\">");
            body.Append("<table cellpadding=\"0\" cellspacing=\"0\">");
            body.Append("<tr>");
            body.Append("<td width=\"357\"><img style=\"display:block; border:0;\" src=\"https://totvss1saudehd.blob.core.windows.net/img-mkt/financeiro-email-report-contas-pagar/imagem_rodape_05.gif\" alt=\"imagem_rodape_04\"></td>");
            body.Append("<td><p  style=\"color:#ffffff; text-decoration:none; font-weight: normal; font-size:14px; margin-bottom:0px;\">Fale Conosco</p>");
            body.Append("<p style=\"color:#ffffff; text-decoration:none; font-weight: bold; font-size:20px; margin-bottom:0px; margin-top:0px\">0800 999 9999</p>");
            body.Append("<p style=\"color:#ffffff; text-decoration:none; font-weight: normal; font-size:14px; margin-top:0px;\">Seg-sex 9h às 19h</p>");
            body.Append("</td>");
            body.Append("</tr>");
            body.Append("</table>");
            body.Append("</td>");
            body.Append("</tr>");
            body.Append("</tr>");
            body.Append("</tbody>");
            body.Append("</table>");
            body.Append("</td>");
            body.Append("</tr>");
            body.Append("</tr>");
            body.Append("</tbody>");
            body.Append("</table>");
            body.Append("</body>");
            body.Append("</html>");

            return string.Concat(subject, AppDefaults.SeparadorSubjectEmailMensageria, body.ToString());
        }
    }
}