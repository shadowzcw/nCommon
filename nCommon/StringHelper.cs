using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace nCommon
{
    public static class StringHelper
    {
        /// <summary>
        /// 转成数字字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToNumber(this string str)
        {
            return String.IsNullOrEmpty(str) ? string.Empty : Regex.Replace(str, @"\D+", "");
        }

        public static Guid ToGuid(this string str)
        {
            if (str == string.Empty) return Guid.Empty;

            try
            {
                return new Guid(str);
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public static string GetLines(this string str, int lines)
        {
            var rows = str.Split('\n');
            var result = string.Empty;
            foreach (var line in rows)
            {
                result += line;
            }
            return result;
        }

        /// <summary>
        /// 转成整数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        /// 
        public static int ToInt(this string str, int defaultValue)
        {
            int result;
            int.TryParse(str, out result);
            return result == 0 ? defaultValue : result;
        }
        public static int ToInt(this string str)
        {
            return ToInt(str, 0);
        }

        public static int? ToIntOrNull(this string str)
        {
            return str == string.Empty ? (int?)null : str.ToInt();
        }

        public static IList<int> ToIntList(this string str)
        {
            return str.Split(',').Select(c => c.ToInt()).ToList();
        }

        public static IList<string> ToNumberList(this string str)
        {
            var list = new List<string>();
            if (str == string.Empty)
                return list;
            list.AddRange(str.Split(',').Select(c => c.ToNumber()));
            return list;
        }

        /// <summary>
        /// 转成DateTime
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str, DateTime defaultValue)
        {
            try
            {
                return DateTime.Parse(str.Trim());
            }
            catch
            {
                return defaultValue;
            }
        }
        /// <summary>
        /// 转成DateTime
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str)
        {
            return ToDateTime(str, DateTime.MinValue);
        }

        public static decimal ToMoney(this string str, decimal defaultValue)
        {
            try
            {
                return decimal.Parse(str.Trim());
            }
            catch
            {
                return defaultValue;
            }
        }

        public static decimal ToMoney(this string str)
        {
            return ToMoney(str, -1m);
        }

        /// <summary>
        /// 转成浮点类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToFloat(this string str)
        {
            if (String.IsNullOrEmpty(str))
                return -1;
            str = Regex.Replace(str, "[^0-9^-^.]", string.Empty);
            if (str != string.Empty)
                str = str.GetLeft(1) + str.Substring(1).Replace('-', '\0');

            var dotIndex = str.IndexOf('.');
            if (dotIndex != -1)
            {
                str = str.Substring(0, dotIndex) + "." + str.Substring(dotIndex + 1).Replace('.', '\0');

                int result;
                int.TryParse(str, out result);
                return result;
            }
            return float.Parse(str);
        }

        /// <summary>
        /// 获取重复字符串
        /// </summary>
        /// <param name="str">字符</param>
        /// <param name="times">重复次数</param>
        /// <returns></returns>
        public static string GetRepeater(this string str, int times)
        {
            if (times >= 1)
            {
                var sb = new StringBuilder();
                for (var i = 1; i < times; i++)
                {
                    sb.Append(str);
                }
                return sb.ToString();
            }
            return string.Empty;
        }
        /// <summary>
        /// 转成布尔
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ToBool(this string str)
        {
            str = str.ToUpper();
            return str == "TRUE" || str == "1";
        }
        /// <summary>
        /// 转成布尔或null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool? ToBoolOrNull(this string str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                if (str.ToUpper() == "TRUE" || str == "1")
                    return true;
                if (str.ToUpper() == "FALSE" || str == "0")
                    return false;
                return null;
            }

            return null;
        }
        /// <summary>
        /// 转成1或0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToBit(this bool str)
        {
            return str ? "1" : "0";
        }

        /// <summary>
        /// 转成MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMd5(this string str)
        {
            using (var provider = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                return BitConverter.ToString(provider.ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-", "").ToLower();
            }
        }
        /// <summary>
        /// 转成HTML
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToHtml(this string str)
        {
            return String.IsNullOrEmpty(str) ? string.Empty : str.Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace(" ", "&nbsp;").Replace("\n", "<br />");
        }

        /// <summary>
        /// 转成非HTML
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToText(this string str)
        {
            return String.IsNullOrEmpty(str) ? string.Empty : str.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&nbsp;", " ").Replace("<br />", "\n");
        }

        public static string NoHtml(this string str)
        {
            return String.IsNullOrEmpty(str) ? string.Empty : Regex.Replace(str, @"</?[^<]+>", "");
        }

        public static string UrlEncode(this string str)
        {
            return HttpUtility.UrlEncode(str);
        }
        public static string UrlDecode(this string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        /// <summary>
        /// 转成Javascript
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToJs(this string str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                str = str.Replace("\"", "\\\"").Replace("'", "\\\'").Replace("/", "\\/").Replace("<\\/script>",
                                                                                                 "<\\/s\'+\'cript>");
                str = Regex.Replace(str, @"\s+", " ", RegexOptions.Multiline);
                str = "document.write('" + str + "');";
                return str;
            }
            return "";
        }

        /// <summary>
        /// 获取字符串左侧字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetLeft(this string str, int length)
        {
            return String.IsNullOrEmpty(str) ? "" : (length >= str.Length ? str : str.Substring(0, length));
        }

        /// <summary>
        /// 获取字符串右侧字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRight(this string str, int length)
        {
            return String.IsNullOrEmpty(str) ? "" : (length >= str.Length ? str : str.Substring(str.Length - length));
        }


        const string KEY_64 = "Rogrand.";
        const string IV_64 = "Rogrand.";

        public static string Encode(string data)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }

        public static string Decode(string data)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            try
            {
                string str = sr.ReadToEnd();
                return str;
            }
            catch
            {
                return null;
            }
        }

        public static string FormatSafeUrl(this string str)
        {
            return String.IsNullOrEmpty(str) ? string.Empty : str.Trim().Replace("&", "_").Replace("?", "_").Replace("=", "_").Replace("/", "_").Replace("\\", "_").Replace(" ", "_");
        }

        public static string UrlFormat(this string str)
        {
            return Regex.Replace(str, @"&|=|\s|#|@|!|＼|｜|、|，|。|,|""|“|\.|\(|\)|（|）", "_");
        }

        public static Byte[] ToByteArray(this string str)
        {
            var ms = new MemoryStream();
            var bf = new BinaryFormatter();
            bf.Serialize(ms, str);
            ms.Seek(0, 0);
            return ms.ToArray();
        }

        public static bool IsNumber(this string lstr)
        {
            const string path = @"^-?\d+$";
            var r = new Regex(path, RegexOptions.Compiled);
            var m = r.Match(lstr);
            return m.Success;
        }

        public static int[] ToIntArray(this string str, string separator)
        {
            if (str == null) return new int[0];
            var strArray = string.IsNullOrEmpty(separator) ? Regex.Split(str, "(?!^|$)") : str.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            var intArray = new int[strArray.Length];
            for (var i = 0; i < strArray.Length; i++)
            {
                intArray[i] = int.Parse(strArray[i]);
            }
            return intArray;
        }

        /// <summary>
        /// 获取中文字符串的首字母
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string GetChineseSpell(string strText)
        {
            int len = strText.Length;
            string myStr = "";
            for (int i = 0; i < len; i++)
            {
                myStr += getSpell(strText.Substring(i, 1));
            }
            return myStr;
        }
        /// <summary>
        /// 获取单个中文的首字母
        /// </summary>
        /// <param name="cnChar"></param>
        /// <returns></returns>
        private static string getSpell(string cnChar)
        {
            byte[] arrCN = Encoding.GetEncoding("gb2312").GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25)
                    {
                        max = areacode[i + 1];
                    }
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.GetEncoding("gb2312").GetString(new byte[] { (byte)(97 + i) });
                    }
                }
                return "*";
            }
            else return cnChar;
        }

        public static string GetPyInChineseArea(string HanZi)
        {
            string PyString = "吖八嚓咑妸发旮铪讥讥咔垃呣拿讴趴七呥仨他哇哇哇夕丫匝咗";
            string CurChar, vReturn = "";
            Regex regex = new Regex("^[\u4e00-\u9fa5]$");
            for (int nFor = 0; nFor < HanZi.Length; nFor++)
            {
                CurChar = HanZi.Substring(nFor, 1);
                if (regex.IsMatch(CurChar))
                {
                    for (int nPyFor = 0; nPyFor < PyString.Length; nPyFor++)
                    {
                        int Ret = CurChar.CompareTo(PyString.Substring(nPyFor, 1));
                        if (Ret < 0)
                        {
                            string Ch = "";
                            if (nPyFor != -1)
                                Ch = Convert.ToChar(64 + nPyFor).ToString();
                            else
                                Ch = CurChar;
                            vReturn = vReturn.Trim() + Ch;
                            break;
                        }
                    }
                }
                else
                {
                    vReturn = vReturn.Trim() + CurChar;
                }
            }
            return vReturn.ToLower();
        }        
    }
}