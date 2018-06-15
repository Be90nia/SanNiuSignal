using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using SanNiuSignal.Basics;

namespace SanNiuSignal.FileCenter
{
   internal class FileState
    {
        private int _fileLabel = 0;//文件的标签
        private long _fileLenth = 0;//文件总长度
        private long _fileOkLenth = 0;//已处理的数据量
        private string _fileName = "";//文件地址
        private int _stateFile = 0;//文件状态
        private FileStream _fileStream = null;//文件流
        private StateBase _stateOne = null;
        /// <summary>
        /// 文件已处理量
        /// </summary>
       internal long FileOkLenth
        {
            get { return _fileOkLenth; }
            set { _fileOkLenth = value; }
        }
       /// <summary>
       /// 文件状态；0是等待；1是正在传输；2是暂停
       /// </summary>
        internal int StateFile
        {
            get { return _stateFile; }
            set { _stateFile = value; }
        }
       /// <summary>
       /// 文件地址
       /// </summary>
        internal string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
       /// <summary>
        /// 发送方的构造函数
       /// </summary>
       /// <param name="fileLabel">文件标签</param>
       /// <param name="fileLenth">文件长度</param>
       /// <param name="fileName">文件地址</param>
       /// <param name="fileStream">文件流</param>
        internal FileState(int fileLabel, long fileLenth, string fileName, FileStream fileStream)
        {
            _fileLabel = fileLabel;
            _fileLenth = fileLenth;
            _fileName = fileName;
            _fileStream = fileStream;
        }
       /// <summary>
       /// 发送文件流
       /// </summary>
        internal FileStream Filestream
        {
            get { return _fileStream; }
            set { _fileStream = value; }
        }
        /// <summary>
        /// 文件长度
        /// </summary>
        internal long FileLenth
        {
            get { return _fileLenth; }
        }
        /// <summary>
        /// 文件的标签
        /// </summary>
        internal int FileLabel
        {
            get { return _fileLabel; }
        }
        /// <summary>
        /// StateBase；
        /// </summary>
        internal StateBase StateOne
        {
            get { return _stateOne; }
            set { _stateOne = value; }
        }
    }
    }

