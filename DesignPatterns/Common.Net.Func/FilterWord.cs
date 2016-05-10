/***************************************************************************** 
*        filename :FilterWord 
*        描述 : 

*        创建者 TangXiaoJun
*        CLR版本:            4.0.30319.42000 
*        新建项输入的名称:   FilterWord 
*        机器名称:           LD 
*        注册组织名:          
*        命名空间名称:       Common.Net.Func 
*        文件名:             FilterWord 
*        创建系统时间:       2016/2/3 10:09:41 
*        创建年份:           2016 
/*****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Net.Func
{
    public class FilterWord
    {
        /// <summary>替换敏感字和去除HTML标记
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FitlerTXT(string s)
        {
            s = WordStop.ParseChinese(s);//过滤敏感词
            s = NoHTML(s);
            return s;
        }
        /// <summary>替换敏感字和去除HTML标记
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsHaveSensitiveWords(string s)
        {
            return WordStop.SensitiveWords(s);//过滤敏感词
            // s = NoHTML(s);

        }
        /// <summary>去除HTML标记
        /// </summary>
        /// <param name="NoHTML">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string NoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            //Htmlstring=HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }



    }
    class WordStop
    {
        private static ChineseWordsHashCountSet _countTable;
        static WordStop()
        {
            _countTable = new ChineseWordsHashCountSet();
            InitFromFile("ChineseDictionary.txt");
        }
        public static string ParseChinese(string s)
        {
            int _length = s.Length;
            string _s = s;
            string _temp = String.Empty;
            string _FixedWord = String.Empty;
            ArrayList _words = new ArrayList();

            string Fillworld = "";

            for (int i = 0; i < s.Length; )
            {
                _temp = s.Substring(i, 1);

                if (_countTable.GetCount(_temp) >= 0)
                {
                    int j = 2;

                    for (; i + j < s.Length + 1 && _countTable.GetCount(s.Substring(i, j)) >= 0; j++)
                    {
                        if (_countTable.GetCount(s.Substring(i, j)) == 1)
                        {
                            Fillworld = "";

                            _FixedWord = s.Substring(i, j);
                            for (int num = 0; num < j; num++)
                            {
                                Fillworld += "*";
                            }

                            _s = _s.Substring(0, i) + Fillworld + _s.Substring(i + j);
                        }

                    }
                    if (_countTable.GetCount(_temp) == 1)
                    {
                        _s = _s.Substring(0, i) + "*" + _s.Substring(i + 1);
                    }
                    i = i + j - 2;

                    i++;
                    _words.Add(_FixedWord);
                    _FixedWord = "";
                }
                else
                {
                    i++;
                    continue;

                }
            }
            string[] _tempStringArray = new string[_words.Count];
            _words.CopyTo(_tempStringArray);
            return _s;
        }
        public static bool SensitiveWords(string s)
        {
            int _length = s.Length;
            string _s = s;
            string _temp = String.Empty;
            string _FixedWord = String.Empty;
            ArrayList _words = new ArrayList();

            string Fillworld = "";

            for (int i = 0; i < s.Length; )
            {
                _temp = s.Substring(i, 1);

                if (_countTable.GetCount(_temp) >= 0)
                {
                    int j = 2;

                    for (; i + j < s.Length + 1 && _countTable.GetCount(s.Substring(i, j)) >= 0; j++)
                    {
                        if (_countTable.GetCount(s.Substring(i, j)) == 1)
                        {
                            Fillworld = "";

                            _FixedWord = s.Substring(i, j);
                            for (int num = 0; num < j; num++)
                            {
                                return false;
                                Fillworld += "*";
                            }

                            _s = _s.Substring(0, i) + Fillworld + _s.Substring(i + j);
                        }

                    }
                    if (_countTable.GetCount(_temp) == 1)
                    {
                        return false;
                        _s = _s.Substring(0, i) + "*" + _s.Substring(i + 1);
                    }
                    i = i + j - 2;

                    i++;
                    // _words.Add(_FixedWord);
                    _FixedWord = "";
                }
                else
                {
                    i++;
                    continue;

                }
            }
            return true;
            string[] _tempStringArray = new string[_words.Count];
            _words.CopyTo(_tempStringArray);

        }

        /**/
        /// <summary>
        /// 从指定的文件中初始化中文词语字典和字符串次数字典。
        /// </summary>
        /// <param name="fileName">文件名</param>
        public static void InitFromFile(string fileName)
        {
            //string path = Directory.GetCurrentDirectory() + @"\  " + fileName;
            string path = AppDomain.CurrentDomain.BaseDirectory + fileName;

            if (File.Exists(path))
            {

                //using (StreamReader sr = new StreamReader(File.ReadAllText(path, Encoding.Default)))
                using (StreamReader sr = new StreamReader(path, Encoding.Default))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        // ChineseWordUnit _tempUnit = InitUnit(s);
                        ChineseWordUnit _tempUnit = new ChineseWordUnit(s);

                        _countTable.InsertWord(_tempUnit.Word);
                    }
                }
            }
        }

        /**/
        /// <summary>
        /// 将一个字符串解析为ChineseWordUnit。
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>解析得到的ChineseWordUnit</returns>
        private static ChineseWordUnit InitUnit(string s)
        {
            Regex reg = new Regex(@"\s+");
            string[] temp = reg.Split(s);
            if (temp.Length != 2)
            {
                throw new Exception("字符串解析错误：" + s);
            }
            return new ChineseWordUnit(temp[0], Int32.Parse(temp[1]));
        }


    }

    //ChineseWordsHashCountSet.cs //词库容器
    /**/
    /// <summary>
    /// 记录字符串出现在中文字典所录中文词语的前端的次数的字典类。如字符串“中”出现在“中国”的前端，则在字典中记录一个次数。
    /// </summary>
    class ChineseWordsHashCountSet
    {
        /**/
        /// <summary>
        /// 记录字符串在中文词语中出现次数的Hashtable。键为特定的字符串，值为该字符串在中文词语中出现的次数。
        /// </summary>
        private Hashtable _rootTable;

        /**/
        /// <summary>
        /// 类型初始化。
        /// </summary>
        public ChineseWordsHashCountSet()
        {
            _rootTable = new Hashtable();
        }

        /**/
        /// <summary>
        /// 查询指定字符串出现在中文字典所录中文词语的前端的次数。
        /// </summary>
        /// <param name="s">指定字符串</param>
        /// <returns>字符串出现在中文字典所录中文词语的前端的次数。若为-1，表示不出现。</returns>
        public int GetCount(string s)
        {
            if (!this._rootTable.ContainsKey(s.Length))
            {
                return -1;
            }
            Hashtable _tempTable = (Hashtable)this._rootTable[s.Length];
            if (!_tempTable.ContainsKey(s))
            {
                return -1;
            }
            return (int)_tempTable[s];
        }

        /**/
        /// <summary>
        /// 向次数字典中插入一个词语。解析该词语，插入次数字典
        /// </summary>
        /// <param name="s">所处理的字符串。</param>

        public void InsertWord(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                string _s = s.Substring(0, i + 1);
                if (i == s.Length - 1)
                {
                    this.InsertSubString(_s, true);
                }
                else
                {
                    this.InsertSubString(_s, false);
                }

            }
        }

        /**/
        /// <summary>
        /// 向次数字典中插入一个字符串的次数记录。
        /// </summary>
        // <param name="s">所插入的字符串。</param>

        private void InsertSubString(string s, bool IsWord)
        {

            if (!_rootTable.ContainsKey(s.Length) && s.Length > 0)
            {
                Hashtable _newHashtable = new Hashtable();
                _rootTable.Add(s.Length, _newHashtable);
            }
            Hashtable _tempTable = (Hashtable)_rootTable[s.Length];

            if (!IsWord)
            {
                if (!_tempTable.ContainsKey(s))
                {
                    _tempTable.Add(s, 0);
                }
                else
                {
                    if ((int)_tempTable[s] != 1)
                    {
                        _tempTable[s] = 0;
                    }
                    else
                    {
                        _tempTable[s] = 1;
                    }
                }
            }
            else
            {
                if (!_tempTable.ContainsKey(s))
                {
                    _tempTable.Add(s, 1);
                }
                else
                {
                    _tempTable[s] = 1;
                }
            }
        }

    }

    //ChineseWordUnit.cs //strUCt--(词语,权重)对
    struct ChineseWordUnit
    {
        private string _word;
        private int _power;

        /**/
        /// <summary>
        /// 中文词语单元所对应的中文词。

        /// </summary>
        public string Word
        {
            get
            {
                return _word;
            }
        }

        /**/
        /// <summary>
        /// 该中文词语的权重。
        /// </summary>
        public int Power
        {
            get
            {
                return _power;
            }
        }

        /**/
        /// <summary> 
        /// 结构初始化。
        /// </summary>
        /// <param name="word">中文词语</param>
        /// <param name="power">该词语的权重</param>
        public ChineseWordUnit(string word, int power)
        {
            this._word = word;
            this._power = power;
        }

        /**/
        /// <summary>
        /// 结构初始化。
        /// </summary>
        /// <param name="word">中文词语</param>
        /// <param name="power">该词语的权重</param>
        public ChineseWordUnit(string word)
        {
            this._word = word;
            this._power = 1;
        }
    }
}
