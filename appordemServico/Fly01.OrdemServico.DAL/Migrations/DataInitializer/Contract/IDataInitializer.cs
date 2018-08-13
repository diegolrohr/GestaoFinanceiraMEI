using Fly01.OrdemServico.API.Models.DAL;

namespace Fly01.OrdemServico.DAL.Migrations.DataInitializer.Contract
{
    public interface IDataInitializer
    {
        void Initialize(AppDataContext context);
    }
}
