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
    public partial class Local2players : Form
    {

        private int[,] currentTrick = new int[4, 4]; //当前的砖块
        private int[,] currentTrickRight = new int[4, 4]; //当前的砖块
        private int currentTrickNum;//当前砖块的数目
        private int currentTrickNumRight;//当前砖块的数目
        private int currentDirection = 0;// 当前砖块的方位
        private int currentDirectionRight = 0;// 当前砖块的方位
        private int currentX;//当前坐标x
        private int currentXRight;//当前坐标x
        private int currentY;//当前坐标y
        private int currentYRight;//当前坐标y
        private int score;//分数
        private int scoreRight;//分数
        private int tricksNum = 4;//方块的数目
        private int statusNum = 4;//方块的方位
        private Image myImage;//游戏面板背景
        private Image myImageRight;//游戏面板背景
        private Random rand = new Random();//随机数

        private bool leftlife = true;
        private bool rightlife = true;
        private int leftplayer = 0;
        private int rightplayer = 0;

        private int[] tricksshapex = new int[200];
        private int[] tricksshapey = new int[200];
        
        /// <summary>
        /// 定义砖块int[i,j,y,x] 
        /// tricks:i为块砖,j为状态,y为列,x为行
        /// </summary>
        private int[,,,] tricks = {{
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

        private int[,] bgGroundRight ={
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


        public Local2players()
        {
            InitializeComponent();
        }

        public Guideform guideform;

        public Local2players(Guideform that)
        {
            InitializeComponent();
            guideform = that;
        }



        private void Local2players_Load(object sender, EventArgs e)
        {
            //初始化面板，得到面板对象作背景图片
            myImage = new Bitmap(panel1.Width, panel1.Height);
            myImageRight = new Bitmap(panel2.Width, panel2.Height);
            //初始分数为0
            score = 0;
            this.comboBox1.SelectedIndex = 0;
            scoreRight = 0;
        }


        /// <summary>
        /// 重写窗体重绘的方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //调用画方块的方法
            DrawTetris();
            DrawTetrisRight();
            base.OnPaint(e);
        }

        /// <summary>  
        /// 随机生成方块和状态  
        /// </summary>  
        private void BeginTricks()
        {
            //随机生成砖码和状态码(0-4)  
            //int i = rand.Next(0, tricksNum);
            //int j = rand.Next(0, statusNum);
            
            currentTrickNum = tricksshapex[leftplayer++];
            currentDirection = tricksshapey[leftplayer++];
            //分配数组  
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    currentTrick[y, x] = tricks[currentTrickNum, currentDirection, y, x];
                }
            }
            //从(7,0)位置开始放砖块
            currentX = 7;
            currentY = 0;
            //开启计时器
            timer1.Start();
        }
        private void BeginTricksRight()
        {
            //随机生成砖码和状态码(0-4)  
            //int i = rand.Next(0, tricksNum);
            //int j = rand.Next(0, statusNum);
            currentTrickNumRight = tricksshapex[rightplayer++]; 
            currentDirectionRight = tricksshapey[rightplayer++]; ;
            //分配数组  
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    currentTrickRight[y, x] = tricks[currentTrickNumRight, currentDirectionRight, y, x];
                }
            }
            //从(7,0)位置开始放砖块
            currentXRight = 7;
            currentYRight = 0;
            //开启计时器
            timer2.Start();
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
        private void ChangeTricksRight()
        {
            //判断当前方块的方位
            if (currentDirectionRight < 3)
            {
                //改变方块的方位
                currentDirectionRight++;
            }
            else
            {
                //恢复到默认方位
                currentDirectionRight = 0;
            }


            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    currentTrickRight[y, x] = tricks[currentTrickNumRight, currentDirectionRight, y, x];
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
                    leftlife = false;
                    if (!rightlife)
                    {
                        if (score > scoreRight)
                            MessageBox.Show("左边玩家获胜");
                        else if (score < scoreRight)
                            MessageBox.Show("右边玩家获胜");
                        else
                            MessageBox.Show("平局");
                    }                    
                    
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

        private void DownTricksRight()
        {
            //判断是否可以下落
            if (CheckIsDownRight())
            {
                //下落时，纵坐标加1
                currentYRight++;
            }
            else
            {
                //如果方块已经堆积到画布的上边界
                if (currentYRight == 0)
                {
                    //计时停止，游戏结束
                    timer2.Stop();
                    rightlife = false;
                    if (!leftlife)
                    {
                        if (score > scoreRight)
                            MessageBox.Show("左边玩家获胜");
                        else if (score < scoreRight)
                            MessageBox.Show("右边玩家获胜");
                        else
                            MessageBox.Show("平局");
                    }
                    return;
                }
                //下落完成，修改背景  
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        if (currentTrickRight[y, x] == 1)
                        {
                            bgGroundRight[currentYRight + y, currentXRight + x] = currentTrickRight[y, x];
                        }
                    }
                }
                CheckScoreRight();

                BeginTricksRight();

            }
            DrawTetrisRight();
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
        
        private bool CheckIsDownRight()
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (currentTrickRight[y, x] == 1)
                    {
                        //超过了背景  
                        if (y + currentYRight + 1 >= 20)
                        {
                            return false;
                        }
                        if (x + currentXRight >= 14)
                        {
                            currentXRight = 13 - x;
                        }
                        if (bgGroundRight[y + currentYRight + 1, x + currentXRight] == 1)
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
        private bool CheckIsLeftRight()
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (currentTrickRight[y, x] == 1)
                    {
                        if (x + currentXRight - 1 < 0)
                        {
                            return false;
                        }
                        if (bgGroundRight[y + currentYRight, x + currentXRight - 1] == 1)
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
        private bool CheckIsRightRight()
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (currentTrickRight[y, x] == 1)
                    {
                        if (x + currentXRight + 1 >= 14)
                        {
                            return false;
                        }
                        if (bgGroundRight[y + currentYRight, x + currentXRight + 1] == 1)
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
                        g.FillRectangle(new SolidBrush(Color.FromArgb(204, 255, 204)), bgx * 20, bgy * 20, 20, 20);
                        g.DrawRectangle(new Pen(Color.FromArgb(46, 139, 87), 1), bgx * 20, bgy * 20, 20, 20);
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
                        g.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), (x + currentX) * 20, (y + currentY) * 20, 20, 20);
                        g.DrawRectangle(new Pen(Color.FromArgb(R, G, B), 1f), (x + currentX) * 20, (y + currentY) * 20, 20, 20);
                    }
                }
            }

            //获取面板的画布
            Graphics gg = panel1.CreateGraphics();

            gg.DrawImage(myImage, 0, 0);
        }
        private void DrawTetrisRight()
        {
            //创建窗体画布
            Graphics gRight = Graphics.FromImage(myImageRight);
            //清除以前画的图形
            gRight.Clear(this.BackColor);
            //画出已经掉下的方块
            // 对于已经落下的砖块，统一用一种颜色表示
            for (int bgy = 0; bgy < 20; bgy++)
            {
                for (int bgx = 0; bgx < 14; bgx++)
                {
                    if (bgGroundRight[bgy, bgx] == 1)
                    {
                        gRight.FillRectangle(new SolidBrush(Color.FromArgb(204, 255, 204)), bgx * 20, bgy * 20, 20, 20);
                        gRight.DrawRectangle(new Pen(Color.FromArgb(46, 139, 87), 1), bgx * 20, bgy * 20, 20, 20);
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
                    if (currentTrickRight[y, x] == 1)
                    {
                        //定义方块每一个小单元的边长为20
                        gRight.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), (x + currentXRight) * 20, (y + currentYRight) * 20, 20, 20);
                        gRight.DrawRectangle(new Pen(Color.FromArgb(R, G, B), 1f), (x + currentXRight) * 20, (y + currentYRight) * 20, 20, 20);
                    }
                }
            }

            //获取面板的画布
            Graphics ggRight = panel2.CreateGraphics();

            ggRight.DrawImage(myImageRight, 0, 0);
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
                    label1.Text = "游戏得分： " + score.ToString(); ;
                    DrawTetris();
                }

            }
        }
        private void CheckScoreRight()
        {
            for (int y = 19; y > -1; y--)
            {
                bool isFull = true;
                for (int x = 13; x > -1; x--)
                {
                    if (bgGroundRight[y, x] == 0)
                    {
                        isFull = false;
                        break;
                    }
                }
                if (isFull)
                {
                    //增加积分  
                    scoreRight = scoreRight + 100;
                    for (int yy = y; yy > 0; yy--)
                    {
                        for (int xx = 0; xx < 14; xx++)
                        {
                            int temp = bgGroundRight[yy - 1, xx];
                            bgGroundRight[yy, xx] = temp;
                        }
                    }
                    y++;
                    label2.Text = "游戏得分： " + scoreRight.ToString(); ;
                    DrawTetrisRight();
                }

            }
        }

        /// <summary>
        /// 计时器事件监听方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            DownTricks();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 14; x++)
                {
                    bgGround[y, x] = 0;
                    bgGroundRight[y, x] = 0;
                }
            }
            // 下落时间间隔，默认难度为1000
             timer1.Interval = 1000;
             timer2.Interval = 1000;

            for (int u=0;u<200;u++)
            {
                tricksshapex[u]= rand.Next(0, tricksNum);
                tricksshapey[u] = rand.Next(0, statusNum);
            }
            score = 0;
            scoreRight = 0;
            label1.Text = "游戏得分： " + score.ToString();
            label2.Text = "游戏得分： " + scoreRight.ToString(); ;

            leftlife = true;
            rightlife = true;
            leftplayer = 0;
            rightplayer = 0;

            BeginTricksRight();
            BeginTricks();
            //this.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "暂停游戏")
            {
                button2.Text = "继续游戏";
                timer1.Stop();
                timer2.Stop();
            }
            else
            {
                button2.Text = "暂停游戏";
                timer1.Start();
                timer2.Start();
            }
        }


        int time_temp;
        int time_tempright;
        /// <summary>
        /// 键盘释放事件监听器方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Local2players_KeyUp(object sender, KeyEventArgs e)
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
            if (e.KeyCode == Keys.K)
            {
                //方块往下掉落
                //Console.WriteLine("S键被释放");
                timer2.Stop();
                timer2.Interval = time_tempright;
                //timer1.Interval = 1000;
                timer2.Start();
            }
        }

        /// <summary>
        /// 键盘按下事件监听器方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Local2players_KeyDown(object sender, KeyEventArgs e)
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
                //方块往右边移动
                //Console.WriteLine("方块往右边移动");
                if (CheckIsRight())
                {
                    currentX++;
                }
                DrawTetris();
            }
            else if (e.KeyCode == Keys.S)
            {
                //方块加速下落
                //Console.WriteLine("S键被按下");
                timer1.Stop();
                time_temp = timer1.Interval;
                timer1.Interval = 10;
                timer1.Start();
            }

            if (e.KeyCode == Keys.I)
            {
                //旋转方块
                //Console.WriteLine("I键被按下");
                ChangeTricksRight();
                DrawTetrisRight();
            }
            else if (e.KeyCode == Keys.J)
            {
                //方块往左边移动
                if (CheckIsLeftRight())
                {
                    currentXRight--;
                }
                DrawTetrisRight();
            }
            else if (e.KeyCode == Keys.L)
            {
                //方块往右边移动
                if (CheckIsRightRight())
                {
                    currentXRight++;
                }
                DrawTetrisRight();
            }
            else if (e.KeyCode == Keys.K)
            {
                //方块加速下落
                
                timer2.Stop();
                time_tempright = timer2.Interval;
                timer2.Interval = 10;
                timer2.Start();
            }
        }
     
    



        private void button3_Click(object sender, EventArgs e)
        {
            //Guideform guideform = new Guideform();
            guideform.Show();
            //this.Owner.Show();
            this.Dispose(true);
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
                timer1.Interval = 1000;
                timer2.Interval = 1000;
            }
            else if (this.comboBox1.SelectedIndex == 1)
            {
                // 正常一点
                Console.WriteLine("now is normal");
                timer1.Interval = 500;
                timer2.Interval = 500;
            }
            else if (this.comboBox1.SelectedIndex == 2)
            {
              
                Console.WriteLine("now is normal");
                timer1.Interval = 300;
                timer2.Interval = 300;
            }
            else if (this.comboBox1.SelectedIndex == 3)
            {
                // 作死难度
                Console.WriteLine("now is compleeeeeex");
                timer1.Interval = 200;
                timer2.Interval = 200;
            }
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

        private void Timer2_Tick(object sender, EventArgs e)
        {
            DownTricksRight();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
