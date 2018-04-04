using System.Web.Mvc;

namespace Fly01.Financeiro
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            // Adicionado para garantir que as páginas necessitem de Autorização para executar,
            // sendo necessário o data annotation [AllowAnonymous]
            // para as actions que não necessitam de autorização.
            filters.Add(new AuthorizeAttribute());
        }
    }
}
