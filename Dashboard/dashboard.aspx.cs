using System;
using System.Collections;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Hosting;
using DataBase;

namespace dashboard {
    public partial class dashboard : System.Web.UI.Page {

        private string trLineForData(string header, string value) {
            if (string.IsNullOrEmpty(value))
                return "<tr><td  class=\"header\">" + header + "</td><td></td></tr>";
            else
                return "<tr><td  class=\"header\">" + header + "</td><td>" + value + "</td></tr>";
        }

        //protected string getLeadZoomTable(SqlConnection cn, int DB, int campId, string number, DateTime date, bool source) {
        protected string getLeadZoomTable(SqlConnection cn, int DB, int leadId, bool source) {
            string title = "Lead Detail";
            
            if (source)
                title = "Lead Source";

            string leadDetailContent = @"<tr> 
    <td  class=""header"">ID</td>
    <td>#ID</td>    
</tr><tr> 
    <td  class=""header"">Submitted</td>
    <td>#SUBMITTED</td>    
</tr>
<tr> 
    <td  class=""header"">First Name</td>
    <td>#FIRSTNAME</td>    
</tr>
<tr> 
    <td  class=""header"">Last Name</td>
    <td>#LASTNAME</td>    
</tr>
<tr> 
    <td  class=""header"">Patient Email</td>
    <td>#FEMAIL</td>    
</tr>
<tr> 
    <td  class=""header"">Patient Phone</td>
    <td>#PHONE</td>    
</tr>
<tr> 
    <td  class=""header"">Dental Need</td>
    <td>#DENTALNEED</td>    
</tr>
<tr> 
    <td  class=""header"">I'm a</td>
    <td>#DENTALCAREFOR</td>    
</tr>
<tr> 
    <td  class=""header"">Insurance Plan</td>
    <td>#INSURANCEPLAN</td>    
</tr>
<tr> 
    <td  class=""header"">Preferred Time</td>
    <td>#PREFERREDTIME</td>    
</tr>
";  



            string s = @"<div class=""popuptitle"">@TITLE<div style=""float:right;""><a style=""font-size:20px;font-weight:bold;text-decoration:none;color:white"" href=""#"" onclick=""closeApptDetailDetail();"">X</a></div></div>
<div class=""popupscroll""><table style=""height:100%""> 
@CONTENT
</table>
</div>
";



            s = s.Replace("@TITLE", title);
            if (!source)
                s = s.Replace("@CONTENT", leadDetailContent);

            try {
                Lead lead = new Lead(cn);
                //if (lead.DBGet(DB, campId, number, date, false)) {
                if (lead.DBGet(leadId)){
                    if (source) {
                        string googleTrackingLine = trLineForData("Source", lead.source);
                        googleTrackingLine += trLineForData("Campaign Id", lead.campaignID);
                        googleTrackingLine += trLineForData("Traffic", lead.medium);
                        googleTrackingLine += trLineForData("Keyword", lead.term);
                        googleTrackingLine += trLineForData("Content", lead.content);
                        googleTrackingLine += trLineForData("Campaign", lead.campaign);
                        googleTrackingLine += trLineForData("Referred By", lead.referred_by);
                        s = s.Replace("@CONTENT", googleTrackingLine);
                    } else {
                        s = s.Replace("#ID", lead.ID.ToString());
                        s = s.Replace("#CAMPAIGN", lead.campaignID);
                        s = s.Replace("#SUBMITTED", lead.timeStamp.ToString());
                        s = s.Replace("#FIRSTNAME", lead.firstName);
                        s = s.Replace("#LASTNAME", lead.lastName);
                        s = s.Replace("#FEMAIL", lead.email);
                        s = s.Replace("#PHONE", Util.phoneForDisplay(lead.inComingNumber));
                        s = s.Replace("#DENTALNEED", lead.DentalNeedToString());
                        s = s.Replace("#DENTALCAREFOR", lead.DentalCareIsForToString());
                        s = s.Replace("#INSURANCEPLAN", lead.InsurancePlanBudgetToString());
                        s = s.Replace("#PREFERREDTIME", lead.PreferredAppointmentTimeToString());
                    }
                }else
                    //s="Cannot Read Campaign "+DB.ToString()+" "+campId.ToString()+" "+number+" "+date.ToShortDateString();
                    s = "Cannot Read Campaign " + DB.ToString() + " " + leadId.ToString();
            } catch (Exception ex) {
                s = "getLeadZoomTable exception:"+ex.Message;
            }
            //s += "<br>";
            //s += "<INPUT type=button value=Close id=pbSave onclick=\"document.getElementById('popupApptDetail').style.display = 'none';\" NAME='pbSave' style='TEXT-ALIGN:center;margin-top:10px;'\">";


            return s;//            "LEAD DB=" + DB.ToString() + " camp=" + campId.ToString() + " number=" + number.ToString() + " date=" + date.ToShortDateString();
        }//getLeadZoomTable


