using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VR.Infrastructure.Utilities
{
    public static class StringExtension
    {
        public static bool StringIsNullEmptyWhiteSpace(this string obj)
        {
            if (string.IsNullOrEmpty(obj) || (string.IsNullOrWhiteSpace(obj)))
                return true;
            return false;
        }
    }
}