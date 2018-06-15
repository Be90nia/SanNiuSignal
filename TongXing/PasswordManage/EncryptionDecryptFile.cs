using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanNiuSignal.PublicTool;
using SanNiuSignal.FileCenter;
using System.IO;
using System.Windows.Forms;
namespace SanNiuSignal.PasswordManage
{
   internal class EncryptionDecryptFile
    {
       /// <summary>
       /// 发送方对一个文件包头进行加密得到一个byte[]
       /// </summary>
       /// <param name="fileSend"></param>
       /// <returns></returns>
       internal static byte[] FileHeadEncryption(FileState fileSend)
       {
           string fileName = CommonMethod.StringRight(fileSend.FileName,"\\");
           byte[] fileNameByte = Encoding.UTF8.GetBytes(fileName);
           byte[] haveDate = new byte[15 + fileNameByte.Length];
           haveDate[0] = PasswordCode._fileCode;
           haveDate[1] = PasswordCode._sendUser;
           haveDate[2] = PasswordCode._fileHeadCode;
           ByteToDate.IntToByte(fileSend.FileLabel, 3, haveDate);
           ByteToDate.IntToByte(fileSend.FileLenth, 7, haveDate);
           fileNameByte.CopyTo(haveDate, 15);
           return haveDate;
       }
       /// <summary>
       /// 对包头文件进行解密
       /// </summary>
       /// <param name="fileDate"></param>
       /// <param name="fs"></param>
       /// <returns></returns>
       internal static FileState FileHeadDecrypt(byte[] fileDate, FileStream fs)
       {
           int lable = ByteToDate.ByteToInt(3, fileDate);
           long fileLenth = ByteToDate.ByteToLong(7, fileDate);
           byte[] fileNameByte = new byte[fileDate.Length-15];
           Array.Copy(fileDate, 15, fileNameByte, 0, fileNameByte.Length);
           string fileName = Encoding.UTF8.GetString(fileNameByte);
           FileState haveState = new FileState(lable, fileLenth, fileName,fs);
           return haveState;
       }
       /// <summary>
       /// 对文件主体进行加密
       /// </summary>
       /// <param name="fileSend">FileState</param>
       /// <param name="bufferSize">缓冲区大小</param>
       /// <returns>加密之后数据</returns>
       internal static byte[] FileSubjectEncryption(FileState fileSend,int bufferSize)
       {
           byte[] dateOverall = null;
           if (fileSend.StateFile == 2)//说明我这里已经暂停了；要发一个暂停的消息给对方
           { 
               dateOverall = FileSevenEncryption(PasswordCode._sendUser, PasswordCode._sendStop, fileSend.FileLabel);
               return dateOverall;
           }
           int BufferSize = bufferSize - 24;
           if (fileSend.FileLenth - fileSend.FileOkLenth < BufferSize)
               BufferSize = (int)(fileSend.FileLenth - fileSend.FileOkLenth);
           byte[] haveDate = new byte[BufferSize];
           int haveDatelenth = 1;
           try
           {
               haveDatelenth = fileSend.Filestream.Read(haveDate, 0, BufferSize);
           }//异常说明这个文件已经被我方取消掉；返回一个取消信息给对方
           catch { return EncryptionDecryptFile.FileSevenEncryption(PasswordCode._sendUser, PasswordCode._fileCancel, fileSend.FileLabel); }
           if (haveDatelenth <= 0)
           return null; //说明数据发完了,文件会到外面去关掉
           fileSend.FileOkLenth = fileSend.FileOkLenth + haveDatelenth;
           dateOverall = ByteToDate.OffsetEncryption(haveDate, fileSend.FileLabel, 3);
           dateOverall[0] = PasswordCode._fileCode;
           dateOverall[1] = PasswordCode._sendUser;
           dateOverall[2] = PasswordCode._fileSubjectCode;
           return dateOverall;
       }
       /// <summary>
       /// 对文件主体进行解密；
       /// </summary>
       /// <param name="fileSend"></param>
       /// <param name="receiveDate"></param>
       /// <returns></returns>
       internal static byte[] FileSubjectDecrypt(FileState fileSend, byte[] receiveDate)
       {
           byte[] haveDate = null;
           if (fileSend.StateFile == 2)//说明我这里已经暂停了；要发一个暂停的消息给对方
               return FileSevenEncryption(PasswordCode._receiveUser, PasswordCode._sendStop, fileSend.FileLabel);
           haveDate = new byte[receiveDate.Length - 7];
           Array.Copy(receiveDate, 7, haveDate, 0, haveDate.Length);
           try
           {
               fileSend.Filestream.Write(haveDate, 0, haveDate.Length);
           }//异常说明这个文件已经被我方取消掉；返回一个取消信息给对方
           catch { return EncryptionDecryptFile.FileSevenEncryption(PasswordCode._receiveUser, PasswordCode._fileCancel, fileSend.FileLabel); }
           fileSend.FileOkLenth = fileSend.FileOkLenth + haveDate.Length;
           haveDate = FileSevenEncryption(PasswordCode._receiveUser, PasswordCode._dateSuccess, fileSend.FileLabel);
           return haveDate;
       }
       /// <summary>
       /// 对7位数的普通通信进行加密；
       /// </summary>
       /// <param name="whoDate">接收方还是发送方的数据</param>
       /// <param name="code">暗号</param>
       /// <param name="fileLabel">文件标签</param>
       /// <returns>加密完成的数据</returns>
       internal static byte[] FileSevenEncryption(byte whoDate,byte code,int fileLabel)
       { 
           byte[] haveDate=new byte[7];
           haveDate[0] = PasswordCode._fileCode;
           haveDate[1] = whoDate;
           haveDate[2] = code;
           ByteToDate.IntToByte(fileLabel, 3, haveDate);
           return haveDate;
       }
       /// <summary>
       /// 用于接收方；对续传时回复长度的数据进行加密
       /// </summary>
       /// <param name="code">是同意时还是发起方</param>
       /// <param name="fileState">FileState</param>
       /// <returns>加密之后的数据</returns>
       internal static byte[] ReceiveContingueEncryption(byte code, FileState fileState)
       {
           byte[] haveDate = new byte[15];
           haveDate[0] = PasswordCode._fileCode;
           haveDate[1] = PasswordCode._receiveUser;
           haveDate[2] = code;
           ByteToDate.IntToByte(fileState.FileLabel, 3, haveDate);
           ByteToDate.IntToByte(fileState.FileOkLenth, 7, haveDate);
           return haveDate;
       }
    }
}