        protected string getApptZoomTable(SqlConnection cn, int DB, int patId, DateTime from, DateTime to) {
            try {

                string WC = "select * from dentalprocedure where DB=" + DB.ToString() + " and patientid=" + patId.ToString();
                // job to put back to 
                // WC += " and dateTime>='" + from.ToShortDateString() + "'  and  dateTime<'" + to.ToShortDateString() + "'";   

                WC += " order by treatmentplan desc, dateTime desc";
                SqlCommand command = new SqlCommand(WC, cn);
                command.CommandTimeout = 300;
                SqlDataReader DbReader = command.ExecuteReader();

                string code = "";
                DateTime date;
                decimal amount;


                string newS = @"<div class=""popuptitle"">#TITLE<div style=""float:right;""><a style=""font-size:20px;font-weight:bold;text-decoration:none;color:white"" href=""#"" onclick=""closeApptDetailDetail();"">X</a></div></div>
<div class=""popupscroll""><div style=""text-align:center"">
			<div class=""small-light-box"">
				<div class=""small-top"">FROM</div>
				<div class=""small-bottom"">#DATE1</div>
			</div>
			<div class=""small-light-box"">
				<div class=""small-top"">TO</div>
				<div class=""small-bottom"">#DATE2</div>
			</div>
			<div class=""small-dark-box"">
				<div class=""small-top"">TOTAL</div>
				<div class=""small-bottom"">#TOTAL</div>
			</div>
		</div>

		<table class=""practiceroi-table practiceroi-plan-table"">
			<tr class=""practiceroi-table-header"" stlye=""font-weight:bold;"">
				<td>Code</td>
				<td>Name</td>
				<td class=""center"">Date</td>
				<td class=""right"">Amount</td>
			</tr>
            @BLOCKLINE_TP
			<tr>
				<td colspan=""4"" class=""nopadding right""><div class=""separator"">Subtotal treatment plan: @SUBTOTAL_TP</div></td>
			</tr>
            @BLOCKLINE_PROD
			<tr>
				<td colspan=""4"" class=""nopadding right""><div class=""separator"">Subtotal Revenue Tracked: @SUBTOTAL_PROD</div></td>
			</tr>
		</table>
</div>
";


                string cssClass = "";
                decimal totalAmount = 0;
                int lineNbr = 0;
                DisplayMode displayMode = DisplayModeSelected(cn, DB);
                bool isTp = false;
                decimal SubTotal = 0;
                decimal total = 0;
                int lastTp = -1;

                string s = "";
                while (DbReader.Read()) {
                    code = DbReader.GetString(2);
                    date = DbReader.GetDateTime(4);
                    amount = DbReader.GetDecimal(5);
                    isTp = DbReader.GetInt16(7)==1;
                    if (lastTp >= 0) {
                        int newiTp =0;
                        if (isTp)
                            newiTp = 1;
                        else
                            newiTp = 0;
                        if (newiTp != lastTp) {
                            if (lastTp == 1) {
                                newS = newS.Replace("@BLOCKLINE_TP", s);
                                newS = newS.Replace("@SUBTOTAL_TP", SubTotal.ToString("C0"));
                                s = "";
                            } else {
                                newS = newS.Replace("@BLOCKLINE_PROD", s);
                                newS = newS.Replace("@SUBTOTAL_PROD", SubTotal.ToString("C0"));
                                s = "";
                            }
                            SubTotal = 0;
                        }
                    }
                    SubTotal += amount;

                    s += "<tr>";
                    s += "<td>";
                    if (isTp) 
                        s += "TP-"+code;
                    else
                        s += code;
                    s += "</td>";

                    s += "<td >";
                    s += "-";//ADA code problem
                    s += "</td>";

                    s += "<td class=\"center\">";
                    s += date.ToShortDateString();
                    s += "</td>";



                    s += "<td class=\"right\">";
                    if (displayMode== DisplayMode.Combine)
                        s += "<b>";
                    else if (displayMode == DisplayMode.Production) {
                        if (!isTp) s += "<b>";
                    } else {
                        if (isTp) s += "<b>";
                    }



                    
                    s += amount.ToString("C0");
                    totalAmount += amount;
                    
                    s += "</b>";//anyway

                    s += "</td>";

                    s += "</tr>";
                    if (isTp)
                        lastTp = 1;
                    else
                        lastTp = 0;

                    lineNbr++;
                }
                if (lastTp == 1) {
                    newS = newS.Replace("@BLOCKLINE_TP", s);
                    newS = newS.Replace("@SUBTOTAL_TP", SubTotal.ToString("C0"));
                } else {
                    newS = newS.Replace("@BLOCKLINE_PROD", s);
                    newS = newS.Replace("@SUBTOTAL_PROD", SubTotal.ToString("C0"));
                }

                 newS = newS.Replace("@BLOCKLINE_TP", "");
                newS = newS.Replace("@SUBTOTAL_TP", "");
                                newS = newS.Replace("@BLOCKLINE_PROD", "");
                                newS = newS.Replace("@SUBTOTAL_PROD", "");
                
                DbReader.Close();
                DbReader.Dispose();

                //newS += "<INPUT type=button value=Close id=pbSave onclick=\"document.getElementById('popupApptDetail').style.display = 'none';\" NAME='pbSave' style='TEXT-ALIGN:center'\">";


                newS = newS.Replace("#DATE1", from.ToShortDateString());
                newS = newS.Replace("#DATE2", to.ToShortDateString());
                newS = newS.Replace("#TOTAL", totalAmount.ToString("C0"));

                WC = "select firstname,name from patient where DB=" + DB.ToString() + " and AMid=" + patId.ToString();
                command = new SqlCommand(WC, cn);
                command.CommandTimeout = 300;
                DbReader = command.ExecuteReader();
                if (DbReader.Read()) {
                    string fname = DbReader.GetString(0);
                    string name = DbReader.GetString(1);
                    newS = newS.Replace("#TITLE", "Procedures Posted to " + fname + " " + name);
                    //s = s.Replace("#TOTAL", totalAmount.ToString("N"));
                }

                return newS;
            } catch (Exception ex) {
                return "getApptZoomTable Exception " + ex.Message;
            }
        }

        private string displayedPhone(string number) {
            string CleanPhone = "";
            if (number.StartsWith("1"))
                number = number.Remove(0, 1);
            for (int i = 0; i < number.Length; i++) {
                if ("0123456789".IndexOf(number[i]) >= 0)
                    CleanPhone += number[i];
            }
            if (CleanPhone.Length == 10)
                return "(" + CleanPhone.Substring(0, 3) + ") " + CleanPhone.Substring(3, 3) + " " + CleanPhone.Substring(6);
            else
                return CleanPhone;
        }

        private static string getSoundLink(string root_id,string IDinPage, string URL) {

            //URL = "http://robtowns.com/music/blind_willie.mp3";

            string s = @"<img src=""images/rewind.png"" id=""restart_01"" onclick=""restart();"" style=""display:none"" title=""Restart""/>
<img src=""images/listen.gif"" id=""play_01"" onclick=""play('01','@URL'); return false;""  title=""Play"" />
<img src=""images/stop.png"" id=""pause_01"" onclick=""pause();"" style=""display:none""  title=""Pause""/>";

            s = s.Replace("01", root_id+"_"+IDinPage);
            s = s.Replace("@URL", URL);

            

            return s;
        }

