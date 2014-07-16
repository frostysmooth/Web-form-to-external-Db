<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Main.aspx.cs" Inherits="MockUp1._Default" Debug="true" ViewStateMode="Enabled" MaintainScrollPositionOnPostback="true"  %>


<asp:Content id="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

    <!--JQUERY IMPORTS -- CUSTOMIZED AND CONTAINS ALL JQUERY WIDGETS AND ICONS JUST CALL AS NEEDED (START BASED THEME) -->
    <link type="text/css" rel="stylesheet" href="styles/jquery-ui-1.9.0.custom.css"/>
    <script  type="text/javascript" src="scripts/jquery-1.8.2.js" ></script>
    <script  type="text/javascript" src="scripts/jquery.mousewheel.js" ></script>
    <script type="text/javascript" src="scripts/jquery-ui-1.9.0.custom.js" ></script>

    <script type="text/javascript" src="scripts/validation.js"></script>

   
    
    <!--BEGIN TIME SPINNER - JQUERY-->
    <script type="text/javascript">
        $(function () {
            $(".TextBox_spinnerB").spinner({
            step: 0.1,
            numberFormat: "n"//allows for the possibility of two decimal places
            //formatting of decimals is obtained from the jquery globalize.js---- https://github.com/jquery/globalize/tree/#format
            });
        }); 
            

        //used to limit values in order to represent time 1.0 - 12.9
        $(function () {
            $("#TextBox_spinnerB").spinner({
                spin: function (event, ui) {
                    $("#TextBox_spinnerE").spinner("value", ui.value);   
                    if (ui.value > 12.9) {
                        $("#TextBox_spinnerB").spinner("value", 1.0);
                        $("#TextBox_spinnerE").spinner("value", 1.0);
                        return false;
                    } else if (ui.value < 1.0) {
                        $("#TextBox_spinnerB").spinner("value", 12.9);
                        $("#TextBox_spinnerE").spinner("value", 12.9);
                        return false;
                    }  //END OF IF
                } //END OF SPIN FUNCTION                  
            }); //END OF TEXT BOX SPINNER
        });//END OF FUNCTION


        $(function () {
            $("#TextBox_spinnerB").spinner({
                stop: function (event, ui) {
                    ValidatorValidate($("#<%= expValidatorBTime.ClientID %>")[0]);
                    ValidatorValidate($("#<%= RequiredFieldValidator1.ClientID %>")[0]);
                    ValidatorValidate($("#<%= RequiredFieldValidator2.ClientID %>")[0]);
                }
            });
        });
        </script>

    <!--BEGIN TIME AM/PM BUTTON SET - JQUERY-->
    <script type="text/javascript">
         $(document).ready(function () {
             $('#radListB').buttonset();
         });
        
    </script>
 
    <!--END TIME SPINNER - JQUERY-->
    <script type="text/javascript">
         $(function () {
             $("#TextBox_spinnerE").spinner({
                 step: 0.1,
                 numberFormat: "n"//allows for the possibility of two decimal places
                 //formatting of decimals is obtained from the jquery globalize.js---- https://github.com/jquery/globalize/tree/#format
             });//END OF SPINNER
         });//END OF FUNCTION


        //used to limit values in order to represent time 1.0 - 12.9
        $(function () {
             $(".TextBox_spinnerE").spinner({
                 spin: function (event, ui) {
                     if (ui.value > 12.9) {
                         $("#TextBox_spinnerE").spinner("value", 1.0);
                         return false;
                     } else if (ui.value < 1.0) {
                         $("#TextBox_spinnerE").spinner("value", 12.9);
                         return false;
                     } //END OF IF
                 } //END OF SPIN FUNCTION
             }); //END OF TEXT BOX SPINNER
         }); //END OF FUNCTION



         $(function () {
             $("#TextBox_spinnerE").spinner({
                 stop: function (event, ui) {
                     ValidatorValidate($("#<%= expValidatorETime.ClientID %>")[0]);
                     ValidatorValidate($("#<%= RequiredFieldValidator2.ClientID %>")[0]);
                 }
             });
         });
         </script>
    
    <!--END TIME AM/PM BUTTON SET - JQUERY-->    
    <script type="text/javascript">
         $(document).ready(function () {
             $('#radListE').buttonset();
         });
        
    </script>
        
    <!--DATE PICKER - JQUERY-->
    <script type="text/javascript">
        $(function () {
            $("#TextBox_datepicker").datepicker({
                numberOfMonths: 2,
                showButtonPanel: true
            });
        });</script>

    <!--INPUT BUTTON APPEARANCE (PRINT TASK LOG, EDIT, & DELETE BUTTONS) - JQUERY-->
    <script type="text/javascript">
        $(document).ready(function () {
            $("input[type=button], button").button().click(function (event) {
                
            });
        });
    </script>

    <!--STYLING OF ASP BUTTONS TO MATCH JQUERY UI BUTTONS - JQUERY-->
    <style type="text/css">
        .button
        {
            font-family: Verdana,Arial,sans-serif; 
            padding: .1em .2em;
            font-size: 1.1em;
            border: 1px solid #4297d7; 
            background: #0078ae url(styles/images/ui-bg_glass_45_0078ae_1x400.png) 50% 50% repeat-x;
            color: #eaf5f7; 
            font-weight: normal; 
            -moz-border-radius-topleft: 5px; -webkit-border-top-left-radius: 5px; -khtml-border-top-left-radius: 5px; border-top-left-radius: 5px;
            -moz-border-radius-topright: 5px; -webkit-border-top-right-radius: 5px; -khtml-border-top-right-radius: 5px; border-top-right-radius: 5px;
            -moz-border-radius-bottomleft: 5px; -webkit-border-bottom-left-radius: 5px; -khtml-border-bottom-left-radius: 5px; border-bottom-left-radius: 5px;
            -moz-border-radius-bottomright: 5px; -webkit-border-bottom-right-radius: 5px; -khtml-border-bottom-right-radius: 5px; border-bottom-right-radius: 5px;
             cursor: pointer; 
        }
        .button:hover
        {
             border: 1px solid #448dae; 
             background: #79c9ec url(styles/images/ui-bg_glass_75_79c9ec_1x400.png) 50% 50% repeat-x;
             font-weight: normal; 
             color: #026890;
        }
        .button:active
        {
            border: 1px solid #acdd4a; 
            background: #6eac2c url(styles/images/ui-bg_gloss-wave_50_6eac2c_500x100.png) 50% 50% repeat-x; 
            font-weight: normal; 
            color: #ffffff;
        }
        .button:focus
        {
            
            border: 1px solid #acdd4a; background: #6eac2c url(images/ui-bg_glass_55_f8da4e_1x400.png) 50% 50% repeat-x; color: #ffffff;
        }
        
    </style>

    <!--CONTAINER FOR THERAPIST INFORMATION-->
    <style type="text/css">        
     .containerInformation
     {
          float:left;
          border:solid 2px #0078ae;
          min-width:300px;
          margin-right:5px;
          padding:1px;
          background-color:white;
          -moz-border-radius: 8px; 
          border-radius: 8px;
          min-height:253px;
     }</style>
     
    <!--CONTAINER FOR DAILY DETAILS-->
    <style type="text/css">
     .containerDailyDetails
     {
          float:right;
          width:660px;
          border:solid 2px #0078ae;
          margin-left:5px;
          padding:1px;
          background-color:white;
         -moz-border-radius: 8px; 
          border-radius: 8px;
          min-height:210px;              
    }     
     </style>

    <!--CREATES A LEFT COLUMN WITHIN DAILY DETAILS-->
    <style type="text/css">
     .leftSide
     {
          float:left;
          width:370px;
          border:solid 1px white;
          margin-right:10px;
          background-color:white;
          
     }</style>

    <!--CONTAINER FOR A SINGLE TASK LOG-->
    <style type="text/css">        
     .containerLog
     {
          float:left;
          border:solid 2px #0078ae;
          margin-top:15px;          
          padding:0px;
          background-color:white;         
          max-width:980px;
          -moz-border-radius: 8px; 
          border-radius: 8px;
     }
     </style>  
   
    <!--PREVENTS RESIZING OF THE TASK DESCRIPTION/NOTES TEXT BOX-->
    <style type="text/css">
     .noResize
     {
         resize:none;
        -moz-border-radius: 8px; 
        border-radius: 8px; 
        font-family:Comic Sans MS; 
     }
        
    </style>

    <!--STYLE FOR 4 CORNER ROUNDED CONTROLS-->
    <style type="text/css">
    .RoundedControls
    {
        font-family:Comic Sans MS; 
        -moz-border-radius: 8px; 
        border-radius: 8px; 
        text-align:center; 
        height:20px;
    }   
    </style>

    <!--STYLE FOR BLUE CONTAINER THAT HOLDS ANCILLARY AND MILES-->
    <style type="text/css">
    .BubbleContainer
    {
        border:1px solid #0078ae; 
        float:right;
        margin:0px 15px 0px 0px; 
        width:95px; 
        text-align:center; 
        padding:2px 2px 2px 2px; 
        background-color:#cce4ee;  
        -moz-border-radius: 8px; 
        border-radius: 8px;
    }   
    </style>

    <!--PRINTS THE CONTENTS OF A SINGLE TASK LOG-->
    <script type="text/javascript">
        function PrintGridData() {

            
            var prtGrid = document.getElementById('<%=GridView_tasklog.ClientID %>');
            prtGrid.border = 0;

            $('td:nth-child(1),th:nth-child(1)').hide();

            $('td:nth-child(15),th:nth-child(15)').hide();

            var prtwin = window.open('', 'PrintGridViewData', 'left=20,top=100,width=900,menubar=1, height=500,toolbar=1,scrollbars=1,status=0,resizable=1');


            prtwin.document.writeln("<div align='center' style='border-bottom-style: groove; border-bottom-width: thin; border-bottom-color: #0A4E7F; color: #0A4E7F '><h3>Task & Travel Log</h3></div>");
            prtwin.document.writeln("Provider Name:  " + document.getElementById('<%=DropDownList_FullName.ClientID %>').value + ", " + document.getElementById('<%=DropDownList_Position.ClientID %>').value + "<br>");
            prtwin.document.writeln("Service Date:  " + document.getElementById('<%=TextBox_datepicker.ClientID %>').value + "<br>");
            prtwin.document.writeln("Service District:  " + document.getElementById('<%=DropDownList_Institution.ClientID %>').value + "<br>");
            
            
            prtwin.document.write(prtGrid.outerHTML);
            prtwin.document.close();

            prtwin.focus();
            //prtwin.print();  OPEN THE PRINTERS WINDOW, BUT IS UNSTABLE FOR SOME REASON
            //prtwin.close();  CLOSES THE WINDOW
            
        }
