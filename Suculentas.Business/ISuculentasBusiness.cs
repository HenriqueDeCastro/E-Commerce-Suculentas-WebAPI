using Suculentas.Domain;

namespace Suculentas.Business
{
    public interface ISuculentasBusiness
    {
        bool CheckProductAvailable(Product product, int amount);
        string FriendlyNameStatusPagSeguro(int status);
        int DefineStatusUpdated(int status, int statusPagseguro);
    }
}