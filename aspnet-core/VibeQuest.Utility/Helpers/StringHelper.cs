using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Helpers
{
    public static class StringHelper
    {
        public static string ToBase64UrlEncode(this string input)
        {
            return Base64UrlEncoder.Encode(input);
        }

        public static string ToBase64UrlDecode(this string input)
        {
            return Base64UrlEncoder.Decode(input);
        }
        public static string ToHtmlEncode(this string input)
        {
            return HtmlEncoder.Default.Encode(input);
        }
        public static string ReplaceWith(this string input, Dictionary<string, string> replace)
        {
            foreach (var keyValue in replace)
            {
                input = input.Replace(keyValue.Key, keyValue.Value);
            }

            return input;
        }
    }
}
