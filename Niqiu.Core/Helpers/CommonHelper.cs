using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Niqiu.Core.Domain;

namespace Niqiu.Core.Helpers
{
    /// <summary>
    /// Represents a common helper
    /// </summary>
    public partial class CommonHelper
    {


        public static bool ValidateString(string data, ValidataType datatype)
        {
            bool ok = false;//返回结果 
            Regex regex = null;
            switch (datatype)
            {
                case ValidataType.Email:  //email格式
                    regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                    break;
                case ValidataType.Domain://域名格式"(^\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$" //^\w+([-.]\w+)*\.\w+([-.]\w+)*$
                    regex = new Regex(@"(^\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$");
                    break;
                case ValidataType.Phone://座机电话格式
                    regex = new Regex(@"^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$");
                    break;
                case ValidataType.Mobile://手机格式
                    //  regex = new Regex(@"^13\d{9}$|^15\d{9}$|^18\d{9}$|^0\d{9,10}$");
                    regex = new Regex(@"^13\d{9}$|^15\d{9}$|^14\d{9}$|^18\d{9}$|^0\d{9,10}$");
                    break;
                case ValidataType.Url://url格式
                    regex = new Regex("^http:\\/\\/[A-Za-z0-9]+\\.[A-Za-z0-9]+[\\/=\\?%\\-&_~`@[\\]\':+!]*([^<>\"\"])*$");
                    break;
                case ValidataType.Money://货币
                    regex = new Regex(@"^\d+(\.\d+)?$");
                    break;
                case ValidataType.Number://正整数
                    regex = new Regex(@"^\d+$");
                    break;
                case ValidataType.Zip://邮编
                    regex = new Regex(@"^\d{6}$");
                    break;
                case ValidataType.Ip://IP地址
                    regex = new Regex(@"^[\d\.]{7,15}$");
                    break;
                case ValidataType.QQ://QQ号
                    regex = new Regex(@"^[1-9]\d{4,9}$");
                    break;
                case ValidataType.Integer://整型
                    regex = new Regex(@"^[-\+]?\d+$");
                    break;
                case ValidataType.Double://浮点数 
                    regex = new Regex(@"^[-\+]?\d+(\.\d+)?$");
                    break;
                case ValidataType.English://英文
                    regex = new Regex(@"^[A-Za-z]+$");
                    break;
                case ValidataType.Chinese://中文
                    regex = new Regex(@"^[\u0391-\uFFE5]+$");
                    break;
                case ValidataType.Enandcn://中文和英文
                    regex = new Regex(@"^[\w\u0391-\uFFE5][\w\u0391-\uFFE5\-\.]+$");
                    break;
            }
            if (regex != null)
            {
                ok = regex.IsMatch(data);
            }
            return ok;
        }

        public static string ConverTime(double timesp)
        {
            var sec = 0;
            int min = 0;
            if (timesp < 1)
            {
                //说明是不到一分钟 就显示描述
                sec = (int)(timesp * 60);
                return sec + "秒";
            }
            if (timesp < 60)
            {
                //说明不到一小时
                var total = timesp * 60;
                min = (int)(total / 60);
                sec = (int)(total % 60);
                return string.Format("{0}分{1}秒", min, sec);
            }

            int hour = 0;
            hour = (int)(timesp / 60);
            min = (int)(timesp % 60);
            return string.Format("{0}小时{1}", hour, min);
        }

        public static string FileLenth(int bit)
        {
            if (bit < 1024)
                return string.Format("{0}b", bit);

            var newk = bit / 1024;

            if (newk < 1024)
            {
                return string.Format("{0}kb", newk);
            }
            var m = newk / 1024;
            if (m < 1024)
            {
                return string.Format("{0}M", m);
            }

            return string.Format("{0}G", Math.Round((double)m / 1024, 2));

        }

        #region  比较字符串相似度
        private static int min(int one, int two, int three)
        {
            int min = one;
            if (two < min)
            {
                min = two;
            }
            if (three < min)
            {
                min = three;
            }
            return min;
        }
        private static int LD(String str1, String str2)
        {
            int[,] d;     // 矩阵 
            int n = str1.Length;
            int m = str2.Length;
            int i;     // 遍历str1的 
            int j;     // 遍历str2的 
            char ch1;     // str1的 
            char ch2;     // str2的 
            int temp;     // 记录相同字符,在某个矩阵位置值的增量,不是0就是1 
            if (n == 0)
            {
                return m;
            }
            if (m == 0)
            {
                return n;
            }
            d = new int[n + 1, m + 1];
            for (i = 0; i <= n; i++)
            {     // 初始化第一列 
                d[i, 0] = i;
            }
            for (j = 0; j <= m; j++)
            {     // 初始化第一行 
                d[0, j] = j;
            }
            for (i = 1; i <= n; i++)
            {     // 遍历str1 
                ch1 = str1[i - 1];
                // 去匹配str2 
                for (j = 1; j <= m; j++)
                {
                    ch2 = str2[j - 1];
                    if (ch1 == ch2)
                    {
                        temp = 0;
                    }
                    else
                    {
                        temp = 1;
                    }
                    // 左边+1,上边+1, 左上角+temp取最小 
                    d[i, j] = min(d[i - 1, j] + 1, d[i, j - 1] + 1, d[i - 1, j - 1] + temp);
                }
            }
            return d[n, m];
        }

