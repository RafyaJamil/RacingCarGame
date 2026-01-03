namespace Game
{
    partial class SelectForm
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
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
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
            label1.Text = "Select Level";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            button1.BackColor = Color.Green;
            button1.Cursor = Cursors.Hand;
            button1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            button1.ForeColor = Color.White;
            button1.Location = new Point(100, 200);
            button1.Name = "button1";
            button1.Size = new Size(200, 60);
            button1.TabIndex = 1;
            button1.Text = "Level 1";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.Green;
            button2.Cursor = Cursors.Hand;
            button2.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            button2.ForeColor = Color.White;
            button2.Location = new Point(300, 200);
            button2.Name = "button2";
            button2.Size = new Size(200, 60);
            button2.TabIndex = 2;
            button2.Text = "Level 2";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.Green;
            button3.Cursor = Cursors.Hand;
            button3.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            button3.ForeColor = Color.White;
            button3.Location = new Point(500, 200);
            button3.Name = "button3";
            button3.Size = new Size(200, 60);
            button3.TabIndex = 3;
            button3.Text = "Level 3";
            button3.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            button4.BackColor = Color.Red;
            button4.Cursor = Cursors.Hand;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button4.ForeColor = Color.White;
            button4.Location = new Point(50, 450);
            button4.Name = "button4";
            button4.Size = new Size(100, 56);
            button4.TabIndex = 4;
            button4.Text = "Back";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // SelectForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.instructions;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(778, 544);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "SelectForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Select Level";
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
    }
}