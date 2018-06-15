using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanNiuSignal.FileCenter.FileBase
{
   /// <summary>
   /// 发送和接收文件必须要处理的一个基础接口
   /// </summary>
  public interface IFileMustBase
    {
        /// <summary>
        /// 对方已经取消这个文件的传输；我方已经关掉这个传输
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        void FileCancel(int FileLabel);
        /// <summary>
        /// 对方暂停；我方也已经暂停；等待着对方的再一次请求传输；会在FileOrNotContingue这里触发
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        void FileStop(int FileLabel);
        /// <summary>
        /// 文件已中断；状态已自动改为暂停状态；等待对方上线的时候；进行续传；
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        /// <param name="Reason">中断原因</param>
        void FileBreak(int FileLabel, string Reason);
        /// <summary>
        /// 文件传输失败
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        void FileFailure(int FileLabel);
        /// <summary>
        /// 文件开始续传；这时不会触发开始传输的方法
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        void FileContinue(int FileLabel);
        /// <summary>
        /// 对方发过来的续传确认信息；你是否同意续传；
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        /// <returns>同意或不同意</returns>
        bool FileOrNotContingue(int FileLabel);
      /// <summary>
      /// 对方拒绝续传;文件又处于暂停状态；
      /// </summary>
      /// <param name="FileLabel">文件标签</param>
        void FileNoContinue(int FileLabel);
      /// <summary>
        /// 得到文件的进度;每次缓冲区为单位折算成百分比输出进度；这样可以提高效率；
      /// </summary>
      /// <param name="FileLabel">文件标签</param>
      /// <param name="Progress">文件进度</param>
        void FileProgress(int FileLabel, int Progress);
    }
}
