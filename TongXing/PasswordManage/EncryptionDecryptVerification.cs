using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SanNiuSignal.PublicTool;

namespace SanNiuSignal.PasswordManage
{
    /// <summary>
    /// 一般的验证暗号处理中心；登录和心跳信息等
    /// </summary>
   internal class EncryptionDecryptVerification
    {
       /// <summary>
       /// 对暗号进行加密
       /// </summary>
       /// <param name="Verification">暗号</param>
       /// <returns>加密之后数据</returns>
        internal static byte[] EncryptionVerification(byte Verification)
        {
            byte[] haveDate = new byte[2];
            haveDate[0] = PasswordCode._verificationCode;
            haveDate[1] = Verification;
            return haveDate;
        }
       /// <summary>
       /// 对暗号进行解密
       /// </summary>
       /// <param name="Verification">收到的暗号数据</param>
       /// <returns>解密之后的数据</returns>
        internal static byte DecryptVerification(byte[] Verification)
        {
            if (Verification.Length != 2)
                return 0;
            return Verification[1];
        }
    }
}
