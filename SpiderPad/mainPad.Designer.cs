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
            this.pbWeb = new System.Windows.Forms.PictureBox();
            this.tblPanel = new System.Windows.Forms.TableLayoutPanel();
            this.cbComponent = new System.Windows.Forms.ComboBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnAddLink = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbWeb)).BeginInit();
            this.tblPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbWeb
            // 
            this.pbWeb.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pbWeb.Location = new System.Drawing.Point(9, 4);
            this.pbWeb.Name = "pbWeb";
            this.pbWeb.Size = new System.Drawing.Size(507, 441);
            this.pbWeb.TabIndex = 0;
            this.pbWeb.TabStop = false;
            // 
            // tblPanel
            // 
            this.tblPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblPanel.ColumnCount = 2;
            this.tblPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPanel.Controls.Add(this.cbComponent, 0, 0);
            this.tblPanel.Controls.Add(this.btnSelect, 1, 0);
            this.tblPanel.Controls.Add(this.btnAddLink, 1, 1);
            this.tblPanel.Controls.Add(this.btnImport, 1, 4);
            this.tblPanel.Location = new System.Drawing.Point(520, 4);
            this.tblPanel.Name = "tblPanel";
            this.tblPanel.RowCount = 5;
            this.tblPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblPanel.Size = new System.Drawing.Size(224, 441);
            this.tblPanel.TabIndex = 1;
            // 
            // cbComponent
            // 
            this.cbComponent.BackColor = System.Drawing.Color.White;
            this.cbComponent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbComponent.FormattingEnabled = true;
            this.cbComponent.Items.AddRange(new object[] {
            "Node",
            "Layer"});
            this.cbComponent.Location = new System.Drawing.Point(3, 3);
            this.cbComponent.Name = "cbComponent";
            this.cbComponent.Size = new System.Drawing.Size(104, 21);
            this.cbComponent.TabIndex = 0;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(115, 3);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(106, 34);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // btnAddLink
            // 
            this.btnAddLink.Location = new System.Drawing.Point(115, 91);
            this.btnAddLink.Name = "btnAddLink";
            this.btnAddLink.Size = new System.Drawing.Size(75, 23);
            this.btnAddLink.TabIndex = 2;
            this.btnAddLink.Text = "Add Link";
            this.btnAddLink.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(115, 355);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // mainPad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(754, 450);
            this.Controls.Add(this.tblPanel);
            this.Controls.Add(this.pbWeb);
            this.Name = "mainPad";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Pad";
            this.Load += new System.EventHandler(this.mainPad_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbWeb)).EndInit();
            this.tblPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pbWeb;
        private TableLayoutPanel tblPanel;
        private ComboBox cbComponent;
        private Button btnSelect;
        private Button btnAddLink;
        private Button btnImport;
    }
}