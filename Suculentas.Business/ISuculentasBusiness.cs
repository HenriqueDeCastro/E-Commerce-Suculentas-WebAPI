using Suculentas.Domain;

namespace Suculentas.Business
{
    public interface ISuculentasBusiness
    {
        // VENDA
        bool VerificaProdutoDisponivel(Produto produto, int quantidade);
        string NomeAmigavelStatusPagSeguro(int status);
        int StatusASerAtualizada(int status, int statusPagseguro);
    }
}