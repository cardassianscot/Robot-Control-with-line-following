namespace Robot_Control
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnFwd = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.dirLabel = new System.Windows.Forms.Label();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.cbPortsInput = new System.Windows.Forms.ComboBox();
            this.serialDisplay = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.cbPortsOutput = new System.Windows.Forms.ComboBox();
            this.serialPort2 = new System.IO.Ports.SerialPort(this.components);
            this.btnSensor = new System.Windows.Forms.Button();
            this.btnFollowLine = new System.Windows.Forms.Button();
            this.btnLidar = new System.Windows.Forms.Button();
            this.cbScanMode = new System.Windows.Forms.ComboBox();
            this.btnCalibrate = new System.Windows.Forms.Button();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.btnFwd, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnLeft, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRight, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnBack, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.dirLabel, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(28, 105);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(312, 257);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnFwd
            // 
            this.btnFwd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFwd.Location = new System.Drawing.Point(107, 3);
            this.btnFwd.Name = "btnFwd";
            this.btnFwd.Size = new System.Drawing.Size(98, 79);
            this.btnFwd.TabIndex = 0;
            this.btnFwd.Text = "Fwd";
            this.btnFwd.UseVisualStyleBackColor = true;
            this.btnFwd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fwdMouseDown);
            this.btnFwd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.fwdMouseUp);
            // 
            // btnLeft
            // 
            this.btnLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLeft.Location = new System.Drawing.Point(3, 88);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(98, 79);
            this.btnLeft.TabIndex = 1;
            this.btnLeft.Text = "Left";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.leftMouseDown);
            this.btnLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.leftMouseUp);
            // 
            // btnRight
            // 
            this.btnRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRight.Location = new System.Drawing.Point(211, 88);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(98, 79);
            this.btnRight.TabIndex = 2;
            this.btnRight.Text = "Right";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rightMouseDown);
            this.btnRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rightMouseUp);
            // 
            // btnBack
            // 
            this.btnBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBack.Location = new System.Drawing.Point(107, 173);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(98, 81);
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.backMouseDown);
            this.btnBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.backMouseUp);
            // 
            // dirLabel
            // 
            this.dirLabel.AutoSize = true;
            this.dirLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dirLabel.Location = new System.Drawing.Point(107, 85);
            this.dirLabel.Name = "dirLabel";
            this.dirLabel.Size = new System.Drawing.Size(98, 85);
            this.dirLabel.TabIndex = 4;
            this.dirLabel.Text = "Stop";
            this.dirLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // serialPort1
            // 
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived);
            // 
            // cbPortsInput
            // 
            this.cbPortsInput.FormattingEnabled = true;
            this.cbPortsInput.Location = new System.Drawing.Point(28, 16);
            this.cbPortsInput.Name = "cbPortsInput";
            this.cbPortsInput.Size = new System.Drawing.Size(173, 28);
            this.cbPortsInput.TabIndex = 1;
            // 
            // serialDisplay
            // 
            this.serialDisplay.Location = new System.Drawing.Point(405, 16);
            this.serialDisplay.Name = "serialDisplay";
            this.serialDisplay.ReadOnly = true;
            this.serialDisplay.Size = new System.Drawing.Size(309, 304);
            this.serialDisplay.TabIndex = 2;
            this.serialDisplay.Text = "";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(27, 448);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(309, 90);
            this.textBox1.TabIndex = 3;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(262, 550);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 31);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(208, 13);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(128, 31);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnectClick);
            // 
            // cbPortsOutput
            // 
            this.cbPortsOutput.FormattingEnabled = true;
            this.cbPortsOutput.Location = new System.Drawing.Point(28, 60);
            this.cbPortsOutput.Name = "cbPortsOutput";
            this.cbPortsOutput.Size = new System.Drawing.Size(173, 28);
            this.cbPortsOutput.TabIndex = 6;
            // 
            // serialPort2
            // 
            this.serialPort2.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived);
            // 
            // btnSensor
            // 
            this.btnSensor.Location = new System.Drawing.Point(31, 368);
            this.btnSensor.Name = "btnSensor";
            this.btnSensor.Size = new System.Drawing.Size(98, 35);
            this.btnSensor.TabIndex = 7;
            this.btnSensor.Text = "Sensor";
            this.btnSensor.UseVisualStyleBackColor = true;
            this.btnSensor.Click += new System.EventHandler(this.btnSensor_Click);
            // 
            // btnFollowLine
            // 
            this.btnFollowLine.Location = new System.Drawing.Point(135, 368);
            this.btnFollowLine.Name = "btnFollowLine";
            this.btnFollowLine.Size = new System.Drawing.Size(98, 34);
            this.btnFollowLine.TabIndex = 8;
            this.btnFollowLine.Text = "Follow";
            this.btnFollowLine.UseVisualStyleBackColor = true;
            this.btnFollowLine.Click += new System.EventHandler(this.followLine_Click);
            // 
            // btnLidar
            // 
            this.btnLidar.Location = new System.Drawing.Point(239, 368);
            this.btnLidar.Name = "btnLidar";
            this.btnLidar.Size = new System.Drawing.Size(97, 34);
            this.btnLidar.TabIndex = 9;
            this.btnLidar.Text = "LIDAR";
            this.btnLidar.UseVisualStyleBackColor = true;
            this.btnLidar.Click += new System.EventHandler(this.lidar_Click);
            // 
            // cbScanMode
            // 
            this.cbScanMode.FormattingEnabled = true;
            this.cbScanMode.Items.AddRange(new object[] {
            "3 Point",
            "Colour by Closeness",
            "Fade with Time"});
            this.cbScanMode.Location = new System.Drawing.Point(405, 510);
            this.cbScanMode.Name = "cbScanMode";
            this.cbScanMode.Size = new System.Drawing.Size(309, 28);
            this.cbScanMode.TabIndex = 10;
            this.cbScanMode.SelectedIndexChanged += new System.EventHandler(this.cbScanMode_SelectedIndexChanged);
            // 
            // btnCalibrate
            // 
            this.btnCalibrate.Location = new System.Drawing.Point(135, 408);
            this.btnCalibrate.Name = "btnCalibrate";
            this.btnCalibrate.Size = new System.Drawing.Size(98, 34);
            this.btnCalibrate.TabIndex = 11;
            this.btnCalibrate.Text = "Calibrate";
            this.btnCalibrate.UseVisualStyleBackColor = true;
            this.btnCalibrate.Click += new System.EventHandler(this.btnCalibrate_Click);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(358, 105);
            this.vScrollBar1.Maximum = 255;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(26, 257);
            this.vScrollBar1.TabIndex = 12;
            this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_ValueChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 602);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.btnCalibrate);
            this.Controls.Add(this.cbScanMode);
            this.Controls.Add(this.btnLidar);
            this.Controls.Add(this.btnFollowLine);
            this.Controls.Add(this.btnSensor);
            this.Controls.Add(this.cbPortsOutput);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.serialDisplay);
            this.Controls.Add(this.cbPortsInput);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Robot Control";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnFwd;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label dirLabel;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.ComboBox cbPortsInput;
        private System.Windows.Forms.RichTextBox serialDisplay;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ComboBox cbPortsOutput;
        private System.IO.Ports.SerialPort serialPort2;
        private System.Windows.Forms.Button btnSensor;
        private System.Windows.Forms.Button btnFollowLine;
        private System.Windows.Forms.Button btnLidar;
        private System.Windows.Forms.ComboBox cbScanMode;
        private System.Windows.Forms.Button btnCalibrate;
        private System.Windows.Forms.VScrollBar vScrollBar1;
    }
}

