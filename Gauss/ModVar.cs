using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Gauss
{
    class ModVar
    {

        BigInteger licznik;
        BigInteger mianownik;
        public ModVar()
        {
            this.licznik = 0;
            this.mianownik = 0;
        }
        public ModVar(BigInteger l, BigInteger m)
        {
            if (l != 0) {
            BigInteger gcd=BigInteger.GreatestCommonDivisor(l, m);
            this.licznik = l/gcd;
            this.mianownik = m/gcd;
            repairSign(this);
            }
            else
            {
                this.licznik = 0;
                this.mianownik = 0;
            }
        }
        private void repairSign(ModVar m)
        {
            if (m.mianownik < 0)
            {
                m.licznik *= (-1);
                m.mianownik *= (-1);
            }

        }

        public static ModVar operator +(ModVar left, ModVar right)
        {
            if (left.licznik == 0)
            {
                return right;
            }
            if(right.licznik == 0)
            {
                return left;
            }
            if (left.mianownik == right.mianownik)
            {
                return new ModVar(left.licznik + right.licznik, left.mianownik);
            }
            BigInteger gcd = BigInteger.GreatestCommonDivisor(left.mianownik, right.mianownik);
            return new ModVar(left.licznik * right.mianownik/gcd + right.licznik * left.mianownik/gcd , left.mianownik * right.mianownik/gcd );

        }
        public static ModVar operator -(ModVar left, ModVar right)
        {
            
            return left+(-right);
        }
        public static ModVar operator -(ModVar val)
        {
            return new ModVar(-val.licznik, val.mianownik);
        }
        public static ModVar operator *(ModVar left, ModVar right)
        {
            return new ModVar(left.licznik*right.licznik, left.mianownik*right.mianownik);
        }

        public static ModVar operator /(ModVar left, ModVar right)
        {
            
            return left* new ModVar(right.mianownik, right.licznik);
        }
        public static bool operator >(ModVar left, ModVar right)
        {
            BigInteger check1 = left.licznik * right.mianownik;
            BigInteger check2 = right.licznik * left.mianownik;
            return BigInteger.Abs(check1) > BigInteger.Abs(check2);
        }
        public static bool operator <(ModVar left, ModVar right)
        {
            BigInteger check1 = left.licznik * right.mianownik;
            BigInteger check2 = right.licznik * left.mianownik;
            return BigInteger.Abs(check1) < BigInteger.Abs(check2);
        }
        
        public override String ToString()
        {
            return licznik.ToString() + "/" + mianownik.ToString();
        }
        

    }
}
