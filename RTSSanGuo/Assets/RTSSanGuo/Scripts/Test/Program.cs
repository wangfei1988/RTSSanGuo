using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
     class Program
    {
        static void Main(string[] args)
        {
            test01();
        }

        static void test01() {
            System.IO.Directory.SetCurrentDirectory("D:\\Dpan\\workspace\\C#\\ConsoleApp1\\ConsoleApp1\\");
            CsvUtil <Student> csvUtil = new CsvUtil<Student>();
            List<Student> list = csvUtil.LoadObjects("student.csv");
            foreach (Student stu in list)
            {
                //Console.Write("id:" + stu.Id + " name :" + stu.Name+"\n" );
            }
            csvUtil.SaveObjects(list, "student2.csv");

        }
    }
}
