
namespace liuyuzhen
{
    partial class MoonSetting
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
            this.labelDOM = new System.Windows.Forms.Label();
            this.labelDEM = new System.Windows.Forms.Label();
            this.pathDOM = new System.Windows.Forms.TextBox();
            this.pathDEM = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // labelDOM
            // 
            this.labelDOM.AutoSize = true;
            this.labelDOM.Font = new System.Drawing.Font("宋体", 11F);
            this.labelDOM.Location = new System.Drawing.Point(34, 58);
            this.labelDOM.Name = "labelDOM";
            this.labelDOM.Size = new System.Drawing.Size(39, 15);
            this.labelDOM.TabIndex = 0;
            this.labelDOM.Text = "DOM:";
            // 
            // labelDEM
            // 
            this.labelDEM.AutoSize = true;
            this.labelDEM.Font = new System.Drawing.Font("宋体", 11F);
            this.labelDEM.Location = new System.Drawing.Point(34, 103);
            this.labelDEM.Name = "labelDEM";
            this.labelDEM.Size = new System.Drawing.Size(39, 15);
            this.labelDEM.TabIndex = 1;
            this.labelDEM.Text = "DEM:";
            // 
            // pathDOM
            // 
            this.pathDOM.Location = new System.Drawing.Point(79, 58);
            this.pathDOM.Name = "pathDOM";
            this.pathDOM.Size = new System.Drawing.Size(303, 21);
            this.pathDOM.TabIndex = 2;
            this.pathDOM.Text = "D:\\刘雨臻\\全月\\DOM\\Mosaci_Moon.tif";
            // 
            // pathDEM
            // 
            this.pathDEM.Location = new System.Drawing.Point(79, 103);
            this.pathDEM.Name = "pathDEM";
            this.pathDEM.Size = new System.Drawing.Size(303, 21);
            this.pathDEM.TabIndex = 3;
            this.pathDEM.Text = "D:\\刘雨臻\\全月\\DEM\\Lunar_LRO_LOLA_Global_LDEM_118m_Mar2014.tif";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(402, 56);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(402, 101);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Browse";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(200, 199);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 37);
            this.button3.TabIndex = 6;
            this.button3.Text = "Confirm";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 11F);
            this.label1.Location = new System.Drawing.Point(76, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Interpolate:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Nearest",
            "Linear"});
            this.comboBox1.Location = new System.Drawing.Point(185, 148);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(148, 20);
            this.comboBox1.TabIndex = 8;
            // 
            // MoonSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 248);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pathDEM);
            this.Controls.Add(this.pathDOM);
            this.Controls.Add(this.labelDEM);
            this.Controls.Add(this.labelDOM);
            this.Name = "MoonSetting";
            this.Text = "MoonSetting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDOM;
        private System.Windows.Forms.Label labelDEM;
        private System.Windows.Forms.TextBox pathDOM;
        private System.Windows.Forms.TextBox pathDEM;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}