using Primatech.FiscalDriver.Helpers;
using Primatech.FiscalDriver.Models;
using Primatech.FiscalModels.JSON.Requests;
using Primatech.FiscalModels.XML.Requests;
using Primatech.FiscalModels.XML.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primatech.FiscalDriver.Infrastructure
{
    public class FiscalApiService
    {
        private ExtendedHttpClient Client = new ExtendedHttpClient();

        public static string Token { get; set; }
        public static string BaseUrl { get; set; }

        public FiscalApiService(string baseUrl,string token)
        {
            BaseUrl = baseUrl;
            Token = token;
        }

        public async Task<EFCommandResponse> CreateReceipt(EFiscalReceiptCommand command)
        {
            return await BaseUrl
                    .WithClient(Client)
                    .AppendPathSegment("api/efiscal/fiscalReceipt")
                    .AcceptJson()
                    .WithOAuthBearerToken(Token)
                    .UseUri()
                    .PostJsonAsync<EFCommandResponse>(command);
        }

        public async Task<EFCommandResponse> CreateReceipt(EFIReceipt command)
        {
            return await CreateReceipt(command.ToXMLModel());
        }

        public async Task<EFCommandResponse> CreateDeposit(EFDepositCommand command)
        {
                return await BaseUrl
                   .WithClient(Client)
                   .AppendPathSegment(
                       "/api/efiscal/deposit"
                   )
                   .AcceptJson()
                   .WithOAuthBearerToken(Token)
                   .UseUri()
                   .PostJsonAsync(command)
                   .ReceiveJson<EFTCRCommandResponse>();
        }

        public async Task<EFCommandResponse> CreateDeposit(EFIDeposit command)
        {
            return await CreateDeposit(command.ToXMLModel());
        }


        //undocumented API
        public async Task<EFTCRCommandResponse> CreateTCR(string baseUrl, EFTCRCommand model)
        {
            return await BaseUrl
                .WithClient(Client)
                .AppendPathSegment(
                    "/api/efiscal/TCR"
                )
                .AcceptJson()
                .WithOAuthBearerToken(Token)
                .UseUri()
                .PostJsonAsync<EFTCRCommandResponse>(model);
        }
    }
}
