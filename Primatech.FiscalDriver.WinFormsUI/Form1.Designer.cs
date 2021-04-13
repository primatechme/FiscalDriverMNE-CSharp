namespace Primatech.FiscalDriver.WinFormsUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnInitialDeposit = new System.Windows.Forms.Button();
            this.btnWithdrawDeposit = new System.Windows.Forms.Button();
            this.btnCashReceipt = new System.Windows.Forms.Button();
            this.btnNonCashReceipt = new System.Windows.Forms.Button();
            this.btnReceiptWithError = new System.Windows.Forms.Button();
            this.txtRequest = new System.Windows.Forms.TextBox();
            this.txtResponse = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTcrCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUserCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtToken = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtBaseUrl = new System.Windows.Forms.TextBox();
            this.btnSepPortal = new System.Windows.Forms.Button();
            this.btnSendReceiptFromRequestField = new System.Windows.Forms.Button();
            this.btnSaveRequest = new System.Windows.Forms.Button();
            this.btnSaveResponse = new System.Windows.Forms.Button();
            this.btnLoadRequest = new System.Windows.Forms.Button();
            this.btnSendDepositFromRequestField = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnInitialDeposit
            // 
            this.btnInitialDeposit.Location = new System.Drawing.Point(9, 128);
            this.btnInitialDeposit.Name = "btnInitialDeposit";
            this.btnInitialDeposit.Size = new System.Drawing.Size(238, 23);
            this.btnInitialDeposit.TabIndex = 0;
            this.btnInitialDeposit.Text = "1. Test initial deposit";
            this.btnInitialDeposit.UseVisualStyleBackColor = true;
            this.btnInitialDeposit.Click += new System.EventHandler(this.btnInitialDeposit_Click);
            // 
            // btnWithdrawDeposit
            // 
            this.btnWithdrawDeposit.Location = new System.Drawing.Point(9, 158);
            this.btnWithdrawDeposit.Name = "btnWithdrawDeposit";
            this.btnWithdrawDeposit.Size = new System.Drawing.Size(238, 23);
            this.btnWithdrawDeposit.TabIndex = 1;
            this.btnWithdrawDeposit.Text = "2. Test withdraw deposit";
            this.btnWithdrawDeposit.UseVisualStyleBackColor = true;
            this.btnWithdrawDeposit.Click += new System.EventHandler(this.btnWithdrawDeposit_Click);
            // 
            // btnCashReceipt
            // 
            this.btnCashReceipt.Location = new System.Drawing.Point(9, 188);
            this.btnCashReceipt.Name = "btnCashReceipt";
            this.btnCashReceipt.Size = new System.Drawing.Size(238, 23);
            this.btnCashReceipt.TabIndex = 2;
            this.btnCashReceipt.Text = "3. Test cash receipt";
            this.btnCashReceipt.UseVisualStyleBackColor = true;
            this.btnCashReceipt.Click += new System.EventHandler(this.btnCashReceipt_Click);
            // 
            // btnNonCashReceipt
            // 
            this.btnNonCashReceipt.Location = new System.Drawing.Point(9, 218);
            this.btnNonCashReceipt.Name = "btnNonCashReceipt";
            this.btnNonCashReceipt.Size = new System.Drawing.Size(238, 23);
            this.btnNonCashReceipt.TabIndex = 3;
            this.btnNonCashReceipt.Text = "4. Test non-cash receipt";
            this.btnNonCashReceipt.UseVisualStyleBackColor = true;
            this.btnNonCashReceipt.Click += new System.EventHandler(this.btnNonCashReceipt_Click);
            // 
            // btnReceiptWithError
            // 
            this.btnReceiptWithError.Location = new System.Drawing.Point(9, 247);
            this.btnReceiptWithError.Name = "btnReceiptWithError";
            this.btnReceiptWithError.Size = new System.Drawing.Size(238, 23);
            this.btnReceiptWithError.TabIndex = 4;
            this.btnReceiptWithError.Text = "5. Test receipt with error";
            this.btnReceiptWithError.UseVisualStyleBackColor = true;
            this.btnReceiptWithError.Click += new System.EventHandler(this.btnReceiptWithError_Click);
            // 
            // txtRequest
            // 
            this.txtRequest.Location = new System.Drawing.Point(253, 128);
            this.txtRequest.Multiline = true;
            this.txtRequest.Name = "txtRequest";
            this.txtRequest.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRequest.Size = new System.Drawing.Size(636, 235);
            this.txtRequest.TabIndex = 5;
            // 
            // txtResponse
            // 
            this.txtResponse.Location = new System.Drawing.Point(253, 394);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResponse.Size = new System.Drawing.Size(636, 233);
            this.txtResponse.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(254, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Request";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(250, 373);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Response";
            // 
            // txtTcrCode
            // 
            this.txtTcrCode.Location = new System.Drawing.Point(12, 22);
            this.txtTcrCode.Name = "txtTcrCode";
            this.txtTcrCode.ReadOnly = true;
            this.txtTcrCode.Size = new System.Drawing.Size(114, 20);
            this.txtTcrCode.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "TCR Code";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(127, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "User Code";
            // 
            // txtUserCode
            // 
            this.txtUserCode.Location = new System.Drawing.Point(130, 22);
            this.txtUserCode.Name = "txtUserCode";
            this.txtUserCode.ReadOnly = true;
            this.txtUserCode.Size = new System.Drawing.Size(117, 20);
            this.txtUserCode.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(250, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "User Name";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(253, 22);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.ReadOnly = true;
            this.txtUserName.Size = new System.Drawing.Size(184, 20);
            this.txtUserName.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Token";
            // 
            // txtToken
            // 
            this.txtToken.Location = new System.Drawing.Point(12, 66);
            this.txtToken.Name = "txtToken";
            this.txtToken.ReadOnly = true;
            this.txtToken.Size = new System.Drawing.Size(877, 20);
            this.txtToken.TabIndex = 15;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(440, 6);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "User Name";
            // 
            // txtBaseUrl
            // 
            this.txtBaseUrl.Location = new System.Drawing.Point(443, 22);
            this.txtBaseUrl.Name = "txtBaseUrl";
            this.txtBaseUrl.ReadOnly = true;
            this.txtBaseUrl.Size = new System.Drawing.Size(446, 20);
            this.txtBaseUrl.TabIndex = 17;
            // 
            // btnSepPortal
            // 
            this.btnSepPortal.Location = new System.Drawing.Point(648, 368);
            this.btnSepPortal.Name = "btnSepPortal";
            this.btnSepPortal.Size = new System.Drawing.Size(166, 23);
            this.btnSepPortal.TabIndex = 19;
            this.btnSepPortal.Text = "Open receipt on SEP portal";
            this.btnSepPortal.UseVisualStyleBackColor = true;
            this.btnSepPortal.Click += new System.EventHandler(this.btnSepPortal_Click);
            // 
            // btnSendReceiptFromRequestField
            // 
            this.btnSendReceiptFromRequestField.Location = new System.Drawing.Point(9, 305);
            this.btnSendReceiptFromRequestField.Name = "btnSendReceiptFromRequestField";
            this.btnSendReceiptFromRequestField.Size = new System.Drawing.Size(238, 23);
            this.btnSendReceiptFromRequestField.TabIndex = 21;
            this.btnSendReceiptFromRequestField.Text = "7. Send receipt from Request field";
            this.btnSendReceiptFromRequestField.UseVisualStyleBackColor = true;
            this.btnSendReceiptFromRequestField.Click += new System.EventHandler(this.btnSendReceiptFromRequestField_Click);
            // 
            // btnSaveRequest
            // 
            this.btnSaveRequest.Location = new System.Drawing.Point(815, 102);
            this.btnSaveRequest.Name = "btnSaveRequest";
            this.btnSaveRequest.Size = new System.Drawing.Size(74, 23);
            this.btnSaveRequest.TabIndex = 22;
            this.btnSaveRequest.Text = "Save";
            this.btnSaveRequest.UseVisualStyleBackColor = true;
            this.btnSaveRequest.Click += new System.EventHandler(this.btnSaveRequest_Click);
            // 
            // btnSaveResponse
            // 
            this.btnSaveResponse.Location = new System.Drawing.Point(815, 368);
            this.btnSaveResponse.Name = "btnSaveResponse";
            this.btnSaveResponse.Size = new System.Drawing.Size(74, 23);
            this.btnSaveResponse.TabIndex = 23;
            this.btnSaveResponse.Text = "Save";
            this.btnSaveResponse.UseVisualStyleBackColor = true;
            this.btnSaveResponse.Click += new System.EventHandler(this.btnSaveResponse_Click);
            // 
            // btnLoadRequest
            // 
            this.btnLoadRequest.Location = new System.Drawing.Point(740, 102);
            this.btnLoadRequest.Name = "btnLoadRequest";
            this.btnLoadRequest.Size = new System.Drawing.Size(74, 23);
            this.btnLoadRequest.TabIndex = 24;
            this.btnLoadRequest.Text = "Load";
            this.btnLoadRequest.UseVisualStyleBackColor = true;
            this.btnLoadRequest.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // btnSendDepositFromRequestField
            // 
            this.btnSendDepositFromRequestField.Location = new System.Drawing.Point(9, 276);
            this.btnSendDepositFromRequestField.Name = "btnSendDepositFromRequestField";
            this.btnSendDepositFromRequestField.Size = new System.Drawing.Size(238, 23);
            this.btnSendDepositFromRequestField.TabIndex = 25;
            this.btnSendDepositFromRequestField.Text = "6. Send deposit from Request field";
            this.btnSendDepositFromRequestField.UseVisualStyleBackColor = true;
            this.btnSendDepositFromRequestField.Click += new System.EventHandler(this.btnSendDepositFromRequestField_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 631);
            this.Controls.Add(this.btnSendDepositFromRequestField);
            this.Controls.Add(this.btnLoadRequest);
            this.Controls.Add(this.btnSaveResponse);
            this.Controls.Add(this.btnSaveRequest);
            this.Controls.Add(this.btnSendReceiptFromRequestField);
            this.Controls.Add(this.btnSepPortal);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtBaseUrl);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtToken);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtUserCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTcrCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.txtRequest);
            this.Controls.Add(this.btnReceiptWithError);
            this.Controls.Add(this.btnNonCashReceipt);
            this.Controls.Add(this.btnCashReceipt);
            this.Controls.Add(this.btnWithdrawDeposit);
            this.Controls.Add(this.btnInitialDeposit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test Fiscal Service";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInitialDeposit;
        private System.Windows.Forms.Button btnWithdrawDeposit;
        private System.Windows.Forms.Button btnCashReceipt;
        private System.Windows.Forms.Button btnNonCashReceipt;
        private System.Windows.Forms.Button btnReceiptWithError;
        private System.Windows.Forms.TextBox txtRequest;
        private System.Windows.Forms.TextBox txtResponse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTcrCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUserCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtToken;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtBaseUrl;
        private System.Windows.Forms.Button btnSepPortal;
        private System.Windows.Forms.Button btnSendReceiptFromRequestField;
        private System.Windows.Forms.Button btnSaveRequest;
        private System.Windows.Forms.Button btnSaveResponse;
        private System.Windows.Forms.Button btnLoadRequest;
        private System.Windows.Forms.Button btnSendDepositFromRequestField;
    }
}