        protected string getCampaignDetailLinesForWhereClause(SqlConnection cn, string WC, bool withPatient, bool withTreatment, bool displaySound, int campId,int DB, string rooIdForSoundPlayer,ref ArrayList matchedPatient) {
            try {
                bool displayDuplicate = SysOption.GetValue(cn, DB, "DASHBOARD_DISPLAY_DUPLICATE", false);
                
                SqlCommand command = new SqlCommand(WC, cn);
                command.CommandTimeout = 300;
                SqlDataReader DbReader = command.ExecuteReader();

                string number = "";
                DateTime ts;
                string patId = "";
                string patName = "";
                DateTime apptDateTime;
                decimal amount = 0;
                System.TimeSpan duration;
                string URL = "";
                string s = "";
                string cssClass = "";
                string formCaptureLeadZoomLink = "";
                int lineNbr = 0;
                bool duplicate = false;
                string dbPhone = "";
                string newLine = "";
                string primaryPhone = "";
                string sourceParam = "";
                string LeadId = "";
                while (DbReader.Read()) {
                    duplicate = false;
                    newLine = "";
                    
                    dbPhone = DbReader.GetString(2);
                    number = displayedPhone(dbPhone);
                    ts = DbReader.GetDateTime(3);
                    duration = new TimeSpan(0, 0, DbReader.GetInt16(4));
                    URL = DbReader.GetString(5);
                    if (!Uri.IsWellFormedUriString(URL, UriKind.Absolute))
                        URL = "";

                    if (withPatient) {
                        patId = DbReader.GetString(6);
                        patName = DbReader.GetString(7) + " " + DbReader.GetString(8);
                        primaryPhone = DbReader.GetString(9);
                        apptDateTime = DateTime.MinValue;

                        if (withTreatment)
                            if (!DbReader.IsDBNull(10))
                                amount = DbReader.GetDecimal(10);
                    } else {
                        primaryPhone = DbReader.GetString(6);
                    }
                    if (number.Trim() == "") {
                        if (dbPhone.Trim() == "") {
                            number = displayedPhone(primaryPhone);
                            if (number == "")
                                number = primaryPhone;
                        } else
                            number = dbPhone;
                    }
                    LeadId = DbReader.GetInt32(0).ToString();
                    
                    //formCaptureLeadZoomLink = "LeadDetail=1&source=1&DB=" + DB.ToString() + "&campId=" + campId.ToString() + "&number=" + dbPhone + "&date=" + ts.Month.ToString() + "-" + ts.Day.ToString() + "-" + ts.Year.ToString();
                    formCaptureLeadZoomLink = "LeadDetail=1&source=1&DB=" + DB.ToString() + "&id=" + LeadId + "&campId=" + campId.ToString() + "&number=" + dbPhone + "&date=" + ts.Month.ToString() + "-" + ts.Day.ToString() + "-" + ts.Year.ToString();

                    newLine += "<tr>";

                    newLine += "<td class=\"col1 first\">";
                    newLine += number;
                    newLine += "</td>";

                    //newLine += "<td class=\"col2 center nopadding\">";
                    //newLine += "<a href=\"javascript:openFCLeadWindow('" + formCaptureLeadZoomLink + "');\" title='Check lead source " + formCaptureLeadZoomLink + " ' ><img src='images/icon-source.png' border='0' /></a>";
                    //newLine += "</td>";
                    formCaptureLeadZoomLink = formCaptureLeadZoomLink.Replace("&source=1", "");


                    newLine += "<td class=\"col3 center\">";
                    newLine += ts.ToString("MM/dd/yy");
                    newLine += "</td>";

                    newLine += "<td class=\"col4 center\">";
                    newLine += ts.ToShortTimeString();
                    newLine += "</td>";

                    newLine += "<td class=\"col5 center\">";
                    newLine += duration.ToString().Replace("00:0", "0");
                    newLine += "</td>";


                    newLine += "<td class=\"col6 center nopadding\">";
                    if (URL != "") {
                        if (URL.EndsWith(")")){
                            int d = 78 / 23;
                        }
                        //newLine += "<a href=\"javascript:openSoundWindow('" + URL + "');\" title='Listen to the call " + URL + " ' > <img src='images/rewind.png' border='0' /> <img src='images/listen.gif' border='0' /></a>";
                        if (displaySound)
                            newLine += getSoundLink(rooIdForSoundPlayer, lineNbr.ToString(), URL);
                        else
                            newLine += "<div style=\"width:20px\">&nbsp;</div>";
                    } else {
                        newLine += "<a href=\"javascript:openFCLeadWindow('" + formCaptureLeadZoomLink + "');\" title='Check lead detail " + formCaptureLeadZoomLink + " ' ><img src='images/icon-detail.png' border='0' /></a>";
                    }

                    //newLine += "<a href=\"" + URL + "\" title=\"download control\">.</a>";



                    newLine += "</td>";

                    //if (!DbReader.IsDBNull(9)) {
                    if (withPatient ){
                        
                        if (matchedPatient.IndexOf(patId) < 0) {
                            duplicate = false;
                            matchedPatient.Add(patId);
                        }else
                            duplicate = true;


                        //newLine += "<td class=\"col7\" title='" + patName + "' onclick=\"alert('" + patName + "')\">";
                        newLine += "<td class=\"col7\" title='" + patName + "' \">";
                        newLine += "Patient #" + patId;//patName;
                        newLine += "</td>";

                        if (duplicate) {
                            newLine += "<td class=\"col8 right\" colspan=2><font color=#AA6666>- <i>duplicate</i> -</font></td>";
                        } else {
                            if (withTreatment) {
                                newLine += "<td class=\"col8 right\" colspan=2>";
                                newLine += "<a href='javascript:openApptDetailDetail(" + patId + ")'><b>" + amount.ToString("C0") + "</b></a>";
                                newLine += "</td>";
                            } else {
                                newLine += "<td class=\"col8\"  colspan=2><font color=#999999>- no treatments -</font></td>";
                            }
                        }
                    } else {
                        newLine += "<td colspan=2><font color=#999999 nowrap>- no appointment  yet -</font></td>";
                        //newLine += "<td class=\"col9 right last\"></td>";
                    }
                    newLine += "</tr>";

                    if (duplicate)
                        if (!displayDuplicate) {
                            newLine = "";
                            lineNbr = lineNbr - 1;
                        }
                    
                    s += newLine;

                    lineNbr++;
                }
                DbReader.Close();
                DbReader.Dispose();
                return s;

            } catch (Exception ex) {
                return "getCampaignDetailLinesForWhereClause Exception " + ex.Message+" with WC="+WC;
            }
        }//getCampaignDetailLinesForWhereClause

        class tableLine {
            public string patientID;
            public string name;
            public string phone;
            public DateTime date;
            public TimeSpan duration;
            public string voiceURL;
            public decimal plan;
            public decimal production;
            public int DB;
            public int campId;
            public int LeadId;

            public void CopyInto(ref tableLine instance){
                instance.patientID = this.patientID;
                instance.name = this.name;
                instance.phone = this.phone;
                instance.date = this.date;
                instance.duration = this.duration;
                instance.voiceURL = this.voiceURL;
                if (this.plan>0)
                    instance.plan = this.plan;
                if (this.production>0)
                    instance.production = this.production;
                instance.LeadId = this.LeadId;
            }

            /// <summary>
            /// Return one line unless patientId is nll or empty OR if patientId already displayed
            /// </summary>
            /// <param name="number"></param>
            /// <param name="matchedPatient"></param>
            /// <returns></returns>
            public string getHtmlLine(bool displayDuplicate, bool displaySound, ref int number, ref ArrayList matchedPatient) {
                if ((this.patientID != "")&(this.patientID!=null)) {
                    bool display = false;
                    bool duplicate = false;
                    if (matchedPatient.IndexOf(this.patientID) < 0) {
                        duplicate = false;
                        matchedPatient.Add(this.patientID);
                    } else
                        duplicate = true;

                    if (duplicate) {
                        if (displayDuplicate) {
                            display = true;
                        }
                    } else
                        display = true;

                    if (display) {
                        number++;
                        string cssClass = "";
                        if (number % 2 == 0)
                            cssClass = "";
                        else
                            cssClass = "class='alternate'";

                        string formCaptureLeadZoomLink = "LeadDetail=1&source=1&DB=" + this.DB.ToString() + "&ID=" + this.LeadId + "&campId=" + this.campId.ToString() + "&number=" + this.phone + "&date=" + this.date.Month.ToString() + "-" + this.date.Day.ToString() + "-" + this.date.Year.ToString();

                        System.IO.StringWriter newLine = new System.IO.StringWriter();
                        newLine.WriteLine("<tr>");

                        newLine.WriteLine("<td class=\"col1 first\">");
                        newLine.WriteLine(this.phone);
                        newLine.WriteLine("</td>");

                        formCaptureLeadZoomLink = formCaptureLeadZoomLink.Replace("&source=1", "");

                        newLine.WriteLine("<td class=\"col3 center\">");
                        newLine.WriteLine(this.date.ToString("MM/dd/yy"));
                        newLine.WriteLine("</td>");

                        newLine.WriteLine("<td class=\"col4 center\">");
                        newLine.WriteLine(this.date.ToShortTimeString());
                        newLine.WriteLine("</td>");

                        newLine.WriteLine("<td class=\"col5 center\">");
                        newLine.WriteLine(this.duration.ToString().Replace("00:0", "0"));
                        newLine.WriteLine("</td>");

                        newLine.WriteLine("<td class=\"col6 center nopadding\">");

                        if (this.voiceURL != "") {
                            if (displaySound) {
                                string s23 = getSoundLink("B", number.ToString(), this.voiceURL);
                                newLine.WriteLine(s23);
                            } else {
                                newLine.WriteLine("<div style=\"width:20px\">&nbsp;</div>");
                            }
                        } else {
                            newLine.WriteLine("<a href=\"javascript:openFCLeadWindow('" + formCaptureLeadZoomLink + "');\" title='Check lead detail " + formCaptureLeadZoomLink + " ' ><img src='images/icon-detail.png' border='0' /></a>");
                        }
                        newLine.WriteLine("</td>");


                        newLine.WriteLine("<td class=\"col7\" title='" + this.name + "' \">");
                        newLine.WriteLine("Patient #" + this.patientID);//patName);
                        newLine.WriteLine("</td>");

                        newLine.WriteLine("<td class=\"col8 right\">");
                        if (this.plan > 0)
                            newLine.WriteLine("<a href='javascript:openApptDetailDetail(" + this.patientID + ")'><b>" + this.plan.ToString("C0") + "</b></a>");
                        newLine.WriteLine("</td>");

                        newLine.WriteLine("<td class=\"col9 right last\">");
                        if (this.production > 0)
                            newLine.WriteLine("<a href='javascript:openApptDetailDetail(" + this.patientID + ")'><b>" + this.production.ToString("C0") + "</b></a>");
                        newLine.WriteLine("</td>");
                        newLine.WriteLine("</tr>");
                        newLine.Close();
                        return newLine.ToString();
                    } else
                        return "";
                } else
                    return "";
            }

