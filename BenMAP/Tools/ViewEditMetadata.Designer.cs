﻿namespace BenMAP
{
    partial class ViewEditMetadata
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.rtbDescription = new System.Windows.Forms.RichTextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnSaveMetaData = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtSetupID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtReference = new System.Windows.Forms.TextBox();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblUnit = new System.Windows.Forms.Label();
            this.txtExtension = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.txtFileDate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblMedia = new System.Windows.Forms.Label();
            this.txtImportDate = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMedian = new System.Windows.Forms.TextBox();
            this.txtGeoName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblSpheroid = new System.Windows.Forms.Label();
            this.txtNumOfFeatures = new System.Windows.Forms.TextBox();
            this.lblNumOfFeatures = new System.Windows.Forms.Label();
            this.txtSpheroid = new System.Windows.Forms.TextBox();
            this.txtProj4String = new System.Windows.Forms.TextBox();
            this.lblProj4String = new System.Windows.Forms.Label();
            this.lblDatumType = new System.Windows.Forms.Label();
            this.txtDatum = new System.Windows.Forms.TextBox();
            this.lblDatum = new System.Windows.Forms.Label();
            this.txtDatumType = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSetupName = new System.Windows.Forms.TextBox();
            this.txtDatasetName = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.1519F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.8481F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(749, 507);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(175, 81);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 472);
            this.label1.Margin = new System.Windows.Forms.Padding(8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Description";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rtbDescription
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.rtbDescription, 2);
            this.rtbDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDescription.Location = new System.Drawing.Point(3, 497);
            this.rtbDescription.Name = "rtbDescription";
            this.rtbDescription.Size = new System.Drawing.Size(391, 138);
            this.rtbDescription.TabIndex = 2;
            this.rtbDescription.Text = "";
            this.rtbDescription.TextChanged += new System.EventHandler(this.rtbDescription_TextChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.btnOK);
            this.flowLayoutPanel1.Controls.Add(this.btnSaveMetaData);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 641);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(391, 33);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(313, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 27);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnSaveMetaData
            // 
            this.btnSaveMetaData.Location = new System.Drawing.Point(232, 3);
            this.btnSaveMetaData.Name = "btnSaveMetaData";
            this.btnSaveMetaData.Size = new System.Drawing.Size(75, 27);
            this.btnSaveMetaData.TabIndex = 2;
            this.btnSaveMetaData.Text = "Save";
            this.btnSaveMetaData.UseVisualStyleBackColor = true;
            this.btnSaveMetaData.Visible = false;
            this.btnSaveMetaData.Click += new System.EventHandler(this.btnSaveMetaData_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.25693F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.74307F));
            this.tableLayoutPanel2.Controls.Add(this.rtbDescription, 0, 17);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 16);
            this.tableLayoutPanel2.Controls.Add(this.txtSetupID, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 0, 18);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 15);
            this.tableLayoutPanel2.Controls.Add(this.txtReference, 1, 15);
            this.tableLayoutPanel2.Controls.Add(this.txtFileName, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblUnit, 0, 14);
            this.tableLayoutPanel2.Controls.Add(this.txtExtension, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.txtUnit, 1, 14);
            this.tableLayoutPanel2.Controls.Add(this.txtFileDate, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.lblMedia, 0, 13);
            this.tableLayoutPanel2.Controls.Add(this.txtImportDate, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.txtMedian, 1, 13);
            this.tableLayoutPanel2.Controls.Add(this.txtGeoName, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.lblName, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.lblSpheroid, 0, 12);
            this.tableLayoutPanel2.Controls.Add(this.txtNumOfFeatures, 1, 8);
            this.tableLayoutPanel2.Controls.Add(this.lblNumOfFeatures, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.txtSpheroid, 1, 12);
            this.tableLayoutPanel2.Controls.Add(this.txtProj4String, 1, 9);
            this.tableLayoutPanel2.Controls.Add(this.lblProj4String, 0, 9);
            this.tableLayoutPanel2.Controls.Add(this.lblDatumType, 0, 11);
            this.tableLayoutPanel2.Controls.Add(this.txtDatum, 1, 10);
            this.tableLayoutPanel2.Controls.Add(this.lblDatum, 0, 10);
            this.tableLayoutPanel2.Controls.Add(this.txtDatumType, 1, 11);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtSetupName, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtDatasetName, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 19;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(397, 678);
            this.tableLayoutPanel2.TabIndex = 3;
            this.tableLayoutPanel2.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel2_Paint);
            // 
            // txtSetupID
            // 
            this.txtSetupID.BackColor = System.Drawing.SystemColors.Control;
            this.txtSetupID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSetupID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSetupID.Enabled = false;
            this.txtSetupID.Location = new System.Drawing.Point(140, 4);
            this.txtSetupID.Margin = new System.Windows.Forms.Padding(4);
            this.txtSetupID.Multiline = true;
            this.txtSetupID.Name = "txtSetupID";
            this.txtSetupID.ReadOnly = true;
            this.txtSetupID.Size = new System.Drawing.Size(253, 21);
            this.txtSetupID.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 8);
            this.label7.Margin = new System.Windows.Forms.Padding(8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "SetupID:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 443);
            this.label9.Margin = new System.Windows.Forms.Padding(8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Reference:";
            // 
            // txtReference
            // 
            this.txtReference.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReference.Location = new System.Drawing.Point(140, 439);
            this.txtReference.Margin = new System.Windows.Forms.Padding(4);
            this.txtReference.Name = "txtReference";
            this.txtReference.Size = new System.Drawing.Size(253, 20);
            this.txtReference.TabIndex = 0;
            this.txtReference.TextChanged += new System.EventHandler(this.txtReference_TextChanged);
            // 
            // txtFileName
            // 
            this.txtFileName.BackColor = System.Drawing.SystemColors.Control;
            this.txtFileName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFileName.Enabled = false;
            this.txtFileName.Location = new System.Drawing.Point(140, 91);
            this.txtFileName.Margin = new System.Windows.Forms.Padding(4);
            this.txtFileName.Multiline = true;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(253, 21);
            this.txtFileName.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 95);
            this.label3.Margin = new System.Windows.Forms.Padding(8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "File name:";
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(8, 414);
            this.lblUnit.Margin = new System.Windows.Forms.Padding(8);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(29, 13);
            this.lblUnit.TabIndex = 0;
            this.lblUnit.Text = "Unit:";
            this.lblUnit.Visible = false;
            // 
            // txtExtension
            // 
            this.txtExtension.BackColor = System.Drawing.SystemColors.Control;
            this.txtExtension.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtExtension.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExtension.Enabled = false;
            this.txtExtension.Location = new System.Drawing.Point(140, 120);
            this.txtExtension.Margin = new System.Windows.Forms.Padding(4);
            this.txtExtension.Multiline = true;
            this.txtExtension.Name = "txtExtension";
            this.txtExtension.ReadOnly = true;
            this.txtExtension.Size = new System.Drawing.Size(253, 21);
            this.txtExtension.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 124);
            this.label4.Margin = new System.Windows.Forms.Padding(8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Extension:";
            // 
            // txtUnit
            // 
            this.txtUnit.BackColor = System.Drawing.SystemColors.Control;
            this.txtUnit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUnit.Enabled = false;
            this.txtUnit.Location = new System.Drawing.Point(140, 410);
            this.txtUnit.Margin = new System.Windows.Forms.Padding(4);
            this.txtUnit.Multiline = true;
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.ReadOnly = true;
            this.txtUnit.Size = new System.Drawing.Size(253, 21);
            this.txtUnit.TabIndex = 0;
            this.txtUnit.Visible = false;
            // 
            // txtFileDate
            // 
            this.txtFileDate.BackColor = System.Drawing.SystemColors.Control;
            this.txtFileDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFileDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFileDate.Enabled = false;
            this.txtFileDate.Location = new System.Drawing.Point(140, 149);
            this.txtFileDate.Margin = new System.Windows.Forms.Padding(4);
            this.txtFileDate.Multiline = true;
            this.txtFileDate.Name = "txtFileDate";
            this.txtFileDate.ReadOnly = true;
            this.txtFileDate.Size = new System.Drawing.Size(253, 21);
            this.txtFileDate.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 153);
            this.label5.Margin = new System.Windows.Forms.Padding(8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "File Date:";
            // 
            // lblMedia
            // 
            this.lblMedia.AutoSize = true;
            this.lblMedia.Location = new System.Drawing.Point(8, 385);
            this.lblMedia.Margin = new System.Windows.Forms.Padding(8);
            this.lblMedia.Name = "lblMedia";
            this.lblMedia.Size = new System.Drawing.Size(50, 13);
            this.lblMedia.TabIndex = 0;
            this.lblMedia.Text = "Meridian:";
            this.lblMedia.Visible = false;
            // 
            // txtImportDate
            // 
            this.txtImportDate.BackColor = System.Drawing.SystemColors.Control;
            this.txtImportDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtImportDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtImportDate.Enabled = false;
            this.txtImportDate.Location = new System.Drawing.Point(140, 178);
            this.txtImportDate.Margin = new System.Windows.Forms.Padding(4);
            this.txtImportDate.Multiline = true;
            this.txtImportDate.Name = "txtImportDate";
            this.txtImportDate.ReadOnly = true;
            this.txtImportDate.Size = new System.Drawing.Size(253, 21);
            this.txtImportDate.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 182);
            this.label6.Margin = new System.Windows.Forms.Padding(8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Import Date:";
            // 
            // txtMedian
            // 
            this.txtMedian.BackColor = System.Drawing.SystemColors.Control;
            this.txtMedian.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMedian.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMedian.Enabled = false;
            this.txtMedian.Location = new System.Drawing.Point(140, 381);
            this.txtMedian.Margin = new System.Windows.Forms.Padding(4);
            this.txtMedian.Multiline = true;
            this.txtMedian.Name = "txtMedian";
            this.txtMedian.ReadOnly = true;
            this.txtMedian.Size = new System.Drawing.Size(253, 21);
            this.txtMedian.TabIndex = 0;
            this.txtMedian.Visible = false;
            // 
            // txtGeoName
            // 
            this.txtGeoName.BackColor = System.Drawing.SystemColors.Control;
            this.txtGeoName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGeoName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGeoName.Enabled = false;
            this.txtGeoName.Location = new System.Drawing.Point(140, 207);
            this.txtGeoName.Margin = new System.Windows.Forms.Padding(4);
            this.txtGeoName.Multiline = true;
            this.txtGeoName.Name = "txtGeoName";
            this.txtGeoName.ReadOnly = true;
            this.txtGeoName.Size = new System.Drawing.Size(253, 21);
            this.txtGeoName.TabIndex = 0;
            this.txtGeoName.Visible = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(8, 211);
            this.lblName.Margin = new System.Windows.Forms.Padding(8);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            this.lblName.Visible = false;
            // 
            // lblSpheroid
            // 
            this.lblSpheroid.AutoSize = true;
            this.lblSpheroid.Location = new System.Drawing.Point(8, 356);
            this.lblSpheroid.Margin = new System.Windows.Forms.Padding(8);
            this.lblSpheroid.Name = "lblSpheroid";
            this.lblSpheroid.Size = new System.Drawing.Size(52, 13);
            this.lblSpheroid.TabIndex = 0;
            this.lblSpheroid.Text = "Spheroid:";
            this.lblSpheroid.Visible = false;
            // 
            // txtNumOfFeatures
            // 
            this.txtNumOfFeatures.BackColor = System.Drawing.SystemColors.Control;
            this.txtNumOfFeatures.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNumOfFeatures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNumOfFeatures.Enabled = false;
            this.txtNumOfFeatures.Location = new System.Drawing.Point(140, 236);
            this.txtNumOfFeatures.Margin = new System.Windows.Forms.Padding(4);
            this.txtNumOfFeatures.Multiline = true;
            this.txtNumOfFeatures.Name = "txtNumOfFeatures";
            this.txtNumOfFeatures.ReadOnly = true;
            this.txtNumOfFeatures.Size = new System.Drawing.Size(253, 21);
            this.txtNumOfFeatures.TabIndex = 0;
            this.txtNumOfFeatures.Visible = false;
            // 
            // lblNumOfFeatures
            // 
            this.lblNumOfFeatures.AutoSize = true;
            this.lblNumOfFeatures.Location = new System.Drawing.Point(8, 240);
            this.lblNumOfFeatures.Margin = new System.Windows.Forms.Padding(8);
            this.lblNumOfFeatures.Name = "lblNumOfFeatures";
            this.lblNumOfFeatures.Size = new System.Drawing.Size(103, 13);
            this.lblNumOfFeatures.TabIndex = 0;
            this.lblNumOfFeatures.Text = "Number of Features:";
            this.lblNumOfFeatures.Visible = false;
            // 
            // txtSpheroid
            // 
            this.txtSpheroid.BackColor = System.Drawing.SystemColors.Control;
            this.txtSpheroid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSpheroid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSpheroid.Enabled = false;
            this.txtSpheroid.Location = new System.Drawing.Point(140, 352);
            this.txtSpheroid.Margin = new System.Windows.Forms.Padding(4);
            this.txtSpheroid.Multiline = true;
            this.txtSpheroid.Name = "txtSpheroid";
            this.txtSpheroid.ReadOnly = true;
            this.txtSpheroid.Size = new System.Drawing.Size(253, 21);
            this.txtSpheroid.TabIndex = 0;
            this.txtSpheroid.Visible = false;
            // 
            // txtProj4String
            // 
            this.txtProj4String.BackColor = System.Drawing.SystemColors.Control;
            this.txtProj4String.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtProj4String.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProj4String.Enabled = false;
            this.txtProj4String.Location = new System.Drawing.Point(140, 265);
            this.txtProj4String.Margin = new System.Windows.Forms.Padding(4);
            this.txtProj4String.Multiline = true;
            this.txtProj4String.Name = "txtProj4String";
            this.txtProj4String.ReadOnly = true;
            this.txtProj4String.Size = new System.Drawing.Size(253, 21);
            this.txtProj4String.TabIndex = 0;
            this.txtProj4String.Visible = false;
            // 
            // lblProj4String
            // 
            this.lblProj4String.AutoSize = true;
            this.lblProj4String.Location = new System.Drawing.Point(8, 269);
            this.lblProj4String.Margin = new System.Windows.Forms.Padding(8);
            this.lblProj4String.Name = "lblProj4String";
            this.lblProj4String.Size = new System.Drawing.Size(61, 13);
            this.lblProj4String.TabIndex = 0;
            this.lblProj4String.Text = "Proj4String:";
            this.lblProj4String.Visible = false;
            // 
            // lblDatumType
            // 
            this.lblDatumType.AutoSize = true;
            this.lblDatumType.Location = new System.Drawing.Point(8, 327);
            this.lblDatumType.Margin = new System.Windows.Forms.Padding(8);
            this.lblDatumType.Name = "lblDatumType";
            this.lblDatumType.Size = new System.Drawing.Size(68, 13);
            this.lblDatumType.TabIndex = 0;
            this.lblDatumType.Text = "Datum Type:";
            this.lblDatumType.Visible = false;
            // 
            // txtDatum
            // 
            this.txtDatum.BackColor = System.Drawing.SystemColors.Control;
            this.txtDatum.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDatum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDatum.Enabled = false;
            this.txtDatum.Location = new System.Drawing.Point(140, 294);
            this.txtDatum.Margin = new System.Windows.Forms.Padding(4);
            this.txtDatum.Multiline = true;
            this.txtDatum.Name = "txtDatum";
            this.txtDatum.ReadOnly = true;
            this.txtDatum.Size = new System.Drawing.Size(253, 21);
            this.txtDatum.TabIndex = 0;
            this.txtDatum.Visible = false;
            // 
            // lblDatum
            // 
            this.lblDatum.AutoSize = true;
            this.lblDatum.Location = new System.Drawing.Point(8, 298);
            this.lblDatum.Margin = new System.Windows.Forms.Padding(8);
            this.lblDatum.Name = "lblDatum";
            this.lblDatum.Size = new System.Drawing.Size(41, 13);
            this.lblDatum.TabIndex = 0;
            this.lblDatum.Text = "Datum:";
            this.lblDatum.Visible = false;
            // 
            // txtDatumType
            // 
            this.txtDatumType.BackColor = System.Drawing.SystemColors.Control;
            this.txtDatumType.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDatumType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDatumType.Enabled = false;
            this.txtDatumType.Location = new System.Drawing.Point(140, 323);
            this.txtDatumType.Margin = new System.Windows.Forms.Padding(4);
            this.txtDatumType.Multiline = true;
            this.txtDatumType.Name = "txtDatumType";
            this.txtDatumType.ReadOnly = true;
            this.txtDatumType.Size = new System.Drawing.Size(253, 21);
            this.txtDatumType.TabIndex = 0;
            this.txtDatumType.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 37);
            this.label8.Margin = new System.Windows.Forms.Padding(8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Setup name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 66);
            this.label2.Margin = new System.Windows.Forms.Padding(8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Dataset Name:";
            this.label2.Visible = false;
            // 
            // txtSetupName
            // 
            this.txtSetupName.BackColor = System.Drawing.SystemColors.Control;
            this.txtSetupName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSetupName.Enabled = false;
            this.txtSetupName.Location = new System.Drawing.Point(140, 33);
            this.txtSetupName.Margin = new System.Windows.Forms.Padding(4);
            this.txtSetupName.Multiline = true;
            this.txtSetupName.Name = "txtSetupName";
            this.txtSetupName.ReadOnly = true;
            this.txtSetupName.Size = new System.Drawing.Size(253, 21);
            this.txtSetupName.TabIndex = 0;
            // 
            // txtDatasetName
            // 
            this.txtDatasetName.BackColor = System.Drawing.SystemColors.Control;
            this.txtDatasetName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDatasetName.Enabled = false;
            this.txtDatasetName.Location = new System.Drawing.Point(140, 62);
            this.txtDatasetName.Margin = new System.Windows.Forms.Padding(4);
            this.txtDatasetName.Multiline = true;
            this.txtDatasetName.Name = "txtDatasetName";
            this.txtDatasetName.ReadOnly = true;
            this.txtDatasetName.Size = new System.Drawing.Size(253, 21);
            this.txtDatasetName.TabIndex = 0;
            this.txtDatasetName.Visible = false;
            // 
            // ViewEditMetadata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 678);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.tableLayoutPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "ViewEditMetadata";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ViewEditMetadata";
            this.Shown += new System.EventHandler(this.ViewEditMetadata_Shown);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox rtbDescription;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnSaveMetaData;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.TextBox txtExtension;
        private System.Windows.Forms.TextBox txtFileDate;
        private System.Windows.Forms.TextBox txtImportDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSetupID;
        private System.Windows.Forms.TextBox txtSetupName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtReference;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblNumOfFeatures;
        private System.Windows.Forms.Label lblProj4String;
        private System.Windows.Forms.Label lblDatum;
        private System.Windows.Forms.Label lblDatumType;
        private System.Windows.Forms.Label lblSpheroid;
        private System.Windows.Forms.Label lblMedia;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.TextBox txtGeoName;
        private System.Windows.Forms.TextBox txtNumOfFeatures;
        private System.Windows.Forms.TextBox txtProj4String;
        private System.Windows.Forms.TextBox txtDatum;
        private System.Windows.Forms.TextBox txtDatumType;
        private System.Windows.Forms.TextBox txtSpheroid;
        private System.Windows.Forms.TextBox txtMedian;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDatasetName;
    }
}