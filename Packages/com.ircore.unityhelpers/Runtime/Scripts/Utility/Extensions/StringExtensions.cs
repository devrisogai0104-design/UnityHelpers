using System;
using System.Collections.Generic;
using System.Text;

namespace IRCore.UnityHelpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Unicode •¶š—ñ‚©‚ç Shift-JIS •¶š—ñ‚É•ÏŠ·‚µ‚Ä•Ô‚·
        /// </summary>
        public static string ToShiftJis(this string unicodeStrings)
        {
            var unicode = Encoding.Unicode;
            var unicodeByte = unicode.GetBytes(unicodeStrings);
            var s_jis = Encoding.GetEncoding("shift_jis");
            var s_jisByte = Encoding.Convert(unicode, s_jis, unicodeByte);
            var s_jisChars = new char[s_jis.GetCharCount(s_jisByte, 0, s_jisByte.Length)];
            s_jis.GetChars(s_jisByte, 0, s_jisByte.Length, s_jisChars, 0);
            return new string(s_jisChars);
        }

        /// <summary>
        /// •¶š—ñ‚ğw’è‚µ‚½•¶š”‚Å•ªŠ„‚·‚é
        /// </summary>
        public static string[] SubstringAtCount(this string self, int count)
        {
            var result = new List<string>();
            var length = (int)Math.Ceiling((double)self.Length / count);

            for (int i = 0; i < length; i++)
            {
                int start = count * i;
                if (self.Length <= start)
                {
                    break;
                }
                if (self.Length < start + count)
                {
                    result.Add(self.Substring(start));
                }
                else
                {
                    result.Add(self.Substring(start, count));
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// •¶š—ñ“à‚Ì‰üs•¶š‚ğíœ‚µ‚½•¶š—ñ‚ğ•Ô‚·
        /// </summary>
        public static string RemoveNewLine(this string self)
        {
            return self.Replace("\r", "").Replace("\n", "");
        }
    }
}