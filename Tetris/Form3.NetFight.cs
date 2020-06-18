using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Tetris.Properties;
using System.Diagnostics;

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
        #region 定义方块

        /// <summary>
        /// 方块 I
        /// </summary>
        int[,,] block1 ={{{0,1,0,0},{0,1,0,0},{0,1,0,0},{0,1,0,0}},
                                            {{0,0,0,0},{1,1,1,1},{0,0,0,0},{0,0,0,0}},
                                            {{0,1,0,0},{0,1,0,0},{0,1,0,0},{0,1,0,0}},
                                            {{0,0,0,0},{1,1,1,1},{0,0,0,0},{0,0,0,0}}};

        /// <summary>
        /// 方块 O
        /// </summary>
        int[,,] block2 ={{{0,0,0,0},{0,3,3,0},{0,3,3,0},{0,0,0,0}},
                                            {{0,0,0,0},{0,3,3,0},{0,3,3,0},{0,0,0,0}},
                                            {{0,0,0,0},{0,3,3,0},{0,3,3,0},{0,0,0,0}},
                                            {{0,0,0,0},{0,3,3,0},{0,3,3,0},{0,0,0,0}}};

        /// <summary>
        /// 方块 T
        /// </summary>
        int[,,] block3 ={{{0,0,0,0},{6,6,6,0},{0,6,0,0},{0,0,0,0}},
                                            {{0,6,0,0},{6,6,0,0},{0,6,0,0},{0,0,0,0}},
                                            {{0,6,0,0},{6,6,6,0},{0,0,0,0},{0,0,0,0}},
                                            {{0,6,0,0},{0,6,6,0},{0,6,0,0},{0,0,0,0}}};

        /// <summary>
        /// 方块 S
        /// </summary>
        int[,,] block4 ={{{0,4,0,0},{4,4,0,0},{4,0,0,0},{0,0,0,0}},
                                            {{4,4,0,0},{0,4,4,0},{0,0,0,0},{0,0,0,0}},
                                            {{0,4,0,0},{4,4,0,0},{4,0,0,0},{0,0,0,0}},
                                            {{4,4,0,0},{0,4,4,0},{0,0,0,0},{0,0,0,0}}};

        /// <summary>
        /// 方块 Z
        /// </summary>
        int[,,] block5 ={{{2,0,0,0},{2,2,0,0},{0,2,0,0},{0,0,0,0}},
                                            {{0,2,2,0},{2,2,0,0},{0,0,0,0},{0,0,0,0}},
                                            {{2,0,0,0},{2,2,0,0},{0,2,0,0},{0,0,0,0}},
                                            {{0,2,2,0},{2,2,0,0},{0,0,0,0},{0,0,0,0}}};

        /// <summary>
        /// 方块 L
        /// </summary>
        int[,,] block6 ={{{0,5,0,0},{0,5,0,0},{0,5,5,0},{0,0,0,0}},
                                            {{0,0,0,0},{0,5,5,5},{0,5,0,0},{0,0,0,0}},
                                            {{0,0,0,0},{0,5,5,0},{0,0,5,0},{0,0,5,0}},
                                            {{0,0,0,0},{0,0,5,0},{5,5,5,0},{0,0,0,0}}};

        /// <summary>
        /// 方块 J
        /// </summary>
        int[,,] block7 ={{{0,0,7,0},{0,0,7,0},{0,7,7,0},{0,0,0,0}},
                                            {{0,0,0,0},{7,7,7,0},{0,0,7,0},{0,0,0,0}},
                                            {{0,0,0,0},{0,7,7,0},{0,7,0,0},{0,7,0,0}},
                                            {{0,0,0,0},{0,7,0,0},{0,7,7,7},{0,0,0,0}}};

        #endregion

        #region 字段声明

        /// <summary>
        /// 地图宽
        /// </summary>
        const int mapWidth = 12;

        /// <summary>
        /// 地图高
        /// </summary>
        const int mapHeight = 21;

        /// <summary>
        /// 格子大小
        /// </summary>
        const int blockSize = 20;

        /// <summary>
        /// 背景图案
        /// </summary>
        Image background;

        /// <summary>
        /// 分数
        /// </summary>
        int[] SCORES = { 0, 50, 100, 200, 300 };

        /// <summary>
        /// 地图
        /// </summary>
        int[,] map;

        /// <summary>
        /// 远端地图
        /// </summary>
        int[,] mapRemote;

        /// <summary>
        /// 方块
        /// </summary>
        List<int[,,]> blocks;

        /// <summary>
        /// 绘制远端地图和下一个方块用笔刷
        /// </summary>
        Brush[] brushBlock;
        
        /// <summary>
        /// 速度
        /// </summary>
        int speed;

        /// <summary>
        /// 分数
        /// </summary>
        int score;
        
        /// <summary>
        /// 当前高度
        /// </summary>
        int height;

        /// <summary>
        /// 当前方块
        /// </summary>
        int[,,] currentBlock;

        /// <summary>
        /// 当前方块横坐标
        /// </summary>
        int bx;

        /// <summary>
        /// 当前方块纵坐标
        /// </summary>
        int by;

        /// <summary>
        /// 当前方块旋转
        /// </summary>
        int rot;

        /// <summary>
        /// 下一个方块
        /// </summary>
        int[,,] nextBlock;
        
        /// <summary>
        /// 游戏状态
        /// </summary>
        GameState state;

        /// <summary>
        /// 随机数
        /// </summary>
        Random rnd;

        /// <summary>
        /// 随机数种子
        /// </summary>
        int seed;

        /// <summary>
        /// 计时器
        /// </summary>
        Stopwatch watch;

        /// <summary>
        /// 缓冲图像
        /// </summary>
        BufferedGraphics bf;

        /// <summary>
        /// 联机窗口
        /// </summary>
        Form2 frmNet;
        
        /// <summary>
        /// 连接客户端
        /// </summary>
        TcpClient client;

        /// <summary>
        /// 接收命令线程
        /// </summary>
        Thread cmdThread;

        /// <summary>
        /// 发送地图线程
        /// </summary>
        Thread mapThread;

        /// <summary>
        /// 自动下落线程
        /// </summary>
        Thread fallThread;

        /// <summary>
        /// 是否在立即下落状态
        /// </summary>
        bool isFalling;
        
        #endregion

        public Form3()
        {
            InitializeComponent();
            InitializeGame();
        }

        public Guideform guideform;

        public Form3(Guideform that)
        {
            InitializeComponent();
            InitializeGame();
            guideform = that;
        }

        /// <summary>
        /// 初始化游戏数据
        /// </summary>
        private void InitializeGame()
        {

            //初始化方块
            blocks = new List<int[,,]>();
            blocks.AddRange(new int[][,,] { block1, block2, block3, block4, block5, block6, block7 });
            
            //初始化地图
            map = new int[mapHeight, mapWidth];
            mapRemote = new int[mapHeight, mapWidth];

            //初始化笔刷, 颜色, 缓冲图形
            brushBlock = new Brush[] { null, Brushes.Red, Brushes.Orange, Brushes.Yellow, Brushes.Green, Brushes.Cyan, Brushes.Blue, Brushes.Purple, Brushes.Gray };
            bf = BufferedGraphicsManager.Current.Allocate(panel1.CreateGraphics(), panel1.DisplayRectangle);

            //其它
            watch = new Stopwatch();
            state = GameState.End;
            frmNet = new Form2();
            
        }

        /// <summary>
        /// 绘制地图
        /// </summary>
        private void PaintMap(Graphics g)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 0)
                    {
                        continue;
                    }
                    g.FillRectangle(new SolidBrush(Color.FromArgb(204, 255, 204)), j * blockSize, i * blockSize, blockSize, blockSize);
                    g.DrawRectangle(new Pen(Color.FromArgb(46, 139, 87), 1), j * blockSize, i * blockSize, blockSize, blockSize);
                }
            }
        }

        /// <summary>
        /// 绘制当前方块
        /// </summary>
        private void PaintCurrentBlock(Graphics g)
        {
            //游戏未开始不画
            if (currentBlock == null)
            {
                return;
            }

            //绘制当前的方块  
            // 初步想法：边框颜色固定，砖块颜色随机显示
            Random rand = new Random();//随机数
            int R = rand.Next(130, 255);
            int G = rand.Next(130, 255);
            int B = rand.Next(130, 255);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (currentBlock[rot, i, j] == 0)
                    {
                        continue;
                    }
                    g.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), (bx + j) * blockSize, (by + i) * blockSize, blockSize, blockSize);
                    g.DrawRectangle(new Pen(Color.FromArgb(R, G, B), 1f), (bx + j) * blockSize, (by + i) * blockSize, blockSize, blockSize);
                }
            }
        }

        /// <summary>
        /// 缓冲画图
        /// </summary>
        private void RePaint()
        {
            RenderBuffer();
        }

        /// <summary>
        /// 游戏窗口绘制
        /// </summary>
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            RePaint();
        }
        
        /// <summary>
        /// 绘制远端地图
        /// </summary>
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    if (mapRemote[i, j] == 0)
                    {
                        continue;
                    }
                    e.Graphics.FillRectangle(brushBlock[mapRemote[i, j]], new Rectangle(j * 5, i * 5, 5, 5));
                }
            }
        }

        /// <summary>
		/// 按键响应
		/// </summary>
		private void Form3_KeyDown(object sender, KeyEventArgs e)
        {
            //游戏未开始不响应按键
            if (state == GameState.End)
            {
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.A:
                    if (IsMovable(-1))
                    {
                        bx--;
                    }
                    break;
                case Keys.D:
                    if (IsMovable(1))
                    {
                        bx++;
                    }
                    break;
                case Keys.W:
                    if (IsChangable(-1))
                    {
                        rot = --rot < 0 ? 3 : rot;
                    }
                    break;
                case Keys.S:
                    if (IsFallable())
                    {
                        by++;
                    }
                    break;
                case Keys.Space:
                    FallDown();
                    break;
            }
            RePaint();
        }

        /// <summary>
        /// 开始联机游戏
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "连接主机/副机")
            {
                DialogResult dr = frmNet.ShowDialog();
                if (dr == DialogResult.Cancel)
                {
                    return;
                }

                button3.Text = "断开连接";
                button1.Enabled = false;

                state = frmNet.State;
                client = frmNet.Client;
                cmdThread = new Thread(new ThreadStart(ReceiveCommand));
                cmdThread.Start();

                if (state == GameState.Host)
                {
                    seed = DateTime.Now.Millisecond;
                    SendCommand(string.Format("rnd:{0}:", seed));
                    GameStart();
                }
            }
            else
            {
                SendCommand("exit:");
                GameEnd("你跑了");
            }
        }
        
        /// <summary>
        /// 返回
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            //Guideform guideform = new Guideform();
            guideform.Show();
            //this.Owner.Show();
            this.Dispose(true);
        }

        
        private void Form3_Load(object sender, EventArgs e)
        {
            //初始化面板，得到面板对象作背景图片
            background = new Bitmap(panel1.Width, panel1.Height);
        }
    }
}
