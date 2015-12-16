using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataBase;

namespace LeadBean {
    public partial class confirm : System.Web.UI.Page {

        protected void TrashLead(int ID) {
            try {
                using (SqlConnection cn = Connect.getDefaultConnection()) {

                    string WC = "insert into leadbean select * from lead where id=" + ID.ToString();
                    SqlCommand cmd = new SqlCommand(WC, cn);
                    int nbRows = cmd.ExecuteNonQuery();
                    if (nbRows != 1)
                        throw new Exception("insert of lead (" + ID.ToString() + ") affects " + nbRows.ToString() + " which is not 1. Update stopped");
                    cmd.Dispose();
                    cmd = null;

                    WC = "delete from lead where id=" + ID.ToString();
                    cmd = new SqlCommand(WC, cn);
                    nbRows = cmd.ExecuteNonQuery();
                    if (nbRows != 1)
                        throw new Exception("Suppresion of lead (" + ID.ToString() + ") affects " + nbRows.ToString() + " which is not 1. Update stopped");
                    cmd.Dispose();
                    cmd = null;

                    cn.Close();
                }
            } catch (Exception ex) {
                throw new Exception("TrashLead ex:" + ex.Message);
            }
        }//TrashLead

        protected void RestoreLead(int ID) {
            try {
                using (SqlConnection cn = Connect.getDefaultConnection()) {

                    string WC = "insert into lead select * from leadbean where id=" + ID.ToString();
                    SqlCommand cmd = new SqlCommand(WC, cn);
                    int nbRows = cmd.ExecuteNonQuery();
                    if (nbRows != 1)
                        throw new Exception("insert of lead (" + ID.ToString() + ") affects " + nbRows.ToString() + " which is not 1. Update stopped");
                    cmd.Dispose();
                    cmd = null;

                    WC = "delete from leadbean where id=" + ID.ToString();
                    cmd = new SqlCommand(WC, cn);
                    nbRows = cmd.ExecuteNonQuery();
                    if (nbRows != 1)
                        throw new Exception("Suppresion of lead (" + ID.ToString() + ") affects " + nbRows.ToString() + " which is not 1. Update stopped");
                    cmd.Dispose();
                    cmd = null;

                    cn.Close();
                }
            } catch (Exception ex) {
                throw new Exception("TrashLead ex:" + ex.Message);
            }
        }//TrashLead


        protected void pbConfirmRestore_Click(object sender, EventArgs e) {
            if (Session["account"] != null) {
                RestoreLead(Util.safeIntParse(this.ID.Text));
                Response.Redirect("Default.aspx");
            }
        }



        protected void pbConfirmDelete_Click(object sender, EventArgs e) {
            if (Session["account"] != null) {
                TrashLead(Util.safeIntParse(this.ID.Text));
                Response.Redirect("Default.aspx");
            }
        }

        protected void pb_save_primary_Click(object sender, EventArgs e) {
            try {
                if (Session["account"] != null) {
                    using (SqlConnection cn = Connect.getDefaultConnection()) {

                        string WC = "update lead set alterPrimaryPhone='" + Util.phoneDigits(this.NewPrimaryPhone.Text) + "'";
                        WC += ",alterName='" + this.AlterName.Text + "', alterFirstName='" + this.AlterFirstName.Text + "'";
                        WC += " where id=" + this.ID.Text;
                        SqlCommand cmd = new SqlCommand(WC, cn);
                        int nbRows = cmd.ExecuteNonQuery();
                        if (nbRows != 1)
                            throw new Exception(WC + " affects " + nbRows.ToString() + " which is not 1. Update stopped");
                        cmd.Dispose();
                        cmd = null;

                        cn.Close();
                    }
                    Response.Redirect("Default.aspx");
                }
            } catch (Exception ex) {
                this.LabelError.Text = "Exception:" + ex.Message;
            }
        }



