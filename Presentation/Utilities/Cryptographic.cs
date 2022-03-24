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
            cs.Close();

            //7. MemoryStream >> byte[] >> string
            byte[] cipherData = msOut.ToArray();
            string cipherString = Convert.ToBase64String(cipherData);

            return cipherString;

        }

        public string DecryptSymmetric(string cipher)
        {
            ////1. string >> byte[]
            ////clear text data
            //byte[] cipherData = //Convert.FromBase64String(cipher)        //change 1

            ////2. decide on the name of the algorithm
            //Rijndael myAlg = Rijndael.Create();

            ////3. generate the secret key and the iv
            //byte[] secretKey = GenerateKey();
            //byte[] iv = GenerateIV();

            //myAlg.Key = secretKey;
            //myAlg.IV = iv;

            ////4. start the engine
            ////4.1 byte[] >> Stream
            //MemoryStream msIn = new MemoryStream(originalData);

            ////CryptoStream cs = new CryptoStream(inputData, alg, read)
            /////change to decyptor
            //change 2
            //CryptoStream cs = new CryptoStream(msIn, myAlg.CreateEncryptor(), CryptoStreamMode.Read);

            ////5. convert/encrypt the data & get the cipher (aka the encrypted data) out 
            //MemoryStream msOut = new MemoryStream(); //a place where to hold my cipher
            //cs.CopyTo(msOut); //extracting the data out of the cs into msout consequently encrypting the data
            //cs.Close();

            ////7. MemoryStream >> byte[] >> string
            //byte[] originalData = msOut.ToArray();     //change 3
            //string originalDataString = Encoding.UTF32.GetString(cipherData);

            //return originalDataString;

            return "";
        }

        
        public AsymmetricKeys GenerateAsymmetricKeys()
        {
            AsymmetricKeys keys = new AsymmetricKeys();
            RSA myAlg = RSACryptoServiceProvider.Create();
            keys.PrivateKey = myAlg.ToXmlString(true);
            keys.PublicKey = myAlg.ToXmlString(false);
            return keys;
        }

        //1. ideally used to encrypt small data (E.g. symmetric keys)
        //2. MUST: data has to be in base64 format
        public string AsymmetricEncrypt(string originalBase64Text, string publicKey)
        {
            RSA myAlg = RSACryptoServiceProvider.Create();
            myAlg.FromXmlString(publicKey);

            byte[] originalBase64Data = Convert.FromBase64String(originalBase64Text);

            byte[] cipherData = myAlg.Encrypt(originalBase64Data, RSAEncryptionPadding.Pkcs1);

            return Convert.ToBase64String(cipherData);
        }

        public string AsymmetricDecrypt(string cipherBase64Text, string privateKey)
        {
            //RSA myAlg = RSACryptoServiceProvider.Create();
            //myAlg.FromXmlString(publicKey); //change 1

            //byte[] cipherBase64Data = Convert.FromBase64String(cipherBase64Text);

            //change 2
            //byte[] originalData = myAlg.Encrypt(cipherBase64Data, RSAEncryptionPadding.Pkcs1);

            //return Convert.ToBase64String(originalData);

            return "";
        }

        public string DigitalSigning(string data, string privateKey )
        {  
            RSA myAlg = RSACryptoServiceProvider.Create();
            myAlg.FromXmlString(privateKey);

            string hashedString = Hash(data);
            byte[] digest = Convert.FromBase64String(hashedString);

            byte[] signature =
                myAlg.SignHash(digest, new HashAlgorithmName("SHA512"), RSASignaturePadding.Pkcs1);

            return Convert.ToBase64String(signature);

        }

        public bool DigitalVerification(string data, string signature, string publicKey)
        {
            //you have to use VerifyHash instead of SignHash
            return true;
        }

        public string Hash(string originalText)
        {
            //1. when input is human readable you have to use Encoding to convert to bytes[]
            //2. when you have 100% certainty that data being handled is already base64 format you have to
            //   use Convert.ToBase64String / Convert.FromBase64String
            //note: every cryptographic algorithm outputs base64 format

            SHA512 myAlg = SHA512.Create();
            byte[] myData = Encoding.UTF32.GetBytes(originalText);
            byte[] digest = myAlg.ComputeHash(myData);

            return Convert.ToBase64String(digest);
        }
    }

    public class AsymmetricKeys
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}
