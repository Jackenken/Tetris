using System;
using System.Drawing;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Tetris
{
    /// <summary>
	/// 游戏状态
	/// </summary>
	public enum GameState
    {
        /// 主机
        Host,

        /// 副机
        Client,

        /// 游戏结束
        End
    }

    public partial class Form3 : Form
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
        private Image myImage;//我方游戏面板背景
        private Image yourImage;//对方游戏面板背景
        private Random rand = new Random();//随机数

        int seed;// 随机数种子
        GameState state;//游戏状态
        TcpClient client;// 连接客户端
        Thread cmdThread;// 接收命令线程
        Thread mapThread;// 发送地图线程
        Thread fallThread;// 自动下落线程
        bool isFalling;// 是否在立即下落状态

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

        public Form3()
        {
            InitializeComponent();
        }

        private Guideform guideform;

        public Form3(Guideform that)
        {
            InitializeComponent();
            guideform = that;
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

        private void Form3_Load(object sender, EventArgs e)
        {
            //初始化面板，得到面板对象作背景图片
            myImage = new Bitmap(panel1.Width, panel1.Height);
            yourImage = new Bitmap(panel2.Width, panel2.Height);
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
            //DrawTetris();
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
        /// 画本地的方块的方法
        /// </summary>
        private void DrawTetris()
        {
            //创建窗体画布
            Graphics g1 = Graphics.FromImage(myImage);
            //清除以前画的图形
            g1.Clear(this.BackColor);
            //画出已经掉下的方块
            //对于已经落下的砖块，统一用一种颜色表示
            for (int bgy = 0; bgy < 20; bgy++)
            {
                for (int bgx = 0; bgx < 14; bgx++)
                {
                    if (bgGround[bgy, bgx] == 1)
                    {
                        g1.FillRectangle(new SolidBrush(Color.FromArgb(204, 255, 204)), bgx * 20, bgy * 20, 20, 20);
                        g1.DrawRectangle(new Pen(Color.FromArgb(46, 139, 87), 1), bgx * 20, bgy * 20, 20, 20);
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
                        g1.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), (x + currentX) * 20, (y + currentY) * 20, 20, 20);
                        g1.DrawRectangle(new Pen(Color.FromArgb(R, G, B), 1f), (x + currentX) * 20, (y + currentY) * 20, 20, 20);
                        
                    }
                }
            }

            //获取面板的画布
            Graphics gg1 = panel1.CreateGraphics();

            gg1.DrawImage(myImage, 0, 0);
        }

        /// <summary>
        /// 画远方的方块的方法
        /// </summary>
        private void DrawRemoteTetris()
        {
            //创建窗体画布
            Graphics g2 = Graphics.FromImage(yourImage);
            //清除以前画的图形
            g2.Clear(this.BackColor);
            //画出已经掉下的方块
            //对于已经落下的砖块，统一用一种颜色表示
            for (int bgy = 0; bgy < 20; bgy++)
            {
                for (int bgx = 0; bgx < 14; bgx++)
                {
                    if (bgGround[bgy, bgx] == 1)
                    {
                        g2.FillRectangle(new SolidBrush(Color.FromArgb(204, 255, 204)), bgx * 5, bgy * 5, 5, 5);
                        g2.DrawRectangle(new Pen(Color.FromArgb(46, 139, 87), 1), bgx * 5, bgy * 5, 5, 5);
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
                        //定义方块每一个小单元的边长为5
                        g2.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), (x + currentX) * 5, (y + currentY) * 5, 5, 5);
                        g2.DrawRectangle(new Pen(Color.FromArgb(R, G, B), 1f), (x + currentX) * 5, (y + currentY) * 5, 5, 5);
                    }
                }
            }

            //获取面板的画布
            Graphics gg2 = panel2.CreateGraphics();

            gg2.DrawImage(yourImage, 0, 0);
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

        /// <summary>
        /// 计时器事件监听方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            DownTricks();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "连接主机/副机")
            {
                //DialogResult dr = frmNet.ShowDialog();
                Form2 frmNet = new Form2();
                frmNet.Show();

                button3.Text = "断开连接";
                button3.Enabled = false;
                button3.Enabled = false;

                state = frmNet.State;
                client = frmNet.Client;
                cmdThread = new Thread(new ThreadStart(ReceiveCommand));
                cmdThread.Start();

                if (state == GameState.Host)
                {
                    seed = DateTime.Now.Millisecond;
                    //SendCommand(string.Format("rnd:{0}:", seed));
                    //GameStart();
                }
            }
            else
            {
                //SendCommand("exit:");
                //GameEnd("你跑了");
            }

        }

        /***
        /// <summary>
		/// 自动下落线程
		/// </summary>
		private void AutoFall()
        {
            while (state != GameState.End)
            {
                Thread.Sleep(speed);

                //防止重复消行
                if (isFalling)
                {
                    continue;
                }

                if (!IsFallable())
                {
                    PlaceBlock();
                    height = CalculateHeight();
                    if (height >= mapHeight)
                    {
                        
                        SendCommand("loose:");
                        GameEnd("你输了");
                        
                        return;
                    }
                    UpdateData();
                    SetCurrentBlock();
                    SetNextBlock();
                }
                else
                {
                    by++;
                }
                RePaint();
            }
        }
        ****/

        /// <summary>
        /// 发送地图信息线程
        /// </summary>
        private void SendMap()
        {
            while (state == GameState.Host || state == GameState.Client)
            {
                Thread.Sleep(3000);
                //SendCommand(string.Format("map:{0}:", SerializeMap(map)));
            }
        }

        /// <summary>
        /// 接收命令线程
        /// </summary>
        private void ReceiveCommand()
        {
            while (state == GameState.Host || state == GameState.Client)
            {
                //buffer要大于 mapHeight * mapWidth + n
                byte[] buffer = new byte[260];

                try
                {
                    client.Client.Receive(buffer);
                }
                catch
                {
                    return;
                }

                string cmd = Encoding.ASCII.GetString(buffer);
                string[] token = cmd.Split(':');
                switch (token[0])
                {
                    //随机数种子 "rnd:seed:"
                    case "rnd":
                        seed = int.Parse(token[1]);
                        //GameStart();
                        break;

                    //地图信息 "map:地图:"
                    case "map":
                        //mapRemote = DeserializeMap(token[1].ToCharArray());
                        //RefreshControl(pnlGameRemote);
                        break;
                    
                    //退出 "exit:"
                    case "exit":
                        //GameEnd("貌似那边撤了");
                        break;

                    //输了 "loose:"
                    case "loose":
                        //GameEnd("嗯, 你赢了");
                        break;

                    //断线
                    default:
                        if (client != null && state != GameState.End)
                        {
                            //GameEnd("貌似是那边 [ 断线/离线 ] 了");
                        }
                        return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            guideform.Show();            
            this.Dispose(true);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form3_Load_1(object sender, EventArgs e)
        {

        }
    }
}
