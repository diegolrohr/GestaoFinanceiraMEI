
namespace Fly01.Compras.Entities.ViewModel
{
    public class GlobalConfigurationVM
    {
        public GlobalConfigurationItemsVM[] GlobalConfiguration { get; set; }
    }

    public class GlobalConfigurationItemsVM
    {
        public string Key { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
    }
}