        protected void pbCancel_Click(object sender, EventArgs e) {
            Response.Redirect("Default.aspx");
        }

        
        private string getLeadTableTemplate(){
            return @"<table>
<tr><th>ID</th><td>@ID</td></tr>
<tr><th>DB</th><td>@DB</td></tr>
<tr><th>Campaign ID</th><td>@campaignID</td></tr>
<tr><th>InComing Number</th><td>@inComingNumber</td></tr>
<tr><th>Primary Phone</th><td>@PrimaryPhone</td></tr>
<tr><th>Alter. Primary Phone</th><td>@AlterPrimaryPhone</td></tr>
<tr><th>Time Stamp</th><td>@timeStamp</td></tr>
<tr><th>Duration In Minutes</th><td>@durationInMinutes</td></tr>
<tr><th>New Patient</th><td>@newPatient</td></tr>
<tr><th>DOB</th><td>@birthDate</td></tr>
<tr><th>PatientID</th><td>@patientID</td></tr>
<tr><th>Sound FileUrl</th><td>@fileUrl</td></tr>
<tr><th>Dental Need</th><td>@dentalNeed</td></tr>
<tr><th>Dental Care Is For</th><td>@dentalCareIsFor</td></tr>
<tr><th>Preferred Appointment Time</th><td>@preferredAppointmentTime</td></tr>
<tr><th>Insurance Plan Budget</th><td>@insurancePlanBudget</td></tr>
<tr><th>First Name</th><td>@firstName</td></tr>
<tr><th>Last Name</th><td>@lastName</td></tr>
<tr><th>Email</th><td>@email</td></tr>
<tr><th>Source</th><td>@source</td></tr>
<tr><th>Medium</th><td>@medium</td></tr>
<tr><th>Term</th><td>@term</td></tr>
<tr><th>Content</th><td>@content</td></tr>
<tr><th>Campaign</th><td>@campaign</td></tr>
<tr><th>Referred by</th><td>@referred_by</td></tr>
<tr><th>Original Status</th><td>@original</td></tr>
</table>";
        }
        
        protected void DisplayLead(bool fromBean,int ID) {
            try {
                using (SqlConnection cn = Connect.getDefaultConnection()) {

                    Lead l = new Lead(cn);
                    if (fromBean) {
                        if (!l.DBGetFromBean(ID))
                            throw new Exception("Could not find lead " + ID.ToString());
                    } else {
                        if (!l.DBGet(ID))
                            throw new Exception("Could not find lead " + ID.ToString());
                    }

                    string s = getLeadTableTemplate();

                    s = s.Replace("@ID",l.ID.ToString());
                    s = s.Replace("@DB",l.DB.ToString());
                    s = s.Replace("@campaignID",l.campaignID.ToString());
                    s = s.Replace("@inComingNumber",l.inComingNumber.ToString());
                    s = s.Replace("@PrimaryPhone", l.PrimaryPhone.ToString());
                    s = s.Replace("@AlterPrimaryPhone", l.alterPrimaryPhone.ToString());
                    s = s.Replace("@timeStamp",l.timeStamp.ToString());
                    s = s.Replace("@durationInMinutes",l.durationInMinutes.ToString());
                    s = s.Replace("@newPatient",l.newPatient.ToString());
                    s = s.Replace("@birthDate",l.birthDate.ToString());
                    s = s.Replace("@patientID",l.patientID.ToString());
                    s = s.Replace("@fileUrl",l.fileUrl.ToString());
                    s = s.Replace("@dentalNeed",l.dentalNeed.ToString());
                    s = s.Replace("@dentalCareIsFor",l.dentalCareIsFor.ToString());
                    s = s.Replace("@preferredAppointmentTime",l.preferredAppointmentTime.ToString());
                    s = s.Replace("@insurancePlanBudget",l.insurancePlanBudget.ToString());
                    s = s.Replace("@firstName",l.firstName.ToString());
                    s = s.Replace("@lastName",l.lastName.ToString());
                    s = s.Replace("@email",l.email.ToString());
                    s = s.Replace("@source",l.source.ToString());
                    s = s.Replace("@medium",l.medium.ToString());
                    s = s.Replace("@term",l.term.ToString());
                    s = s.Replace("@content",l.content.ToString());
                    s = s.Replace("@campaign",l.campaign.ToString());
                    s = s.Replace("@referred_by",l.referred_by.ToString());
                    s = s.Replace("@original",l.original.ToString());

                    this.labelMain.Text = s;
                    this.NewPrimaryPhone.Text = l.alterPrimaryPhone;
                    this.AlterName.Text = l.alterName;
                    this.AlterFirstName.Text = l.alterFirstName;
                    cn.Close();
                }
            } catch (Exception ex) {
                throw new Exception("DisplayLead ex:" + ex.Message);
            }
        }//DisplayLead
        
        
        protected void Page_Load(object sender, EventArgs e) {
            try {
                if (Session["account"] != null) {
                    if (!this.IsPostBack) {
                        if (Request["DELID"] != null) {
                            this.pbConfirmRestore.Visible = false;
                            this.Title.Text = "Confirm Suppression of Lead #";
                            this.ID.Text = Request["DELID"];
                            DisplayLead(false,Util.safeIntParse(Request["DELID"]));
                        } else if (Request["RESTOREID"] != null) {
                            this.pbConfirmDelete.Visible = false;
                            this.Title.Text = "Confirm Restoration of Lead #";
                            this.ID.Text = Request["RESTOREID"];
                            DisplayLead(true,Util.safeIntParse(Request["RESTOREID"]));
                            this.PanelAltermativePhone.Visible = false;
                        }
                            
                    }
                } else
                    Response.Redirect("login.aspx");
            } catch (Exception ex) {
                this.LabelError.Text = "Exception:" + ex.Message;
            }
        }


    }
}
