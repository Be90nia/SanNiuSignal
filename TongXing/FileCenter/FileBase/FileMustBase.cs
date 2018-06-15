using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanNiuSignal.PublicTool;

namespace SanNiuSignal.FileCenter.FileBase
{
    internal class FileMustBase : IFileMustBase
    {
        private IFileMustBase fileMustBase = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="MustBase">IFileMustBase</param>
        public FileMustBase(IFileMustBase MustBase)
        {
            fileMustBase = MustBase;
        }
        #region IFileMustBase 成员
        /// <summary>
        /// 对方已经取消这个文件的传输；我方已经关掉这个传输
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        public void FileCancel(int FileLabel)
        {
            CommonMethod.eventInvoket(() => { this.fileMustBase.FileCancel(FileLabel); });
        }
        /// <summary>
        /// 对方暂停；我方也已经暂停；等待着对方的再一次请求传输；会在FileOrNotContingue这里触发
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        public void FileStop(int FileLabel)
        {
            CommonMethod.eventInvoket(() => { this.fileMustBase.FileStop(FileLabel); });
        }
        /// <summary>
        /// 文件已中断；状态已自动改为暂停状态；等待对方上线的时候；进行续传；
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        /// <param name="Reason">中断原因</param>
        public void FileBreak(int FileLabel, string Reason)
        {
            CommonMethod.eventInvoket(() => { this.fileMustBase.FileBreak(FileLabel, Reason); });
        }
        /// <summary>
        /// 文件传输失败
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        public void FileFailure(int FileLabel)
        {
            CommonMethod.eventInvoket(() => { this.fileMustBase.FileFailure(FileLabel); });
        }
        /// <summary>
        /// 文件开始续传；这时不会触发开始传输的方法
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        public void FileContinue(int FileLabel)
        {
            CommonMethod.eventInvoket(() => { this.fileMustBase.FileContinue(FileLabel); });
        }
        /// <summary>
        /// 对方发过来的续传确认信息；你是否同意续传；
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        /// <returns>同意或不同意</returns>
        public bool FileOrNotContingue(int FileLabel)
        {
           object haveBool= CommonMethod.eventInvoket(() => { return this.fileMustBase.FileOrNotContingue(FileLabel); });
           bool haveb = (bool)haveBool;
           return haveb;
        }
        /// <summary>
        /// 对方拒绝续传;文件又处于暂停状态；
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        public void FileNoContinue(int FileLabel)
        {
            CommonMethod.eventInvoket(() => { this.fileMustBase.FileNoContinue(FileLabel); });
        }
        /// <summary>
        /// 得到文件的进度;每次缓冲区为单位折算成百分比输出进度；这样可以提高效率；
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        /// <param name="Progress">文件进度</param>
        public void FileProgress(int FileLabel, int Progress)
        {
            CommonMethod.eventInvoket(() => { this.fileMustBase.FileProgress(FileLabel, Progress); });
        }
        /// <summary>
        /// 内部用的进度；里面会自动计算
        /// </summary>
        /// <param name="state">FileState</param>
        internal void FileProgress(FileState state)
        {
            if (state.FileLenth == state.FileOkLenth)
                FileProgress(state.FileLabel, 100);
            else
            {
                long haveLong = state.FileOkLenth * 100 / state.FileLenth;
                int haveInt = (int)haveLong;
                FileProgress(state.FileLabel, haveInt); 
            }
        }
        #endregion
    }
}