            public void completeAmountFrom(tableLine tl) {
                if (tl.plan > 0)
                    this.plan = tl.plan;
                if (tl.production > 0)
                    this.production = tl.production;
            }

            public void clear() {
                this.patientID = "";
                this.name = "";
                this.phone = "";
                this.duration = TimeSpan.MinValue; 
                this.voiceURL = "";
                this.plan = 0;
                this.production = 0;
            }

            public tableLine(int DB, int campId) {
                this.DB = DB;
                this.campId = campId;
            }
        }//tableLine 

        private tableLine getLineFromReader(SqlDataReader DbReader, int DB, int campId) {
            try {
                tableLine line = new tableLine(DB,campId);
                string dbPhone = DbReader.GetString(0);
                string number = displayedPhone(dbPhone);
                line.date = DbReader.GetDateTime(1);
                line.duration = new TimeSpan(0, 0, DbReader.GetInt16(2));
                line.voiceURL = DbReader.GetString(3);
                line.LeadId = DbReader.GetInt32(10);
                if (!Uri.IsWellFormedUriString(line.voiceURL, UriKind.Absolute))
                    line.voiceURL = "";
                line.patientID = DbReader.GetString(4);

                line.name = DbReader.GetString(5) + " " + DbReader.GetString(6);
                string primaryPhone = DbReader.GetString(7);
                decimal amount = 0;
                if (!DbReader.IsDBNull(8))
                    amount = DbReader.GetDecimal(8);

                //Debug.WriteLine(patId + " " + amount.ToString("C0") + " " + ts.ToShortDateString());
                bool isTreatmentPLan = DbReader.GetInt16(9) == 1;
                if (isTreatmentPLan)
                    line.plan = amount;
                else
                    line.production = amount;
                
                line.phone = number;
                if (number.Trim() == "") {
                    if (dbPhone.Trim() == "") {
                        line.phone = displayedPhone(primaryPhone);
                        if (line.phone == "")
                            line.phone = primaryPhone;
                    } else
                        line.phone = dbPhone;
                }
                

                return line;
            } catch (Exception ex) {
                throw new Exception("getLineFromReader Exception " + ex.Message);
            }
        }

        protected string getCampaignDetailLinesForTreatments(SqlConnection cn, int campId, int DB, DateTime from, DateTime to, ref ArrayList matchedPatient) {
            try {
                bool displayDuplicate = SysOption.GetValue(cn, DB, "DASHBOARD_DISPLAY_DUPLICATE", false);
                string WC = "usp_LeadsTpAndProduction " + DB.ToString() + "," + campId.ToString() + ",'" + from.ToShortDateString() + "','" + to.ToShortDateString() + "'";
                SqlCommand command = new SqlCommand(WC, cn);
                command.CommandTimeout = 300;
                SqlDataReader DbReader = command.ExecuteReader();
                tableLine lastRead = new tableLine(DB,campId);
                int lineNbr = 0;
                System.IO.StringWriter sw = new System.IO.StringWriter();
                bool duplicate = false;
                decimal totalCombineForDebug = 0;
                int forDebugLastLine = 0;
                bool displaySound = !SysOption.GetValue(cn, DB, "DO_NOT_SHOW_SOUND", false);
                while (DbReader.Read()) {
                    tableLine tLine = getLineFromReader(DbReader,DB,campId);
                    if (tLine.patientID != lastRead.patientID) {
                        forDebugLastLine = lineNbr;

                        sw.WriteLine(lastRead.getHtmlLine(displayDuplicate, displaySound, ref lineNbr,ref matchedPatient));
                        if (lineNbr != forDebugLastLine) {
                            totalCombineForDebug += lastRead.plan;
                            totalCombineForDebug += lastRead.production;
                        }
                        lastRead.clear();
                    } else {
                        lastRead.completeAmountFrom(tLine);
                    }
                    tLine.CopyInto(ref lastRead);
                }
                forDebugLastLine = lineNbr;
                sw.WriteLine(lastRead.getHtmlLine(displayDuplicate, displaySound ,ref lineNbr, ref matchedPatient));
                if (lineNbr != forDebugLastLine) {
                    totalCombineForDebug += lastRead.plan;
                    totalCombineForDebug += lastRead.production;
                }

                DbReader.Close();
                DbReader.Dispose();
                sw.Close();

                return sw.ToString();
            } catch (Exception ex) {
                return "getCampaignDetailLinesForTreatments Exception " + ex.Message;
            }
        }


        protected string getCampaignDetailTable(SqlConnection cn, int DB, int campId, DateTime from, DateTime to, DisplayMode displayMode) {
            try {
                string WC = "select name,phonenumber from campaign where db=" + DB.ToString() + " and ID=" + campId.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                command.CommandTimeout = 300;
                SqlDataReader DbReader = command.ExecuteReader();
                string campName = "";
                string campPhone = "";
                while (DbReader.Read()) {
                    campName = DbReader.GetString(0);
                    campPhone = DbReader.GetString(1);
                }
                bool onlineFormCaptureCampaign = (campPhone.ToUpper().StartsWith("FORMCAPTURE"));
                DbReader.Close();
                DbReader.Dispose();
                command.Dispose();

                
                string s = "";
                s += @"
<div id=""campaign-details"">
<div class=""practiceroi-hr""></div>
<div id=""campaign-details-title"">" + campName + @"</div>
<div class=""practiceroi-hr""></div>

<table class=""practiceroi-table"">
			<tr class=""practiceroi-table-header"">
				<td class=""col1Title first"">Phone Number</td>
				<td class=""col3 center"">Date</td>
				<td class=""col4 center"">Time</td>
				<td class=""col5 center"">Call Duration</td>
				<td class=""col6 center"">Detail</td>
				<td class=""col7"">Patient</td>
				<td class=""col8 right"">Planned</td>
				<td class=""col9Title right last"">Production</td>
			</tr>
		</table>
		<div id=""practiceroi-table-container"">
		<table class=""practiceroi-table"">
";

                if (onlineFormCaptureCampaign)
                    s = s.Replace("Listen", "Lead");
                bool displaySound = !SysOption.GetValue(cn, DB, "DO_NOT_SHOW_SOUND", false);

                ArrayList matchedPatient = new ArrayList();
                
                s += getCampaignDetailLinesForTreatments(cn, campId, DB, from,to,ref matchedPatient);

                WC = "usp_LeadsMatchNoTreatments " + DB.ToString() + "," + campId.ToString() + ",'" + from.ToShortDateString() + "','" + to.ToShortDateString() + "'";

                string tmpS = getCampaignDetailLinesForWhereClause(cn, WC, true, false, displaySound, campId, DB, "LMNT", ref matchedPatient);
                if (tmpS != "")
                    s += "<tr><td colspan=\"9\" class=\"nopadding\"><div class=\"separator\">Matches found without treatments posted yet</div></td></tr>";
                s += tmpS;

                WC = "usp_LeadsNoMatch " + DB.ToString() + "," + campId.ToString() + ",'" + from.ToShortDateString() + "','" + to.ToShortDateString() + "'";
                tmpS = getCampaignDetailLinesForWhereClause(cn, WC, false, false,displaySound, campId, DB,"LNM", ref matchedPatient);
                if (tmpS != "")
                    s += "<tr><td colspan=\"9\" class=\"nopadding\"><div class=\"separator\">Unconverted Opportunities</div></td></tr>";
                s += tmpS;

                s += "</table></div><div class=\"practiceroi-hr\"></div></div></div>";
                return s;
            } catch (Exception ex) {
                return "getCampaignDetailTable Exception " + ex.Message;
            }

        }//getCampaignDetailTable

