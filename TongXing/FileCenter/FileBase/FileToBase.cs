using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanNiuSignal.PublicTool;
using System.Windows.Forms;
namespace SanNiuSignal.FileCenter.FileBase
{
    /// <summary>
    /// 发送和接收文件的基础类；
    /// </summary>
   public class FileToBase
    {
        internal List<FileState> FS = new List<FileState>();
        private int _bufferSize = 8192;//数据最大长度
       /// <summary>
       /// 文件的缓冲区大小；默认为8192字节;缓冲越大发送速度越快；但同时消耗内存越大；发送和接收文件
       /// 缓冲区大小请设置相同；否则接收的文件数据会不正确
       /// </summary>
        public int BufferSize
        {
            get { return _bufferSize; }
            set { _bufferSize = value; }
        }
        /// <summary>
        /// 得到正在处理中的文件总数
        /// </summary>
        public int[] FileAllOn
        {
            get 
            {
                if (FS.Count == 0)
                    throw new Exception("没有任何文件在发送");
                int[] haveInt = new int[FS.Count];
                int i = 0;
                foreach (FileState state in FS)
                {
                    haveInt[i] = state.FileLabel;
                    i++;
                }
                return haveInt;
            }
        }
       /// <summary>
       /// 判断某个文件是否存在
       /// </summary>
       /// <param name="FileLabel">文件标签</param>
       /// <returns>是否存在</returns>
        public bool FileFind(int FileLabel)
        {
            FileState state = FileLabelToState(FileLabel);
            if (state == null)
            { return false; }
            return true;
        }
        /// <summary>
        /// 得到文件名
        /// </summary>
        /// <param name="FileLabel">文件编号</param>
        /// <returns>文件名</returns>
        public string GetFileName(int FileLabel)
        {
            FileState state = FileLabelToState(FileLabel);
            if (state == null)
            { throw new Exception("这文件已不存在"); }
            string haveStr = CommonMethod.StringRight(state.FileName, "\\");
            return haveStr;
        }
        /// <summary>
        /// 文件状态；0是等待；1是正在传输；2是暂停
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        /// <returns>文件状态</returns>
        public int GetFileState(int FileLabel)
        {
            FileState state = FileLabelToState(FileLabel);
            if (state == null)
            { throw new Exception("这文件已不存在"); }
            return state.StateFile;
        }
        /// <summary>
        /// 得到文件总长度；以字节为单位
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        /// <returns>文件总长度</returns>
        public long GetFileLenth(int FileLabel)
        {
            FileState state = FileLabelToState(FileLabel);
            if (state == null)
            { throw new Exception("这文件已不存在"); }
            return state.FileLenth;
        }
        /// <summary>
        /// 得到文件已处理长度；以字节为单位
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        /// <returns>文件已处理长度</returns>
        public long GetFileOk(int FileLabel)
        {
            FileState state = FileLabelToState(FileLabel);
            if (state == null)
            { throw new Exception("这文件已不存在"); }
            return state.FileOkLenth;
        }
       /// <summary>
       /// 得到文件的进度；返回大于0小于100的进度数；如果文件已不存在返回100；不抛异常
       /// </summary>
       /// <param name="FileLabel">文件标签</param>
       /// <returns></returns>
        public int FileProgress(int FileLabel)
        {
            FileState state = FileLabelToState(FileLabel);
            if (state == null)
                return 100;
            long haveLong = state.FileOkLenth * 100 / state.FileLenth;
            int haveInt = (int)haveLong;
            return haveInt;
        }

        /// <summary>
        /// 取消发送;文件不存在会抛出异常
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        public void FileCancel(int FileLabel)
        {
            try
            {
                int offset=FS.FindIndex(delegate(FileState state1) { return state1.FileLabel == FileLabel; });
                FileState state2 = FS[offset];
                if (state2.Filestream != null)
                    state2.Filestream.Close();//移除一个文件之后把所有的一切都处理掉
                FS.RemoveAt(offset);
                try { int offset1 = FS.FindIndex(delegate(FileState state1) { return state1.StateOne == state2.StateOne; }); }
                catch
                {
                    if (state2.StateOne.Buffer.Length != state2.StateOne.BufferSize)
                    { state2.StateOne.BufferBackup = state2.StateOne.Buffer; state2.StateOne.Buffer = new byte[state2.StateOne.BufferSize]; }
                }
            }
            catch
            { throw new Exception("文件已不存在"); }
        }
       /// <summary>
       /// 取消所有的文件；不抛异常
       /// </summary>
        public void FileCancelAll()
        {
            FS.ForEach(delegate(FileState state)
            {
                state.Filestream.Close(); if (state.StateOne.Buffer.Length != state.StateOne.BufferSize)
                { state.StateOne.BufferBackup = state.StateOne.Buffer; state.StateOne.Buffer = new byte[state.StateOne.BufferSize]; }
            });
            FS = new List<FileState>();
        }
        /// <summary>
        /// 暂停发送;文件不存在或在等待状态会抛出异常；
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        public void FileStop(int FileLabel)
        {
            FileState state = FileLabelToState(FileLabel);
            if (state == null)
            { throw new Exception("这文件已不存在"); }
            if (state.StateFile == 2)
            { throw new Exception("这个文件已处于暂停状态"); }
            if (state.StateFile == 0)
            { throw new Exception("这个文件处于等待状态;不能暂停"); }
            state.StateFile = 2;
        }
       /// <summary>
       /// 暂停所有的;不抛异常
       /// </summary>
        public void FileStopAll()
        {
            foreach (FileState state in FS)
            {
                state.StateFile=2;
            }
        }
       /// <summary>
       /// 移除一个文件；主要是系统内部用的
       /// </summary>
       /// <param name="FileLabel">文件标签</param>
       /// <returns>是否移除成功</returns>
        internal bool FileRemove(int FileLabel)
        {
            try
            {
                FileCancel(FileLabel);
                return true;
            }
            catch
            { return false; }
        }
       /// <summary>
        /// 暂停一个文件发送；内部用的，它在别的状态的时候也可以暂停
       /// </summary>
        /// <param name="state">FileState</param>
        /// <param name="SendMust">IFileSendMust</param>
        internal void FileStopIn(FileState state, IFileMustBase SendMust)
        {
            if (state.StateFile != 2)
            {
                state.StateFile = 2;
                SendMust.FileStop(state.FileLabel);
            }
        }
        /// <summary>
        /// 把文件标签转化成FileState
        /// </summary>
        /// <param name="FileLabel">文件标签</param>
        /// <returns>FileState</returns>
        internal FileState FileLabelToState(int FileLabel)
        {
            try
            {
                
                return FS.Find(delegate(FileState state1) { return state1.FileLabel == FileLabel; });
            }
            catch { return null; }
        }
    }
}
