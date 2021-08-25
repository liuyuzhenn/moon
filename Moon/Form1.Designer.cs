
namespace Moon
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orbitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(676, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingToolStripMenuItem
            // 
            this.settingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moonToolStripMenuItem,
            this.orbitToolStripMenuItem,
            this.cameraToolStripMenuItem});
            this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            this.settingToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            this.settingToolStripMenuItem.Text = "Setting";
            // 
            // moonToolStripMenuItem
            // 
            this.moonToolStripMenuItem.Name = "moonToolStripMenuItem";
            this.moonToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.moonToolStripMenuItem.Text = "Moon";
            this.moonToolStripMenuItem.Click += new System.EventHandler(this.moonToolStripMenuItem_Click);
            // 
            // orbitToolStripMenuItem
            // 
            this.orbitToolStripMenuItem.Name = "orbitToolStripMenuItem";
            this.orbitToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.orbitToolStripMenuItem.Text = "Orbit";
            this.orbitToolStripMenuItem.Click += new System.EventHandler(this.orbitToolStripMenuItem_Click);
            // 
            // cameraToolStripMenuItem
            // 
            this.cameraToolStripMenuItem.Name = "cameraToolStripMenuItem";
            this.cameraToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.cameraToolStripMenuItem.Text = "Camera";
            this.cameraToolStripMenuItem.Click += new System.EventHandler(this.cameraToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(298, 313);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(359, 22);
            this.progressBar1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 323);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Null";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 11F);
            this.button1.Location = new System.Drawing.Point(298, 246);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 43);
            this.button1.TabIndex = 3;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 11F);
            this.label2.Location = new System.Drawing.Point(110, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Exposure Interval:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 11F);
            this.label3.Location = new System.Drawing.Point(134, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Exposure times:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 11F);
            this.label4.Location = new System.Drawing.Point(141, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Time to start:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(267, 74);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(276, 21);
            this.textBox1.TabIndex = 8;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(267, 110);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(276, 21);
            this.textBox2.TabIndex = 9;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 9F);
            this.label12.Location = new System.Drawing.Point(528, 157);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(23, 12);
            this.label12.TabIndex = 33;
            this.label12.Text = "sec";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(462, 148);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(60, 21);
            this.textBox9.TabIndex = 32;
            this.textBox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 9F);
            this.label15.Location = new System.Drawing.Point(332, 157);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(29, 12);
            this.label15.TabIndex = 31;
            this.label15.Text = "hour";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 9F);
            this.label14.Location = new System.Drawing.Point(433, 157);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(23, 12);
            this.label14.TabIndex = 30;
            this.label14.Text = "min";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(367, 148);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(60, 21);
            this.textBox8.TabIndex = 29;
            this.textBox8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(266, 148);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(60, 21);
            this.textBox7.TabIndex = 28;
            this.textBox7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(266, 190);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(60, 21);
            this.textBox3.TabIndex = 34;
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 11F);
            this.label5.Location = new System.Drawing.Point(165, 196);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 15);
            this.label5.TabIndex = 35;
            this.label5.Text = "Threashold:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 11F);
            this.label6.Location = new System.Drawing.Point(364, 196);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(119, 15);
            this.label6.TabIndex = 37;
            this.label6.Text = "Max Iteration:";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(483, 190);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(60, 21);
            this.textBox4.TabIndex = 36;
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 353);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orbitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cameraToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    }
}

