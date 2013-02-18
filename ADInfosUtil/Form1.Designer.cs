namespace Feng.Windows.Utils
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnExportAllToOne = new System.Windows.Forms.Button();
            this.btnExportPath = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExportTemplate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDbTableName = new System.Windows.Forms.TextBox();
            this.btnExportData = new System.Windows.Forms.Button();
            this.btnExcuteScript = new System.Windows.Forms.Button();
            this.btnExportViews = new System.Windows.Forms.Button();
            this.btnBackup = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnImportFolder = new System.Windows.Forms.Button();
            this.btnDeleteData = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.ckbFKSelection = new System.Windows.Forms.CheckBox();
            this.btnEnableFK = new System.Windows.Forms.Button();
            this.btnDisableFK = new System.Windows.Forms.Button();
            this.btnTestView = new System.Windows.Forms.Button();
            this.lblServerName = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnUnInstallModule = new System.Windows.Forms.Button();
            this.btnInstallModule = new System.Windows.Forms.Button();
            this.btnPackgeModuleAccoringInfo = new System.Windows.Forms.Button();
            this.txtModuleName = new System.Windows.Forms.TextBox();
            this.btnModuleGenerateInfo = new System.Windows.Forms.Button();
            this.btnPackageCurrentDb = new System.Windows.Forms.Button();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.btnUpdateConnectionString = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnGenerateHbm = new System.Windows.Forms.Button();
            this.btnSchemaUpdate = new System.Windows.Forms.Button();
            this.btnSchemaExport = new System.Windows.Forms.Button();
            this.btnOpenConfig = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btnGenerateHelpHtml = new System.Windows.Forms.Button();
            this.btnGenerateHelpXml = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.btnGenerateAdInfos2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAdInfoCreatorGridName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClearDefaultSettings = new System.Windows.Forms.Button();
            this.btnCreateDefaultSettings = new System.Windows.Forms.Button();
            this.txtAdInfoCreatorType = new System.Windows.Forms.TextBox();
            this.btnGenerateAdInfos = new System.Windows.Forms.Button();
            this.btnPythonScript = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExportAllToOne
            // 
            this.btnExportAllToOne.Location = new System.Drawing.Point(23, 20);
            this.btnExportAllToOne.Name = "btnExportAllToOne";
            this.btnExportAllToOne.Size = new System.Drawing.Size(164, 23);
            this.btnExportAllToOne.TabIndex = 0;
            this.btnExportAllToOne.Text = "导出到单一文件";
            this.btnExportAllToOne.UseVisualStyleBackColor = true;
            this.btnExportAllToOne.Click += new System.EventHandler(this.btnExportAllToOne_Click);
            // 
            // btnExportPath
            // 
            this.btnExportPath.Location = new System.Drawing.Point(23, 49);
            this.btnExportPath.Name = "btnExportPath";
            this.btnExportPath.Size = new System.Drawing.Size(164, 23);
            this.btnExportPath.TabIndex = 1;
            this.btnExportPath.Text = "导出到文件夹";
            this.btnExportPath.UseVisualStyleBackColor = true;
            this.btnExportPath.Click += new System.EventHandler(this.btnExportPath_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(25, 20);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(164, 23);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "导入数据";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExportTemplate
            // 
            this.btnExportTemplate.Location = new System.Drawing.Point(23, 78);
            this.btnExportTemplate.Name = "btnExportTemplate";
            this.btnExportTemplate.Size = new System.Drawing.Size(164, 23);
            this.btnExportTemplate.TabIndex = 3;
            this.btnExportTemplate.Text = "导出模板";
            this.btnExportTemplate.UseVisualStyleBackColor = true;
            this.btnExportTemplate.Click += new System.EventHandler(this.btnExportTemplate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnExportAllToOne);
            this.groupBox1.Controls.Add(this.btnExportTemplate);
            this.groupBox1.Controls.Add(this.btnExportPath);
            this.groupBox1.Location = new System.Drawing.Point(19, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(211, 117);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "配置文件(AD_*)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "表名";
            // 
            // txtDbTableName
            // 
            this.txtDbTableName.Location = new System.Drawing.Point(56, 57);
            this.txtDbTableName.Multiline = true;
            this.txtDbTableName.Name = "txtDbTableName";
            this.txtDbTableName.Size = new System.Drawing.Size(130, 23);
            this.txtDbTableName.TabIndex = 4;
            // 
            // btnExportData
            // 
            this.btnExportData.Location = new System.Drawing.Point(22, 86);
            this.btnExportData.Name = "btnExportData";
            this.btnExportData.Size = new System.Drawing.Size(164, 23);
            this.btnExportData.TabIndex = 3;
            this.btnExportData.Text = "导出单一表";
            this.btnExportData.UseVisualStyleBackColor = true;
            this.btnExportData.Click += new System.EventHandler(this.btnExportData_Click);
            // 
            // btnExcuteScript
            // 
            this.btnExcuteScript.Location = new System.Drawing.Point(19, 20);
            this.btnExcuteScript.Name = "btnExcuteScript";
            this.btnExcuteScript.Size = new System.Drawing.Size(164, 23);
            this.btnExcuteScript.TabIndex = 6;
            this.btnExcuteScript.Text = "执行脚本";
            this.btnExcuteScript.UseVisualStyleBackColor = true;
            this.btnExcuteScript.Click += new System.EventHandler(this.btnExcuteScript_Click);
            // 
            // btnExportViews
            // 
            this.btnExportViews.Location = new System.Drawing.Point(22, 20);
            this.btnExportViews.Name = "btnExportViews";
            this.btnExportViews.Size = new System.Drawing.Size(164, 23);
            this.btnExportViews.TabIndex = 7;
            this.btnExportViews.Text = "导出视图过程函数触发器";
            this.btnExportViews.UseVisualStyleBackColor = true;
            this.btnExportViews.Click += new System.EventHandler(this.btnExportViews_Click);
            // 
            // btnBackup
            // 
            this.btnBackup.Location = new System.Drawing.Point(41, 269);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(164, 23);
            this.btnBackup.TabIndex = 8;
            this.btnBackup.Text = "备份（AD_*, 视图等）";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnExportData);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.btnExportViews);
            this.groupBox3.Controls.Add(this.txtDbTableName);
            this.groupBox3.Location = new System.Drawing.Point(19, 143);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(211, 120);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "数据库";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.btnBackup);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(29, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(265, 318);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "导出";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnImportFolder);
            this.groupBox4.Controls.Add(this.btnDeleteData);
            this.groupBox4.Controls.Add(this.btnImport);
            this.groupBox4.Location = new System.Drawing.Point(329, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(211, 137);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "导入";
            // 
            // btnImportFolder
            // 
            this.btnImportFolder.Location = new System.Drawing.Point(25, 49);
            this.btnImportFolder.Name = "btnImportFolder";
            this.btnImportFolder.Size = new System.Drawing.Size(164, 23);
            this.btnImportFolder.TabIndex = 4;
            this.btnImportFolder.Text = "批量导入数据";
            this.btnImportFolder.UseVisualStyleBackColor = true;
            this.btnImportFolder.Click += new System.EventHandler(this.btnImportFolder_Click);
            // 
            // btnDeleteData
            // 
            this.btnDeleteData.Location = new System.Drawing.Point(25, 78);
            this.btnDeleteData.Name = "btnDeleteData";
            this.btnDeleteData.Size = new System.Drawing.Size(164, 23);
            this.btnDeleteData.TabIndex = 3;
            this.btnDeleteData.Text = "删除数据";
            this.btnDeleteData.UseVisualStyleBackColor = true;
            this.btnDeleteData.Click += new System.EventHandler(this.btnDeleteData_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.ckbFKSelection);
            this.groupBox5.Controls.Add(this.btnEnableFK);
            this.groupBox5.Controls.Add(this.btnDisableFK);
            this.groupBox5.Controls.Add(this.btnTestView);
            this.groupBox5.Controls.Add(this.btnExcuteScript);
            this.groupBox5.Location = new System.Drawing.Point(329, 182);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(211, 148);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "其他";
            // 
            // ckbFKSelection
            // 
            this.ckbFKSelection.AutoSize = true;
            this.ckbFKSelection.Checked = true;
            this.ckbFKSelection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbFKSelection.Location = new System.Drawing.Point(70, 115);
            this.ckbFKSelection.Name = "ckbFKSelection";
            this.ckbFKSelection.Size = new System.Drawing.Size(72, 16);
            this.ckbFKSelection.TabIndex = 10;
            this.ckbFKSelection.Text = "only AD_";
            this.ckbFKSelection.UseVisualStyleBackColor = true;
            // 
            // btnEnableFK
            // 
            this.btnEnableFK.Location = new System.Drawing.Point(114, 88);
            this.btnEnableFK.Name = "btnEnableFK";
            this.btnEnableFK.Size = new System.Drawing.Size(89, 23);
            this.btnEnableFK.TabIndex = 9;
            this.btnEnableFK.Text = "Enable外键";
            this.btnEnableFK.UseVisualStyleBackColor = true;
            this.btnEnableFK.Click += new System.EventHandler(this.btnEnableFK_Click);
            // 
            // btnDisableFK
            // 
            this.btnDisableFK.Location = new System.Drawing.Point(19, 88);
            this.btnDisableFK.Name = "btnDisableFK";
            this.btnDisableFK.Size = new System.Drawing.Size(89, 23);
            this.btnDisableFK.TabIndex = 8;
            this.btnDisableFK.Text = "Disable外键";
            this.btnDisableFK.UseVisualStyleBackColor = true;
            this.btnDisableFK.Click += new System.EventHandler(this.btnDisableFK_Click);
            // 
            // btnTestView
            // 
            this.btnTestView.Location = new System.Drawing.Point(19, 54);
            this.btnTestView.Name = "btnTestView";
            this.btnTestView.Size = new System.Drawing.Size(164, 23);
            this.btnTestView.TabIndex = 7;
            this.btnTestView.Text = "测试视图";
            this.btnTestView.UseVisualStyleBackColor = true;
            this.btnTestView.Click += new System.EventHandler(this.btnTestView_Click);
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Location = new System.Drawing.Point(27, 337);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(101, 12);
            this.lblServerName.TabIndex = 13;
            this.lblServerName.Text = "ConnectionString";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnUnInstallModule);
            this.groupBox6.Controls.Add(this.btnInstallModule);
            this.groupBox6.Controls.Add(this.btnPackgeModuleAccoringInfo);
            this.groupBox6.Controls.Add(this.txtModuleName);
            this.groupBox6.Controls.Add(this.btnModuleGenerateInfo);
            this.groupBox6.Controls.Add(this.btnPackageCurrentDb);
            this.groupBox6.Location = new System.Drawing.Point(556, 13);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(203, 194);
            this.groupBox6.TabIndex = 14;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Modules";
            // 
            // btnUnInstallModule
            // 
            this.btnUnInstallModule.Location = new System.Drawing.Point(19, 164);
            this.btnUnInstallModule.Name = "btnUnInstallModule";
            this.btnUnInstallModule.Size = new System.Drawing.Size(164, 23);
            this.btnUnInstallModule.TabIndex = 8;
            this.btnUnInstallModule.Text = "反安装Module";
            this.btnUnInstallModule.UseVisualStyleBackColor = true;
            this.btnUnInstallModule.Click += new System.EventHandler(this.btnUnInstallModule_Click);
            // 
            // btnInstallModule
            // 
            this.btnInstallModule.Location = new System.Drawing.Point(19, 135);
            this.btnInstallModule.Name = "btnInstallModule";
            this.btnInstallModule.Size = new System.Drawing.Size(164, 23);
            this.btnInstallModule.TabIndex = 7;
            this.btnInstallModule.Text = "安装Module";
            this.btnInstallModule.UseVisualStyleBackColor = true;
            this.btnInstallModule.Click += new System.EventHandler(this.btnInstallModule_Click);
            // 
            // btnPackgeModuleAccoringInfo
            // 
            this.btnPackgeModuleAccoringInfo.Location = new System.Drawing.Point(19, 106);
            this.btnPackgeModuleAccoringInfo.Name = "btnPackgeModuleAccoringInfo";
            this.btnPackgeModuleAccoringInfo.Size = new System.Drawing.Size(164, 23);
            this.btnPackgeModuleAccoringInfo.TabIndex = 6;
            this.btnPackgeModuleAccoringInfo.Text = "根据AD_Module生成Module";
            this.btnPackgeModuleAccoringInfo.UseVisualStyleBackColor = true;
            this.btnPackgeModuleAccoringInfo.Click += new System.EventHandler(this.btnPackgeModuleAccoringInfo_Click);
            // 
            // txtModuleName
            // 
            this.txtModuleName.Location = new System.Drawing.Point(19, 19);
            this.txtModuleName.Multiline = true;
            this.txtModuleName.Name = "txtModuleName";
            this.txtModuleName.Size = new System.Drawing.Size(164, 23);
            this.txtModuleName.TabIndex = 5;
            this.txtModuleName.Text = "HdCx";
            // 
            // btnModuleGenerateInfo
            // 
            this.btnModuleGenerateInfo.Location = new System.Drawing.Point(19, 77);
            this.btnModuleGenerateInfo.Name = "btnModuleGenerateInfo";
            this.btnModuleGenerateInfo.Size = new System.Drawing.Size(164, 23);
            this.btnModuleGenerateInfo.TabIndex = 4;
            this.btnModuleGenerateInfo.Text = "生成默认Info";
            this.btnModuleGenerateInfo.UseVisualStyleBackColor = true;
            this.btnModuleGenerateInfo.Click += new System.EventHandler(this.btnModuleGenerateInfo_Click);
            // 
            // btnPackageCurrentDb
            // 
            this.btnPackageCurrentDb.Location = new System.Drawing.Point(19, 48);
            this.btnPackageCurrentDb.Name = "btnPackageCurrentDb";
            this.btnPackageCurrentDb.Size = new System.Drawing.Size(164, 23);
            this.btnPackageCurrentDb.TabIndex = 3;
            this.btnPackageCurrentDb.Text = "当前库打包";
            this.btnPackageCurrentDb.UseVisualStyleBackColor = true;
            this.btnPackageCurrentDb.Click += new System.EventHandler(this.btnPackageCurrentDb_Click);
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(29, 353);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(511, 21);
            this.txtConnectionString.TabIndex = 15;
            // 
            // btnUpdateConnectionString
            // 
            this.btnUpdateConnectionString.Location = new System.Drawing.Point(556, 353);
            this.btnUpdateConnectionString.Name = "btnUpdateConnectionString";
            this.btnUpdateConnectionString.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateConnectionString.TabIndex = 16;
            this.btnUpdateConnectionString.Text = "更新";
            this.btnUpdateConnectionString.UseVisualStyleBackColor = true;
            this.btnUpdateConnectionString.Click += new System.EventHandler(this.btnUpdateConnectionString_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnGenerateHbm);
            this.groupBox7.Controls.Add(this.btnSchemaUpdate);
            this.groupBox7.Controls.Add(this.btnSchemaExport);
            this.groupBox7.Location = new System.Drawing.Point(782, 13);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(158, 120);
            this.groupBox7.TabIndex = 17;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Schema";
            // 
            // btnGenerateHbm
            // 
            this.btnGenerateHbm.Location = new System.Drawing.Point(18, 77);
            this.btnGenerateHbm.Name = "btnGenerateHbm";
            this.btnGenerateHbm.Size = new System.Drawing.Size(75, 23);
            this.btnGenerateHbm.TabIndex = 2;
            this.btnGenerateHbm.Text = "Hbm";
            this.btnGenerateHbm.UseVisualStyleBackColor = true;
            this.btnGenerateHbm.Click += new System.EventHandler(this.btnGenerateHbm_Click);
            // 
            // btnSchemaUpdate
            // 
            this.btnSchemaUpdate.Location = new System.Drawing.Point(18, 48);
            this.btnSchemaUpdate.Name = "btnSchemaUpdate";
            this.btnSchemaUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnSchemaUpdate.TabIndex = 1;
            this.btnSchemaUpdate.Text = "Update";
            this.btnSchemaUpdate.UseVisualStyleBackColor = true;
            this.btnSchemaUpdate.Click += new System.EventHandler(this.btnSchemaUpdate_Click);
            // 
            // btnSchemaExport
            // 
            this.btnSchemaExport.Location = new System.Drawing.Point(18, 18);
            this.btnSchemaExport.Name = "btnSchemaExport";
            this.btnSchemaExport.Size = new System.Drawing.Size(75, 23);
            this.btnSchemaExport.TabIndex = 0;
            this.btnSchemaExport.Text = "All";
            this.btnSchemaExport.UseVisualStyleBackColor = true;
            this.btnSchemaExport.Click += new System.EventHandler(this.btnSchemaExport_Click);
            // 
            // btnOpenConfig
            // 
            this.btnOpenConfig.Location = new System.Drawing.Point(651, 353);
            this.btnOpenConfig.Name = "btnOpenConfig";
            this.btnOpenConfig.Size = new System.Drawing.Size(75, 23);
            this.btnOpenConfig.TabIndex = 18;
            this.btnOpenConfig.Text = "打开config";
            this.btnOpenConfig.UseVisualStyleBackColor = true;
            this.btnOpenConfig.Click += new System.EventHandler(this.btnOpenConfig_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.btnGenerateHelpHtml);
            this.groupBox8.Controls.Add(this.btnGenerateHelpXml);
            this.groupBox8.Location = new System.Drawing.Point(782, 139);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(158, 66);
            this.groupBox8.TabIndex = 19;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Help";
            // 
            // btnGenerateHelpHtml
            // 
            this.btnGenerateHelpHtml.Location = new System.Drawing.Point(18, 43);
            this.btnGenerateHelpHtml.Name = "btnGenerateHelpHtml";
            this.btnGenerateHelpHtml.Size = new System.Drawing.Size(109, 23);
            this.btnGenerateHelpHtml.TabIndex = 1;
            this.btnGenerateHelpHtml.Text = "Generate HTML";
            this.btnGenerateHelpHtml.UseVisualStyleBackColor = true;
            this.btnGenerateHelpHtml.Click += new System.EventHandler(this.btnGenerateHelpHtml_Click);
            // 
            // btnGenerateHelpXml
            // 
            this.btnGenerateHelpXml.Location = new System.Drawing.Point(18, 16);
            this.btnGenerateHelpXml.Name = "btnGenerateHelpXml";
            this.btnGenerateHelpXml.Size = new System.Drawing.Size(109, 23);
            this.btnGenerateHelpXml.TabIndex = 0;
            this.btnGenerateHelpXml.Text = "Generate XML";
            this.btnGenerateHelpXml.UseVisualStyleBackColor = true;
            this.btnGenerateHelpXml.Click += new System.EventHandler(this.btnGenerateHelpXml_Click);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.btnGenerateAdInfos2);
            this.groupBox9.Controls.Add(this.label3);
            this.groupBox9.Controls.Add(this.txtAdInfoCreatorGridName);
            this.groupBox9.Controls.Add(this.label2);
            this.groupBox9.Controls.Add(this.btnClearDefaultSettings);
            this.groupBox9.Controls.Add(this.btnCreateDefaultSettings);
            this.groupBox9.Controls.Add(this.txtAdInfoCreatorType);
            this.groupBox9.Controls.Add(this.btnGenerateAdInfos);
            this.groupBox9.Location = new System.Drawing.Point(556, 215);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(384, 115);
            this.groupBox9.TabIndex = 20;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "AdInfoCreator";
            // 
            // btnGenerateAdInfos2
            // 
            this.btnGenerateAdInfos2.Location = new System.Drawing.Point(128, 86);
            this.btnGenerateAdInfos2.Name = "btnGenerateAdInfos2";
            this.btnGenerateAdInfos2.Size = new System.Drawing.Size(75, 23);
            this.btnGenerateAdInfos2.TabIndex = 12;
            this.btnGenerateAdInfos2.Text = "GenerateWS";
            this.btnGenerateAdInfos2.UseVisualStyleBackColor = true;
            this.btnGenerateAdInfos2.Click += new System.EventHandler(this.btnGenerateAdInfos2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "GridName";
            // 
            // txtAdInfoCreatorGridName
            // 
            this.txtAdInfoCreatorGridName.Location = new System.Drawing.Point(65, 55);
            this.txtAdInfoCreatorGridName.Multiline = true;
            this.txtAdInfoCreatorGridName.Name = "txtAdInfoCreatorGridName";
            this.txtAdInfoCreatorGridName.Size = new System.Drawing.Size(136, 23);
            this.txtAdInfoCreatorGridName.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "Type";
            // 
            // btnClearDefaultSettings
            // 
            this.btnClearDefaultSettings.Location = new System.Drawing.Point(227, 66);
            this.btnClearDefaultSettings.Name = "btnClearDefaultSettings";
            this.btnClearDefaultSettings.Size = new System.Drawing.Size(151, 23);
            this.btnClearDefaultSettings.TabIndex = 8;
            this.btnClearDefaultSettings.Text = "清除默认配置信息";
            this.btnClearDefaultSettings.UseVisualStyleBackColor = true;
            this.btnClearDefaultSettings.Click += new System.EventHandler(this.btnClearDefaultSettings_Click);
            // 
            // btnCreateDefaultSettings
            // 
            this.btnCreateDefaultSettings.Location = new System.Drawing.Point(226, 26);
            this.btnCreateDefaultSettings.Name = "btnCreateDefaultSettings";
            this.btnCreateDefaultSettings.Size = new System.Drawing.Size(151, 23);
            this.btnCreateDefaultSettings.TabIndex = 7;
            this.btnCreateDefaultSettings.Text = "创建默认配置信息窗口";
            this.btnCreateDefaultSettings.UseVisualStyleBackColor = true;
            this.btnCreateDefaultSettings.Click += new System.EventHandler(this.btnCreateDefaultSettings_Click);
            // 
            // txtAdInfoCreatorType
            // 
            this.txtAdInfoCreatorType.Location = new System.Drawing.Point(67, 26);
            this.txtAdInfoCreatorType.Multiline = true;
            this.txtAdInfoCreatorType.Name = "txtAdInfoCreatorType";
            this.txtAdInfoCreatorType.Size = new System.Drawing.Size(136, 23);
            this.txtAdInfoCreatorType.TabIndex = 6;
            // 
            // btnGenerateAdInfos
            // 
            this.btnGenerateAdInfos.Location = new System.Drawing.Point(21, 84);
            this.btnGenerateAdInfos.Name = "btnGenerateAdInfos";
            this.btnGenerateAdInfos.Size = new System.Drawing.Size(75, 23);
            this.btnGenerateAdInfos.TabIndex = 0;
            this.btnGenerateAdInfos.Text = "Generate";
            this.btnGenerateAdInfos.UseVisualStyleBackColor = true;
            this.btnGenerateAdInfos.Click += new System.EventHandler(this.btnGenerateAdInfos_Click);
            // 
            // btnPythonScript
            // 
            this.btnPythonScript.Location = new System.Drawing.Point(800, 353);
            this.btnPythonScript.Name = "btnPythonScript";
            this.btnPythonScript.Size = new System.Drawing.Size(134, 23);
            this.btnPythonScript.TabIndex = 21;
            this.btnPythonScript.Text = "PythonScript";
            this.btnPythonScript.UseVisualStyleBackColor = true;
            this.btnPythonScript.Click += new System.EventHandler(this.btnPythonScript_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 409);
            this.Controls.Add(this.btnPythonScript);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.btnOpenConfig);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.btnUpdateConnectionString);
            this.Controls.Add(this.txtConnectionString);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.lblServerName);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AdInfosUtil";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExportAllToOne;
        private System.Windows.Forms.Button btnExportPath;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExportTemplate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtDbTableName;
        private System.Windows.Forms.Button btnExportData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExcuteScript;
        private System.Windows.Forms.Button btnExportViews;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnTestView;
        private System.Windows.Forms.Button btnDeleteData;
        private System.Windows.Forms.Button btnEnableFK;
        private System.Windows.Forms.Button btnDisableFK;
        private System.Windows.Forms.CheckBox ckbFKSelection;
        private System.Windows.Forms.Button btnImportFolder;
        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnPackageCurrentDb;
        private System.Windows.Forms.TextBox txtModuleName;
        private System.Windows.Forms.Button btnModuleGenerateInfo;
        private System.Windows.Forms.Button btnPackgeModuleAccoringInfo;
        private System.Windows.Forms.Button btnUnInstallModule;
        private System.Windows.Forms.Button btnInstallModule;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Button btnUpdateConnectionString;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btnSchemaUpdate;
        private System.Windows.Forms.Button btnSchemaExport;
        private System.Windows.Forms.Button btnGenerateHbm;
        private System.Windows.Forms.Button btnOpenConfig;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button btnGenerateHelpXml;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.TextBox txtAdInfoCreatorType;
        private System.Windows.Forms.Button btnGenerateAdInfos;
        private System.Windows.Forms.Button btnClearDefaultSettings;
        private System.Windows.Forms.Button btnCreateDefaultSettings;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAdInfoCreatorGridName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGenerateHelpHtml;
        private System.Windows.Forms.Button btnGenerateAdInfos2;
        private System.Windows.Forms.Button btnPythonScript;
    }
}

