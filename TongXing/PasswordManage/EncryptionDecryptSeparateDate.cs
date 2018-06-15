using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanNiuSignal.Basics;
using SanNiuSignal.PublicTool;
using System.Windows.Forms;
namespace SanNiuSignal.PasswordManage
{
    /// <summary>
    /// 大包数据处理中心
    /// </summary>
   internal class EncryptionDecryptSeparateDate
    {
       /// <summary>
       /// 发送文件初始化；发送文件前先发一个小包让对方进行确认
       /// </summary>
       /// <param name="date">数据</param>
       /// <param name="textCode">什么文件</param>
       /// <param name="state">StateBase</param>
       /// <returns>加密之后的包头</returns>
       internal static byte[] SendHeadEncryption(byte[] date, byte textCode, StateBase state)
       {
           state.SendFile = new FileBase(date);
           state.SendFile.FileLabel = RandomPublic.RandomNumber(14562);
           byte[] headDate = new byte[11];
           headDate[0] = PasswordCode._bigDateCode; headDate[1] = PasswordCode._fileHeadCode; headDate[2] = textCode;
           ByteToDate.IntToByte(state.SendFile.FileLabel, 3, headDate);
           ByteToDate.IntToByte(date.Length, 7, headDate);
           return headDate;
       }
       /// <summary>
       /// 对文件主体部分进行加密
       /// </summary>
       /// <param name="date">数据</param>
       /// <param name="sendDateLabel">标签</param>
       /// <returns>加密之后的数据</returns>
       private static byte[] SendSubjectEncryption(byte[] date, int sendDateLabel)
       {
           byte[] dateOverall = ByteToDate.OffsetEncryption(date, sendDateLabel,2);
           dateOverall[0] = PasswordCode._bigDateCode;
           dateOverall[1] = PasswordCode._fileSubjectCode;
           return dateOverall;
       }
       /// <summary>
       /// 当收到是分组数据代码的到这里来统一处理
       /// </summary>
       /// <param name="date">数据</param>
       /// <param name="state">StateBase</param>
       /// <returns>StateCode</returns>
       internal static StateCode FileDecrypt(byte[] date, StateBase state)
       {
           StateCode stateCode = null;
           if (date.Length < 6)
               return stateCode;
           byte headDate = date[1];
           if (headDate == PasswordCode._fileAgreeReceive)
           {//对方同意接收文件;我应该怎么处理
               int FileLabel = ByteToDate.ByteToInt(2, date);
               
               if (state.SendFile != null && state.SendFile.FileLabel == FileLabel)
               {
                   byte[] SendSubjectDate = FileGetSendDate(state);
                   if (SendSubjectDate == null)
                       stateCode = new StateCode(PasswordCode._dateSuccess);
                   else
                       stateCode = new StateCode(SendSubjectDate);//直接发送
               }
           }
           else if (headDate == PasswordCode._dateSuccess)
           {//对方已经接收到数据
               int FileLabel = ByteToDate.ByteToInt(2, date);
               if (state.SendFile != null && state.SendFile.FileLabel == FileLabel)
               {
                   byte[] SendSubjectDate = FileGetSendDate(state);
                   if (SendSubjectDate == null)
                       stateCode = new StateCode(PasswordCode._dateSuccess);
                   else
                       stateCode = new StateCode(SendSubjectDate);//直接发送
               }
           }
           //上面是发送方接收要做的;下面是接收方发送要做的事情
           else if (headDate == PasswordCode._fileHeadCode)
           {//收到的是文件包头部分
               byte whatCode = date[2];
               int fileLabel = ByteToDate.ByteToInt(3, date);
               int fileLenth = ByteToDate.ByteToInt(7, date);
               state.ReceiveFile = new FileBase(whatCode, fileLabel, fileLenth);
               byte[] dateAll = new byte[6];
               dateAll[0] = PasswordCode._bigDateCode;
               dateAll[1] = PasswordCode._fileAgreeReceive;
               ByteToDate.IntToByte(fileLabel, 2, dateAll);
               stateCode = new StateCode(dateAll);
           }
           else if (headDate == PasswordCode._fileSubjectCode)
           {//收到的是文件主体部分
               int SendDateLabel = 0;
               byte[] dateAll = ByteToDate.OffsetDecrypt(date, out SendDateLabel, 2);
               byte[] ReplyDate = ByteToDate.CombinationTwo(PasswordCode._bigDateCode, PasswordCode._dateSuccess, state.ReceiveFile.FileLabel);
               if (state.ReceiveFile.FileDateAll == null)
               {
                   state.ReceiveFile.FileDateAll = dateAll;//是第一次接收到主体数据
                   stateCode = new StateCode(ReplyDate); 
               }
               else
               {
                   byte[] FileDateAll = new byte[state.ReceiveFile.FileDateAll.Length + dateAll.Length];
                   state.ReceiveFile.FileDateAll.CopyTo(FileDateAll, 0);
                   dateAll.CopyTo(FileDateAll, state.ReceiveFile.FileDateAll.Length);
                   state.ReceiveFile.FileDateAll = FileDateAll;
                   if (FileDateAll.Length == state.ReceiveFile.FileLenth)
                   {
                       if (state.ReceiveFile.FileClassification == PasswordCode._textCode)
                       {
                           string str = Encoding.UTF8.GetString(FileDateAll);
                           stateCode = new StateCode(PasswordCode._textCode, str, ReplyDate);
                       }
                       else
                       {
                           stateCode = new StateCode(PasswordCode._photographCode, FileDateAll, ReplyDate);
                       }
                       state.ReceiveFile = null;//文件接收完成；释放接收器
                   }
                   else
                   { stateCode = new StateCode(ReplyDate); }
               }
           }
           return stateCode;
       }
       /// <summary>
       /// 直接从这里提取一个要发送的主体字节集数据;已经加密完成
       /// </summary>
       /// <param name="state">StateBase</param>
       /// <returns>字节集</returns>
       private static byte[] FileGetSendDate(StateBase state)
       {
           int Date_Max = state.BufferSize- 24;
           if (state.SendFile.FileDateAll == null)
               return null;//说明文件已经发完了
           int FileDateAllLenth = state.SendFile.FileDateAll.Length;
           if (FileDateAllLenth <= Date_Max)
           {
               state.SendFile.SendDate = SendSubjectEncryption(state.SendFile.FileDateAll, state.SendFile.FileLabel);
               state.SendFile.FileDateAll = null;
           }else 
           {
               byte[] dateS = new byte[Date_Max];
               byte[] dateL = new byte[FileDateAllLenth-Date_Max];
               Array.Copy(state.SendFile.FileDateAll, 0, dateS, 0, Date_Max);
               Array.Copy(state.SendFile.FileDateAll, Date_Max, dateL, 0, FileDateAllLenth - Date_Max);
               state.SendFile.FileDateAll = dateL;
               state.SendFile.SendDate = SendSubjectEncryption(dateS, state.SendFile.FileLabel);
           }
           return state.SendFile.SendDate;
       }
       
    }
}
