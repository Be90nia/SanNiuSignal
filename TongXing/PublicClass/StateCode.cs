using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanNiuSignal.PasswordManage
{
    /// <summary>
    /// 一个普通的工具类,对解码和外部类起到一个桥梁的作用
    /// </summary>
    internal class StateCode
    {
        private byte _state = 0;//状态码
        private byte[] _dateByte = null;//字节数据
        private string _datestring = null;//文本数据
        private int _sendDateLabel = 0;//发送数据的标签
        private byte[] _replyDate = null;//直接回复的数据
        /// <summary>
        /// 直接回复的数据
        /// </summary>
        internal byte[] ReplyDate
        {
            get { return _replyDate; }
        }
        /// <summary>
        /// 提取状态码
        /// </summary>
        internal byte State
        {
            get { return _state; }
        }
        /// <summary>
        /// 提取字节数组
        /// </summary>
        internal byte[] DateByte
        {
            get { return _dateByte; }
        }
        /// <summary>
        /// 提取文本数据
        /// </summary>
        internal string Datestring
        {
            get { return _datestring; }
        }
        /// <summary>
        /// 发送文本数据的时候需要用到的
        /// </summary>
        /// <param name="i">文本代码</param>
        /// <param name="str">文本内容</param>
        internal StateCode(byte i, string str)
        {
            _state = i;
            _datestring = str;
        }
        /// <summary>
        /// 当接收到正确的文本数据的时候要用到的
        /// </summary>
        /// <param name="i">文件代号</param>
        /// <param name="str">文本内容</param>
        /// <param name="replyDate">回复的数据</param>
        internal StateCode(byte i, string str,byte[] replyDate)
        {
            _state = i;
            _datestring = str;
            _replyDate = replyDate;
        }
        /// <summary>
        /// 当接收到正确的字节集数据的时候要用到的
        /// </summary>
        /// <param name="i"></param>
        /// <param name="dateByte"></param>
        /// <param name="replyDate"></param>
        internal StateCode(byte i, byte[] dateByte, byte[] replyDate)
        {
            _state = i;
            _dateByte = dateByte;
            _replyDate = replyDate;
        }
        /// <summary>
        /// 用于只有状态码和字节集
        /// </summary>
        /// <param name="i">归类2,3</param>
        /// <param name="b">字节数组b</param>
        internal StateCode(byte i,byte[] b)
        {
            _state = i;
            _dateByte = b;
        }
        /// <summary>
        /// 对数据进行直接回复
        /// </summary>
        /// <param name="replyDate">字节集</param>
        internal StateCode(byte[] replyDate)
        {
            _replyDate = replyDate;
        }
        /// <summary>
        /// 用与只有已发数据和编号
        /// </summary>
        /// <param name="Label">编号</param>
        /// <param name="dateByte">已发数据</param>
        internal StateCode(int Label, byte[] dateByte)
        {
            _sendDateLabel = Label;
            _dateByte = dateByte;
        }
        /// <summary>
        /// 用与状态码和已发数据编号
        /// </summary>
        /// <param name="state"></param>
        /// <param name="Label"></param>
        internal StateCode(byte state,int Label)
        {
            _state = state;
            _sendDateLabel = Label;
        }
        /// <summary>
        /// 有些不需要数据
        /// </summary>
        /// <param name="i">归类3,4</param>
        internal StateCode(byte i)
        {
            _state = i;
        }
        /// <summary>
        /// 已发数据的标签
        /// </summary>
        internal int SendDateLabel
        {
            get { return _sendDateLabel; }
        }
    }
}
