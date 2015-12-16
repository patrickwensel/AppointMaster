using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace dashboard {
    public class Global : System.Web.HttpApplication {

        protected void Application_Start(object sender, EventArgs e) {

        }

        protected void Session_Start(object sender, EventArgs e) {
            EO.Web.Runtime.AddLicense(
    "5ZekscufWZfA8g/jWev9ARC8W8v29hDVotz7s8v1nun3+hrtdpm7v9uhWabC" +
    "nrWfWZekscufWbPl9Q+frfD09uihhuzwBRTPmt7ps8v1nun3+hrtdpm7v9uh" +
    "WabCnrWfWZekscufWbPl9Q+frfD09uihfNjw9hnjmummsSHkq+rtABm8W66y" +
    "wc2faLWRm8ufWZekscufddjo9cvzsufpzs3CmuPw8wzipJmkBxDxrODz/+ih" +
    "cKW0s8uud4SOscufWZekscu7mtvosR/4qdzBs+zJes/ZARfumtvpA82fr9z2" +
    "BBTup7SmyNmvW5ezz7iJWZekscufWZfA8g/jWev9ARC8W7vt8hfuoJmkBxDx" +
    "rODz/+ihcKW0s8uud4SOscufWZekscu7mtvosR/4qdzBs/7vpeD4BRDxW5f6" +
    "9h3youbyzs22Z6emsdq9RoGkscufWZeksefgndukBSTvnrSm3gzypNzo1g/o" +
    "rZmkBxDxrODz/+ihcKW0s8uud4SOscufWZekscu7mtvosR/4qdzBs/Lxotum" +
    "sSHkq+rtABm8W66ywc2faLWRm8ufWZekscufddjo9cvzsufpzs3CqOPzA/vo" +
    "nOLpA82fr9z2BBTup7SmyNmvW5ezz7iJWZekscufWZfA8g/jWev9ARC8W8r0" +
    "9hfrfN/p9Bbkq5mkBxDxrODz/+ihcKW0s8uud4SOscufWZekscu7mtvosR/4" +
    "qdzBs/DjouvzA82fr9z2BBTup7SmyNmvW5ezz7iJWZekscufWZfA8g/jWev9" +
    "ARC8W8Dx8hLkk+bz/s2fr9z2BBTup7SmyNmvW5ezz7iJWZekscufWZfA8g/j" +
    "Wev9ARC8W7vzCBnrqNjo9h2hWe3pAx7oqOXBs+KtaZmkwOmMQ5ekscufWZek" +
    "zQzjnZf4ChvkdpnK/Rrgrdz2s8v1nun3+hrtdpm7v9uhWabCnrWfWZekzdrg" +
    "pePzCOmMQ5ekscu7rODr/wzzrunpzxSqm+bI4ADjptHdCQTBe9rZ8hbNcLjB" +
    "zueurODr/wzzrunpz7iJdabw+g7kp+rpz7iJdePt9BDtrNzCnrWfWZekzRfo" +
    "nNzyBBDInbW2yOCybqi6xOCydabw+g7kp+rp2g+9RoGkscufdePt9BDtrNzp" +
    "z+eupeDn9hnyntzCnrWfWZekzQzrpeb7z7iJWZekscufWZfA8g/jWev9ARC8" +
    "W8Tp/yChWe3pAx7oqOXBs+KtaZmkwOmMQ5ekscufWZekzQzjnZf4ChvkdpnX" +
    "/RTjnsTp/yChWe3pAx7oqOXBs+KtaZmkwOmMQ5ekscufWZekzQzjnZf4Chvk" +
    "dpnY8g3SrentAc2fr9z2BBTup7SmyNmvW5ezz7iJWQ==");


        }

        protected void Application_BeginRequest(object sender, EventArgs e) {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) {

        }

        protected void Application_Error(object sender, EventArgs e) {

        }

        protected void Session_End(object sender, EventArgs e) {

        }

        protected void Application_End(object sender, EventArgs e) {

        }
    }
}