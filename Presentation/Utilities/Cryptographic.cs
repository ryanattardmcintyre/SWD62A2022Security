using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Presentation.Utilities
{
    public class Cryptographic
    {

        string password = "P@$$w0rd*";
        string salt = "9123479283740912387409123874";
        public byte[] GenerateKey() {
            Rfc2898DeriveBytes myGenerator = new Rfc2898DeriveBytes(password,
                Encoding.UTF32.GetBytes(salt));
            
            Rijndael myAlg = Rijndael.Create();

            byte[] key = myGenerator.GetBytes(myAlg.KeySize / 8);

            return key;
        }
        public byte[] GenerateIV()
        {
            Rfc2898DeriveBytes myGenerator = new Rfc2898DeriveBytes(password,
                Encoding.UTF32.GetBytes(salt));

            Rijndael myAlg = Rijndael.Create();

            byte[] iv = myGenerator.GetBytes(myAlg.BlockSize / 8);

            return iv;
        }


        public string SymmetricEncrypt(string input)
        {
            //1. string >> byte[]
            //clear text data
            byte[] originalData = Encoding.UTF32.GetBytes(input);

            //2. decide on the name of the algorithm
            Rijndael myAlg = Rijndael.Create();
            
            //3. generate the secret key and the iv
            byte[] secretKey = GenerateKey();
            byte[] iv = GenerateIV();

            myAlg.Key = secretKey;
            myAlg.IV = iv;

            //4. start the engine
            //4.1 byte[] >> Stream
            MemoryStream msIn = new MemoryStream(originalData);

            //CryptoStream cs = new CryptoStream(inputData, alg, read)
            CryptoStream cs = new CryptoStream(msIn, myAlg.CreateEncryptor(), CryptoStreamMode.Read);

            //5. convert/encrypt the data & get the cipher (aka the encrypted data) out 
            MemoryStream msOut = new MemoryStream(); //a place where to hold my cipher
            cs.CopyTo(msOut); //extracting the data out of the cs into msout consequently encrypting the data
            cs.FlushFinalBlock();

            //7. MemoryStream >> byte[] >> string
            byte[] cipherData = msOut.ToArray();
            string cipherString = Convert.ToBase64String(cipherData);

            return cipherString;

        }

        public string DecryptSymmetric(string cipher)
        {
            
        }
    }
}
