namespace Game
{
    partial class InstructionsForm
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
            label2 = new Label();
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(50, 30);
            label1.Name = "label1";
            label1.Size = new Size(700, 50);
            label1.TabIndex = 0;
            label1.Text = "How to Play";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 14F);
            label2.Location = new Point(50, 100);
            label2.Name = "label2";
            label2.Size = new Size(700, 200);
            label2.TabIndex = 1;
            label2.Text = "- Use Arrow keys to move the car\r\n- Avoid enemy cars to survive\r\n- Collect fuel to keep going\r\n- Score points by surviving longer\r\n- Reach the maximum score to complete the level";
            // 
            // button1
            // 
            button1.BackColor = Color.DarkGreen;
            button1.Cursor = Cursors.Hand;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            button1.Location = new Point(300, 400);
            button1.Name = "button1";
            button1.Size = new Size(200, 50);
            button1.TabIndex = 2;
            button1.Text = "Start Game";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.Red;
            button2.Cursor = Cursors.Hand;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button2.Location = new Point(10, 10);
            button2.Name = "button2";
            button2.Size = new Size(100, 40);
            button2.TabIndex = 3;
            button2.Text = "Back";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // InstructionsForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonFace;
            BackgroundImage = Properties.Resources.instructions;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(778, 544);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            ForeColor = Color.White;
            Name = "InstructionsForm";
            Text = "Instructions";
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Label label2;
        private Button button1;
        private Button button2;
    }
}