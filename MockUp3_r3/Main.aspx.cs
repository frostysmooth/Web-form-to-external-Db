using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MockUp1 {
    public partial class _Default : System.Web.UI.Page {
        private string connString = System.Configuration.ConfigurationManager.ConnectionStrings["AncillaryConnString"].ToString();

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////PAGE LIFE CYCLE EVENTS
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //PAGE INITIALIZATION EVENT
        void Page_Init(object sender, EventArgs e) {
            Debug.WriteLine("Page_Init fire " + DateTime.Now.ToString("HH:mm:ss.ffffff"));

            /*
             * This event fires after each control has been initialized, 
             * each control's UniqueID is set and any skin settings have been applied. 
             * You can use this event to change initialization values for controls. 
             * The “Init” event is fired first for the most bottom control in the hierarchy, 
             * and then fired up the hierarchy until it is fired for the page itself.  
             */

        }//END OF PAGE INITIALIZATION EVENT

        //PAGE INITIALIZATION COMPLETE EVENT
        protected void Page_InitComplete(object sender, EventArgs e) {
            Debug.WriteLine("Page_InitComplete fire " + DateTime.Now.ToString("HH:mm:ss.ffffff"));

            /* Raised by the  Page object. 
             * Use this event for processing tasks that require all initialization be complete. 
             */
        }//END OF PAGE INITIALIZATION COMPLETE EVENT

        //ON PRE LOAD EVENT
        protected override void OnPreLoad(EventArgs e) {
            Debug.WriteLine("OnPreLoad fire " + DateTime.Now.ToString("HH:mm:ss.ffffff"));
            /* Use this event if you need to perform processing on your page or control before the  Load event.
             * Before the Page instance raises this event, it loads view state for itself and all controls, 
             * and then processes any postback data included with the Request instance.
             */
            
            

        }//END OF ON PRE LOAD EVENT

        //PAGE LOAD EVENT
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {               
                //must bind name and fiscal year on page load
                BindDropDown_fiscalYear();
                BindDropDown_name();
                

                //must bind task log totals to account for users logging in and being bound to their name
                BindGridViewTimesheetsDetailTotals();

                
                //disable daily detail panel
                panel_daily_details.Enabled = false;
               
                //register any client validation scripts on controls
                DropDownList_taskType.Attributes.Add("onChange", "validate();");
                TextBox_spinnerB.Attributes.Add("onkeyup", "checkBText();");
                TextBox_spinnerE.Attributes.Add("onkeyup", "checkEText();");

                //check for, and get users information
                if (User.Identity.IsAuthenticated) {
                    string strHostName = System.Net.Dns.GetHostName();//host computer name
                    string clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(1).ToString();//host IP address

                    Label_username.Text = "Welcome:  " + User.Identity.Name;
                    Label_userIP.Text = "Host Name:  " + strHostName + " Host IP address:  " + clientIPAddress;
                    
                    Page.Title = "Ancillary Web Form for:  " + User.Identity.Name;
                }
                else {
                    Label_username.Text = "No user identity available.";
                }//end of user identity information    

                
            }//end of post back check

            
        }//END OF PAGE LOAD EVENT

        //PAGE LOAD COMPLETE EVENT
        protected void Page_LoadComplete(object sender, EventArgs e) {
            Debug.WriteLine("\nPage_LoadComplete fire " + DateTime.Now.ToString("HH:mm:ss.ffffff"));

            /* This event signals the end of Load. 
             * Use this event for tasks that require that all other controls on the page be loaded.
             */

        }//END OF PAGE LOAD COMPLETE EVENT

        //PRE RENDER EVENT
        protected override void OnPreRender(EventArgs e) {
            Debug.WriteLine("OnPreRender fire " + DateTime.Now.ToString("HH:mm:ss.ffffff"));

            /* Each data bound control whose DataSourceID property is set calls its DataBind method.
             * The PreRender event occurs for each control on the page. 
             * Use the event to make final changes to the contents of the page or its controls.
             */

            //WAS NOT FUNCTIONING PROPERLY WHEN CALLED HERE...
            //MOVED TO PAGE LOAD EVENT

            //BindDropDownListData_name();
            //BindDropDown_fiscalYear();
            //BindDropDown_name();

            //SQLDBConnectionStaffInfo();
            SQLDBConnectionMileageInfo();

        }//END OF PRE RENDER EVENT

        //ON SAVE STATE COMPLETE EVENT
        protected override void OnSaveStateComplete(EventArgs e) {
            Debug.WriteLine("OnSaveStateComplete fire " + DateTime.Now.ToString("HH:mm:ss.ffffff"));

            /* Before this event occurs,  ViewState has been saved for the page and for all controls. 
             * Any changes to the page or controls at this point will be ignored.        
             * Use this event perform tasks that require view state to be saved, but that do not make any changes to controls.
             */
        }//END OF ON SAVE STATE COMPLETE EVENT

        //PAGE UN-LOAD EVENT
        protected void Page_Unload(object sender, EventArgs e) {
            Debug.WriteLine("Page_Unload fire " + DateTime.Now.ToString("HH:mm:ss.ffffff"));

            /* This event occurs for each control and then for the page. 
             * In controls, use this event to do final cleanup for specific controls, 
             * such as closing control-specific database connections.        
             * During the unload stage, the page and its controls have been rendered, 
             * so you cannot make further changes to the response stream.           
             * If you attempt to call a method such as the Response.
             * Write method, the page will throw an exception.  
             */
        }//END OF PAGE UNLOAD EVENT



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////GRID VIEW CONTROLS BEING BOUND BY SQL DATA SOURCES
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region testing the removal of this for the function below it
        ////PROVIDES THE DATA STRUCTURE FOR THE TASK LOG
        //private void BindGridViewTimesheetsDetail() {
        //    /*cell order
        //      * 0 = edit
        //      * 1 = TimesheetDetailID
        //      * 2 = TimesheetDetailTotalID
        //      * 3 = FullName
        //      * 4 = ServiceCode 
        //      * 5 = ServiceDate
        //      * 6 = InstitutionName
        //      * 7 = BeginTime
        //      * 8 = BeginTimeAMPM
        //      * 9 = EndTime
        //      * 10 = EndTimeAMPM
        //      * 11 = Location
        //      * 12 = Ancillary
        //      * 13 = DC
        //      * 14 = RS
        //      * 15 = IDT
        //      * 16 = NIDT
        //      * 17 = IDTMiles
        //      * 18 = NIDTMiles
        //      * 19 = Notes
        //      * 20 = Trips
        //      * 21 = delete
        //      * */
            
        //    //connection string
        //    //string connString = System.Configuration.ConfigurationManager.ConnectionStrings["AncillaryConnString"].ToString();           
           
        //    //get the name in the text box do not need to format because this is how it is stored in the database
        //    string name = DropDownList_FullName.Text;
        //    string district = DropDownList_Institution.Text;
        //    string svc_date = TextBox_datepicker.Text;

        //    DataTable myDataTable = new DataTable();
        //    SqlConnection connection = new SqlConnection(connString);
        //    try {
        //        connection.Open();
        //        string sqlStatement = "SELECT * FROM [Sapphire].[dbo].[tblAncillaryTimesheetsDetail] WHERE ([ServiceDate]=@svcDate AND [FullName]=@fullName)AND ([InstitutionName]=@svcDistrict)";
                
        //        SqlCommand sqlCmd = new SqlCommand(sqlStatement, connection);

        //        sqlCmd.Parameters.AddWithValue("@svcDate", svc_date);
        //        sqlCmd.Parameters.AddWithValue("@fullName", name);
        //        sqlCmd.Parameters.AddWithValue("@svcDistrict", district);

        //        SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCmd);

        //        sqlDa.Fill(myDataTable);

        //        if (myDataTable.Rows.Count > 0) {//if data is present show it...bind it
        //            //set gridview to data source
        //            GridView_tasklog.DataSource = myDataTable;

        //            //bind gridview to data source
        //            GridView_tasklog.DataBind();

        //            //total the columns necessary
        //            totalTimesheetDetailColumns(GridView_tasklog, myDataTable);

        //            ////GETS THE COLUMN NAMES FROM THE GRIDVIEW TASK LOG AND FORMATS THE HEADER NAME ACCORDINGLY
        //            reformatColumnNames(GridView_tasklog);


        //            CalculateHoursPaid(GridView_tasklog, myDataTable);


        //        }
        //        else {//else no data present show empty data
        //            GridView_tasklog.DataSource = new DataTable();
        //            GridView_tasklog.DataBind();
        //        }
        //        // NO RECORDS FOUND
        //    }
        //    catch (System.Data.SqlClient.SqlException ex) {
        //        string msg = "Fetch Error:";
        //        msg += ex.Message;
        //        throw new Exception(msg);
        //    }
        //    finally {
        //        connection.Close();
        //    }
            

           

        //}//END OF DATA STRUCTURE TASK LOG
        #endregion

        //PROVIDES THE DATA STRUCTURE FOR THE TASK LOG
        private void BindGridViewTimesheetsDetailTEST(string name, string date, string district, string ancillary) {
            /*cell order
              * 0 = edit
              * 1 = TimesheetDetailID
              * 2 = TimesheetDetailTotalID
              * 3 = FullName
              * 4 = ServiceCode 
              * 5 = ServiceDate
              * 6 = InstitutionName
              * 7 = BeginTime
              * 8 = BeginTimeAMPM
              * 9 = EndTime
              * 10 = EndTimeAMPM
              * 11 = Location
              * 12 = Ancillary
              * 13 = DC
              * 14 = RS
              * 15 = IDT
              * 16 = NIDT
              * 17 = IDTMiles
              * 18 = NIDTMiles
              * 19 = Notes
              * 20 = Trips
              * 21 = delete
              * */
            //PREPARE DATA TABLE FOR DATA
            DataTable myDataTable = new DataTable();

            SqlConnection connection = new SqlConnection(connString);
            try {
                connection.Open();
                string sqlStatement = "SELECT * FROM [Sapphire].[dbo].[tblAncillaryTimesheetsDetail] WHERE ([ServiceDate]=@svcDate AND [FullName]=@fullName) AND ([InstitutionName]=@svcDistrict AND [Ancillary]=@Ancillary)";

                SqlCommand sqlCmd = new SqlCommand(sqlStatement, connection);

                sqlCmd.Parameters.AddWithValue("@svcDate", date);
                sqlCmd.Parameters.AddWithValue("@fullName", name);
                sqlCmd.Parameters.AddWithValue("@svcDistrict", district);
                sqlCmd.Parameters.AddWithValue("@Ancillary", ancillary);

                SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCmd);

                sqlDa.Fill(myDataTable);

                if (myDataTable.Rows.Count > 0) {//if data is present show it...bind it
                    //set gridview to data source
                    GridView_tasklog.DataSource = myDataTable;

                    //bind gridview to data source
                    GridView_tasklog.DataBind();

                    //total the columns necessary
                    totalTimesheetDetailColumns(GridView_tasklog, myDataTable);

                    ////GETS THE COLUMN NAMES FROM THE GRIDVIEW TASK LOG AND FORMATS THE HEADER ACCORDINGLY
                    reformatColumnNames(GridView_tasklog);
                   

                    CalculateHoursPaid(GridView_tasklog, myDataTable);


                }
                else {//else no data present show empty data
                    GridView_tasklog.DataSource = new DataTable();
                    GridView_tasklog.DataBind();
                }
                // NO RECORDS FOUND
            }
            catch (System.Data.SqlClient.SqlException ex) {
                string msg = "Fetch Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            finally {
                connection.Close();
            }




        }//END OF DATA STRUCTURE TASK LOG

        //GETS THE TASK LOG WITH THE CORRESPONDING ID WHEN TOTALS ROW EDITING IS CALLED
        private void BindTaskLogForEditing(int TimesheetDetailTotalID) {

            //update takes care of the task log update
            string queryDetails = "SELECT * FROM [Sapphire].[dbo].[tblAncillaryTimesheetsDetail] WHERE [TimesheetDetailTotalID]=@TimesheetDetailTotalID";

            SqlConnection connectionQuery = new SqlConnection(connString);
            DataTable myDataTable = new DataTable();

            try {//and UPDATE the tblAncillaryTimesheetDetail with the time sheet detail total ID    
                //open connection
                connectionQuery.Open();

                SqlCommand sqlCommand = new SqlCommand(queryDetails, connectionQuery);

                //update based on matching district and date
                sqlCommand.Parameters.AddWithValue("@TimesheetDetailTotalID", TimesheetDetailTotalID);

                SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand);

                sqlDa.Fill(myDataTable);

                if (myDataTable.Rows.Count > 0) {//if data is present show it...bind it
                    //set gridview to data source
                    GridView_tasklog.DataSource = myDataTable;

                    //bind gridview to data source
                    GridView_tasklog.DataBind();


                    //total the columns necessary
                    totalTimesheetDetailColumns(GridView_tasklog, myDataTable);

                    ////GETS THE COLUMN NAMES FROM THE GRIDVIEW TASK LOG AND FORMATS THE HEADER NAME ACCORDINGLY
                    reformatColumnNames(GridView_tasklog);


                    CalculateHoursPaid(GridView_tasklog, myDataTable);

                }
                else {//else no data present show empty data
                    GridView_tasklog.DataSource = new DataTable();
                    GridView_tasklog.DataBind();
                }

                //sqlCommand.ExecuteNonQuery();


            }
            catch (System.Data.SqlClient.SqlException ex) {
                string msg = "Populate Task Log by TimesheetDetailTotalsID: ";
                msg += ex.Message;
                this.ClientScript.RegisterStartupScript(this.GetType(), "Could get Task Log data!", "<script language=\"javaScript\">" + "alert(' " + msg + " ');" + "</script>");
            }
            finally {
                //close connection
                connectionQuery.Close();

            }

        }//END OF EDIT TASK LOG STRUCTURE

        //PROVIDES THE DATA STRUCTURE OF THE EMPLOYEE TASK LOG TOTALS
        private void BindGridViewTimesheetsDetailTotals() {
            /*cell order
             * 0 = edit
             * 1 = TimesheetDetailID
             * 2 = TimesheetDetailTotalID
             * 3 = InstitutionName
             * 4 = Program
             * 5 = PO
             * 6 = ServiceCode
             * 7 = FullName
             * 8 = EntryDate
             * 9 = ServiceDate
             * 10 = Ancillary
             * 11 = DC
             * 12 = RS
             * 13 = IDT
             * 14 = NIDT
             * 15 = OffSitePrep
             * 16 = OnSiteTotal
             * 17 = HoursWorked
             * 18 = HoursPaid
             * 19 = EmployeeTotal
             * 20 = DistrictTotal
             * 21 = Trips
             * 22 = IDTMiles
             * 23 = NIDTMiles
             * 24 = EmployeeIDTMileageTotal
             * 25 = DistrictIDTMileageTotal
             * 26 = EmployeeNIDTMileageTotal
             * 27 = DistrictNIDTMileageTotal
             * 28 = EmployeeTotalMileage
             * 29 = DistrictTotalMileage
             * 30 = PerDiemDays
             * 31 = EmployeePerDiemTotal
             * 32 = DistrictPerDiemTotal
             * 33 = EmployeeTravelTotal
             * 34 = DistrictTravelTotal
             * 35 = EmployeeTimesheetTotal
             * 36 = DistrictTimesheetTotal
             * 37 = edit
             */

            //connection string
            //string connString = System.Configuration.ConfigurationManager.ConnectionStrings["AncillaryConnString"].ToString();

            //get the name in the text box do not need to format because this is how it is stored in the database
            string name = DropDownList_FullName.Text;
            string district = DropDownList_Institution.Text;
            string svc_date = TextBox_datepicker.Text;

            DataTable myDataTable = new DataTable();
            SqlConnection connection = new SqlConnection(connString);

            try {
                connection.Open();
                //string sqlStatement = "SELECT * FROM [Sapphire].[dbo].[tblAncillaryTimesheetsDetailTotals] WHERE ([ServiceDate]=@svcDate AND [FullName]=@fullName)AND ([InstitutionName]=@svcDistrict)";
                string sqlStatement = "SELECT * FROM [Sapphire].[dbo].[tblAncillaryTimesheetsDetailTotals] WHERE ([FullName]=@fullName) ORDER BY [ServiceDate] ASC";
                SqlCommand sqlCmd = new SqlCommand(sqlStatement, connection);

                //sqlCmd.Parameters.AddWithValue("@svcDate", svc_date);
                sqlCmd.Parameters.AddWithValue("@fullName", name);
                //sqlCmd.Parameters.AddWithValue("@svcDistrict", district);

                SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCmd);

                sqlDa.Fill(myDataTable);

                if (myDataTable.Rows.Count > 0) {//if data is present show it...bind it
                    //set gridview to data source
                    GridView_tasklogTotal.DataSource = myDataTable;

                    //bind gridview to data source
                    GridView_tasklogTotal.DataBind();

                    //total the columns necessary
                    totalTimesheetDetailTotalsColumns(GridView_tasklogTotal, myDataTable);

                    ////GETS THE COLUMN NAMES FROM THE GRIDVIEW TASK LOG AND FORMATS THE HEADER NAME ACCORDINGLY
                    reformatColumnNames(GridView_tasklogTotal);

                }
                else {//else no data present show empty data
                    GridView_tasklogTotal.DataSource = new DataTable();
                    GridView_tasklogTotal.DataBind();
                }
                // NO RECORDS FOUND
            }
            catch (System.Data.SqlClient.SqlException ex) {
                string msg = "Fetch Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            finally {
                connection.Close();
            }









        }//END OF DATA STRUCTURE TASKLOG TOTALS




        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////REFORMATTING & TOTALS FOR BOTH GRIDVIEWS
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //USED TO REFORMAT COLUMN NAMES FOR ALL GRID VIEWS
        private void reformatColumnNames(GridView grd) {

            Debug.WriteLine("\n" + "GRID VIEW NAME-->" + grd.ID.ToString());

            String str_grd = grd.ID.ToString();
            
            if (str_grd == "GridView_tasklog") {
                for (int i = 0; i < grd.Columns.Count; i++) {

                    Debug.WriteLine("COLUMN NUMBER-->" + i + " COLUMN NAMES-->" + grd.Columns[i].HeaderText);

                    //USED FOR GRID VIEW TASK LOG
                    if (grd.Columns[i].HeaderText == "TimesheetDetailID") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "TimesheetDetailTotalID") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "FullName") {
                        grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "ServiceCode") {
                        grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "ServiceDate") {
                        grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "InstitutionName") {
                        grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "BeginTime") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "BeginTimeAMPM") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "EndTime") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "EndTimeAMPM") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "Location") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "Ancillary") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "N";                             
                    }
                    else if (grd.Columns[i].HeaderText == "DC") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "RS") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "IDT") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "NIDT") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "IDTMiles") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "NIDTMiles") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "Notes") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }                   
                    else if (grd.Columns[i].HeaderText == "Trips") {
                        grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }//end of if statement - column names
                }//end of for loop

            }
            //USED TO REFORMAT COLUMN NAMES FOR "EMPLOYEE" TOTALS GRIDVIEW
            else if (str_grd == "GridView_tasklogTotal") {
                for (int i = 0; i < grd.Columns.Count; i++) {

                    Debug.WriteLine("COLUMN NUMBER-->" + i + " COLUMN NAMES-->" + grd.Columns[i].HeaderText);

                    if (grd.Columns[i].HeaderText == "TimesheetDetailTotalID") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "Submit") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "InstitutionName") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "Program") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "PO") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "Name";
                    }
                    else if (grd.Columns[i].HeaderText == "ServiceCode") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "FullName") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "Date";
                    }
                    else if (grd.Columns[i].HeaderText == "EntryDate") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "District";
                    }
                    else if (grd.Columns[i].HeaderText == "ServiceDate") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "Ancillary") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "DC") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "RS") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "IDT") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "NIDT") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "OffSitePrep") {
                        grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "OnSiteTotal") {
                        grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "HoursWorked") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "HoursPaid") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "EmployeeTotal") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "DistrictTotal") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "Trips") {
                        grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "IDTMiles") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "NIDTMiles") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "EmployeeIDTMileageTotal") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "DistrictIDTMileageTotal") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "EmployeeNIDTMileageTotal") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "DistrictNIDTMileageTotal") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "EmployeeTotalMileage") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "DistrictTotalMileage") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "PerDiemDays") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "EmployeePerDiemTotal") {
                        //grd.Columns[i].Visible = false;
                    }//grd.Columns[i].HeaderText = "";
                    else if (grd.Columns[i].HeaderText == "DistrictPerDiemTotal") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "EmployeeTravelTotal") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "DistrictTravelTotal") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "EmployeeTimesheetTotal") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "DistrictTimesheetTotal") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "OffSitePrep") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }
                    else if (grd.Columns[i].HeaderText == "OnSiteTotal") {
                        //grd.Columns[i].Visible = false;
                        //grd.Columns[i].HeaderText = "";
                    }//end of if statement
                }//end of for loop

            }//end of if
            
            

        }//END OF REFORMATTING COLUMN NAMES

        //TOTALS THE COLUMNS WITHIN THE TASK LOG
        private void totalTimesheetDetailColumns(GridView grd, DataTable dtbl) {
            /*cell order
               * 0 = edit
               * 1 = TimesheetDetailID
               * 2 = TimesheetDetailTotalID
               * 3 = FullName
               * 4 = ServiceCode 
               * 5 = ServiceDate
               * 6 = InstitutionName
               * 7 = BeginTime
               * 8 = BeginTimeAMPM
               * 9 = EndTime
               * 10 = EndTimeAMPM
               * 11 = Location
               * 12 = Ancillary
               * 13 = DC
               * 14 = RS
               * 15 = IDT
               * 16 = NIDT
               * 17 = IDTMiles
               * 18 = NIDTMiles
               * 19 = Notes
               * 20 = Trips
               * 21 = delete
               * */
            //HANDLE THE TOTALS FOR THE FOOTER ROW IN THE TASK LOG GRID VIEW 
            //grd.FooterRow.Cells[0].Text = "Totals " + GridView_tasklog.Rows.Count.ToString();

            //sum the columns that need to be totaled so that the data can be transfered into the queue
            grd.FooterRow.Cells[13].Text = dtbl.Compute("sum(DC)", "").ToString();
            grd.FooterRow.Cells[14].Text = dtbl.Compute("sum(RS)", "").ToString();
            grd.FooterRow.Cells[15].Text = dtbl.Compute("sum(IDT)", "").ToString();
            grd.FooterRow.Cells[16].Text = dtbl.Compute("sum(NIDT)", "").ToString();
            grd.FooterRow.Cells[17].Text = dtbl.Compute("sum(IDTMiles)", "").ToString();           
            grd.FooterRow.Cells[18].Text = dtbl.Compute("sum(NIDTMiles)", "").ToString();

        }//END OF TOTAL TASK LOG COLUMNS

        //TOTALS THE COLUMNS WITHIN THE TASK LOG TOTALS
        private void totalTimesheetDetailTotalsColumns(GridView grd, DataTable dtbl) {
            /*cell order
              * 0 = edit
             * 1 = TimesheetDetailID
             * 2 = TimesheetDetailTotalID
             * 3 = InstitutionName
             * 4 = Program
             * 5 = PO
             * 6 = ServiceCode
             * 7 = FullName
             * 8 = EntryDate
             * 9 = ServiceDate
             * 10 = Ancillary
             * 11 = DC
             * 12 = RS
             * 13 = IDT
             * 14 = NIDT
             * 15 = OffSitePrep
             * 16 = OnSiteTotal
             * 17 = HoursWorked
             * 18 = HoursPaid
             * 19 = EmployeeTotal
             * 20 = DistrictTotal
             * 21 = Trips
             * 22 = IDTMiles
             * 23 = NIDTMiles
             * 24 = EmployeeIDTMileageTotal
             * 25 = DistrictIDTMileageTotal
             * 26 = EmployeeNIDTMileageTotal
             * 27 = DistrictNIDTMileageTotal
             * 28 = EmployeeTotalMileage
             * 29 = DistrictTotalMileage
             * 30 = PerDiemDays
             * 31 = EmployeePerDiemTotal
             * 32 = DistrictPerDiemTotal
             * 33 = EmployeeTravelTotal
             * 34 = DistrictTravelTotal
             * 35 = EmployeeTimesheetTotal
             * 36 = DistrictTimesheetTotal
             * 37 = edit
             */

            //HANDLE THE TOTALS FOR THE FOOTER ROW IN THE TASK LOG GRID VIEW 
            //grd.FooterRow.Cells[0].Text = "Totals " + GridView_tasklog.Rows.Count.ToString();

            //sum the columns that need to be totaled so that the data can be transfered into the queue
            grd.FooterRow.Cells[11].Text = dtbl.Compute("sum(DC)", "").ToString();
            grd.FooterRow.Cells[12].Text = dtbl.Compute("sum(RS)", "").ToString();
            grd.FooterRow.Cells[13].Text = dtbl.Compute("sum(IDT)", "").ToString();
            grd.FooterRow.Cells[14].Text = dtbl.Compute("sum(NIDT)", "").ToString();
            grd.FooterRow.Cells[17].Text = dtbl.Compute("sum(HoursWorked)", "").ToString();

            grd.FooterRow.Cells[22].Text = dtbl.Compute("sum(IDTMiles)", "").ToString();
            grd.FooterRow.Cells[23].Text = dtbl.Compute("sum(NIDTMiles)", "").ToString();
            //add any remaining columns to be totaled here

        }//END OF TOTAL TOTALS COLUMNS





        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////TASK LOG GRID VIEW CONTROLS
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //USED FOR HIGHLIGHTING THE ROW THAT CONTAINS LUNCH AND NIDT TIME
        protected void GridView_tasklog_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {

                if (e.Row.RowType == DataControlRowType.DataRow) {
                   

                    //DropDownList ddll = (DropDownList)e.Row.FindControl("DropDownList1");
                    //BindGridView_DDL_location(ddll);
                    
                    if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Location")) == "Lunch") {
                        e.Row.BackColor = System.Drawing.Color.LightPink;
                    }
                    else if (Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "NIDT")) > 0) {
                        e.Row.BackColor = System.Drawing.Color.Coral;

                    }                        

                }
               
            }
            catch (Exception ex) {
                //ErrorLabel.Text = ex.Message;
            }           
           
        }//END OF TASK LOG ROW DATA BOUND

        //CALLED WHEN AN EDIT BUTTON IS PRESSED IN A ROW IN THE TASK LOG 
        protected void GridView_tasklog_RowEditing(object sender, GridViewEditEventArgs e) {
            Debug.WriteLine("\n\n\nEdit Row Button has been pressed");
            BindDropDown_location();
            /*cell order
              * 0 = edit
              * 1 = TimesheetDetailID
              * 2 = TimesheetDetailTotalID
              * 3 = FullName
              * 4 = ServiceCode 
              * 5 = ServiceDate
              * 6 = InstitutionName
              * 7 = BeginTime
              * 8 = BeginTimeAMPM
              * 9 = EndTime
              * 10 = EndTimeAMPM
              * 11 = Location
              * 12 = Ancillary
              * 13 = DC
              * 14 = RS
              * 15 = IDT
              * 16 = NIDT
              * 17 = IDTMiles
              * 18 = NIDTMiles
              * 19 = Notes
              * 20 = Trips
              * 21 = delete
              * */
            
            Button_addLine.Visible = false;
            Button_addLine.Enabled = false;
           

            Button_changeInfo.Visible = false;

           

            //PREPARE DATATABLE
            //DataTable myDataTable = (DataTable)ViewState["dtTasklog"];
            int TimesheetDetailTotalID = 0;


            try {
                TimesheetDetailTotalID = int.Parse(GridView_tasklog.Rows[e.NewEditIndex].Cells[2].Text);
            }
            catch (Exception ex) {

                this.ClientScript.RegisterStartupScript(this.GetType(), "Missing Detail Total ID", "<script language=\"javaScript\">" + "alert('This log does not have a TimesheetDetailTotalID yet " + ex.ToString() + "');" + "</script>");
            }
            finally {
                //this.ClientScript.RegisterStartupScript(this.GetType(), "Has Detail Total ID", "<script language=\"javaScript\">" + "alert('This log has a TimesheetDetailTotalID');" + "</script>");
            }
            

            //GridView_tasklog.DataSource = myDataTable;

            //SETS A NEW INDEX
            GridView_tasklog.EditIndex = e.NewEditIndex;

            //GET THE GRID VIEW ROW BEING EDITED
            GridViewRow gvr = GridView_tasklog.Rows[e.NewEditIndex];

            //NEED TO REBIND HERE IN ORDER TO PREVENT THE DOUBLE CLICK EDIT SCENARIO
            
            if (TimesheetDetailTotalID > 0) {
                BindTaskLogForEditing(TimesheetDetailTotalID);
            }
            else {
                //BindGridViewTimesheetsDetail();

                string name = DropDownList_FullName.Text;
                string date = TextBox_datepicker.Text;
                string district = DropDownList_Institution.Text;
                string ancillary = DropDownList_ancillary.Text;

                BindGridViewTimesheetsDetailTEST(name, date, district, ancillary);
            }

           

            //TextBox notes = (TextBox)GridView_tasklog.Rows[e.NewEditIndex].FindControl("TextBox6");
            //ImageButton print = (ImageButton)GridView_tasklog.FooterRow.FindControl("btnPrintLog");
            //Button_PrintLog.Visible = false;


            double begin = Double.Parse(gvr.Cells[7].Text);
            String beginAMPM = gvr.Cells[8].Text;

            double end = Double.Parse(gvr.Cells[9].Text);
            String endAMPM = gvr.Cells[10].Text;

            string loc = gvr.Cells[11].Text.Replace("&nbsp;", "");
            string notes = gvr.Cells[19].Text;
      
            //non ancillary            
            string strAncillary = gvr.Cells[12].Text;

            if (strAncillary == "Yes") {                
                DropDownList_ancillary.SelectedValue = "Yes";
            }
            else {
                DropDownList_ancillary.SelectedValue = "No";
            }     
            

            double dc = Double.Parse(gvr.Cells[13].Text.Replace("&nbsp;", "0.0"));
            double rs = Double.Parse(gvr.Cells[14].Text.Replace("&nbsp;", "0.0"));
            double idt = Double.Parse(gvr.Cells[15].Text.Replace("&nbsp;", "0.0"));
            double nidt = Double.Parse(gvr.Cells[16].Text.Replace("&nbsp;", "0.0"));
            double idtMi = Double.Parse(gvr.Cells[17].Text.Replace("&nbsp;", "0"));            
            double nidtMi = Double.Parse(gvr.Cells[18].Text.Replace("&nbsp;", "0"));     

            //repopulate task type drop down list
            if(dc > 0){//if there is time in the dc column
                DropDownList_taskType.SelectedValue = "DC";
                TextBox_miles.Enabled = false;

            }else if(rs > 0){//if there is time in the rs column
                DropDownList_taskType.SelectedValue = "RS";
                TextBox_miles.Enabled = false;

            }else if (idt > 0){//if there is time in the idt column...
                DropDownList_taskType.SelectedValue = "IDT";

                if (idtMi > 0) {//and there are miles
                    TextBox_miles.Text = idtMi.ToString();
                }

                TextBox_miles.Enabled = true;
                //CheckBox_nonAncillary.Enabled = false;

            }else if(nidt > 0){//if there is nidt time in the nidt column...
                DropDownList_taskType.SelectedValue = "NIDT";

                if (nidtMi > 0) {//and there are miles
                    TextBox_miles.Text = nidtMi.ToString();
                }
                TextBox_miles.Enabled = true;
                //CheckBox_nonAncillary.Enabled = false;
            }//end of repopulate task type drop down list
           

            //Debug.WriteLine("\n\n\nBreakPt Test--> ");

            //SET THE BEGIN AND END TIME SPINNERS TO THE VALUES IN THE GRID VIEW
            TextBox_spinnerB.Text = begin.ToString();
            if (beginAMPM == "True") {
                radListBegin.SelectedValue = "1";
            }
            else {
                radListBegin.SelectedValue = "0";
            }


            TextBox_spinnerE.Text = end.ToString();
            if (endAMPM == "True") {
                radListEnd.SelectedValue = "1";
            }
            else {
                radListEnd.SelectedValue = "0";
            }

           
            //check for an empty location cell and populate drop down accordingly
            if (loc == "" || loc == "Lunch") {
                DropDownList_Location.SelectedValue = "Select Location...";
                if (loc == "Lunch") {
                    DropDownList_taskType.SelectedValue = "LB";
                }
            }
            else {
                DropDownList_Location.SelectedValue = loc;
            }

            //notes
            TextBox_notes.Text = notes;

            

        }//END OF TASK LOG ROW EDITING

        //CALLED WHEN THE UPDATE BUTTON IS PRESSED IN THE TASK LOG (CALLED AFTER EDIT BUTTON PRESS)
        protected void GridView_tasklog_RowUpdating(object sender, GridViewUpdateEventArgs e) {
            /*cell order
              * 0 = edit
              * 1 = TimesheetDetailID
              * 2 = TimesheetDetailTotalID
              * 3 = FullName
              * 4 = ServiceCode 
              * 5 = ServiceDate
              * 6 = InstitutionName
              * 7 = BeginTime
              * 8 = BeginTimeAMPM
              * 9 = EndTime
              * 10 = EndTimeAMPM
              * 11 = Location
              * 12 = Ancillary
              * 13 = DC
              * 14 = RS
              * 15 = IDT
              * 16 = NIDT
              * 17 = IDTMiles
              * 18 = NIDTMiles
              * 19 = Notes
              * 20 = Trips
              * 21 = delete
              * */
            panel_daily_details.Enabled = true;

            Button_addLine.Visible = true;
            Button_addLine.Enabled = true;


             //USED FOR CALCULATING TOTAL HOURS WORKED 
            //begin and end time am/pm values 
            String strBeginAmPm = radListBegin.SelectedValue;
            String strEndAmPm = radListEnd.SelectedValue;

            //check data stored in hidden fields for access purposes
            Debug.WriteLine("HIDDEN DATA VALUE BEGIN --->" + strBeginAmPm);
            Debug.WriteLine("HIDDEN DATA VALUE END--->" + strEndAmPm);

            //bTime IS BEGIN TIME
            //eTime IS END TIME
            double bTime = Double.Parse(TextBox_spinnerB.Text);
            double eTime = Double.Parse(TextBox_spinnerE.Text);

            //calculate the hours worked
            double hoursWorked = CalculateHoursWorked(strBeginAmPm, strEndAmPm, bTime, eTime);


             //get TimesheetDetailID
            int TimesheetDetailID = int.Parse(GridView_tasklog.Rows[e.RowIndex].Cells[1].Text);
            int TimesheetDetailTotalID = int.Parse(GridView_tasklog.Rows[e.RowIndex].Cells[2].Text);
            


            //connection string
            //string connString = System.Configuration.ConfigurationManager.ConnectionStrings["AncillaryConnString"].ToString(); 


            DataTable myDataTable = new DataTable();
            SqlConnection connection = new SqlConnection(connString);


            try {
                connection.Open();
                string sqlStatement = "UPDATE [Sapphire].[dbo].[tblAncillaryTimesheetsDetail] SET [BeginTime]=@BeginTime, [BeginTimeAMPM]=@BeginTimeAMPM, [EndTime]=@EndTime, [EndTimeAMPM]=@EndTimeAMPM, [Location]=@Location, [Ancillary]=@Ancillary, [DC]=@DC, [RS]=@RS, [IDT]=@IDT, [NIDT]=@NIDT, [IDTMiles]=@IDTMiles, [NIDTMiles]=@NIDTMiles, [Notes]=@Notes, [Trips]=@trips WHERE [TimesheetDetailID]=@TimesheetDetailID";

                SqlCommand sqlCmd = new SqlCommand(sqlStatement, connection);

                sqlCmd.Parameters.AddWithValue("@TimesheetDetailID", TimesheetDetailID);



                sqlCmd.Parameters.AddWithValue("@BeginTime", TextBox_spinnerB.Text);
                sqlCmd.Parameters.AddWithValue("@BeginTimeAMPM", radListBegin.SelectedValue);

                sqlCmd.Parameters.AddWithValue("@EndTime", TextBox_spinnerE.Text);
                sqlCmd.Parameters.AddWithValue("@EndTimeAMPM", radListEnd.SelectedValue);

                sqlCmd.Parameters.AddWithValue("@Ancillary", DropDownList_ancillary.Text);




                //INSERT TIME WORKED AND PLACE IT IN THE COLUMN
                //ACCORDING TO THE TASK TYPE SELECTED DROP DOWN LIST
                if (DropDownList_taskType.Text == "DC") {//DC DROP DOWN SELECTION
                    sqlCmd.Parameters.AddWithValue("@DC", Math.Round(hoursWorked, 2));

                    sqlCmd.Parameters.AddWithValue("@RS", 0);
                    sqlCmd.Parameters.AddWithValue("@IDT", 0);
                    sqlCmd.Parameters.AddWithValue("@IDTMiles", 0);
                    sqlCmd.Parameters.AddWithValue("@NIDT", 0);
                    sqlCmd.Parameters.AddWithValue("@NIDTMiles", 0);
                    if (DropDownList_Location.Text == "Select Location...") {
                        sqlCmd.Parameters.AddWithValue("@Location", "");
                    }
                    else {
                        sqlCmd.Parameters.AddWithValue("@Location", DropDownList_Location.Text);
                    }

                }
                else if (DropDownList_taskType.Text == "RS") {//RS DROP DOWN SELECTION
                    sqlCmd.Parameters.AddWithValue("@RS", Math.Round(hoursWorked, 2));

                    sqlCmd.Parameters.AddWithValue("@DC", 0);
                    sqlCmd.Parameters.AddWithValue("@IDT", 0);
                    sqlCmd.Parameters.AddWithValue("@IDTMiles", 0);
                    sqlCmd.Parameters.AddWithValue("@NIDT", 0);
                    sqlCmd.Parameters.AddWithValue("@NIDTMiles", 0);

                    if (DropDownList_Location.Text == "Select Location...") {
                        sqlCmd.Parameters.AddWithValue("@Location", "");
                    }
                    else {
                        sqlCmd.Parameters.AddWithValue("@Location", DropDownList_Location.Text);
                    }


                }else if (DropDownList_taskType.Text == "LB") {

                    sqlCmd.Parameters.AddWithValue("@Location", "Lunch");

                    sqlCmd.Parameters.AddWithValue("@DC", 0);
                    sqlCmd.Parameters.AddWithValue("@RS", 0);
                    sqlCmd.Parameters.AddWithValue("@IDT", 0);
                    sqlCmd.Parameters.AddWithValue("@IDTMiles", 0);
                    sqlCmd.Parameters.AddWithValue("@NIDT", 0);
                    sqlCmd.Parameters.AddWithValue("@NIDTMiles", 0);
                }
                else if (DropDownList_taskType.Text == "IDT") {//IDT DROP DOWN SELECTION
                    sqlCmd.Parameters.AddWithValue("@IDT", Math.Round(hoursWorked, 2));

                    sqlCmd.Parameters.AddWithValue("@DC", 0);
                    sqlCmd.Parameters.AddWithValue("@RS", 0);
                    sqlCmd.Parameters.AddWithValue("@NIDT", 0);
                    sqlCmd.Parameters.AddWithValue("@NIDTMiles", 0);

                    if (TextBox_miles.Text == "") {
                        sqlCmd.Parameters.AddWithValue("@IDTMiles", 0);//IF MILES ARE BLANK
                    }
                    else {
                        sqlCmd.Parameters.AddWithValue("@IDTMiles", Int32.Parse(TextBox_miles.Text));//INSERT IDT MILES
                    }


                    if (DropDownList_Location.Text == "Select Location...") {
                        sqlCmd.Parameters.AddWithValue("@Location", "");
                    }
                    else {
                        sqlCmd.Parameters.AddWithValue("@Location", DropDownList_Location.Text);
                    }

                }
                else if (DropDownList_taskType.Text == "NIDT") {//NIDT DROP DOWN SELECTION
                    sqlCmd.Parameters.AddWithValue("@NIDT", Math.Round(hoursWorked, 2));

                    sqlCmd.Parameters.AddWithValue("@DC", 0);
                    sqlCmd.Parameters.AddWithValue("@RS", 0);
                    sqlCmd.Parameters.AddWithValue("@IDT", 0);
                    sqlCmd.Parameters.AddWithValue("@IDTMiles", 0);


                    if (TextBox_miles.Text == "") {
                        sqlCmd.Parameters.AddWithValue("@NIDTMiles", 0);//IF MILES ARE BLANK
                    }
                    else
                        sqlCmd.Parameters.AddWithValue("@NIDTMiles", Int32.Parse(TextBox_miles.Text));//INSERT NIDT MILES


                    if (DropDownList_Location.Text == "Select Location...") {
                        sqlCmd.Parameters.AddWithValue("@Location", "");
                    }
                    else {
                        sqlCmd.Parameters.AddWithValue("@Location", DropDownList_Location.Text);
                    }
                }

                //INSERT ANY NOTES BY COLUMN NAME
                sqlCmd.Parameters.AddWithValue("@Notes", TextBox_notes.Text);

                sqlCmd.Parameters.AddWithValue("@Trips", 0);


                sqlCmd.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex) {
                string msg = "Task Log UPDATE Error:";
                msg += ex.Message;
                this.ClientScript.RegisterStartupScript(this.GetType(), "Could NOT update!", "<script language=\"javaScript\">" + "alert('Could NOT update the information!');" + "</script>");
                throw new Exception(msg);                
            }
            finally {
                connection.Close();
                this.ClientScript.RegisterStartupScript(this.GetType(), "Update Successful!", "<script language=\"javaScript\">" + "alert('You have successfully updated the information!');" + "</script>");
            }     
           
                       
            ////GET THE INDEX OF THE ROW WANTING TO UPDATE - THE EFFECT IS THAT IT CONFIRMS THE CHANGES
            GridView_tasklog.EditIndex = -1;


            if (TimesheetDetailTotalID > 0) {
                BindTaskLogForEditing(TimesheetDetailTotalID);
            }
            else {
                //BindGridViewTimesheetsDetail();

                string name = DropDownList_FullName.Text;
                string date = TextBox_datepicker.Text;
                string district = DropDownList_Institution.Text;
                string ancillary = DropDownList_ancillary.Text;

                BindGridViewTimesheetsDetailTEST(name, date, district, ancillary);
            }

            //REFORMATS DAILY DETAILS CONTROLS FOR NEW LINE
            PrepareControlsForNewLine();
            
            Button_PrintLog.Visible = true;

            UDP_GridViewTimesheetDetails.Update();

        }//END OF TASK LOG ROW UPDATING
       
        //CALLED WHEN THE CANCEL BUTTON IS PRESSED IN THE TASK LOG
        protected void GridView_tasklog_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) {
            Button_addLine.Visible = true;
            Button_addLine.Enabled = true;
            //Button_changeInfo.Visible = true;

            //ResetDailyDetailsControls();
            ////PREPARE DATATABLE
            //DataTable myDataTable = (DataTable)ViewState["dtTasklog"];

            //GridView_tasklog.DataSource = myDataTable;
            GridView_tasklog.EditIndex = -1;

            int TimesheetDetailTotalID = 0;

            try {
                TimesheetDetailTotalID = int.Parse(GridView_tasklog.Rows[e.RowIndex].Cells[2].Text);
            }
            catch (Exception ex) {

            }
            finally {

            }

            if (TimesheetDetailTotalID > 0) {
                BindTaskLogForEditing(TimesheetDetailTotalID);
                
            }
            else {
                //BindGridViewTimesheetsDetail();

                string name = DropDownList_FullName.Text;
                string date = TextBox_datepicker.Text;
                string district = DropDownList_Institution.Text;
                string ancillary = DropDownList_ancillary.Text;

                BindGridViewTimesheetsDetailTEST(name, date, district, ancillary);
               
            }
           

            GridView_tasklog.EditIndex = -1;

           

            panel_daily_details.Enabled = true;

            //RETOTAL TASK LOG COLUMNS
            //totalTimesheetDetailColumns(GridView_tasklog, myDataTable);

            BindDropDown_location();

        }//END OF TASK LOG ROW CANCELLING EDIT

        //CALLED WHEN THE DELETE BUTTON IS PRESSED IN THE TASK LOG - DELETES ONE LINE ITEM
        protected void GridView_tasklog_RowDeleting(object sender, GridViewDeleteEventArgs e) {   


                //connection string
                //string connString = System.Configuration.ConfigurationManager.ConnectionStrings["AncillaryConnString"].ToString();

                //get TimesheetDetailID
                int TimesheetDetailID = int.Parse(GridView_tasklog.Rows[e.RowIndex].Cells[1].Text);

                //SQL DELETE statement
                string statement = "DELETE FROM[Sapphire].[dbo].[tblAncillaryTimesheetsDetail] WHERE [TimesheetDetailID]=@TimesheetDetailID";
                
                //prepare connection
                SqlConnection connection = new SqlConnection(connString);                
                
                try {//to delete some data
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(statement, connection);
                    cmd.Parameters.AddWithValue("@TimesheetDetailID", TimesheetDetailID);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                }
                catch (System.Data.SqlClient.SqlException ex) {//catch any exceptions
                    string msg = "Delete Task Log line Error:";
                    msg += ex.Message;
                    throw new Exception(msg);
                }
                finally {//finally dispaly message to user and close connection
                    connection.Close();
                    //this.ClientScript.RegisterStartupScript(this.GetType(), "Successful", "<script language=\"javaScript\">" + "alert('Line item successfully deleted!');" + "</script>");
                }
                

                //BindGridViewTimesheetsDetail();

                string name = DropDownList_FullName.Text;
                string date = TextBox_datepicker.Text;
                string district = DropDownList_Institution.Text;
                string ancillary = DropDownList_ancillary.Text;

                BindGridViewTimesheetsDetailTEST(name, date, district, ancillary);
                //GridView_tasklog.DataSource = dt;
                //GridView_tasklog.DataBind();

                //show or hide task log visibility if user deletes all rows
                if (GridView_tasklog.Rows.Count > 0) {
                    GridView_tasklog.Visible = true;
                    Button_logComplete.Visible = true;
                }
                else {
                    //GridView_tasklog.Visible = false;
                    //Button_changeInfo.Enabled = true;//ENABLE BUTTON
                    //Button_changeInfo.Visible = true;//SHOW BUTTON
                    panel_daily_details.Enabled = false;
                    panel_information.Enabled = true;
                    Button_logComplete.Visible = false;
                    //...BETTER TO BE SAFE THAN SORRY
                    
                    
                    ResetDailyDetailsControls();
                }
                UDP_GridViewTimesheetDetails.Update();

        }//END OF GRIDVIEW TASK LOG ROW DELETING




        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////TASK LOG TOTALS GRID VIEW CONTROLS
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //USED FOR HIGHLIGHTING THE ROW 
        protected void GridView_tasklogTotals_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {

                if (e.Row.RowType == DataControlRowType.DataRow) {


                    //DropDownList ddll = (DropDownList)e.Row.FindControl("DropDownList1");
                    //BindGridView_DDL_location(ddll);

                    //if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Location")) == "Lunch") {
                    //    e.Row.BackColor = System.Drawing.Color.LightPink;
                    //}
                    //else if (Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "NIDT")) > 0) {
                    //    e.Row.BackColor = System.Drawing.Color.Coral;

                    //}

                }

            }
            catch (Exception ex) {
                //ErrorLabel.Text = ex.Message;
            }

        }//END OF TASK LOG TOTALS ROW DATA BOUND

        //CALLED WHEN AN EDIT BUTTON IS PRESSED IN A ROW IN THE TASK LOG TOTALS
        protected void GridView_tasklogTotals_RowEditing(object sender, GridViewEditEventArgs e) {
            Debug.WriteLine("\n\n\nEdit Task Log Totals Row Button has been pressed");
            /*cell order
             * 0 = edit
             * 1 = TimesheetDetailID
             * 2 = TimesheetDetailTotalID
             * 3 = InstitutionName
             * 4 = Program
             * 5 = PO
             * 6 = ServiceCode
             * 7 = FullName
             * 8 = EntryDate
             * 9 = ServiceDate
             * 10 = Ancillary
             * 11 = DC
             * 12 = RS
             * 13 = IDT
             * 14 = NIDT
             * 15 = OffSitePrep
             * 16 = OnSiteTotal
             * 17 = HoursWorked
             * 18 = HoursPaid
             * 19 = EmployeeTotal
             * 20 = DistrictTotal
             * 21 = Trips
             * 22 = IDTMiles
             * 23 = NIDTMiles
             * 24 = EmployeeIDTMileageTotal
             * 25 = DistrictIDTMileageTotal
             * 26 = EmployeeNIDTMileageTotal
             * 27 = DistrictNIDTMileageTotal
             * 28 = EmployeeTotalMileage
             * 29 = DistrictTotalMileage
             * 30 = PerDiemDays
             * 31 = EmployeePerDiemTotal
             * 32 = DistrictPerDiemTotal
             * 33 = EmployeeTravelTotal
             * 34 = DistrictTravelTotal
             * 35 = EmployeeTimesheetTotal
             * 36 = DistrictTimesheetTotal
             * 37 = edit
             */
            string ancillary = GridView_tasklogTotal.Rows[e.NewEditIndex].Cells[10].Text.ToString();

            GridView_tasklog.Visible = true;
            Button_PrintLog.Visible = true;
            panel_information.Enabled = false;
            panel_daily_details.Enabled = true;
            Button_logComplete.Visible = false;

            TextBox_datepicker.Text = GridView_tasklogTotal.Rows[e.NewEditIndex].Cells[9].Text.ToString();
            
            DropDownList_ancillary.SelectedValue = ancillary;
            if (ancillary == "No") {
                ancillary = " **Non-Ancillary Time Only** ";
            }
            else {
                ancillary = " **Ancillary Time Only** ";
            }

            //reference tblAncillaryTimesheetsDetailTotals for the TimesheetDetailTotalID and...
            //populate the gridview task log with those contents based on corresponding TimesheetDetailTotalID 
            int TimesheetDetailTotalID = int.Parse(GridView_tasklogTotal.Rows[e.NewEditIndex].Cells[1].Text);

            //set the labels accordingly
            Label_timesheet_name.Text = GridView_tasklogTotal.Rows[e.NewEditIndex].Cells[7].Text.ToString() + ", " + GridView_tasklogTotal.Rows[e.NewEditIndex].Cells[6].Text.ToString();
            Label_timesheet_date.Text = GridView_tasklogTotal.Rows[e.NewEditIndex].Cells[9].Text.ToString() + ancillary;
            Label_timesheet_district.Text = GridView_tasklogTotal.Rows[e.NewEditIndex].Cells[3].Text.ToString();

            BindTaskLogForEditing(TimesheetDetailTotalID);

            //SETS A NEW INDEX
            GridView_tasklogTotal.EditIndex = e.NewEditIndex;

            //NEED TO REBIND HERE IN ORDER TO PREVENT THE DOUBLE CLICK EDIT SCENARIO
            BindGridViewTimesheetsDetailTotals();
            UDP_GridViewTimesheetDetails.Update();
            

        }//END OF TASK LOG TOTALS ROW EDITING       

        //CALLED WHEN THE UPDATE BUTTON IS PRESSED IN THE TASK LOG (CALLED AFTER EDIT BUTTON PRESS)
        protected void GridView_tasklogTotals_RowUpdating(object sender, GridViewUpdateEventArgs e) {
            /*cell order of GridView_totals
              * 0 = edit
              * 1 = TimesheetDetailTotalID
              * 2 = ********************************* submit
              * 3 = InstitutionName
              * 4 = Program
              * 5 = ServiceCode
              * 6 = FullName
              * 7 = EntryDate
              * 8 = ServiceDate
              * 9 = Ancillary
              * 10 = DC
              * 11 = RS
              * 12 = IDT
              * 13 = NIDT
              * 14 = OffSitePrep
              * 15 = OnSiteTotal
              * 16 = HoursWorked
              * 17 = HoursPaid
              * 18 = EmployeeTotal
              * 19 = DistrictTotal
              * 20 = Trips
              * 21 = IDTMiles
              * 22 = NIDTMiles
              * 23 = EmployeeIDTMileageTotal
              * 24 = DistrictIDTMileageTotal
              * 25 = EmployeeNIDTMileageTotal
              * 26 = DistrictNIDTMileageTotal
              * 27 = EmployeeTotalMileage
              * 28 = DistrictTotalMileage
              * 29 = PerDiemDays
              * 30 = EmployeePerDiemTotal
              * 31 = DistrictPerDiemTotal
              * 32 = EmployeeTravelTotal
              * 33 = DistrictTravelTotal
              * 34 = EmployeeTimesheetTotal
              * 35 = DistrictTimesheetTotal
              * 36 = edit
              */
            //there will always be an ID at this point in the web form so use it to update any necessary records with the same ID
            int TimesheetDetailTotalID = int.Parse(GridView_tasklogTotal.Rows[e.RowIndex].Cells[1].Text);

            string name = DropDownList_FullName.Text;
            string date = TextBox_datepicker.Text;
            string district = DropDownList_Institution.Text;
            string ancillary = DropDownList_ancillary.Text;



            //try to update the tblAncillaryTimesheetsDetailTotals with new tblAncillaryTImesheetsDetail data 
            SqlConnection connection = new SqlConnection(connString);                                                                                                                                                                                                                                                                                                                                                   
            try {                
                connection.Open();

                string sqlStatement = "UPDATE [Sapphire].[dbo].[tblAncillaryTimesheetsDetailTotals] SET [InstitutionName]=@InstitutionName, [Program]=@Program, [ServiceCode]=@ServiceCode, [FullName]=@FullName, [EntryDate]=@EntryDate, [ServiceDate]=@ServiceDate, [Ancillary]=@Ancillary, [DC]=@DC, [RS]=@RS, [IDT]=@IDT, [NIDT]=@NIDT, [OffSitePrep]=@OffSitePrep, [OnSiteTotal]=@OnSiteTotal, [HoursWorked]=@HoursWorked, [HoursPaid]=@HoursPaid, [EmployeeTotal]=@EmployeeTotal, [DistrictTotal]=@DistrictTotal, [Trips]=@Trips, [IDTMiles]=@IDTMiles, [NIDTMiles]=@NIDTMiles, [EmployeeIDTMileageTotal]=@EmployeeIDTMileageTotal, [DistrictIDTMileageTotal]=@DistrictIDTMileageTotal, [EmployeeNIDTMileageTotal]=@EmployeeNIDTMileageTotal, [DistrictNIDTMileageTotal]=@DistrictNIDTMileageTotal, [EmployeeTotalMileage]=@EmployeeTotalMileage, [DistrictTotalMileage]=@DistrictTotalMileage, [PerDiemDays]=@PerDiemDays, [EmployeePerDiemTotal]=@EmployeePerDiemTotal, [DistrictPerDiemTotal]=@DistrictPerDiemTotal, [EmployeeTravelTotal]=@EmployeeTravelTotal, [DistrictTravelTotal]=@DistrictTravelTotal, [EmployeeTimesheetTotal]=@EmployeeTimesheetTotal, [DistrictTimesheetTotal]=@DistrictTimesheetTotal WHERE [TimesheetDetailTotalID]=@TimesheetDetailTotalID";

                SqlCommand dataCommand = new SqlCommand(sqlStatement, connection);

                dataCommand.Parameters.AddWithValue("@TimesheetDetailTotalID", TimesheetDetailTotalID);


                dataCommand.Parameters.AddWithValue("InstitutionName", DropDownList_Institution.Text);
                dataCommand.Parameters.AddWithValue("Program", DropDownList_Program.Text);
                dataCommand.Parameters.AddWithValue("ServiceCode", DropDownList_Position.Text);
                dataCommand.Parameters.AddWithValue("FullName", DropDownList_FullName.Text);
                dataCommand.Parameters.AddWithValue("EntryDate", DateTime.Now);
                dataCommand.Parameters.AddWithValue("ServiceDate", TextBox_datepicker.Text);
                dataCommand.Parameters.AddWithValue("Ancillary", DropDownList_ancillary.Text);


               
                double hoursW = 0.0;

                double dc =  Double.Parse(GridView_tasklog.FooterRow.Cells[13].Text);
                double rs = Double.Parse(GridView_tasklog.FooterRow.Cells[14].Text);
                double idt = Double.Parse(GridView_tasklog.FooterRow.Cells[15].Text);
                double nidt = Double.Parse(GridView_tasklog.FooterRow.Cells[16].Text);



                hoursW = (dc + rs) + (idt + nidt);


                if (GridView_tasklog.FooterRow.Cells[13].Text.Length == 0) {
                    dataCommand.Parameters.AddWithValue("DC", 0);
                }
                else {
                    dataCommand.Parameters.AddWithValue("DC", GridView_tasklog.FooterRow.Cells[13].Text);
                }


                if (GridView_tasklog.FooterRow.Cells[14].Text.Length == 0) {
                    dataCommand.Parameters.AddWithValue("RS", 0);
                }
                else {
                    dataCommand.Parameters.AddWithValue("RS", GridView_tasklog.FooterRow.Cells[14].Text);
                }


                if (GridView_tasklog.FooterRow.Cells[15].Text.Length == 0) {
                    dataCommand.Parameters.AddWithValue("IDT", 0);
                }
                else {
                    dataCommand.Parameters.AddWithValue("IDT", GridView_tasklog.FooterRow.Cells[15].Text);
                }


                if (GridView_tasklog.FooterRow.Cells[16].Text.Length == 0) {
                    dataCommand.Parameters.AddWithValue("NIDT", 0);
                }
                else {
                    dataCommand.Parameters.AddWithValue("NIDT", GridView_tasklog.FooterRow.Cells[16].Text);
                }

                dataCommand.Parameters.AddWithValue("OffSitePrep", 0);
                dataCommand.Parameters.AddWithValue("OnSiteTotal", 0);

                dataCommand.Parameters.AddWithValue("HoursWorked", hoursW);
                dataCommand.Parameters.AddWithValue("HoursPaid", hoursW);

                dataCommand.Parameters.AddWithValue("EmployeeTotal", 0);
                dataCommand.Parameters.AddWithValue("DistrictTotal", 0);

                dataCommand.Parameters.AddWithValue("Trips", 0);

                if (GridView_tasklog.FooterRow.Cells[14].Text == "0.00") {
                    dataCommand.Parameters.AddWithValue("IDTMiles", 0);
                }
                else {
                    dataCommand.Parameters.AddWithValue("IDTMiles", GridView_tasklog.FooterRow.Cells[16].Text);
                }


                if (GridView_tasklog.FooterRow.Cells[17].Text == "0.00") {
                    dataCommand.Parameters.AddWithValue("NIDTMiles", 0);
                }
                else {
                    dataCommand.Parameters.AddWithValue("NIDTMiles", GridView_tasklog.FooterRow.Cells[17].Text);
                }

                dataCommand.Parameters.AddWithValue("EmployeeIDTMileageTotal", 0);
                dataCommand.Parameters.AddWithValue("DistrictIDTMileageTotal", 0);

                dataCommand.Parameters.AddWithValue("EmployeeNIDTMileageTotal", 0);
                dataCommand.Parameters.AddWithValue("DistrictNIDTMileageTotal", 0);

                dataCommand.Parameters.AddWithValue("EmployeeTotalMileage", 0);
                dataCommand.Parameters.AddWithValue("DistrictTotalMileage", 0);

                DropDownList ddl_pd = ((DropDownList)GridView_tasklog.FooterRow.FindControl("DropDownList_perDiem"));
                double perD = Double.Parse(ddl_pd.SelectedValue);
                dataCommand.Parameters.AddWithValue("PerDiemDays", perD);


                dataCommand.Parameters.AddWithValue("EmployeePerDiemTotal", 0);
                dataCommand.Parameters.AddWithValue("DistrictPerDiemTotal", 0);

                dataCommand.Parameters.AddWithValue("EmployeeTravelTotal", 0);
                dataCommand.Parameters.AddWithValue("DistrictTravelTotal", 0);

                dataCommand.Parameters.AddWithValue("EmployeeTimesheetTotal", 0);
                dataCommand.Parameters.AddWithValue("DistrictTimesheetTotal", 0);

                //connection.Open();
                dataCommand.ExecuteNonQuery();
                connection.Close();

                resetLabels();
            }
            catch (System.Data.SqlClient.SqlException ex) {
                string msg = "GridView_tasklogTotalsRowUpdating: ";
                msg += ex.Message;
                this.ClientScript.RegisterStartupScript(this.GetType(), "Task Log Totals UPDATE Error:", "<script language=\"javaScript\">" + "alert('Could NOT update Task Log totals with the information!" + msg + "');" + "</script>");
                //throw new Exception(msg);
            }
            finally {               
                try {//and UPDATE the tblAncillaryTimesheetDetail with the time sheet detail total ID in case any records were added        
                    //update takes care of the task log ID update
                    string updateStatement = "UPDATE [Sapphire].[dbo].[tblAncillaryTimesheetsDetail] SET [TimesheetDetailTotalID]=@TimesheetDetailTotalID WHERE ([InstitutionName]=@InstitutionName AND [ServiceDate]=@ServiceDate) AND ([Ancillary]=@Ancillary)";


                    SqlConnection connectionUpdate = new SqlConnection(connString);
                    SqlCommand sqlCommand = new SqlCommand(updateStatement, connectionUpdate);

                    //update based on matching district and date
                    sqlCommand.Parameters.AddWithValue("@TimesheetDetailTotalID", TimesheetDetailTotalID);
                    sqlCommand.Parameters.AddWithValue("@InstitutionName", DropDownList_Institution.Text);
                    sqlCommand.Parameters.AddWithValue("@ServiceDate", TextBox_datepicker.Text);
                    sqlCommand.Parameters.AddWithValue("@Ancillary", DropDownList_ancillary.Text);

                    //open connection
                    connectionUpdate.Open();

                    sqlCommand.ExecuteNonQuery();

                    //close connection
                    connectionUpdate.Close();
                }//if UPDATE tblAncillaryTimesheetDetail with TimesheetDetailTotalID fails do this...
                catch (System.Data.SqlClient.SqlException ex) {
                    string msg = "GridView_tasklogTotalsRowUpdating: ";
                    msg += ex.Message;
                    this.ClientScript.RegisterStartupScript(this.GetType(), "Could not UPDATE tblAncillaryTimesheetsDetail with TimesheetDetailTotalID!", "<script language=\"javaScript\">" + "alert('" + msg + "');" + "</script>");
                }
                finally {//if UPDATE tblAncillaryTimesheetDetail with TimesheetDetailTotalID is a success do this
                    this.ClientScript.RegisterStartupScript(this.GetType(), "Successfully completed Task Log", "<script language=\"javaScript\">" + "alert('Task Log Successfully Added');" + "</script>");
                }//end of try/catch for updating tblAncillaryTimesheetsDetail with TimesheetDetailTotalID      
                

            }//end of try/catch for UPDATE task log totals



            ////GET THE INDEX OF THE ROW WANTING TO UPDATE - THE EFFECT IS THAT IT CONFIRMS THE CHANGES
            GridView_tasklogTotal.EditIndex = -1;

            //reset any necessary controls
            ResetDailyDetailsControls();
            panel_information.Enabled = true;
            GridView_tasklog.Visible = false;
            Button_PrintLog.Visible = false;
            
            //re-bind the gridviews
            BindGridViewTimesheetsDetailTotals();
            UDP_GridViewTimesheetDetailTotals.Update();

            BindGridViewTimesheetsDetailTEST(name, date, district, ancillary);
            UDP_GridViewTimesheetDetails.Update();

        }//END OF TASK LOG TOTALS ROW UPDATING

        //CALLED WHEN THE CANCEL BUTTON IS PRESSED IN THE TASK LOG TOTALS
        protected void GridView_tasklogTotals_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) {
            
            GridView_tasklogTotal.EditIndex = -1;

            BindGridViewTimesheetsDetailTotals();

           
            resetLabels();
            ResetDailyDetailsControls();
            Button_PrintLog.Visible = false;


            string name = DropDownList_FullName.Text;
            string date = TextBox_datepicker.Text;
            string district = DropDownList_Institution.Text;
            string ancillary = DropDownList_ancillary.Text;

            BindGridViewTimesheetsDetailTEST(name, date, district, ancillary);

            UDP_GridViewTimesheetDetails.Update();

        }//END OF TASK LOG TOTALS ROW CANCELLING EDIT

        //CALLED WHEN THE DELETE BUTTON IS PRESSED IN THE TASK LOG TOTALS - DELETES AN ENTIRE TASK LOG
        protected void GridView_tasklogTotals_RowDeleting(object sender, GridViewDeleteEventArgs e) {


            //get TimesheetDetailID
            int TimesheetDetailTotalID = int.Parse(GridView_tasklogTotal.Rows[e.RowIndex].Cells[1].Text);

            //SQL DELETE statement from totals
            string statement = "DELETE FROM [Sapphire].[dbo].[tblAncillaryTimesheetsDetailTotals] WHERE [TimesheetDetailTotalID]=@TimesheetDetailTotalID";
            //SQL DELETE corresponding record from details
            string statement2 = "DELETE FROM [Sapphire].[dbo].[tblAncillaryTimesheetsDetail] WHERE [TimesheetDetailTotalID]=@TimesheetDetailTotalID";

            //prepare connection
            SqlConnection connection = new SqlConnection(connString);

            try {//to delete a row in task log totals
                connection.Open();
                SqlCommand cmd = new SqlCommand(statement, connection);
                cmd.Parameters.AddWithValue("@TimesheetDetailTotalID", TimesheetDetailTotalID);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

            }
            catch (System.Data.SqlClient.SqlException ex) {//catch any exceptions
                string msg = "Delete Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            finally {//finally dispaly message to user and close connection
                connection.Close();
                this.ClientScript.RegisterStartupScript(this.GetType(), "Successful", "<script language=\"javaScript\">" + "alert('Successfully Deleted the Task Log (in TOTALS)!');" + "</script>");

                try {//to delete the corresponding records in details with the same total id
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(statement2, connection);
                    cmd.Parameters.AddWithValue("@TimesheetDetailTotalID", TimesheetDetailTotalID);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                }
                catch (System.Data.SqlClient.SqlException ex) {//catch any exceptions
                    string msg = "Delete Error:";
                    msg += ex.Message;
                    this.ClientScript.RegisterStartupScript(this.GetType(), "Unsuccessful", "<script language=\"javaScript\">" + "alert('Could Not Delete Task Log (in Details) " + msg + "');" + "</script>");
                }
                finally {//finally dispaly message to user and close connection
                    connection.Close();
                    this.ClientScript.RegisterStartupScript(this.GetType(), "Successful", "<script language=\"javaScript\">" + "alert('Successfully Deleted the Task Log! (in Details)');" + "</script>");
                }
            }

            BindGridViewTimesheetsDetailTotals();
           

            //show or hide task log visibility if user deletes all rows
            if (GridView_tasklogTotal.Rows.Count > 0) {
                GridView_tasklogTotal.Visible = true;
                
            }
            else {
                GridView_tasklogTotal.Visible = false;
               
            }
            UDP_GridViewTimesheetDetails.Update();
            UDP_GridViewTimesheetDetailTotals.Update();

        }//END OF GRIDVIEW TASK LOG TOTALS ROW DELETING




        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////BUTTON CLICK EVENTS
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //ALLOWS USER TO BEGIN ENTERING LINE ITEM INFORMATION FOR THAT SPECIFIC DAY
        protected void Button_enterDetailsClick(object sender, EventArgs e) {
            Debug.WriteLine("\n\n\n***ENTER DETAILS HAS BEEN CLICKED***\n\n\n");

            SQLDBConnectionPOInfo();
            UDP_PurchaseOrder.Update();
            

            string name = DropDownList_FullName.Text;
            string date = TextBox_datepicker.Text;
            string district = DropDownList_Institution.Text;
            string ancillary = DropDownList_ancillary.Text;

            //used as a check to see if task logs exist with the following criteria
            BindGridViewTimesheetsDetailTEST(name, date, district, ancillary);

            //SHOW ERROR IF SERVICE DATE IS MISSING
            if (TextBox_datepicker.Text == "") {
                this.ClientScript.RegisterStartupScript(this.GetType(), "Missing Date", "<script language=\"javaScript\">" + "alert('Please enter a Service Date before continuing!');" + "</script>");
                
                //set focus back on name to ensure calendar re-renders on focus
                DropDownList_FullName.Focus();

            }//SHOW ERROR IF DISTRICT IS MISSING            
            else if (DropDownList_Institution.Text == "Select District..." || DropDownList_Institution.Text.Length == 0) {
                this.ClientScript.RegisterStartupScript(this.GetType(), "Missing District", "<script language=\"javaScript\">" + "alert('Please select a District before continuing!');" + "</script>");
                
                DropDownList_Institution.Focus();//focus back on district drop down
            }else if (GridView_tasklog.Rows.Count > 0) {
                Button_logComplete.Visible = false;
                
                GridView_tasklog.Visible = false;

                panel_daily_details.Enabled = false;                

                if (ancillary == "Yes") {
                    ancillary = "Ancillary Time";
                }
                else {
                    ancillary = "Non-Ancillary Time";
                }

                //using this for javascript alerts prevents strange css rendering when message is showed
                this.ClientScript.RegisterStartupScript(this.GetType(), "Log Exists Already", "<script language=\"javaScript\">" + "alert('A Task Log for " + district + " dated " + date + ", with " + ancillary + " has already been submitted!');" + "</script>");

            }
            else {//if no logs are present then allow user to enter a new log                

                panel_daily_details.Enabled = true;
                panel_information.Enabled = false;


                //set the task logs labels accordingly
                setLabels();


                Button_changeInfo.Visible = true;

            }//end of check for existing logs

            BindDropDown_location();           

        }//END OF ENTER DETAILS CLICK

        //ALLOWS USER TO GO BACK AND CHANGE INFROMATION WITHIN THE CONTAINER_INFORMATION
        protected void Button_changeInfoClick(object sender, EventArgs e) {

            //enable information panel
            panel_information.Enabled = true;

            //disable daily detail panel
            panel_daily_details.Enabled = false;

            //RESETS DAILY DETAIL CONTROLS AND READYS FOR A NEW DAY...SO TO SPEAK
            ResetDailyDetailsControls();

            //reset the labels in the task log
            resetLabels();

            //UDP_InformationContainer.Update();

        }//END OF CHANGE INFO CLICK

        //GETS DATA FROM DAILY DETAILS CONTROLS AND POPULATES GRIDVIEW_TASKLOG WITH IT
        protected void Button_addLineClick(object sender, EventArgs e) {
            //Debug.WriteLine("PLACE TO TEST BEFORE INSERTING ANY DATA");
            
          

            //bTime IS BEGIN TIME
            //eTime IS END TIME
            double bTime = Double.Parse(TextBox_spinnerB.Text);
            double eTime = Double.Parse(TextBox_spinnerE.Text);

            ////USED FOR CALCULATING TOTAL HOURS WORKED 
            ////begin and end time am/pm values 
            String strBeginAmPm = radListBegin.SelectedValue;
            String strEndAmPm = radListEnd.SelectedValue;


            ////check data stored in hidden fields for access purposes
            Debug.WriteLine("HIDDEN DATA VALUE BEGIN --->" + strBeginAmPm);
            Debug.WriteLine("HIDDEN DATA VALUE END--->" + strEndAmPm);

            ////calculate the hours worked
            double hoursWorked = CalculateHoursWorked(strBeginAmPm, strEndAmPm, bTime, eTime);


            ////SHOW TASK LOG CONTAINER
            ////container_tasklog.Visible = true;
            GridView_tasklog.Visible = true;
            Button_PrintLog.Visible = true;

            //string connString = System.Configuration.ConfigurationManager.ConnectionStrings["AncillaryConnString"].ToString();


            string sqlQuery = "INSERT INTO tblAncillaryTimesheetsDetail (FullName, ServiceCode, ServiceDate, InstitutionName, BeginTime, BeginTimeAMPM, EndTime, EndTimeAMPM, Location, Ancillary, DC, RS, IDT, NIDT, IDTMiles, NIDTMiles, Notes, Trips)";
            sqlQuery += " VALUES (@FullName, @ServiceCode, @ServiceDate, @InstitutionName, @BeginTime, @BeginTimeAMPM, @EndTime, @EndTimeAMPM, @Location, @Ancillary, @DC, @RS, @IDT, @NIDT, @IDTMiles, @NIDTMiles, @Notes, @Trips)";
            
            try {
                using (SqlConnection dataConnection = new SqlConnection(connString)) {
                    using (SqlCommand dataCommand = new SqlCommand(sqlQuery, dataConnection)) {
                        dataCommand.Parameters.AddWithValue("FullName", DropDownList_FullName.Text);
                        dataCommand.Parameters.AddWithValue("ServiceCode", DropDownList_Position.Text);
                        dataCommand.Parameters.AddWithValue("ServiceDate", TextBox_datepicker.Text);
                        dataCommand.Parameters.AddWithValue("InstitutionName", DropDownList_Institution.Text);
                        dataCommand.Parameters.AddWithValue("BeginTime", TextBox_spinnerB.Text);
                        dataCommand.Parameters.AddWithValue("BeginTimeAMPM", radListBegin.SelectedValue);

                        dataCommand.Parameters.AddWithValue("EndTime", TextBox_spinnerE.Text);
                        dataCommand.Parameters.AddWithValue("EndTimeAMPM", radListEnd.SelectedValue);
                        //INSERT TASK TYPE WHEN LUNCH/BREAK IS SELECTED 
                        //LOCATION DOESN'T MATTER IN THIS SCENARIO 

                        dataCommand.Parameters.AddWithValue("Ancillary", DropDownList_ancillary.Text);

                        //string nonAnc = CheckBox_nonAncillary.Checked.ToString();

                        //if(CheckBox_nonAncillary.Checked){
                        //    dataCommand.Parameters.AddWithValue("Ancillary", nonAnc);
                        //}else{
                        //    dataCommand.Parameters.AddWithValue("Ancillary", nonAnc);
                        //}


                    
                        //INSERT TIME WORKED AND PLACE IT IN THE COLUMN
                        //ACCORDING TO THE TASK TYPE SELECTED DROP DOWN LIST
                        if (DropDownList_taskType.Text == "DC") {//DC DROP DOWN SELECTION
                            dataCommand.Parameters.AddWithValue("DC", Math.Round(hoursWorked, 2));


                            dataCommand.Parameters.AddWithValue("RS", 0);
                            dataCommand.Parameters.AddWithValue("IDT", 0);
                            dataCommand.Parameters.AddWithValue("NIDT", 0);
                            dataCommand.Parameters.AddWithValue("IDTMiles", 0);
                            dataCommand.Parameters.AddWithValue("NIDTMiles", 0);
                            if (DropDownList_Location.Text == "Select Location...") {
                                dataCommand.Parameters.AddWithValue("Location", "");

                            }
                            else {
                                dataCommand.Parameters.AddWithValue("Location", DropDownList_Location.Text);

                            }
                
                        }
                        else if (DropDownList_taskType.Text == "RS") {//RS DROP DOWN SELECTION
                            dataCommand.Parameters.AddWithValue("RS", Math.Round(hoursWorked, 2));

                            dataCommand.Parameters.AddWithValue("DC", 0);
                            dataCommand.Parameters.AddWithValue("IDT", 0);
                            dataCommand.Parameters.AddWithValue("NIDT", 0);
                            dataCommand.Parameters.AddWithValue("IDTMiles", 0);
                            dataCommand.Parameters.AddWithValue("NIDTMiles", 0);
                            if (DropDownList_Location.Text == "Select Location...") {
                                dataCommand.Parameters.AddWithValue("Location", "");

                            }
                            else {
                                dataCommand.Parameters.AddWithValue("Location", DropDownList_Location.Text);

                            }

                        }
                        else if (DropDownList_taskType.Text == "IDT") {//IDT DROP DOWN SELECTION
                            dataCommand.Parameters.AddWithValue("IDT", Math.Round(hoursWorked, 2));

                            dataCommand.Parameters.AddWithValue("DC", 0);
                            dataCommand.Parameters.AddWithValue("RS", 0);
                            dataCommand.Parameters.AddWithValue("NIDT", 0);
                            dataCommand.Parameters.AddWithValue("NIDTMiles", 0);

                            if (DropDownList_Location.Text == "Select Location...") {
                                dataCommand.Parameters.AddWithValue("Location", "");

                            }
                            else {
                                dataCommand.Parameters.AddWithValue("Location", DropDownList_Location.Text);

                            }

                            if (TextBox_miles.Text == "") {
                                dataCommand.Parameters.AddWithValue("IDTMiles", 0);//IF MILES ARE BLANK
                            }
                            else
                                dataCommand.Parameters.AddWithValue("IDTMiles", Int32.Parse(TextBox_miles.Text));//INSERT IDT MILES

                        }
                        else if (DropDownList_taskType.Text == "NIDT") {//NIDT DROP DOWN SELECTION
                            dataCommand.Parameters.AddWithValue("NIDT", Math.Round(hoursWorked, 2));

                            dataCommand.Parameters.AddWithValue("DC", 0);
                            dataCommand.Parameters.AddWithValue("RS", 0);
                            dataCommand.Parameters.AddWithValue("IDT", 0);
                            dataCommand.Parameters.AddWithValue("IDTMiles", 0);

                            if (DropDownList_Location.Text == "Select Location...") {
                                dataCommand.Parameters.AddWithValue("Location", "");

                            }
                            else {
                                dataCommand.Parameters.AddWithValue("Location", DropDownList_Location.Text);

                            }

                            if (TextBox_miles.Text == "") {
                                dataCommand.Parameters.AddWithValue("NIDTMiles", 0);//IF MILES ARE BLANK
                            }
                            else
                                dataCommand.Parameters.AddWithValue("NIDTMiles", Int32.Parse(TextBox_miles.Text));//INSERT NIDT MILES
                        }
                        else if (DropDownList_taskType.Text == "LB") {

                            dataCommand.Parameters.AddWithValue("Location", "Lunch");

                            dataCommand.Parameters.AddWithValue("DC", 0);
                            dataCommand.Parameters.AddWithValue("RS", 0);
                            dataCommand.Parameters.AddWithValue("IDT", 0);
                            dataCommand.Parameters.AddWithValue("NIDT", 0);
                            dataCommand.Parameters.AddWithValue("IDTMiles", 0);
                            dataCommand.Parameters.AddWithValue("NIDTMiles", 0);
                        }                    

                        //INSERT ANY NOTES BY COLUMN NAME
                        dataCommand.Parameters.AddWithValue("Notes", TextBox_notes.Text);


                        //INSERT TRIP INFORMATION IF ANY
                        dataCommand.Parameters.AddWithValue("Trips", 0);

                        dataConnection.Open();
                        dataCommand.ExecuteNonQuery();
                        dataConnection.Close();


                        //BindGridViewTimesheetsDetail();

                        string name = DropDownList_FullName.Text;
                        string date = TextBox_datepicker.Text;
                        string district = DropDownList_Institution.Text;
                        string ancillary = DropDownList_ancillary.Text;

                        BindGridViewTimesheetsDetailTEST(name, date, district, ancillary);

                    }//end of using data command
                }//end of using data connection
            }
            catch (System.Data.SqlClient.SqlException ex) {
                string msg = "Task Log INSERT Error:";
                msg += ex.Message;
                this.ClientScript.RegisterStartupScript(this.GetType(), "Could NOT insert data!", "<script language=\"javaScript\">" + "alert('Could NOT insert the information!');" + "</script>");
                throw new Exception(msg);
            }
            finally {                
                //this.ClientScript.RegisterStartupScript(this.GetType(), "Update Successful!", "<script language=\"javaScript\">" + "alert('You have successfully inserted the information!');" + "</script>");
            }//end of try/catch


            //REFORMATS CONTROLS FOR NEW LINE
            PrepareControlsForNewLine();
           

            //BIND THE LOCATION TO ITS DATA SOURCE
            BindDropDown_location();


            ////IF THERE IS DATA IN THE TASK LOG HIDE THE BACK BUTTON TO PREVENT MULTIPLE ENTRIES
            if (GridView_tasklog.Rows.Count > 0) {
                Button_changeInfo.Enabled = false;//DISABLE BUTTON
                Button_changeInfo.Visible = false;//HIDE BUTTON
                Button_logComplete.Visible = true;
                //...BETTER TO BE SAFE THAN SORRY
            }
            else {
                Button_logComplete.Visible = false;
            }//end of if


        }//END OF NEW LINE CLICK

        //CALLED WHEN TASK LOG IS COMPLETE AND PLACES THE TASK LOG IN THE EMPLOYEE TASK LOG TOTALS GRID VIEW
        protected void Button_logCompleteClick(object sender, EventArgs e) {
            /*cell order of GridView_totals
              * 0 = edit
             * 1 = TimesheetDetailID
             * 2 = TimesheetDetailTotalID
             * 3 = InstitutionName
             * 4 = Program
             * 5 = PO
             * 6 = ServiceCode
             * 7 = FullName
             * 8 = EntryDate
             * 9 = ServiceDate
             * 10 = Ancillary
             * 11 = DC
             * 12 = RS
             * 13 = IDT
             * 14 = NIDT
             * 15 = OffSitePrep
             * 16 = OnSiteTotal
             * 17 = HoursWorked
             * 18 = HoursPaid
             * 19 = EmployeeTotal
             * 20 = DistrictTotal
             * 21 = Trips
             * 22 = IDTMiles
             * 23 = NIDTMiles
             * 24 = EmployeeIDTMileageTotal
             * 25 = DistrictIDTMileageTotal
             * 26 = EmployeeNIDTMileageTotal
             * 27 = DistrictNIDTMileageTotal
             * 28 = EmployeeTotalMileage
             * 29 = DistrictTotalMileage
             * 30 = PerDiemDays
             * 31 = EmployeePerDiemTotal
             * 32 = DistrictPerDiemTotal
             * 33 = EmployeeTravelTotal
             * 34 = DistrictTravelTotal
             * 35 = EmployeeTimesheetTotal
             * 36 = DistrictTimesheetTotal
             * 37 = edit
             */
           
            //log complete visibility
            Button_logComplete.Visible = false;


            //if data is present in grid view task log then process it
            if (GridView_tasklog.Rows.Count > 0) {//indexed at 0
                string insertStatement = "INSERT INTO tblAncillaryTimesheetsDetailTotals (InstitutionName, Program, PO, ServiceCode, FullName, EntryDate, ServiceDate, Ancillary, DC, RS, IDT, NIDT, OffSitePrep, OnSiteTotal, HoursWorked, HoursPaid, EmployeeTotal, DistrictTotal, Trips, IDTMiles, NIDTMiles, EmployeeIDTMileageTotal, DistrictIDTMileageTotal, EmployeeNIDTMileageTotal, DistrictNIDTMileageTotal, EmployeeTotalMileage, DistrictTotalMileage, PerDiemDays, EmployeePerDiemTotal, DistrictPerDiemTotal, EmployeeTravelTotal, DistrictTravelTotal, EmployeeTimesheetTotal, DistrictTimesheetTotal)";
                insertStatement += " VALUES (@InstitutionName, @Program, @PO, @ServiceCode, @FullName, @EntryDate, @ServiceDate, @Ancillary, @DC, @RS, @IDT, @NIDT, @OffSitePrep, @OnSiteTotal, @HoursWorked, @HoursPaid, @EmployeeTotal, @DistrictTotal, @Trips, @IDTMiles, @NIDTMiles, @EmployeeIDTMileageTotal, @DistrictIDTMileageTotal, @EmployeeNIDTMileageTotal, @DistrictNIDTMileageTotal, @EmployeeTotalMileage, @DistrictTotalMileage, @PerDiemDays, @EmployeePerDiemTotal, @DistrictPerDiemTotal, @EmployeeTravelTotal, @DistrictTravelTotal, @EmployeeTimesheetTotal, @DistrictTimesheetTotal)";


                try {//to INSERT task log data into tblAncillaryTimesheetsDetaolTotals
                    using (SqlConnection dataConnection = new SqlConnection(connString)) {
                        using (SqlCommand dataCommand = new SqlCommand(insertStatement, dataConnection)) {
                            /*cell order of GridView_tasklog using some of the footer rows from here in GridView_totals                           
                                * 0 = edit
                                * 1 = TimesheetDetailID
                                * 2 = TimesheetDetailTotalID
                                * 3 = FullName
                                * 4 = ServiceCode 
                                * 5 = ServiceDate
                                * 6 = InstitutionName
                                * 7 = BeginTime
                                * 8 = BeginTimeAMPM
                                * 9 = EndTime
                                * 10 = EndTimeAMPM
                                * 11 = Location
                                * 12 = Ancillary
                                * 13 = DC
                                * 14 = RS
                                * 15 = IDT
                                * 16 = NIDT
                                * 17 = IDTMiles
                                * 18 = NIDTMiles
                                * 19 = Notes
                                * 20 = Trips
                                * 21 = delete
                                * */

                            dataCommand.Parameters.AddWithValue("InstitutionName", DropDownList_Institution.Text);
                            dataCommand.Parameters.AddWithValue("Program", DropDownList_Program.Text);
                            if (Label_purchaseOrder.Text.Length > 0) {
                                dataCommand.Parameters.AddWithValue("PO", Label_purchaseOrder.Text);
                            }
                            else {
                                dataCommand.Parameters.AddWithValue("PO", "");
                            }
                            dataCommand.Parameters.AddWithValue("ServiceCode", DropDownList_Position.Text);
                            dataCommand.Parameters.AddWithValue("FullName", DropDownList_FullName.Text);
                            dataCommand.Parameters.AddWithValue("EntryDate", DateTime.Now);
                            dataCommand.Parameters.AddWithValue("ServiceDate", TextBox_datepicker.Text);

                            dataCommand.Parameters.AddWithValue("Ancillary", DropDownList_ancillary.Text);

                               
                            double hoursW = 0.0;


                            if (GridView_tasklog.FooterRow.Cells[13].Text.Length == 0) {
                                dataCommand.Parameters.AddWithValue("DC", 0);
                            }
                            else {
                                dataCommand.Parameters.AddWithValue("DC", GridView_tasklog.FooterRow.Cells[13].Text);
                            }


                            if (GridView_tasklog.FooterRow.Cells[14].Text.Length == 0) {
                                dataCommand.Parameters.AddWithValue("RS", 0);
                            }
                            else {
                                dataCommand.Parameters.AddWithValue("RS", GridView_tasklog.FooterRow.Cells[14].Text);
                            }


                            if (GridView_tasklog.FooterRow.Cells[15].Text.Length == 0) {
                                dataCommand.Parameters.AddWithValue("IDT", 0);
                            }
                            else {
                                dataCommand.Parameters.AddWithValue("IDT", GridView_tasklog.FooterRow.Cells[15].Text);
                            }


                            if (GridView_tasklog.FooterRow.Cells[16].Text.Length == 0) {
                                dataCommand.Parameters.AddWithValue("NIDT", 0);
                            }
                            else {
                                dataCommand.Parameters.AddWithValue("NIDT", GridView_tasklog.FooterRow.Cells[16].Text);
                            }

                            dataCommand.Parameters.AddWithValue("OffSitePrep", 0);
                            dataCommand.Parameters.AddWithValue("OnSiteTotal", 0);

                            dataCommand.Parameters.AddWithValue("HoursWorked", hoursW);
                            dataCommand.Parameters.AddWithValue("HoursPaid", hoursW);

                            dataCommand.Parameters.AddWithValue("EmployeeTotal", 0);
                            dataCommand.Parameters.AddWithValue("DistrictTotal", 0);

                            dataCommand.Parameters.AddWithValue("Trips", 0);

                            if (GridView_tasklog.FooterRow.Cells[14].Text == "0.00") {
                                dataCommand.Parameters.AddWithValue("IDTMiles", 0);
                            }
                            else {
                                dataCommand.Parameters.AddWithValue("IDTMiles", GridView_tasklog.FooterRow.Cells[16].Text);
                            }


                            if (GridView_tasklog.FooterRow.Cells[17].Text == "0.00") {
                                dataCommand.Parameters.AddWithValue("NIDTMiles", 0);
                            }
                            else {
                                dataCommand.Parameters.AddWithValue("NIDTMiles", GridView_tasklog.FooterRow.Cells[17].Text);
                            }

                            dataCommand.Parameters.AddWithValue("EmployeeIDTMileageTotal", 0);
                            dataCommand.Parameters.AddWithValue("DistrictIDTMileageTotal", 0);

                            dataCommand.Parameters.AddWithValue("EmployeeNIDTMileageTotal", 0);
                            dataCommand.Parameters.AddWithValue("DistrictNIDTMileageTotal", 0);

                            dataCommand.Parameters.AddWithValue("EmployeeTotalMileage", 0);
                            dataCommand.Parameters.AddWithValue("DistrictTotalMileage", 0);

                            DropDownList ddl_pd = ((DropDownList)GridView_tasklog.FooterRow.FindControl("DropDownList_perDiem"));
                            double perD = Double.Parse(ddl_pd.SelectedValue);
                            dataCommand.Parameters.AddWithValue("PerDiemDays", perD);


                            dataCommand.Parameters.AddWithValue("EmployeePerDiemTotal", 0);
                            dataCommand.Parameters.AddWithValue("DistrictPerDiemTotal", 0);

                            dataCommand.Parameters.AddWithValue("EmployeeTravelTotal", 0);
                            dataCommand.Parameters.AddWithValue("DistrictTravelTotal", 0);

                            dataCommand.Parameters.AddWithValue("EmployeeTimesheetTotal", 0);
                            dataCommand.Parameters.AddWithValue("DistrictTimesheetTotal", 0);

                            dataConnection.Open();
                            dataCommand.ExecuteNonQuery();
                            dataConnection.Close();

                            //bind gridview time sheets detail totals 
                            BindGridViewTimesheetsDetailTotals();


                        }//end of using sql command
                    }//end of using sql connection

                }//if INSERT task log into totals table fails do this...
                catch (System.Data.SqlClient.SqlException ex) {
                    string msg = "Button_logCompleteClick SQL ERROR: ";
                    msg += ex.Message;
                    this.ClientScript.RegisterStartupScript(this.GetType(), "Could NOT insert Task Log!", "<script language=\"javaScript\">" + "alert('" + msg + "');" + "</script>");
                        
                }
                finally {//if INSERT task log into totals table succeeds do this...                    
                    int TimesheetDetailTotalID = 0;

                    //query gets the total id
                    string query = "SELECT TimesheetDetailTotalID FROM [Sapphire].[dbo].[tblAncillaryTimesheetsDetailTotals] WHERE ([InstitutionName]=@InstitutionName AND [ServiceDate]=@ServiceDate) AND ([Ancillary]=@Ancillary)";

                        
                    try {//to get the timesheet detail total ID from totals table
                        SqlConnection connectionGetID = new SqlConnection(connString);
                        SqlCommand sqlCommand = new SqlCommand(query, connectionGetID);

                        //get the id that matches the district and date parameters
                        sqlCommand.Parameters.AddWithValue("@InstitutionName", DropDownList_Institution.Text);
                        sqlCommand.Parameters.AddWithValue("@ServiceDate", TextBox_datepicker.Text);
                        sqlCommand.Parameters.AddWithValue("@Ancillary", DropDownList_ancillary.Text);

                        //open connection
                        connectionGetID.Open();
                        sqlCommand.ExecuteNonQuery();

                        //get the timesheet detail total id
                        TimesheetDetailTotalID = (int)sqlCommand.ExecuteScalar();

                        //close connection
                        connectionGetID.Close();
                    }//if getting the timesheet detail total ID from totals table fails do this...
                    catch (System.Data.SqlClient.SqlException ex) {
                        string msg = "Button_logCompleteClick SQL get TimesheetDetailTotalID from totals table ERROR: ";
                        msg += ex.Message;
                        this.ClientScript.RegisterStartupScript(this.GetType(), "Could not obtain TimesheetDetailTotalID!", "<script language=\"javaScript\">" + "alert('" + msg + "');" + "</script>");
                    }
                    finally {//if getting timesheet detail total ID from totals table is a success do this...

                        try {//and UPDATE tblAncillaryTimesheetDetail with the TimesheetDetailTotalID obtained from the query        
                            //update takes care of the task log update
                            string updateStatement = "UPDATE [Sapphire].[dbo].[tblAncillaryTimesheetsDetail] SET [TimesheetDetailTotalID]=@TimesheetDetailTotalID WHERE ([InstitutionName]=@InstitutionName AND [ServiceDate]=@ServiceDate) AND ([Ancillary]=@Ancillary)";


                            SqlConnection connectionUpdate = new SqlConnection(connString);
                            SqlCommand sqlCommand = new SqlCommand(updateStatement, connectionUpdate);

                            //update based on matching district and date
                            sqlCommand.Parameters.AddWithValue("@TimesheetDetailTotalID", TimesheetDetailTotalID);
                            sqlCommand.Parameters.AddWithValue("@InstitutionName", DropDownList_Institution.Text);
                            sqlCommand.Parameters.AddWithValue("@ServiceDate", TextBox_datepicker.Text);
                            sqlCommand.Parameters.AddWithValue("@Ancillary", DropDownList_ancillary.Text);

                            //open connection
                            connectionUpdate.Open();

                            sqlCommand.ExecuteNonQuery();

                            //close connection
                            connectionUpdate.Close();
                        }//if UPDATE tblAncillaryTimesheetDetail with TimesheetDetailTotalID fails do this...
                        catch (System.Data.SqlClient.SqlException ex) {
                            string msg = "Button_logCompleteClick SQL UPDATE tblAncillaryTimesheetsDetail ERROR: ";
                            msg += ex.Message;
                            this.ClientScript.RegisterStartupScript(this.GetType(), "Could not UPDATE tblAncillaryTimesheetsDetail with TimesheetDetailTotalID!", "<script language=\"javaScript\">" + "alert('" + msg + "');" + "</script>");
                        }
                        finally {//if UPDATE tblAncillaryTimesheetDetail with TimesheetDetailTotalID is a success do this, and take a breath...
                            this.ClientScript.RegisterStartupScript(this.GetType(), "Successfully completed Task Log", "<script language=\"javaScript\">" + "alert('Task Log sucessfully added to Totals');" + "</script>");
                        }//end of try/catch for updating tblAncillaryTimesheetsDetail with TimesheetDetailTotalID

                    }//end of try/catch for getting the TimesheetDetailTotalID from totals table


                    //RESETS DAILY DETAIL CONTROLS AND READYS FOR A NEW DAY...SO TO SPEAK
                    ResetDailyDetailsControls();

                    //disable daily details
                    panel_daily_details.Enabled = false;

                    //enable information
                    panel_information.Enabled = true;
                    DropDownList_ancillary.Enabled = true;

                    //reset datepicker back on date picker
                    TextBox_datepicker.Text = "";

                    //focus back on name ddl
                    DropDownList_FullName.Focus();


                    //get variables to rebind task log information
                    string name = DropDownList_FullName.Text;
                    string date = TextBox_datepicker.Text;
                    string district = DropDownList_Institution.Text;
                    string ancillary = DropDownList_ancillary.Text;

                    BindGridViewTimesheetsDetailTEST(name, date, district, ancillary);

                    Button_PrintLog.Visible = false;

                    //reset the task log labels
                    resetLabels();                       

                }//end of try/catch task log complete


            }//end of grid view task log row check

                

        }//END OF LOG COMPLETE CLICK




        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////USED FOR TESTING QUERIES TO DATABASES (REMOVE)    
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////       
        //USED TO TEST THE CONNECTION AND CHECK THE CONTENTS OF ANCILLARY STAFF
        //***ONLY USED FOR TESTING PURPOSES*** REMOVE WHEN DEPLOYING
        private void SQLDBConnectionStaffInfo() {
            //DBConnection is the name of the connection string that was set up from the web config file
            //string connString = System.Configuration.ConfigurationManager.ConnectionStrings["AncillaryConnString"].ToString();

            string select = "SELECT * FROM [Sapphire].[dbo].[view_ancillary_Staff] ORDER BY [LastName], [FirstName] ASC";

            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(select, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read()) {
                Debug.WriteLine("Fiscal Year: {0, -15} Full Name: {1, -25} First Name: {2, -15} Last Name: {3, -20} Middle: {4, -10} Preferred: {5, -15} Courtesy: {6, -10} Svc Code: {7, -15} Position: {8, -30} Default: {9, -10} ???: {10, -20} ???: {11, -20}", reader[0], reader[1], reader[2], reader[3], reader[4], reader[5], reader[6], reader[7], reader[8], reader[9], reader[10], reader[11]);
            }            
        }//end of connection test


        //USED TO TEST THE CONNECTION AND CHECK THE CONTENTS OF view_ancillary_EmployeesWithPO
        //***ONLY USED FOR TESTING PURPOSES*** REMOVE WHEN DEPLOYING
        private void SQLDBConnectionPOInfo() {
            //DBConnection is the name of the connection string that was set up from the web config file
            //string connString = System.Configuration.ConfigurationManager.ConnectionStrings["AncillaryConnString"].ToString();
            //get the fiscal year
            string fy = DropDownList_FiscalYear.Text;

            //get the name in the text box and split it for a proper query
            string na = DropDownList_FullName.Text;
            string institutionName = DropDownList_Institution.Text;
            string program = DropDownList_Program.Text;
            string serviceCode = DropDownList_Position.SelectedValue;//.Text;

            string select = "SELECT PO FROM [Sapphire].[dbo].[view_ancillary_EmployeesWithPO] WHERE ([FiscalYear]=@FiscalYear AND [InstitutionName]=@InstitutionName) AND ([Program]=@Program AND [ServiceCode]=@ServiceCode) AND ([FullName]=@FullName)";

            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(select, conn);
            cmd.Parameters.AddWithValue("@FiscalYear", fy);
            cmd.Parameters.AddWithValue("@FullName", formatNameforQuery(na));
            cmd.Parameters.AddWithValue("@InstitutionName", institutionName);
            cmd.Parameters.AddWithValue("@Program", program);
            cmd.Parameters.AddWithValue("@ServiceCode", serviceCode);
            using (SqlDataReader rdr = cmd.ExecuteReader()) {
                while (rdr.Read()) {
                    Label_purchaseOrder.Text = rdr.GetString(0);
                }
            }

            conn.Close();
        }//end of connection test

        //USED TO TEST THE CONNECTION AND CHECK THE CONTENTS OF MILEAGE/DISTRICT INFO
        //***ONLY USED FOR TESTING PURPOSES*** REMOVE WHEN DEPLOYING
        private void SQLDBConnectionMileageInfo() {
            //DBConnection is the name of the connection string that was set up from the web config file
            //string connString = System.Configuration.ConfigurationManager.ConnectionStrings["AncillaryConnString"].ToString();

            string select = "SELECT * FROM [Sapphire].[dbo].[tblAncillaryMileageChart] ORDER BY [EntityName], [Location] ASC";

            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(select, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read()) {
                Debug.WriteLine("MileageChartID: {0, -5} Entity Name: {1, -35} Full Name: {2, -25} Location: {3, -30} NIDT: {4, -10} NIDT Miles: {5, -10} Trips: {6, -5} Audit Trail: {7, -20} ", reader[0], reader[1], reader[2], reader[3], reader[4], reader[5], reader[6], reader[7]);
            }
        }//end of connection test





        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////DROP DOWN LIST CONTROLS BEING BOUND BY SQL DATA SOURCES     
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //BINDS THE THE RESULTS OF THE QUERY TO THE FISCAL YEAR DROP DOWN LIST
        private void BindDropDown_fiscalYear() {
            Debug.WriteLine("\n\n***BIND DROP DOWN FISCAL YEAR***\n\n");

            string select = "SELECT DISTINCT[FiscalYear] FROM [Sapphire].[dbo].[view_ancillary_Staff] ORDER BY [FiscalYear] DESC";

            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(select, conn);
            SqlDataReader reader = cmd.ExecuteReader();


            DataSet ds = new DataSet();
            DropDownList_FiscalYear.DataSource = reader;
            DropDownList_FiscalYear.DataValueField = "FiscalYear";
            DropDownList_FiscalYear.DataTextField = "FiscalYear";
            DropDownList_FiscalYear.DataBind();
            conn.Close();


        }//end of bind data to drop down list fiscal year

        //BINDS THE THE RESULTS OF THE QUERY TO THE NAME DROP DOWN LIST
        private void BindDropDown_name() {
            Debug.WriteLine("\n\n***BIND DROP DOWN NAME***\n\n");
            
            //get the fiscal year from the drop down list to populate the name drop down list
            string fy = DropDownList_FiscalYear.Text;

            //select statement
            string select = "SELECT DISTINCT [LastName] + ', ' + [FirstName] as LnFname FROM [Sapphire].[dbo].[view_ancillary_Staff] WHERE [FiscalYear]=@fiscalyear ORDER BY LnFname";

            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(select, conn);
            cmd.Parameters.AddWithValue("@fiscalyear", fy);
            SqlDataReader reader = cmd.ExecuteReader();


            DataSet ds = new DataSet();
            DropDownList_FullName.DataSource = reader;
            DropDownList_FullName.DataValueField = "LnFname";
            DropDownList_FullName.DataBind();
            conn.Close();

            DropDownList_FullName.Items.Insert(0, "Select Therapist...");
            DropDownList_FullName.SelectedValue = "Select Therapist...";
           
            

        }//end of bind data to drop down list name

        ////BINDS THE THE RESULTS OF THE QUERY TO THE SERVICE CODE DROP DOWN LIST
        private void BindDropDown_svcCode() {
            Debug.WriteLine("\n\n***BIND DROP DOWN POSITION***\n\n");
            
            //get the fiscal year
            string fy = DropDownList_FiscalYear.Text;

            //get the name in the text box and split it for a proper query
            string na = DropDownList_FullName.Text;            
            
            //QUERY
            string select = "SELECT [Position], [ServiceCode] FROM [Sapphire].[dbo].[view_ancillary_Staff] WHERE ([FiscalYear]=@fiscalyear AND [FullName]=@FullName) ORDER BY [Position] DESC";

            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(select, conn);
            cmd.Parameters.AddWithValue("@fiscalyear", fy);
            cmd.Parameters.AddWithValue("@FullName", formatNameforQuery(na));
            //cmd.Parameters.AddWithValue("@firstN", first);
            SqlDataReader reader = cmd.ExecuteReader();

            //prepare to bind drop down to data source
            DataSet ds = new DataSet();
            DropDownList_Position.DataSource = reader;
            DropDownList_Position.DataTextField = "Position";
            DropDownList_Position.DataValueField = "ServiceCode";
            DropDownList_Position.DataBind();
            conn.Close();

            if (DropDownList_Position.Items.Count > 1) {
                DropDownList_Position.Enabled = true;
            }
            else {
                DropDownList_Position.Enabled = false;
            }

           
        }//end of bind data to drop down list service code

        //BINDS THE RESULTS OF THE QUERY TO THE PROGRAM DROP DOWN LIST
        private void BindDropDown_program() {
            Debug.WriteLine("\n\n***BIND DROP DOWN PROGRAM***\n\n");
            
            //get the fiscal year from the drop down
            string fy = DropDownList_FiscalYear.Text;

            //get the name in the text box and split it for a proper query
            string na = DropDownList_FullName.Text;
            
            //QUERY
            string select = "SELECT DISTINCT[Program] FROM [Sapphire].[dbo].[view_ancillary_ProgramsByStaff] WHERE ([FullName]=@FullName AND [FiscalYear]=@FiscalYear)"; 

            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(select, conn);
            cmd.Parameters.AddWithValue("@FullName", formatNameforQuery(na));
            cmd.Parameters.AddWithValue("@FiscalYear", fy);
            SqlDataReader reader = cmd.ExecuteReader();

            //prepare to bind drop down to data source
            DataSet ds = new DataSet();
            DropDownList_Program.DataSource = reader;
            DropDownList_Program.DataValueField = "Program";
            DropDownList_Program.DataBind();
            conn.Close();


            if (DropDownList_Program.Items.Count > 1) {
                DropDownList_Program.Enabled = true;
            }
            else {
                DropDownList_Program.Enabled = false;
            }

           

        }//end of bind data to drop down list program

        ////BINDS THE THE RESULTS OF THE QUERY TO THE DISTRICT DROP DOWN LIST
        private void BindDropDown_district() {
            Debug.WriteLine("\n\n***BIND DROP DOWN DISTRICT***\n\n");

            //get the name in the text box and split it for a proper query
            string na = DropDownList_FullName.Text;
            
            //QUERY
            string select = "SELECT DISTINCT[EntityName] FROM [Sapphire].[dbo].[tblAncillaryMileageChart] WHERE [FullName]=@fullName ORDER BY [EntityName]"; 

            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(select, conn);
            //cmd.Parameters.AddWithValue("@fiscalyear", fy);
            //cmd.Parameters.AddWithValue("@lastN", last);
            //cmd.Parameters.AddWithValue("@firstN", first);
            cmd.Parameters.AddWithValue("@fullName", formatNameforQuery(na));
            SqlDataReader reader = cmd.ExecuteReader();

            //prepare to bind drop down to data source
            DataSet ds = new DataSet();
            DropDownList_Institution.DataSource = reader;
            DropDownList_Institution.DataValueField = "EntityName";
            DropDownList_Institution.DataBind();
            conn.Close();

            //set a default value
            //DropDownList_Institution.Items.Insert(0, "Select District...");
            //DropDownList_Institution.SelectedValue = "Select District...";

            if (DropDownList_Institution.Items.Count > 1) {
                DropDownList_Institution.Enabled = true;
            }
            else {
                DropDownList_Institution.Enabled = false;
            }
            
        }//end of bind data to drop down list service code

        ////BINDS THE THE RESULTS OF THE QUERY TO THE DISTRICT DROP DOWN LIST
        private void BindDropDown_location() {
            Debug.WriteLine("\n\n***BIND DROP DOWN LOCATION***\n\n");

            //get the name in the text box and split it for a proper query
            string na = DropDownList_FullName.Text;            

            //QUERY
            string select = "SELECT DISTINCT[Location] FROM [Sapphire].[dbo].[tblAncillaryMileageChart] WHERE [FullName]=@fullName ORDER BY [Location]";

            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(select, conn);
            //cmd.Parameters.AddWithValue("@fiscalyear", fy);
            //cmd.Parameters.AddWithValue("@lastN", last);
            //cmd.Parameters.AddWithValue("@firstN", first);
            cmd.Parameters.AddWithValue("@fullName", formatNameforQuery(na));
            SqlDataReader reader = cmd.ExecuteReader();

            //prepare to bind drop down to data source
            DataSet ds = new DataSet();
            DropDownList_Location.DataSource = reader;
            DropDownList_Location.DataValueField = "Location";
            DropDownList_Location.DataBind();
            conn.Close();


            //set a default value and add an additional...
            //WORK FROM HOME selection to the drop down
            DropDownList_Location.Items.Insert(0,"Select Location...");
            DropDownList_Location.SelectedValue = "Select Location...";
            DropDownList_Location.Items.Add("Work From Home");

        }//end of bind data to drop down list service code




        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////DROP DOWN LIST SELECTED INDEX CHANGED EVENTS
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //HANDLES DROP DOWN FISCAL YEAR INDEX CHANGED
        protected void DropDownList_fiscalYear_SelectedIndexChanged(object sender, EventArgs e) {
            Debug.WriteLine("\n\n***DROP DOWN FISCAL YEAR SELECTED INDEX CHANGED***\n\n");

            BindDropDown_name();
            BindDropDown_district();           
        }//end of selected index changed fiscal year        
        
        //HANDLES DROP DOWN NAME SELECTED INDEX CHANGED
        protected void DropDownList_name_SelectedIndexChanged(object sender, EventArgs e) {
            Debug.WriteLine("\n\n***DROP DOWN NAME SELECTED INDEX CHANGED***\n\n");

            if (DropDownList_FullName.SelectedIndex == 0) {
                DropDownList_Position.SelectedValue = "Select Position...";
                DropDownList_Program.SelectedValue = "Select Program...";
                DropDownList_Institution.SelectedValue = "Select District";
            }
            else {

                BindDropDown_svcCode();
                UDP_Position.Update(); 

                BindDropDown_district();
                UDP_Institution.Update();

                BindDropDown_program();
                UDP_Program.Update();

                BindGridViewTimesheetsDetailTotals();
                UDP_GridViewTimesheetDetailTotals.Update();
                
            }
        }//end of selected index changed name

        //HANDLES DROP DOWN DISTRICT SELECTED INDEX CHANGED
        protected void DropDownList_districts_SelectedIndexChanged(object sender, EventArgs e) {
            //BindDropDown_location();
            //udpDetails.Update();
        }//end of selected index changed districts




        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////RANDOM FUNCTIONS, BUT VERY IMPORTANT
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //CALCULATES TIME WORKED PROVIDING THE FOLLOWING PARAMETERS
        //@Param: bAPm - Begin time AM/PM selection
        //@Param: eAPm - End time AM/PM selection
        //@Param: bT - Begin time
        //@Param: eT - End time
        private double CalculateHoursWorked(string bAPm, string eAPm, double bT, double eT){


            //hours worked 
            double hW = 0;

            /*HANDLE THE FOLLOWING SCENARIOS FOR BEGIN & END TIME...
                * 1)BEGIN TIME - AM, END TIME - AM
                * 2)BEGIN TIME - AM, END TIME - PM
                * 3)BEGIN TIME - PM, END TIME - AM (least likely, but still possible)
                * 4)BEGIN TIME - PM, END TIME - PM
                *
                *
                * THESE FORMULAS WERE QUICK AND DIRTY AND HAVE NOT BEEN VALIDATED FOR LARGE AMOUNTS OF VARYING DATA
                */
            if (bAPm == "1" && eAPm == "1") {//DropDownList_eTimeAmPm.Text == "AM") {
                if (eT < bT && bT >= 12.0) {
                    hW = (eT - bT) + 12.0;
                }
                else if (eT < bT && bT < 12.0) {
                    hW = (eT - bT) + 12.0;
                }
                else if (bT == eT) {//IF BEGIN TIME AND END TIME ARE THE SAME(PM/PM)
                    hW = 0.0;
                }
                else
                    hW = eT - bT;
            }
            else if (bAPm == "1" && eAPm == "0") {
                if (eT >= 12.0) {//needed to handle when end time is in the 12.# range
                    hW = eT - bT;
                }
                else
                    hW = (12.0 - (bT)) + (eT);
            }
            else if (bAPm == "0" && eAPm == "1") {
                if (eT >= 12.0) {//needed to handle when end time is in the 12.# range
                    hW = eT - bT;
                }
                else
                    hW = (12.0 - (bT)) + (eT);

            }
            else if (bAPm == "0" && eAPm == "0") {
                if (eT < bT && bT >= 12.0) {
                    hW = (eT - bT) + 12.0;
                }
                else if (eT < bT && bT < 12.0) {
                    hW = (eT - bT) + 12.0;
                }
                else if (bT == eT) {//IF BEGIN TIME AND END TIME ARE THE SAME(PM/PM)
                    hW = 0.0;
                }
                else
                    hW = eT - bT;
            }


            /*PLACE TO VERIFY INFORMATION VIA BREAKPOINTS IF NECESSARY BECAUSE..... 
                * WITH THE JQUERY SPINNERS THE NUMBER DISPLAYED IS NOT THE ACTUAL NUMBER.
                * NUMBERS ARE GENERATED BASED OFF OF HOW LONG A USER HAS PRESSED THE UP/DOWN ARROWS.
                * THERE IS THE POTENTIAL THAT A NUMBER CAN DISPLAY AS 2.2, BUT REALLY BE 2.2000000000000021 WHICH
                * WILL CAUSE MATHMATICAL ERRORS IF NOT HANDLED APPROPRIATELY...I THINK I HAVE IT COVERED...FOR NOW
                * 
                * IF THE DIFFERENCE IN TIME IS 0.0 THEN ERRORS ARE THROWN AT THIS POINT.  THIS WILL NEED TO BE FIXED!!!
                * 
                */
            //BREAK POINT TESTING PURPOSES 
            //Debug.WriteLine("BRKPT- TEST");
            //Debug.WriteLine("B Time  " + bT);
            //Debug.WriteLine("E Time  " + eT);

            //Debug.WriteLine("Time worked  " + hW.ToString("#.#"));


            return hW;
        }//END OF CALCULATE HOURS WORKED

        //CALCULATES HOURS PAID
        private double CalculateHoursPaid(GridView mygV, DataTable mydT) {
            //hours paid = hours worked (DC + RS + IDT + (NIDT * .5))            

            //sum the columns that need to be totaled so that the data can be transfered into the queue
            double dc = Double.Parse(mydT.Compute("sum(DC)", "").ToString());
            double rs = Double.Parse(mydT.Compute("sum(RS)", "").ToString()); 
            double idt = Double.Parse(mydT.Compute("sum(IDT)", "").ToString()); 
            double nidt = Double.Parse(mydT.Compute("sum(NIDT)", "").ToString());
            nidt = (nidt * 0.5);
            
            double hP = 0;
            hP = (dc + rs) + (idt + nidt);

            Debug.WriteLine("\n\n\nCalculate Hours Paid---->" + hP + " Hours\n\n\n");
            return hP;
        }//END OF CALCULATE HOURS PAID

        //PREPARES DAILY DETAIL CONTROLS FOR A NEW LINE OF DATA
        private void PrepareControlsForNewLine() {
            //CLEAR OUT ANY SELECTIONS IN THE DAILY DETAILS AND PREPARE FOR A NEW LINE
            DropDownList_taskType.ClearSelection();//TASK TYPE
            DropDownList_Location.ClearSelection();//LOCATION

            //SET BEGIN TIME SPINNER TO END TIME OF PREVIOUS LINE ITEM
            TextBox_spinnerB.Text = TextBox_spinnerE.Text;

            //SET CORRESPONDING AM/PM VALUES
            radListBegin.SelectedValue = radListEnd.SelectedValue;

            //SET FOCUS BACK ON END TIME SPINNER
            TextBox_spinnerE.Focus();

            //SET MILES BACK TO 0            
            TextBox_miles.Text = "";

            //RESET CHECKBOX NON-ANCILLRY JUST IN CASE
            //CheckBox_nonAncillary.Checked = false;
            DropDownList_ancillary.Enabled = false;

            //RESET NOTES FIELD
            TextBox_notes.Text = "";
        }//END OF PREPARE CONTROLS FOR NEW LINE

        //RESETS ALL THE DAILY DETAILS CONTROLS BACK TO THEIR DEFAULT VALUES IN PREPARATION OF A NEW DAY
        private void ResetDailyDetailsControls() {
            //reset date picker               
            TextBox_datepicker.Text = "";
            //reset begin/end times
            TextBox_spinnerB.Text = "7";
            TextBox_spinnerE.Text = "7";
            //reset notes
            TextBox_notes.Text = "";
            //reset miles
            TextBox_miles.Text = "";
            //reset non ancillary
            //CheckBox_nonAncillary.Checked = false;

            DropDownList_Location.ClearSelection();
            DropDownList_taskType.ClearSelection();

            
        }//END OF RESET DAILY DETAIL CONTROLS

        //SETS THE TASK LOG LABELS ACCORDINGLY
        private void setLabels() {
            string ancillary = "";
            DropDownList_ancillary.SelectedValue = ancillary;
            if (ancillary == "No") {
                ancillary = " **Non-Ancillary Time Only** ";
            }
            else {
                ancillary = " **Ancillary Time Only** ";
            }
            
            
            //ADD INFORMATION TO THE TASK LOG HEADER SECTION FOR PRINTING PURPOSES
            Label_timesheet_name.Text = DropDownList_FullName.Text + ", " + DropDownList_Position.Text;
            Label_timesheet_date.Text = TextBox_datepicker.Text + ancillary;
            Label_timesheet_district.Text = DropDownList_Institution.Text;



        }//END OF SET LABELS

        //SETS THE TASK LOG LABELS ACCORDINGLY
        private void resetLabels() {
            //ADD INFORMATION TO THE TASK LOG HEADER SECTION FOR PRINTING PURPOSES
            Label_timesheet_name.Text = "";
            Label_timesheet_date.Text = "";
            Label_timesheet_district.Text = "";

        }//END OF SET LABELS

        //REFORMATS NAME TO A PROPER STRING TO QUERY THE DATA BASE (Last, First is returned as First Last)
        private string formatNameforQuery(string name) {
            string fmtFullName = "";

            string[] lname = name.Split(',');

            //first name result
            string first = "";
            //last name result
            string last = "";
            foreach (string word in lname) {
                //verify
                Debug.WriteLine("\n\n\n\nBIND DROP DOWN LOCATION LOOP TEST1-->  " + word);

                //set-em
                last = lname[0];
                first = lname[1];

            }
            //trim any white space
            first = first.Trim();
            last = last.Trim();

            //verify
            Debug.WriteLine("\n\n\n\nBIND DROP DOWN LOCATION LAST NAME TRIM TEST2-->  " + last + "  LENGTH--> " + last.Length);
            Debug.WriteLine("\n\n\n\nBIND DROP DOWN LOCATION FIRST NAME TRIM TEST3-->  " + first + "  LENGTH--> " + first.Length);

            fmtFullName = first + " " + last;
            Debug.WriteLine("\n\n\nFULL NAME TEST---> " + fmtFullName + " FULL NAME LENGTH---> " + fmtFullName.Length);
            
            return fmtFullName;
        }//END OF FORMAT NAME FOR QUERY

        //BLOWS AWAY ENTIRE DATABASE...BOTH OF THEM
        protected void Button_obliterateOnClick(object sender, EventArgs e) {
            //SQL DELETE statement from totals
            string statementDeleteAllDetails = "DELETE FROM [Sapphire].[dbo].[tblAncillaryTimesheetsDetailTotals]";
            //SQL DELETE corresponding record from details
            string statementDeleteAllTotals = "DELETE FROM [Sapphire].[dbo].[tblAncillaryTimesheetsDetail]";

             //prepare connection
            SqlConnection connection = new SqlConnection(connString);

            try {//to delete a row in task log totals
                connection.Open();
                SqlCommand cmd = new SqlCommand(statementDeleteAllDetails, connection);                
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

            }
            catch (System.Data.SqlClient.SqlException ex) {//catch any exceptions
                string msg = "Delete * tblDetails:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            finally {//finally dispaly message to user and close connection
                connection.Close();
                //this.ClientScript.RegisterStartupScript(this.GetType(), "Delete * Successful", "<script language=\"javaScript\">" + "alert('Successfully OBLITERATED * FROM tblDetails');" + "</script>");

                try {//to delete a row in task log totals
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(statementDeleteAllTotals, connection);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                }
                catch (System.Data.SqlClient.SqlException ex) {//catch any exceptions
                    string msg = "Delete * tblTotals:";
                    msg += ex.Message;
                    throw new Exception(msg);
                }
                finally {//finally dispaly message to user and close connection
                    connection.Close();
                    this.ClientScript.RegisterStartupScript(this.GetType(), "Delete * Successful", "<script language=\"javaScript\">" + "alert('Successfully OBLITERATED * FROM tblDetails & tblTotals');" + "</script>");
                }//end of total try/catch
            }//end of details try/catch            

            //BindGridViewTimesheetsDetail();

            string name = DropDownList_FullName.Text;
            string date = TextBox_datepicker.Text;
            string district = DropDownList_Institution.Text;
            string ancillary = DropDownList_ancillary.Text;

            BindGridViewTimesheetsDetailTEST(name, date, district, ancillary);


            BindGridViewTimesheetsDetailTotals();
            resetLabels();
            ResetDailyDetailsControls();
            panel_daily_details.Enabled = false;
            panel_information.Enabled = true;

            Button_changeInfo.Visible = true;
            Button_addLine.Visible = true;



        }//END OF OBLITERATE *

        

    }//end of class


}//end of namespace
