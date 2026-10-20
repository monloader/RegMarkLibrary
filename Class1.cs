using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace REG_MARK_LIB
{
    public class RegMark
    {
        public static Boolean CheckMark(String mark)
        {
            if (string.IsNullOrEmpty(mark))
            {
                return false;
            }
            string pattern = (@"^([A-Z])(\d{3})([A-Z]{2})(\d{2,3})$");
            Regex regex = new Regex(pattern);

            mark = mark.ToUpper();

            if (!Regex.IsMatch(mark, pattern))
            {
                return false;
            }

            string firstLetter = mark.Substring(0, 1);
            string numberPart = mark.Substring(1, 3);
            string lastLetters = mark.Substring(4, 2);
            string regionPart = mark.Substring(6);

            if (firstLetter != "A" && firstLetter != "B" && firstLetter != "E" && firstLetter != "K" && firstLetter != "M" &&
                firstLetter != "H" && firstLetter != "O" && firstLetter != "P" && firstLetter != "C" && firstLetter != "T" &&
                firstLetter != "Y" && firstLetter != "X")
            {
                return false;
            }

            if (!int.TryParse(numberPart, out int number) || number < 1 || number > 999)
            {
                return false;
            }

            foreach (char letter in lastLetters)
            {
                string currentLetter = letter.ToString();
                if (currentLetter != "A" && currentLetter != "B" && currentLetter != "E" && currentLetter != "K" && currentLetter != "M" &&
                    currentLetter != "H" && currentLetter != "O" && currentLetter != "P" && currentLetter != "C" && currentLetter != "T" &&
                    currentLetter != "Y" && currentLetter != "X")
                {
                    return false;
                }
            }

            if (!int.TryParse(regionPart, out int region) || region < 1 || region > 199)
            {
                return false;
            }

            return true;
        }

        public static string GetNextMarkAfter(string mark)
        {
            if (!CheckMark(mark))
            {
                return ("Неверный номерной знак");
            }

            mark = mark.ToUpper();

            string firstLetter = mark.Substring(0, 1);
            int numberPart = int.Parse(mark.Substring(1, 3));
            string lastLetters = mark.Substring(4, 2);
            int region = int.Parse(mark.Substring(6));

            numberPart++;

            if (numberPart > 999)
            {
                numberPart = 1;

                char firstChar = lastLetters[0];
                char secondChar = lastLetters[1];

                if (secondChar != 'X')
                {
                    secondChar = NextLetter(secondChar);
                }
                else
                {
                    secondChar = 'A';
                    if (firstChar != 'X')
                    {
                        firstChar = NextLetter(firstChar);
                    }
                    else
                    {
                        firstChar = 'A';
                        secondChar = 'A';

                        if (firstLetter != "X")
                        {
                            firstLetter = NextLetter(firstLetter[0]).ToString();
                        }
                        else
                        {
                            firstLetter = "A";
                            region++;
                            if (region > 199)
                            {
                                region = 1;
                            }
                        }
                    }
                }

                lastLetters = $"{firstChar}{secondChar}";
            }

            string regionString = region.ToString();
            if (region < 10 && regionString.Length == 1)
            {
                regionString = region.ToString("D2");
            }

            return $"{firstLetter}{numberPart:D3}{lastLetters}{regionString}";
        }

        private static char NextLetter(char currentLetter)
        {
            switch (currentLetter)
            {
                case 'A': return 'B';
                case 'B': return 'E';
                case 'E': return 'K';
                case 'K': return 'M';
                case 'M': return 'H';
                case 'H': return 'O';
                case 'O': return 'P';
                case 'P': return 'C';
                case 'C': return 'T';
                case 'T': return 'Y';
                case 'Y': return 'X';
                case 'X': return 'A';
                default: return 'A';
            }
        }
    }
}
