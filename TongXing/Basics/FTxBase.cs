using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.IO;
using SanNiuSignal.PasswordManage;
using SanNiuSignal.PublicTool;
using SanNiuSignal.FileCenter;
namespace SanNiuSignal.Basics
{
    /// <summary>
    /// 一个有基本属性的父类;
    /// </summary>
    public class FTxBase : ITxBase
    {
        #region ITxBase 成员
        /// <summary>
        /// 当引擎非正常原因自动断开的时候触发此事件
        /// </summary>
       public event TxDelegate<string> EngineLost;
        /// <summary>
        /// 当引擎完全关闭释放资源的时候
        /// </summary>
       public event TxDelegate EngineClose;
        /// <summary>
        /// 当接收到文本数据的时候,触发此事件
        /// </summary>
       public event TxDelegate<IPEndPoint, string> AcceptString;
        /// <summary>
        /// 当接收到图片数据的时候,触发此事件
        /// </summary>
       public event TxDelegate<IPEndPoint, byte[]> AcceptByte;
        /// <summary>
        /// 当将数据发送成功且对方已经收到的时候,触发此事件
        /// </summary>
       public event TxDelegate<IPEndPoint> dateSuccess;
        private int _heartTime = 10;//心跳间隔时间
        private int _bufferSize = 1024;//缓冲区大小
        private int _port = 0;
        private string _fileLog = "";//记录地址，如果为空表示不记录
        private IPEndPoint _ipEndPoint = null;//终结点地址项目里面用
       /// <summary>
       /// 本地的终结点地址封装；
       /// </summary>
        internal IPEndPoint IpEndPoint
        {
            get
            {
                try
                {
                    IPAddress ipAddress = null;
                    if (Ip == "")
                        ipAddress = IPAddress.Any;
                    else
                        ipAddress = IPAddress.Parse(CommonMethod.Hostname2ip(Ip));
                    _ipEndPoint = new IPEndPoint(ipAddress, Port);
                    _port = _ipEndPoint.Port;
                }
                catch { throw; }
                    return _ipEndPoint;
            }
        }
       /// <summary>
        /// //客户端引擎是否已经启动;
       /// </summary>
        protected bool _engineStart = false;
        private string _ip = "";//服务器的IP地址
       /// <summary>
       /// ip地址设置和读取
       /// </summary>
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
       /// <summary>
       /// 端口号设置和读取
       /// </summary>
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }
        /// <summary>
        /// 缓冲区大小；默认为1024字节；不影响最大发送量，如果内存够大或经常发送大数据可以适当加大缓冲区
        /// 大小；从而可以提高发送速度；否则会自动分包发送，到达对方自动组包；
        /// </summary>
        public int BufferSize
        {
            get
            {
                return _bufferSize;
            }
            set
            {
                _bufferSize = value;
            }
        }
        /// <summary>
        /// 引擎是否已经启动;
        /// </summary>
        public bool EngineStart
        {
            get { return _engineStart; }
        }
       /// <summary>
        /// 是否记录引擎历史记录；为空表示不记录
       /// </summary>
        public string FileLog
        {
            get { return _fileLog; }
            set { _fileLog = value; }
        }
        /// <summary>
        /// 启动引擎
        /// </summary>
        virtual public void StartEngine()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 关闭引擎释放资源
        /// </summary>
        virtual public void CloseEngine()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region 事件注册成方法
        /// <summary>
        /// 当引擎非正常原因自动断开的时候触发此事件
        /// </summary>
        /// <param name="str">断开原因</param>
        internal void OnEngineLost(string str)
        {
            if (this.EngineLost != null)
            {
                CommonMethod.eventInvoket(() => { this.EngineLost(str); });
                FileStart.FileStopAll();//文件处理那里中断所有的文件
            }
        }
        /// <summary>
        /// 当引擎完全关闭释放资源的时候触发此事件
        /// </summary>
        internal void OnEngineClose()
        {
            if (this.EngineClose != null && _engineStart == true)
            {
                _engineStart = false;
                CommonMethod.eventInvoket(() => { this.EngineClose(); });
                FileStart.FileStopAll();//文件处理那里中断所有的文件
            }
        }
        /// <summary>
        /// 当接收到文本数据的时候,触发此事件
        /// </summary>
        /// <param name="iPEndPoint">对方终结点</param>
        /// <param name="str">文本数据</param>
        internal void OnAcceptString(IPEndPoint iPEndPoint, string str)
        {
            if (this.AcceptString != null)
            {
                CommonMethod.eventInvoket(() => { AcceptString(iPEndPoint, str); });
            }
        }
        /// <summary>
        /// 当接收到图片数据的时候,触发此事件
        /// </summary>
        /// <param name="iPEndPoint">对方终结点</param>
        /// <param name="bytes">图片数据</param>
        internal void OnAcceptByte(IPEndPoint iPEndPoint, byte[] bytes)
        {
            if (this.AcceptByte != null)
            {
                CommonMethod.eventInvoket(() => { this.AcceptByte(iPEndPoint, bytes); });
            }
        }
        /// <summary>
        /// 当将数据发送成功且对方已经收到的时候,触发此事件
        /// </summary>
        /// <param name="iPEndPoint">对方终结点</param>
        internal void OndateSuccess(IPEndPoint iPEndPoint)
        {
            if (this.dateSuccess != null)
            {
                CommonMethod.eventInvoket(() => { this.dateSuccess(iPEndPoint); });
            }
        }
        #endregion
        /// <summary>
        /// 服务器向客户端发送图片数据
        /// </summary>
        /// <param name="stateOne">StateBase</param>
        /// <param name="data">未加密的数据</param>
        internal void sendMessage(StateBase stateOne, byte[] data)
        {
            if (stateOne == null)
                return;
            StateCode stateCode = new StateCode(PasswordCode._photographCode, data);
            byte[] sendDate = EncryptionDecrypt.encryption(stateCode, stateOne);
            stateOne.SendDate = sendDate;
            Send(stateOne, sendDate);
        }
        /// <summary>
        /// 服务器向客户端发送文本数据
        /// </summary>
        /// <param name="stateOne">StateBase</param>
        /// <param name="data">未加密的数据</param>
        internal void sendMessage(StateBase stateOne, string data)
        {
            if (stateOne == null)
                return;
            StateCode stateCode = new StateCode(PasswordCode._textCode, data);
            byte[] sendDate = EncryptionDecrypt.encryption(stateCode, stateOne);
            stateOne.SendDate = sendDate;
            Send(stateOne, sendDate);
        }
        /// <summary>
        /// 向客户端发送数据,最基础的发送
        /// </summary>
        /// <param name="stateOne">StateBase</param>
        /// <param name="data">发送的数据</param>
        virtual internal void Send(StateBase stateOne, byte[] data)
        { }
        /// <summary>
        /// 发送文件；首先要注册文件发送系统；会返回一个整数型的文件标签；用来控制这个文件以后一系列操作
        /// </summary>
        /// <param name="stateOne">StateBase</param>
        /// <param name="fileName">文件地址</param>
        /// <returns>文件标签</returns>
        internal int FileSend(StateBase stateOne,string fileName)
        {
            if (FileStart.fileSend == null)
                throw new Exception("请先注册文件发送系统");
            int haveInt = 0;
            byte[] haveByte = null;
            try
            {
                haveByte = FileStart.fileSend.Send(ref haveInt, fileName, stateOne);
            }
            catch { throw; }
            Send(stateOne, haveByte);
            return haveInt;
        }
        /// <summary>
        /// 对文件进行续传;
        /// </summary>
        /// <param name="stateOne">StateBase</param>
        /// <param name="fileLable">文件标签</param>
        internal void FileContinue(StateBase stateOne,int fileLable)
        {
            byte[] haveDate = FileStart.FileContinue(fileLable, stateOne);
            if (haveDate == null)
                throw new Exception("文件不存在或状态不在暂停状态");
            Send(stateOne, haveDate);
        }
        /// <summary>
        /// 数据第二层分配中心；把数据归类
        /// </summary>
        /// <param name="stateOne"></param>
        /// <param name="statecode"></param>
        internal void codeManage(StateBase stateOne, StateCode statecode)
        {
            if (statecode == null || stateOne == null)
                return;
            StateCode stateCode = null;
            switch (statecode.State)
            {
                case PasswordCode._commonCode://普通数据信息;抛给普通Code去处理
                    stateCode = EncryptionDecrypt.deciphering(statecode.DateByte, stateOne);
                    CommonCodeManage(stateOne, stateCode);
                    break;
                case PasswordCode._bigDateCode://抛给分包Code去处理
                    stateCode = EncryptionDecryptSeparateDate.FileDecrypt(statecode.DateByte, stateOne);
                    CommonCodeManage(stateOne, stateCode);
                    break;
                case PasswordCode._fileCode://抛给文件处理器去处理；如果返回null就不用发送了
                    byte[] haveDate = FileStart.ReceiveDateTO(statecode.DateByte, stateOne);
                    if (haveDate != null)
                        Send(stateOne, haveDate);
                    break;
            }
        }
        /// <summary>
        /// 接收到的普通数据处中心
        /// </summary>
        /// <param name="stateOne">StateBase</param>
        /// <param name="stateCode">StateCode</param>
        internal void CommonCodeManage(StateBase stateOne, StateCode stateCode)
        {
            
            if (stateCode == null || stateOne == null)
                return;
            switch (stateCode.State)
            {
                case PasswordCode._textCode://文本信息
                    Send(stateOne, stateCode.ReplyDate);
                    OnAcceptString(stateOne.IpEndPoint, stateCode.Datestring); //触发文本收到的事件
                    break;
                case PasswordCode._photographCode://图片信息
                    Send(stateOne, stateCode.ReplyDate);
                    OnAcceptByte(stateOne.IpEndPoint, stateCode.DateByte);//触发图片收到的事件
                    break;
                case PasswordCode._dateSuccess://数据发送成功
                    stateOne.SendDate = null;
                    OndateSuccess(stateOne.IpEndPoint);//对方收到触发事件
                    break;
                case 0://说明这个数据只要直接回复给对方就可以了
                    Send(stateOne, stateCode.ReplyDate);
                    break;
            }
        }
        /// <summary>
        /// 设置发送心跳间隔的时间,以秒为单位,默认为10秒
        /// </summary>
        internal int HeartTime
        {
            get
            {
                return _heartTime;
            }
            set
            {
                _heartTime = value;
            }
        }
        /// <summary>
        /// 引擎信息记录
        /// </summary>
        /// <param name="str">信息</param>
        internal void FileOperate(string str)
        {
            try
            {
                CommonMethod.FileOperate(FileLog, str);
            }
            catch { FileLog = ""; }
        }
    }
}
