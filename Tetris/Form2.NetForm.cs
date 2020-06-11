using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Tetris
{
    public partial class Form2 : Form
    {
        #region 声明变量

        /// <summary>
        /// 自机ip
        /// </summary>
        IPAddress hostIP;

        /// <summary>
        /// 远端ip
        /// </summary>
        IPAddress remoteIP;

        /// <summary>
        /// tcp监听器, 做主机用
        /// </summary>
        TcpListener listener;

        /// <summary>
        /// tcp客户端, 做副机用
        /// </summary>
        TcpClient client;

        /// <summary>
        /// tcp客户端
        /// </summary>
        public TcpClient Client
        {
            get { return client; }
            set { client = value; }
        }

        /// <summary>
        /// 是否取消连接
        /// </summary>
        bool abort;
        
        /// <summary>
        /// 异步状态
        /// </summary>
        IAsyncResult ar;
        #endregion

        public Form2()
        {
            InitializeComponent2();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        public void Restore()
        {
            lbl_tip.Text = "选择主机或副机";
            rad_host.Enabled = true;
            rad_client.Enabled = true;
            mtb_remoteIp.Enabled = rad_client.Checked;
            cbo_ip.Enabled = rad_host.Checked;
            btn_connect.Text = rad_host.Checked ? "开始(&o)" : "连接(&o)";
            btn_cancel.Enabled = true;

            if (listener != null)
            {
                listener.Stop();
            }
        }

        #region 异步回调方法

        /// <summary>
        /// 监听器等待连接的回调方法
        /// </summary>
        private void DoAcceptTcpClientCallBack(IAsyncResult iar)
        {

            //如果是由于取消连接而完成异步, 则直接返回
            if (abort)
            {
                abort = false;
                return;
            }

            //否则获取套接字并关闭窗口
            TcpListener tmpListener = iar.AsyncState as TcpListener;
            try
            {
                Client = tmpListener.EndAcceptTcpClient(iar);
            }
            catch
            {
                MessageBox.Show(this, "创建主机失败");
                Restore();
                return;
            }
            //state = GameState.Host;
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        /// <summary>
        /// 连接主机的回调方法
        /// </summary>
        private void DoBeginConnectCallBack(IAsyncResult iar)
        {

            //如果由于取消连接而完成异步, 则直接返回
            if (abort)
            {
                abort = false;
                return;
            }

            //否则获取套接字并关闭窗口
            TcpClient tmpClient = iar.AsyncState as TcpClient;
            try
            {
                tmpClient.EndConnect(iar);
            }
            catch
            {
                MessageBox.Show(this, "连接失败");
                Restore();
                return;
            }
            //state = GameState.Client;
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        #endregion

        #region 窗口事件

        /// <summary>
        /// 窗口加载事件, 显示本机ip
        /// </summary>
        private void form_net_Load(object sender, EventArgs e)
        {
            Restore();

            //绑定ip数据到ip下拉框
            cbo_ip.DataSource = Dns.GetHostAddresses(Dns.GetHostName());

            hostIP = cbo_ip.SelectedItem as IPAddress;
        }

        /// <summary>
        /// "host" 单选按钮事件
        /// </summary>
        private void rad_host_CheckedChanged(object sender, EventArgs e)
        {
            btn_connect.Text = "开始(&o)";
            cbo_ip.Enabled = true;
            mtb_remoteIp.Enabled = false;
        }

        /// <summary>
        /// "client" 单选按钮事件
        /// </summary>
        private void rad_client_CheckedChanged(object sender, EventArgs e)
        {
            btn_connect.Text = "连接(&o)";
            mtb_remoteIp.Enabled = true;
            mtb_remoteIp.Focus();
            mtb_remoteIp.SelectAll();
            cbo_ip.Enabled = false;
        }

        /// <summary>
        /// ip下拉框事件, 切换ip
        /// </summary>
        private void cbo_ip_SelectedIndexChanged(object sender, EventArgs e)
        {
            hostIP = cbo_ip.SelectedItem as IPAddress;
        }

        /// <summary>
        /// "cancel" 按钮事件
        /// </summary>
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 连接按钮事件, 根据不同文本进行处理
        /// </summary>
        private void btn_connect_Click(object sender, EventArgs e)
        {
            //如果选择做主机
            if (rad_host.Checked)
            {

                //如果主机处于监听状态则停止监听
                if (btn_connect.Text == "停止(&o)")
                {
                    abort = true;
                    listener.Stop();
                    Restore();
                    return;
                }

                //如果处于等待状态则开始异步监听端口35335的连接
                lbl_tip.Text = "等待副机连接...";
                listener = new TcpListener(hostIP, 35335);
                listener.Start();
                ar = listener.BeginAcceptTcpClient(DoAcceptTcpClientCallBack, listener);
                btn_connect.Text = "停止(&o)";
                rad_client.Enabled = false;
                rad_host.Enabled = false;
                btn_cancel.Enabled = false;
                cbo_ip.Enabled = false;
            }

            //如果选择做副机
            else if (rad_client.Checked)
            {
                //如果处于连接主机中的状态, 则取消连接
                if (btn_connect.Text == "取消连接(&o)")
                {
                    abort = true;
                    client.Close();
                    Restore();
                    return;
                }

                //否则开始异步连接主机
                lbl_tip.Text = "连接到主机...";
                try
                {
                    remoteIP = IPAddress.Parse(mtb_remoteIp.Text.Replace(" ", ""));
                }
                catch
                {
                    MessageBox.Show(this, "IP错误");
                    Restore();
                    return;
                }
                client = new TcpClient();
                ar = client.BeginConnect(remoteIP, 35335, DoBeginConnectCallBack, client);
                btn_connect.Text = "取消连接(&o)";
                rad_client.Enabled = false;
                rad_host.Enabled = false;
                mtb_remoteIp.Enabled = false;
                btn_cancel.Enabled = false;
            }
        }

        /// <summary>
        /// 窗口非正常关闭的时候检测是否需要关闭连接
        /// </summary>
        private void form_net_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ar != null && !ar.IsCompleted)
            {
                abort = true;

                if (listener != null)
                {
                    listener.Stop();
                }
                else if (client != null)
                {
                    client.Close();
                }
            }
        }
        #endregion
    }
}
