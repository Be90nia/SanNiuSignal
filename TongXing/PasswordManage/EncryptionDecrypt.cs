using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SanNiuSignal.Basics;
using SanNiuSignal.PublicTool;
namespace SanNiuSignal.PasswordManage
{
    /// <summary>
    /// 普通数据加密和解密中心
    /// </summary>
    internal class EncryptionDecrypt
    {
        /// <summary>
        /// 对文本和图片数据进行加密;如果长度超过限制，直接抛给文件处理中心
        /// </summary>
        /// <param name="stateCode">StateCode</param>
        /// <param name="state">StateBase</param>
        /// <returns>要发送的数据</returns>
        internal static byte[] encryption(StateCode stateCode,StateBase state)
        {
            byte[] returnByte = null;
            if (stateCode.State == PasswordCode._textCode)
            {
                byte textCode = PasswordCode._textCode;
                byte[] date = Encoding.UTF8.GetBytes(stateCode.Datestring);
                returnByte = encryptionTemporary(date, textCode, state);
            }
            else if (stateCode.State == PasswordCode._photographCode)
            {
                byte photographCode = stateCode.State;
                returnByte = encryptionTemporary(stateCode.DateByte, photographCode, state);
            }
            return returnByte;
        }
       /// <summary>
        /// 一个普通的对数据主体部分进行加密；
       /// </summary>
       /// <param name="date">要加密的数据</param>
       /// <param name="textCode"></param>
        /// <param name="state">StateBase</param>
       /// <returns>加密之后的数据</returns>
        private static byte[] encryptionTemporary(byte[] date, byte textCode,StateBase state)
        {
            if (date.Length > state.BufferSize - 20)
                return EncryptionDecryptSeparateDate.SendHeadEncryption(date, textCode, state);//超出通过文件系统发送
            state.SendDateLabel = RandomPublic.RandomNumber(16787);//给发送的数据进行编号
            byte[] dateOverall = ByteToDate.OffsetEncryption(date, state.SendDateLabel, 2);
            dateOverall[0] = PasswordCode._commonCode; dateOverall[1] = textCode;
            return dateOverall;
        }
        /// <summary>
        /// 对文本和图片数据进行解密;
        /// </summary>
        /// <param name="date">接收到的数据</param>
        /// <param name="state">StateBase</param>
        /// <returns>StateCode</returns>
        internal static StateCode deciphering(byte[] date, StateBase state)
        {
            StateCode stateCode = null;
            if (date.Length < 6)
                return stateCode;//收到的数据不正确
            byte headDate = date[1];
            if (headDate == PasswordCode._textCode || headDate == PasswordCode._photographCode)//解密到的是文本数据
            {
                int SendDateLabel = 0;
                byte[] dateAll = ByteToDate.OffsetDecrypt(date, out SendDateLabel, 2);
                byte[] ReplyDate = ByteToDate.CombinationTwo(PasswordCode._commonCode, PasswordCode._dateSuccess, SendDateLabel);//直接回复发送成功
                if (headDate == PasswordCode._textCode)
                        {
                            string str = Encoding.UTF8.GetString(dateAll);
                            stateCode = new StateCode(PasswordCode._textCode, str, ReplyDate);//解析出来是文本数据
                        }
                else
                        {
                            stateCode = new StateCode(PasswordCode._photographCode, dateAll, ReplyDate);//解释出来是图片数据
                        }
                    
            }
            else if (headDate == PasswordCode._dateSuccess)//数据成功或重发
            {
                int SendDateLabel = ByteToDate.ByteToInt(2, date);//找出已发数据的标签
                if (headDate == PasswordCode._dateSuccess)
                {
                    stateCode = new StateCode(headDate);
                    if (SendDateLabel == state.SendDateLabel)
                    { state.SendDate = null; }//已经成功对已发数据进行删除
                }
            }
            return stateCode;
        }
    }
}