        protected string getServerName() {
            try {
                if (Session["server"] == null) {
                    Session["server"] = Util.getServerName();
                }
                return (string)Session["server"];
            } catch (Exception ex) {
                throw new Exception("getServerName exception:"+ex.Message);
            }
        }


        private void deleteXMLFilesFor(int DB) {
            try {
                string path = HostingEnvironment.MapPath("~");
                string XMLFileName = "data_" + DB.ToString() + "_*.xml";
                string[] fileNames = System.IO.Directory.GetFiles(path, XMLFileName);
                foreach (string file in fileNames) {
                    if (System.IO.File.Exists(file))
                        System.IO.File.Delete(file);
                }

            } catch (Exception ex) {
                throw new Exception("deleteXMLFilesFor ex:" + ex.ToString());
            }
        }

        protected string getLeadTable(SqlConnection cn, int DB, DateTime from, DateTime to, bool genXML, string generationNumber, DisplayMode displayMode) {
            try {

                bool DisplayRoi = SysOption.GetValue(cn, DB, "DisplayRoi", false);
                int RoiPercentage = SysOption.GetValue(cn, DB, "RoiDisplay", 0);
                bool autoHide = SysOption.GetValue(cn, (int)Session["DB"], "AutoHide", true);

                System.IO.StreamWriter strmw = null;
                System.IO.StringWriter sw = null;// new System.IO.StringWriter(); 
                if (genXML) {
                    string path = HostingEnvironment.MapPath("~");
                    //string XMLFileName = path + "\\data_"+DB.ToString()+"_" + period.ToString() + ".xml";
                    string XMLFileName = path + "\\data_" + DB.ToString() + "_" + generationNumber + ".xml";

                    deleteXMLFilesFor(DB);

                    //sw = new System.IO.StreamWriter(XMLFileName);
                    sw = new System.IO.StringWriter();
                    strmw = new System.IO.StreamWriter(XMLFileName);
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    sw.WriteLine("<pie>");
                }

                string WC = "";
                
                ArrayList campaigns = new ArrayList();
                WC = "Select ID,name,budget from campaign where db=" + DB.ToString()+" and (visible=1 or visible is null)";//simple version

                SqlCommand command = new SqlCommand(WC, cn);
                command.CommandTimeout = 300;
                SqlDataReader DbReader = command.ExecuteReader();
                while (DbReader.Read()) {
                    Campaign c = new Campaign(cn);
                    c.DB = DB;
                    if (!DbReader.IsDBNull(0))
                        c.ID = DbReader.GetInt32(0);
                    if (!DbReader.IsDBNull(1))
                        c.name = DbReader.GetString(1);
                    if (!DbReader.IsDBNull(2))
                        c.budget = (double)DbReader.GetDecimal(2);
                    //if (!DbReader.IsDBNull(3)) c._nbLeads = DbReader.GetInt32(3);
                    campaigns.Add(c);
                }
                DbReader.Close();
                DbReader.Dispose();



                int campId = 0;
                string campName = "";
                int nbLeads = 0;
                int totalLeads = 0;
                int nbPatients = 0;
                int totalNbPatients = 0;
                double percent = 0;
                double slicePercent = 0;
                decimal amount = 0;
                decimal total = 0;
                decimal budget = 104;
                decimal totalBugdet = 0;
                //DisplayMode displayMode = DisplayModeSelected(cn, DB);

                

                string sHighlightTotals = @"<div id=""large-numbers"">
<div class=""large-dark"">
<div class=""large-top"">$@TOTAL_ROI</div>
<div class=""large-bottom"">RTITLE</div>
</div>
<div class=""large-light"">
<div class=""large-top"">@TOTAL_LEAD</div>
<div class=""large-bottom"">LEADS</div>
</div>
<div class=""large-light"">
<div class=""large-top"">@TOTAL_PATIENT</div>
<div class=""large-bottom"">NEW PATIENTS</div>
</div>
<div class=""large-light"">
<div class=""large-top"">@CONVERSION</div>
<div class=""large-bottom"">CONVERSION</div>
</div>
</div>
";



                string[] colWidth = new string[6] { "32", "175", "72", "76", "83", "92" };
                string[] colWidthInside = new string[6] { "32", "115", "72", "76", "83", "92" };
                
                string s = @"<div class=""Campaign-tables-wrapper""><table class=""practiceroi-summary-table"" >
<tbody>
<tr class=""practiceroi-summary-header"">
<td style=""width:@w0px;"" ></td>
<!-- no title, the column for the colored dot -->
<td style=""width:@w1px;"" >Campaign</td>
<td style=""width:@w2px;"" class=""center"">Leads</td>
<td style=""width:@w3px;"" class=""center"">Patients</td>
<td style=""width:@w4px;"" class=""center"">Conversion</td>
<td style=""width:@w5px;"" class=""right"">Tracked</td>@ROIHEADER
<td></td>
</tr>
</table>
<div class=""ScrollCampaigns"" id=""scrolling-campaign-div"">
<table class=""practiceroi-summary-table-inside"" id=""LeadTableTable"">
";

                if (displayMode == DisplayMode.Combine)
                    sHighlightTotals = sHighlightTotals.Replace("RTITLE", "Tracked");
                else if (displayMode == DisplayMode.Production) {
                    sHighlightTotals = sHighlightTotals.Replace("RTITLE", "Production");
                    s = s.Replace("Tracked", "PROD.");
                } else {
                    sHighlightTotals = sHighlightTotals.Replace("RTITLE", "Plan");
                    s = s.Replace("Tracked", "PLAN.");
                }



                
                if (DisplayRoi) {
                    colWidth = new string[7] { "22", "155", "72", "66", "63", "82","70" };
                    colWidthInside = new string[7] { "32", "105", "62", "66", "63", "82","70" };
                    s = s.Replace("@ROIHEADER", "<td style=\"width:@w6px; text-align:right\" >ROI</td>");
                    s = s.Replace("Tracked", "Comb.");
                    s = s.Replace("Conversion", "Conv.");
                    s = s.Replace("Patients", "# Pat");
                } else {
                    s = s.Replace("@ROIHEADER", "");
                }

                string cssClass = "";
                string pullOut = " pull_out=\"true\"";
                int lineNbr = 0;
                this.firstCampaign.Text = "";
                ArrayList totals = new ArrayList();
                string[] colors = new string[13] { "#4471a6", "#a84542", "#88a44c", "#71578f", "#3f96ae", "#d9823b", "#6AF9C4", "#4471a6", "#a84542", "#71578f", "#3f96ae", "#d9823b", "#6AF9C4" };
                bool display = true;    
                for (int i = 0; i < campaigns.Count; i++) {
                    Campaign campaign = (Campaign)campaigns[i];
                    campaign._nbLeads = campaign.leadCount("timestamp>='" + from.ToShortDateString() + "' and  timestamp<'" + to.ToShortDateString() + "'");
                    if (campaign._nbLeads > 0)
                        display = true;
                    else
                        display = !autoHide;
                    if (display) {

                        WC = "usp_OneCampaignAllResult  " + DB.ToString() + "," + campaign.ID.ToString() + ",'" + from.ToShortDateString() + "','" + to.ToShortDateString() + "'";


                        WC = "select count(distinct(patientID)) from lead where db=" + DB.ToString() + " and campaignId=" + campaign.ID.ToString();
                        WC += " and timestamp>='" + from.ToShortDateString() + "' and  timestamp<'" + to.ToShortDateString() + "' and original=0";
                        WC += " and patientID<>''";
                        command = new SqlCommand(WC, cn);
                        command.CommandTimeout = 300;
                        DbReader = command.ExecuteReader();
                        if (DbReader.Read()) {
                            if (DbReader.IsDBNull(0))
                                nbPatients = 0;
                            else
                                nbPatients = DbReader.GetInt32(0);
                        }
                        DbReader.Close();
                        totalNbPatients += nbPatients;



                        WC = "select count(distinct(lead.patientID)),sum(amount) from dentalprocedure join lead on dentalprocedure.patientId=lead.patientid";
                        WC += " and lead.db=" + DB.ToString() + " and dentalprocedure.db=" + DB.ToString();
                        if (displayMode == DisplayMode.Production)
                            WC += " and treatmentPlan=0";
                        else if (displayMode == DisplayMode.TreatmentPlan)
                            WC += " and treatmentPlan=1";
                        WC += " and timestamp>='" + from.ToShortDateString() + "' and  timestamp<'" + to.ToShortDateString() + "' and original=0";
                        WC += " and campaignId=" + campaign.ID.ToString();



                        command = new SqlCommand(WC, cn);
                        command.CommandTimeout = 300;
                        DbReader = command.ExecuteReader();
                        if (DbReader.Read()) {
                            nbLeads = campaign._nbLeads;
                            totalLeads += nbLeads;


                            /*if (DbReader.IsDBNull(0))
                                nbPatients = 0;
                            else
                                nbPatients = DbReader.GetInt32(0);*/

                            if (DbReader.IsDBNull(1))
                                amount = 0;
                            else
                                amount = DbReader.GetDecimal(1);
                        }
                        //totalNbPatients += nbPatients;
                        DbReader.Close();
                        totals.Add(amount);

                        campId = campaign.ID;
                        campName = campaign.name;
                        budget = (decimal)campaign.budget;
                        //string st1 = DbReader.GetFieldType(4).ToString();
                        //st1 = DbReader.GetFieldType(5).ToString();
                        percent = 0;

                        if (this.firstCampaign.Text == "")
                            this.firstCampaign.Text = campId.ToString();
                        if (nbLeads > 0) {
                            percent = nbPatients;
                            percent = percent / nbLeads;
                        }

                        slicePercent = 0;
                        if (amount > 0) {

                        }


                        if (genXML) {
                            string amtDisplay = amount.ToString("### ### ###");
                            if (amtDisplay.Trim() == "")
                                amtDisplay = "0";
                            sw.WriteLine("  <slice title=\"" + campName + "\nTracked ammout " + "\" url=\"javascript:loadCampaignDetail(" + lineNbr.ToString() + ",'" + campId + "',false)\" description=\"click for detail\" " + pullOut + "  >" + amtDisplay + "</slice>");
                            pullOut = "";
                        }

                        s += "<tr class=\"practiceroi-summary-row\" id=\"leadtableRow" + lineNbr.ToString() + "\"  onclick=\"javascript:loadCampaignDetail(" + lineNbr.ToString() + ", '" + campId + "',true)\" title=\"" + campName + "\">";
                        string color = colors[i % 13];
                        s += "<td style=\"width:@w0ipx;color:" + color + "\" class=\"colordot\" >&bull;</td>";
                        s += "<td style=\"width:@w1ipx;\"  title='ID " + campaign.ID.ToString() + "'>";
                        s += campName;
                        s += "</td>";

                        /*if (displayBudget) {
                            s += "<td  style=\"width:@w6ipx; text-align:right\" align='right'>";
                            s += budget.ToString("C0");
                            s += "</td>";
                        }*/


                        s += "<td style=\"width:@w2ipx;\"  align='right'>";
                        s += nbLeads.ToString();
                        s += "</td>";

                        s += "<td style=\"width:@w3ipx;\"  align='right'>";
                        s += nbPatients.ToString();
                        s += "</td>";

                        s += "<td style=\"width:@w4ipx;\"  align='right'>";
                        s += percent.ToString("P0");
                        s += "</td>";


                        s += "<td style=\"width:@w5ipx; white-space: nowrap;\"  align='right' title='" + amount.ToString("C0") + "'>";
                        s += "<b>" + amount.ToString("C0") + "</b>";
                        s += "</td>";

                        

                        if (DisplayRoi) {
                            s += "<td style=\"width:@w6ipx; white-space: nowrap;\"  align='right' title='" + amount.ToString("C0") + "'>";
                            if (budget > 0) {
                                System.TimeSpan ts = to.Subtract(from);
                                decimal periodBudget = (decimal)ts.TotalDays;
                                periodBudget = periodBudget * budget;
                                periodBudget = periodBudget / 30;
                                if (periodBudget > 0) {
                                    decimal budgetPerc = amount / periodBudget;
                                    s += "<b>" + budgetPerc.ToString("P0") + "</b>";
                                }else
                                    s += "<b>n/a</b>";
                            } else {
                                s += "<b>N/A</b>";
                            }
                            s += "</td>";
                        }
                        s += "</tr>";
                        total += amount;
                        totalBugdet += budget;
                        lineNbr++;
                    }
                }//FOR


                s += @"</table></div><table class=""practiceroi-summary-table"" >
<tr class=""practiceroi-summary-header"">
<td  style=""width:@w0px;"" ></td>
<td style=""width:@w1px;"" >Total</td>
<td style=""width:@w2px;"" class=""center"">@TOTAL_LEAD</td>
<td style=""width:@w3px;"" class=""center"">@TOTAL_PATIENT</td>
<td style=""width:@w4px;"" class=""center"">@CONVERSION</td>
<td style=""width:@w5px;"" class=""right"">$@TOTAL_ROI</td>@ROIFOOTER
<td></td>
</tr>";
                if (DisplayRoi) {
                    s = s.Replace("@ROIFOOTER", "<td style=\"width:@w6px;\" >-</td>");
                } else {
                    s = s.Replace("@ROIFOOTER", "");
                }
                for (int i = 0; i < colWidth.Length; i++) {
                    s = s.Replace("@w" + i.ToString() + "ipx", colWidthInside[i] + "px");
                    s = s.Replace("@w" + i.ToString() + "px", colWidth[i] + "px");
                    
                }

                s = s.Replace("@TOTAL_LEAD", totalLeads.ToString());
                s = s.Replace("@TOTAL_PATIENT", totalNbPatients.ToString());
                percent = 0;
                if (totalLeads > 0) {
                    percent = totalNbPatients;
                    percent = percent / totalLeads;
                }
                s = s.Replace("@CONVERSION", percent.ToString("P0"));
                s = s.Replace("@TOTAL_ROI", total.ToString("N0"));



                sHighlightTotals = sHighlightTotals.Replace("@TOTAL_LEAD", totalLeads.ToString());
                sHighlightTotals = sHighlightTotals.Replace("@TOTAL_PATIENT", totalNbPatients.ToString());
                percent = 0;
                if (totalLeads > 0) {
                    percent = totalNbPatients;
                    percent = percent / totalLeads;
                }
                sHighlightTotals = sHighlightTotals.Replace("@CONVERSION", percent.ToString("P0"));
                
                var culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
                if (total>1000000)
                    sHighlightTotals = sHighlightTotals.Replace("@TOTAL_ROI", (total / 1000000).ToString("##.# M"));
                else if (total>1000)
                    sHighlightTotals = sHighlightTotals.Replace("@TOTAL_ROI", (total / 1000).ToString("### K"));
                else if (total>0)
                    sHighlightTotals = sHighlightTotals.Replace("@TOTAL_ROI", total.ToString("###.###"));
                else
                    sHighlightTotals = sHighlightTotals.Replace("@TOTAL_ROI", "0 K");

                s += "</table></div>";
                if (genXML) {
                    sw.WriteLine("</pie>");
                    sw.Close();
                }

                string pie = sw.ToString();
                for (int i = 0; i < totals.Count; i++) {
                    if (total > 0) {
                        decimal perct = (decimal)totals[i];
                        perct = perct / total;
                        pie = pie.Replace("p" + i.ToString() + "%", (100 * perct).ToString("##")+"%");
                    }else
                        pie = pie.Replace("p" + i.ToString() + "%", "0%");
                }
                strmw.WriteLine(pie);
                strmw.Close();
                

                DbReader.Close();
                DbReader.Dispose();


                string graph = @"<div id=""campaign-summaries"">
<div id=""summary-chart-container-with-title"">
<div id=""summary-chart-title"">Campaign Breakdown</div>
<div id=""summary-chart-container"">
<div id=""summary-chart""></div></div>
</div>
";


                return sHighlightTotals + graph + s + "</div>";
            } catch (Exception ex) {
                return "getLeadTable Exception " + ex.Message;
            }
        }//getLeadTable

