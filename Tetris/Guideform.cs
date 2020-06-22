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
            this.Width = 750;
            this.Height = 450;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackgroundImage = Properties.Resources.Tetris1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.BackgroundImage = Properties.Resources.Btn1;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.BackgroundImage = Properties.Resources.Btn2;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button3.BackgroundImage = Properties.Resources.Btn3;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button4.BackgroundImage = Properties.Resources.Btn4;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
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

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            this.button4.BackgroundImage = Properties.Resources.Btn4deep;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            this.button4.BackgroundImage = Properties.Resources.Btn4;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            this.button4.BackgroundImage = Properties.Resources.Btn4glow;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            this.button4.BackgroundImage = Properties.Resources.Btn4;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            this.button1.BackgroundImage = Properties.Resources.Btn1deep;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            this.button1.BackgroundImage = Properties.Resources.Btn1glow;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            this.button1.BackgroundImage = Properties.Resources.Btn1;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            this.button1.BackgroundImage = Properties.Resources.Btn1;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            this.button2.BackgroundImage = Properties.Resources.Btn2deep;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            this.button2.BackgroundImage = Properties.Resources.Btn2glow;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            this.button2.BackgroundImage = Properties.Resources.Btn2;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            this.button2.BackgroundImage = Properties.Resources.Btn2;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            this.button3.BackgroundImage = Properties.Resources.Btn3deep;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            this.button3.BackgroundImage = Properties.Resources.Btn3glow;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            this.button3.BackgroundImage = Properties.Resources.Btn3;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            this.button3.BackgroundImage = Properties.Resources.Btn3;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }
    }
}
