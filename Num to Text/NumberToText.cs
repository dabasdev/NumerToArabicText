using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Num_to_Text
{
    public class NumberToText
    {
        // Arrays of Text numbers
        private readonly string[] _range019 = { "", "واحد", "إثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة", "عشرة", "أحد عشر", "إثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };
        private readonly string[] _range2090 = { "", "", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };
        private readonly string[] _range100900 = { "", "مائة", "مائتان", "ثلاثمائة", "أربعمائة", "خمسمائة", "ستمائة", "سبعمائة", "ثمانمائة", "تسعمائة" };

        // قوائم من الالف الى الترليون يمكنك اضافة اعداد اخري 
        // عن طريق الاضافة المباشرة او باستخام أمر 
        // add_new
        // وكذلك استخدام show_max لاظهار اخر رقم تمت اضافته

        private readonly List<string> _groupOnes = new List<string>() { "", "ألف", "مليون", "مليار", "ترليون" };
        private readonly List<string> _groupTwos = new List<string>() { "", "ألفان", "مليونان", "ملياران", "ترليونان" };
        private readonly List<string> _groupThreeTen = new List<string>() { "", "ألاف", "ملايين", "مليارات", "ترليونات" };
        private readonly List<string> _groupOverTen = new List<string>() { "", "ألفاً", "مليوناً", "ملياراً", "ترليوناً" };

        private const string And = "  و ";
        private const string Zero = "صفر ";
        //public string Dinar = "دينار";
        //public string Dirham = "درهم";

        // This method shows the last large number in the list "Group_Ones" or add by add_new method.   
        public string show_max()
        {
            return _groupOnes[_groupOnes.Count() - 1];
        }

        // This method inserts new LARGE NUMBER to given lists
        // Here are some of LARGE NUMBERS
        //كوادرليون 15- كونتليون 18- سكستليون 21- سبيتلليون 24- أوكتليون 27- نونيلليون 30- دشيليون 33
        public void Add_new(string largeNumber)
        {
            if (!_groupOnes.Contains(largeNumber))
            {
                _groupOnes.Add(largeNumber);
                _groupTwos.Add(largeNumber + "ان");
                _groupThreeTen.Add(largeNumber + "ات");
                _groupOverTen.Add(largeNumber + "اً");
            }
            else
            {
                throw new System.ArgumentException("هذا الرقم موجود مسبقا", "This Large number already exist");
            }
        }

        // Convert (integer) number to text
        public string IntToText(string integerNumber)
        {
            // Remove left zeros
            integerNumber = integerNumber.TrimStart('0');

            // Definition of variables
            string converted = string.Empty, convertAll = string.Empty;
            var count = 0;

            // Cut  the Number every three chars starting from the end of Number 
            for (var i = integerNumber.Count(); i > 0; i -= 3)
            {
                if (count >= _groupOnes.Count)
                {
                    throw new System.ArgumentException("تم تجاوز اكبر رقم، استخدم امر الاضافة لادراج رقم كبير جديد", "Max large number exceeded, use add_new to add a new large number");
                }

                string substr;

                if (i > 2)
                {
                    substr = integerNumber.Substring(i - 3, 3);
                }
                else
                {
                    substr = integerNumber.Substring(0, i);
                    substr = new string('0', 3 - substr.Length) + substr;
                }

                // Manipulate the substr of three chars and convert it 
                // First: Ones and Tens

                var substrToInt = int.Parse(substr.Substring(1, 2));
                if (substrToInt < 20)
                {
                    converted = ToWords(_range019, substr, 1, 2);
                }
                else switch (true)
                    {
                        case true when (substrToInt % 10) == 0:
                            converted = ToWords(_range2090, substr, 1);
                            break;
                        case true when (substrToInt % 10) != 0:
                            converted = ToWords(_range019, substr, 2) + And + ToWords(_range2090, substr, 1);
                            break;
                    }

                // Second: Hundreds
                substrToInt = int.Parse(substr);
                if (substrToInt > 99)
                {
                    if (substr.Substring(1, 2) == "00")
                    {
                        converted = ToWords(_range100900, substr, 0);
                    }
                    else
                    {
                        converted = ToWords(_range100900, substr, 0) + And + converted;
                    }
                }

                switch (substrToInt)
                {
                    case 1 when count > 0:
                        convertAll = (converted.Replace("واحد", _groupOnes[count]) + And + convertAll).Trim();
                        break;
                    // Manipulate thousands and above
                    case 1:
                        convertAll = (converted + " " + _groupOnes[count] + And + convertAll).Trim();
                        break;
                    case 2 when count > 0:
                        convertAll = (converted.Replace("إثنان", _groupTwos[count]) + And + convertAll).Trim();
                        break;
                    case 2:
                        convertAll = (converted + " " + _groupTwos[count] + And + convertAll).Trim();
                        break;
                    default:
                        {
                            if (substrToInt > 2)
                            {
                                convertAll = substrToInt < 11 ? (converted + " " + _groupThreeTen[count] + And + convertAll).Trim() :
                                    (converted + " " + _groupOverTen[count] + And + convertAll).Trim();
                            }

                            break;
                        }
                }
                count++;
            }

            // Remove extra joining arabic char at the end.
            if ((convertAll.Trim()).EndsWith("و"))
            {
                convertAll = (convertAll.Trim()).Remove(convertAll.Length - 2);
            }
            return convertAll;
        } // End of NoToText method.

        // Method to catch text number 
        private static string ToWords(IReadOnlyList<string> range, string subst, int a, int b = 1)
        {
            return range[int.Parse(subst.Substring(a, b))];
        }

        // Main Method
        public string DblToText(string doubleNumber, string mainCurrency, string partCurrency)
        {

            if (doubleNumber == "")
            { return ""; }

            if (double.Parse(doubleNumber) == 0)
            { return Zero; }

            var number = new string[2];
            string finalText;
            if (doubleNumber.Contains("."))
            {
                number = doubleNumber.Split('.');

                if (int.Parse(number[0]) == 0)
                {
                    finalText = IntToText(number[1]) + partCurrency;
                }
                else
                {
                    if (number[1] == "")
                    {
                        finalText = IntToText(number[0]) + mainCurrency;
                    }
                    else if (int.Parse(number[1]) == 0)
                    {
                        finalText = IntToText(number[0]) + mainCurrency;
                    }
                    else
                    {
                        finalText = IntToText(number[0]) + mainCurrency + And + IntToText(number[1]) + partCurrency;
                    }
                }
            }
            else
            {
                number[0] = doubleNumber;
                finalText = IntToText(number[0]) + mainCurrency;
            }

            return $"# {finalText} #";
        }


    }
}
