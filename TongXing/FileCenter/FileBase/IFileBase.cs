using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanNiuSignal.FileCenter.FileBase
{
    /// <summary>
    /// 发送和接收文件的基础接口
    /// </summary>
   public interface IFileBase
    {
       /// <summary>
        /// 文件的缓冲区大小；默认为8192字节;缓冲越大发送速度越快；但同时消耗内存越大；发送和接收文件
        /// 缓冲区大小请设置相同；否则接收的文件数据会不正确;启用UDP传输文件请不要超过65507字节；
       /// </summary>
       int BufferSize
       { get; set; }
       /// <summary>
        /// 得到正在处理中的文件总数
       /// </summary>
       int[] FileAllOn
       { get; }
       /// <summary>
        /// 得到文件名
        /// </summary>
        /// <param name="FileLabel">文件编号</param>
        /// <returns>文件名</returns>
       string GetFileName(int FileLabel);
       /// <summary>
        /// 文件状态；0是等待；1是正在传输；2是暂停
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        /// <returns>文件状态</returns>
       int GetFileState(int FileLabel);
       /// <summary>
        /// 得到文件总长度；以字节为单位
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        /// <returns>文件总长度</returns>
       long GetFileLenth(int FileLabel);
       /// <summary>
        /// 得到文件已处理长度；以字节为单位
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        /// <returns>文件已处理长度</returns>
       long GetFileOk(int FileLabel);
       /// <summary>
        /// 取消发送
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
       void FileCancel(int FileLabel);
       /// <summary>
        /// 暂停发送
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
       void FileStop(int FileLabel);
       /// <summary>
       /// 得到文件的进度；返回大于0小于100的进度数；如果文件已不存在返回100；不抛异常
       /// </summary>
       /// <param name="FileLabel">文件标签</param>
       /// <returns></returns>
       int FileProgress(int FileLabel);
    }
}
