using System.Windows.Forms;
using System.Drawing;
namespace SpiderPad
{
    partial class mainPad
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        public System.ComponentModel.IContainer components = null;

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
            pbWeb = new PictureBox();
            tblPanel = new TableLayoutPanel();
            comboBox1 = new ComboBox();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)pbWeb).BeginInit();
            tblPanel.SuspendLayout();
            SuspendLayout();
            // 
            // pbWeb
            // 
            pbWeb.BackColor = SystemColors.ControlLightLight;
            pbWeb.Location = new Point(10, 5);
            pbWeb.Name = "pbWeb";
            pbWeb.Size = new Size(591, 509);
            pbWeb.TabIndex = 0;
            pbWeb.TabStop = false;
            // 
            // tblPanel
            // 
            tblPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tblPanel.ColumnCount = 2;
            tblPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tblPanel.Controls.Add(comboBox1, 0, 0);
            tblPanel.Controls.Add(button1, 1, 0);
            tblPanel.Location = new Point(607, 5);
            tblPanel.Name = "tblPanel";
            tblPanel.RowCount = 5;
            tblPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblPanel.Size = new Size(261, 509);
            tblPanel.TabIndex = 1;
            tblPanel.Paint += tblPanel_Paint;
            // 
            // comboBox1
            // 
            comboBox1.BackColor = Color.White;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Node" });
            comboBox1.Location = new Point(3, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(133, 3);
            button1.Name = "button1";
            button1.Size = new Size(125, 39);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // mainPad
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(880, 519);
            Controls.Add(tblPanel);
            Controls.Add(pbWeb);
            Name = "mainPad";
            SizeGripStyle = SizeGripStyle.Show;
            Text = "Pad";
            Load += Pad_Load;
            Resize += Pad_Resize;
            ((System.ComponentModel.ISupportInitialize)pbWeb).EndInit();
            tblPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pbWeb;
        private TableLayoutPanel tblPanel;
        private ComboBox comboBox1;
        private Button button1;
    }
}