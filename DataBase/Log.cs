using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBase {
    
    /// <summary>
    /// A class to log events for trace purposes
    /// </summary>
    public class Log {

        public enum Application{
            DashBoard,
            AccountSync,
            CDRChecking,
            MatchingAccount,
            MatchingALL
        }


        public string filename = "";
        private int _index = 0;
        string ident = "";

        public void push() {
            ident += "  ";
        }
        public void pull() {
            if (ident.Length > 1)
                ident = ident.Substring(2);
            else
                ident = "";
        }

        private DateTime lastWrite = DateTime.Now;

        public void WriteLine(string message) {
            Console.WriteLine(message);
            try {
                System.TimeSpan ts = DateTime.Now.Subtract(lastWrite);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(filename, true);
                string header=DateTime.Now.Minute.ToString().PadLeft(2,'0') + ":" + DateTime.Now.Second.ToString().PadLeft(2,'0');
                if (ts.TotalSeconds > 10)
                    header += "** " + ts.TotalSeconds.ToString().PadLeft(5)+"s ";
                else 
                    header += " - ";

                sw.WriteLine(header + ident + message);

                sw.Close();
                lastLineWasNotReturn = true;
                lastWrite = DateTime.Now;
            } catch (Exception ex) {
            }
        }

        bool lastLineWasNotReturn = true;

        public void Write(string message) {
            try {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(filename, true);
                if (lastLineWasNotReturn)
                    sw.WriteLine(DateTime.Now.ToShortTimeString().PadLeft(15) + ": " + message);
                else {
                    sw.WriteLine(message);
                    lastLineWasNotReturn = false;
                }

                sw.Close();
            } catch (Exception ex) {
            }
        }

        private string logDirectory() {
            return "c:\\temp\\LpiLogs";
        }

        public Log(Application appli) {
            if (appli == Application.DashBoard)
                filename = logDirectory()+"\\DashBoard_" + DateTime.Today.Year.ToString() + "_" + DateTime.Today.Month.ToString() + "_" + DateTime.Today.Day.ToString() + ".txt";
            else {
                filename = logDirectory()+"\\"+appli.ToString()+ "_" + DateTime.Today.Year.ToString() + "_" + DateTime.Today.Month.ToString() + "_" + DateTime.Today.Day.ToString() + ".txt";
                while (System.IO.File.Exists(filename)) {
                    _index++;
                    filename = logDirectory() + "\\" + appli.ToString() + "_" + DateTime.Today.Year.ToString() + "_" + DateTime.Today.Month.ToString() + "_" + DateTime.Today.Day.ToString() + "_" + _index.ToString() + ".txt";
                    if (_index > 500) {
                        throw new Exception("Log constructor cannot find a file after 500 iterations on " + filename);
                    }
                }
                System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
                sw.WriteLine("***************************************************************************************************");
                sw.WriteLine(appli.ToString()+ " Log Created "+DateTime.Now.ToString());
                sw.WriteLine("***************************************************************************************************");

                sw.Close();


            }
        }



    }
}
