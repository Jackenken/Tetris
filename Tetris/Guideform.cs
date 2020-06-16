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

        //使得关闭按钮灰化
        protected override CreateParams CreateParams
        {
            get
            {
                int CS_NOCLOSE = 0x200;
                CreateParams parameters = base.CreateParams;
                parameters.ClassStyle |= CS_NOCLOSE;
                return parameters;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Form1 singlegame = new Form1(this);
            singlegame.Show();
            this.Hide();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 netgame = new Form3(this);
            netgame.Show();
            this.Hide();

        }

        private void Guideform_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Local2players localdoubelsform = new Local2players(this);
            localdoubelsform.Show();
            this.Hide();
        }
    }
}
