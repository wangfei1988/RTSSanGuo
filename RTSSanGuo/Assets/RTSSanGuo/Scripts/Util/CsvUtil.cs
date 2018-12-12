using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.ComponentModel;
using System.Linq;
 

    // This class uses Reflection and Linq so it's not the fastest thing in the
    // world; however I only use it in development builds where we want to allow
    // game data to be easily tweaked so this isn't an issue; I would recommend
    // you do the same.
    /*反射的性能损失对于大部分的场景可以忽略不计，只有收入特别高的人（比如月入100K以上）
    Java Spring 大量依赖反射， 不用反射，直接就是代码，当然比用反射快点。
    对反射的性能损失最大的出现在GetType()和GetMethod()这几个操作上面，因为是通过字符串遍历程序集寻找的，
    没有直接调用的快，至于说耗性能，我想说，大概跟你从一堆字符串里面查子串差不多的效果。
    只要你不一万次循环里面每次都GetType...这些东西，就没有性能问题，至于说Invoke/SetValue之类的成员操作，
    性能和你直接操作是一样的。总的来说，反射确实不如直接写性能来得好，因为直接写那个耗时非常非常小，
    有时候几个或者几十个CPU周期就差不多了，而你遍历字符串，虽然可能就几毫秒或者几毫秒都不到，
    但是，依然比前者高出几个数量级。然后事实上，好多东西的对比只拿数量级来对比是有失分寸的，
    不过这也造成了好多人喜欢拿这个吹牛逼的原因。用反射可以省去几百行代码，减少多种现在开发以及将来维护人工编码可能产生的Bug，
    而代价就是多用几毫秒甚至几毫秒不到的时间。那么 ，这又算得了什么呢？

    spring都是大量反射机制，难道不用吗？
    绝大部分系统的性能瓶颈还远远没有到需要考虑反射这里，逻辑层和数据层上的优化对性能的提升比优化反射高n个数量级。
    */

    public  class CsvUtil<T> where T: new()   {

        // Quote semicolons too since some apps e.g. Numbers don't like them ,，
        //如果字段string 本身包含， ；则必须转义保存
         char[] quotedChars = new char[] { ',', ';'};

        FieldInfo[] fi = null; 
        
        public CsvUtil(){
           fi = typeof(T).GetFields( BindingFlags.NonPublic | BindingFlags.Instance);
        }
        
        public  List<T> LoadObjects(string filename, bool strict = true)  {
            using (var stream = File.Open(filename, FileMode.Open)) {
                using (var rdr = new StreamReader(stream)) {
                    return LoadObjects(rdr, strict);
                }
            }
        }

        // Load a CSV into a list of struct/classes from a stream where each line = 1 object
        // First line of the CSV must be a header containing property names
        // Can optionally include any other columns headed with #foo, which are ignored
        // E.g. you can include a #Description column to provide notes which are ignored
        // Field names are matched case-insensitive for convenience
        // @param rdr Input reader
        // @param strict If true, log errors if a line doesn't have enough
        //   fields as per the header. If false, ignores and just fills what it can

        //存储注释
        public List<string> comment = new List<string>();

        public List<T> LoadObjects(TextReader rdr, bool strict = true)  {
            var ret = new List<T>();
            string header="";
            string line = "";
            while((line = rdr.ReadLine()) != null){ //读取一行
                if(line.StartsWith("#")){
                    comment.Add(line);//注释                    
                }
                else{
                    header=line; //第一个非注释行是header
                    break;
                }
            }
            if(header.Length<2){
                //
                return null;
            }             
            var fieldDefs = ParseHeader(header);
            //BindingFlag.Public | BindingFlags.Instance	指定 public 修饰的实例成员 
            //BindingFlag.Public | BindingFlags.Static.	指定 public 修饰的静态成员  静态的所有对象共用，不需要单独存储
            FieldInfo[] fi = typeof(T).GetFields( BindingFlags.NonPublic | BindingFlags.Instance);
            //支持数据类型只有int float double enum 等基础数据类型，引用类型不支持
            //PropertyInfo[] pi = typeof(T).GetProperties( BindingFlags.NonPublic | BindingFlags.Instance);
            //这里只需要字段，不需要属性。属性可能是非基础类型
            //bool isValueType = typeof(T).IsValueType; // 值类型（基础数据类型）还是引用类型  这个其实没必要，一般都是引用类型
            //基础数据类型不需要存储一行  当然struct例外，但是很少使用
            while((line = rdr.ReadLine()) != null) {
                var obj = new T();
                // box manually to avoid issues with structs
                object boxed = obj;
                if (ParseLineToObject(line, fieldDefs, fi, boxed, strict)) {
                    //这一行，每一列都能在对象当中找到对应的字段
                    // unbox value types
                    //if (isValueType)
                    //    obj = (T)boxed;
                    ret.Add(obj);
                }else{
                    //匹配失败
                }
            }
            return ret;
        }


    // Save multiple objects to a CSV file
    // Writes a header line with field names, followed by one line per
    // object with each field value in each column
    // This method throws exceptions if unable to write
    //FileMode.Create 不存在则创建，存在则Truncate 
    public void SaveObjects(IEnumerable<T> objs, string filename) {
            using (var stream = File.Open(filename, FileMode.Create)) {
                using (var wtr = new StreamWriter(stream)) {
                    SaveObjects(objs, wtr);
                }
            }
        }

        // Save multiple objects to a CSV stream
        // Writes a header line with field names, followed by one line per
        // object with each field value in each column
        // This method throws exceptions if unable to write
        public  void SaveObjects(IEnumerable<T> objs, TextWriter w) {
            //FieldInfo[] fi = typeof(T).GetFields();
            if(fi==null || fi.Length==0) return;
            WriteComment(w);//写入注释
            WriteHeader(fi, w); //写入头  这样子顺序不一样了

            bool firstLine = true;
            foreach (T obj in objs) {
                // Good CSV files don't have a trailing newline so only add here
                if (firstLine)
                    firstLine = false;
                else
                    w.Write(Environment.NewLine);

                WriteObjectToLine(obj, fi, w);

            }
        }

        public  void WriteComment(TextWriter w) {
            
            foreach (string line in comment) {
                // Good CSV files don't have a trailing comma so only add here                
                w.Write(line.Replace(Environment.NewLine,"") );
                w.Write(Environment.NewLine);
            }            
        }

        public  void WriteHeader(FieldInfo[] fi, TextWriter w) {
            bool firstCol = true;
            foreach (FieldInfo f in fi) {
                // Good CSV files don't have a trailing comma so only add here
                if (firstCol)
                    firstCol = false;
                else
                    w.Write(",");

                w.Write(f.Name);
            }
            w.Write(Environment.NewLine);
        }

        public  void WriteObjectToLine(T obj, FieldInfo[] fi, TextWriter w) {
            bool firstCol = true;
            foreach (FieldInfo f in fi) {
                // Good CSV files don't have a trailing comma so only add here
                if (firstCol)
                    firstCol = false;
                else
                    w.Write(",");

                string val = f.GetValue(obj).ToString();
                // Quote if necessary   值本身包含,  ;这些就需要转义
                if (val.IndexOfAny(quotedChars) != -1) {
                    val = string.Format("\"{0}\"", val);
                }
                w.Write(val);
            }
        }

        // Parse the header line and return a mapping of field names to column
        // indexes. Columns which have a '#' prefix are ignored.
        public  Dictionary<string, int> ParseHeader(string header) {
            var headers = new Dictionary<string, int>();
            int n = 0;
            foreach(string field in EnumerateCsvLine(header)) {
                var trimmed = field.Trim();
                if (!trimmed.StartsWith("#")) {
                    trimmed = RemoveSpaces(trimmed);
                    headers[trimmed] = n;
                }
                ++n;
            }
            return headers;
        }

        // Parse an object line based on the header, return true if any fields matched
        public  bool ParseLineToObject(string line, Dictionary<string, int> fieldDefs, FieldInfo[] fi,  object destObject, bool strict) {
            //fieldDefs key=字段名  value=字段在第几列
            string[] values = EnumerateCsvLine(line).ToArray();
            bool setAll = true;
            foreach(string field in fieldDefs.Keys) {
                int index = fieldDefs[field];
                //这个字段对应这一行的第几列
                if (index < values.Length) {
                    string val = values[index]; //字段值
                    setAll = SetField(field, val, fi,  destObject) && setAll;
                    //只要有一个找不到对应的字段，setALL就是false
                    //全部成功才返回true
                } else if (strict) {
                    //Debug.LogWarning(string.Format("CsvUtil: error parsing line '{0}': not enough fields", line));
                    //严格模式下csv文件  标题行列数，和其他所有房列数都一致
                }
            }
            return setAll;  //至少匹配上一个
        }
        //找到就返回true
        public  bool SetField(string fieldName, string val, FieldInfo[] fi, object destObject) {
            bool result = false;
            foreach(FieldInfo f in fi) {
                // Case insensitive comparison
                if (string.Compare(fieldName, f.Name, true) == 0) {
                    // Might need to parse the string into the field type
                    object typedVal = f.FieldType == typeof(string) ? val : ParseString(val, f.FieldType);
                    f.SetValue(destObject, typedVal);
                    result = true;
                    break;
                }
            }
            return result;
        }

        public  object ParseString(string strValue, Type t) {
            var cv = TypeDescriptor.GetConverter(t);
            return cv.ConvertFromInvariantString(strValue);
        }

        public  IEnumerable<string> EnumerateCsvLine(string line) {
            // Regex taken from http://wiki.unity3d.com/index.php?title=CSVReader
            //用这个正则表达式分割字符串
            foreach(Match m in Regex.Matches(line,
                @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
                RegexOptions.ExplicitCapture)) {
                yield return m.Groups[1].Value;
            }
        }

        public  string RemoveSpaces(string strValue) {
            return Regex.Replace(strValue, @"\s", string.Empty);
        }
    }
 