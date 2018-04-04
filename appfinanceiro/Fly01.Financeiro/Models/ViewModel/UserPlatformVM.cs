namespace Fly01.Financeiro.Models.ViewModel
{
    public class UserPlatformVM
    {
        public int Count { get; set; }
        public UserPlatformItemsVM[] Items { get; set; }
    }

    public class UserPlatformItemsVM
    {
        public string Description { get; set; }
        public string Code { get; set; }
    }
}