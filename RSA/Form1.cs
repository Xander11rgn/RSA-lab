using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace RSA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        BigInteger p, q, n, m, d, ee;

        private void button3_Click(object sender, EventArgs e)
        {
            string text = richTextBox2.Text;
            richTextBox7.Text = "";
            foreach (string str in text.Split(' '))
            {
                richTextBox7.Text += Convert.ToChar((int)BigInteger.ModPow(BigInteger.Parse(str), d, n)).ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            for (int i = 0; i < richTextBox1.Text.Length; i++)
            {
                BigInteger text = BigInteger.ModPow(BigInteger.Parse(Convert.ToInt64(richTextBox1.Text[i]).ToString()), ee, n);
                richTextBox2.Text += text.ToString() + " ";
            }
            richTextBox2.Text = richTextBox2.Text.Remove(richTextBox2.Text.Length - 1);
        }

        private bool testFerma(BigInteger n, int t)
        {
            for (int index = 1; index <= t; ++index)
            {
                if (BigInteger.ModPow(NextBigInteger(2, n - 2), n - 1, n) != 1)
                    return false;
            }
            return true;
        }

        public BigInteger NextBigInteger(int bitLength)
        {
            Random random = new Random();
            int a = bitLength / 8;
            int b = bitLength % 8;
            byte[] buffer = new byte[a + 1];
            random.NextBytes(buffer);
            byte c = (byte)(byte.MaxValue >> 8 - b);
            buffer[buffer.Length - 1] &= c;
            return new BigInteger(buffer);
        }

        public BigInteger NextBigInteger(BigInteger start, BigInteger end)
        {
            if (start == end)
                return start;
            BigInteger bigInteger1 = end;
            BigInteger bigInteger2;
            if (start > end)
            {
                end = start;
                start = bigInteger1;
                bigInteger2 = end - start;
            }
            else
                bigInteger2 = bigInteger1 - start;
            int num = 8 + 8 * bigInteger2.ToByteArray().Length;
            return NextBigInteger(num + 1) * bigInteger2 / BigInteger.Pow(2, num + 1) + start;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            while (true)
            {
                p = NextBigInteger(1024);
                while (!testFerma(p, 10))
                {
                    p = NextBigInteger(1024);
                }
                    
                q = NextBigInteger(1024);
                while (!testFerma(q, 10) || q == p)
                {
                    q = NextBigInteger(1024);

                }

                n = BigInteger.Multiply(p, q);
                richTextBox5.Text = richTextBox4.Text = n.ToString();

                m = BigInteger.Multiply(p - 1, q - 1);

                ee = 3;
                while (true)
                {
                    if (!(BigInteger.GreatestCommonDivisor(ee, m) == 1))
                        ee++;
                    else
                        break;
                }

                BigInteger k = 2;
                while (true)
                {
                    d = (k * m + 1) / ee;
                    if (!(BigInteger.GreatestCommonDivisor(d, m) == 1))
                        k += 2;
                    else
                        break;
                }
                richTextBox6.Text = d.ToString();
                richTextBox3.Text = ee.ToString();

                if (BigInteger.ModPow(BigInteger.ModPow(1072, ee, n), d, n) == 1072)
                {
                    break;
                }
            }
            
        }
    }
}
