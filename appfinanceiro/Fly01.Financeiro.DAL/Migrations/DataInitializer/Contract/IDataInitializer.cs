using Fly01.Financeiro.API.Models.DAL;

namespace Fly01.Financeiro.DAL.Migrations.DataInitializer.Contract
{
    public interface IDataInitializer
    {
        void Initialize(AppDataContext context);
    }
}
