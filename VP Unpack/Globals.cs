﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace VP_Unpack
{
    public static class Globals
    {
        public static MainForm mainForm;
        public static CaffHeaderControl caffHeaderControl;

        public static Pkg currentPkg;

        //View options.
        public static bool clearLogCheck = false;

        public static MemoryStream vpltMem;
        public static BinaryReader vpltBR;

        public static BinaryWriter vpltBW;

        public static int GetMD5Record(byte[] input)
        {
            vpltMem.Seek(32, SeekOrigin.Begin);

            for (int i = 0; i < 491; i++)
            {
                byte[] b = vpltBR.ReadBytes(16);

                if (b.SequenceEqual(input))
                {
                    return i;
                }
            }

            return -1;
        }

        public static byte[] GenerateMD5(FileStream fileStream)
        {
            using (var md5 = MD5.Create())
            {
                byte[] result = md5.ComputeHash(fileStream);
                return result;
            }
        }

        public static void CopyStream(Stream input, Stream output, int bytes)
        {
            byte[] buffer = new byte[64*1024];
            int read;
            while (bytes > 0 && (read = input.Read(buffer, 0, Math.Min(buffer.Length, bytes))) > 0)
            {
                output.Write(buffer, 0, read);
                bytes -= read;
            }
            output.Seek(0, SeekOrigin.Begin);
        }

        public static string ReadNullTerminatedString(BinaryReader br)
        {
            string str = "";
            char ch;
            while ((ch = br.ReadChar()) != 0)
            {
                str = str + ch;
            }
            return str;
        }

        public static string StripChunkName(string str)
        {
            Regex rx0 = new Regex(@",.*?(?=\()");
            Regex rx1 = new Regex(@",.*?$");
            if (rx0.IsMatch(str)) { return rx0.Replace(str, ""); }
            else { return rx1.Replace(str, ""); }
        }

        public static string MakeFileNameHash()
        {
            string str = "doctorclinicopen";
            byte[] b = Encoding.ASCII.GetBytes(str); //To byte array, length 24.

            uint output = 0;

            for (int i = 0; i < b.Length; i++)
            {
                output = 16 * output + (Convert.ToUInt32(b[i]) & 0xDF);

                uint hiBit = output & 0xF0000000;
                if (hiBit != 0) //Doesn't get hit, assume this is correct but I'd have to do it by hand to make sure.
                {
                    output ^= hiBit | (hiBit >> 24);
                }
            }

            return output.ToString("X8"); //1264...
        }
    }
}
