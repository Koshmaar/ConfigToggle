using System;
using System.Collections;
using System.Windows.Forms;
using PluginCore;
using WeifenLuo.WinFormsUI;
using ProjectManager.Projects.AS3;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConfigToggle
{

	public class PluginUI : UserControl
    {
        private CheckedListBox config_constants;
		private PluginMain pluginMain;
        
		public PluginUI(PluginMain pluginMain)
		{
			this.InitializeComponent();
			this.pluginMain = pluginMain;
		}


        /*public RichTextBox Output
        {
            get { return this.richTextBox; }
        }*/
		
		#region Windows Forms Designer Generated Code

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() 
        {
            this.config_constants = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // config_constants
            // 
            this.config_constants.CheckOnClick = true;
            this.config_constants.ColumnWidth = 90;
            this.config_constants.Dock = System.Windows.Forms.DockStyle.Fill;
            this.config_constants.FormattingEnabled = true;
            this.config_constants.Location = new System.Drawing.Point(0, 0);
            this.config_constants.MultiColumn = true;
            this.config_constants.Name = "config_constants";
            this.config_constants.Size = new System.Drawing.Size(280, 352);
            this.config_constants.TabIndex = 1;
            this.config_constants.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.config_constants_ItemCheck);
            // 
            // PluginUI
            // 
            this.Controls.Add(this.config_constants);
            this.Name = "PluginUI";
            this.Size = new System.Drawing.Size(280, 352);
            this.ResumeLayout(false);

		}

		#endregion

        public string[] GetCompilerConstants()
        {
            IProject project = PluginBase.CurrentProject;
            if (project == null)
            {
                //Output.Text += "No project loaded";
                return null;
            }

            AS3Project as3_project = project as AS3Project;

            return as3_project.CompilerOptions.CompilerConstants;
        }

        public bool IsConstantCONFIG( string constant )
        {
            return constant.EndsWith("true") || constant.EndsWith("false");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshUI();
        }

        public void RefreshUI()
        {
            string[] constants = GetCompilerConstants();

            string all = "";

            config_constants.Items.Clear();
            config_constants.ItemCheck -= config_constants_ItemCheck;

            foreach (string constant in constants)
            {
                if (IsConstantCONFIG(constant))
                {
                    string key;
                    string value;
                    string conf_namespace;
                    ExtractCONFIGKeyValue(constant, out key, out value, out conf_namespace);

                    //all += "config: >" + key + "<  ]" + value + "[\n";

                    config_constants.Items.Add(key, value == "true");
                }
                //else                    all += " unrecog: " + constant + "\n";
            }

            //Output.Text += "Compiler constants:\n" + all;

            config_constants.ItemCheck += config_constants_ItemCheck;
        }

        // %snamespace::key%s,%svalue
        public Regex config_regex = new Regex(@"\s*(?<namespace>[a-zA-z1-9]*)::(?<key>[a-zA-z1-9]*)\s*,\s*(?<value>[a-zA-z1-9]*)");

        private void ExtractCONFIGKeyValue(string constant, out string key, out string value, out string conf_namespace)
        {
            Match match = config_regex.Match(constant);
            
            key = match.Groups["key"].Value;
            value = match.Groups["value"].Value;
            conf_namespace = match.Groups["namespace"].Value;

            /*Output.Text += " namespace: " + conf_namespace;
            Output.Text += " key: " + key;
            Output.Text += " value: " + value;*/
        }

        private void config_constants_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            System.Windows.Forms.CheckedListBox obj = sender as CheckedListBox;

            //Output.Text += "Clicked " + obj.Text; // +" newvalue " + e.NewValue + "\n";

            IProject project = PluginBase.CurrentProject;
            if (project == null)
            {
                //Output.Text += "No project loaded";
                return;
            }
            AS3Project as3_project = project as AS3Project;

            string[] constants = GetCompilerConstants();

            for (int i = 0; i < constants.GetLength(0); i++)
            {
                //Output.Text += " " + constants[i];

                if (IsConstantCONFIG(constants[i]))
                {
                    string key;
                    string value;
                    string conf_namespace;
                    ExtractCONFIGKeyValue(constants[i], out key, out value, out conf_namespace);

                    if (key == obj.Text) // is this the clicked one
                    {
                        constants[i] = conf_namespace + "::" + key + "," + (e.NewValue == CheckState.Checked ? "true" : "false");
                        //Output.Text += "toggled " + constants[i] + "\n";

                        as3_project.Save();
                        return;
                    }
                }

            
            }


        }
				
 	}

}
