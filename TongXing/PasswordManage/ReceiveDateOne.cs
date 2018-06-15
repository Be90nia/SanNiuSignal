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
    /// 对接收到的数据进行第一次处理；找出需要的数据；把空的去掉
    /// </summary>
   internal class ReceiveDateOne
    {
       /// <summary>
        /// 把缓冲区的数据拿出来；并且把缓冲区清空；
       /// </summary>
        /// <param name="stateOne">StateBase</param>
       /// <param name="insert">数据实际长度</param>
       /// <returns>需要的数据</returns>
       internal static byte[] DateOneManage(StateBase stateOne,int insert)
       {
           byte[] receiveByte = null;
           if (stateOne.Buffer[0] == 0 && stateOne.BufferBackup != null && stateOne.BufferBackup.Length >= insert)
           { receiveByte = stateOne.BufferBackup; stateOne.BufferBackup = null;}//主要用于缓冲区有扩大缩小
           else { receiveByte = stateOne.Buffer;}
           byte[] haveDate = ByteToDate.ByteToByte(receiveByte, insert,0);
           Array.Clear(stateOne.Buffer, 0, stateOne.Buffer.Length);
           return haveDate;
       }
    }
}
