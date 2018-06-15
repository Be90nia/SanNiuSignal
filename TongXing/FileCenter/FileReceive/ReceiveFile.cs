using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanNiuSignal.FileCenter.FileBase;
using SanNiuSignal.PublicTool;
using SanNiuSignal.PasswordManage;
using System.IO;
using System.Net;
using SanNiuSignal.Basics;
using System.Windows.Forms;

namespace SanNiuSignal.FileCenter.FileReceive
{
    /// <summary>
    /// 文件接收处理中心
    /// </summary>
    public class ReceiveFile : FileToBase, IFileReceive
    {
        /// <summary>
        /// 接收方必须实现的一些方法
        /// </summary>
        internal FileReceiveMust ReceiveMust = null;
        /// <summary>
        /// 带参数和构造函数
        /// </summary>
        /// <param name="receiveMust">IFileReceiveMust</param>
        internal ReceiveFile(IFileReceiveMust receiveMust)
        {
            ReceiveMust = new FileReceiveMust(receiveMust);
        }
        /// <summary>
        /// 扩充因要接收文件而增大的缓冲区
        /// </summary>
        /// <param name="state">FileState</param>
        /// <param name="stateOne">stateOne</param>
        private void ExpandBuffer(FileState state, StateBase stateOne)
        {
            if (state.StateOne != stateOne)
            {
                if (stateOne.Buffer.Length != BufferSize)
                {
                    stateOne.BufferBackup = stateOne.Buffer;
                    stateOne.Buffer = new byte[BufferSize];
                }
                state.StateOne = stateOne;
            }
        }
        /// <summary>
        /// 接收方的文件进行续传;如果回复是null;说明不存在
        /// </summary>
        /// <param name="fileLable">文件标签</param>
        /// <param name="stateOne">StateBase</param>
        /// <returns>byte[]</returns>
        internal byte[] FileContinue(int fileLable, StateBase stateOne)
        {
            byte[] haveDate = null;
            FileState state = FileLabelToState(fileLable);
            if (state != null && state.StateFile == 2)
            { 
                haveDate = EncryptionDecryptFile.ReceiveContingueEncryption(PasswordCode._fileContinue, state);
                ExpandBuffer(state, stateOne);
            }
            return haveDate;
        }
        /// <summary>
        /// 发送方发过来的数据；
        /// </summary>
        /// <param name="receiveToDate">收到的数据</param>
        /// <param name="stateOne">StateBase</param>
        /// <returns>需要回复的数据</returns>
        internal byte[] ReceiveDateTO(byte[] receiveToDate, StateBase stateOne)
        {
            byte[] haveDate = null;
            int fileLabel = ByteToDate.ByteToInt(3, receiveToDate);
            byte code = receiveToDate[2];
            if(code==PasswordCode._fileHeadCode)//说明发送过来的是文件是否传输的包头信息
            {
                FileStream fs=null;
                FileState stateHead = EncryptionDecryptFile.FileHeadDecrypt(receiveToDate,fs);
                string fileName= ReceiveMust.ReceiveOrNo(stateHead.FileLabel, stateHead.FileName, stateHead.FileLenth);
                if (fileName == "")
                {haveDate = EncryptionDecryptFile.FileSevenEncryption(PasswordCode._receiveUser, PasswordCode._fileRefuse, fileLabel); }
                else { 
                    fs = CommonMethod.FileStreamWrite(fileName);
                    if (fs == null)
                    { haveDate = EncryptionDecryptFile.FileSevenEncryption(PasswordCode._receiveUser, PasswordCode._fileRefuse, fileLabel); }
                    else
                    {
                        ExpandBuffer(stateHead, stateOne); 
                        stateHead.FileName = fileName; stateHead.Filestream = fs; stateHead.StateFile = 1;
                        
                        FS.Add(stateHead); 
                        haveDate = EncryptionDecryptFile.FileSevenEncryption(PasswordCode._receiveUser, PasswordCode._fileOk, fileLabel); 
                    }
                }
                return haveDate;
            }
            FileState state = FileLabelToState(fileLabel);
            if (state == null)
            {
                if (code == PasswordCode._fileCancel)
                    return null;
                else
                    return EncryptionDecryptFile.FileSevenEncryption(PasswordCode._receiveUser, PasswordCode._fileCancel, fileLabel);
            }
            else
            {
                switch (code)
                {
                    case PasswordCode._fileSubjectCode://收到的是文件主体代码
                        haveDate = EncryptionDecryptFile.FileSubjectDecrypt(state, receiveToDate);
                        ReceiveMust.FileProgress(state);//输出文件进度
                        if (state.FileOkLenth >= state.FileLenth)//说明文件接收完成了
                        { FileRemove(fileLabel); ReceiveMust.ReceiveSuccess(fileLabel); }
                        break;
                    case PasswordCode._fileCancel://对方已经取消了这个文件；
                        FileRemove(fileLabel);
                        ReceiveMust.FileCancel(fileLabel);
                        break;
                    case PasswordCode._sendStop://对方暂停发送
                        FileStopIn(state, ReceiveMust);
                        break;
                    case PasswordCode._fileContinue://对方发过来一个续传的确认信息;你是否同意；
                        ExpandBuffer(state, stateOne);//有可能是stateOne换过了；
                        FileStopIn(state, ReceiveMust);//对方已经暂停了这个文件；我这边肯定也要先暂停掉
                        bool orStop = ReceiveMust.FileOrNotContingue(fileLabel);//让客户确认；是否续传
                        if (orStop)
                        {
                            haveDate = EncryptionDecryptFile.ReceiveContingueEncryption(PasswordCode._fileContinueOk,state);
                            state.StateFile = 1;
                            ReceiveMust.FileContinue(fileLabel);
                        }
                        else { haveDate = EncryptionDecryptFile.FileSevenEncryption(PasswordCode._receiveUser, PasswordCode._fileContinueNo, fileLabel); }
                        break;
                    case PasswordCode._fileContinueOk://对方同意续传
                        state.StateFile = 1;
                        ReceiveMust.FileContinue(fileLabel);
                        haveDate = EncryptionDecryptFile.FileSubjectDecrypt(state, receiveToDate);
                        if (state.FileOkLenth >= state.FileLenth)//说明文件接收完成了
                        { FileRemove(fileLabel); ReceiveMust.ReceiveSuccess(fileLabel); }
                        break;
                    case PasswordCode._fileContinueNo://对方拒绝续传
                        ReceiveMust.FileNoContinue(fileLabel);
                        FileStopIn(state, ReceiveMust);
                        break;
                }
            }
            return haveDate;
        }
    }
}
