using System;
using System.CodeDom.Compiler;
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
    
    public partial class Form1 : Form
    {

        //private Graphics g;//定义窗体画布
        private int[,] currentTrick = new int[4, 4]; //当前的砖块
        private int currentTrickNum;//当前砖块的数目
        private int currentDirection = 0;// 当前砖块的方位
        private int currentX;//当前坐标x
        private int currentY;//当前坐标y
        private int score;//分数
        private int tricksNum = 4;//方块的数目
        private int statusNum = 4;//方块的方位
        private Image myImage;//游戏面板背景
        private Random rand = new Random();//随机数
        
        /// <summary>
        /// 定义砖块int[i,j,y,x] 
        /// tricks:i为块砖,j为状态,y为列,x为行
        /// </summary>
        private int[, , ,] tricks = {{  
                                     {  
                                         {1,0,0,0},  
                                         {1,0,0,0},  
                                         {1,0,0,0},  
                                         {1,0,0,0}  
                                     },  
                                     {  
                                         {1,1,1,1},  
                                         {0,0,0,0},  
                                         {0,0,0,0},  
                                         {0,0,0,0}  
                                     },  
                                     {  
                                         {1,0,0,0},  
                                         {1,0,0,0},  
                                         {1,0,0,0},  
                                         {1,0,0,0}  
                                     },  
                                     {  
                                         {1,1,1,1},  
                                         {0,0,0,0},  
                                         {0,0,0,0},  
                                         {0,0,0,0}  
                                     }  
                                },  
                                {  
                                      {  
                                          {1,1,0,0},  
                                          {1,1,0,0},  
                                          {0,0,0,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {1,1,0,0},  
                                          {1,1,0,0},  
                                          {0,0,0,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {1,1,0,0},  
                                          {1,1,0,0},  
                                          {0,0,0,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {1,1,0,0},  
                                          {1,1,0,0},  
                                          {0,0,0,0},  
                                          {0,0,0,0}  
                                      }  
                                  },  
                                  {  
                                      {  
                                          {1,0,0,0},  
                                          {1,1,0,0},  
                                          {0,1,0,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {0,1,1,0},  
                                          {1,1,0,0},  
                                          {0,0,0,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {1,0,0,0},  
                                          {1,1,0,0},  
                                          {0,1,0,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {0,1,1,0},  
                                          {1,1,0,0},  
                                          {0,0,0,0},  
                                          {0,0,0,0}  
                                      }  
                                  },  
                                  {  
                                      {  
                                          {1,1,0,0},  
                                          {0,1,0,0},  
                                          {0,1,0,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {0,0,1,0},  
                                          {1,1,1,0},  
                                          {0,0,0,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {1,0,0,0},  
                                          {1,0,0,0},  
                                          {1,1,0,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {1,1,1,0},  
                                          {1,0,0,0},  
                                          {0,0,0,0},  
                                          {0,0,0,0}  
                                      }  
                                  }
                                    };


        /// <summary>
        /// 定义背景
        /// 14*20的二维数组
        /// 20行，14列
        /// </summary>
        private int[,] bgGround ={  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},  
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0}  
                                };

        /// <summary>
        /// 主函数入口
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        public Guideform guideform;

        public Form1(Guideform that)
        {
            InitializeComponent();
            guideform = that;
        }

        /// <summary>
        /// 窗体默认加载方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //初始化面板，得到面板对象作背景图片
            myImage = new Bitmap(panel1.Width, panel1.Height);
            //初始分数为0
            score = 0;
        }

        /// <summary>
        /// 重写窗体重绘的方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //调用画方块的方法
            DrawTetris();
            base.OnPaint(e);
        }

        /// <summary>  
        /// 随机生成方块和状态  
        /// </summary>  
        private void BeginTricks()
        {
            //随机生成砖码和状态码(0-4)  
            int i = rand.Next(0, tricksNum);
            int j = rand.Next(0, statusNum);
            currentTrickNum = i;
            currentDirection = j;
            //分配数组  
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    currentTrick[y, x] = tricks[i, j, y, x];
                }
            }
            //从(7,0)位置开始放砖块
            currentX = 7;
            currentY = 0;
            //开启计时器
            timer1.Start();
        }


        /// <summary>  
        ///  旋转方块  
        /// </summary>  
        private void ChangeTricks()
        {
            //判断当前方块的方位
            if (currentDirection < 3)
            {
                //改变方块的方位
                currentDirection++;
            }
            else
            {
                //恢复到默认方位
                currentDirection = 0;
            }


            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    currentTrick[y, x] = tricks[currentTrickNum, currentDirection, y, x];
                }
            }
        }

        /// <summary>  
        /// 下落方块  
        /// </summary>  
        private void DownTricks()
        {
            //判断是否可以下落
            if (CheckIsDown())
            {
                //下落时，纵坐标加1
                currentY++;
            }
            else
            {
                //如果方块已经堆积到画布的上边界
                if (currentY == 0)
                {
                    //计时停止，游戏结束
                    timer1.Stop();
                    MessageBox.Show("哈哈，你玩完了");
                    return;
                }
                //下落完成，修改背景  
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        if (currentTrick[y, x] == 1)
                        {
                            bgGround[currentY + y, currentX + x] = currentTrick[y, x];
                        }
                    }
                }
                CheckScore();

                BeginTricks();

            }
            DrawTetris();
        }


        /// <summary>  
        /// 检测是否可以向下了  
        /// </summary>  
        /// <returns></returns>  
        private bool CheckIsDown()
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (currentTrick[y, x] == 1)
                    {
                        //超过了背景  
                        if (y + currentY + 1 >= 20)
                        {
                            return false;
                        }
                        if (x + currentX >= 14)
                        {
                            currentX = 13 - x;
                        }
                        if (bgGround[y + currentY + 1, x + currentX] == 1)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>  
        /// 检测方块是否可以左移  
        /// </summary>  
        /// <returns></returns>  
        private bool CheckIsLeft()
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (currentTrick[y, x] == 1)
                    {
                        if (x + currentX - 1 < 0)
                        {
                            return false;
                        }
                        if (bgGround[y + currentY, x + currentX - 1] == 1)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }


        /// <summary>  
        /// 检测方块是否可以右移  
        /// </summary>  
        /// <returns></returns>  
        private bool CheckIsRight()
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (currentTrick[y, x] == 1)
                    {
                        if (x + currentX + 1 >= 14)
                        {
                            return false;
                        }
                        if (bgGround[y + currentY, x + currentX + 1] == 1)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 画方块的方法
        /// </summary>
        private void DrawTetris()
        {
            //创建窗体画布
            Graphics g = Graphics.FromImage(myImage);
            //清除以前画的图形
            g.Clear(this.BackColor);
            //画出已经掉下的方块
            // 对于已经落下的砖块，统一用一种颜色表示
            for (int bgy = 0; bgy < 20; bgy++)
            {
                for (int bgx = 0; bgx < 14; bgx++)
                {
                    if (bgGround[bgy, bgx] == 1)
                    {
                        g.FillRectangle(new SolidBrush(Color.FromArgb(204,255,204)), bgx * 20, bgy * 20, 20, 20);
                        g.DrawRectangle(new Pen(Color.FromArgb(46,139,87),1), bgx * 20, bgy * 20, 20, 20);
                    }
                }
            }
            //绘制当前的方块  
            // 初步想法：边框颜色固定，砖块颜色随机显示
            // 定义随机数种子生成rgb值
            int R = rand.Next(130, 255);
            int G = rand.Next(130, 255);
            int B = rand.Next(130, 255);
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (currentTrick[y, x] == 1)
                    {
                        //定义方块每一个小单元的边长为20
                        g.FillRectangle(new SolidBrush(Color.FromArgb(R,G,B)), (x + currentX) * 20, (y + currentY) * 20, 20, 20);
                        g.DrawRectangle(new Pen(Color.FromArgb(R, G, B), 1f), (x + currentX) * 20, (y + currentY) * 20, 20, 20);
                    }
                }
            }

            //获取面板的画布
            Graphics gg = panel1.CreateGraphics();

            gg.DrawImage(myImage, 0, 0);
        }


        /// <summary>
        /// 判断是否一行填满取得奖励得分的方法
        /// </summary>
        private void CheckScore()
        {
            for (int y = 19; y > -1; y--)
            {
                bool isFull = true;
                for (int x = 13; x > -1; x--)
                {
                    if (bgGround[y, x] == 0)
                    {
                        isFull = false;
                        break;
                    }
                }
                if (isFull)
                {
                    //增加积分  
                    score = score + 100;
                    for (int yy = y; yy > 0; yy--)
                    {
                        for (int xx = 0; xx < 14; xx++)
                        {
                            int temp = bgGround[yy - 1, xx];
                            bgGround[yy, xx] = temp;
                        }
                    }
                    y++;
                    label1.Text = "游戏得分： "+score.ToString(); ;
                    DrawTetris();
                }

            }
        }

        /// <summary>
        /// 计时器事件监听方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            DownTricks();
        }

        /// <summary>
        /// 按钮1的事件监听方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 14; x++)
                {
                    bgGround[y, x] = 0;
                }
            }
            // 下落时间间隔，默认难度为1000
            // timer1.Interval = 1000;
            BeginTricks();
            this.Focus();
        }

        /// <summary>
        /// 按钮2的事件监听方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "暂停游戏")
            {
                button2.Text = "开始游戏";
                timer1.Stop();
            }
            else
            {
                button2.Text = "暂停游戏";
                timer1.Start();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        int time_temp;
        /// <summary>
        /// 键盘释放事件监听器方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S)
            {
                //方块往下掉落
                //Console.WriteLine("S键被释放");
                timer1.Stop();
                timer1.Interval = time_temp;
                //timer1.Interval = 1000;
                timer1.Start();
            }
        }

        /// <summary>
        /// 键盘按下事件监听器方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            //Console.WriteLine("键盘监听器被检测");
            if (e.KeyCode == Keys.W)
            {
                //旋转方块
                //Console.WriteLine("W键被按下");
                ChangeTricks();
                DrawTetris();
            }
            else if (e.KeyCode == Keys.A)
            {
                //方块往左边移动
                //Console.WriteLine("方块往左边移动");
                if (CheckIsLeft())
                {
                    currentX--;
                }
                DrawTetris();
            }
            else if (e.KeyCode == Keys.D)
            {
                //方块加速下落
                //Console.WriteLine("方块往右边移动");
                if (CheckIsRight())
                {
                    currentX++;
                }
                DrawTetris();
            }
            else if (e.KeyCode == Keys.S)
            {
                //方块往右边移动
                //Console.WriteLine("S键被按下");
                timer1.Stop();
                time_temp = timer1.Interval;
                timer1.Interval = 10;
                timer1.Start();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 这里是游戏难度设置的代码
            //  初步想法 设置下落时间
            // this.comboBox1.SelectedIndex = 0;
            if (this.comboBox1.SelectedIndex == 0)
            {
                 // 简单难度
                Console.WriteLine("now is simple");
                 timer1.Interval = 500;
            }
            else if (this.comboBox1.SelectedIndex == 1)
            {
                // 正常一点
                Console.WriteLine("now is normal");
                timer1.Interval = 1000;
            }
            else if (this.comboBox1.SelectedIndex == 2)
            {
                // 作死难度
                Console.WriteLine("now is compleeeeeex");
                timer1.Interval = 200;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 netgame = new Form2();
            netgame.Show();
        }

        

        
        //返回主页面
        private void button4_Click(object sender, EventArgs e)
        {
            //Guideform guideform = new Guideform();
            guideform.Show();
            //this.Owner.Show();
            this.Dispose(true);
        }
    }
}