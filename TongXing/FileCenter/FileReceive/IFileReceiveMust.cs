using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanNiuSignal.FileCenter.FileBase;

namespace SanNiuSignal.FileCenter
{
    /// <summary>
    /// 文件接收必须实现的接口;它继承IFileMustBase
    /// </summary>
    public interface IFileReceiveMust : IFileMustBase
    {
        /// <summary>
        /// 接收完成
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        void ReceiveSuccess(int FileLabel);
        /// <summary>
        /// 对方要发送文件过来；你是否同意接收；如果同意请回复文件地址；不同意直接回复空文本；
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        /// <param name="FileName">文件名称</param>
        /// <param name="FileLenth">文件长度</param>
        /// <returns>文件地址</returns>
        string ReceiveOrNo(int FileLabel,string FileName,long FileLenth);
    }
}
