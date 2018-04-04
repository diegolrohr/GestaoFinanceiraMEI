namespace Fly01.EmissaoNFE.DAL.Migrations.DataInitializer.Contract
{
    public interface IDataInitializer
    {
        void Initialize(AppDataContext context);
    }
}
