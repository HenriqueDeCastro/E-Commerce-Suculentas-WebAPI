using Suculentas.Domain;
using Microsoft.Extensions.Configuration;
using System;

namespace Suculentas.Business
{
    public class SuculentasBusiness : ISuculentasBusiness
    {
        private readonly IConfiguration _config;

        public SuculentasBusiness(IConfiguration config)
        {
            _config = config;
        }

        // PAGSEGURO
        public string FriendlyNameStatusPagSeguro(int status)
        {
            if (status == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:NaoExisteTransacao").Value))
            {
                return "Nenhuma Transa��o Encontrada";
            }
            else if (status == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:AguardandoPagamento").Value))
            {
                return "Aguardando Pagamento";
            }
            else if (status == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:EmAnalise").Value))
            {
                return "Em An�lise";
            }
            else if (status == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:Paga").Value))
            {
                return "Pago";
            }
            else if (status == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:Disponivel").Value))
            {
                return "Dispon�vel";
            }
            else if (status == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:EmDisputa").Value))
            {
                return "Em Disputa";
            }
            else if (status == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:Devolvida").Value))
            {
                return "Devolvida";
            }
            else if (status == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:Cancelada").Value))
            {
                return "Cancelada";
            }
            else if (status == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:Debitado").Value))
            {
                return "Debitado (Devolvido)";
            }
            else if (status == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:RetencaoTemporaria").Value))
            {
                return "Reten��o Temp.";
            }
            else
            {
                return "Falha ao resolver status pagseguro.";
            }
        }

        // VENDA
        public bool CheckProductAvailable(Product product, int quantity) {
            
            if (product.ProductTypeId == Int16.Parse(_config.GetSection("AppSettings:ProductType:Encomenda").Value)) {
                if (product.Active == true && product.MaximumQuantity > 0 && product.MaximumQuantity >= quantity) {
                    return true;
                } else {
                    return false;
                }
            } else {
                if (product.Active == true && product.Inventory > 0 && product.Inventory >= quantity) {
                    return true;
                } else {
                    return false;
                }
            }
        }

        public int DefineStatusUpdated(int status, int statusPagseguro)
        {
            // Aguardando Pagamento
            if(statusPagseguro == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:AguardandoPagamento").Value) 
                    || statusPagseguro == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:EmAnalise").Value))
            {
                return Int16.Parse(_config.GetSection("AppSettings:StatusSales:AguardandoPagamento").Value);
            }
            // Paga ou Disponivel
            else if(statusPagseguro == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:Paga").Value) 
                    || (statusPagseguro == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:Disponivel").Value) 
                        && status == Int16.Parse(_config.GetSection("AppSettings:StatusSales:AguardandoPagamento").Value)))
            {
                return Int16.Parse(_config.GetSection("AppSettings:StatusSales:AguardandoEnvio").Value);
            }
            // Em Disputa
            else if(statusPagseguro == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:EmDisputa").Value)
                    || statusPagseguro == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:RetencaoTemporaria").Value))
            {
                return Int16.Parse(_config.GetSection("AppSettings:StatusSales:EmDisputa").Value);
            }
            //Devolvida ou Cancelada
            else if(statusPagseguro == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:Devolvida").Value) 
                    || statusPagseguro == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:Cancelada").Value)
                    || statusPagseguro == Int16.Parse(_config.GetSection("AppSettings:StatusPagSeguro:Debitado").Value))
            {
                return Int16.Parse(_config.GetSection("AppSettings:StatusSales:Cancelado").Value);
            }
            else
            {
                return status;
            }
        }
    }
}