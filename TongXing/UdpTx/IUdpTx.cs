using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using SanNiuSignal.Basics;

namespace SanNiuSignal
{
    /// <summary>
    /// Udp通信接口类
    /// </summary>
    public interface IUdpTx : ITxBase
    {
        /// <summary>
        /// 服务器向客户端发送图片数据
        /// </summary>
        /// <param name="ipEndPoint">IPEndPoint</param>
        /// <param name="data">未加密的数据</param>
        void sendMessage(IPEndPoint ipEndPoint, byte[] data);
        /// <summary>
        /// 服务器向客户端发送文本数据
        /// </summary>
        /// <param name="ipEndPoint">IPEndPoint</param>
        /// <param name="data">未加密的数据</param>
        void sendMessage(IPEndPoint ipEndPoint, string data);
        /// <summary>
        /// 发送文件；如果地址等不正确会抛出相应的异常；首先要先到FileStart启动文件发送系统;
        /// </summary>
        /// <param name="ipEndPoint">IPEndPoint</param>
        /// <param name="fileName">文件地址</param>
        /// <returns>返回文件标签；可以控制文件的任何事情</returns>
        int SendFile(IPEndPoint ipEndPoint, string fileName);
        /// <summary>
        /// 对文件进行续传；如果有不正确会抛出相应的异常
        /// </summary>
        /// <param name="ipEndPoint">IPEndPoint</param>
        /// <param name="fileLable">文件标签</param>
        void ContinueFile(IPEndPoint ipEndPoint, int fileLable);
    }
}
