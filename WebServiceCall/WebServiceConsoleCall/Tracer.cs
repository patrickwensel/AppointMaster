using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace WebServiceConsoleCall {
    public class Tracer {

        static ArrayList list = null;

        public static void AddWord(string w) {
            if (list == null)
                list = new ArrayList();
            if (list.IndexOf(w) < 0)
                list.Add(w);
        }

        

    }
}
