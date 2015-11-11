using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web.Mail;

namespace LPI.DataBase {
    
    public class Email {

        private int DB = 0;
        private string errorMessage="";
        public string content;

        public string error {
            get {
                return errorMessage;
            }
        }

        
        public bool sendTo(string to, string toName, string subject) {
            return sendTo(Util.GetConfigValue("FROM_NOTIFICATION"),Util.GetConfigValue("LPI Administration"), to, toName, subject, true, true,"");
        }


        public bool sendTo(string from, string fromName, string to, string toName, string subject, bool html,string attachedFile) {
            return sendTo(from, fromName, to, toName, subject, html, true,attachedFile);
        }

        public bool sendTo(string from, string fromName, string to, string toName, string subject, bool html, bool saveEmail, string attachedFile) {
            string SmtpServer = Util.GetConfigValue("SMTP_SERVER"); // put smtp server you will use here 
            string userId = Util.GetConfigValue("SMTP_SERVER_USERID");//cgissinger
            string password = Util.GetConfigValue("SMTP_SERVER_PASSWORD");//kiki2008
            string smtp_port = Util.GetConfigValue("SMTP_PORT");//587
            int iPort = Util.safeIntParse(smtp_port);
            bool smtp_ssl = (Util.GetConfigValue("SMTP_SSL") == "1");//587

            return sendTo(from, fromName, to, toName, subject, html, saveEmail, SmtpServer, userId, password, iPort, smtp_ssl,attachedFile);
        }

        //This THE method to use to send email out. This method checks if the server is authorized to send email etc...
        public bool sendTo(string from, string fromName, string to, string toName, string subject, bool html, bool saveEmail,
                    string smtp_server,
                    string smtp_userId,
                    string smtp_password,
                    int smtp_port,
                    bool smtp_ssl, string attachedFile) {

            if (Util.isValidEmailAddress(to)) {
                
                bool status = true;
                errorMessage = "";
                string[] possibleRecipients = {   
											  "cgissinger@packonline.com",//A
											  "cgissinger@appointmaster.com",//A
											  "rob@appointmaster.com",//A
											  "jc@appointmaster.com",//A
											  "brian@appointmaster.com",//A
											  "aaron.drew@localpatient.com",//A
											  "info@packonline.com",//B
											  "7036065471@txt.att.net",
											  "7035074724@txt.att.net",
											  "7036061612@txt.att.net",
											  "7039275978@txt.att.net",
											  "7035974029@messaging.sprintpcs.com",
											  "alice.gissinger@gmail.com",
											  "doclebeau@msn.com"
											  //B
			};
                //"salebeau@drlebeau.com",//B
                //"steve@drlebeau.com",//B
                //"doclebeau@msn.com",//B
                //"salebeau@verizon.net",

                //cn.trace.WriteLine(" Email.sendTo FRM:"+from+" TO:"+to+" subj:"+subject+" nbcars="+curContent.Length.ToString());

                string problem = "";
                string optionEmail = Util.GetConfigValue("EMAIL_SENDING");
                string savedTo = to;
                try {
                    bool possible = false;

                    if (optionEmail == "NONE") {
                        possible = false;
                        status = false;
                    } else if (optionEmail == "ALL") {
                        possible = true;
                    } else if (optionEmail == "LIMITED") {
                        for (int i = 0; i < possibleRecipients.Length; i++) {
                            if (to.ToUpper() == possibleRecipients[i].ToUpper()) {
                                possible = true;
                                break;
                            }
                        }
                        if (!possible)
                            problem = to + " not authorized";
                    }
                    if (possible) {
                        //System.Net.NetworkCredential nc=new System.Net.NetworkCredential();
                        // create mail message object
                        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                        //MailMessage mail = new MailMessage();
                        mail.From = new System.Net.Mail.MailAddress(from, fromName);
                        mail.To.Add(new System.Net.Mail.MailAddress(to, toName));

                        //mail.Attachments.Add(new System.Net.Mail.Attachment(

                        mail.Subject = subject;        // put subject here	
                        mail.Body = this.content;           // put body of email here
                        mail.IsBodyHtml = true;

                        System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient(smtp_server, smtp_port);
                        smtpClient.Host = smtp_server;
                        smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                        smtpClient.UseDefaultCredentials = true;

                        string userId = smtp_userId;	//Util.GetConfigValue("SMTP_SERVER_USERID");//cgissinger
                        string password = smtp_password;	//	Util.GetConfigValue("SMTP_SERVER_PASSWORD");//kiki2008


                        if (userId != "") {
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.Credentials = new System.Net.NetworkCredential(userId, password);
                        }
                        

                        if (smtp_port > 0)
                            smtpClient.Port = smtp_port;
                        if (smtp_ssl)
                            smtpClient.EnableSsl = true;

                        //************************************
                        smtpClient.Send(mail);
                        //************************************

                    } else {
                        problem="not sent because out of the possible Recipients";
                        status = false;
                    }
                } catch (Exception ex) {
                    //cn.trace.WriteLine(" Email.sendTo FRM:"+from+" TO:"+to+" subj:"+subject+" nbcars="+curContent.Length.ToString()+" exception:"+ex.Message);
                    problem = ex.Message;
                    int level = 0;
                    while (ex.InnerException != null) {
                        problem += "\r\nDetail(" + level.ToString() + "): " + ex.InnerException.ToString();
                        ex = ex.InnerException;
                        level++;
                    }
                    status = false;
                }
                if (saveEmail) {
                    //Email.traceEmailOnFile(curAccount.DataBaseNumber, html, SmtpMail.SmtpServer, StmpUserIdentified, from, savedTo, curContent, subject, optionEmail, problem);
                }
                errorMessage = problem;
                return status;
            } else {
                errorMessage = "Invalid Email";
                return false;
            }
                
        }//sendTo




        public Email(int DB) {
            this.DB = DB;
        }


    }
}
