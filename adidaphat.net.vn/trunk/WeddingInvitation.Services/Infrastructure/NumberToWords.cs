using System;

namespace WeddingInvitation.Services.Infrastructure
{
    public class NumberToWords
    {
        public static string ChangeNumericToWords(double numb)
        {
            var num = numb.ToString();
            return ChangeToWords(num, false);
        }

        public static string ChangeCurrencyToWords(string numb)
        {
            return ChangeToWords(numb, true);
        }

        public static string ChangeNumericToWords(string numb)
        {
            return ChangeToWords(numb, false);
        }

        public static string ChangeCurrencyToWords(decimal numb)
        {
            return ChangeToWords(String.Format("{0:0}", numb), true);
        }

        private static string ChangeToWords(string numb, bool isCurrency)
        {
            var val = "";
            var wholeNo = numb;
            var andStr = "";
            var pointStr = "";
            var endStr = (isCurrency) ? ("đồng chẵn") : ("");
            try
            {
                var decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    var points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = (isCurrency) ? ("và") : ("chấm");// just to separate whole numbers from points/Rupees
                        endStr = (isCurrency) ? ("đồng" + endStr) : ("");
                        pointStr = TranslateRupees(points);
                    }
                }
                val = String.Format("{0} {1}{2}{3}", TranslateWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch
            {
            }
            return val;
        }

        private static string TranslateWholeNumber(string number)
        {
            var word = "";
            try
            {
                var isDone = false;//test if already translated
                var dblAmt = (Convert.ToDouble(number));
                //if ((dblAmt > 0) && number.StartsWith("0"))

                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric
                    var beginsZero = number.StartsWith("0");//tests for 0XX
                    var numDigits = number.Length;
                    var pos = 0;//store digit grouping
                    var place = "";//digit grouping name:hundres,thousand,etc...
                    switch (numDigits)
                    {
                        case 1://ones' range
                            word = Ones(number);
                            isDone = true;
                            break;
                        case 2://tens' range
                            word = Tens(number);
                            isDone = true;
                            break;
                        case 3://hundreds' range
                            pos = (numDigits % 3) + 1;
                            place = " trăm ";
                            break;
                        case 4://thousands' range
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " nghìn ";
                            break;
                        case 7://millions' range
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " triệu ";
                            break;
                        case 10://Billions's range
                            pos = (numDigits % 10) + 1;
                            place = " tỷ ";
                            break;
                        //add extra case options for anything above Billion...
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)
                        word = TranslateWholeNumber(number.Substring(0, pos)) + place + TranslateWholeNumber(number.Substring(pos));
                        //check for trailing zeros
                        if (beginsZero) word = " và " + word.Trim();
                    }
                    //ignore digit grouping names
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch
            {
            }
            return word.Trim();
        }

        private static string Tens(string digit)
        {
            var digt = Convert.ToInt32(digit);
            string name = null;
            switch (digt)
            {
                case 10:
                    name = "mười";
                    break;
                case 11:
                    name = "mười một";
                    break;
                case 12:
                    name = "mười hai";
                    break;
                case 13:
                    name = "mười ba";
                    break;
                case 14:
                    name = "mười bốn";
                    break;
                case 15:
                    name = "mười lăm";
                    break;
                case 16:
                    name = "mười sáu";
                    break;
                case 17:
                    name = "mười bảy";
                    break;
                case 18:
                    name = "mười tám";
                    break;
                case 19:
                    name = "mười chín";
                    break;
                case 20:
                    name = "hai mươi";
                    break;
                case 30:
                    name = "ba mươi";
                    break;
                case 40:
                    name = "bốn mươi";
                    break;
                case 50:
                    name = "năm mươi";
                    break;
                case 60:
                    name = "sáu mươi";
                    break;
                case 70:
                    name = "bảy mươi";
                    break;
                case 80:
                    name = "tám mươi";
                    break;
                case 90:
                    name = "chín mươi";
                    break;
                default:
                    if (digt > 0)
                    {
                        name = Tens(digit.Substring(0, 1) + "0") + " " + Ones(digit.Substring(1), false);
                    }
                    break;
            }
            return name;
        }

        private static string Ones(string digit, bool isFirstNumber = true)
        {
            var digt = Convert.ToInt32(digit);
            var name = "";
            switch (digt)
            {
                case 1:
                    name = isFirstNumber ? "một" : "mốt";
                    break;
                case 2:
                    name = "hai";
                    break;
                case 3:
                    name = "ba";
                    break;
                case 4:
                    name = "bốn";
                    break;
                case 5:
                    name = isFirstNumber ? "năm" : "lăm";
                    break;
                case 6:
                    name = "sáu";
                    break;
                case 7:
                    name = "bảy";
                    break;
                case 8:
                    name = "tám";
                    break;
                case 9:
                    name = "chín";
                    break;
            }
            return name;
        }

        private static string TranslateRupees(string rupees)
        {
            var cts = "";
            for (var i = 0; i < rupees.Length; i++)
            {
                var digit = rupees[i].ToString();
                var engOne = "";
                if (digit.Equals("0"))
                {
                    engOne = "không";
                }
                else
                {
                    engOne = Ones(digit);
                }
                cts += " " + engOne;
            }
            return cts;
        }
    }
}
