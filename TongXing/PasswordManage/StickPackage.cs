using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanNiuSignal.PublicTool;

namespace SanNiuSignal.PasswordManage
{
    /// <summary>
    /// 一个处理粘包的工具；
    /// </summary>
    internal class StickPackage
    {
        /// <summary>
        /// 对TCP发送进行粘包加密；
        /// </summary>
        /// <param name="sendDate">要加密的数据</param>
        /// <returns>加密之后的数据</returns>
        internal static void EncryptionPackage(ref byte[] sendDate)
        {
            byte[] dateAll = new byte[sendDate.Length + 8];
            ByteToDate.IntToByte(PasswordCode._stickPackageCode, 0, dateAll);
            ByteToDate.IntToByte(sendDate.Length, 4, dateAll);
            sendDate.CopyTo(dateAll, 8);
            sendDate=dateAll;
        }
        /// <summary>
        /// 对TCP粘包数据进行解密；把所有完整的包通过集合形式返回给客户
        /// </summary>
        /// <param name="receiveDate">接收到的数据</param>
        /// <param name="residualpackage">上次残留的数据</param>
        /// <returns>返回的数据集合;</returns>
        internal static List<byte[]> DecryptPackage(byte[] receiveDate, ref byte[] residualpackage)
        {
            List<byte[]> listDate = new List<byte[]>();
            if (receiveDate.Length < 4)
                return listDate;
            while (true)
            {
                byte[] haveDate = DecryptByte(receiveDate, ref residualpackage);
                if (haveDate != null)
                    listDate.Add(haveDate);
                if (haveDate == null || residualpackage == null)
                    break;
                if (residualpackage.Length < 4)
                    break;
                receiveDate = residualpackage; residualpackage = null;
            }
            return listDate;
        }
        private static byte[] DecryptByte(byte[] receiveDate, ref byte[] residualpackage)
        {
            byte[] haveDate = null;
            bool ddd=false;
            int stickPackageCode = ByteToDate.ByteToInt(0, receiveDate);
            if (stickPackageCode != PasswordCode._stickPackageCode)
            {//第一个与暗号不相同；说明数据有可能是前面一个接下去的
                ddd = true;
                if (residualpackage != null)
                { //说明真的是前面一个延续下来的
                    byte[] addDate = new byte[receiveDate.Length + residualpackage.Length];
                    residualpackage.CopyTo(addDate, 0);
                    receiveDate.CopyTo(addDate, residualpackage.Length);
                    receiveDate = addDate; residualpackage = null;
                }
                else//不知道是什么数据，直接扔掉
                { return null; }
            }
            if (ddd)
                stickPackageCode = ByteToDate.ByteToInt(0, receiveDate);
            if (stickPackageCode == PasswordCode._stickPackageCode)
            { //第一个暗号相同，说明是一个新包开始了；不会是一个前面的残留包
                if (receiveDate.Length < 9)
                { residualpackage = receiveDate; return null; }
                int datelenth = ByteToDate.ByteToInt(4, receiveDate);
                residualpackage = null;//对残留数据进行初始化
                if (datelenth == receiveDate.Length - 8)
                { //说明整个收到的数据就是一个完整包
                    haveDate = new byte[datelenth];
                    Array.Copy(receiveDate, 8, haveDate, 0, datelenth);
                }
                else if (datelenth > receiveDate.Length - 8)
                { //说明数据没有收完全,把数据放在残留包里进行下一轮接收
                    residualpackage = receiveDate;
                }
                else
                {//说明有至少二个包粘在一起
                    haveDate = new byte[datelenth];
                    Array.Copy(receiveDate, 8, haveDate, 0, datelenth);
                    residualpackage = new byte[receiveDate.Length - 8 - datelenth];
                    Array.Copy(receiveDate, 8 + datelenth, residualpackage, 0, receiveDate.Length - 8 - datelenth);
                    //把剩下的扔然放在残留里
                }
            }
            return haveDate;
        }
    }
}
