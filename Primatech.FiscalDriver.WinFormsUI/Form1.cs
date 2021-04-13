using Newtonsoft.Json;
using Primatech.FiscalDriver.Helpers;
using Primatech.FiscalDriver.Infrastructure;
using Primatech.FiscalDriver.Infrastructure.Builders;
using Primatech.FiscalModels.JSON.Requests;
using Primatech.FiscalModels.XML.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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

        public Form1()
        {
            InitializeComponent();
            LoadConfigValues();
            _fiscalApiService = new FiscalApiService(BASE_URL, TOKEN);
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
                var result = await _fiscalApiService.CreateDeposit(deposit.ToXMLModel());
                DisplayResponse(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error");// + "\nStackTrace: " + ex.StackTrace);
            }
            EnableControls(true);
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
                  .SetBuyer("Buyer Company Name", "07654321", "Some Buyer Address")
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

        private async Task SendReceipt(EFIReceipt receipt)
        {
            await SendReceipt(receipt, true);
        }

        private async Task SendReceipt(EFIReceipt receipt,bool showRequest)
        {
            EnableControls(false);
            if (showRequest)
                DisplayRequest(receipt);

            try
            {
                var result = await _fiscalApiService.CreateReceipt(receipt.ToXMLModel());
                result.Url.UrlContent = result.Url.ToModel();
                DisplayResponse(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            EnableControls(true);
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

    }
}
