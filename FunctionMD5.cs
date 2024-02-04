using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QLKTX
{
    internal class FunctionMD5
    {
        public static string Create_md5(string str1) {
            String str = "";
            Byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str1);
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            buffer = md5.ComputeHash(buffer);
            foreach (Byte b in buffer)
            {
                str += b.ToString("x");
            }
            return str;
        }
       
    }
}
