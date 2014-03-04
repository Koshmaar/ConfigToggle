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
        private RichTextBox richTextBox;
        private SplitContainer split;
        private TreeView values;
        private ContextMenuStrip right_click_context;
        private System.ComponentModel.IContainer components;
        private ToolStripTextBox toolStripTextBox1;
		private PluginMain pluginMain;
        
		public PluginUI(PluginMain pluginMain)
		{
			this.InitializeComponent();
			this.pluginMain = pluginMain;
		}


        public RichTextBox Output
        {
            get { return this.richTextBox; }
        }
		
		#region Windows Forms Designer Generated Code

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() 
        {
            this.components = new System.ComponentModel.Container();
            this.config_constants = new System.Windows.Forms.CheckedListBox();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.split = new System.Windows.Forms.SplitContainer();
            this.values = new System.Windows.Forms.TreeView();
            this.right_click_context = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.right_click_context.SuspendLayout();
            this.SuspendLayout();
            // 
            // config_constants
            // 
            this.config_constants.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.config_constants.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.config_constants.CheckOnClick = true;
            this.config_constants.ColumnWidth = 90;
            this.config_constants.Dock = System.Windows.Forms.DockStyle.Fill;
            this.config_constants.FormattingEnabled = true;
            this.config_constants.Location = new System.Drawing.Point(0, 0);
            this.config_constants.MultiColumn = true;
            this.config_constants.Name = "config_constants";
            this.config_constants.Size = new System.Drawing.Size(143, 291);
            this.config_constants.TabIndex = 1;
            this.config_constants.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.config_constants_ItemCheck);
            // 
            // richTextBox
            // 
            this.richTextBox.Location = new System.Drawing.Point(3, 154);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(170, 109);
            this.richTextBox.TabIndex = 2;
            this.richTextBox.Text = "";
            this.richTextBox.Visible = false;
            // 
            // split
            // 
            this.split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split.Location = new System.Drawing.Point(0, 0);
            this.split.Name = "split";
            // 
            // split.Panel1
            // 
            this.split.Panel1.Controls.Add(this.config_constants);
            // 
            // split.Panel2
            // 
            this.split.Panel2.Controls.Add(this.richTextBox);
            this.split.Panel2.Controls.Add(this.values);
            this.split.Size = new System.Drawing.Size(280, 291);
            this.split.SplitterDistance = 143;
            this.split.TabIndex = 5;
            // 
            // values
            // 
            this.values.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.values.Dock = System.Windows.Forms.DockStyle.Fill;
            this.values.Indent = 19;
            this.values.LabelEdit = true;
            this.values.Location = new System.Drawing.Point(0, 0);
            this.values.Name = "values";
            this.values.Size = new System.Drawing.Size(133, 291);
            this.values.TabIndex = 6;
            this.values.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.values_AfterLabelEdit);
            this.values.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.values_NodeMouseClick);
            this.values.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.values_KeyPress);
            // 
            // right_click_context
            // 
            this.right_click_context.Enabled = false;
            this.right_click_context.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1});
            this.right_click_context.Name = "contextMenuStrip1";
            this.right_click_context.Size = new System.Drawing.Size(161, 51);
            this.right_click_context.Text = "Add below";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.AcceptsReturn = true;
            this.toolStripTextBox1.Enabled = false;
            this.toolStripTextBox1.HideSelection = false;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBox1.Text = "Add new config below";
            this.toolStripTextBox1.ToolTipText = "Type new config below and press enter";
            this.toolStripTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.right_click_textbox_handler);
            // 
            // PluginUI
            // 
            this.AutoScroll = true;
            this.ContextMenuStrip = this.right_click_context;
            this.Controls.Add(this.split);
            this.Name = "PluginUI";
            this.Size = new System.Drawing.Size(280, 291);
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel2.ResumeLayout(false);
            this.split.ResumeLayout(false);
            this.right_click_context.ResumeLayout(false);
            this.right_click_context.PerformLayout();
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

        public bool IsConstantVALUE(string constant) // CONFIG::dupa, 123  but it also can be a string value! in both cases input will do
        {
            return constant.IndexOf(',') != -1;

            //return constant.EndsWith("true") || constant.EndsWith("false");
        }

        public void RefreshUI()
        {
            string[] constants = GetCompilerConstants();

            string all = "";

            config_constants.Items.Clear();
            config_constants.ItemCheck -= config_constants_ItemCheck;

            values.Nodes.Clear();

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
                else
                if (IsConstantVALUE(constant))
                {
                    string key;
                    string value;
                    ExtractVALUEKeyValue(constant, out key, out value);

                    TreeNode parent = values.Nodes.Add(key);
                    parent.Nodes.Add(value);
                }
                else
                {
                    all += " unrecog: " + constant + "\n";
                }
                    
            }

            //Output.Text += "Compiler constants:\n" + all;

            values.ExpandAll();
            config_constants.ItemCheck += config_constants_ItemCheck;
        }

        // %snamespace::key%s,%svalue
        public Regex config_regex = new Regex(@"\s*(?<namespace>[a-zA-z1-9]*)::(?<key>[a-zA-z1-9]*)\s*,\s*(?<value>[a-zA-z1-9]*)");


        public Regex value_regex = new Regex(@"\s*(?<key>[a-zA-z1-9:]*)\s*,\s*(?<value>[a-zA-z1-9"" -]*)");

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


        private void ExtractVALUEKeyValue(string constant, out string key, out string value)
        {
            Match match = value_regex.Match(constant);

            key = match.Groups["key"].Value;
            value = match.Groups["value"].Value;
        }

        private void config_constants_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //Output.Text += "Clicked " + obj.Text; // +" newvalue " + e.NewValue + "\n";

            IProject project = PluginBase.CurrentProject;
            if (project == null)
            {
                //Output.Text += "No project loaded";
                return;
            }
            AS3Project as3_project = project as AS3Project;

            string[] constants = GetCompilerConstants();
            System.Windows.Forms.CheckedListBox obj = sender as CheckedListBox;

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


        private void values_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            e.Node.BeginEdit();
        }

        private void values_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (values.SelectedNode != null && !values.SelectedNode.IsEditing)
                    values.SelectedNode.BeginEdit();
            }
        }

        private void values_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.CancelEdit)
                return;

            /*if (e.Node.Parent == null)
                Output.Text += "Parent is null / " + e.Node.Text + "\n";
            else
                Output.Text += e.Node.Parent.Text + " / " + e.Node.Text + "\n";

            return;*/

            IProject project = PluginBase.CurrentProject;
            if (project == null)
            {
                //Output.Text += "No project loaded";
                return;
            }
            AS3Project as3_project = project as AS3Project;

            string[] constants = GetCompilerConstants();

            string old_key = "";
            string new_key = "";
            string new_value = null;

            if (e.Node.Parent == null) // edited the CONFIG::name part, so change the key
            {
                old_key = e.Node.Text;
                new_key = e.Label;
                //new_value shall be taken from

                if (old_key == new_key)
                    return;
            }
            else // edited the value
            {
                old_key = e.Node.Parent.Text;
                new_key = old_key;
                new_value = e.Label;
                //Output.Text += e.Node.Parent.Text + " / " + e.Node.Text;
            }            

            for (int i = 0; i < constants.GetLength(0); i++)
            {
                //Output.Text += " " + constants[i];

                if (!IsConstantCONFIG(constants[i]) && IsConstantVALUE(constants[i]))
                {
                    string key;
                    string value;
                    ExtractVALUEKeyValue(constants[i], out key, out value);

                    if (key == new_key) // is this the clicked one
                    {
                        if (new_value == null)
                            new_value = value;

                        constants[i] = new_key + "," + new_value;
                        Output.Text += "edited to " + constants[i] + "\n";

                        as3_project.Save();
                        return;
                    }

                }
            }

        }


        private void right_click_textbox_handler(object sender, KeyPressEventArgs e)
        {
            //Output.Text += "key down" + e.KeyChar + "\n";
            /*
            if (e.KeyChar == 13)
            {

                IProject project = PluginBase.CurrentProject;
                if (project == null || !(project is AS3Project))
                {
                    return;
                }
                AS3Project as3_project = project as AS3Project;

                as3_project.CompilerOptions.CompilerConstants. how to add new?

                //contextMenuStrip1.Hide();

                as3_project.Save();

            }
            */
        }

        //this.toolStripTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.right_click_textbox_handler);

				
 	}

}