</script>

    <!--STYLING OF ASP BUTTONS TO MATCH JQUERY UI BUTTONS - JQUERY-->
    <style type="text/css">
        .buttonPrint
        {
            font-family: Verdana,Arial,sans-serif; 
            padding: .1em .2em;
            font-size: 1.1em;
            border: 1px solid #4297d7; 
            background: #f00 url(styles/images/ui-bg_glass_55_f8da4e_1x400.png) 50% 50% repeat-x;
            color: #4297d7; 
            font-weight: normal; 
            -moz-border-radius-topleft: 5px; -webkit-border-top-left-radius: 5px; -khtml-border-top-left-radius: 5px; border-top-left-radius: 5px;
            -moz-border-radius-topright: 5px; -webkit-border-top-right-radius: 5px; -khtml-border-top-right-radius: 5px; border-top-right-radius: 5px;
            -moz-border-radius-bottomleft: 5px; -webkit-border-bottom-left-radius: 5px; -khtml-border-bottom-left-radius: 5px; border-bottom-left-radius: 5px;
            -moz-border-radius-bottomright: 5px; -webkit-border-bottom-right-radius: 5px; -khtml-border-bottom-right-radius: 5px; border-bottom-right-radius: 5px;
             cursor: pointer; 
        }
        .buttonPrint:hover
        {
             border: 1px solid #448dae; 
             background: #f00 url(styles/images/ui-bg_glass_75_79c9ec_1x400.png) 50% 50% repeat-x;
             font-weight: normal; 
             color: #026890;
        }
        .buttonPrint:active
        {
            border: 1px solid #acdd4a; 
            background: #f00 url(styles/images/ui-bg_gloss-wave_50_6eac2c_500x100.png) 50% 50% repeat-x; 
            font-weight: normal; 
            color: #ffffff;
        }
        .buttonPrint:focus
        {
            
            border: 1px solid #acdd4a; background: #f00 url(images/ui-bg_glass_55_f8da4e_1x400.png) 50% 50% repeat-x; color: #ffffff;
        }
        
    </style>
   

    <!--VALIDATION SCRIPTS-->
    <script type="text/javascript">
        /*VALIDATES BEGIN TIME SPINNER TEXT BOX WHEN USER MANUALLY ENTERS TEXT ON KEYPAD AND POPULATES END TIME TEXT BOX SPINNER*/
        function checkBText() {
            var val1 = document.getElementById('<%=TextBox_spinnerB.ClientID %>').value; //BEGIN TIME

            if (val1 > 12.9) {
                $("#TextBox_spinnerB").spinner("value", 12.9);
                //set end time spinner to that of begin time spinner when text has changed via keypad
                $("#TextBox_spinnerE").spinner("value", 12.9);

                return false;
            } else {
                $("#TextBox_spinnerE").spinner("value", val1);
            }


            ValidatorValidate($("#<%= expValidatorBTime.ClientID %>")[0]);
            ValidatorValidate($("#<%= RequiredFieldValidator1.ClientID %>")[0]);

            ValidatorValidate($("#<%= expValidatorETime.ClientID %>")[0]);
            ValidatorValidate($("#<%= RequiredFieldValidator2.ClientID %>")[0]);


        }


        /*VALIDATES END TIME SPINNER TEXT BOX WHEN USER MANUALLY ENTERS TEXT ON KEYPAD, BUT FUNCTION INDEPENDENT OF BEGIN TIME TEXT BOX SPINNER*/
        function checkEText() {
            var val2 = document.getElementById('<%= TextBox_spinnerE.ClientID %>').value; //END TIME

            if (val2 > 12.9) {
                $("#TextBox_spinnerE").spinner("value", 12.9);
                return false;
            }

            ValidatorValidate($("#<%= expValidatorETime.ClientID %>")[0]);
            ValidatorValidate($("#<%= RequiredFieldValidator2.ClientID %>")[0]);

        }


        /*VALIDATES THE TASK TYPE SELECTED AND CONTROLS IF MILES AND NON-ANCILLARY ARE ENABLED *** ONLY WORKS AS EMBEDDED SCRIPT AND NOT EXTERNAL***
        * DC, RS - MILES(DISABLED), NON-ANCILLARY(ENABLED)
        * IDT, NIDT - MILES(ENABLED), NON-ANCILLARY(DISABLED)
        * LB - MILES(DISABLED), NON-ANCILLARY(DISABLED)*/
        function validate() {
            var dropDown = document.getElementById('<%=DropDownList_taskType.ClientID %>')//get drop down id
            var ddlSelectedValue = dropDown.options[dropDown.selectedIndex].value; //value of drop down list
            var textBox = document.getElementById('<%=TextBox_miles.ClientID %>'); //miles
            //var ddl = document.getElementById('<%=DropDownList_ancillary.ClientID %>'); //non ancillary
            if (ddlSelectedValue == "DC" || ddlSelectedValue == "RS") {
                textBox.disabled = true;
                textBox.value = "";
                //ddl.disabled = false;

            } else if (ddlSelectedValue == "LB") {
                textBox.value = "";
                textBox.disabled = true;

                //ddl.value=
                //ddl.checked = false;
                //ddl.disabled = true;

            } else {
                textBox.disabled = false;

                //ddl.checked = false;
                //ddl.disabled = true;

            }
        }



        /*DISPLAYS COUNT OF CHARACTERS IN NOTES LABEL AS TEXT CHANGES*/
        function setCharacters(e) {
            var count = document.getElementById('<%=TextBox_notes.ClientID%>');
            var lblcount = document.getElementById('<%=Label_notesLength.ClientID%>');
            var total = parseInt(count.value.length);
            lblcount.innerHTML = 'Maximum ' + total + '/140 characters';
            return false;
        }


        /*VALIDATES THE MAXIMUM NUMBER OF CHARACTERS WITHIN THE TASK DESCRIPTION/NOTES TEXTBOX - 140 MAX*/
        function setLength(e) {
            var count = document.getElementById('<%=TextBox_notes.ClientID%>');
            var total = parseInt(count.value.length);
            if (total > 139) {
                if (window.event) {
                    window.event.returnValue = false;
                }
                else {
                    if (e.which > 31) {
                        e.preventDefault();
                    }
                    else {
                        return true;
                    }
                }
            }
        }       
    
    </script>
   
    <!--STYLE FOR DROP DOWN LIST CONTROL...ONLY ROUND LEFT SIDE OF CONTROL TO MAINTAIN DROP DOWN ARROW CONTROL-->
    <style type="text/css">  
        .DropDownListCssClass  
        {  
            color:Black;  
            background-color:white;
            font-family:Comic Sans MS;  
            font-size:small;  
            font-style:normal;                 
            -moz-border-radius-topleft: 8px; -webkit-border-top-left-radius: 8px; -khtml-border-top-left-radius: 8px; border-top-left-radius: 8px;   
            -moz-border-radius-bottomleft: 8px; -webkit-border-bottom-left-radius: 8px; -khtml-border-bottom-left-radius: 8px; border-bottom-left-radius: 8px;    
                          
        
        }  
        
        select:disabled
        {
            border: solid 1px silver;
            background-color: #cce4ee;
            color:#0078ae;
        }

    </style>  


