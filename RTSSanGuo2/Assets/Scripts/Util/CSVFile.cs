using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace RTSSanGuo
{
    public class CSVFile
    {
        public Encoding csvEncoding = Encoding.UTF8; //字符流读写文件时的编码规则
        public string csvFilePath="";
        public char fieldSeprator = ',';
        public string commentStr = "#";
        public List<string> commentLines = new List<string>();
        public List<string[]> valueLines = new List<string[]>();

        public virtual void SaveCsv() {
            if (csvFilePath.Length > 3)
                SaveCsv(csvFilePath);           
        }

        //绝对路径，传入之前计算好
        public virtual void SaveCsv(string fullPath )
        {
            csvFilePath = fullPath;
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            //已经存在则会覆盖
            FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            StreamWriter sw = new StreamWriter(fs, csvEncoding);
              
            //写出注释,注释建议写在前面几行，就算写到中间，保存的时候也会强制写到前面
            for (int i = 0; i < commentLines.Count; i++)
            {                  
                sw.WriteLine(commentLines[i]);
            }
            //写出各行数据
            for (int i = 0; i < valueLines.Count; i++)
            {
                string line = "";
                for (int j = 0; j < valueLines[i].Length; j++)
                {
                    line = line + valueLines[i][j];
                    if (j < valueLines[i].Length - 1)
                    {
                        line += ",";
                    }
                }
                sw.WriteLine(line);
            }
            sw.Close();
            fs.Close();                
        }
        public virtual void  ReadCsv(string filePath)
        {
            csvFilePath = filePath;
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, csvEncoding);

            //记录每次读取的一行记录
            string strLine = "";             
            commentLines.Clear();
            valueLines.Clear();
            while ((strLine = sr.ReadLine()) != null)
            {
                if (strLine.StartsWith(commentStr))
                {
                    commentLines.Add(strLine);
                }
                else {                    
                    string[] filedArr = strLine.Split(fieldSeprator);
                    valueLines.Add(filedArr);
                }                   
            }                
            sr.Close();
            fs.Close();
        }
            
    }
}
