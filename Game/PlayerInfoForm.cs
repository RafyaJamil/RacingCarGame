using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class PlayerInfoForm : Form
    {
        public PlayerInfoForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim();

            if (name == "")
            {
                MessageBox.Show("Please enter name");
                return;
            }

            GameSession.PlayerName = name;

            if (FileManager.PlayerExists(name))
            {
                int lastLevel = FileManager.GetPlayerLevel(name);
                OpenNextLevel(lastLevel);
            }
            else
            {
                SelectForm f = new SelectForm();
                f.ShowDialog();
            }

            this.Hide();
        }

        void OpenNextLevel(int lastLevel)
        {
            if (lastLevel == 1)
            {
                Level2Form l2 = new Level2Form();
                l2.Show();
            }
            else if (lastLevel == 2)
            {
                Level3Form l3 = new Level3Form();
                l3.Show();
            }
            else
            {
                MessageBox.Show("You already completed all levels!");
                SelectForm menu = new SelectForm();
                menu.Show();
            }
        }

    }
}
