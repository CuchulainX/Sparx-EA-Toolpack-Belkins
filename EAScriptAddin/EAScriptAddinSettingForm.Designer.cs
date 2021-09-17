﻿/*
 * Created by SharpDevelop.
 * User: Geert
 * Date: 26/10/2014
 * Time: 6:59
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace EAScriptAddin
{
	partial class EAScriptAddinSettingForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EAScriptAddinSettingForm));
            this.operationsPanel = new System.Windows.Forms.Panel();
            this.addFunctionButton = new System.Windows.Forms.Button();
            this.defaultScriptLabel = new System.Windows.Forms.Label();
            this.ScriptCombo = new System.Windows.Forms.ComboBox();
            this.allOperationsCheckBox = new System.Windows.Forms.CheckBox();
            this.functionsListBox = new System.Windows.Forms.CheckedListBox();
            this.operationsListBox = new System.Windows.Forms.CheckedListBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.developerModeCheckBox = new System.Windows.Forms.CheckBox();
            this.scriptPathLabel = new System.Windows.Forms.Label();
            this.scriptPathTextBox = new System.Windows.Forms.TextBox();
            this.scriptPathSelectButton = new System.Windows.Forms.Button();
            this.operationsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // operationsPanel
            // 
            this.operationsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.operationsPanel.Controls.Add(this.addFunctionButton);
            this.operationsPanel.Controls.Add(this.defaultScriptLabel);
            this.operationsPanel.Controls.Add(this.ScriptCombo);
            this.operationsPanel.Controls.Add(this.allOperationsCheckBox);
            this.operationsPanel.Controls.Add(this.functionsListBox);
            this.operationsPanel.Controls.Add(this.operationsListBox);
            this.operationsPanel.Location = new System.Drawing.Point(5, 12);
            this.operationsPanel.Name = "operationsPanel";
            this.operationsPanel.Size = new System.Drawing.Size(574, 402);
            this.operationsPanel.TabIndex = 0;
            // 
            // addFunctionButton
            // 
            this.addFunctionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addFunctionButton.Enabled = false;
            this.addFunctionButton.Location = new System.Drawing.Point(481, 197);
            this.addFunctionButton.Name = "addFunctionButton";
            this.addFunctionButton.Size = new System.Drawing.Size(86, 23);
            this.addFunctionButton.TabIndex = 5;
            this.addFunctionButton.Text = "Add Function";
            this.addFunctionButton.UseVisualStyleBackColor = true;
            this.addFunctionButton.Click += new System.EventHandler(this.AddFunctionButtonClick);
            // 
            // defaultScriptLabel
            // 
            this.defaultScriptLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.defaultScriptLabel.Location = new System.Drawing.Point(267, 180);
            this.defaultScriptLabel.Name = "defaultScriptLabel";
            this.defaultScriptLabel.Size = new System.Drawing.Size(100, 16);
            this.defaultScriptLabel.TabIndex = 4;
            this.defaultScriptLabel.Text = "Script";
            this.defaultScriptLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // ScriptCombo
            // 
            this.ScriptCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ScriptCombo.DropDownHeight = 107;
            this.ScriptCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ScriptCombo.FormattingEnabled = true;
            this.ScriptCombo.IntegralHeight = false;
            this.ScriptCombo.Location = new System.Drawing.Point(266, 199);
            this.ScriptCombo.Name = "ScriptCombo";
            this.ScriptCombo.Size = new System.Drawing.Size(209, 21);
            this.ScriptCombo.TabIndex = 3;
            this.ScriptCombo.SelectedIndexChanged += new System.EventHandler(this.ScriptComboSelectedIndexChanged);
            // 
            // allOperationsCheckBox
            // 
            this.allOperationsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.allOperationsCheckBox.Checked = true;
            this.allOperationsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.allOperationsCheckBox.Location = new System.Drawing.Point(4, 373);
            this.allOperationsCheckBox.Name = "allOperationsCheckBox";
            this.allOperationsCheckBox.Size = new System.Drawing.Size(157, 24);
            this.allOperationsCheckBox.TabIndex = 2;
            this.allOperationsCheckBox.Text = "Show all operations";
            this.allOperationsCheckBox.UseVisualStyleBackColor = true;
            this.allOperationsCheckBox.CheckedChanged += new System.EventHandler(this.AllOperationsCheckBoxCheckedChanged);
            // 
            // functionsListBox
            // 
            this.functionsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.functionsListBox.FormattingEnabled = true;
            this.functionsListBox.Location = new System.Drawing.Point(266, 4);
            this.functionsListBox.Name = "functionsListBox";
            this.functionsListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.functionsListBox.Size = new System.Drawing.Size(305, 169);
            this.functionsListBox.TabIndex = 1;
            // 
            // operationsListBox
            // 
            this.operationsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.operationsListBox.FormattingEnabled = true;
            this.operationsListBox.Location = new System.Drawing.Point(4, 4);
            this.operationsListBox.Name = "operationsListBox";
            this.operationsListBox.Size = new System.Drawing.Size(257, 349);
            this.operationsListBox.TabIndex = 0;
            this.operationsListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OperationsListBoxItemCheck);
            this.operationsListBox.SelectedIndexChanged += new System.EventHandler(this.OperationsListBoxSelectedIndexChanged);
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.Location = new System.Drawing.Point(504, 487);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(423, 487);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // developerModeCheckBox
            // 
            this.developerModeCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.developerModeCheckBox.AutoSize = true;
            this.developerModeCheckBox.Location = new System.Drawing.Point(9, 420);
            this.developerModeCheckBox.Name = "developerModeCheckBox";
            this.developerModeCheckBox.Size = new System.Drawing.Size(105, 17);
            this.developerModeCheckBox.TabIndex = 3;
            this.developerModeCheckBox.Text = "Developer Mode";
            this.developerModeCheckBox.UseVisualStyleBackColor = true;
            this.developerModeCheckBox.CheckedChanged += new System.EventHandler(this.developerModeCheckBox_CheckedChanged);
            // 
            // scriptPathLabel
            // 
            this.scriptPathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scriptPathLabel.AutoSize = true;
            this.scriptPathLabel.Location = new System.Drawing.Point(6, 440);
            this.scriptPathLabel.Name = "scriptPathLabel";
            this.scriptPathLabel.Size = new System.Drawing.Size(59, 13);
            this.scriptPathLabel.TabIndex = 4;
            this.scriptPathLabel.Text = "Script Path";
            // 
            // scriptPathTextBox
            // 
            this.scriptPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptPathTextBox.Location = new System.Drawing.Point(5, 456);
            this.scriptPathTextBox.Name = "scriptPathTextBox";
            this.scriptPathTextBox.Size = new System.Drawing.Size(538, 20);
            this.scriptPathTextBox.TabIndex = 5;
            // 
            // scriptPathSelectButton
            // 
            this.scriptPathSelectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptPathSelectButton.Location = new System.Drawing.Point(549, 454);
            this.scriptPathSelectButton.Name = "scriptPathSelectButton";
            this.scriptPathSelectButton.Size = new System.Drawing.Size(30, 23);
            this.scriptPathSelectButton.TabIndex = 6;
            this.scriptPathSelectButton.Text = "...";
            this.scriptPathSelectButton.UseVisualStyleBackColor = true;
            this.scriptPathSelectButton.Click += new System.EventHandler(this.scriptPathSelectButton_Click);
            // 
            // EAScriptAddinSettingForm
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(591, 522);
            this.Controls.Add(this.scriptPathSelectButton);
            this.Controls.Add(this.scriptPathTextBox);
            this.Controls.Add(this.scriptPathLabel);
            this.Controls.Add(this.developerModeCheckBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.operationsPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(607, 490);
            this.Name = "EAScriptAddinSettingForm";
            this.Text = "Settings";
            this.operationsPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Button addFunctionButton;
		private System.Windows.Forms.ComboBox ScriptCombo;
		private System.Windows.Forms.Label defaultScriptLabel;
		private System.Windows.Forms.CheckBox allOperationsCheckBox;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button OkButton;
		private System.Windows.Forms.CheckedListBox operationsListBox;
		private System.Windows.Forms.CheckedListBox functionsListBox;
		private System.Windows.Forms.Panel operationsPanel;
		private System.Windows.Forms.CheckBox developerModeCheckBox;
        private System.Windows.Forms.Label scriptPathLabel;
        private System.Windows.Forms.TextBox scriptPathTextBox;
        private System.Windows.Forms.Button scriptPathSelectButton;
    }
}