        protected string getTopProcedureTable(SqlConnection cn, int DB, DateTime from, DateTime to) {
            try {

                string WC = "usp_TopProcedures " + DB.ToString() + ",'" + from.ToShortDateString() + "','" + to.ToShortDateString() + "'";
                SqlCommand command = new SqlCommand(WC, cn);
                command.CommandTimeout = 300;
                SqlDataReader DbReader = command.ExecuteReader();

                string code = "";
                string patName = "";
                string campaignName = "";
                decimal amount;

                string s = "<table width=\"100%\" bgcolor=\"#EFF0F7\" class=\"LeadsDetailTable\" id=\"Table5\">";
                s += "<tr bgcolor=\"#EFF0F7\"><td height=\"5\" colspan=\"4\" background=\"images/line_06.jpg\"></td></tr><tr  bgcolor=\"#EFF0F7\"><th height=\"41\">Procedure</th><th>Patient</th><th>Campaign</th><th title='Treatment Plan Revenue'>Plan</th></tr>";
                string cssClass = "";
                int lineNbr = 0;
                while (DbReader.Read()) {
                    code = DbReader.GetString(0);
                    patName = DbReader.GetString(1);
                    string st1 = DbReader.GetFieldType(2).ToString();
                    //st1 = DbReader.GetFieldType(5).ToString();
                    campaignName = DbReader.GetString(6);
                    amount = DbReader.GetDecimal(5);

                    if (lineNbr % 2 == 0)
                        cssClass = "";
                    else
                        cssClass = "class='alternate'";

                    s += "<tr>";

                    s += "<td " + cssClass + ">";
                    s += code;
                    s += "</td>";

                    s += "<td " + cssClass + " title='" + patName + "'>";
                    s += "Patient #"+lineNbr.ToString();
                    s += "</td>";

                    s += "<td " + cssClass + " align='left'>";
                    s += campaignName;
                    s += "</td>";

                    s += "<td " + cssClass + " align='right'>";
                    s += amount.ToString("C0");
                    s += "</td>";

                    s += "</tr>";
                    lineNbr++;
                }
                s += "</table>";
                DbReader.Close();
                DbReader.Dispose();
                return s;
            } catch (Exception ex) {
                return "getTopProcedureTable Exception " + ex.Message;
            }
        }//getTopProcedureTable

