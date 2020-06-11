using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{

    

    public partial class Guideform : Form
    {

       // private Form1 form1;


        public Guideform()
        {
            InitializeComponent();
        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 singlegame = new Form1();
            singlegame.Show();
            //this.Close();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 netgame = new Form3();
            netgame.Show();
            //this.Close();

        }

        private void Guideform_Load(object sender, EventArgs e)
        {

        }
    }
}
