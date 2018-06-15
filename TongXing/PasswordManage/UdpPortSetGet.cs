using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanNiuSignal.PublicTool;
using System.Windows.Forms;
namespace SanNiuSignal.PasswordManage
{
   internal class UdpPortSetGet
    {
       /// <summary>
       /// 把一个端口号放到数据里加密
       /// </summary>
       /// <param name="port">端口号</param>
       /// <param name="date">数据</param>
       /// <returns>返回的数据</returns>
       internal static byte[] SetPort(int port,byte[] date)
       { 
          byte[] haveDate=new byte[date.Length+4];
          ByteToDate.IntToByte(port, 0, haveDate);
          date.CopyTo(haveDate,4);
          return haveDate;
       }
       /// <summary>
       /// 取出一个端口号；同时得到去除这个端口号的数据；
       /// </summary>
       /// <param name="date">数据</param>
       /// <returns>返回端口号</returns>
       internal static int GetPort(ref byte[] date)
       {
           int haveInt = ByteToDate.ByteToInt(0, date);
               date = ByteToDate.ByteToByte(date, date.Length - 4, 4);
           return haveInt;
       }
    }
}
