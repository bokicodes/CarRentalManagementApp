using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp
{
    public static class Utils
    {
        public static string HashPassword(string password)
        {

            // ENCRYPTING THE PASSWORD
            SHA256 sha = SHA256.Create();

            //Converting the input string to a byte array and computing the hash
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            //Creating Stringbuilder to collect the bytes and create a string
            StringBuilder sb = new StringBuilder();

            //Loop through each byte of the hashed data and format each one as a 
            //hexadecimal string
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2")); // x2 makes it hexadecimal, it formats the string and without it the sb would be decimal
            }

            string hashedPassword = sb.ToString();

            return hashedPassword;
        }

        public static string GenericHashedPassword()
        {

            // ENCRYPTING THE PASSWORD
            SHA256 sha = SHA256.Create();

            //Converting the input string to a byte array and computing the hash
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes("password123"));

            //Creating Stringbuilder to collect the bytes and create a string
            StringBuilder sb = new StringBuilder();

            //Loop through each byte of the hashed data and format each one as a 
            //hexadecimal string
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2")); // x2 makes it hexadecimal, it formats the string and without it the sb would be decimal
            }

            string hashedPassword = sb.ToString();

            return hashedPassword;
        }


    }
}
