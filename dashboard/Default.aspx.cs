using System;
using System.Collections;
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



using System.Xml.Linq;

namespace dashboard {
    public partial class _Default : System.Web.UI.Page {



        protected string getApptZoomTable(SqlConnection cn, int DB, int patId,DateTime from, DateTime to){
            try {

                string WC = "select * from dentalprocedure where DB=" + DB.ToString() + " and patientid=" + patId.ToString();
                WC += " and dateTime>='"+from.ToShortDateString()+"'  and  dateTime<'"+to.ToShortDateString()+"'";
                SqlCommand command = new SqlCommand(WC, cn);
                SqlDataReader DbReader = command.ExecuteReader();

                string code = "";
                DateTime date;
                decimal amount;

                string s = @"<h4 style='font-size:18px;font-family:Arial; '>Appointment with #PATIENT</h4>
<table style='font-size:12px;font-family:Arial;  '>
<tr> 
    <th>From Date</th>
    <td>#DATE1</td>    
</tr>
<tr> 
    <th>To Date</th>
    <td>#DATE2</td>    
</tr>
<tr> 
    <th>Total</th>
    <td align=right><b>#TOTAL</b></td>    
</tr>
</table>

<table width=90% style=' font-family:Arial' align=center>
<tr>
    <th>Code</th>
    <th>Name</th>
    <th>$</th>
</tr>
</table>

<div id='floatdiv' style=' width:390;height:200; padding-top:0px;
background:#FFFFFF; 
border:1px solid #2266AA;
color:black;
overflow:scroll;
'> 
<table width=100% style=' font-family:Arial'>";



                string cssClass = "";
                decimal totalAmount = 0;
                int lineNbr = 0;
                while (DbReader.Read()) {
                    code = DbReader.GetString(2);
                    date = DbReader.GetDateTime(4);
                    amount = DbReader.GetDecimal(5);

                    if (lineNbr % 2 == 0)
                        cssClass = "";
                    else
                        cssClass = "class='alternate'";

                    s += "<tr>";

                    s += "<td " + cssClass + ">";
                    s += code;
                    s += "</td>";

                    s += "<td " + cssClass + " >";
                    s += "-";//ADA code problem
                    s += "</td>";

                    s += "<td " + cssClass + " align='right'>";
                    s += amount.ToString("N");
                    totalAmount += amount;
                    s += "</td>";

                    s += "</tr>";
                    lineNbr++;
                }
                s += "</table>";
                DbReader.Close();
                DbReader.Dispose();

                s += "</div> <br><div style='	POSITION:absolute;	TOP:360px'>";
                s += "<INPUT type=button value=Close id=pbSave onclick=\"document.getElementById('popupApptDetail').style.display = 'none';\" NAME='pbSave' style='TEXT-ALIGN:center'\"></div>";


                s = s.Replace("#DATE1", from.ToShortDateString());
                s = s.Replace("#DATE2", to.ToShortDateString());
                s = s.Replace("#TOTAL", totalAmount.ToString("N"));

                WC = "select firstname,name from patient where DB=" + DB.ToString() + " and AMid=" + patId.ToString();
                command = new SqlCommand(WC, cn);
                DbReader = command.ExecuteReader();
                if (DbReader.Read()) {
                    string fname = DbReader.GetString(0);
                    string name = DbReader.GetString(1);
                    s = s.Replace("#PATIENT", fname+" "+name);
                    s = s.Replace("#TOTAL", totalAmount.ToString("N"));
                }

                return s;
            } catch (Exception ex) {
                return "getTopProcedureTable Exception " + ex.Message;
            }
        }

