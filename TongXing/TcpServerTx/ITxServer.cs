using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using SanNiuSignal.Basics;

namespace SanNiuSignal
{
    /// <summary>
    /// 一个服务器的接口,所有方法和事件都在里面
    /// </summary>
    public interface ITxServer : ITxBase
    {
        /// <summary>
        /// 当有客户连接成功的时候,触发此事件
        /// </summary>
        event TxDelegate<IPEndPoint> Connect;
        /// <summary>
        /// 当有客户突然断开的时候,触发此事件,文本参数是代表断开的原因
        /// </summary>
        event TxDelegate<IPEndPoint, string> Disconnection;
       /// <summary>
       /// 当前客户端数量
       /// </summary>
       int ClientNumber
        {
            get;
        }
       /// <summary>
        /// 允许最多客户端数
       /// </summary>
       int ClientMax
       {
           get;
           set;
       }
       /// <summary>
       /// 得到所有的客户端
       /// </summary>
       List<IPEndPoint> ClientAll
       {
           get;
       }
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
       /// <summary>
       /// 服务器强制关闭一个客户端
       /// </summary>
       /// <param name="ipEndPoint">IPEndPoint</param>
       void clientClose(IPEndPoint ipEndPoint);
       /// <summary>
       /// 检查某个客户端是否在线
       /// </summary>
       /// <param name="ipEndPoint">IPEndPoint</param>
       /// <returns>bool</returns>
       bool clientCheck(IPEndPoint ipEndPoint);
        /// <summary>
        /// 关闭所有客户端连接
        /// </summary>
       void clientAllClose();
    }
}
