using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using System.Timers;

namespace RTSSanGuo
{

    public class LogTool
    {         
        public static string dateStr = "2018-01-25";
        static LogTool(){
            dateStr = string.Format("{0:d}", DateTime.Now); //运行时当天的日志,其实这个DateTime.Now 比较消耗时间
        }
        
        public static string InfoLogFile {
            get { return PathTool.LogFileRootFold +"/INFO_"+ dateStr;}
        }

        private static StreamWriter infoWriter = null;
        public static StreamWriter InfoWriter
        {
            get {
                if (infoWriter == null) {              
                    Debug.Log(InfoLogFile);
                    try
                    {
                        if (!Directory.Exists(PathTool.LogFileRootFold))
                        {
                            Directory.CreateDirectory(PathTool.LogFileRootFold);
                        }

                        infoWriter = File.AppendText(InfoLogFile);//不存在则创建，存在则啥也不干
                        infoWriter.AutoFlush = true;
                    }
                    catch (Exception e)
                    {
                        infoWriter = null;
                        Debug.LogError("Create Log File Failed" + e.Message);
                    }
                }
                return infoWriter;
            }
        }

        public static string WarnLogFile
        {
            get { return PathTool.LogFileRootFold + "/WARN_" + dateStr; }
        }

        private static StreamWriter warnWriter = null;
        public static StreamWriter WarnWriter
        {
            get
            {
                if (warnWriter == null)
                {
                    Debug.Log(WarnLogFile);
                    try
                    {
                        if (!Directory.Exists(PathTool.LogFileRootFold))
                        {
                            Directory.CreateDirectory(PathTool.LogFileRootFold);
                        }

                        infoWriter = File.AppendText(WarnLogFile);//不存在则创建，存在则啥也不干
                        infoWriter.AutoFlush = true;
                    }
                    catch (Exception e)
                    {
                        warnWriter = null;
                        Debug.LogError("Create Log File Failed" + e.Message);
                    }
                }
                return warnWriter;
            }
        }
        public static string ErrorLogFile
        {
            get { return PathTool.LogFileRootFold + "/ERROR_" + dateStr; }
        }
        private static StreamWriter errorWriter = null;
        public static StreamWriter ErrorWriter
        {
            get
            {
                if (errorWriter == null)
                {
                    Debug.Log(ErrorLogFile);
                    try
                    {
                        if (!Directory.Exists(PathTool.LogFileRootFold))
                        {
                            Directory.CreateDirectory(PathTool.LogFileRootFold);
                        }

                        infoWriter = File.AppendText(ErrorLogFile);//不存在则创建，存在则啥也不干
                        infoWriter.AutoFlush = true;
                    }
                    catch (Exception e)
                    {
                        errorWriter = null;
                        Debug.LogError("Create Log File Failed" + e.Message);
                    }
                }
                return errorWriter;
            }
        }
        public static string FullLogFile
        {
            get { return PathTool.LogFileRootFold + "/FULL_" + dateStr; }
        }
        private static StreamWriter fullWriter = null;
        public static StreamWriter FullWriter
        {
            get
            {
                if (fullWriter == null)
                {
                    Debug.Log(FullLogFile);
                    try
                    {
                        if (!Directory.Exists(PathTool.LogFileRootFold))
                        {
                            Directory.CreateDirectory(PathTool.LogFileRootFold);
                        }

                        infoWriter = File.AppendText(FullLogFile);//不存在则创建，存在则啥也不干
                        infoWriter.AutoFlush = true;
                    }
                    catch (Exception e)
                    {
                        fullWriter = null;
                        Debug.LogError("Create Log File Failed" + e.Message);
                    }
                }
                return fullWriter;
            }
        }


        public static void LogInfo(string message)
        {   
            string str ="INFO:"+ GetLogTime() + message;
            Debug.Log( str);
            LogToFile(infoWriter, str);
            LogToFile(fullWriter, str);
        }

        public static void LogWarn(string message)
        {
            string str = "WARN:" + GetLogTime() + message;
            Debug.LogWarning(str);
            LogToFile(warnWriter, str);
            LogToFile(fullWriter, str);
        }
        public static void LogError(string message)
        {
            string str = "ERROR:" + GetLogTime() + message;
            Debug.LogError(str);
            LogToFile(errorWriter, str);
            LogToFile(fullWriter, str);
        }       


        private static string GetLogTime()
        {
            DateTime now = DateTime.Now;
            string str = now.ToString("HH:mm:ss.fff") + "=>>";
            return str;
        }

        public static void LogToFile(StreamWriter writer, string message)
        {
            

            if (writer == null)
            {                
               Debug.LogError("No Writer ");               
            }else 
            {
                try
                {
                    writer.WriteLine(message);                    
                }
                catch (Exception e)
                {
                    Debug.LogError("LogToWrite Failed" + e.Message);
                }

            }
        }
    }
}



 
  