        protected string getCampaignDetailTable(SqlConnection cn, int DB, int campId, DateTime from, DateTime to) {
            try {
                string WC = "select name from campaign where db=" + DB.ToString() + " and ID=" + campId.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                SqlDataReader DbReader = command.ExecuteReader();
                string campName = "";
                while (DbReader.Read()) {
                    campName = DbReader.GetString(0);
                }
                DbReader.Close();
                DbReader.Dispose();

                WC = "usp_Leads " + DB.ToString() + "," + campId.ToString() + ",'" + from.ToShortDateString() + "','" + to.ToShortDateString() + "'";
                command = new SqlCommand(WC, cn);
                DbReader = command.ExecuteReader();

                string number = "";
                DateTime ts;
                string apptId = "";
                string patName = "";
                DateTime apptDateTime;
                decimal amount=0;
                int duration = 0;
                string URL = "";


                string s = "<span style='font-family: Arial Black; font-size: 18px; color: #347828;'>" + campName + "</span>";
                s += @"<table width='100%' cellpadding='10'>
                    <tr>
                        <td style='background-image: url(images/backGroundPie.gif);' align='left' width='100%' valign='top'>
                            <span style='font-family: Arial Black; font-size: 13px;'>Generated Leads</span>
                            <table class='LeadsDetailTable' id='Table3' width='100%'>
                                <tr>
                                    <th rowspan=2 width=20%>
                                        Phone Number
                                    </th>
                                    <th rowspan=2  width=10%>
                                        Date
                                    </th>
                                    <th rowspan=2  width=10%>
                                        Time
                                    </th>
                                    <th rowspan=2  width=5%>
                                        Duration
                                    </th>
                                    <th rowspan=2  width=5%>
                                        Listen
                                    </th>
                                    <th colspan=5>
                                        Generated Appointments
                                    </th>
                                </tr>
                                <tr>
                                    <th  width=20%>
                                        Patient
                                    </th>
                                    <th  width=10%>
                                        Date
                                    </th>
                                    <th  width=10%>
                                        Time
                                    </th>
                                    <th  width=10%>
                                        Amount
                                    </th>
                                </tr>";

                s += "</table>";
                s += "<div style=\" padding-top:0px;margin-top:0px;margin-left:5px;margin-right:0px; border:none; height:300px;overflow:scroll;\">";
                s += "<table class='LeadsDetailTable' id='Table3' width='100%'>";
                
                string cssClass = "";
                int lineNbr = 0;
                while (DbReader.Read()) {
                    number = DbReader.GetString(2);
                    ts = DbReader.GetDateTime(3);
                    duration = DbReader.GetInt16(4);
                    URL = DbReader.GetString(5);

                    apptId = DbReader.GetString(6);
                    patName = DbReader.GetString(7);
                    apptDateTime = DateTime.MinValue;

                    if (!DbReader.IsDBNull(8))
                        apptDateTime = DbReader.GetDateTime(8);
                    if (!DbReader.IsDBNull(9))
                        amount = DbReader.GetDecimal(9);


                    //string st1 = DbReader.GetFieldType(2).ToString();
                    //st1 = DbReader.GetFieldType(5).ToString();

                    if (lineNbr % 2 == 0)
                        cssClass = "";
                    else
                        cssClass = "class='alternate'";

                    s += "<tr>";

                    s += "<td " + cssClass + " nowrap width=20%>";
                    s += number;
                    s += "</td>";

                    s += "<td " + cssClass + "  width=10%>";
                    s += ts.ToShortDateString();
                    s += "</td>";

                    s += "<td " + cssClass + "  align=center width=10%>";
                    s += ts.ToShortTimeString();
                    s += "</td>";

                    s += "<td " + cssClass + " align=center  width=7%>";
                    s += duration.ToString();
                    s += "</td>";

                    s += "<td " + cssClass + " align='center' width=5%>";
                    s += "<a href='#' title='Listen to the call " + URL + " '><img src='images/listen.gif' border='0' /></a>";
                    s += "</td>";

                    if (!DbReader.IsDBNull(8)) {
                        s += "<td " + cssClass + "  width=20%>";
                        s += patName;
                        s += "</td>";

                        s += "<td " + cssClass + " align='center'  width=10%>";
                        s += apptDateTime.ToShortDateString();
                        s += "</td>";

                        s += "<td " + cssClass + " align='center'  width=10%>";
                        s += apptDateTime.ToShortTimeString();
                        s += "</td>";

                        s += "<td " + cssClass + " align='right'  width=10%>";
                        s += "<a href='javascript:openApptDetailDetail("+ apptId + ")'><b>" + amount.ToString("N") + "</b></a>";
                        s += "</td>";
                    } else {
                        s += "<td " + cssClass + "  width=20%>";
                        s += "</td>";

                        s += "<td " + cssClass + " align='center'  width=10%>";
                        s += "</td>";

                        s += "<td " + cssClass + " align='center'  width=10%>";
                        s += "</td>";

                        s += "<td " + cssClass + " align='right'  width=10%>";
                        s += "</td>";
                    }
                    s += "</tr>";
                    lineNbr++;
                }
                s += "</table>";
                s += "</div>";
                DbReader.Close();
                DbReader.Dispose();
                return s;
            } catch (Exception ex) {
                return "getCampaignDetailTable Exception " + ex.Message;
            }

        }//getCampaignDetailTable

