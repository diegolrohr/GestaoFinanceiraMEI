using Fly01.Core.Helpers;
using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Commons
{
    public enum Operation
    {
        Create,
        Edit,
        Delete
    }

    public static class JsonResponseStatus
    {
        /// <summary>
        /// Converte Mensagem de Erro para Html
        /// </summary>
        /// <param name="errorMsg">Mensagem de Erro da API</param>
        /// <returns>Retorna Mensagem de erro em HTML com quebras</returns>
        private static string MsgToHtml(string errorMsg)
        {
            return Regex.Replace(errorMsg, @"\r\n?|\n", "<br />");
        }

        public static JsonResult Get(ErrorInfo errorInfo, Operation operation, Guid id = default(Guid))
        {
            string msgSuccess = String.Empty;

            switch (operation)
            {
                case Operation.Create:
                    msgSuccess = AppDefaults.CreateSuccessMessage;
                    break;
                case Operation.Edit:
                    msgSuccess = AppDefaults.EditSuccessMessage;
                    break;
                case Operation.Delete:
                    msgSuccess = AppDefaults.DeleteSuccessMessage;
                    break;
            }

            var response = new JsonResult();
            if (!errorInfo.HasError)
            {
                if (id == default(Guid))
                    response.Data = new {success = true, message = msgSuccess};
                else
                    response.Data = new {success = true, message = msgSuccess, id = id.ToString()};
            }
            else
                response.Data = new {success = false, message = MsgToHtml(errorInfo.Message)};

            return response;
        }

        public static JsonResult GetSuccess(string msgCustom)
        {
            var response = new JsonResult();
            response.Data = new { success = true, message = MsgToHtml(msgCustom) };

            return response;
        }

        public static JsonResult GetFailure(string errorMsg)
        {
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    success = false,
                    message = MsgToHtml(errorMsg)
                }
            };
        }

        public static JsonResult GetJson(object jsonObj)
        {
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = jsonObj
            };
        }
    }
}