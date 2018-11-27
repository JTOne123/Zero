using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Zero.NETCore.Util
{
    /// <summary>
    /// ���ܰ����� 
    /// </summary>
    public class CryptoHelper
    {

        /// <summary>
        /// 3DES�ӽ��ܵ�Ĭ����Կ, ǰ8λ��Ϊ����
        /// </summary>
        private const string KEY_Complement = "Z!E@R#O$Z%H^E&N*G(L)I_N+G{J}U|N?";


        #region ʹ��Get�����滻�ؼ��ַ�Ϊȫ�ǺͰ��ת��
        /// <summary>
        /// ʹ��Get�����滻�ؼ��ַ�Ϊȫ��
        /// </summary>
        /// <param name="UrlParam"></param>
        /// <returns></returns>
        public static string UrlParamUrlEncodeRun(string UrlParam)
        {
            UrlParam = UrlParam.Replace("+", "��");
            UrlParam = UrlParam.Replace("=", "��");
            UrlParam = UrlParam.Replace("&", "��");
            UrlParam = UrlParam.Replace("?", "��");
            return UrlParam;
        }

        /// <summary>
        /// ʹ��Get�����滻�ؼ��ַ�Ϊ���
        /// </summary>
        /// <param name="UrlParam"></param>
        /// <returns></returns>
        public static string UrlParamUrlDecodeRun(string UrlParam)
        {
            UrlParam = UrlParam.Replace("��", "+");
            UrlParam = UrlParam.Replace("��", "=");
            UrlParam = UrlParam.Replace("��", "&");
            UrlParam = UrlParam.Replace("��", "?");
            return UrlParam;
        }
        #endregion

        #region  MD5����

        /// <summary>
        /// ��׼MD5����
        /// </summary>
        /// <param name="source">�������ַ���</param>
        /// <param name="addKey">�����ַ���</param>
        /// <param name="encoding">���뷽ʽ</param>
        /// <returns></returns>
        public static string MD5_Encrypt(string source, string addKey, Encoding encoding)
        {
            if (addKey.Length > 0)
            {
                source = source + addKey;
            }
            return MD5_Encrypt(encoding.GetBytes(source));
        }


        /// <summary>
        /// ��׼md5����
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string MD5_Encrypt(byte[] source)
        {
            MD5 MD5 = new MD5CryptoServiceProvider();
            byte[] newSource = MD5.ComputeHash(source);
            string byte2String = null;
            for (int i = 0; i < newSource.Length; i++)
            {
                string thisByte = newSource[i].ToString("x");
                if (thisByte.Length == 1)
                {
                    thisByte = "0" + thisByte;
                }

                byte2String += thisByte;
            }
            return byte2String;
        }

        /// <summary>
        /// ��׼MD5����
        /// </summary>
        /// <param name="source">�������ַ���</param>
        /// <param name="encoding">���뷽ʽ</param>
        /// <returns></returns>
        public static string MD5_Encrypt(string source, Encoding encoding)
        {
            return MD5_Encrypt(source, string.Empty, encoding);
        }
        /// <summary>
        /// ��׼MD5����
        /// </summary>
        /// <param name="source">�����ܵ��ַ���</param>
        /// <returns></returns>
        public static string MD5_Encrypt(string source)
        {
            return MD5_Encrypt(source, string.Empty, Encoding.UTF8);
        }


        #endregion

        #region �������
        /// <summary>
        /// ����ʹ��MD5���ܺ��ַ���
        /// </summary>
        /// <param name="strpwd">�������ַ���</param>
        /// <returns>���ܺ��ַ���</returns>
        public static string RegUser_MD5_Pwd(string strpwd)
        {
            #region

            string appkey = KEY_Complement; //������һ������ַ����ټ��ܣ���������ȫЩ
            //strpwd += appkey;

            MD5 MD5 = new MD5CryptoServiceProvider();
            byte[] a = Encoding.Default.GetBytes(appkey);
            byte[] datSource = Encoding.Default.GetBytes(strpwd);
            byte[] b = new byte[a.Length + 4 + datSource.Length];

            int i;
            for (i = 0; i < datSource.Length; i++)
            {
                b[i] = datSource[i];
            }

            b[i++] = 163;
            b[i++] = 172;
            b[i++] = 161;
            b[i++] = 163;

            foreach (byte t in a)
            {
                b[i] = t;
                i++;
            }

            byte[] newSource = MD5.ComputeHash(b);
            string byte2String = null;
            for (i = 0; i < newSource.Length; i++)
            {
                string thisByte = newSource[i].ToString("x");
                if (thisByte.Length == 1)
                {
                    thisByte = "0" + thisByte;
                }

                byte2String += thisByte;
            }
            return byte2String;

            #endregion
        }
        #endregion

        #region  DES �ӽ���
        /// <summary>
        /// Desc���� Ĭ��ʹ��Mode=CBC,Padding=PKCS7,Encoding=UTF8
        /// </summary>
        /// <param name="source">�������ַ�</param>
        /// <param name="key">��Կ</param>
        /// <returns>string</returns>
        public static string DES_Encrypt(string source, string key)
        {
            key = BuildKey(key);
            byte[] btKey = Encoding.UTF8.GetBytes(key);
            byte[] btIV = Encoding.UTF8.GetBytes(key);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] inData = Encoding.UTF8.GetBytes(source);
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);
                        cs.FlushFinalBlock();
                    }

                    StringBuilder ret = new StringBuilder();
                    foreach (byte b in ms.ToArray())
                    {
                        ret.AppendFormat("{0:X2}", b);
                    }

                    return ret.ToString();
                }
            }
        }

        /// <summary>
        /// ʹ��Ĭ��key �� DES���� Ĭ��ʹ��Mode=CBC,Padding=PKCS7,Encoding=UTF8
        /// </summary>
        /// <param name="source">����</param>
        /// <returns>����</returns>
        public static string DES_Encrypt(string source)
        {

            return DES_Encrypt(source, KEY_Complement);
        }
        /// <summary>
        /// ʹ��Ĭ��key �� DES���� Ĭ��ʹ��Mode=CBC,Padding=PKCS7,Encoding=UTF8
        /// </summary>
        /// <param name="source">����</param>
        /// <returns>����</returns>
        public static string DES_Decrypt(string source)
        {
            return DES_Decrypt(source, KEY_Complement);
        }

        /// <summary>
        /// DES���� Ĭ��ʹ��Mode=CBC,Padding=PKCS7,Encoding=UTF8
        /// </summary>
        /// <param name="source">����</param>
        /// <param name="key">��Կ</param>
        /// <returns>����</returns>
        public static string DES_Decrypt(string source, string key)
        {
            //���ַ���תΪ�ֽ�����  
            byte[] inputByteArray = new byte[source.Length / 2];
            for (int x = 0; x < source.Length / 2; x++)
            {
                int i = (Convert.ToInt32(source.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            key = BuildKey(key);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                des.Key = Encoding.UTF8.GetBytes(key);
                des.IV = Encoding.UTF8.GetBytes(key);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

        #endregion


        #region 3DES�ӽ���

        /// <summary>
        /// ʹ��ָ����key��iv������input����
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key">��Կ������Ϊ24λ����</param>
        /// <param name="iv">΢��������Ϊ8λ����</param>
        /// <returns></returns>
        public static string TripleDES_Encrypt(string input, string key = null, string iv = null)
        {
            key = BuildKey(key, 24);

            iv = BuildKey(iv, 8);

            byte[] arrKey = Encoding.UTF8.GetBytes(key);
            byte[] arrIV = Encoding.UTF8.GetBytes(iv);

            // ��ȡ���ܺ���ֽ�����
            byte[] arrData = Encoding.UTF8.GetBytes(input);
            byte[] result = TripleDesEncrypt(arrKey, arrIV, arrData);

            // ת��Ϊ16�����ַ���
            StringBuilder ret = new StringBuilder();
            foreach (byte b in result)
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary>
        /// ʹ��ָ����key��iv������input����
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key">��Կ������Ϊ24λ����</param>
        /// <param name="iv">΢��������Ϊ8λ����</param>
        /// <returns></returns>
        public static string TripleDES_Decrypt(string input, string key = null, string iv = null)
        {
            key = BuildKey(key, 24);

            iv = BuildKey(iv, 8);

            byte[] arrKey = Encoding.UTF8.GetBytes(key);
            byte[] arrIV = Encoding.UTF8.GetBytes(iv);

            // ��ȡ���ܺ���ֽ�����
            int len = input.Length / 2;
            byte[] arrData = new byte[len];
            for (int x = 0; x < len; x++)
            {
                int i = (Convert.ToInt32(input.Substring(x * 2, 2), 16));
                arrData[x] = (byte)i;
            }

            byte[] result = TripleDesDecrypt(arrKey, arrIV, arrData);
            return Encoding.UTF8.GetString(result);
        }


        #region TripleDesEncrypt����(3DES����)
        /// <summary>
        /// 3Des���ܣ���Կ���ȱ�����24�ֽ�
        /// </summary>
        /// <param name="key">��Կ�ֽ�����</param>
        /// <param name="iv">�����ֽ�����</param>
        /// <param name="source">Դ�ֽ�����</param>
        /// <returns>���ܺ���ֽ�����</returns>
        private static byte[] TripleDesEncrypt(byte[] key, byte[] iv, byte[] source)
        {
            TripleDESCryptoServiceProvider dsp = new TripleDESCryptoServiceProvider
            {
                Mode = CipherMode.CBC, // Ĭ��ֵ
                Padding = PaddingMode.PKCS7 // Ĭ��ֵ
            };
            using (MemoryStream mStream = new MemoryStream())
            using (CryptoStream cStream = new CryptoStream(mStream, dsp.CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                cStream.Write(source, 0, source.Length);
                cStream.FlushFinalBlock();
                byte[] result = mStream.ToArray();
                cStream.Close();
                mStream.Close();
                return result;
            }
        }
        #endregion

        #region TripleDesDecrypt����(3DES����)
        /// <summary>
        /// 3Des���ܣ���Կ���ȱ�����24�ֽ�
        /// </summary>
        /// <param name="key">��Կ�ֽ�����</param>
        /// <param name="iv">�����ֽ�����</param>
        /// <param name="source">���ܺ���ֽ�����</param>
        /// <param name="dataLen">���ܺ�����ݳ���</param>
        /// <returns>���ܺ���ֽ�����</returns>
        private static byte[] TripleDesDecrypt(byte[] key, byte[] iv, byte[] source, out int dataLen)
        {
            TripleDESCryptoServiceProvider dsp = new TripleDESCryptoServiceProvider
            {
                Mode = CipherMode.CBC, // Ĭ��ֵ
                Padding = PaddingMode.PKCS7 // Ĭ��ֵ
            };
            using (MemoryStream mStream = new MemoryStream(source))
            using (CryptoStream cStream = new CryptoStream(mStream, dsp.CreateDecryptor(key, iv), CryptoStreamMode.Read))
            {
                byte[] result = new byte[source.Length];
                dataLen = cStream.Read(result, 0, result.Length);
                cStream.Close();
                mStream.Close();
                return result;
            }
        }

        /// <summary>
        /// 3Des���ܣ���Կ���ȱ�����24�ֽ�
        /// </summary>
        /// <param name="key">��Կ�ֽ�����</param>
        /// <param name="iv">�����ֽ�����</param>
        /// <param name="source">���ܺ���ֽ�����</param>
        /// <returns>���ܺ���ֽ�����</returns>
        private static byte[] TripleDesDecrypt(byte[] key, byte[] iv, byte[] source)
        {
            byte[] result = TripleDesDecrypt(key, iv, source, out int dataLen);

            if (result.Length != dataLen)
            {
                // ������鳤�Ȳ��ǽ��ܺ��ʵ�ʳ��ȣ���Ҫ�ض϶�������ݣ��������Gzip��"Magic byte doesn't match"������
                byte[] resultToReturn = new byte[dataLen];
                Array.Copy(result, resultToReturn, dataLen);
                return resultToReturn;
            }
            else
            {
                return result;
            }
        }
        #endregion


        #endregion


        #region SHA1����

        /// <summary>
        /// SHA1���ܣ���Ч�� PHP �� SHA1() ����
        /// </summary>
        /// <param name="source">�����ܵ��ַ���</param>
        /// <returns>���ܺ���ַ���</returns>
        public static string SHA1_Encrypt(string source)
        {
            byte[] temp1 = Encoding.UTF8.GetBytes(source);

            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            byte[] temp2 = sha.ComputeHash(temp1);
            sha.Clear();

            //ע�⣬���������
            //string output = Convert.ToBase64String(temp2); 

            string output = BitConverter.ToString(temp2);
            output = output.Replace("-", "");
            output = output.ToLower();
            return output;
        }
        #endregion

        #region ͨ��HTTP���ݵ�Base64����
        /// <summary>
        /// ���� ͨ��HTTP���ݵ�Base64����
        /// </summary>
        /// <param name="source">����ǰ��</param>
        /// <returns>������</returns>
        public static string HttpBase64Encode(string source)
        {
            //�մ�����
            if (string.IsNullOrEmpty(source))
            {
                return "";
            }

            //����
            string encodeString = Convert.ToBase64String(Encoding.UTF8.GetBytes(source));

            //����
            encodeString = encodeString.Replace("+", "~");
            encodeString = encodeString.Replace("/", "@");
            encodeString = encodeString.Replace("=", "$");

            //����
            return encodeString;
        }
        #endregion

        #region ͨ��HTTP���ݵ�Base64����
        /// <summary>
        /// ���� ͨ��HTTP���ݵ�Base64����
        /// </summary>
        /// <param name="source">����ǰ��</param>
        /// <returns>������</returns>
        public static string HttpBase64Decode(string source)
        {
            //�մ�����
            if (string.IsNullOrEmpty(source))
            {
                return "";
            }

            //��ԭ
            string deocdeString = source;
            deocdeString = deocdeString.Replace("~", "+");
            deocdeString = deocdeString.Replace("@", "/");
            deocdeString = deocdeString.Replace("$", "=");

            //Base64����
            deocdeString = Encoding.UTF8.GetString(Convert.FromBase64String(deocdeString));

            //����
            return deocdeString;
        }
        #endregion

        /// <summary>
        /// �����ļ���MD5ֵ������
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            using (FileStream file = new FileStream(fileName, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                return BitConverter.ToString(retVal).Replace("-", "");
            }
        }

        /// <summary>  
        ///AES���ܣ����ܲ��裩  
        ///1�������ַ����õ�2�������飻  
        ///2����2��ֹ����תΪ16���ƣ�  
        ///3������base64����  
        /// </summary>  
        /// <param name="toEncrypt">Ҫ���ܵ��ַ���</param>  
        /// <param name="key">��Կ</param>  
        public static string AES_Encrypt(string toEncrypt, string key)
        {
            byte[] _Key = Encoding.ASCII.GetBytes(BuildKey(key, 32));
            byte[] _Source = Encoding.UTF8.GetBytes(toEncrypt);

            Aes aes = Aes.Create("AES");
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = _Key;
            ICryptoTransform cTransform = aes.CreateEncryptor();
            byte[] cryptData = cTransform.TransformFinalBlock(_Source, 0, _Source.Length);
            string HexCryptString = Hex_2To16(cryptData);
            byte[] HexCryptData = Encoding.UTF8.GetBytes(HexCryptString);
            string CryptString = Convert.ToBase64String(HexCryptData);
            return CryptString;
        }

        /// <summary>  
        /// AES���ܣ����ܲ��裩  
        /// 1����BASE64�ַ���תΪ16��������  
        /// 2����16��������תΪ�ַ���  
        /// 3�����ַ���תΪ2��������  
        /// 4����AES��������  
        /// </summary>  
        /// <param name="encryptedSource">�Ѽ��ܵ�����</param>  
        /// <param name="key">��Կ</param>  
        public static string AES_Decrypt(string encryptedSource, string key)
        {
            byte[] _Key = Encoding.ASCII.GetBytes(BuildKey(key, 32));
            Aes aes = Aes.Create("AES");
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = _Key;
            ICryptoTransform cTransform = aes.CreateDecryptor();

            byte[] encryptedData = Convert.FromBase64String(encryptedSource);
            string encryptedString = Encoding.UTF8.GetString(encryptedData);
            byte[] _Source = Hex_16To2(encryptedString);
            byte[] originalSrouceData = cTransform.TransformFinalBlock(_Source, 0, _Source.Length);
            string originalString = Encoding.UTF8.GetString(originalSrouceData);
            return originalString;
        }

        private static string BuildKey(string key, int length = 8)
        {
            return ((key ?? string.Empty) + KEY_Complement).Substring(0, length);
        }

        private static string Hex_2To16(byte[] bytes)
        {
            string hexString = string.Empty;
            int iLength = 65535;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                if (bytes.Length < iLength)
                {
                    iLength = bytes.Length;
                }

                for (int i = 0; i < iLength; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        private static byte[] Hex_16To2(string hexString)
        {
            if ((hexString.Length % 2) != 0)
            {
                hexString += " ";
            }
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }
    }
}
