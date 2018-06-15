using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanNiuSignal.PublicTool
{
    /// <summary>
    /// 一个数据交换暗号的常量中心
    /// </summary>
   internal class PasswordCode
   {
       #region 普通文件部分 EncryptionDecrypt使用
       /// <summary>
       /// 发送普通信息的代码
       /// </summary>
       internal const byte _commonCode = 33;
       /// <summary>
       /// 发送文本的代码
       /// </summary>
        internal const byte _textCode = 34;
       /// <summary>
        /// 发送的图片的代码
       /// </summary>
        internal const byte _photographCode = 35;
       /// <summary>
        /// 数据已经发送成功的代码;
       /// </summary>
        internal const byte _dateSuccess = 36;
       #endregion
        #region 大数据处理部分 EncryptionDecryptFileTool使用
       /// <summary>
       /// 分包数据使用的代码
       /// </summary>
        internal const byte _bigDateCode = 37;
       /// <summary>
        /// 大文件的包头代码
       /// </summary>
        internal const byte _fileHeadCode = 38;
       /// <summary>
        /// 大文件的主体代码
       /// </summary>
        internal const byte _fileSubjectCode = 39;
       /// <summary>
        /// 同意接收文件的代码
       /// </summary>
        internal const byte _fileAgreeReceive = 40;
        #endregion
        #region 小型代码验证处理中心如心跳等；EncryptionDecryptVerification使用
       /// <summary>
        /// 一般验证需要用的代号
       /// </summary>
        internal const byte _verificationCode = 41;
       /// <summary>
        /// 发送心跳的代码
       /// </summary>
        internal const byte _heartbeatCode = 42;
       /// <summary>
        /// 服务器发给客户端的登录信息
       /// </summary>
        internal const byte _serverToClient = 43;
       /// <summary>
        /// 客户端回给服务器的登录信息
       /// </summary>
        internal const byte _clientToServer = 44;
       /// <summary>
        /// 客户端收到这个信息就会不重连的关掉
       /// </summary>
        internal const byte _clientCloseCode = 45;
        #endregion
        #region 粘包处理中心的代码 StickPackage使用
        /// <summary>
        /// TCP粘包协议的代码
       /// </summary>
        internal const int _stickPackageCode = 14354;
        #endregion
        #region 文件处理部分 EncryptionDecryptFile使用
        /// <summary>
        /// 文件的标示符号
        /// </summary>
        internal const byte _fileCode = 46;
       /// <summary>
       /// 对方拒绝
       /// </summary>
        internal const byte _fileRefuse = 47;
       /// <summary>
       /// 对方同意
       /// </summary>
        internal const byte _fileOk = 48;
       /// <summary>
       /// 发送方的数据
       /// </summary>
        internal const byte _sendUser = 49;
       /// <summary>
       /// 接收方的数据
       /// </summary>
        internal const byte _receiveUser = 50;
       /// <summary>
       /// 暂停发送
       /// </summary>
        internal const byte _sendStop = 51;
       /// <summary>
       /// 对方文件已经取消
       /// </summary>
        internal const byte _fileCancel = 52;
       /// <summary>
       /// 代表是续传数据的代码
       /// </summary>
        internal const byte _fileContinue = 53;
       /// <summary>
       /// 同意续传
       /// </summary>
        internal const byte _fileContinueOk = 54;
       /// <summary>
       /// 拒绝续传
       /// </summary>
        internal const byte _fileContinueNo = 55;
        #endregion
   }
}
