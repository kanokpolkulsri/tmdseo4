namespace db
{
    partial class print_tempOut
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(print_tempOut));
            this.tb_tempOutBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.databaseDataSet = new db.databaseDataSet();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.tb_tempOutTableAdapter = new db.databaseDataSetTableAdapters.tb_tempOutTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.tb_tempOutBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.databaseDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // tb_tempOutBindingSource
            // 
            this.tb_tempOutBindingSource.DataMember = "tb_tempOut";
            this.tb_tempOutBindingSource.DataSource = this.databaseDataSet;
            // 
            // databaseDataSet
            // 
            this.databaseDataSet.DataSetName = "databaseDataSet";
            this.databaseDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.tb_tempOutBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "db.tempOut.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(26, 39);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(633, 352);
            this.reportViewer1.TabIndex = 0;
            // 
            // tb_tempOutTableAdapter
            // 
            this.tb_tempOutTableAdapter.ClearBeforeFill = true;
            // 
            // print_tempOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 415);
            this.Controls.Add(this.reportViewer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "print_tempOut";
            this.Text = "print_tempOut";
            this.Load += new System.EventHandler(this.print_tempOut_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tb_tempOutBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.databaseDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource tb_tempOutBindingSource;
        private databaseDataSet databaseDataSet;
        private databaseDataSetTableAdapters.tb_tempOutTableAdapter tb_tempOutTableAdapter;
    }
}