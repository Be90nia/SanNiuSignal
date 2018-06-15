using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanNiuSignal.Basics;

namespace SanNiuSignal
{
    /// <summary>
    /// 一个客户端的接口所有方法和事件都在里面
    /// </summary>
    public interface ITxClient : ITxBase
    {
        /// <summary>
        /// 引擎登录成功或失败都会触发此事件,登录失败的话会有失败的原因
        /// </summary>
        event TxDelegate<bool, string> StartResult;
        /// <summary>
        /// 自动重连开始的时候,触发此事件
        /// </summary>
        event TxDelegate ReconnectionStart;
       /// <summary>
        /// 当连接断开时是否重连,0为不重连,默认重连三次;
       /// </summary>
        int ReconnectMax
        {
            get;
            set;
        }
        /// <summary>
        /// 登录超时时间，默认为10秒
        /// </summary>
        int OutTime
        {
            get;
            set;
        }
        /// <summary>
        /// 客户端向服务器发送图片数据
        /// </summary>
        /// <param name="data">字节数据</param>
        void sendMessage(byte[] data);
        /// <summary>
        /// 客户端向服务器发送文本数据
        /// </summary>
        /// <param name="data">文本数据</param>
        void sendMessage(string data);
        /// <summary>
        /// 发送文件；如果地址等不正确会抛出相应的异常；首先要先到FileStart启动文件发送系统;
        /// </summary>
        /// <param name="fileName">文件地址</param>
        int SendFile(string fileName);
        /// <summary>
        /// 对文件进行续传；如果有不正确会抛出相应的异常
        /// </summary>
        /// <param name="fileLable">文件标签</param>
        void ContinueFile(int fileLable);
    }
}
