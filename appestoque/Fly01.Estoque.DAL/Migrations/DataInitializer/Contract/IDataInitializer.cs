namespace Fly01.Estoque.DAL.Migrations.DataInitializer.Contract
{
    public interface IDataInitializer
    {
        void Initialize(AppDataContext context);
    }
}
