using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using SanNiuSignal.Basics;
using SanNiuSignal.PasswordManage;
using SanNiuSignal.PublicTool;
using System.Threading;
namespace SanNiuSignal
{
    /// <summary>
    /// 面向Udp的主线程类
    /// </summary>
    public class UdpTx : FTxBase, IUdpTx
    {
        private UdpState state = null;
        /// <summary>
        /// 启动Udp系统,端口号在Port属性里设置，否则由系统自动分配可用端口号
        /// </summary>
        override public void StartEngine()
        {
            if (EngineStart)
                return;
            try
            {
                Socket Udpsocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram,
                ProtocolType.Udp);
                Udpsocket.Bind(IpEndPoint);
                //启动异步接收
                state = new UdpState(Udpsocket,BufferSize);
                BeginReceiveFrom();
                _engineStart = true;
                Thread.Sleep(100);
            }
            catch(Exception Ex)
            {
                CloseEngine();
                throw new Exception(Ex.Message); 
            }
        }
        /// <summary>
        /// 接收到信息的回调函数
        /// </summary>
        /// <param name="iar"></param>
        private void EndReceiveFromCallback(IAsyncResult iar)
        {
            int byteRead = 0;
            try
            {
                //完成接收 
                byteRead = state.WorkSocket.EndReceiveFrom(iar, ref state.RemoteEP);
            }
            catch {}
                if (byteRead > 0)
                {
                    state.IpEndPoint = (IPEndPoint)state.RemoteEP;
                    byte[] haveDate = ReceiveDateOne.DateOneManage(state, byteRead);//接收完成之后对数组进行重置
                    BeginReceiveFrom();
                    int havePort = UdpPortSetGet.GetPort(ref haveDate);
                    if (havePort != 0)
                        state.IpEndPoint.Port = havePort;
                    StateCode statecode = ReceiveDateDistribution.Distribution(haveDate);
                    codeManage(state, statecode);
                }
                else
                {
                    BeginReceiveFrom();
                }
        }
        /// <summary>
        /// 异步接收函数
        /// </summary>
        private void BeginReceiveFrom()
        {
            try
            {
                state.WorkSocket.BeginReceiveFrom(
                             state.Buffer, 0, state.Buffer.Length,
                             SocketFlags.None,
                             ref state.RemoteEP,
                             EndReceiveFromCallback,
                             state);
            }
            catch(Exception Ex)
            { OnEngineLost(Ex.Message);//当引擎突然断开触发此事件
                CloseEngine();
            }
        }
        /// <summary>
        /// 向对方发送图片数据
        /// </summary>
        /// <param name="ipEndPoint">IPEndPoint</param>
        /// <param name="data">未加密的数据</param>
        public void sendMessage(IPEndPoint ipEndPoint, byte[] data)
        {
            if (_engineStart == false || state == null)
                return;
            state.IpEndPoint = ipEndPoint;
            sendMessage(state, data);
        }
        /// <summary>
        /// 向对方发送文本数据
        /// </summary>
        /// <param name="ipEndPoint">IPEndPoint</param>
        /// <param name="data">未加密的数据</param>
        public void sendMessage(IPEndPoint ipEndPoint, string data)
        {
            if (_engineStart == false || state == null)
                return;
            state.IpEndPoint = ipEndPoint;
            sendMessage(state, data);
        }
        /// <summary>
        /// 发送文件；如果地址等不正确会抛出相应的异常；首先要先到FileStart启动文件发送系统;
        /// </summary>
        /// <param name="ipEndPoint">IPEndPoint</param>
        /// <param name="fileName">文件地址</param>
        /// <returns>返回文件标签；可以控制文件的任何事情</returns>
        public int SendFile(IPEndPoint ipEndPoint, string fileName)
        {
            if (_engineStart == false || state == null)
                return 0;
            state.IpEndPoint = ipEndPoint;
            try
            {
                int haveInt = FileSend(state, fileName);
                return haveInt;
            }
            catch { throw; }
        }
        /// <summary>
        /// 对文件进行续传；如果有不正确会抛出相应的异常
        /// </summary>
        /// <param name="ipEndPoint">IPEndPoint</param>
        /// <param name="fileLable">文件标签</param>
        public void ContinueFile(IPEndPoint ipEndPoint, int fileLable)
        {
            if (_engineStart == false || state == null)
                return;
            state.IpEndPoint = ipEndPoint;
            try
            {
                FileContinue(state, fileLable);
            }
            catch { throw; }
        }
        /// <summary>
        /// 最基础的Udp异步发送
        /// </summary>
        /// <param name="stateBase">StateBase</param>
        /// <param name="bytes">数据</param>
        override internal void Send(StateBase stateBase, byte[] bytes)
        {
            if (stateBase == null)
                return;
            EndPoint remoteEndPoint = (EndPoint)stateBase.IpEndPoint;
            byte[] sendDate = UdpPortSetGet.SetPort(Port, bytes);
            try
            {
                state.WorkSocket.BeginSendTo(sendDate, 0, sendDate.Length, 0, remoteEndPoint, new AsyncCallback(SendCallback), state.WorkSocket);
            }
            catch { }
            #region 异步发送的另一种方法
            //try
            //{
            //    EndPoint remoteEndPoint = (EndPoint)stateBase.IpEndPoint;
            //    state.SendSocketArgs.SetBuffer(bytes, 0, bytes.Length);
            //    state.SendSocketArgs.RemoteEndPoint = remoteEndPoint;
            //   bool bb= state.WorkSocket.SendToAsync(state.SendSocketArgs);
            //   if (bb == false)
            //   {
            //       state.SendSocketArgs = new SocketAsyncEventArgs();
            //       Send(stateBase, bytes);
            //   }
            //}
            //catch {}
            #endregion
        }
        /// <summary>
        /// 发送完数据之后的回调函数
        /// </summary>
        /// <param name="ar">Clicent</param>
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);
            }
            catch
            {
            }
        }
        /// <summary>
        /// 关闭Udp引擎,释放所有资源
        /// </summary>
        override public void CloseEngine()
        {
            try
            {
                if (state.WorkSocket != null)
                {
                    state.WorkSocket.Close();
                    state.WorkSocket = null;
                }
                state.SendSocketArgs.Dispose();
                OnEngineClose();
            }
            catch { }
        }
    }
    }