        //返回两个字符串的相似度，返回一个0到100之间的整数，值越大，表示相似度越高
        public static int SimilarStrings(String newStr, String targetStr)
        {
            int ld = LD(newStr, targetStr);
            double i = 1 - (double)ld / (double)Math.Max(newStr.Length, targetStr.Length);
            int similar = Convert.ToInt32(Math.Round((Convert.ToDecimal(i)), 2, MidpointRounding.AwayFromZero) * 100);
            return similar;
        }
        #endregion
        /// <summary>
        /// Ensures the subscriber email or throw.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public static string EnsureSubscriberEmailOrThrow(string email) {
            string output = EnsureNotNull(email);
            output = output.Trim();
            output = EnsureMaximumLength(output, 255);

            if(!IsValidEmail(output)) {
                throw new PortalException("Email is not valid.");
            }

            return output;
        }

        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
            return result;
        }

        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            string str = string.Empty;
            for (int i = 0; i < length; i++)
                str = String.Concat(str, random.Next(10).ToString());
            return str;
        }

        /// <summary>
        /// Returns an random interger number within a specified rage
        /// </summary>
        /// <param name="min">Minimum number</param>
        /// <param name="max">Maximum number</param>
        /// <returns>Result</returns>
        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
        {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }

        /// <summary>
        /// Ensure that a string doesn't exceed maximum allowed length
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="maxLength">Maximum length</param>
        /// <param name="postfix">A string to add to the end if the original string was shorten</param>
        /// <returns>Input string if its lengh is OK; otherwise, truncated input string</returns>
        public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            if (str.Length > maxLength)
            {
                var result = str.Substring(0, maxLength);
                if (!String.IsNullOrEmpty(postfix))
                {
                    result += postfix;
                }
                return result;
            }

            return str;
        }

        /// <summary>
        /// Ensures that a string only contains numeric values
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
        public static string EnsureNumericOnly(string str)
        {
            if (String.IsNullOrEmpty(str))
                return string.Empty;

            var result = new StringBuilder();
            foreach (char c in str)
            {
                if (Char.IsDigit(c))
                    result.Append(c);
            }
            return result.ToString();
        }

        /// <summary>
        /// Ensure that a string is not null
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result</returns>
        public static string EnsureNotNull(string str)
        {
            if (str == null)
                return string.Empty;

            return str;
        }

        /// <summary>
        /// Indicates whether the specified strings are null or empty strings
        /// </summary>
        /// <param name="stringsToValidate">Array of strings to validate</param>
        /// <returns>Boolean</returns>
        public static bool AreNullOrEmpty(params string[] stringsToValidate) {
            bool result = false;
            Array.ForEach(stringsToValidate, str => {
                if (string.IsNullOrEmpty(str)) result = true;
            });
            return result;
        }

        /// <summary>
        /// Compare two arrasy
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="a1">Array 1</param>
        /// <param name="a2">Array 2</param>
        /// <returns>Result</returns>
        public static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            //also see Enumerable.SequenceEqual(a1, a2);
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }
            return true;
        }

        private static AspNetHostingPermissionLevel? _trustLevel;
        /// <summary>
        /// Finds the trust level of the running application (http://blogs.msdn.com/dmitryr/archive/2007/01/23/finding-out-the-current-trust-level-in-asp-net.aspx)
        /// </summary>
        /// <returns>The current trust level.</returns>
        public static AspNetHostingPermissionLevel GetTrustLevel()
        {
            if (!_trustLevel.HasValue)
            {
                //set minimum
                _trustLevel = AspNetHostingPermissionLevel.None;

                //determine maximum
                foreach (AspNetHostingPermissionLevel trustLevel in new[] {
                                AspNetHostingPermissionLevel.Unrestricted,
                                AspNetHostingPermissionLevel.High,
                                AspNetHostingPermissionLevel.Medium,
                                AspNetHostingPermissionLevel.Low,
                                AspNetHostingPermissionLevel.Minimal 
                            })
                {
                    try
                    {
                        new AspNetHostingPermission(trustLevel).Demand();
                        _trustLevel = trustLevel;
                        break; //we've set the highest permission we can
                    }
                    catch (System.Security.SecurityException)
                    {
                        continue;
                    }
                }
            }
            return _trustLevel.Value;
        }

        /// <summary>
        /// Sets a property on an object to a valuae.
        /// </summary>
        /// <param name="instance">The object whose property to set.</param>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetProperty(object instance, string propertyName, object value)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            Type instanceType = instance.GetType();
            PropertyInfo pi = instanceType.GetProperty(propertyName);
            if (pi == null)
                throw new PortalException("No property '{0}' found on the instance of type '{1}'.", propertyName, instanceType);
            if (!pi.CanWrite)
                throw new PortalException("The property '{0}' on the instance of type '{1}' does not have a setter.", propertyName, instanceType);
            if (value != null && !value.GetType().IsAssignableFrom(pi.PropertyType))
                value = To(value, pi.PropertyType);
            pi.SetValue(instance, value, new object[0]);
        }

        public static TypeConverter GetNopCustomTypeConverter(Type type)
        {
            //we can't use the following code in order to register our custom type descriptors
            //TypeDescriptor.AddAttributes(typeof(List<int>), new TypeConverterAttribute(typeof(GenericListTypeConverter<int>)));
            //so we do it manually here

            //if (type == typeof(List<int>))
            //    return new GenericListTypeConverter<int>();
            //if (type == typeof(List<decimal>))
            //    return new GenericListTypeConverter<decimal>();
            //if (type == typeof(List<string>))
            //    return new GenericListTypeConverter<string>();
            //if (type == typeof(ShippingOption))
            //    return new ShippingOptionTypeConverter();
            //if (type == typeof(List<ShippingOption>) || type == typeof(IList<ShippingOption>))
            //    return new ShippingOptionListTypeConverter();

            return TypeDescriptor.GetConverter(type);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value != null)
            {
                var sourceType = value.GetType();

                TypeConverter destinationConverter = GetNopCustomTypeConverter(destinationType);
                TypeConverter sourceConverter = GetNopCustomTypeConverter(sourceType);
                if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
                    return destinationConverter.ConvertFrom(null, culture, value);
                if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
                    return sourceConverter.ConvertTo(null, culture, value, destinationType);
                if (destinationType.IsEnum && value is int)
                    return Enum.ToObject(destinationType, (int)value);
                if (!destinationType.IsInstanceOfType(value))
                    return Convert.ChangeType(value, destinationType, culture);
            }
            return value;
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <returns>The converted value.</returns>
        public static T To<T>(object value)
        {
            //return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            return (T)To(value, typeof(T));
        }
        
        /// <summary>
        /// Convert enum for front-end
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Converted string</returns>
        public static string ConvertEnum(string str)
        {
            string result = string.Empty;
            char[] letters = str.ToCharArray();
            foreach (char c in letters)
                if (c.ToString() != c.ToString().ToLower())
                    result += " " + c.ToString();
                else
                    result += c.ToString();
            return result;
        }

        /// <summary>
        /// Set Telerik (Kendo UI) culture
        /// </summary>
        public static void SetTelerikCulture()
        {
            //little hack here
            //always set culture to 'en-US' (Kendo UI has a bug related to editing decimal values in other cultures). Like currently it's done for admin area in Global.asax.cs
            
            var culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }


        public static string StripHtml(string strHtml)
        {
            if (string.IsNullOrEmpty(strHtml)) return "";
            string[] aryReg ={
          @"<script[^>]*?>.*?</script>",

          @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
          @"([\r\n])[\s]+",
          @"&(quot|#34);",
          @"&(amp|#38);",
          @"&(lt|#60);",
          @"&(gt|#62);", 
          @"&(nbsp|#160);", 
          @"&(iexcl|#161);",
          @"&(cent|#162);",
          @"&(pound|#163);",
          @"&(copy|#169);",
          @"&#(\d+);",
          @"-->",
          @"<!--.*\n"
         
         };

            string[] aryRep = {
           "",
           "",
           "",
           "\"",
           "&",
           "<",
           ">",
           " ",
           "\xa1",//chr(161),
           "\xa2",//chr(162),
           "\xa3",//chr(163),
           "\xa9",//chr(169),
           "",
           "\r\n",
           ""
          };

            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, aryRep[i]);
            }

            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("&time", "");
            strOutput.Replace("\r\n", "");


            return strOutput;
        }

        public static int[] GetRandomNums(int num, int minValue, int maxValue)
        {

            List<int> arrNum = new List<int>();

            for (int j = 0; j < num; j++)
            {
                var i = GetRandomNum(minValue, maxValue, arrNum);
                arrNum.Add(i);
            }

            var ids = arrNum.ToArray();
            // sort(ids);

            return ids;
        }


        private static int GetRandomNum(int min, int max, List<int> ids)
        {
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            var res = ra.Next(min, max);
            if (ids.Count == 0) return res;

            while (ids.Contains(res))
            {
                res = ra.Next(min, max);
            }

            return res;
        }

    }
}
