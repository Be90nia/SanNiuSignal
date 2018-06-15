using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanNiuSignal.PublicTool
{
    /// <summary>
    /// 数据计算的一些方法
    /// </summary>
   internal class ByteToDate
    {
        /// <summary>
        /// 把一个整数和二个字节组成新的字节数组
        /// </summary>
        /// <param name="a">字节</param>
        /// <param name="b">字节</param>
        /// <param name="c">整数</param>
        /// <returns>完成后的数组</returns>
        internal static byte[] CombinationTwo(byte a, byte b, int c)
        {
            byte[] dateOverall = new byte[6];
            dateOverall[0] = a;
            dateOverall[1] = b;
            IntToByte(c, 2, dateOverall);
            return dateOverall;
        }
        /// <summary>
        /// 从一个字节数组里面取出一个整数
        /// </summary>
        /// <param name="a">起始位置</param>
        /// <param name="b">字节数组</param>
        /// <returns>整数</returns>
        internal static int ByteToInt(int a, byte[] b)
        {
            byte[] inta = new byte[4];
            Array.Copy(b, a, inta, 0, 4);
            int dl = BitConverter.ToInt32(inta, 0);
            return dl;
        }
        /// <summary>
        /// 从一个字节数组里面取出一个长整数
        /// </summary>
        /// <param name="a">起始位置</param>
        /// <param name="b">字节数组</param>
        /// <returns>长整数</returns>
        internal static long ByteToLong(int a, byte[] b)
        {
            byte[] inta = new byte[8];
            Array.Copy(b, a, inta, 0, 8);
            long dl = BitConverter.ToInt64(inta,0);
            return dl;
        }
        /// <summary>
        /// 把一个整数Copy到一个字节数组的指定位置
        /// </summary>
        /// <param name="a">整数</param>
        /// <param name="b">起始位置</param>
        /// <param name="c">字节数组</param>
        internal static void IntToByte(int a, int b, byte[] c)
        {
            byte[] inta = BitConverter.GetBytes(a);
            inta.CopyTo(c, b);
        }
        /// <summary>
        /// 把一个长整数Copy到一个字节数组的指定位置
        /// </summary>
        /// <param name="a">长整数</param>
        /// <param name="b">起始位置</param>
        /// <param name="c">字节数组</param>
        internal static void IntToByte(long a, int b, byte[] c)
        {
            byte[] inta = BitConverter.GetBytes(a);
            inta.CopyTo(c, b);
        }
        /// <summary>
        /// 把一个数组取出指定长度
        /// </summary>
        /// <param name="a">数据</param>
        /// <param name="b">长度</param>
        /// <param name="index">起始位置</param>
        /// <returns>返回的数据</returns>
        internal static byte[] ByteToByte(byte[] a, int b,int index)
        {
                byte[] haveDate = new byte[b];
                Array.Copy(a, index, haveDate, 0, b);
                return haveDate;
        }
        /// <summary>
        /// 通过偏移量对一段数据进行加密；把标签和数据当做一个整体；
        /// </summary>
        /// <param name="date">需要加密的数据</param>
        /// <param name="sendDateLabel">标签</param>
        /// <param name="offset">偏移量</param>
        /// <returns>加密完成的数据</returns>
        internal static byte[] OffsetEncryption(byte[] date, int sendDateLabel, int offset)
        {
            byte[] dateOverall = new byte[date.Length + 4 + offset];
            IntToByte(sendDateLabel, offset, dateOverall);
            date.CopyTo(dateOverall, 4 + offset);
            return dateOverall;
        }
        /// <summary>
        /// 通过偏移量对一段数据进行解密；
        /// </summary>
        /// <param name="date">待解密的数据</param>
        /// <param name="sendDateLabel">标签</param>
        /// <param name="offset">偏移量</param>
        /// <returns>解密出来的数据</returns>
        internal static byte[] OffsetDecrypt(byte[] date, out int sendDateLabel, int offset)
        {
            sendDateLabel = ByteToInt(offset, date);
            byte[] dateOverall = new byte[date.Length - 4 - offset];
            Array.Copy(date, 4 + offset, dateOverall, 0, dateOverall.Length);
            return dateOverall;
        }
    }
}
