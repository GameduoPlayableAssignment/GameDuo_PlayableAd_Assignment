using System;
using System.Numerics;
using UnityEngine;

namespace Helper
{
    public class BigIntegerHelper
    {
        private static readonly BigInteger CORRECTION_VALUE_BIGINTEGER = new (1000000);
        private static readonly int CORRECTION_VALUE_INT = 1000000;
        private static readonly BigInteger VALUE_BIGINTEGER_10 = new (10);
        private static readonly int MAX_CALC_LENGTH = 13;

        private const char CHAR_A = 'A';
        private const char CHAR_DOT = '.';

        private const string STRING_DOT = ".";

        public static BigInteger MultiplyFloor(BigInteger leftValue, decimal rightValue)
        {
            return leftValue * new BigInteger(rightValue * CORRECTION_VALUE_INT) / CORRECTION_VALUE_BIGINTEGER;
        }
    
        public static BigInteger MultiplyFloor(BigInteger leftValue, double rightValue)
        {
            return leftValue * new BigInteger(rightValue * CORRECTION_VALUE_INT) / CORRECTION_VALUE_BIGINTEGER;
        }

        /// <summary>
        /// (중요) 나누기를 하다가 범위를 벗어나면 double.MaxValue 값으로 설정함
        /// </summary>
        public static double Divide(BigInteger leftValue, BigInteger rightValue)
        {
            return (double)leftValue / (double)rightValue;
        
            string leftValueString = leftValue.ToString();
            string rightValueString = rightValue.ToString();

            // 1. 나누는 수를 범위 내로 줄이고, 그에 맞도록 나눠지는 수도 길이를 잘라서 조정함
            if (rightValueString.Length > MAX_CALC_LENGTH)
            {
                leftValueString = leftValueString.Substring(0, leftValueString.Length - (rightValueString.Length - MAX_CALC_LENGTH));
                rightValueString = rightValueString.Substring(0, MAX_CALC_LENGTH);
            }
        
            // (s) 범위를 벗어날 경우 무조건 double의 최대값으로 대치
            try
            {
                double result = double.Parse(leftValueString) / double.Parse(rightValueString);
                return result;
            }
            catch (OverflowException e)
            {
                return double.MaxValue;
            }
            // (e) 범위를 벗어날 경우 무조건 double의 최대값으로 대치
        }
    
        public static string ToShortString(BigInteger bigInteger, int decimalPlaces = 3)
        {
            string s = bigInteger.ToString();
            int l = s.Length;

            // ex: 120 => 120 (알파벳 붙지 않음), -102 => -102 (알파벳 붙지 않음)
            if (l <= 3 || (s[0] == '-' && l <= 4))
            {
                return s;
            }

            int divider = l / 3;
            int remainder = l % 3;
            int dotIndex;

            if (remainder > 0)
            {
                dotIndex = remainder;
            }
            else
            {
                dotIndex = 3;
                divider -= 1;
            }

            string dotLeftNumber = s.Substring(0, dotIndex);
            string dotRightNumber = s.Substring(dotIndex, decimalPlaces);
            string numberAlphabet = GetAlphabets(divider);

            return string.Format($"{dotLeftNumber}.{dotRightNumber}{numberAlphabet}");
        }

        private static string GetAlphabets(int divider)
        {
            string result = string.Empty;
        
            int digitNumber = 1;
            while (true)
            {
                if (Math.Pow(26, digitNumber) >= divider)
                    break;
                digitNumber++;
            }

            for (int i = digitNumber - 1; i >= 0; i--)
            {
                int temp = 1;
                while (true)
                {
                    // 다음 temp 값으로 범위를 벗어나면 현재것으로 정해서 계산하면 됨 (이해가 헷갈렸음) 
                    if (Math.Pow(26, i) * (temp + 1) > divider)
                        break;
                
                    temp++;
                }
                divider = (int)(divider - (Math.Pow(26, i) * temp));
                result += GetAlphabet(temp);
            }
        
            return result;
        }

        
        
