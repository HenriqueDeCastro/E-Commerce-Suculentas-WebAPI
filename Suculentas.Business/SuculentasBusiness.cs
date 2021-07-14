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
        public string NomeAmigavelStatusPagSeguro(int status)
        {
            string retorno;

            if (status == Int16.Parse(_config.GetSection("StatusPagSeguro:NaoExisteTransacao").Value))
            {
                retorno = "Nenhuma Transação Encontrada";
            }
            else if (status == Int16.Parse(_config.GetSection("StatusPagSeguro:AguardandoPagamento").Value))
            {
                retorno = "Aguardando Pagamento";
            }
            else if (status == Int16.Parse(_config.GetSection("StatusPagSeguro:EmAnalise").Value))
            {
                retorno = "Em Análise";
            }
            else if (status == Int16.Parse(_config.GetSection("StatusPagSeguro:Paga").Value))
            {
                retorno = "Pago";
            }
            else if (status == Int16.Parse(_config.GetSection("StatusPagSeguro:Disponivel").Value))
            {
                retorno = "Disponível";
            }
            else if (status == Int16.Parse(_config.GetSection("StatusPagSeguro:EmDisputa").Value))
            {
                retorno = "Em Disputa";
            }
            else if (status == Int16.Parse(_config.GetSection("StatusPagSeguro:Devolvida").Value))
            {
                retorno = "Devolvida";
            }
            else if (status == Int16.Parse(_config.GetSection("StatusPagSeguro:Cancelada").Value))
            {
                retorno = "Cancelada";
            }
            else if (status == Int16.Parse(_config.GetSection("StatusPagSeguro:Debitado").Value))
            {
                retorno = "Debitado (Devolvido)";
            }
            else if (status == Int16.Parse(_config.GetSection("StatusPagSeguro:RetencaoTemporaria").Value))
            {
                retorno = "Retenção Temp.";
            }
            else
            {
                retorno = "Falha ao resolver status pagseguro.";
            }

            return retorno;
        }

        // VENDA
        public bool VerificaProdutoDisponivel(Produto produto, int quantidade) {
            
            if (produto.TipoProdutoId == Int16.Parse(_config.GetSection("TipoProduto:Encomenda").Value)) {
                if (produto.Ativo == true && produto.QuantidadeMaxima > 0 && produto.QuantidadeMaxima >= quantidade) {
                    return true;
                } else {
                    return false;
                }
            } else {
                if (produto.Ativo == true && produto.Estoque > 0 && produto.Estoque >= quantidade) {
                    return true;
                } else {
                    return false;
                }
            }
        }

        public int StatusASerAtualizada(int status, int statusPagseguro)
        {
            // Aguardando Pagamento
            if(statusPagseguro == Int16.Parse(_config.GetSection("StatusPagSeguro:AguardandoPagamento").Value) 
                    || statusPagseguro == Int16.Parse(_config.GetSection("StatusPagSeguro:EmAnalise").Value))
            {
                return Int16.Parse(_config.GetSection("StatusVendas:AguardandoPagamento").Value);
            }
            // Paga ou Disponivel
            else if(statusPagseguro == Int16.Parse(_config.GetSection("StatusPagSeguro:Paga").Value) 
                    || (statusPagseguro == Int16.Parse(_config.GetSection("StatusPagSeguro:Disponivel").Value) 
                        && status == Int16.Parse(_config.GetSection("StatusVendas:AguardandoPagamento").Value)))
            {
                return Int16.Parse(_config.GetSection("StatusVendas:AguardandoEnvio").Value);
            }
            // Em Disputa
            else if(statusPagseguro == Int16.Parse(_config.GetSection("StatusPagSeguro:EmDisputa").Value)
                    || statusPagseguro == Int16.Parse(_config.GetSection("StatusPagSeguro:RetencaoTemporaria").Value))
            {
                return Int16.Parse(_config.GetSection("StatusVendas:EmDisputa").Value);
            }
            //Devolvida ou Cancelada
            else if(statusPagseguro == Int16.Parse(_config.GetSection("StatusPagSeguro:Devolvida").Value) 
                    || statusPagseguro == Int16.Parse(_config.GetSection("StatusPagSeguro:Cancelada").Value)
                    || statusPagseguro == Int16.Parse(_config.GetSection("StatusPagSeguro:Debitado").Value))
            {
                return Int16.Parse(_config.GetSection("StatusVendas:Cancelado").Value);
            }
            else
            {
                return status;
            }
        }
    }
}