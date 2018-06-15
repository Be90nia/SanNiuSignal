using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace SanNiuSignal.PublicTool
{
    /// <summary>
    /// 普通方法工具箱
    /// </summary>
   internal class CommonMethod
    {
        /// <summary>
        /// 域名转换为IP地址
        /// </summary>
        /// <param name="hostname">域名或IP地址</param>
        /// <returns>IP地址</returns>
        internal static string Hostname2ip(string hostname)
        {
            try
            {
                IPAddress ip;
                if (IPAddress.TryParse(hostname, out ip))
                    return ip.ToString();
                else
                    return Dns.GetHostEntry(hostname).AddressList[0].ToString();
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 服务器信息记录
        /// </summary>
        /// <param name="FileLog">记录地址</param>
        /// <param name="str">记录内容</param>
        internal static void FileOperate(string FileLog,string str)
        {
            if (FileLog == "")
                return;
            try
            {
                FileStream fs = new FileStream(FileLog, FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.WriteLine(str + DateTime.Now.ToString());
                sw.Close();
                fs.Close();
            }
            catch { throw; }
        }
        /// <summary>
        /// 外部调用是否需要用Invoket
        /// </summary>
        /// <param name="func">事件参数</param>
        internal static void eventInvoket(Action func)
        {
            Form form = Application.OpenForms.Cast<Form>().FirstOrDefault();
            if (form != null && form.InvokeRequired)
            {
                form.Invoke(func);
            }
            else
            {
                func();
            }
        }
       /// <summary>
        /// 具有返回值的 非bool 外部调用是否需要用Invoket
       /// </summary>
        /// <param name="func">方法</param>
       /// <returns>返回客户操作之后的数据</returns>
        internal static object eventInvoket(Func<object> func)
        {
            object haveStr;
            Form form = Application.OpenForms.Cast<Form>().FirstOrDefault();
            if (form != null && form.InvokeRequired)
            {
               haveStr=form.Invoke(func);
            }
            else
            {
                haveStr = func();
            }
            return haveStr;
        }
       /// <summary>
        /// 取文本中某个文本的右边文本
       /// </summary>
       /// <param name="AllDate">总文本</param>
       /// <param name="offstr">标志文本</param>
       /// <returns>取出的文本</returns>
        internal static string StringRight(string AllDate,string offstr)
        {
            int lastoff = AllDate.LastIndexOf(offstr)+offstr.Length;
            string haveString = AllDate.Substring(lastoff, AllDate.Length - lastoff);
            return haveString;
        }
       /// <summary>
       /// throw文本过滤;
       /// </summary>
       /// <param name="str">原文本</param>
        /// <returns>过滤之后的文本</returns>
        internal static string BetweenThrow(string str)
        {
            int dd=str.IndexOf(":");
            if (dd == 0)
                return str;
           return Between(str, ":", ".");
        }
        /// <summary>  
        /// 取文本中间内容  
        /// </summary>  
        /// <param name="str">原文本</param>  
        /// <param name="leftstr">左边文本</param>  
        /// <param name="rightstr">右边文本</param>  
        /// <returns>返回中间文本内容</returns>  
        internal static string Between(string str, string leftstr, string rightstr)
        {
            int i = str.IndexOf(leftstr) + leftstr.Length;
            string temp = str.Substring(i, str.IndexOf(rightstr, i) - i);
            return temp;
        }
       /// <summary>
        /// 读文件操作；如果打开正常返回文件流；异常返回null
       /// </summary>
       /// <param name="fileName">文件地址</param>
       /// <returns>文件流</returns>
        internal static FileStream FileStreamRead(string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                return fs;
            }
            catch { return null; }
        }
       /// <summary>
        /// 写文件操作；如果打开正常返回文件流；异常返回null
       /// </summary>
        /// <param name="fileName">文件地址</param>
        /// <returns>FileStream</returns>
        internal static FileStream FileStreamWrite(string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                return fs;
            }
            catch { return null; }
        
        }
    }
}
