namespace Game
{
    partial class PlayerInfoForm
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
            label1 = new Label();
            textBox1 = new TextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(50, 50);
            label1.Name = "label1";
            label1.Size = new Size(700, 50);
            label1.TabIndex = 0;
            label1.Text = "Enter Your Name";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 16F);
            textBox1.Location = new Point(200, 150);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(400, 50);
            textBox1.TabIndex = 1;
            // 
            // button1
            // 
            button1.BackColor = Color.DarkGreen;
            button1.Cursor = Cursors.Hand;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            button1.ForeColor = Color.White;
            button1.Location = new Point(300, 400);
            button1.Name = "button1";
            button1.Size = new Size(200, 50);
            button1.TabIndex = 2;
            button1.Text = "Continue";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // PlayerInfoForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.instructions;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(778, 544);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Name = "PlayerInfoForm";
            Text = "Player Information";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private Button button1;
    }
}