        private static string GetAlphabetsV2(int divider)
        {
            string result = string.Empty;

            int digitCount = 26; // 알파벳 갯수
            int digitPositionCount = 1; // 자리수

            int cumulativeCursorValue = 0;
            // int startDigitPositionCount = 0;
            // int startValue = 0;
            
            while (true)
            {
                if (Math.Pow(digitCount, digitPositionCount) > divider)
                    break;
                
                cumulativeCursorValue += (int)Math.Pow(digitCount, digitPositionCount);
                digitPositionCount++;
            }

            int[] tempArray = new int[digitPositionCount];
            for (int i = 0; i < tempArray.Length; i++)
            {
                tempArray[i] = 1;
            }
            
            for (int i = digitPositionCount - 1; i >= 0; i--)
            {
                int temp = 1;
                while (true)
                {
                    // 다음 temp 값으로 범위를 벗어나면 현재것으로 정해서 계산하면 됨 (이해가 헷갈렸음) 
                    if (Math.Pow(26, i) * (temp + 1) > divider)
                        break;
                
                    temp++;
                }
                divider = (int)(divider - (Math.Pow(26, i) * temp));
                result += GetAlphabet(temp);
            }
        
            return result;
        }
        
        
        
        
        private static string GetAlphabet(int divider)
        {
            return ((char)(divider - 1 + CHAR_A)).ToString();
        }
    
        public static BigInteger Create(string alphabetNumber)
        {
            return _GetBigIntegerFromAlphabetNumber(alphabetNumber);
        }

        public static BigInteger Create(decimal decimalNumber)
        {
            return new BigInteger(decimal.ToInt32(decimalNumber));
        }
    
        private static BigInteger _GetBigIntegerFromAlphabetNumber(string alphabetNumber)
        {
            BigInteger returnValue;
        
            bool hasAlphabet = char.IsLetter(alphabetNumber[alphabetNumber.Length - 1]);

            if (hasAlphabet)
            {
                int alphabetStartIndex = alphabetNumber.Length - 1;
                for (int i = alphabetNumber.Length - 1; i >= 0; i--)
                {
                    if (char.IsLetter(alphabetNumber[i]) == false)
                        break;
                    alphabetStartIndex = i;
                }

                string alphabets = alphabetNumber.Substring(alphabetStartIndex);
                int length = GetAlphabetsLength(alphabets);
                int dotIndex = alphabetNumber.IndexOf(CHAR_DOT);
            
                if (dotIndex > 0)
                {
                    int dotNumberLength = (alphabetStartIndex - 1) - dotIndex;
                    length -= dotNumberLength;
                
                    string onlyNumbers = alphabetNumber.Substring(0, alphabetStartIndex).Replace(STRING_DOT, string.Empty);
                    returnValue = BigInteger.Parse(onlyNumbers);
                }
                else
                {
                    string onlyNumbers = alphabetNumber.Substring(0, alphabetStartIndex);
                    returnValue = BigInteger.Parse(onlyNumbers);
                }
            
                for (int i = 0; i < length; i++)
                {
                    returnValue *= VALUE_BIGINTEGER_10;
                }
            }
            else
            {
                try
                {
                    returnValue = BigInteger.Parse(alphabetNumber);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return returnValue;
        }

        private static int GetAlphabetsLength(string alphabets)
        {
            int length = 0;
            for (int i = 0; i < alphabets.Length; i++)
            {
                // 1의 자리부터 차례대로 계산한다.
                string digit = alphabets.Substring(alphabets.Length - (i + 1), 1);

                // 자리수에 따라 26의 n제곱으로 늘어난다.
                int multiply = (int)Mathf.Pow(26, i) * 3;

                // 알파벳 모양에 따라 계산해 준다.
                int num = (Convert.ToChar(digit) - CHAR_A + 1) * multiply;
                length += num;
            }
            return length;
        }
    }
}