        protected string getServerName() {
            if (Session["server"] == null) {
                string path = HostingEnvironment.MapPath("~");
                string line = "";
                string server="";
                System.IO.StreamReader file =   new System.IO.StreamReader(path +"\\lpi.ini");
                while ((line = file.ReadLine()) != null) {
                    if (line.StartsWith("server=")) {
                        server = line.Substring(line.IndexOf("server=") + 7);
                    }
                }
                file.Close();
                Session["server"] = server;
            }
            return (string)Session["server"];
        }

        protected string getLeadTable(SqlConnection cn, int DB, int period, DateTime from, DateTime to, bool genXML) {
            try {
                System.IO.StreamWriter sw = null;
                if (genXML) {
                    string path = HostingEnvironment.MapPath("~");
                    string XMLFileName = path + "\\data" + period.ToString() + ".xml";
                    sw = new System.IO.StreamWriter(XMLFileName);
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    sw.WriteLine("<pie>");
                }

                string WC = "usp_CampaignResult "+DB.ToString()+",'"+from.ToShortDateString()+"','"+to.ToShortDateString()+"'";
                SqlCommand command = new SqlCommand(WC, cn);
                SqlDataReader DbReader = command.ExecuteReader();
                int campId=0;
                string campName = "";
                int nbLeads = 0;
                int nbPatients = 0;
                double percent = 0;
                decimal amount = 0;

                string s = "<table class='LeadTable' id='LeadTableTable' width=100%><tr><th>Campaign</th><th>Leads<br>#</th><th>Appointments<br>#</th><th>Return<br>%</th><th>Total<br>$</th></tr>";
                string cssClass = "";
                string pullOut = " pull_out=\"true\"";
                int lineNbr = 0;
                this.firstCampaign.Text = "";
                while (DbReader.Read()) {
                    campId= DbReader.GetInt32(0);
                    campName = DbReader.GetString(1);
                    nbLeads = DbReader.GetInt32(2);
                    nbPatients = DbReader.GetInt32(3);
                    //string st1 = DbReader.GetFieldType(4).ToString();
                    //st1 = DbReader.GetFieldType(5).ToString();
                    percent = 0;

                    if (this.firstCampaign.Text == "")
                        this.firstCampaign.Text = campId.ToString();
                    if (nbLeads > 0) {
                        percent = nbPatients;
                        percent = percent / nbLeads;
                    }

                    amount = DbReader.GetDecimal(5);
                    if (genXML) {
                        sw.WriteLine("  <slice title=\"" + campName + "\" url=\"javascript:loadCampaignDetail(" + lineNbr.ToString() + ",'" + campId + "',false)\" description=\"click for detail\" " + pullOut + "  >" + amount.ToString() + "</slice>");
                        pullOut = "";
                    }



                    if (lineNbr % 2 == 0)
                        cssClass = "";
                    else
                        cssClass = "";
                        //cssClass = "class='alternate'";

                    s += "<tr id=\"leadtableRow"+lineNbr.ToString()+"\">";

                    s += "<td " + cssClass + ">";
                    s += campName;
                    s += "</td>";

                    s += "<td " + cssClass + " align='right'>";
                    s += nbLeads.ToString();
                    s += "</td>";

                    s += "<td " + cssClass + " align='right'>";
                    s += nbPatients.ToString();
                    s += "</td>";

                    s += "<td " + cssClass + " align='center'>";
                    s += percent.ToString("P");
                    s += "</td>";


                    s += "<td " + cssClass + " align='right'>";
                    s += "<a href=\"javascript:loadCampaignDetail("+lineNbr.ToString()+", '" + campId + "',true)\">"+amount.ToString("F")+"</a>";
                    s += "</td>";

                    s += "</tr>";
                    lineNbr++;
                }
                s += "</table>";
                if (genXML) {
                    sw.WriteLine("</pie>");
                    sw.Close();
                }
                DbReader.Close();
                DbReader.Dispose();

                return s;
            } catch (Exception ex) {
                return "getLeadTable Exception " + ex.Message;
            }
        }//getLeadTable

