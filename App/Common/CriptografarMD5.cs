using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Common
{
    public  class CriptografarMD5
    {

        public string Criptografar(string password)
        {
            if (password == null || !password.Any())
                return null;


            password = "|240&c0-bb@43-6e3$81=" + password + "=MaP_Ew#Td@x3";

            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] data = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(password));
            System.Text.StringBuilder sbString = new System.Text.StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sbString.Append(data[i].ToString("x2"));
            return sbString.ToString();
        }
    }
}
