using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Domain.Helper
{
    public class Base64Helper
    {
        public static string Encode(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        public static string Decode(string base64Input)
        {
            var bytes = Convert.FromBase64String(base64Input);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
