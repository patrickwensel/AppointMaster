using System;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPI.DataBase {
    public class Util {

        public static string CharNumericalSet = "0123456789";
        public static string CharLowerAlphaSet = "abcdefghijklmnopqrstuvwxyz";
        public static string CharUpperAlphaSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string CharAlphaSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        public static string CharAlphaNumSet = CharLowerAlphaSet + CharUpperAlphaSet + CharNumericalSet;
        public static string CharSeparatorsWithoutSpace = @"`~!@#$%^&*()-_=+{}[];:'<>,.?/" + '"' + char.Parse(@"\");
        public static string CharFileInvalidSet = "/:*?<>|" + '"' + @"\";
        public static string CharEmailLocalPartValidSet = CharAlphaSet + CharNumericalSet + "!#$%&'*+-/=?^_`{|}~";
        public static string CharDomainNameValidSet = CharAlphaSet + CharNumericalSet + ".-";
        public static string LocalPartEmailValidSet = "!#$%&'*+-/=?^_`{|}~" + CharAlphaSet + CharNumericalSet + ".";

        public static DateTime safeDateParse(string s, System.IFormatProvider ifp) {
            try {
                DateTime d = DateTime.Parse(s, ifp);
                return d;
            } catch {
                return DateTime.MinValue;
            }
        }

        public static DateTime safeDateParse(string s, DateTime defaultDate) {
            try {
                DateTime d = DateTime.Parse(s);
                return d;
            } catch {
                return defaultDate;
            }
        }


        public static DateTime safeDateParse(string s) {
            return safeDateParse(s, DateTime.MinValue);
        }



        public static long safeLongParse(string s) {
            if (s != null) {
                try {
                    return (long.Parse(s));
                } catch {
                    return 0;
                }
            } else
                return 0;
        }

        public static int safeIntParse(string s) {
            if (s != null) {
                try {
                    return (int.Parse(s));
                } catch {
                    return 0;
                }
            } else
                return 0;
        }

        public static bool isValidEmailAddress(string emailAddress) {
            string patternLenient = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex reLenient = new Regex(patternLenient);
            string patternStrict = @"^(([^<>()[\]\\.,;:\s@\""]+"
                + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                + @"[a-zA-Z]{2,}))$";
            Regex reStrict = new Regex(patternStrict);

            if (reStrict.IsMatch(emailAddress)) {
                string[] elts = emailAddress.ToLower().Split('@');
                if (elts.Length > 1) {
                    string domain = elts[1];
                    return (domain != "none.com");
                }
                return (emailAddress.ToLower().IndexOf("@none.com") < 0);
            } else
                return false;
        }

        
        
        private static string getTraceFilename() {
            try {
                string directory = "c:\\LPIServer";
                if (!System.IO.Directory.Exists(directory)) {
                    directory = "c:\\temp\\LPITrace";
                }
                if (!System.IO.Directory.Exists(directory)) {
                    System.IO.Directory.CreateDirectory(directory);
                }
                return directory + "\\traceDashBoard" + DateTime.Today.Year.ToString() + "_" + DateTime.Today.Month.ToString() + "_" + DateTime.Today.Day.ToString()  +".txt";
            } catch (Exception ex) {
                return "";
            }
        }

        public static void Trace(string message){
            try {
                string filename = getTraceFilename();
                System.IO.StreamWriter sw = new System.IO.StreamWriter(filename, true);
                sw.WriteLine(DateTime.Now.ToShortTimeString().PadLeft(15)+": "+ message);
                sw.Close();
            } catch (Exception ex) {
            }
        }

        public static string getServerName() 
        {
            string server = GetConfigValue("server");

            return server;
        }

        public static string getLocalListingURL() {

            string localListing = GetConfigValue("localListing");

            return localListing;
        }

        public static string stringCharsInSet(string originalString, string validSet) {
            string s = "";
            for (int i = 0; i < originalString.Length; i++) {
                if (validSet.IndexOf(originalString[i]) >= 0)
                    s += originalString[i];
            }
            return s;
        }


        public static string phoneDigits(string phone) {
            return stringCharsInSet(phone, Util.CharNumericalSet);
        }

        public static bool isValidPhoneNumber(string number) {
            if (!string.IsNullOrEmpty(number)) {
                if ((Util.phoneDigits(number).Length == 10) | ((Util.phoneDigits(number).Length == 11) & (number.StartsWith("1")))) {
                    if ((number == "1111111111") | (number == "2222222222") | (number == "3333333333") | (number == "4444444444") | (number == "5555555555") |
                        (number == "6666666666") | (number == "7777777777") | (number == "8888888888") | (number == "9999999999"))
                        return false;
                    if ((number.IndexOf("111111")>=0) | (number.IndexOf("222222")>=0) | (number.IndexOf("333333")>=0)| (number.IndexOf("444444")>=0)
                        | (number.IndexOf("555555")>=0)| (number.IndexOf("666666")>=0)| (number.IndexOf("777777")>=0)| (number.IndexOf("888888")>=0)
                        | (number.IndexOf("999999")>=0)| (number.IndexOf("000000")>=0))
                        return false;

                    long lPhone = Util.safeLongParse(number);
                    if (lPhone > 0) {
                        if (Util.phoneDigits(number).Length == 10)
                            lPhone = Util.safeLongParse(number.Substring(3));
                        else
                            lPhone = Util.safeLongParse(number.Substring(4));

                        return lPhone > 0;
                    }
                }
            }
            return false;
        }
        
        public static string phoneForDisplay(string digitsPhone) {
            if (digitsPhone.Length > 10) {
                return "(" + digitsPhone.Substring(0, 3) + ") " + digitsPhone.Substring(3, 3) + "-" + digitsPhone.Substring(6, 4) + " #" + digitsPhone.Substring(10);
            } else if (digitsPhone.Length == 10)
                return "(" + digitsPhone.Substring(0, 3) + ") " + digitsPhone.Substring(3, 3) + "-" + digitsPhone.Substring(6, 4);
            else
                return digitsPhone;
        }


        public static string GetConfigValue(string variable) {

            switch (variable)
            {
                case "server":
                    return ConfigurationManager.AppSettings["server"];
                case "connnectionstring":
                    return ConfigurationManager.AppSettings["connnectionstring"];
                case "AMserver":
                    return ConfigurationManager.AppSettings["AMserver"];
                case "html_source_pages":
                    return ConfigurationManager.AppSettings["html_source_pages"];
                case "SMTP_SERVER":
                    return ConfigurationManager.AppSettings["SMTP_SERVER"];
                case "SMTP_SERVER_USERID":
                    return ConfigurationManager.AppSettings["SMTP_SERVER_USERID"];
                case "SMTP_SERVER_PASSWORD":
                    return ConfigurationManager.AppSettings["SMTP_SERVER_PASSWORD"];
                case "SMTP_PORT":
                    return ConfigurationManager.AppSettings["SMTP_PORT"];
                case "SMTP_SSL":
                    return ConfigurationManager.AppSettings["SMTP_SSL"];
                case "EMAIL_SENDING":
                    return ConfigurationManager.AppSettings["EMAIL_SENDING"];
                case "FROM_NOTIFICATION":
                    return ConfigurationManager.AppSettings["FROM_NOTIFICATION"];
                case "localListing":
                    return ConfigurationManager.AppSettings["localListing"];
                case "ADMIN_PASSWORD":
                    return ConfigurationManager.AppSettings["ADMIN_PASSWORD"];
                case "FIRST_CDR_DATE":
                    return ConfigurationManager.AppSettings["FIRST_CDR_DATE"];
                case "WebServicePassword":
                    return ConfigurationManager.AppSettings["WebServicePassword"];
                case "NOTIFY_EMAIL_0":
                    return ConfigurationManager.AppSettings["NOTIFY_EMAIL_0"];
                case "NOTIFY_EMAIL_NAME_0":
                    return ConfigurationManager.AppSettings["NOTIFY_EMAIL_NAME_0"];
                case "NOTIFY_EMAIL_1":
                    return ConfigurationManager.AppSettings["NOTIFY_EMAIL_1"];
                case "NOTIFY_EMAIL_NAME_1":
                    return ConfigurationManager.AppSettings["NOTIFY_EMAIL_NAME_1"];
                case "NOTIFY_EMAIL_2":
                    return ConfigurationManager.AppSettings["NOTIFY_EMAIL_2"];
                case "NOTIFY_EMAIL_NAME_3":
                    return ConfigurationManager.AppSettings["NOTIFY_EMAIL_NAME_3"];
                case "NOTIFY_EMAIL_3":
                    return ConfigurationManager.AppSettings["NOTIFY_EMAIL_3"];
                case "NOTIFY_EMAIL_NAME_2":
                    return ConfigurationManager.AppSettings["NOTIFY_EMAIL_NAME_2"];
                default:
                    return null;
            }
        }

        public static string getTextFileContent(string filename, bool insertLineFeed, bool htmlFormat) {
            try {
                System.IO.StreamReader sr = new System.IO.StreamReader(filename);
                if (sr != null) {
                    string s = sr.ReadLine();
                    string content = "";
                    while (s != null) {
                        content += s;
                        if (insertLineFeed)
                            content += Environment.NewLine;
                        if (htmlFormat)
                            content += "<br>";
                        s = sr.ReadLine();
                    }
                    sr.Close();
                    return content;
                }
                return "";
            } catch {
                return "";
            }
        }

        public static string EncodeBase64String(string sInput) {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sInput));
        }

        public static string DecodeBase64String(string sInput) {
            try {
                return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(sInput));
            } catch {
                return "";
            }
        }

        static System.Random r = new Random();
        public static string GetRandomPassword(int nbChar) {
            int seed;
            seed = System.DateTime.Now.Millisecond;
            string possible = "98765432abcdefghjkmnpqrstuvwxyzABCDEFGHJKMNPQRSTUVWXYZ23456789";
            string result = "";

            int j = 0;
            int max = DateTime.Now.Second;
            if (max < nbChar / 2)
                max = nbChar;
            for (int i = 0; i < max; i++) {
                j = r.Next(seed);
            }

            for (int i = 0; i < nbChar; i++) {
                result += possible.Substring(r.Next(1, possible.Length - 1), 1);
            }

            return result;
        }

        /*
        private static char GetEncryptedChar(int forNumber) {
            Random n = new Random(DateTime.Now.Millisecond);
            int randonIndex = n.Next(3);
            char[] possibleChars;

            if (forNumber == 0) {
                possibleChars = new char[3] { 'f', 'g', '5' };
            }
            if (forNumber == 1) {
                possibleChars = new char[3] { 'f', '9', 'g' };
            }
            if (forNumber == 2) {
                possibleChars = new char[3] { 'f', 'g', '0' };
            }
            if (forNumber == 3) {
                possibleChars = new char[3] { 'f', 'g', '1' };
            }
            if (forNumber == 4) {
                possibleChars = new char[3] { 'f', 'g', '7' };
            }
            if (forNumber == 5) {
                possibleChars = new char[3] { 'f', 'g', '8' };
            }
            if (forNumber == 6) {
                possibleChars = new char[3] { 'f', 'g', '6' };
            }
            if (forNumber == 7) {
                possibleChars = new char[3] { 'f', 'g', '4' };
            }
            if (forNumber == 8) {
                possibleChars = new char[3] { 'f', 'g', '2' };
            }
            if (forNumber == 9) {
                possibleChars = new char[3] { 'z', 'a', '3' };
            }
            return possibleChars[randonIndex];
            return ' ';
        }

        public static string Encode(int DB) {
            int milli = DateTime.Now.Second & 10;
            string encrypted = DB.ToString();
            //Shift Left
            for (int i=0;i<milli;i++){
                encrypted=encrypted.Substring(1)+encrypted[encrypted.Length-1];
            }
            return encrypted;
        }

*/

    }
}