        private void getMinMaxDates(int period, ref DateTime from, ref DateTime to) {
            DateTime td = DateTime.Today;
            if (period == 0) {
                //last week
                from = td.AddDays(-(int)td.DayOfWeek).AddDays(-7);
                to = from.AddDays(7);
            } else if (period == 1) {
                //last month
                from = new DateTime(td.AddMonths(-1).Year, td.AddMonths(-1).Month, 1);
                to = from.AddMonths(1).AddDays(-1);
            } else if (period == 2) {
                //year to date
                from = new DateTime(td.Year, 1, 1);
                to = td;
            } else if (period == 3) {
                //last year
                from = new DateTime(td.Year - 1, 1, 1);
                to = from.AddYears(1).AddDays(-1);
            } else {
                //all
                from = new DateTime(1900, 1, 1);
                to = new DateTime(2100, 1, 1);


                from = new DateTime(2008, 1, 1);
                to = DateTime.Today.AddDays(1);

            }
        }

        protected string getAccountName(SqlConnection cn, int DB) {
            try {
                if (Session["accountname"] == null) {
                    string WC = "select name from account where DB=" + DB.ToString();
                    SqlCommand command = new SqlCommand(WC, cn);
                    command.CommandTimeout = 300;
                    SqlDataReader DbReader = command.ExecuteReader();
                    string name = "";
                    if (DbReader.Read()) {
                        name = DbReader.GetString(0);
                        Session["accountname"] = name;
                    }
                    DbReader.Close();
                    DbReader.Dispose();
                }
                return (string)Session["accountname"];
            } catch (Exception ex) {
                throw new Exception("getAccountName exception:" + ex.Message);
            }
        }

        public DisplayMode DisplayModeSelected(SqlConnection cn , int db) {
            if (Session["displayTP"] == null) {
                int i=SysOption.GetValue(cn, db, "DISPLAY_TREATMENTPLAN", 0);
                Session["displayTP"] = (DisplayMode)i;
            }
            return (DisplayMode)Session["displayTP"];
        }

        private void SetDisplayModeSelected(SqlConnection cn, int db, DisplayMode mode) {
            Session["displayTP"] = mode;
            SysOption.SetValue(cn, db, "DISPLAY_TREATMENTPLAN", (int)mode);
        }

