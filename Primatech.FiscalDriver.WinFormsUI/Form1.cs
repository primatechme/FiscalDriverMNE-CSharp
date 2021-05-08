using Newtonsoft.Json;
using Primatech.FiscalDriver.Helpers;
using Primatech.FiscalDriver.Infrastructure;
using Primatech.FiscalDriver.Infrastructure.Builders;
using Primatech.FiscalModels.JSON.Requests;
using Primatech.FiscalModels.XML.Requests;
using Primatech.FiscalModels.XML.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Primatech.FiscalDriver.WinFormsUI
{
    public partial class Form1 : Form
    {
        private static string BASE_URL = "https://***";
        private static string TCR_CODE = "*****";
        private static string TOKEN = "*****";
        private static string USER_CODE = "*****";
        private static string USER_NAME = "*****";
        private static string DEPOSIT_PATH = "*.json";
        private static string RECEIPT_PATH = "*.json";

        private static FiscalApiService _fiscalApiService;
        private static string _receiptUrl;


        EFIClient _seller;
        EFIClient _buyer;
       
        

        public Form1()
        {
            InitializeComponent();
            LoadConfigValues();
            SetVariables();
            _fiscalApiService = new FiscalApiService(BASE_URL, TOKEN);
        }

        private void SetVariables()
        {
            _seller = new EFIClient
            {
                Name = "Primatech d.o.o.",
                VATNumber = "02863782",
                Address = "Podgorica"
            };

            _buyer = new EFIClient
            {
                Name = "Other Company d.o.o.",
                VATNumber = "12345678",
                Address = "Cetinje"
            };

        }

        private void LoadConfigValues()
        {
            BASE_URL = ConfigurationManager.AppSettings["BASE_URL"];
            TCR_CODE = ConfigurationManager.AppSettings["TCR_CODE"];
            TOKEN = ConfigurationManager.AppSettings["TOKEN"];
            USER_CODE = ConfigurationManager.AppSettings["USER_CODE"];
            USER_NAME = ConfigurationManager.AppSettings["USER_NAME"];
            DEPOSIT_PATH = ConfigurationManager.AppSettings["DEPOSIT_PATH"];
            RECEIPT_PATH = ConfigurationManager.AppSettings["RECEIPT_PATH"];

            txtTcrCode.Text = TCR_CODE;
            txtUserCode.Text = USER_CODE;
            txtUserName.Text = USER_NAME;
            txtBaseUrl.Text = BASE_URL;
            txtToken.Text = TOKEN;
        }

        private void EnableControls(bool enabled)
        {
            btnInitialDeposit.Enabled = enabled;
            btnWithdrawDeposit.Enabled = enabled;
            btnCashReceipt.Enabled = enabled;
            btnNonCashReceipt.Enabled = enabled;
            btnReceiptWithError.Enabled = enabled;
            btnSendDepositFromRequestField.Enabled = enabled;
            btnSendReceiptFromRequestField.Enabled = enabled;
            btnLoadRequest.Enabled = enabled;
            btnSaveRequest.Enabled = enabled;
            btnSepPortal.Enabled = enabled;
            btnSaveResponse.Enabled = enabled;
            txtResponse.Enabled = enabled;
        }


        string PrettyPrint(object data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

       

        private void DisplayRequest(object request)
        {
            txtRequest.Text = PrettyPrint(request);
            txtResponse.Text = "";
            _receiptUrl = null;
            btnSepPortal.Enabled = false;
        }

        private void DisplayResponse(object response)
        {
            txtResponse.Text = PrettyPrint(response);
            if(response is EFCommandResponse)
            {
                var resp=response as EFCommandResponse;
                if (resp.Url != null && !String.IsNullOrEmpty(resp.Url.UrlRaw))
                {
                    _receiptUrl = resp.Url.UrlRaw;
                    btnSepPortal.Enabled = true;
                }
            }
        }

        private async void btnInitialDeposit_Click(object sender, EventArgs e)
        {
            var deposit = DepositBuilder.Build(TCR_CODE, 25m)
                        .SetTime(DateTime.Now)
                        .SetAmount(25m)
                        .SetUser(USER_NAME, USER_CODE);

            await SendDeposit(deposit);
        }

        private async void btnWithdrawDeposit_Click(object sender, EventArgs e)
        {
            var deposit = DepositBuilder.Build(TCR_CODE, -20m)
                        .SetTime(DateTime.Now)
                         .SetUser(USER_NAME, USER_CODE);

            await SendDeposit(deposit);
            
        }

        private async Task SendDeposit(EFIDeposit deposit)
        {
            await SendDeposit(deposit, true);
        }

        private async Task SendDeposit(EFIDeposit deposit,bool showRequest)
        {
            EnableControls(false);

            if (showRequest)
                DisplayRequest(deposit);

            try
            {
                var result = await _fiscalApiService.CreateDeposit(deposit);
                SaveRequestAndResponseIfRequired(@"Logs\\Deposit\", deposit.ToXMLModel(),result, chkSaveToXml.Checked);

                DisplayResponse(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error");// + "\nStackTrace: " + ex.StackTrace);
            }
            EnableControls(true);
        }

        private void SaveRequestAndResponseIfRequired(string path, object request,object response,bool saveData)
        {
            if (!saveData)
                return;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //write response as xml
            if(request is EFiscalReceiptCommand)
            {
                (request as EFiscalReceiptCommand).Type = "XML";
            }
            var guid = Guid.NewGuid();
            File.WriteAllText(Path.Combine(path, guid + "_Request.xml"), request.XmlSerializeToString());
            File.WriteAllText(Path.Combine(path, guid + "_Response.xml"), response.XmlSerializeToString());
        }

        private async void btnCashReceipt_Click(object sender, EventArgs e)
        {
            var receipt = ReceiptBuilder.Build(Guid.NewGuid(), 1)
                  .SetTCRCode(TCR_CODE)
                  .SetDates(DateTime.Now, null)
                  .SetIsCash(true)
                  .SetJsonResponse()
                  .SetUser(USER_NAME, USER_CODE)
                  .SetSeller("Primatech d.o.o.", "02863782", "Podgorica")
                  .AddSaleItem("12", "Coca Cola 0.25l", 2, 2.20m, 21m)
                  .AddSaleItem("22", "Fanta 0.5l", 2, 2.20m, 21m, 10)
                  .AddSaleItem("19", "Sir Gauda", 2, 2.20m, 7m, 0)
                  .CalculateTotalAmount("BANKNOTE");

            await SendReceipt(receipt);
        }

        private async void btnNonCashReceipt_Click(object sender, EventArgs e)
        {
            var receipt = ReceiptBuilder.Build(Guid.NewGuid(), 2)
                  .SetTCRCode(TCR_CODE)
                  .SetDates(DateTime.Now, null)
                  .SetIsCash(false)
                  .SetJsonResponse()
                  .SetUser(USER_NAME, USER_CODE)
                  .SetSeller("Primatech d.o.o.", "02863782", "Podgorica")
                  .SetBuyer("Buyer Company Name", "07654321", "Some Buyer Address")
                  .AddSaleItem("12", "Coca Cola 0.25l", 120, 1m, 21m)
                  .AddPayment("ACCOUNT", 100m)
                  .AddPayment("BUSINESSCARD", 20m);

            await SendReceipt(receipt);
        }

        private async void btnReceiptWithError_Click(object sender, EventArgs e)
        {
            var receipt = ReceiptBuilder.Build(Guid.NewGuid(), 3)
                  .SetTCRCode(TCR_CODE)
                  .SetDates(DateTime.Now, null)
                  .SetIsCash(false)
                  .SetJsonResponse()
                  .SetUser(USER_NAME, USER_CODE)
                  .SetSeller("Primatech d.o.o.", "02863781", "Podgorica")
                  .SetBuyer("Buyer Company Name", "07654321", "Some Buyer Address")
                  .AddSaleItem("12", "Coca Cola 0.25l", 2, 2.20m, 21m)
                  .CalculateTotalAmount("ACCOUNT");

            await SendReceipt(receipt);
        }

        private async Task<EFCommandResponse> SendReceipt(EFIReceipt receipt)
        {
            return await SendReceipt(receipt, true);
        }

        private async Task<EFCommandResponse> SendReceipt(EFIReceipt receipt,bool showRequest)
        {
            EFCommandResponse result = null;

            EnableControls(false);
            if (showRequest)
                DisplayRequest(receipt);

            try
            {
                //var model = receipt.ToXMLModel();
                //var json=model.ToJson();
                result = await _fiscalApiService.CreateReceipt(receipt);
                SaveRequestAndResponseIfRequired(@"Logs\Receipts\", receipt.ToXMLModel(), result, chkSaveToXml.Checked);
                result.Url.UrlContent = result.Url.ToModel();
                DisplayResponse(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            EnableControls(true);
            return result;
        }

        private void btnSepPortal_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(_receiptUrl))
                Process.Start(_receiptUrl);
        }

        

        private async void btnSendDepositFromRequestField_Click(object sender, EventArgs e)
        {
            try
            {
                EFIDeposit deposit = JsonConvert.DeserializeObject<EFIDeposit>(txtRequest.Text);
                await SendDeposit(deposit, false);
            }
            catch(Exception ex)
            {
                txtResponse.Text = "";
                MessageBox.Show(ex.Message, "Error");
            }
            
        }

        private async void btnSendReceiptFromRequestField_Click(object sender, EventArgs e)
        {
            try
            {
                EFIReceipt receipt = JsonConvert.DeserializeObject<EFIReceipt>(txtRequest.Text);
                await SendReceipt(receipt, false);
            }
            catch (Exception ex)
            {
                txtResponse.Text = "";
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void btnSaveRequest_Click(object sender, EventArgs e)
        {
            SaveContentToFile(txtRequest.Text);
        }

        private void btnSaveResponse_Click(object sender, EventArgs e)
        {
            SaveContentToFile(txtResponse.Text);
        }

        private void SaveContentToFile(string text)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = "JSON files (*.json)|*.json",
                Title = "Select JSON File"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != "")
            {
                System.IO.File.WriteAllText(saveFileDialog.FileName, text);
            }
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select JSON File",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "json",
                Filter = "JSON files (*.json)|*.json",
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtRequest.Text = System.IO.File.ReadAllText(openFileDialog.FileName);
            }
        }

        private async void btnTestCorrectiveInvoice_Click(object sender, EventArgs e)
        {
            //1. Create receipt
            var receipt = ReceiptBuilder.Build(Guid.NewGuid(), 1)
                 .SetTCRCode(TCR_CODE)
                 .SetDates(DateTime.Now, null)
                 .SetIsCash(true)
                 .SetUser(USER_NAME, USER_CODE)
                 .SetSeller(_seller.Name, _seller.VATNumber, _seller.Address)
                 .AddSaleItem("1", "Coca Cola 0.5", 2, 2.5m, 21m)
                 .CalculateTotalAmount(EFIPaymentTypeEnum.CARD);

            var result = await SendReceipt(receipt);
            MessageBox.Show("Created recipt with IKOF " + result.UIDRequest);
            var receiptReference = new
            {
                IKOFReference = result.UIDRequest,
                IssuedAt = receipt.ReceiptTime
            };

            //2. Delate receipt
            var correctiveReceipt = ReceiptBuilder.Build(Guid.NewGuid(), 2)
                .SetTCRCode(TCR_CODE)
                .SetDates(DateTime.Now, null)
                .SetIsCash(true)
                .SetUser(USER_NAME, USER_CODE)
                .SetSeller(_seller.Name, _seller.VATNumber, _seller.Address)
                .SetCorrectiveInvoice()
                .AddIKOFReference(receiptReference.IKOFReference, receiptReference.IssuedAt)
                .AddSaleItem("1", "Coca Cola 0.5", -2, 2.5m, 21m)
                .CalculateTotalAmount(EFIPaymentTypeEnum.CARD);

            result = await SendReceipt(correctiveReceipt);
        }

        private async void btnTestAdvanceInvoice_Click(object sender, EventArgs e)
        {
            //1. Create advance
            var advance = ReceiptBuilder.Build(Guid.NewGuid(), 1)
                 .SetTCRCode(TCR_CODE)
                 .SetDates(DateTime.Now, null)
                 .SetIsCash(false)
                 .SetUser(USER_NAME, USER_CODE)
                 .SetSeller(_seller.Name, _seller.VATNumber, _seller.Address)
                 .SetBuyer(_buyer.Name, _buyer.VATNumber, _buyer.Address)
                 .AddSaleItem("1", "Avans za gradjevinski materijal", 1, 400m, 21m)
                 .CalculateTotalAmount(EFIPaymentTypeEnum.ADVANCE);

            var result = await SendReceipt(advance);
            MessageBox.Show("Created recipt with IKOF " + result.UIDRequest);
            var advanceReference = new
            {
                IKOFReference = result.UIDRequest,
                IssuedAt = advance.ReceiptTime
            };
            //2. Delate partialy or full amount of the advance payment
            var correctiveInvoice = ReceiptBuilder.Build(Guid.NewGuid(), 2)
                .SetTCRCode(TCR_CODE)
                .SetDates(DateTime.Now, null)
                .SetIsCash(false)
                .SetUser(USER_NAME, USER_CODE)
                .SetSeller(_seller.Name, _seller.VATNumber, _seller.Address)
                .SetBuyer(_buyer.Name, _buyer.VATNumber, _buyer.Address)
                .SetCorrectiveInvoice()
                .AddIKOFReference(advanceReference.IKOFReference, advanceReference.IssuedAt)
                .AddSaleItem("1", "Avans za gradjevinski materijal", -1, 150m, 21m)
                .CalculateTotalAmount(EFIPaymentTypeEnum.ADVANCE);

            result = await SendReceipt(correctiveInvoice);
            MessageBox.Show("Created recipt with IKOF " + result.UIDRequest);

            //3. Use partialy or full amount of the Advance payment
            var connectedInvoice = ReceiptBuilder.Build(Guid.NewGuid(), 3)
                .SetTCRCode(TCR_CODE)
                .SetDates(DateTime.Now, null)
                .SetIsCash(false)
                .SetUser(USER_NAME, USER_CODE)
                .SetSeller(_seller.Name, _seller.VATNumber, _seller.Address)
                .SetBuyer(_buyer.Name, _buyer.VATNumber, _buyer.Address)
                .AddIKOFReference(advanceReference.IKOFReference, advanceReference.IssuedAt)
                .AddSaleItem("2", "Gradjevinski materijal tip 1", 1, 100m, 21m)
                .AddSaleItem("3", "Gradjevinski materijal tip 2", 1, 50m, 21m)
                .CalculateTotalAmount(EFIPaymentTypeEnum.ACCOUNT);

            result = await SendReceipt(connectedInvoice);
        }

        private async void btnTestOrderInvoice_Click(object sender, EventArgs e)
        {
            var order1 = ReceiptBuilder.Build(Guid.NewGuid(), 1)
                 .SetTCRCode(TCR_CODE)
                 .SetDates(DateTime.Now, null)
                 .SetIsCash(true)
                 .SetUser(USER_NAME, USER_CODE)
                 .SetSeller(_seller.Name, _seller.VATNumber, _seller.Address)
                 .AddSaleItem("1", "Coca Cola 0.25l", 2, 2.50m, 21m)
                 .CalculateTotalAmount(EFIPaymentTypeEnum.ORDER);

            var result=await SendReceipt(order1);
            MessageBox.Show("Created recipt with IKOF " + result.UIDRequest);
            var firstOrder = new
            {
                IKOFReference = result.UIDRequest,
                IssuedAt = order1.ReceiptTime
            };
            var order2 = ReceiptBuilder.Build(Guid.NewGuid(), 2)
                .SetTCRCode(TCR_CODE)
                .SetDates(DateTime.Now, null)
                .SetIsCash(true)
                .SetUser(USER_NAME, USER_CODE)
                .SetSeller(_seller.Name, _seller.VATNumber, _seller.Address)
                .AddSaleItem("2", "Fanta 0.25l", 1, 1.50m, 21m)
                .AddSaleItem("2", "Bavaria", 1, 3.50m, 21m)
                .CalculateTotalAmount(EFIPaymentTypeEnum.ORDER);

            result = await SendReceipt(order2);
            MessageBox.Show("Created recipt with IKOF " + result.UIDRequest);
            var secondOrder = new
            {
                IKOFReference = result.UIDRequest,
                IssuedAt = order2.ReceiptTime
            };

            //summary invoice
            //1. Set Header
            var receipt = ReceiptBuilder.Build(Guid.NewGuid(), 3)
                .SetTCRCode(TCR_CODE)
                .SetDates(DateTime.Now, null)
                .SetIsCash(true)
                .SetUser(USER_NAME, USER_CODE)
                .SetSeller(_seller.Name, _seller.VATNumber, _seller.Address);

            //2. Add items from all orders
            foreach (var saleItem in order1.Sales)
            {
                receipt.AddSaleItem(saleItem);
            }
            foreach (var saleItem in order2.Sales)
            {
                receipt.AddSaleItem(saleItem);
            }
            //3. Set type (summary), and add references to the orders
            receipt.SetSummaryInvoice();
            receipt.AddIKOFReference(firstOrder.IKOFReference, firstOrder.IssuedAt);
            receipt.AddIKOFReference(secondOrder.IKOFReference, secondOrder.IssuedAt);

            //4. Set payment type of summary invoice
            receipt.CalculateTotalAmount(EFIPaymentTypeEnum.CARD);

            result = await SendReceipt(receipt);

        }
    }
}         