using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Suculentas.PagSeguro.Dtos;

namespace Suculentas.PagSeguro
{
    public class SuculentasPagSeguro: ISuculentasPagSeguro
    {		
        public string Checkout(string emailUsuario, string token, string urlCheckout, List<PagSeguroItemDTO> itens, string reference)
        {
            System.Collections.Specialized.NameValueCollection postData = new System.Collections.Specialized.NameValueCollection();
            postData.Add("email", emailUsuario);
            postData.Add("token", token);
            postData.Add("currency", "BRL");

            for (int i=0; i<itens.Count; i++)
            {
                postData.Add(string.Concat("itemId", i+1), itens[i].itemId);
                postData.Add(string.Concat("itemDescription", i+1), itens[i].itemDescription);
                postData.Add(string.Concat("itemAmount", i+1), itens[i].itemAmount);
                postData.Add(string.Concat("itemQuantity", i+1), itens[i].itemQuantity);
                postData.Add(string.Concat("itemWeight", i+1) , itens[i].itemWeight);
            }

            postData.Add("reference", reference);
            postData.Add("redirectURL", "https://suculentasdaro.com.br/vendas/concluida/" + reference);
            postData.Add("notificationURL", "http://www.xn--suculentasdar-1lb.com.br/venda/notificationPagSeguro");
            postData.Add("shippingAddressRequired", "false");
            postData.Add("acceptPaymentMethodGroup", "CREDIT_CARD,BOLETO");

            string xmlString = null;

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                var result = wc.UploadValues(urlCheckout, postData);
                xmlString = Encoding.ASCII.GetString(result);
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            var code = xmlDoc.GetElementsByTagName("code")[0];
            var date = xmlDoc.GetElementsByTagName("date")[0];

            return code.InnerText;
        }

        public ConsultaTransacaoPagSeguroTransactionDTO ConsultaPorCodigoReferencia(string emailUsuario, string token, string urlConsultaTransacao, string codigoReferencia)
        {
            ConsultaTransacaoPagSeguroTransactionDTO consulta = new ConsultaTransacaoPagSeguroTransactionDTO();

            try
            {
                string uri = string.Concat(urlConsultaTransacao, codigoReferencia, "?email=", emailUsuario, "&token=", token);

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                string xmlString = null;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            xmlString = reader.ReadToEnd();
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(xmlString);

                            var transactions = xmlDoc.GetElementsByTagName("transaction")[0];

                            if (transactions != null)
                            {
                                XmlNodeList dados = transactions.ChildNodes;

                                foreach (XmlNode childNode in transactions.ChildNodes)
                                {
                                    if (childNode.Name == "reference")
                                    {
                                        consulta.Reference = childNode.InnerText;
                                    }
                                    else if (childNode.Name == "code")
                                    {
                                        consulta.Code = childNode.InnerText;
                                    }
                                    else if (childNode.Name == "type")
                                    {
                                        consulta.type = Convert.ToInt32(childNode.InnerText);
                                    }
                                    else if (childNode.Name == "status")
                                    {
                                        consulta.Status = Convert.ToInt32(childNode.InnerText);
                                    }
                                    else if (childNode.Name == "paymentMethod")
                                    {
                                        foreach (XmlNode nodePaymentMethod in childNode.ChildNodes)
                                        {
                                            if (nodePaymentMethod.Name == "type")
                                            {
                                                consulta.PaymentMethodType = Convert.ToInt32(childNode.InnerText);
                                            }
                                        }
                                    }
                                }
                            }

                            reader.Close();
                            dataStream.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return consulta;
        }

        /// <summary>
        /// Cancela transação com status de "Aguardando Pagamento" ou "Em Análise".
        /// </summary>
        /// <param name="emailUsuario">E-mail usuário pagseguro.</param>
        /// <param name="token">Token.</param>
        /// <param name="urlCancelamento">URL Cancelamento.</param>
        /// <param name="transactionCode">Código da transação.</param>
        /// <returns>Bool. Caso true, transação foi cancelada. Caso false, transação não foi cancelada.</returns>
        public bool CancelarTransacao(string emailUsuario, string token, string urlCancelamento, string transactionCode)
        {
            //Monta url completa para solicitação.
            string urlCompleta = string.Concat(urlCancelamento);

            //String que receberá o XML de retorno.
            string xmlString = null;

            //Webclient faz o post para o servidor de pagseguro.
            using (WebClient wc = new WebClient())
            {
                //Informa header sobre URL.
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                //PostData.
                System.Collections.Specialized.NameValueCollection postData = new System.Collections.Specialized.NameValueCollection();
                postData.Add("email", emailUsuario);
                postData.Add("token", token);
                postData.Add("transactionCode", transactionCode);

                //Faz o POST e retorna o XML contendo resposta do servidor do pagseguro.
                var result = wc.UploadValues(urlCancelamento, postData);

                //Obtém string do XML.
                xmlString = Encoding.ASCII.GetString(result);
            }

            //Cria documento XML.
            XmlDocument xmlDoc = new XmlDocument();

            //Carrega documento XML por string.
            xmlDoc.LoadXml(xmlString);

            //Obtém código de transação (Checkout).
            //Caso ocorra tudo ok, a API do PagSeguro retornará uma tag "result" com o conteúdo "OK"
            //Caso o cancelamento não ocorra, será retornado as tags errors -> error e dentro da error, as tags code e message.
            var xmlResult = xmlDoc.GetElementsByTagName("result");

            //Retorno.
            bool retorno;

            //Verifica se tem a tag resultado.
            if (xmlResult.Count > 0)
            {
                retorno = xmlResult[0].InnerText == "OK";
            }
            else
            {
                retorno = false;
            }

            //Retorno do método.
            return retorno;
        }
    }
}