        private void DisplayMainPage(SqlConnection cn, DateTime from, DateTime to) {
            Debug.WriteLine("In Dashboard(1)");
            Debug.WriteLine("In Dashboard(1)", "sdsd");
            int db = (int)Session["DB"];// .Parse(Request.Params["db"]);

            this.DB.Text = db.ToString();
            this.GenerationNumber.Text = "22";
            this.GenerationNumber.Text = DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            DisplayMode displayMode = DisplayModeSelected(cn, db); //DisplayMode.Combine;
            if (Request["TP"] != null) {
                int newdisplayMode = Util.safeIntParse(Request["TP"]);
                if (newdisplayMode != (int)displayMode) {
                    SetDisplayModeSelected(cn, db, (DisplayMode)newdisplayMode);
                }
                displayMode = (DisplayMode)newdisplayMode;
            }
            this.displayMode.SelectedIndex = (int)displayMode;

            this.leadTable.Text = getLeadTable(cn, db, from, to, true, this.GenerationNumber.Text, displayMode);
            this.AccountName.Text = "<span title='Account #" + db.ToString() + "'>" + getAccountName(cn, db) + "</span>";

            Session["from"] = from;
            Session["to"] = to;
            if (Request["message"] != null) {
                if (Request["message"] != "")
                    this.labelError.Text += "msg:" + Request["message"];
            }
            if ((Request["iFrame"] != null) | (Session["iFrame"] != null)) {
                this.PanelHeader.Visible = false;
                this.PanelIframeHeader.Visible = true;
                this.iFrameIndicatore.Text = "1";
                //this.LinkLogOut.Visible = false;
                if (this.LinkLogOut.NavigateUrl.ToLower().IndexOf("iframe") < 0)
                    this.LinkLogOut.NavigateUrl += "&iFrame=1";
                Session["iFrame"] = 1;
            }
        }

        protected void pbPeriodSelec_Click(object sender, EventArgs e) {
            try {
                using (SqlConnection cn = new SqlConnection()) {
                    cn.ConnectionString = Connect.connectString();// @"Data Source=" + SQLserver + ";User ID=sa;Password=sa;" + "Initial Catalog=" + DataBase;
                    cn.Open();
                    DisplayMainPage(cn, fromDate.SelectedDate, toDate.SelectedDate);
                    this.labelError.Text = "From " + fromDate.SelectedDate.ToShortDateString()+" "+toDate.SelectedDate.ToShortDateString();

                    this.period.Items.Clear();
                    this.period.Items.Add(new ListItem("Last week"));
                    this.period.Items.Add(new ListItem("Last month"));
                    this.period.Items.Add(new ListItem("Year to date"));
                    this.period.Items.Add(new ListItem("Last year"));
                    this.period.Items.Add(new ListItem("All"));
                    this.period.Items.Add(new ListItem("   "+fromDate.SelectedDate.ToShortDateString() + " --> " + toDate.SelectedDate.ToShortDateString())+"   ");
                    this.period.SelectedIndex = 5;
                    cn.Close();
                }
            } catch (Exception ex) {
                this.labelError.Text += "Error:"+ex.Message;
            }
        }

        private DateTime MiniLead(SqlConnection cn, int DB) {
            try{
                DateTime d=DateTime.Today;
                SqlCommand cmd = new SqlCommand("select min(timestamp) from lead where db=" + DB.ToString(), cn);
                SqlDataReader r = cmd.ExecuteReader();
                if (r.Read()) {
                    d = r.GetDateTime(0);
                }
                r.Close();
                r.Dispose();
                cmd.Cancel();
                cmd.Cancel();
                return d;
            } catch (Exception ex) {
                this.labelError.Text += "Error:" + ex.Message;
                return new DateTime(DateTime.Today.Year, 1, 1);
            }
        }

        private int SessionPeriod {
            get {
                if (Request["p"] != null)
                    Session["period"] = int.Parse(Request.Params["p"]);

                if (Session["period"] == null)
                    return 2;
                else
                    return (int)Session["period"];
            }
            set {
                Session["period"] = (int)value;
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            try {
                System.Web.HttpContext.Current.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
                this.EntryURL.Text = Request.Url.ToString();
                this.labelError.Text = "";
                if (Session["superadmin"] != null) {
                    //this.superLink.Text = "<a href='campaignNames.aspx'><small>Settings</small></a>";
                }
                string SQLserver = getServerName();
                string DataBase = "LPI";
                using (SqlConnection cn = new SqlConnection()) {
                    cn.ConnectionString = Connect.connectString();// @"Data Source=" + SQLserver + ";User ID=sa;Password=sa;" + "Initial Catalog=" + DataBase;
                    cn.Open();

                    if (Request["count"] == null) {
                        //Simple load
                        if (!this.IsPostBack) {
                            if (Session["DB"] != null) {
                                //this.fromDate.SelectedDate = MiniLead(cn, (int)Session["DB"]);//  new DateTime( DateTime.Today.Year-1,1,1);
                                this.fromDate.SelectedDate = new DateTime(DateTime.Today.Year, 1, 1);
                                this.toDate.SelectedDate = DateTime.Today;
                                int period = SessionPeriod;// int.Parse(Request.Params["p"]);
                                DateTime from = new DateTime(2011, 3, 1);
                                DateTime to = new DateTime(2011, 3, 31);
                                getMinMaxDates(period, ref from, ref to);
                                this.period.SelectedIndex = period;
                                DisplayMainPage(cn, from, to);
                            } else
                                Response.Redirect("login.aspx");
                        }
                    } else {
                        System.Web.HttpContext.Current.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
                        //Ajax Flow call
                        string response = null;
                        if (Request["x"] != null) {
                            response = Request["x"] + "|Test Value from server";
                        } else if (Request["campDetail"] != null) {
                            string campaignId = Request["campDetail"];
                            int iCampId = 0;
                            try {
                                iCampId = int.Parse(campaignId);
                            } catch {
                            }

                            if (Session["DB"] != null) {
                                int db = (int)Session["DB"];
                                DateTime from = (DateTime)Session["from"];
                                DateTime to = (DateTime)Session["to"];
                                DisplayMode displayMode = DisplayModeSelected(cn, db);
                                response = "campaignDetail|" + getCampaignDetailTable(cn, db, iCampId, from, to, displayMode);
                            } else {
                                response = "campaignDetail|Session Time Out";
                            }
                        } else if (Request["apptDetail"] != null) {
                            string patId = Request["apptDetail"];
                            if (Session["DB"] != null) {
                                int db = (int)Session["DB"];
                                DateTime from = (DateTime)Session["from"];
                                DateTime to = (DateTime)Session["to"];
                                response = getApptZoomTable(cn, db, int.Parse(patId), from, to);
                            }
                        } else if (Request["LeadDetail"] != null) {
                            //"LeadDetail=1&DB=" + DB.ToString() + "&campId=" + campId.ToString() + "&number=" + Util.phoneDigits(number) + "&date=" + ts.Month.ToString() + "-" + ts.Day.ToString() + "-" + ts.Year.ToString();
                            int campId = Util.safeIntParse(Request["campId"]);
                            string number = Request["number"];
                            DateTime date = Util.safeDateParse(Request["date"]);
                            if (Session["DB"] != null) {
                                int db = (int)Session["DB"];
                                string id = Request["ID"];
                                if (!string.IsNullOrEmpty(id))
                                    //response = getLeadZoomTable(cn, db, campId, number, date, (Request["source"] != null));
                                    response = getLeadZoomTable(cn, db, Util.safeIntParse(id), (Request["source"] != null));
                            }
                        }

                        if (response != null) {
                            this.Response.Clear();
                            this.Response.Write(response);
                            try {
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                                this.Response.End();
                            } catch (Exception ex) {
                                
                            } finally {
                                
                            }
                        }


                    }
                    cn.Close();
                }
                System.Web.HttpContext.Current.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
            } catch (Exception ex) {
                this.leadTable.Text = "Exception in Page:" + ex.ToString();
                this.labelError.Text += "Exception in Page:" + ex.ToString();
           }
        }
    }
}
//login.aspx?db=1161&mk=masterkey2013&iFrame=1