using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Niqiu.Core.Helpers
{
    /// <summary>
    /// 字符串加密组件
    /// </summary>
    public class Encrypt : IDisposable
    {
        #region "定义加密字串变量"
        private static readonly SymmetricAlgorithm mCSP = new DESCryptoServiceProvider();  //声明对称算法变量
        private const string defaultKey = "fGhjsK78";
        private const string defaultCiv = "ss8edKe3rTsfhH43c+ddwKUs";
        #endregion
        /// <summary>
        /// 实例化
        /// </summary>
        public Encrypt()
        {
            //定义访问数据加密标准 (DES) 算法的加密服务提供程序 (CSP) 版本的包装对象,此类是SymmetricAlgorithm的派生类
        }



        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="value">需加密的字符串</param>
        /// <param name="key">密钥，8位</param>
        /// <param name="civ">初始化向量,24位</param>
        /// <returns>加密后字符串</returns>
        public static string EncryptString(string value, string key = "", string civ = "")
        {
            if (String.IsNullOrEmpty(civ))
            {
                civ = defaultCiv;
            }
            if (String.IsNullOrEmpty(key))
            {
                key = defaultKey;
            }
            //CreateEncryptor创建(对称数据)加密对象
            //定义基本的加密转换运算
            //用指定的密钥和初始化向量创建对称数据加密标准
            var ct = mCSP.CreateEncryptor(Encoding.Default.GetBytes(key), Encoding.Default.GetBytes(civ)); 
            var byt = Encoding.UTF8.GetBytes(value); //将Value字符转换为UTF-8编码的字节序列
            //创建内存流
            var ms = new MemoryStream(); 
            //定义将内存流链接到加密转换的流
            var cs = new CryptoStream(ms, ct, CryptoStreamMode.Write); //将内存流链接到加密转换的流
            cs.Write(byt, 0, byt.Length); //写入内存流
            cs.FlushFinalBlock(); //将缓冲区中的数据写入内存流，并清除缓冲区
            cs.Close(); //释放内存流
            return Convert.ToBase64String(ms.ToArray()); //将内存流转写入字节数组并转换为string字符
        }

        /// <summary>
        /// Encrypts a string using the SHA256 algorithm.
        /// </summary>
        /// <param name="plainMessage">
        /// The plain Message.
        /// </param>
        /// <returns>
        /// The hash password.
        /// </returns>
        public static string HashPassword(string plainMessage)
        {
            var data = Encoding.UTF8.GetBytes(plainMessage);
            using (HashAlgorithm sha = new SHA256Managed())
            {
                sha.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(sha.Hash);
            }
        }

        /// <summary>
        /// Create salt key
        /// </summary>
        /// <param name="size">Key size</param>
        /// <returns>Salt key</returns>
        public static string CreateSaltKey(int size)
        {
            // Generate a cryptographic random number
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);
            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="value">要解密的字符串</param>
        /// <param name="key">密钥，8位</param>
        /// <param name="civ">初始化向量，24位</param>
        /// <returns>解密后字符串</returns>
        public static string DecryptString(string value, string key = "", string civ = "")
        {
            if (String.IsNullOrEmpty(civ))
            {
                civ = defaultCiv;
            } 
            if (String.IsNullOrEmpty(key))
            {
                key = defaultKey;
            }
            //定义基本的加密转换运算
            //用指定的密钥和初始化向量创建对称数据解密标准
            var ct = mCSP.CreateDecryptor(Encoding.Default.GetBytes(key), Encoding.Default.GetBytes(civ));
            var byt = Convert.FromBase64String(value); //将Value(Base 64)字符转换成字节数组
            //定义内存流
            var ms = new MemoryStream();
            //定义将数据流链接到加密转换的流
            var cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray()); //将字节数组中的所有字符解码为一个字符串
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            mCSP.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetMd5Code(string text="")
        {
            if (string.IsNullOrEmpty(text)) text = "";

            var result = Encoding.Default.GetBytes(text);
            MD5 md5 = new MD5CryptoServiceProvider();
            var oBytes = md5.ComputeHash(result);
            return BitConverter.ToString(oBytes).Replace("-", "");
        }
        /// <summary>
        /// Create a password hash
        /// </summary>
        /// <param name="password">{assword</param>
        /// <param name="saltkey">Salk key</param>
        /// <param name="passwordFormat">Password format (hash algorithm)</param>
        /// <returns>Password hash</returns>
        public static string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1")
        {
            if (String.IsNullOrEmpty(passwordFormat))
                passwordFormat = "SHA1";
            string saltAndPassword = String.Concat(password, saltkey);

            //return FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPassword, passwordFormat);
            var algorithm = HashAlgorithm.Create(passwordFormat);
            if (algorithm == null)
                throw new ArgumentException("Unrecognized hash name");

            var hashByteArray = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword));
            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }

        public static string GenerateOrderNumber()
        {
            string strDateTimeNumber = DateTime.Now.ToString("yyyyMMddHHmmssms");
            Thread.Sleep(1);
            string strRandomResult = NextRandom(1000, 1).ToString(CultureInfo.InvariantCulture);
            return strDateTimeNumber + strRandomResult;
        }
        /// <summary>
        /// 参考：msdn上的RNGCryptoServiceProvider例子
        /// </summary>
        /// <param name="numSeeds"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static int NextRandom(int numSeeds, int length)
        {
            // Create a byte array to hold the random value.  
            byte[] randomNumber = new byte[length];
            // Create a new instance of the RNGCryptoServiceProvider.  
            var rng = new RNGCryptoServiceProvider();
            // Fill the array with a random value.  
            rng.GetBytes(randomNumber);
            // Convert the byte to an uint value to make the modulus operation easier.  
            uint randomResult = 0x0;
            for (int i = 0; i < length; i++)
            {
                randomResult |= ((uint)randomNumber[i] << ((length - 1 - i) * 8));
            }
            return (int)(randomResult % numSeeds) + 1;
        }
    }
}
