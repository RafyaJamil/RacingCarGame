using Game.Audios;
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
    public partial class SelectForm : Form
    {
        public SelectForm()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            InstructionsForm instructionsForm = new InstructionsForm();
            instructionsForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            RacingCar c = new RacingCar();
            c.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Level2Form level2 = new Level2Form();
            level2.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Level3Form level3 = new Level3Form();
            level3.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AudioManager.StopAll();   // music بند
            Application.Exit();
        }
    }
}
