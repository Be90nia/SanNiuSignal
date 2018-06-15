using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanNiuSignal.PublicTool;
using SanNiuSignal.PasswordManage;
using SanNiuSignal.FileCenter.FileSend;
using System.Net;
using SanNiuSignal.FileCenter.FileReceive;
using SanNiuSignal.Basics;
using System.Windows.Forms;
namespace SanNiuSignal.FileCenter
{
    /// <summary>
    /// 文件启动中心;在这里你可以启动文件发送和接收项目；
    /// </summary>
   public class FileStart
    {
       internal static SendFile fileSend = null;
       internal static ReceiveFile fileReceive = null;
       /// <summary>
       /// 启动文件发送系统；
       /// </summary>
       /// <param name="sendMust">IFileSendMust</param>
       /// <returns>IFileSend</returns>
       public static IFileSend StartFileSend(IFileSendMust sendMust)
       {
           if (fileSend != null)
               throw new Exception("已经启动了文件发送系统；");
           fileSend = new SendFile(sendMust);
           IFileSend IfileSend = fileSend;
           return IfileSend;
       }
       /// <summary>
       /// 启动文件接收系统
       /// </summary>
       /// <param name="receiveMust">IFileReceiveMust</param>
       /// <returns>IFileReceive</returns>
       public static IFileReceive StartFileReceive(IFileReceiveMust receiveMust)
       {
           if (fileReceive != null)
               throw new Exception("已经启动了文件接收系统；");
           fileReceive = new ReceiveFile(receiveMust);
           IFileReceive IfileReceive = fileReceive;
           return IfileReceive;
       }
       /// <summary>
       /// 文件数据分析处理中心；如果是发送方发过来的数据让接收方去处理
       /// </summary>
       /// <param name="fileDate">文件数据</param>
       /// <param name="stateOne">StateBase</param>
       /// <returns>处理以后的数据</returns>
       internal static byte[] ReceiveDateTO(byte[] fileDate, StateBase stateOne)
       {
           if (fileDate.Length < 7)
               return null;
           byte[] haveDate = null;
           byte code=fileDate[1];
           if (code == PasswordCode._receiveUser)
           {
               if (fileSend == null)//这里没有开启；直接让对方取消
               { haveDate = EncryptionDecryptFile.FileSevenEncryption(PasswordCode._sendUser, PasswordCode._fileCancel, ByteToDate.ByteToInt(3, fileDate)); }
               else { haveDate = fileSend.ReceiveDateTO(fileDate, stateOne); }
           }
           else if (code == PasswordCode._sendUser)
           {
               if (fileReceive == null)//这里没有开启；直接让对方取消
               { haveDate = EncryptionDecryptFile.FileSevenEncryption(PasswordCode._receiveUser, PasswordCode._fileCancel, ByteToDate.ByteToInt(3, fileDate)); }
               else { haveDate = fileReceive.ReceiveDateTO(fileDate, stateOne); }
           }
           return haveDate;
       }
       /// <summary>
       /// 对文件进行续传；只要一个文件标签，会自动分析是发送方还是接收方的文件标签；
       /// </summary>
       /// <param name="fileLable">文件标签</param>
       /// <param name="stateOne">StateBase</param>
       /// <returns></returns>
       internal static byte[] FileContinue(int fileLable, StateBase stateOne)
       {
           byte[] haveDate = null;
           if (fileSend != null)
           {
               haveDate = fileSend.FileContinue(fileLable, stateOne);
           }
           else if (haveDate==null && fileReceive != null)
           {
               haveDate = fileReceive.FileContinue(fileLable, stateOne);
           }
           return haveDate;
       }
       /// <summary>
       /// 中断所有的文件；内部网络全部中断的时候用到的
       /// </summary>
       internal static void FileStopAll()
       {
           if (fileSend != null)
           {
               foreach (FileState state in fileSend.FS)
               {
                   if (state.StateFile == 2)
                       continue;
                   state.StateFile = 2;
                   fileSend.SendMust.FileBreak(state.FileLabel,"断线");
               }
           }
           else if (fileReceive!=null)
           {
               foreach (FileState state in fileReceive.FS)
               {
                   if (state.StateFile == 2)
                       continue;
                   state.StateFile = 2;
                   fileReceive.ReceiveMust.FileBreak(state.FileLabel, "断线");
               }
           }
       }
       /// <summary>
       /// 通过StateBase中断所有的文件；服务器用到的
       /// </summary>
       /// <param name="stateOne">StateBase</param>
       internal static void FileStopITxBase(StateBase stateOne)
       {
           if (fileSend != null)
           {
               foreach (FileState state in fileSend.FS)
               {
                   if (state.StateOne == stateOne)
                   {
                       state.StateFile = 2;
                       fileSend.SendMust.FileBreak(state.FileLabel, "对方下线或断线");
                   }
               }
           }
           else if (fileReceive != null)
           {
               foreach (FileState state in fileReceive.FS)
               {
                   if (state.StateOne == stateOne)
                   {
                       state.StateFile = 2;
                       fileReceive.ReceiveMust.FileBreak(state.FileLabel, "对方下线或断线");
                   }
               }
           }
       }
    }
}
