using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanNiuSignal.FileCenter.FileBase;

namespace SanNiuSignal.FileCenter
{
    /// <summary>
    /// 使用文件发送必须实现的一个接口
    /// </summary>
    public interface IFileSendMust : IFileMustBase
    {
       /// <summary>
       /// 发送完成
       /// </summary>
       /// <param name="FileLabel">文件标签</param>
       void SendSuccess(int FileLabel);
       /// <summary>
       /// 接收方拒绝接收文件
       /// </summary>
       /// <param name="FileLabel">文件标签</param>
       void FileRefuse(int FileLabel);
       /// <summary>
       /// 开始传输
       /// </summary>
       /// <param name="FileLabel">文件标签</param>
       void FileStartOn(int FileLabel);
    }
}