</asp:Content>





<asp:Content id="MainContent" runat="server" ContentPlaceHolderID="MainContent">
        <!-- TOKEN SCRIPT MANAGER NEEDED FOR ANY UPDATE PANELS-->
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <!-- CONTAINS USER NAME, IP ADDRESS AND PURCHASE ORDER #-->
        <div style="text-align:left; padding-bottom:15px" >
            <!--label fiscal year-->
            <asp:Label ID="Label_username" runat="server" style="color:Black"></asp:Label>
            <br />
            <asp:Label ID="Label_userIP" runat="server" style="color:Black"></asp:Label>                               
            <br />

            <!--UPDATE PANEL FOR PURCHASE ORDER NUMBER - TRIGGERED BY NAME-->
            <asp:UpdatePanel ID="UDP_PurchaseOrder" runat="server" UpdateMode="Conditional">                
                <ContentTemplate>
                    <asp:Label ID="Label_purchaseOrder" runat="server" style="color:Black"></asp:Label>                               
                </ContentTemplate>
            </asp:UpdatePanel>
            
        </div>

        <!--CONTAINS FISCAL YEAR DROP DOWN AND DELETE * BUTTON-->    
        <div style="text-align:left; padding-bottom:15px" >
            <!--label fiscal year-->
            <asp:Label ID="Label_fy" runat="server" Text="Fiscal Year" Width="75"></asp:Label> 

            <!--fiscal year drop down list-->
            <asp:DropDownList ID="DropDownList_FiscalYear" runat="server" Width="100px" 
                onselectedindexchanged="DropDownList_fiscalYear_SelectedIndexChanged" AutoPostBack="true" TabIndex="0">
            </asp:DropDownList>                 
            
            <asp:Button  ID="Button_obliterate" runat="server" CssClass="button" Text="Delete *" OnClick="Button_obliterateOnClick" style="text-align:right; font-weight:bold" />
                     
        </div>
        <!--FISCAL YEAR DROP DOWN AND DELETE * BUTTON-->

     

               
    <!--PANEL FOR INFORMATION IS USED ONLY FOR SHOW AND HIDE FUNCTIONALIY-->  
    <asp:Panel ID="panel_information" runat="server" >         
        <!--DIV CONTAINER STYLE FOR ALL INFORMATION CONTROLS-->
        <div class="containerInformation">
        
            <!--CONTAINER HEADER-->
            <div class="divLine" ><h3>Information</h3></div>
           
            <!--USED TO GIVE CONTAINER THE PROPER APPEARANCE...TO MATCH THE PADDING IN DAILY DETAILS
            ONLY USED FOR TOP PADDING PURPOSES-->
            <div style="padding-top:10px"></div>
            
            <!--PROVIDES LEFT SIDE PADDING FOR CONTROLS-->
            <div style="padding-left:5px;">


                <!--UPDATE PANEL FOR NAME DROP DOWN LIST - TRIGGERED BY FISCAL YEAR-->
                <asp:UpdatePanel ID="UDP_FullName" runat="server" UpdateMode="Conditional">                     
                    <ContentTemplate>
                        <!--label name-->
                        <asp:Label ID="Label_name" runat="server" Text="Name" Width="75"></asp:Label>        
                        <!--text box name-->
                        <asp:DropDownList ID="DropDownList_FullName" runat="server" Width="205" CssClass="DropDownListCssClass"
                            onselectedindexchanged="DropDownList_name_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                    </ContentTemplate>                    
                </asp:UpdatePanel>
           

                <!--UPDATE PANEL FOR OCCUPATION/POSTIONS DROP DOWN LIST - TRIGGERED BY NAME-->
                <asp:UpdatePanel ID="UDP_Position" runat="server" UpdateMode="Conditional">                     
                    <ContentTemplate>
                        <div style="padding-top:12px; padding-bottom:12px;">
                        <!--label occupation...a.k.a. service code-->
                        <asp:Label ID="Label_occupation" runat="server" Text="Position" Width="75"></asp:Label>        
                        <!--dropdown list occupation a.k.a postion-->
                        <asp:DropDownList ID="DropDownList_Position" runat="server" Width="205px" CssClass="DropDownListCssClass">
                            <asp:ListItem Text="Select Position..." Value="Select Position..."></asp:ListItem>                           
                        </asp:DropDownList>
                        </div>
                    </ContentTemplate>                    
                </asp:UpdatePanel>


                <!--UPDATE PANEL FOR OCCUPATION/POSTIONS DROP DOWN LIST - TRIGGERED BY NAME-->
                <asp:UpdatePanel ID="UDP_Program" runat="server" UpdateMode="Conditional">                     
                    <ContentTemplate>
                        <div style="padding-bottom:12px;">
                            <!--label occupation...a.k.a. service code-->
                            <asp:Label ID="Label_program" runat="server" Text="Program" Width="75"></asp:Label>        
                            <!--dropdown list occupation a.k.a postion-->
                            <asp:DropDownList ID="DropDownList_Program" runat="server" Width="205px" CssClass="DropDownListCssClass">
                                <asp:ListItem Text="Select Program..." Value="Select Program..."></asp:ListItem>                        
                            </asp:DropDownList>
                        </div>
                    </ContentTemplate>                    
                </asp:UpdatePanel>



           
                <div style="padding-bottom:12px">                       
                    <!--label service date-->
                    <asp:Label ID="Label_date" runat="server" Text="Service Date" Width="75"></asp:Label>        
                    <!--text box datepicker-->
                    <asp:TextBox ID="TextBox_datepicker" runat="server" BorderColor="#2E90BD" ClientIDMode='Static' Width="80" CssClass="RoundedControls"></asp:TextBox>                            
                                                           
                            
                    <div ID="Div1" runat="server" class="BubbleContainer">
                        <asp:Label ID="Label1" runat="server" Text="Ancillary" style="color:Black; text-align:center; font-weight:bold; font-size:smaller;"></asp:Label>
                                
                        <asp:DropDownList ID="DropDownList_ancillary" runat="server" Font-Size="Smaller" CssClass="DropDownListCssClass">                              
                            <asp:ListItem Value="Yes" Selected="True" ></asp:ListItem>
                            <asp:ListItem Value="No" ></asp:ListItem>
                        </asp:DropDownList>                         

                    </div> 
                </div>                 
                
            
                <!--UPDATE PANEL FOR DISTRICTS DROP DOWN LIST - TRIGGERED BY NAME-->
                <asp:UpdatePanel ID="UDP_Institution" runat="server" UpdateMode="Conditional">                     
                    <ContentTemplate>
                    <!--label districts-->
                    <asp:Label ID="Label_districts" runat="server" Text="District" Width="75"></asp:Label>        
                    <!--dropdown list districts-->
                    <asp:DropDownList ID="DropDownList_Institution" runat="server" Width="205" CssClass="DropDownListCssClass" 
                        onselectedindexchanged="DropDownList_districts_SelectedIndexChanged">
                        <asp:ListItem Text="Select District..." Value="Select District..." ></asp:ListItem>
                    </asp:DropDownList>
                </ContentTemplate>                    
                </asp:UpdatePanel>

            </div><!--END OF PADDING-->
          
            
            


            <!--BOTTOM DIVIDER LINE STYLING-->
            <div class="divLine" style="padding-top:15px;"></div>

            <!--DIV TO FORMAT ENTER DETAIL BUTTON-->
            <div align="center" style="padding-top:5px; vertical-align:middle;">
                <!--BUTTON FOR ENTERING DETAILS-->
                <asp:Button ID="Button_enterDetails" runat="server" Text="Enter Details" OnClick="Button_enterDetailsClick" CssClass="button" />
            </div>
        </div><!--END OF INFORMATION CONTAINER-->

    </asp:Panel><!--END OF PANEL FOR INFORMATION-->    

    
    <!--PANEL FOR DAILY DETAILS USED ONLY FOR SHOW AND HIDE FUNCTIONALIY--> 
    <asp:Panel ID="panel_daily_details" runat="server"  >

        <!--CONTAINER FOR DAILY DETAILS-->
        <div class="containerDailyDetails" id="container_daily_details" runat="server" >
                
            <!--SECTION HEADER-->
            <div class="divLine"><h3>Daily Details</h3></div>


            
            <!--HOLDS LEFT COLUM CONTROLS WITHIN DAILY DETAILS CONTAINER... 
                * LOCATION
                * BEGIN TIME
                * END TIME
                * TASK TYPE
                * MILES
                * NON-ANCILLARY-->                    
            <div class="leftSide" style="padding-top:10px; padding-left:5px;">          

                <!--UPDATE PANEL FOR LOCATION DROP DOWN LIST-->
                <asp:UpdatePanel ID="UDP_Location" runat="server" UpdateMode="Conditional">       
                    <ContentTemplate>
                    <div style="padding-bottom:10px">                    
                            <!--label for location-->
                            <asp:Label ID="Label_location" runat="server" Text="Location" Width="75"></asp:Label>        
                            <!--drop down list for location-->    
                            <asp:DropDownList ID="DropDownList_Location" runat="server" Width="200" CssClass="DropDownListCssClass">
                                <asp:ListItem>Select Location...</asp:ListItem>
                                <asp:ListItem Text="Elementary"></asp:ListItem>
                                <asp:ListItem Text="Middle School"></asp:ListItem>
                                <asp:ListItem Text="High School"></asp:ListItem>
                            </asp:DropDownList> 
                    </div><!--END OF DIV LOCATION-->
                </ContentTemplate>
                </asp:UpdatePanel>

                   

                <!--HOLDS ALL TASK TYPE CONTROLS-->
                <div style="padding-bottom:5px">
                    <!--label for task type-->
                    <asp:Label ID="Label_taskType" runat="server" Text="Task Type" Width="75" ></asp:Label>        
                    <!--drop down list for task type-->
                    <asp:DropDownList ID="DropDownList_taskType"  Width="180" runat="server" CausesValidation="true" onChange="validate();" CssClass="DropDownListCssClass">
                        <asp:ListItem Text="Direct Contact (DC)" Value="DC"></asp:ListItem>
                        <asp:ListItem Text="Reports/Staffing (RS)" Value="RS"></asp:ListItem>
                        <asp:ListItem Text="Lunch/Break (LB)" Value="LB"></asp:ListItem>
                        <asp:ListItem Text="In-District Travel (IDT)" Value="IDT"></asp:ListItem>
                        <asp:ListItem Text="To/From District (NIDT)" Value="NIDT"></asp:ListItem>
                    </asp:DropDownList>
                </div><!--END OF TASK TYPE CONTROLS-->


                <!--HOLDS ALL BEGIN TIME COLUMN CONTROLS-->
                <table>
                    <tr>                            
                        <td><asp:Label ID="Label_BeginTime" runat="server" Text="Begin Time" Width="70"></asp:Label></td>
                            
                        <td><asp:TextBox ID='TextBox_spinnerB' name='TextBox_spinnerB' Text="7.0" runat='server' ClientIDMode='Static' MaxLength="4" Width="50px" 
                            onkeyup="checkBText();" CausesValidation="true" CssClass="TextBox_spinnerB" > </asp:TextBox>
                        <asp:RegularExpressionValidator ID="expValidatorBTime" Display="Dynamic" runat="server" ForeColor="Red" ErrorMessage="Invalid time" 
                            ValidationExpression="^([1]{1})([0-2]{1})(\.[0-9][0]{1})?$|^([1-9]{1})(\.[0-9]{1}[0]{1})?$|^([1]{1})([0-2]{1})(\.[0-9])?$|^([1-9]{1})(\.[0-9])?$" 
                            ControlToValidate="TextBox_spinnerB"/>   
                        <asp:RequiredFieldValidator id="RequiredFieldValidator1" Display="Dynamic" runat="server" ControlToValidate="TextBox_spinnerB"
                            ErrorMessage="* Required"
                            ForeColor="Red">
                        </asp:RequiredFieldValidator></td>
                            
                        <td><div id="radListB" >  
                            <asp:RadioButtonList ID="radListBegin" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Selected="True">AM</asp:ListItem>
                                <asp:ListItem Value="0">PM</asp:ListItem>
                            </asp:RadioButtonList>                                               
                        </div></td>                
                            
                        <td>                    
                            <div id="Div2" runat="server" class="BubbleContainer">
                                <asp:Label ID="Label_miles" runat="server" Text="Miles" style="color:Black;" Font-Bold="true" Font-Size="Smaller" ></asp:Label>                 
                                 
                                <asp:TextBox ID="TextBox_miles" runat="server" BorderColor="#2E90BD" Font-Size="Small" MaxLength="3" Width="40" Enabled="false" style="height:18px;" CssClass="RoundedControls"></asp:TextBox>   
                        </div>
                        </td>
                    </tr>
                </table><!--END OF BEGIN TIME COLUMN CONTROLS-->
                        
                        
                        
                <!--HOLDS ALL END TIME COLUMN CONTROLS-->
                <table>
                    <tr>                                                        
                        <td><asp:Label ID="Label_endTime" runat="server" Text="End Time" Width="70"></asp:Label></td>
                            
                        <td><asp:TextBox ID='TextBox_spinnerE' name='TextBox_spinnerE' Text="7.0" runat='server' MaxLength="4" ClientIDMode='Static' Width="50px"
                            onkeyup="checkEText();" CausesValidation="true" CssClass="TextBox_spinnerE"/>
			            <asp:RegularExpressionValidator ID="expValidatorETime" Display="Dynamic" runat="server" ForeColor="Red" ErrorMessage="Invalid time" 
                            ValidationExpression="^([1]{1})([0-2]{1})(\.[0-9][0]{1})?$|^([1-9]{1})(\.[0-9]{1}[0]{1})?$|^([1]{1})([0-2]{1})(\.[0-9])?$|^([1-9]{1})(\.[0-9])?$" 
                            ControlToValidate="TextBox_spinnerE"  />
                        <asp:RequiredFieldValidator id="RequiredFieldValidator2" Display="Dynamic" runat="server" ControlToValidate="TextBox_spinnerE"
                            ErrorMessage="* Required"
                            ForeColor="Red">
                        </asp:RequiredFieldValidator></td>
                            
                        <td>
                            <div id="radListE">
                                <asp:RadioButtonList ID="radListEnd" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Selected="True">AM</asp:ListItem>
                                    <asp:ListItem Value="0">PM</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </td>          
                           
                    </tr>
                </table><!--END OF END TIME COLUMN CONTROLS-->


            </div><!--END OF LEFT COLUMN CONTAINER WITHIN DAILY DETAILS-->
                
                
                
            <!--HOLDS RIGHT COLUMN CONTROLS WITHIN DAILY DETAILS CONTAINER * NOTES/TASK DESCRIPTION-->                
            <div  style="padding-top:5px">  
                <!--label for notes-->
                <asp:Label ID="Label_notes" runat="server" Text="Task Description/Notes"></asp:Label>
                    
                <br />
                <!--text box for notes-->
                <asp:TextBox ID="TextBox_notes" runat="server"  CssClass="noResize" 
                    onkeypress="return setLength(event);" 
                    onkeyup="return setCharacters(event);"
                    TextMode="MultiLine" 
                    Height="80" Width="260"
                    BorderColor="#2E90BD">
                </asp:TextBox>
                    
                <!--LABEL THAT DISPLAYS THE TOTAL NUMBER OF CHARACTERS ENTERED IN THE NOTES FIELD-->
                <asp:Label ID="Label_notesLength" runat="server" Width="260px" Text="Maximum 0/140 characters" CssClass="rightAlign"></asp:Label>
            </div><!--END OF RIGHT COLUMN CONTROLS-->
                
            <!--bottom divider line seperator-->
            <div class="divLine" style="padding-top:27px"></div>
     
            <!--HOLDS BACK AND ADD LINE BUTTON CONTROLS ONLY-->
            <div align="center" style="padding-top:1px">
                    <!--table used as a container to format the position of two buttons-->
                <table>
                    <tr>
                        <!--button for changing information-->
                        <td><asp:Button ID="Button_changeInfo" runat="server" OnClick="Button_changeInfoClick" Text="Back" CssClass="button" /></td>
                        <!--button for entering a new line of intormation-->
                        <td><asp:Button ID="Button_addLine" runat="server" OnClick="Button_addLineClick" Text="Add Line" CssClass="button" /></td>
                    </tr>
                </table>
            </div><!--END OF CONTAINER FOR BUTTON CONTROLS-->

        </div><!--END OF CONTAINER DAILY DETAILS-->  

            
    </asp:Panel><!--END OF PANEL FOR DAILY DETAILS-->
    






   
    <!--CONTAINER FOR A SINGLE TASK LOG--> 
    <div class="containerLog" id="container_tasklog" runat="server" style="white-space:nowrap;  min-width:980px; overflow:auto;" >
        
        <!--CONTAINER HEADER-->
        <div class="divLine"><h3>Task Log</h3></div>            


         <!--UPDATE PANEL FOR TASK LOG LABLES-->
        <asp:UpdatePanel ID="UDP_TaskLogLabels" runat="server" UpdateMode="Conditional">          
            <ContentTemplate>
            <div style="padding-left:10px">         
                <!--label for therapist name on snapshot-->
                <asp:Label runat="server" style="color:Black" Text="Provider:  " Font-Bold="true"></asp:Label>
                <asp:Label ID="Label_timesheet_name" runat="server"></asp:Label><br />
                
                <!--label for service date on snapshot-->
                <asp:Label runat="server" style="color:Black" Text="Service Date:  " Font-Bold="true"></asp:Label>
                <asp:Label ID="Label_timesheet_date" runat="server"></asp:Label><br />
               
                <!--label for service district on snapshot-->
                <asp:Label runat="server" style="color:Black" Text="District:  " Font-Bold="true"></asp:Label>
                <asp:Label ID="Label_timesheet_district" runat="server"></asp:Label><br />
                <br />
                <br />          
            </div> 
            </ContentTemplate> 
        </asp:UpdatePanel> 
               
        
        <!--UPDATE PANEL FOR TASK LOG DETAILS GRID VIEW-->
        <asp:UpdatePanel ID="UDP_GridViewTimesheetDetails" runat="server" UpdateMode="Conditional">             
            <ContentTemplate>
            <!--GRID VIEW TASKLOG-->
            <asp:GridView ID="GridView_tasklog" runat="server"
            AutoGenerateColumns="False" 
            ShowHeaderWhenEmpty="False"
            EditRowStyle-BorderColor="Red"
            EditRowStyle-BorderStyle="Solid"
            EditRowStyle-BorderWidth="2px"
            EditRowStyle-Font-Bold="true"
            CellPadding="2" 
            HorizontalAlign="Center" BackColor="White" BorderColor="#cce4ee" 
            BorderStyle="Solid" BorderWidth="1px" ShowFooter="True" 
            OnRowDataBound="GridView_tasklog_RowDataBound"             
            OnRowEditing="GridView_tasklog_RowEditing"
            OnRowUpdating="GridView_tasklog_RowUpdating"           
            onrowdeleting="GridView_tasklog_RowDeleting"
            OnRowCancelingEdit="GridView_tasklog_RowCancelingEdit">
                                
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <Columns>                             
                
                <asp:TemplateField>            
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" CommandName="Edit" Text="E" CssClass="button" />                                           
                    </ItemTemplate>                   
                   

                    <EditItemTemplate>                   
                        <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="U" CssClass="button"/>
                        <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="C" CssClass="button" CausesValidation="false" />                        
                    </EditItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" Width="35px"/>
                    <ItemStyle HorizontalAlign="Center"/>     
                </asp:TemplateField>                                   
                
               


                <asp:BoundField HeaderText="TimesheetDetailID" DataField="TimesheetDetailID" ReadOnly="true" Visible="true" >
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>

                <asp:BoundField HeaderText="TimesheetDetailTotalID" DataField="TimesheetDetailTotalID" ReadOnly="true" Visible="true" >
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>


                <asp:BoundField HeaderText="FullName" DataField="FullName" ReadOnly="true" Visible="true" >
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>

                 <asp:BoundField HeaderText="ServiceCode" DataField="ServiceCode" ReadOnly="true" Visible="true" >
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>

                 <asp:BoundField HeaderText="ServiceDate" DataField="ServiceDate" ReadOnly="true" Visible="true" DataFormatString="{0:MM/dd/yyyy}">
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>

                 <asp:BoundField HeaderText="InstitutionName" DataField="InstitutionName" ReadOnly="true" Visible="true" >
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>               
                      
                <asp:BoundField HeaderText="BeginTime" DataField="BeginTime" ReadOnly="true" DataFormatString="{0:N1}">
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle Width="50px"/>                      
                </asp:BoundField>       

                
                <asp:BoundField HeaderText="BeginTimeAMPM" DataField="BeginTimeAMPM" ReadOnly="true">
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle Width="50px"/>
                </asp:BoundField>


                <asp:BoundField HeaderText="EndTime" DataField="EndTime" ReadOnly="true" DataFormatString="{0:N1}">
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle Width="50px"/>
                </asp:BoundField>


                <asp:BoundField HeaderText="EndTimeAMPM" DataField="EndTimeAMPM" ReadOnly="true">
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle Width="50px"/>
                </asp:BoundField>


                <asp:BoundField HeaderText="Location" DataField="Location" ReadOnly="true">
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle Width="150px" />
                </asp:BoundField>


                <asp:BoundField HeaderText="Ancillary" DataField="Ancillary" ReadOnly="true">
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle Width="20px"></HeaderStyle>
                </asp:BoundField>


                <asp:BoundField HeaderText="DC" DataField="DC" ReadOnly="true" DataFormatString="{0:N1}">
                    <HeaderStyle Width="35px"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>


                <asp:BoundField HeaderText="RS" DataField="RS" ReadOnly="true" DataFormatString="{0:N1}">
                    <HeaderStyle Width="35px" HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>


                <asp:BoundField HeaderText="IDT" DataField="IDT" ReadOnly="true" DataFormatString="{0:N1}">
                    <HeaderStyle Width="35px" HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>

                <asp:BoundField HeaderText="NIDT" DataField="NIDT" ReadOnly="true" DataFormatString="{0:N1}">
                    <HeaderStyle Width="35px" HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>


                <asp:BoundField HeaderText="IDTMiles" DataField="IDTMiles" ReadOnly="true" DataFormatString="{0:N0}">
                    <HeaderStyle Width="35px" HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>                


                 <asp:BoundField HeaderText="NIDTMiles" DataField="NIDTMiles" ReadOnly="true" DataFormatString="{0:N0}">
                    <HeaderStyle Width="35px" HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                

                <asp:TemplateField HeaderText="Notes">
                    <ItemTemplate>                        
                        <div style="overflow:hidden; white-space:nowrap; ">
                            <asp:Label ID="Label6" runat="server" Width="80px"  Text='<%# Bind("Notes") %>' ToolTip='<%# Bind("Notes") %>'></asp:Label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" Width="80px" />  
                    
                    
                    <FooterTemplate>
                        <asp:Label ID="Label_perDiem" runat="server" Text="Per Diem" Width="60" Font-Bold="True"></asp:Label>
                        <asp:DropDownList ID="DropDownList_perDiem" runat="server" Width="60">
                            <asp:ListItem Selected="True" Text="None"  Value="0.0"/>
                            <asp:ListItem Text="1.0" Value="1.0" />
                            <asp:ListItem Text="0.5" Value="0.5"/>              
                        </asp:DropDownList>   
                    </FooterTemplate>                      
                </asp:TemplateField>


                <asp:BoundField HeaderText="Trips" DataField="Trips" ReadOnly="true" Visible="true">
                    <HeaderStyle Width="35px" HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>

                <asp:TemplateField>
                    <ItemTemplate>
                         <asp:ImageButton ID="btnDelete" CommandName="Delete" CssClass="button" runat="server" 
                            ImageUrl="~/Styles/images/delX.png" style=" width:15px; margin-top:2px; padding-top:3px; padding-bottom:3px"
                            CausesValidation="true" OnClientClick="return confirm('Are you sure? This operation cannot be undone!')"/>                       
                    </ItemTemplate>          
                    <HeaderStyle HorizontalAlign="Center" Width="35px"/>
                    <ItemStyle HorizontalAlign="Center" />                          
                </asp:TemplateField>              
            </Columns>
                
            <EditRowStyle BorderColor="Red" BorderWidth="2px" BorderStyle="Solid"></EditRowStyle>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Center" Font-Bold="true"/>
            <HeaderStyle BackColor="#93c6dc" Font-Bold="True" ForeColor="Black" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#0000A9" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#000065" />               
        </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel> 

        
        

        <!--HOLDS LOG COMPLETE AND PRINT LOG BUTTONS-->
        <div style="padding: 0px 0px 20px 40px">
            <asp:Button ID="Button_logComplete" runat="server" Text="Log Complete" OnClick="Button_logCompleteClick" CssClass="button" Visible="false"/>

            <asp:ImageButton ID="Button_PrintLog" runat="server" ToolTip="Print My Task Log" OnClientClick="PrintGridData()" ImageUrl="~/Styles/images/print_ico.png" CssClass="buttonPrint"
                            style="width:2%; padding:3px 3px 3px 3px" Visible="false" />   
        </div>
          

    </div>

    <br />

            
    <!--CONTAINER FOR TASK LOG TOTALS-->
    <div class="containerLog" id="container_tasklogTotal" runat="server" style="min-width:980px; overflow:auto;" >
        <!--SETION HEADER-->
        <div class="divLine"><h3>Employee Task Log Totals</h3></div>
        
        <br />
        <br />
        
        <!--UPDATE PANEL FOR GRID VIEW TASK LOG DETAIL TOTALS - TRIGGERED BY NAME-->
        <asp:UpdatePanel ID="UDP_GridViewTimesheetDetailTotals" runat="server" UpdateMode="Conditional">             
            <ContentTemplate>            
            <asp:GridView ID="GridView_tasklogTotal" runat="server"
               AutoGenerateColumns="False" 
                ShowHeaderWhenEmpty="False"
                EditRowStyle-BorderColor="Red"
                EditRowStyle-BorderStyle="Solid"
                EditRowStyle-BorderWidth="2px"
                EditRowStyle-Font-Bold="true"
                CellPadding="2" 
                HorizontalAlign="Center" BackColor="White" BorderColor="#2E90BD" 
                BorderStyle="Solid" BorderWidth="2px" ShowFooter="True"
                OnRowDataBound="GridView_tasklogTotals_RowDataBound"             
                OnRowEditing="GridView_tasklogTotals_RowEditing"
                OnRowUpdating="GridView_tasklogTotals_RowUpdating"           
                onrowdeleting="GridView_tasklogTotals_RowDeleting"
                OnRowCancelingEdit="GridView_tasklogTotals_RowCancelingEdit">
                                
                <AlternatingRowStyle BackColor="#DCDCDC" />
                <Columns>
                    <asp:TemplateField>            
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" CommandName="Edit" Text="E" CssClass="button" />                                           
                        </ItemTemplate>     
                     
                        <FooterTemplate>                                                
                            <asp:ImageButton ID="btnPrintLog" runat="server" ImageUrl="~/Styles/images/print_ico.png" CssClass="buttonPrint"
                                style="width:65%; padding:3px 3px 3px 3px"  />                    
                        </FooterTemplate>                                   
                                   

                        <EditItemTemplate>                   
                            <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="U" CssClass="button"/>
                            <asp:Button ID="btnCancel" runat="server" CommandName="Cancel" Text="C" CssClass="button" CausesValidation="false" />                        
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="45px"/>
                        <ItemStyle HorizontalAlign="Center"/>     
                    </asp:TemplateField>                   
                
               

                    <asp:BoundField HeaderText="TimesheetDetailTotalID" DataField="TimesheetDetailTotalID" ReadOnly="true">
                        <HeaderStyle Width="35px" HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                
                    <asp:TemplateField HeaderText="Submit" ItemStyle-HorizontalAlign="Center" >
	                    <ItemTemplate>
	                        <asp:CheckBox ID="CheckBox_submit" runat="server"/>
	                    </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
	                </asp:TemplateField>


                     <asp:BoundField HeaderText="InstitutionName" DataField="InstitutionName" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>


                     <asp:BoundField HeaderText="Program" DataField="Program" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                     <asp:BoundField HeaderText="PO" DataField="PO" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                     <asp:BoundField HeaderText="ServiceCode" DataField="ServiceCode" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>


                    <asp:BoundField HeaderText="FullName" DataField="FullName" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="EntryDate" DataField="EntryDate" ReadOnly="true">
                        <HeaderStyle Width="35px" HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>               
                
                
                    <asp:BoundField HeaderText="ServiceDate" DataField="ServiceDate" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>             
               

                    <asp:BoundField HeaderText="Ancillary" DataField="Ancillary" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                
                    <asp:BoundField HeaderText="DC" DataField="DC" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                
                    <asp:BoundField HeaderText="RS" DataField="RS" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                
                    <asp:BoundField HeaderText="IDT" DataField="IDT" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>     

                  
                    <asp:BoundField HeaderText="NIDT" DataField="NIDT" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>     


                    <asp:BoundField HeaderText="OffSitePrep" DataField="OffSitePrep" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>


                    <asp:BoundField HeaderText="OnSiteTotal" DataField="OnSiteTotal" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>                
                           
               
                    <asp:BoundField HeaderText="HoursWorked" DataField="HoursWorked" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>


                    <asp:BoundField HeaderText="HoursPaid" DataField="HoursPaid" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />                    
                    </asp:BoundField>


                    <asp:BoundField HeaderText="EmployeeTotal" DataField="EmployeeTotal" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                
                    <asp:BoundField HeaderText="DistrictTotal" DataField="DistrictTotal" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>


                    <asp:BoundField HeaderText="Trips" DataField="Trips" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>  
                              

                    <asp:BoundField HeaderText="IDTMiles" DataField="IDTMiles" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>


                    <asp:BoundField HeaderText="NIDTMiles" DataField="NIDTMiles" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>


                     <asp:BoundField HeaderText="EmployeeIDTMileageTotal" DataField="EmployeeIDTMileageTotal" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>


                    <asp:BoundField HeaderText="DistrictIDTMileageTotal" DataField="DistrictIDTMileageTotal" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                

                    <asp:BoundField HeaderText="EmployeeNIDTMileageTotal" DataField="EmployeeNIDTMileageTotal" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                
                    <asp:BoundField HeaderText="DistrictNIDTMileageTotal" DataField="DistrictNIDTMileageTotal" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="EmployeeTotalMileage" DataField="EmployeeTotalMileage" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                     <asp:BoundField HeaderText="DistrictTotalMileage" DataField="DistrictTotalMileage" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="PerDiemDays" DataField="PerDiemDays" DataFormatString="{0:N1}" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="EmployeePerDiemTotal" DataField="EmployeePerDiemTotal" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                     <asp:BoundField HeaderText="DistrictPerDiemTotal" DataField="DistrictPerDiemTotal" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="EmployeeTravelTotal" DataField="EmployeeTravelTotal" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="DistrictTravelTotal" DataField="DistrictTravelTotal" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="EmployeeTimesheetTotal" DataField="EmployeeTimesheetTotal" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>

                     <asp:BoundField HeaderText="DistrictTimesheetTotal" DataField="DistrictTimesheetTotal" ReadOnly="true">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                
                
                    <asp:TemplateField>
                    <ItemTemplate>
                         <asp:ImageButton ID="btnDelete_totals" CommandName="Delete" CssClass="button" runat="server" 
                            ImageUrl="~/Styles/images/delX.png" style=" width:15px; margin-top:2px; padding-top:3px; padding-bottom:3px"
                            CausesValidation="true" OnClientClick="return confirm('Are you sure you want to delete this Task Log? This operation cannot be undone!')"/>                       
                    </ItemTemplate>          
                    <HeaderStyle HorizontalAlign="Center" Width="35px"/>
                    <ItemStyle HorizontalAlign="Center" />                          
                </asp:TemplateField>             
                </Columns>
                
                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Center" Font-Bold="true"/>
                <HeaderStyle BackColor="#2E90BD" Font-Bold="True" ForeColor="Black" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#0000A9" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#000065" />                
            </asp:GridView>
            </ContentTemplate>                    
        </asp:UpdatePanel>
        
       
    </div>

   

     
</asp:Content>
