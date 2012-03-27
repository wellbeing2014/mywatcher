/*
 * 由SharpDevelop创建。
 * 用户： ZhuXinPei
 * 日期: 2012/2/22
 * 时间: 9:12
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Text;  
using System.Security.Cryptography; 

namespace WatchCore.Common
{
	/// <summary>
    /// MD5加密解密的通用操作类
    /// 说明：java加密后的密码，与此处的对比相同。
    /// </summary>
    public static class MD5Common
    {
        /// <summary>
        /// 返回指定字符串的Md5
        /// </summary>
        /// <param name="strInput">指定字符串</param>
        /// <returns>返回字符串的Md5</returns> 
        public static string GetMd5Hash(string strInput)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] btData = md5Hasher.ComputeHash(Encoding.Default.GetBytes(strInput));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < btData.Length; i++)
            {
                sBuilder.Append(btData[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// 检查一个普通字符串的Md5，与传递的Md5字符串是否相同
        /// </summary>
        /// <param name="strInput">普通字符串</param>
        /// <param name="strHash">Md5字符串</param>
        /// <returns>返回是否相同</returns> 
        public static bool VerifyMd5Hash(string strInput, string strHash)
        {
            // Hash the input.
            string strhashOfInput = GetMd5Hash(strInput);

            // Create a StringComparer an comare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(strhashOfInput, strHash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
