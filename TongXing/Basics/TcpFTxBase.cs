using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanNiuSignal.PasswordManage;
using SanNiuSignal.PublicClass;
using SanNiuSignal.PublicTool;
using System.Windows.Forms;
namespace SanNiuSignal.Basics
{
    /// <summary>
    /// 主要是继承FTxBase;还有一些TCP协议需要用到的一些方法；
    /// </summary>
    public class TcpFTxBase : FTxBase
    {
        /// <summary>
        /// 当Tcp收到数据全部在这里处理;也是数据的第一次处理
        /// </summary>
        /// <param name="stateOne">TcpState</param>
        /// <param name="reciverByte">数据</param>
        internal void TcpDateOne(TcpState stateOne, byte[] reciverByte)
        {
            stateOne.HeartTime = DateTime.Now;
            List<byte[]> listDate = StickPackage.DecryptPackage(reciverByte, ref stateOne.Residualpackage);
            
            foreach (byte[] date in listDate)
            {
                StateCode statecode = ReceiveDateDistribution.Distribution(date);
                TcpCodeManage(stateOne, statecode);
            }
        }
        /// <summary>
        /// TCP协议使用的数据第二层分配中心；把数据归类;
        /// </summary>
        /// <param name="stateOne"></param>
        /// <param name="statecode"></param>
        internal void TcpCodeManage(TcpState stateOne, StateCode statecode)
        {
            if (statecode == null || stateOne == null)
                return;
            if (statecode.State == PasswordCode._verificationCode)//说明是暗号；抛给暗号处理中心
            {
                byte haveDate = EncryptionDecryptVerification.DecryptVerification(statecode.DateByte);
                VerificationCodeManage(stateOne, haveDate);
            }
            else
            {
                codeManage(stateOne, statecode); }
        }
        /// <summary>
        /// 要被TCP主类重写的；关于暗号怎么处理的类；
        /// </summary>
        /// <param name="stateOne">TcpState</param>
        /// <param name="haveDate">字节</param>
        virtual internal void VerificationCodeManage(TcpState stateOne, byte haveDate)
        { }
    }
}