        protected string getTopProcedureTable(SqlConnection cn, int DB, DateTime from, DateTime to) {
            try {

                string WC = "usp_TopProcedures " + DB.ToString() + ",'" + from.ToShortDateString() + "','" + to.ToShortDateString() + "'";
                SqlCommand command = new SqlCommand(WC, cn);
                SqlDataReader DbReader = command.ExecuteReader();

                string code = "";
                string patName = "";
                string date="";
                decimal amount;
                
                string s = "<table class='LeadTable' width=100%><tr><th>Code</th><th>Name</th><th>Date</th><th>$</th></tr>";
                string cssClass = "";
                int lineNbr = 0;
                while (DbReader.Read()) {
                    code= DbReader.GetString(0);
                    patName= DbReader.GetString(1);
                    string st1 = DbReader.GetFieldType(2).ToString();
                    //st1 = DbReader.GetFieldType(5).ToString();
                    date = DbReader.GetString(2);
                    amount = DbReader.GetDecimal(3);

                    if (lineNbr % 2 == 0)
                        cssClass = "";
                    else
                        cssClass = "class='alternate'";

                    s += "<tr>";

                    s += "<td " + cssClass + ">";
                    s += code;
                    s += "</td>";

                    s += "<td " + cssClass + " >";
                    s += patName;
                    s += "</td>";

                    s += "<td " + cssClass + " align='center'>";
                    s += date;
                    s += "</td>";

                    s += "<td " + cssClass + " align='center'>";
                    s += amount.ToString("N");
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

        private void getMinMaxDates(int period, ref DateTime from,ref DateTime to) {
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
                from = new DateTime(1900,1,1);
                to = new DateTime(2100,1,1);
            }
        }

        protected string getAccountName(SqlConnection cn, int DB) {
            try {
                string WC = "select name from account where DB=" + DB.ToString();
                SqlCommand command = new SqlCommand(WC, cn);
                SqlDataReader DbReader = command.ExecuteReader();
                string name="";
                if (DbReader.Read()){
                    name=DbReader.GetString(0);
                }
                DbReader.Close();
                DbReader.Dispose();
                return name;

            } catch (Exception ex) {
                throw new Exception("getAccountName exception:"+ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            try {
                string SQLserver = getServerName();
                
                string DataBase = "LPI";

                using (SqlConnection cn = new SqlConnection()) {
                    cn.ConnectionString = @"Data Source=" + SQLserver + ";User ID=sa;Password=sa;" + "Initial Catalog=" + DataBase;
                    cn.Open();

                    if (Request["count"] == null) {
                        //Simple load
                        
                        int db = int.Parse(Request.Params["db"]);
                        this.DB.Text = db.ToString();
                        int period = int.Parse(Request.Params["p"]);
                        DateTime from = new DateTime(2011, 3, 1);
                        DateTime to = new DateTime(2011, 3, 31);
                        getMinMaxDates(period, ref from, ref to);
                        this.period.SelectedIndex = period;
                        
                        this.leadTable.Text = getLeadTable(cn, 500,period, from, to,true);
                        this.topProcedures.Text = getTopProcedureTable(cn, db, from, to);
                        this.AccountName.Text = getAccountName(cn, db);

                        Session["DB"] = db;
                        Session["from"] = from;
                        Session["to"] = to;

                    } else {
                        //Ajax Flow call
                        string response = null;
                        if (Request["x"] != null) {
                            response = Request["x"] + "|Test Value from server";
                        } else if (Request["campDetail"] != null) {
                            string campaignId = Request["campDetail"];
                            int iCampId=int.Parse(campaignId);

                            if (Session["DB"]!=null){
                                int db=(int)Session["DB"];
                                DateTime from=(DateTime)Session["from"];
                                DateTime to =(DateTime)Session["to"];
                                response = "campaignDetail|" + getCampaignDetailTable(cn,db, iCampId,from,to);
                            }else{
                                response = "campaignDetail|Session Time Out" ;
                            }
                        } else if (Request["apptDetail"] != null) {
                            string patId = Request["apptDetail"];
                            if (Session["DB"] != null) {
                                int db = (int)Session["DB"];
                                DateTime from = (DateTime)Session["from"];
                                DateTime to = (DateTime)Session["to"];
                                response = getApptZoomTable(cn, db, int.Parse(patId), from, to);
                            }
                        }

                        if (response != null) {
                            this.Response.Clear();
                            this.Response.Write(response);
                            try {
                                this.Response.End();
                            } catch (Exception ex) {

                            } finally {

                            }
                        }
                    }


                    cn.Close();
                }
            } catch (Exception ex) {
                this.leadTable.Text = "Exception in Page:" + ex.Message;
            }
        }
    }
}
