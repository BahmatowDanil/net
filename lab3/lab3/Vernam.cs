using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    internal class Vernam
    {
        Dictionary<char, int> alph = new Dictionary<char, int>();
        Dictionary<int, char> alph_r = new Dictionary<int, char>();

        public Vernam(IEnumerable<char> Alphabet)
        {
            int i = 0;
            foreach (char c in Alphabet)
            {
                alph.Add(c, i);
                alph_r.Add(i++, c);
            }
        }


        public string Crypt(string Text, string Key)
        {
            char[] key = Key.ToCharArray();
            char[] text = Text.ToCharArray();
            var sb = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                int ind;
                if (alph.TryGetValue(text[i], out ind))
                {
                    sb.Append(alph_r[(ind ^ alph[key[i % key.Length]]) % alph.Count]);
                }
            }

            return sb.ToString();
        }
    }
}
