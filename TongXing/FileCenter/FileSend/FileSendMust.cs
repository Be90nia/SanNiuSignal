using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanNiuSignal.FileCenter.FileBase;
using SanNiuSignal.PublicTool;

namespace SanNiuSignal.FileCenter.FileSend
{
    internal class FileSendMust : FileMustBase, IFileSendMust
    {
        private IFileSendMust fileSendMust = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="FileSendMust">IFileSendMust</param>
        public FileSendMust(IFileSendMust FileSendMust)
            : base(FileSendMust)
        {
            fileSendMust = FileSendMust;
        }

        #region IFileSendMust 成员

        public void SendSuccess(int FileLabel)
        {
            CommonMethod.eventInvoket(() => { this.fileSendMust.SendSuccess(FileLabel); });
        }

        public void FileRefuse(int FileLabel)
        {
            CommonMethod.eventInvoket(() => { this.fileSendMust.FileRefuse(FileLabel); });
        }

        public void FileStartOn(int FileLabel)
        {
            CommonMethod.eventInvoket(() => { this.fileSendMust.FileStartOn(FileLabel); });
        }

        #endregion
    }
}
