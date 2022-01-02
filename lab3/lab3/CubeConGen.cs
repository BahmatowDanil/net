using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    internal class CubeConGen
    {
        public int[] sequence = new int[10000];
        public int len = 1000;
        int a = 12, b = 64, c = 789, d = 132, m = 512;
        public void generateSeq(int key)
        {

            sequence[0] = key % m;
            for (int i = 0; i < len; i++)
            {
                sequence[i + 1] = Math.Abs((int)(d + sequence[i] * c + Math.Pow(sequence[i], 2) * b + Math.Pow(sequence[i], 3) * a) % m);
            }
        }

        public int generateElem(int key)
        {
            generateSeq(key);
            return sequence[50];
        }

    }
}
