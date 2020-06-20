using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Tetris
{
    public partial class Form3
    {
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

        /// <summary>
        /// 发送地图信息线程
        /// </summary>
        private void SendMap()
        {
            while (state == GameState.Host || state == GameState.Client)
            {
                Thread.Sleep(3000);
                SendCommand(string.Format("map:{0}:", SerializeMap(map)));
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
                        GameStart();
                        break;

                    //地图信息 "map:地图:"
                    case "map":
                        mapRemote = DeserializeMap(token[1].ToCharArray());
                        RefreshControl(panel2);
                        break;
                    
                    //连续消行信息 "add:行数:"
                    case "add":
                        int number = int.Parse(token[1]);
                        AddLine(number);
                        break;

                    //退出 "exit:"
                    case "exit":
                        GameEnd("貌似那边撤了");
                        break;

                    //输了 "loose:"
                    case "loose":
                        GameEnd("嗯, 你赢了");
                        break;

                    //断线或其它未知问题, 不知道发生时间
                    default:
                        if (client != null && state != GameState.End)
                        {
                            GameEnd("貌似是那边 [ 断线/离线 ] 了");
                        }
                        return;
                }
            }
        }
        
        /// <summary>
        /// 跨线程刷新控件委托, 不知是否需要
        /// </summary>
        /// <param name="control">需要刷新的控件</param>
        delegate void AsyncRefresh(Control control);

        /// <summary>
        /// 跨线程渲染缓冲图形委托, 不知是否有效
        /// </summary>
        delegate void AsyncRender();

        /// <summary>
        /// 跨线程刷新控件
        /// </summary>
        /// <param name="control">需要刷新的控件</param>
        private void RefreshControl(Control control)
        {
            if (control.InvokeRequired)
            {
                AsyncRefresh asr = RefreshControl;
                control.Invoke(asr, new object[] { control });
            }
            else
            {
                control.Refresh();
            }
        }

        /// <summary>
        /// 跨线程渲染缓冲图形
        /// </summary>
        private void RenderBuffer()
        {
            if (panel1.InvokeRequired)
            {
                AsyncRender asr = RenderBuffer;
                panel1.Invoke(asr);
            }
            else
            {
                bf.Graphics.DrawImage(background, panel1.DisplayRectangle);
                PaintMap(bf.Graphics);
                PaintCurrentBlock(bf.Graphics);
                bf.Render();
            }
        }

        /// <summary>
		/// 游戏开始
		/// </summary>
		private void GameStart()
        {
            rnd = new Random(seed);

            map = new int[mapHeight, mapWidth];
            mapRemote = new int[mapHeight, mapWidth];

            height = 0;
            
            score = 0;
            label1.Text = score.ToString();
            
            txtMessage.Clear();

            SetNextBlock();
            SetCurrentBlock();
            SetNextBlock();
            
            speed = 400;
            
            watch.Reset();
            watch.Start();

            AppendMessage("游戏开始", Color.Red);

            RePaint();
            panel2.Refresh();

            mapThread = new Thread(new ThreadStart(SendMap));
            mapThread.Start();

            fallThread = new Thread(new ThreadStart(AutoFall));
            fallThread.Start();

            this.Activate();
        }

        /// <summary>
        /// 游戏结束, 关闭连接
        /// </summary>
        private void GameEnd(string msg)
        {
            state = GameState.End;

            watch.Stop();

            if (!string.IsNullOrEmpty(msg))
            {
                AppendMessage(msg, Color.Red);
            }

            button3.Text = "连接主机/副机";
            button1.Enabled = true;

            if (client != null)
            {
                Disconnection();
            }

            frmNet.Restore();

            //关闭线程

            if (fallThread != null)
            {
                fallThread.Abort();
                fallThread.Join();
                fallThread = null;
            }

            if (cmdThread != null)
            {
                cmdThread.Abort();
                cmdThread.Join();
                cmdThread = null;
            }

            if (mapThread != null)
            {
                mapThread.Abort();
                mapThread.Join();
                mapThread = null;
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        private void Disconnection()
        {
            client.Close();
            client = null;
        }

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="msg">消息内容</param>
        /// <param name="color">字体颜色</param>
        private void AppendMessage(string msg, Color color)
        {
            txtMessage.SelectionColor = color;
            txtMessage.AppendText(string.Format("\r\n{0:D2}m{1:D2}s{2:D3} {3}", watch.Elapsed.Minutes, watch.Elapsed.Seconds, watch.Elapsed.Milliseconds, msg));
            txtMessage.ScrollToCaret();
        }

        /// <summary>
        /// 设置下一个方块
        /// </summary>
        /// <returns>下一个方块</returns>
        private void SetNextBlock()
        {
            nextBlock = blocks[rnd.Next(blocks.Count)];
            //RefreshControl(pnlNext);
        }

        /// <summary>
        /// 设置当前方块
        /// </summary>
        private void SetCurrentBlock()
        {
            bx = mapWidth / 2 - 2;
            by = 0;
            rot = 0;
            currentBlock = nextBlock;
        }

        /// <summary>
        /// 堆放方块
        /// </summary>
        private void PlaceBlock()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (currentBlock[rot, i, j] == 0 || bx + j < 0 || bx + j >= mapWidth || by + i >= mapHeight)
                    {
                        continue;
                    }
                    map[by + i, bx + j] = currentBlock[rot, i, j];
                }
            }
        }

        /// <summary>
        /// 判断是否可以下落
        /// </summary>
        /// <returns>是否可以下落</returns>
        private bool IsFallable()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (currentBlock[rot, i, j] != 0)
                    {
                        if (by + i + 1 >= mapHeight || map[by + i + 1, bx + j] != 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 判断是否可以移动
        /// </summary>
        /// <param name="dx">移动方向</param>
        /// <returns>是否可以移动</returns>
        private bool IsMovable(int dx)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (currentBlock[rot, i, j] != 0)
                    {
                        if (bx + j + dx >= mapWidth || bx + j + dx < 0 || map[by + i, bx + j + dx] != 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 判断是否可以旋转
        /// </summary>
        /// <param name="drot">旋转方向</param>
        /// <returns>是否可以旋转</returns>
        private bool IsChangable(int drot)
        {
            int tmp = rot + drot;
            if (tmp > 3)
            {
                tmp = 0;
            }
            else if (tmp < 0)
            {
                tmp = 3;
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (currentBlock[tmp, i, j] != 0)
                    {
                        //如果旋转后超出地图, 则向左/右移动一格再旋转
                        if (bx + j >= mapWidth || bx + j < 0)
                        {
                            if (bx < mapWidth / 2)
                            {
                                bx++;
                                if (IsChangable(drot))
                                {
                                    return true;
                                }
                                else
                                {
                                    bx--;
                                    continue;
                                }
                            }
                            else
                            {
                                bx--;
                                if (IsChangable(drot))
                                {
                                    return true;
                                }
                                else
                                {
                                    bx++;
                                    continue;
                                }
                            }
                        }

                        if (map[by + i, bx + j] != 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 计算当前高度
        /// </summary>
        private int CalculateHeight()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] != 0)
                    {
                        return mapHeight - i;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 检查是否可以消行
        /// </summary>
        /// <returns>需要消掉的行下标</returns>
        private List<int> CheckClearBlock()
        {
            bool clearable;
            List<int> list = new List<int>();

            for (int i = map.GetLength(0) - height; i < map.GetLength(0); i++)
            {
                clearable = true;
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 0)
                    {
                        clearable = false;
                        break;
                    }
                }
                if (clearable)
                {
                    list.Add(i);
                }
            }

            return list;
        }

        /// <summary>
        /// 消行
        /// </summary>
        /// <param name="list">需要消行的下标</param>
        private void ClearBlock(List<int> list)
        {
            //消行动画
            foreach (int i in list)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    map[i, j] = 8;
                }
            }

            RePaint();

            Thread.Sleep(100);

            foreach (int i in list)
            {
                for (int j = i; j >= map.GetLength(0) - height; j--)
                {
                    for (int k = 0; k < map.GetLength(1); k++)
                    {
                        map[j, k] = map[j - 1, k];
                    }
                }
            }
        }

        /// <summary>
        /// 更新数据, 包括消行, 算分
        /// </summary>
        private void UpdateData()
        {
            List<int> list = CheckClearBlock();
            if (list.Count != 0)
            {
                ClearBlock(list);
                score += SCORES[list.Count];
                label1.Text = score.ToString();

                if (list.Count >= 2)
                {
                    SendCommand(string.Format("add:{0}:", list.Count - 1));
                }
                
                
            }
        }

        /// <summary>
        /// 立即下落
        /// </summary>
        private void FallDown()
        {
            isFalling = true;

            while (IsFallable())
            {
                by++;
            }

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
            RePaint();

            isFalling = false;
        }

        /// <summary>
        /// 增行
        /// </summary>
        /// <param name="line">行数</param>
        private void AddLine(int line)
        {
            while (line-- > 0)
            {

                //将当前方块相应提升高度
                if (by > 0)
                {
                    by--;
                }

                for (int i = mapHeight - height - 1; i < mapHeight; i++)
                {
                    for (int j = 0; j < mapWidth; j++)
                    {
                        if (i != mapHeight - 1)
                        {
                            map[i, j] = map[i + 1, j];
                        }
                        else
                        {
                            map[i, j] = rnd.Next(2);
                        }
                    }
                }

                height = CalculateHeight();
                if (height >= mapHeight)
                {
                    SendCommand("loose:");
                    GameEnd("挤死了, 你输了");
                    return;
                }
            }
        }

        /// <summary>
        /// 序列化地图
        /// </summary>
        /// <param name="map">本机地图</param>
        /// <returns>地图配置字符串</returns>
        private string SerializeMap(int[,] map)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    sb.Append(map[i, j]);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 反序列化地图
        /// </summary>
        /// <param name="mapc">地图配置数组</param>
        /// <returns>远端地图</returns>
        private int[,] DeserializeMap(char[] mapc)
        {
            int n = 0;
            int[,] mapi = new int[mapHeight, mapWidth];
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    mapi[i, j] = int.Parse(mapc[n].ToString());
                    n++;
                }
            }
            return mapi;
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="cmd">命令</param>
        private void SendCommand(string cmd)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(cmd);

            try
            {
                client.Client.Send(buffer, 0, buffer.Length, SocketFlags.None);
            }
            catch
            {
                return;
            }
        }
    }
}
