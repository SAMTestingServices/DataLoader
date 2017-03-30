
using System.Collections.Generic;
using System.Linq;

using System.Data.SqlClient;
using System.Data;
using Dapper;
using System.Data.OleDb;
using System.Configuration;

using System.Diagnostics;
using System;
using System.Windows.Forms;
using LAFBulkDataLoader;
using Lloyds.LAF.Agent.Restricted.Contracts.Tasks;
using Lloyds.LAF.Agent.Restricted.Domain.Search;
using Lloyds.LAF.Agent.Restricted.Domain;

namespace WindowsFormsApplication3
{
    public enum Actions
    {
        CreateCredentialValues,
        CreateUserGroups,
        AddCredentialValuesToUserGroups,
        AddUsersToUserGroups,
     }

    class DataProcessingMethods
    {

        static string database = ConfigurationManager.AppSettings["Database"];
        static IUserGroupTask userGroupTasks = Lloyds.LAF.Agent.Restricted.Factory.Resolve<IUserGroupTask>();
        static string dbInstance  = ConfigurationManager.AppSettings["Connection"];
        static IDbConnection conn = DataProcessingMethods.GetDBConnection(dbInstance);
        static string auditText1 = ConfigurationManager.AppSettings["AuditIdentifierText1"];
        static string auditText2 = ConfigurationManager.AppSettings["AuditIdentifierText2"];
        static string currentUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        static string DA_UserGroupDescription = ConfigurationManager.AppSettings["DA_UserGroup_CredDescription"];
        static string LAFRole_CredDescription = ConfigurationManager.AppSettings["LAFRole_CredDescription"];
        static string DA_CredValue = ConfigurationManager.AppSettings["DA_CredValue"];
        static string DA_UserGroupName_Prefix = ConfigurationManager.AppSettings["DA_UserGroupName_Prefix"];
        static string DA_UserGroupDescription_Prefix = ConfigurationManager.AppSettings["DA_UserGroupDescription_Prefix"];


        public static IDbConnection GetDBConnection(string datasource)
        {

            string connString = @"Data Source=" + datasource + ";Initial Catalog="
                     + database + ";Integrated Security = True;";

            SqlConnection conn = new SqlConnection(connString);

            return conn;
        }



        public static void InsertCredentialValues(List<CredentialValues> credentialList,  string credentialTypeDescription, string changeDescription, string fileName, ProgressBar progressBar1, CheckBox chkAudit, string appName, string credURNName, string appuRL, string Urn, int ApplicationID, int CredentialID, int urnID, string sourceApplicationuRL)
        {


            progressBar1.Value = 0;


            
            string uRL = auditText1 + currentUser + auditText2 + changeDescription;

            

            int i = 1;

            DateTime dtm = DateTime.Now;

            List<IInputData> successList = new List<IInputData>();

            progressBar1.Step = 1;
            progressBar1.Maximum = credentialList.Count();

            var credentialTasks = Lloyds.LAF.Agent.Restricted.Factory.Resolve<ICredentialTasks>();



            foreach (CredentialValues row in credentialList)
            {

                progressBar1.PerformStep();

                int result = InsertCred(row.Value, row.Description, fileName, uRL, chkAudit.Checked, i, credentialTypeDescription, appName, credURNName, appuRL, Urn, ApplicationID, sourceApplicationuRL, credentialTasks);

                //int result = RestrictedAgentMethods.AddCredentialRA(row.Value, row.Description, fileName, uRL, chkAudit.Checked, i, credentialTypeDescription, appName, false);

                if (result > 0)
                {
                    if (!chkAudit.Checked)
                    {
                        ExcelMethods.WriteToCell(i, "Status", "Credential Value successfully created ", "CredentialValues", fileName);
                        row.CredentialValueID = result;
                        successList.Add(row);
                    }
                    else
                    {
                        ExcelMethods.WriteToCell(i, "Status", "Audit record successfully created", "CredentialValues", fileName);
                        row.CredentialValueID = result;
                        successList.Add(row);
                    }
                }
                else if (result == -2)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Credential Value NOT  created - BLANK VALUE NOT PERMITTED", "CredentialValues", fileName);
                }
                else if (result == -1)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Credential Value NOT  created - value already exists (duplicate)", "CredentialValues", fileName);
                }
                else if (result == -12)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Audit record not created - Credential Value does not exist", "CredentialValues", fileName);
                }
                else if (result == -9)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Exception - Credential Value not created", "CredentialValues", fileName);
                }



                i++;


            }

            //Need to add some error handline around audit entries

            try
            {
                if (successList.Count > 0) DataProcessingMethods.InsertAuditEntriesCredValues(successList, 9000, uRL, CredentialID, ApplicationID, credentialTypeDescription, urnID, dtm);
                if (!chkAudit.Checked) { MessageBox.Show("Upload complete. Number of Credential Values (+ audit entries) successfully created = " + successList.Count.ToString() + " out of " + credentialList.Count + " attempted. See input excel file for individual upload status.", "Upload Complete"); }
                else { MessageBox.Show("Upload complete. Number of Audit entries successfully created = " + successList.Count.ToString() + " out of " + credentialList.Count + " attempted. See input excel file for individual upload status.", "Upload Complete"); }
            }
            catch
            {
                if (!chkAudit.Checked) { MessageBox.Show("Unknown Exception while adding Audit entries. However, the credential value records themselves were added. Number of records credential values created = " + successList.Count.ToString() + " out of " + credentialList.Count + " attempted. See input excel file for individual upload status.", "Upload Complete - Error adding Audit Records"); }
                else { MessageBox.Show("Unknown Exception. Audit entries not added.", "Unknown Exception"); }
            }

            progressBar1.Value = 0;


        }

        internal static bool CheckIfStoredProcsExist(string storedProcKey)
        {
            

            string storedProc = ConfigurationManager.AppSettings[storedProcKey];
            string database = ConfigurationManager.AppSettings["Database"];
            try {
                conn.Open();
                string query = String.Format(" EXEC {0}.{1}", database, storedProc);


                conn.Query(query);
                conn.Close();
                //Will never execute next line (intended):
                return true;

            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Could not find stored procedure"))
                {

                    MessageBox.Show(String.Format("This option is not available - the stored proc with name '{0}' does not exist in the target database. Verify name is correct in app.config - see key '{1}'", storedProc, storedProcKey), "Function unavailable - stored procedure could not be found",
MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conn.Close();
                    return false;
                }

                else
                {
                    conn.Close();
                    return true;
                }
            }
            
        }

        static int InsertCred(string credentialValueToAdd, string credentialDescriptionToAdd,  string fileName, string uRL, bool auditOnly, int i, string credentialTypeDescription, string appName, string credURNName, string appuRL, string Urn, int ApplicationID, string sourceApplicationuRL, ICredentialTasks credentialTasks)
        {


            Debug.WriteLine("Inside Insert Cred");


            if (credentialValueToAdd == "")
            {


                return -2;
            }

            //First make sure that the credential does not exist for that credential type...


            CredentialValueSearchOptions search1 = new CredentialValueSearchOptions();
            search1.Value = credentialValueToAdd;
            search1.Urn = Urn;
            search1.ApplicationId = ApplicationID;

            var credentialValuesFound = credentialTasks.SearchForCredentialValues(search1);



            if (!auditOnly) // i.e. Normal Mode (add Cred Values)
            {

                if (credentialValuesFound.Count > 0)
                {
                    return -1;
                }
                else
                {


                    try
                    {
                        CredentialValue addedValue;
                        if (!(appName == "LAF")) { addedValue = credentialTasks.AddCredentialValue(appuRL, credURNName, credentialValueToAdd, credentialDescriptionToAdd); }
                        else { addedValue = credentialTasks.AddLafCredentialValue(appuRL, sourceApplicationuRL, credURNName, credentialValueToAdd, credentialDescriptionToAdd); }
                        return addedValue.CredentialValueId;


                    }
                    catch
                    {
                        return -9;
                    }
                }
            }
            else //i.e. auditOnly Mode
            {
                if (credentialValuesFound.Count > 0)
                {
                    return credentialValuesFound.Last().CredentialValueId;
                }
                else
                {
                    return -12;
                }
            }
        }

        internal static void InsertAuditEntriesCredValues(List<IInputData> successList, int actionId, string uRL, int credID, int appID, string credTypeDesc, int uRNID, DateTime dtm)
        {

            string xmlHeader = string.Format("<audit action=\"CredentialValueAction.Created\"><data><credentials><credential id=\"" + credID + "\" applicationid=\"" + appID + "\" description=\"" + credTypeDesc + "\" uniformresourcenameid=\"" + uRNID + "\" /></credentials><credentialvalues>");
            string xmlEnd = "</credentialvalues></data></audit>";

            string xmlValues = "";
            foreach (var item in successList)
            {

                xmlValues = xmlValues + "<credentialvalue id=\"" + item.CredentialValueID + "\" credentialid=\"" + credID + "\" description=\"" + item.Description + "\" value=\"" + item.Value + "\" />";

            }
            string finalXml = xmlHeader + xmlValues + xmlEnd;

            CreateAuditEntry(actionId, uRL, finalXml, dtm);

        }


        internal static void InsertUserGroups(List<LAFUserGroups> userGroupsToLoad,  string appName, string changeDescription, string fileName, ProgressBar progressbar1, CheckBox chkAudit, int appID)
        {
            //First get the CredentialID for the type (as per supplied description)

            string uRL = auditText1 + currentUser + auditText2 + changeDescription;

            progressbar1.Value = 0;
            //int credentialID = .ToInt32(credID.FirstConvert());
            int i = 1;


            DateTime dtm = DateTime.Now;

            progressbar1.Step = 1;
            progressbar1.Maximum = userGroupsToLoad.Count();

            List<IInputData> successList = new List<IInputData>();
            //Debug.WriteLine("log: Entered GetTestDataMethod")    
            foreach (IInputData row in userGroupsToLoad)
            {


                progressbar1.PerformStep();

                var userGroupTasks = Lloyds.LAF.Agent.Restricted.Factory.Resolve<IUserGroupTask>();

                int result = InsertUG( row.UserGroupName, row.Description, fileName, uRL, chkAudit.Checked, i, appName, "", appID, userGroupTasks);

                if (result > 0)
                {
                    if (!chkAudit.Checked)
                    {
                        ExcelMethods.WriteToCell(i, "Status", "User Group successfully  created", "UserGroups", fileName);
                        row.UserGroupID = result;
                        successList.Add(row);
                    }
                    else
                    {
                        ExcelMethods.WriteToCell(i, "Status", "Audit record successfully created", "UserGroups", fileName);
                        row.UserGroupID = result;
                        successList.Add(row);
                    }
                }
                else if (result == -1)
                {
                    ExcelMethods.WriteToCell(i, "Status", "User Group NOT created - group already exists (duplicate)", "UserGroups", fileName);
                }
                else if (result == -9)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Exception - User Group not created", "UserGroups", fileName);
                }
                else if (result == -2)
                {
                    ExcelMethods.WriteToCell(i, "Status", "User Group NOT created - BLANK NAME NOT PERMITTED", "UserGroups", fileName);
                }
                else if (result == -12)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Audit record not created - User Group does not exist", "UserGroups", fileName);
                }
                i++;
            }
            try
            {
                if (successList.Count > 0) { DataProcessingMethods.InsertAuditEntriesUserGroups(successList, uRL, appID, 7000, dtm); }
                if (!chkAudit.Checked) { MessageBox.Show("Upload complete. Number of User Groups (+ audit entries) successfully created = " + successList.Count.ToString() + " out of " + userGroupsToLoad.Count + " attempted. See input excel file for individual upload status.", "Upload Complete"); }
                else { MessageBox.Show("Upload complete. Number of Audit entries successfully created = " + successList.Count.ToString() + " out of " + userGroupsToLoad.Count + " attempted. See input excel file for individual upload status.", "Upload Complete"); }
            }
            catch
            {
                if (!chkAudit.Checked) { MessageBox.Show("Unknown Exception while adding Audit entries. However, the User Group records themselves were added. Number of records credential values created = " + successList.Count.ToString() + " out of " + userGroupsToLoad.Count + " attempted. See input excel file for individual upload status.", "Upload Complete - Error adding Audit Records"); }
                else { MessageBox.Show("Unknown Exception. Audit entries not added.", "Unknown Exception"); }
            }
            progressbar1.Value = 0;
        }

        static int InsertUG(string userGroupToAdd, string userGroupDescriptionToAdd,  string fileName, string uRL, bool auditOnly, int i, string appName, string appuRL, int ApplicationID, IUserGroupTask userGroupTasks)
        {


            Debug.WriteLine("Inside Insert Cred");


            if (userGroupToAdd == "")
            {


                return -2;
            }


            //Now see if USer Group already exists..

            //First make sure that the credential does not exist for that credential type...





            var userGroupsFound = userGroupTasks.SearchByNames(userGroupToAdd);



            if (!auditOnly) // i.e. Normal Mode (add Cred Values)
            {

                if (userGroupsFound.Count > 0)
                {
                    return -1;
                }
                else
                {
                    try
                    {

                        var userGroup = new UserGroup();
                        userGroup.Name = userGroupToAdd;
                        userGroup.Description = userGroupDescriptionToAdd;
                        userGroup.ApplicationId = ApplicationID;
                        userGroupTasks.AddUserGroup(userGroup);
                        return userGroupTasks.SearchByNames(userGroupToAdd)[0].UserGroupId;


                    }
                    catch
                    {
                        return -9;
                    }
                }
            }
            else //i.e. auditOnly Mode
            {
                if (userGroupsFound.Count > 0)
                {
                    return userGroupsFound.Last().UserGroupId;
                }
                else
                {
                    return -12;
                }
            }
        }

        private static void InsertAuditEntriesUserGroups(List<IInputData> successList,string uRL, int appID, int actionID, DateTime dtm)
        {


            string xmlHeader = string.Format("<audit action=\"UserGroupAction.Created\"><data>");
            string xmlEnd = "</data></audit>";

            string xmlValues = "";
            foreach (var item in successList)
            {

                xmlValues = xmlValues + "<usergroup id=\"" + item.UserGroupID + "\" name=\"" + item.UserGroupName + "\" applicationid=\"" + appID + "\" />";

            }
            string finalXml = xmlHeader + xmlValues + xmlEnd;


            CreateAuditEntry(actionID, uRL, finalXml, dtm);



        }




        public static void InsertUserGroupCredentialValues(List<UserGroupCredentialValues> userGroupCredentialValueList,  string credentialDescription, string changeDescription, string fileName, ProgressBar progressBar1, CheckBox chkAudit, string urn, int appId)
        {
            /**
             * 
            NB: The code for inserting cred values to usergroups is slightly different to the insert user groups and insert cred value methods - reason is that it is not possible
             * to insert multiple audit details in the same audit entry when the user group is different.
             * Therefore, here the audit records will be added one by one immediatley after the value has been created.
            
             * **/


            //First get the CredentialID for the type (as per supplied description)

            progressBar1.Value = 0;

            progressBar1.Step = 1;
            progressBar1.Maximum = userGroupCredentialValueList.Count();

            //int credentialID = .ToInt32(credID.FirstConvert());
            int i = 0;
            List<UserGroupCredentialValues> successList = new List<UserGroupCredentialValues>();
            string currentUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            progressBar1.Step = 1;
            progressBar1.Maximum = userGroupCredentialValueList.Count();

            DateTime dtm = DateTime.Now;
            int imported = 0;
            string uRL = auditText1 + currentUser + auditText2 + changeDescription;

            var userGroupTasks = Lloyds.LAF.Agent.Restricted.Factory.Resolve<IUserGroupTask>();
            var credTasks = Lloyds.LAF.Agent.Restricted.Factory.Resolve<ICredentialTasks>();

            foreach (UserGroupCredentialValues row in userGroupCredentialValueList)
            {
                i++;

                progressBar1.PerformStep();

                int result = InsertUGtoCV( row.UserGroupName, row.CredentialValue, fileName, chkAudit.Checked, i, credentialDescription, urn, appId, uRL, dtm, userGroupTasks, credTasks);

                if (result > 0)
                {
                    imported++;
                    if (!chkAudit.Checked) { ExcelMethods.WriteToCell(i, "Status", "Credential Value successfully  added to user group (with audit record)", "UserGroupCredentialValues", fileName); }
                    if (chkAudit.Checked) { ExcelMethods.WriteToCell(i, "Status", "Audit record successfully added", "UserGroupCredentialValues", fileName); }

                }
                else if (result == -1)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Credential Value NOT added to User Group - it was already added", "UserGroupCredentialValues", fileName);
                    continue;
                }
                else if (result == -2)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Credential Value NOT added to User Group - User Group does not exist", "UserGroupCredentialValues", fileName);
                    continue;
                }
                else if (result == -3)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Credential Value NOT added to User Group - Credential Value with that Credential type does not exist", "UserGroupCredentialValues", fileName);
                    continue;
                }
                else if (result == -8)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Exception - Credential Value added to User Group but Audit Record NOT added due to an unknown exception", "UserGroupCredentialValues", fileName);
                    continue;
                }
                else if (result == -9)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Exception - Credential Value not added to User Group.", "UserGroupCredentialValues", fileName);
                    continue;
                }
                else if (result == -12)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Audit Record not added - Credential has not yet been added to User Group", "UserGroupCredentialValues", fileName);
                    continue;
                }
                else if (result == -13)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Audit Record not added - No such User Group exists", "UserGroupCredentialValues", fileName);
                    continue;
                }
                else if (result == -14)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Audit Record not added - No such Credential Value exists", "UserGroupCredentialValues", fileName);
                    continue;
                }
                else if (result == -19)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Exception - error inserting audit record.", "UserGroupCredentialValues", fileName);
                    continue;
                }



            }




            MessageBox.Show("Upload complete. Number of records successfully created = " + imported + " out of " + userGroupCredentialValueList.Count + " attempted. See input excel file for individual upload status.", "Upload Complete");
            //Debug.WriteLine("log: Entered GetTestDataMethod")  
            progressBar1.Value = 0;
        }


        private static int InsertUGtoCV( string userGroupName, string credentialValue,  string fileName, bool auditOnly, int i, string credentialDescription, string uRN, int appId, string uRL, DateTime dtm, IUserGroupTask userGroupTasks, ICredentialTasks credTasks)
        {

            string sql;





            //First make sure that this combination does not already exist...

            //To do that need to get CredentialValueID from value


            //Need to create a credentialValue search to do that

            var credValues = GetCredentialValues(uRN, appId, credentialValue, credTasks);





            if (credValues.Count == 0)
            {
                if (!auditOnly) { return -3; }
                if (auditOnly) { return -14; }
            }//Cred Value does not exist}
            else if (credValues.Count > 1)
            {
                //Don't think this can happen...
                return -9;
            }
            //so must be only 1...
            int credValueID = credValues.First().CredentialValueId;


            //Now  check if user group exists...

            var userGroupsSearch = userGroupTasks.SearchByNames(userGroupName);
            int userGroupId;


            //First check that the ug exists
            if (userGroupsSearch.Count == 0)
            {
                if (!auditOnly) { return -2; }
                if (auditOnly) { return -13; }
            }

            //list.First().

            var userGroups = userGroupTasks.GetUserGroupsAssociatedToCredentialValue(credValueID);

            if (auditOnly)
            {
                if (userGroups.Count == 0)
                {
                    //No such combination exists as the credential Value isn't in any group at all
                    return -12;

                }
                else
                {
                    if (!isUserGroupinList(userGroups, userGroupName))
                    {
                        //N osuch combination exists as the credential Value is not in that group
                        return -12;
                    }
                    else
                    {
                        //is in group so can carry on...
                    }

                }


            }
            else //!auditOnly i.e. Normal Mode
            {
                if (userGroups.Count > 0)
                {
                    if (isUserGroupinList(userGroups, userGroupName))
                    {

                        //is already in the group, so do not add
                        return -1;
                    }

                }

            }



            //So not in list, so can continue and add the value...

            //Already have the CredValueID, just need to fetch the UserGroupID...






            userGroupId = userGroupsSearch.First().UserGroupId;



            //Then check if appId matches - also reject if not...
            if (!(userGroupsSearch.First().ApplicationId == appId))
            {
                if (!auditOnly) { return -2; }
                if (auditOnly) { return -13; }
                //Check if this is a valid error - can I add a Cred value from different app to a UG? //This is not the correct application  - Do Not Add, throw error. What about the Credential ID? This is already verified via the earlier search...
            }



            var UGCV = new UserGroupCredentialValue();
            UGCV.UserGroupId = userGroupId;
            UGCV.CredentialValueId = credValueID;

            if (!auditOnly)
            {

                try
                {
                    userGroupTasks.AddUserGroupCredentialValue(UGCV);


                }
                catch
                {
                    return -9;
                }
            }

            //Finally, lookup in DB to get UGCVID (not possible via API)

            sql = string.Format("Select TOP 1 UserGroupCredentialValueID FROM {0}.LAF.UserGroupCredentialValue Where UserGroupID = '{1}' AND CredentialValueID = '{2}' Order by 1 DESC", database,userGroupId, credValueID);
            var ugcv = ExecuteQuery<UGCVData>(sql);

            int userGroupCredentialValueID = ugcv.First().UserGroupCredentialValueID;

            try
            {
                DataProcessingMethods.InsertAuditEntryUGCV(userGroupId, appId, userGroupCredentialValueID, credValueID, uRL, 7005, dtm, userGroupName);
            }
            catch
            {
                //Problem adding Audit Record

                if (!auditOnly) { return -8; }
                if (auditOnly) { return -19; }
            }



            return userGroupCredentialValueID;
        }

        private static List<CredentialValue> GetCredentialValues(string uRN, int appId, string credentialValue, ICredentialTasks credTasks)
        {
            CredentialValueSearchOptions credSearch = new CredentialValueSearchOptions();

            //Need URN for that
            credSearch.Urn = uRN;
            //And ApplicaitonID:
            credSearch.ApplicationId = appId;
            credSearch.Value = credentialValue;


            return credTasks.SearchForCredentialValues(credSearch);
        }

        internal static void InsertUserGroupMembers(List<UserGroupMembers> userGroupMembersToLoad,  string changeDescription, string fileName, ProgressBar progressBar1, CheckBox chkAudit)
        {
            //First get the CredentialID for the type (as per supplied description)

            DateTime dtm = DateTime.Now;
            string uRL = auditText1 + currentUser + auditText2 + changeDescription;

            progressBar1.Value = 0;
            progressBar1.Step = 1;
            progressBar1.Maximum = userGroupMembersToLoad.Count();
            //int credentialID = .ToInt32(credID.FirstConvert());
            int i = 0;
            int imported = 0;



            foreach (UserGroupMembers row in userGroupMembersToLoad)
            {
                i++;

                progressBar1.PerformStep();

                //Check user exists first:

                int result = InsertUGMember( row.Email, fileName, row.UserGroupName, chkAudit.Checked, uRL, dtm);

                switch (result)
                {
                    case 0:

                        imported++;
                        if (!chkAudit.Checked) { ExcelMethods.WriteToCell(i, "Status", "User added to User Group successfully  created (with audit record)", "UserGroupMembers", fileName); }
                        if (chkAudit.Checked) { ExcelMethods.WriteToCell(i, "Status", "Audit record successfully added", "UserGroupMembers", fileName); }
                        continue;

                    case -1:
                        ExcelMethods.WriteToCell(i, "Status", "Not added - User does not exist", "UserGroupMembers", fileName);

                        continue;

                    case -2:
                        ExcelMethods.WriteToCell(i, "Status", "Not added - User Group does not exist", "UserGroupMembers", fileName);

                        continue;

                    case -3:
                        ExcelMethods.WriteToCell(i, "Status", "User Group NOT  created - BLANK NAME NOT PERMITTED", "UserGroupMembers", fileName);
                        i++;
                        continue;

                    case -4:
                        ExcelMethods.WriteToCell(i, "Status", "User not added to User Group - user is already a member", "UserGroupMembers", fileName);
                        continue;

                    case -9:
                        ExcelMethods.WriteToCell(i, "Status", "Exception - User not added to User Group", "UserGroupMembers", fileName);
                        continue;

                    case -11:
                        ExcelMethods.WriteToCell(i, "Status", "Audit record not created - User is not yet a member of Group", "UserGroupMembers", fileName);
                        continue;

                    case -8:
                        ExcelMethods.WriteToCell(i, "Status", "Exception - User added to User Group but Audit Record NOT added due to an unknown exception", "UserGroupMembers", fileName);
                        continue;

                    case -19:
                        ExcelMethods.WriteToCell(i, "Status", "Exception - error inserting audit record.", "UserGroupMembers", fileName);
                        continue;

                }
            }
            MessageBox.Show("Upload complete. Number of records successfully created = " + imported.ToString() + " out of " + userGroupMembersToLoad.Count + " attempted. See input excel file for individual upload status.", "Upload Complete");
            //Debug.WriteLine("log: Entered GetTestDataMethod")  
            progressBar1.Value = 0;
        }



        //Connects direct to DB 
        private static int InsertUGMember(string emailAddress,  string fileName, string userGroup, bool auditOnly, string uRL, DateTime dtm)
        {

            //First Check if user exists                   
            

            string sql = "Select UserID FROM " + database + ".LAF.[User] WHERE EmailAddress = '" + emailAddress + "'";
            int result = ExecuteQuery(sql).Count();

            if (result == 0)
            {
                ;
                return -1;
            }
            //Then check User Group exists:

            sql = "Select UserGroupID FROM " + database + ".LAF.[UserGroup] WHERE Name = '" + userGroup + "'";
            result = ExecuteQuery(sql).Count();
            if (result == 0)
            {
                ;
                return -2;
            }

            //Then make sure that supplied values are not null:
            if (userGroup == "")
            {
                ;
                return -3;
            }



            //Then get count of how many of this combination exist:
            sql = "Select UserGroupMembersID FROM " + database + ".LAF.UserGroupMembers WHERE UserGroupID = (Select UserGroupID FROM " + database + ".LAF.UserGroup Where NAme = '" + userGroup + "') AND UserID = (Select UserID FROM " + database + ".LAF.[User] WHERE EmailAddress = '" + emailAddress + "')";
            result = ExecuteQuery(sql).Count();

            if (!auditOnly && result > 0)
            {

                ;
                return -4;
            }
            if (auditOnly && result == 0)
            {
                ;
                return -11;
            }


            string sqlDeclarations = String.Format("DECLARE @userGroupID int = (Select UserGroupID FROM {0}.LAF.UserGroup WHERE name = '{1}') DECLARE @userID int = (Select UserID FROM {0}.LAF.[User] WHERE EmailAddress = '{2}')",database,userGroup,emailAddress);
            string sqlInsert = String.Format(" EXEC {0}.{1} @UserId,@UserGroupId",database, ConfigurationManager.AppSettings["sp_InsertUserGroupMember"]);

            string sqlCombined;


            if (!auditOnly)
            {

                try
                {

                    sqlCombined = "Begin Transaction " + sqlDeclarations + sqlInsert + " Commit Transaction";
                    ExecuteQuery(sqlCombined);
                    
                }
                catch
                {
                    ;
                    return -9;
                }

            }

            try
            {
                InsertAuditEntryUGMember(uRL, 7003, dtm, userGroup, emailAddress);
                ;
                return 0;
            }
            catch
            {
                if (!auditOnly)
                {
                    ;
                    return -8;
                }
                else
                {
                    ;
                    return -19; }
            }
            

        }


        private static void InsertAuditEntryUGMember(string uRL, int actionId, DateTime dtm, string userGroupName, string emailAddress)
        {
            var userGroupTasks = Lloyds.LAF.Agent.Restricted.Factory.Resolve<IUserGroupTask>();
            var userGroup = userGroupTasks.SearchByNames(userGroupName).First();

            var userData = GetUserData( emailAddress).First();




            string xmlHeader = string.Format("<audit action=\"UserGroupAction.UserAdded\"><data>");

            string xmlBody = string.Format("<usergroup id=\"{0}\" name=\"{1}\" applicationid=\"{2}\" /><users><user id=\"{3}\" uid=\"{4}\" isactive =\"{5}\" isdisabled=\"{6}\" failedpasswordattemptcount =\"{7}\" />", userGroup.UserGroupId, userGroupName, userGroup.ApplicationId, userData.userID, userData.Uid, userData.isActive, userData.isDisabled, userData.FailedPasswordAttemptCount);
            string xmlEnd = "</users></data></audit>";

            string finalXml = xmlHeader + xmlBody + xmlEnd;

            CreateAuditEntry(actionId, uRL, finalXml, dtm);

        }

        public static int GetCountSQLQuery( string select, string table, string where)
        {
            string sql = "Select " + select + " FROM " + table + " WHERE " + where;
            int count = ExecuteQuery(sql).Count();
            return count;
        }


        internal static void InsertUsers(List<Users> usersToLoad,  string changeDescription, string fileName, frmCreateUsers form)
        {
            //First get the CredentialID for the type (as per supplied description)

            form.progressBar1.Value = 0;

            int i = 1;
            int imported = 0;

            string uRL = auditText1 + currentUser + auditText2 + changeDescription;



            //Debug.WriteLine("log: Entered GetTestDataMethod")    
            foreach (Users row in usersToLoad)
            {
                form.progressBar1.Step = 1;
                form.progressBar1.Maximum = usersToLoad.Count();

                form.progressBar1.PerformStep();

                string sql = "Select UserID FROM " + database + ".LAF.[User] WHERE EmailAddress = '" + row.Email + "'";
                int result = ExecuteQuery(sql).Count();
                if (result > 0)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Not created - Email address already exists", "Users", fileName);
                    i++;
                    continue;
                }






                //First make sure that user group member combination does not exist...

                if (row.Email == "" || row.Email == null)
                {
                    ExcelMethods.WriteToCell(i, "Status", "User NOT created - BLANK EMAIL ADDRESS PROVIDED", "Users", fileName);
                    i++;
                    continue;
                }



                try
                {


        string sqlInsert = string.Format("EXEC {0}.{11} '{1}','{2}','{3}','{4}',{5},{6},'{7}','{8}',{9},'{10}'", database, row.Email, row.Title, row.FirstName, row.LastName, 12, 1, "CompName", "TestTitle", 233, 1234, ConfigurationManager.AppSettings["sp_InsertUser"]);
                    string sqlAudit = String.Format("EXEC {0}.LAF.{1} '" + row.Email + "', '" + uRL + "'",database, ConfigurationManager.AppSettings["sp_Audit_InsertUser"]);
                    string sqlCombined = "Begin Transaction " + sqlInsert + sqlAudit + " Commit Transaction";
                    ExecuteQuery(sqlCombined);
                    ExcelMethods.WriteToCell(i, "Status", "User created successfully (with audit record)", "Users", fileName);
                    imported++;
                }
                catch (Exception ex)
                {

                    ExcelMethods.WriteToCell(i, "Status", "Exception - User NOT created", "Users", fileName);

                }

                i++;
            }

            //"Invalid object name 'LAF_Audit_Stable.Audit.Entry'."







            MessageBox.Show("Upload complete. Number of users successfully created = " + imported.ToString() + " out of " + usersToLoad.Count + " attempted. See input excel file for individual upload status.", "Upload Complete");

            form.progressBar1.Value = 0;
        }

        public static IEnumerable<LAFCredentialData> GetLAFCredentials(string appName)
        {
            
            string sql;
            if (appName == null) { sql = "Select * from " + database + ".LAF.Credential"; }
            else { sql = "Select * from " + database + ".LAF.Credential where ApplicationId = (Select ApplicationID From " + database + ".LAF.Application Where Name = '" + appName + "')"; };

            var appdata = ExecuteQuery<LAFCredentialData>(sql);
            ;
            return appdata;
        }






        public static void DeleteCredentialValues(List<CredentialValues> credentialList,  string credentialTypeDescription, string changeDescription, string fileName, ProgressBar progressBar1, CheckBox chkAudit, string appName)
        {
            //First get the CredentialID for the type (as per supplied description)

            progressBar1.Value = 0;
            //int credentialID = .ToInt32(credID.FirstConvert());

            string uRL = auditText1 + currentUser + auditText2 + changeDescription;

            DateTime dtm = DateTime.Now;
            List<int> credValuesDeleted = new List<int>();

            int i = 1;
            
            foreach (CredentialValues row in credentialList)
            {
                progressBar1.Step = 1;
                progressBar1.Maximum = credentialList.Count();
                progressBar1.PerformStep();

                int result = DeleteCred( row.Value, row.Description, fileName, uRL, i, credentialTypeDescription, appName);
                if (result > 0)
                {
                    credValuesDeleted.Add(result);
                    ExcelMethods.WriteToCell(i, "Status", "Credential Value successfully deleted", "CredentialValues", fileName);
                }
                else if (result == -2)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Credential Value NOT deleted - BLANK VALUE NOT PERMITTED", "CredentialValues", fileName);
                }
                else if (result == -1)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Credential Value NOT deleted - value does not exist", "CredentialValues", fileName);
                }
                else if (result == -3)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Credential Value NOT deleted - it is present in 1 or more group(s) - delete from group(s) first", "CredentialValues", fileName);
                }



                i++;


            }

            try
            {
                if (credValuesDeleted.Count > 0) { DataProcessingMethods.InsertAuditEntriesDeleteCredValues(credValuesDeleted, uRL, 9001, dtm); }
                MessageBox.Show("Upload complete. Number of CredValues (+ audit entries) successfully deleted = " + credValuesDeleted.Count.ToString() + " out of " + credentialList.Count + " attempted. See input excel file for individual upload status.", "Upload Complete");

            }
            catch
            {
                MessageBox.Show("Unknown Exception while adding Audit entries. However, the Credential Value records themselves were deleted. Number of deletions made = " + credValuesDeleted.Count.ToString() + " out of " + credentialList.Count + " attempted. See input excel file for individual upload status.", "Upload Complete - Error adding Audit Records");
            }


            
            //Debug.WriteLine("log: Entered GetTestDataMethod")    
            progressBar1.Value = 0;
        }

        private static void InsertAuditEntriesDeleteCredValues(List<int> credValuesDeleted, string uRL, int actionId, DateTime dtm)
        {



            string xmlHeader = string.Format("<audit action=\"CredentialValueAction.Deleted\"><data><credentialvalues>");
            string xmlEnd = "</credentialvalues></data></audit>";

            string xmlValues = "";
            foreach (var item in credValuesDeleted)
            {

                xmlValues = xmlValues + "<credentialvalue id=\""+item.ToString() +"\"/>";

            }
            string finalXml = xmlHeader + xmlValues + xmlEnd;

            CreateAuditEntry(9001, uRL, finalXml, dtm);
        }

        static int DeleteCred( string credentialValueToAdd, string credentialDescriptionToAdd,  string fileName, string uRL, int i, string credentialTypeDescription, string appName)
        {


            Debug.WriteLine("Inside Delete Cred");

            

            if (credentialValueToAdd == "")
            {


                return -2;
                ;
            }

            //First make sure that the credential DOES exist for that credential type...
            string sql = "Select CredentialValueID FROM " + database + ".LAF.CredentialValue WHERE Value = '" + credentialValueToAdd + "' AND CredentialID IN (Select CredentialID FROM " + database + ".LAF.Credential WHERE description = '" + credentialTypeDescription + "')";
            //MAp the above result to credentialValueLoadData (just to get CredentialValueID)

            var credValues = ExecuteQuery<CredentialValues>(sql).ToList();

            //int result = ExecuteQuery(sql).Count();


            if (!(credValues.Count == 1))
            {
                //Does not exist (or more than 1) so can't delete - there should never be more than one though..
                ;
                return -1;
            }

            int credValueID = credValues.First().CredentialValueID;

            //Then make sure that the Credential is not in any groups

            //Needs updating:
            sql = "Select CredentialValueID FROM " + database + ".LAF.UserGroupCredentialValue WHERE CredentialValueID IN (Select CredentialValueID FROM " + database + ".LAF.CredentialValue WHERE Value = '" + credentialValueToAdd + "' AND CredentialID IN (Select CredentialID FROM " + database + ".LAF.Credential WHERE description = '" + credentialTypeDescription + "'))";
            int result = ExecuteQuery(sql).Count();


            if (result > 0)
            {
                //Can't delete - the credential value is being used in one or more groups - need to delete from the groups first
                ;
                return 3;
            }

            

            string sqlDeclarations;


            sqlDeclarations = "DECLARE @credValueID varchar(12) = (Select CredentialValueID FROM " + database + ".LAF.CredentialValue WHERE Value = '" + credentialValueToAdd + "')";

            Debug.WriteLine("In insert Cred, SQL Declarationa = " + sqlDeclarations);
            
        string sqlDelete = String.Format(" EXEC {0}.{1} @credValueID",database, ConfigurationManager.AppSettings["sp_DeleteCredentialValue"]);


            
            string sqlCombined;




            try
            {


                sqlCombined = "Begin Transaction " + sqlDeclarations + sqlDelete + " Commit Transaction";

                Debug.WriteLine("about to execute Insert Cred with SQL comb = " + sqlCombined);
                ExecuteQuery(sqlCombined);
                ;
                return credValueID;



            }
            catch
            {

                //Unknown exception has occurred
                ;
                return -9;

            }
            

        }



        internal static void DeleteUserGroupCredentialValues(List<UserGroupCredentialValues> userGroupCredsToLoad,  string changeDescription, string fileName, ProgressBar progressbar1, CheckBox chkAudit, string credentialTypeDescription)
        {
            //First get the CredentialID for the type (as per supplied description)

            string uRL = auditText1 + currentUser + auditText2 + changeDescription;

            DateTime dtm = DateTime.Now;

            progressbar1.Value = 0;
            //int credentialID = .ToInt32(credID.FirstConvert());
            int i = 1;
            int imported = 0;





            //Debug.WriteLine("log: Entered GetTestDataMethod")    
            foreach (UserGroupCredentialValues row in userGroupCredsToLoad)
            {

                progressbar1.Step = 1;
                progressbar1.Maximum = userGroupCredsToLoad.Count();
                progressbar1.PerformStep();

                int result = DeleteSpecificUGCV( row.CredentialValue, row.UserGroupName, fileName, uRL, credentialTypeDescription,dtm);
                Debug.WriteLine("UGCVResult = " + result);

                if (result == 0)
                {
                    imported++;
                    ExcelMethods.WriteToCell(i, "Status", "Credential Value successfully deleted from User Group (and audit record added)", "UserGroupCredentialValues", fileName);
                }
                else if (result == 1)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Credential Value NOT removed from group - No such Group/Credential Value combination exists - Check spelling", "UserGroupCredentialValues", fileName);
                }

                else if (result == 2)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Exception - Credential Value AND / OR User Group supplied in blank", "UserGroupCredentialValues", fileName);
                }
                else if (result == 9)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Exception - Credential Value not deleted from User Group", "UserGroupCredentialValues", fileName);
                }




                i++;
            }
            MessageBox.Show("Process complete. Number of records successfully deleted = " + imported.ToString() + " out of " + userGroupCredsToLoad.Count + " attempted. See input excel file for individual upload status.", "Upload Complete");
            //Debug.WriteLine("log: Entered GetTestDataMethod")
            progressbar1.Value = 0;
        }

        private static int DeleteSpecificUGCV(string credentialValueToDelete, string userGroupToDelete,  string fileName, string uRL, string credentialTypeDescription, DateTime dtm)
        {


            Debug.WriteLine("Inside Delete Cred");



            if (credentialValueToDelete == "" || userGroupToDelete == "")
            {

                //Can't if either are blank
                return 2;
            }

            string sql = "Select * FROM " + database + ".LAF.UserGroupCredentialValue ugcv JOIN " + database + ".LAF.UserGroup ug ON ug.usergroupId = ugcv.usergroupid JOIN " + database + ".LAF.CredentialValue cv ON ugcv.credentialvalueId = cv.credentialvalueId WHERE cv.Value = '" + credentialValueToDelete + "' AND ug.name = '" + userGroupToDelete + "' AND cv.CredentialID IN (Select CredentialID FROM " + database + ".LAF.Credential WHERE description = '" + credentialTypeDescription + "')";

            var UserGroupCredValues = ExecuteQuery<LAFUserGroupCredentialValue>(sql);




            if (UserGroupCredValues.Count() < 1)
            {
                //Does not existso can't delete..
                return 1;
            }

            //So there must be something to delete


            var UserGroupCredValuesDeleted = new List<LAFUserGroupCredentialValue>();

            foreach (var UserGroupCredValue in UserGroupCredValues)

            {



                //First make sure that the credential DOES exist for that credential type...


                //Then make sure that the Credential is not in any groups


                //Ready to go..



                try
                {

                    string sqlDeclarations = String.Format("DECLARE @UGCVsTable AS LAF.IntegerListTableType Insert INTO  @UGCVsTable values({0})", UserGroupCredValue.UserGroupCredentialValueID); ;

                    string sqlDelete = String.Format(" EXEC {0}.{1} {2},@UGCVsTable", database, ConfigurationManager.AppSettings["sp_DeleteUserGroupCredentialValues"], UserGroupCredValue.UserGroupID);

                    string sqlCombined = "Begin Transaction " + sqlDeclarations + sqlDelete + " Commit Transaction"; ;



                    Debug.WriteLine("about to execute Delete UGCV Cred with SQL comb = " + sqlCombined);
                    ExecuteQuery(sqlCombined);
                    UserGroupCredValuesDeleted.Add(UserGroupCredValue);




                }
                catch
                {

                    //Unknown exception has occurred
                    return 9;

                }


            }

            
            

            InsertAuditEntriesDeleteUserGroupCredentialValues(UserGroupCredValuesDeleted, uRL, 7006, dtm);
            //If reached here, there must have been successful deletions
            return 0;


        }

        private static void InsertAuditEntriesDeleteUserGroupCredentialValues(List<LAFUserGroupCredentialValue> userGroupCredValuesDeleted, string uRL, int actionId, DateTime dtm)
        {

            //DECLARE	@detail	xml  = '<audit action="UserGroupAction.CredentialRemoved"><data><usergroup id="'+CAST(@UserGroupID as VARCHAR(50))+'" /><usergroupcredentialvalues><usergroupcredentialvalue id="'+CAST(@UserGroupCredentialValueID as VARCHAR(50))+'" /></usergroupcredentialvalues>     </data></audit>' 


            string xmlHeader = string.Format("<audit action=\"UserGroupAction.CredentialRemoved \"><data><usergroup id=\"{0}\" /><userGroupCredentialValues>", userGroupCredValuesDeleted.First().UserGroupID);
            string xmlEnd = "</userGroupCredentialValues></data></audit>";

            string xmlValues = "";
            foreach (var userGCVData in userGroupCredValuesDeleted)
            {

                xmlValues = xmlValues + "<userGroupCredentialValue id = \"" + userGCVData.UserGroupCredentialValueID + " \"/>";

            }
            string finalXml = xmlHeader + xmlValues + xmlEnd;


            CreateAuditEntry(actionId, uRL, finalXml, dtm);

        }
        private static void InsertAuditEntriesDeleteUserGroupCredentialValues(LAFUserGroupCredentialValue userGroupCredValuesDeleted, string uRL, int v, DateTime dtm)
        {
            throw new NotImplementedException();
        }

        //If reached here, there must have been successful deletions



        internal static void DeleteUserGroupMembers(List<UserGroupMembers> usersToLoad,  string changeDescription, string fileName, ProgressBar progressbar1, CheckBox chkAudit)
        {
            //First get the CredentialID for the type (as per supplied description)

            string uRL = auditText1 + currentUser + auditText2 + changeDescription;

            DateTime dtm = DateTime.Now;
            progressbar1.Value = 0;
            //int credentialID = .ToInt32(credID.FirstConvert());
            int i = 1;
            int imported = 0;





            //Debug.WriteLine("log: Entered GetTestDataMethod")    
            foreach (UserGroupMembers row in usersToLoad)
            {

                progressbar1.Step = 1;
                progressbar1.Maximum = usersToLoad.Count();
                progressbar1.PerformStep();

                int result = DeleteSpecificUGMember( row.Email, row.UserGroupName, fileName, uRL,dtm);
                Debug.WriteLine("UGCVResult = " + result);

                if (result == 0)
                {
                    imported++;
                    ExcelMethods.WriteToCell(i, "Status", "User successfully removed from User Group (and audit record added)", "UserGroupMembers", fileName);
                }
                else if (result == -1)
                {
                    ExcelMethods.WriteToCell(i, "Status", "User NOT removed from group - No such User/Group Value combination exists - already removed or no such user/group", "UserGroupMembers", fileName);
                }

                else if (result == -2)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Exception - User AND / OR User Group supplied is blank", "UserGroupMembers", fileName);
                }
                else if (result == -9)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Exception - User not removed from User Group", "UserGroupMembers", fileName);
                }




                i++;
            }
            MessageBox.Show("Process complete. Number of users successfully removed from user groups= " + imported.ToString() + " out of " + usersToLoad.Count + " attempted. See input excel file for individual upload status.", "Upload Complete");
            //Debug.WriteLine("log: Entered GetTestDataMethod")
            progressbar1.Value = 0;
        }


        static int DeleteAllUGMembers( string userGroupToDelete, string fileName, string uRL, string appName, DateTime dtm)
        {


            Debug.WriteLine("Inside Delete Cred");



            if (userGroupToDelete == "")
            {

                //Can't if blank
                return -2;
            }

            string sql = "Select * FROM " + database + ".LAF.UserGroupMembers ugm JOIN " + database + ".LAF.UserGroup ug ON ugm.userGroupID = ug.UserGroupID JOIN " + database + ".LAF.[User] u ON ugm.UserId = u.UserId WHERE ug.name = '" + userGroupToDelete + "'";
            //int result = ExecuteQuery(CountChecksql).Count();


            var UserGroupMembersToDelete = ExecuteQuery<LAFUserGroupMembers>(sql);

            

            if (UserGroupMembersToDelete.Count() < 1)
            {
                //Does not existso can't delete..
                return -1;
            }

            //So there must be something to delete


            // string sql = "Select * FROM " + database + ".LAF.UserGroupMembers ugm JOIN " + database + ".LAF.UserGroup ug ON ugm.userGroupID = ug.UserGroupID JOIN " + database + ".LAF.[User] u ON ugm.UserId = u.UserId WHERE ug.name = '" + userGroupToDelete + "' AND  u.EmailAddress = '" + UserToDelete + "'";

            //var UserGroupMembersData = ExecuteQuery<LAFUserGroupMembers>(sql);


            var UserGroupMembersDeleted = new List<LAFUserGroupMembers>();

            foreach(var ugm in UserGroupMembersToDelete)
            {



                //First make sure that the credential DOES exist for that credential type...


                //Then make sure that the Credential is not in any groups


                //Ready to go..



                try
                {

                    string sqlDeclarations = "DECLARE @UserGroupID int = (Select UserGroupID FROM " + database + ".LAF.UserGroup WHERE Name = '" + userGroupToDelete + "') DECLARE @UserGroupMembersID int = (Select TOP 1 UserGroupMembersID FROM " + database + ".LAF.UserGroupMembers ugm JOIN " + database + ".LAF.UserGroup ug ON ug.usergroupId = ugm.usergroupid WHERE ug.name = '" + userGroupToDelete + "' ) DECLARE @UserID int = (Select UserID FROM " + database + ".LAF.UserGroupMembers WHERE UserGroupMembersID = @UserGroupMembersID)"
                    + "DECLARE @UGMembersTable AS LAF.IntegerListTableType Insert INTO @UGMembersTable values(@UserGroupMembersID)";
                    string sqlDelete = String.Format(" EXEC {0}.{1} @UserGroupID,@UGMembersTable",database,ConfigurationManager.AppSettings["sp_DeleteUserGroupMemberFromUserGroup"]);
                    string sqlCombined = "Begin Transaction " + sqlDeclarations + sqlDelete + " Commit Transaction"; 




                    Debug.WriteLine("about to execute Delete UGM with SQL comb = " + sqlCombined);
                    ExecuteQuery(sqlCombined);

                    UserGroupMembersDeleted.Add(ugm);


                }
                catch
                {

                    //Unknown exception has occurred
                    return -9;

                }



                
            }
            //If reached here, there must have been successful deletions
            InsertAuditEntriesDeleteUserGroupMembers(UserGroupMembersDeleted, uRL, 7004, dtm);
            return 0;
        }


        static int DeleteSpecificUGMember( string UserToDelete, string userGroupToDelete,  string fileName, string uRL,DateTime dtm)
        {


            Debug.WriteLine("Inside Delete Cred");





            if (UserToDelete == "" || userGroupToDelete == "")
            {

                //Can't if either are blank
                return 2;
            }

            string sql = "Select * FROM " + database + ".LAF.UserGroupMembers ugm JOIN " + database + ".LAF.UserGroup ug ON ugm.userGroupID = ug.UserGroupID JOIN " + database + ".LAF.[User] u ON ugm.UserId = u.UserId WHERE ug.name = '" + userGroupToDelete + "' AND  u.EmailAddress = '" + UserToDelete + "'";



            var UserGroupMembersData = ExecuteQuery<LAFUserGroupMembers>(sql);

            

            if (UserGroupMembersData.Count() < 1)
            {
                //Does not existso can't delete..
                return 1;
            }

            //So there must be something to delete

            var UserGroupMembersDeleted = new List<LAFUserGroupMembers>();









            //while (UserGroupMembersData.Count() > 0)
            foreach (LAFUserGroupMembers LAFUserGroupMember in UserGroupMembersData)
            {



                //First make sure that the credential DOES exist for that credential type...


                //Then make sure that the Credential is not in any groups


                //Ready to go..



                try
                {
                    string sqlDeclarations2 = "DECLARE @UserGroupID int = (Select UserGroupID FROM " + database + ".LAF.UserGroup WHERE Name = '" + userGroupToDelete + "') DECLARE @UserGroupMembersID int = (Select TOP 1 UserGroupMembersID FROM " + database + ".LAF.UserGroupMembers ugm JOIN " + database + ".LAF.UserGroup ug ON ug.usergroupId = ugm.usergroupid JOIN " + database + ".LAF.[User] u ON u.userid = ugm.userid WHERE u.EmailAddress = '" + UserToDelete + "') "
                        + "DECLARE @UserID int = (Select UserID FROM " + database + ".LAF.[User] WHERE EmailAddress = '" + UserToDelete + "') "
                    + "DECLARE @UGMembersTable AS LAF.IntegerListTableType Insert INTO @UGMembersTable values(@UserGroupMembersID)";

                    string sqlDeclarations = String.Format("DECLARE @UGMembersTable AS LAF.IntegerListTableType Insert INTO @UGMembersTable values({0})", LAFUserGroupMember.UserGroupMembersID);
                    string sqlDelete = String.Format(" EXEC {0}.{1} {2},@UGMembersTable",database, ConfigurationManager.AppSettings["sp_DeleteUserGroupMemberFromUserGroup"], LAFUserGroupMember.UserGroupID);
                    string sqlCombined = "Begin Transaction " + sqlDeclarations + sqlDelete + " Commit Transaction";

                    ExecuteQuery(sqlCombined);

                    UserGroupMembersDeleted.Add(LAFUserGroupMember);




                }
                catch (Exception ex)
                {

                    //Unknown exception has occurred
                    return 9;

                }
                //UserGroupMembersData = ExecuteQuery<LAFUserGroupMembers>(sql);

            }

            InsertAuditEntriesDeleteUserGroupMembers(UserGroupMembersDeleted,uRL,7004,dtm);
            //If reached here, there must have been successful deletions
            return 0;
        }

        
        private static void InsertAuditEntriesDeleteUserGroupMembers(List<LAFUserGroupMembers> userGroupMembersDeleted, string uRL, int actionId, DateTime dtm)
        {

            //DECLARE	@detail	xml  = '<audit action="UserGroupAction.UserRemoved"><data><usergroup id="'+CAST(@UserGroupID as VARCHAR(50))+'" /><usergroupmembers><user id="'+CAST(@UserID as VARCHAR(50))+'" /></usergroupmembers>     </data></audit>' 

           

            
            

            

            

            string xmlHeader = String.Format("<audit action=\"UserGroupAction.UserRemoved \"><data><usergroup id=\"{0}\" /><usergroupmembers>", userGroupMembersDeleted.First().UserGroupID) ;
            string xmlEnd = "</usergroupmembers></data></audit>";

            string xmlValues = "";
            foreach (var userMemberData in userGroupMembersDeleted)
            {

                //xmlValues = xmlValues + "<usergroup id=\"" + userMemberData.UserGroupID + "\" /><userGroupMembers><user id = \""+ userMemberData.UserID+ "\"/></usergroupmembers>";
                xmlValues = xmlValues + string.Format("<user id =\"{0}\" />",userMemberData.UserID);
            }
            string finalXml = xmlHeader + xmlValues + xmlEnd;


            CreateAuditEntry(actionId, uRL, finalXml, dtm);

        }

        static int DeleteAllUGCV( string userGroupToDelete, string fileName, string uRL, string appName,DateTime dtm)
        {


            Debug.WriteLine("Inside Delete Cred");



            if (userGroupToDelete == "")
            {

                //Can't if either are blank
                return 2;
            }

            string CountChecksql = "Select * FROM " + database + ".LAF.UserGroupCredentialValue ugcv JOIN " + database + ".LAF.UserGroup ug ON ug.usergroupId = ugcv.usergroupid JOIN " + database + ".LAF.CredentialValue cv ON ugcv.credentialvalueId = cv.credentialvalueId WHERE ug.name = '" + userGroupToDelete + "'";

            var userGroupCredentialValues = ExecuteQuery<LAFUserGroupCredentialValue>(CountChecksql);
            
            

            if (userGroupCredentialValues.Count() < 1)
            {
                //Does not existso can't delete..
                return 1;
            }

            //So there must be something to delete

            var ugcvsDeleted = new List<LAFUserGroupCredentialValue>();

           foreach (var ugcvToDelete in userGroupCredentialValues)
            {



                //First make sure that the credential DOES exist for that credential type...


                //Then make sure that the Credential is not in any groups


                //Ready to go..



                try
                {

                    string sqlDeclarations = "DECLARE @UserGroupID int = (Select UserGroupID FROM " + database + ".LAF.UserGroup WHERE Name = '" + userGroupToDelete + "') DECLARE @UserGroupCredentialValueID int = (Select TOP 1 UserGroupCredentialValueID FROM " + database + ".LAF.UserGroupCredentialValue ugcv JOIN " + database + ".LAF.UserGroup ug ON ug.usergroupId = ugcv.usergroupid JOIN " + database + ".LAF.CredentialValue cv ON ugcv.credentialvalueId = cv.credentialvalueId WHERE ug.name = '" + userGroupToDelete + "' )"
                    + "DECLARE @UGCVsTable AS LAF.IntegerListTableType Insert INTO  @UGCVsTable values(@UserGroupCredentialValueID)"; 
                    string sqlDelete = String.Format(" EXEC {0}.{1} @UserGroupID,@UGCVsTable", database, ConfigurationManager.AppSettings["sp_DeleteUserGroupCredentialValues"]);
                    string sqlCombined = "Begin Transaction " + sqlDeclarations + sqlDelete + " Commit Transaction"; ;




                    Debug.WriteLine("about to execute Delete UGCV Cred with SQL comb = " + sqlCombined);
                    ExecuteQuery(sqlCombined);

                    ugcvsDeleted.Add(ugcvToDelete);


                }
                catch(Exception ex)
                {

                    //Unknown exception has occurred
                    return 9;

                }

                
            }

            InsertAuditEntriesDeleteUserGroupCredentialValues(ugcvsDeleted, uRL, 7006, dtm);

            //If reached here, there must have been successful deletions
            return 0;
        }

        internal static void DeleteUserGroups(List<LAFUserGroups> userGroupsToLoad, string appName, string changeDescription, string fileName, ProgressBar progressbar1, CheckBox chkAudit)
        {
            //First get the CredentialID for the type (as per supplied description)

            string uRL = auditText1 + currentUser + auditText2 + changeDescription;

            DateTime dtm = DateTime.Now;
            progressbar1.Value = 0;
            //int credentialID = .ToInt32(credID.FirstConvert());
            int i = 1;
            
            List<int> userGroupsDeleted = new List<int>();

            var userGroupTasks = Lloyds.LAF.Agent.Restricted.Factory.Resolve<IUserGroupTask>();
           


            var auditServiceClient1 = new ServiceReferences.AuditService.AuditServiceClient();


            

            //Debug.WriteLine("log: Entered GetTestDataMethod")    
            foreach (LAFUserGroups row in userGroupsToLoad)
            {
                
                progressbar1.Step = 1;
                progressbar1.Maximum = userGroupsToLoad.Count();
                progressbar1.PerformStep();

                //First check that the UserGroup / App ID match is OK - User Group name is actually unique, but this acts as an extra check..
                string CountChecksql = "Select UserGroupID FROM " + database + ".LAF.UserGroup ug WHERE ug.name = '" + row.UserGroupName + "' AND ug.ApplicationID = (Select ApplicationID FROM " + database + ".LAF.Application WHERE Name = '" + appName + "')";

                int result = ExecuteQuery(CountChecksql).Count();
                             
                
                    
                    

                if (result < 1)
                {
                    //Does not existso can't delete..
                    ExcelMethods.WriteToCell(i, "Status", "User Group NOT deleted - No User Group with this name belongs to this Application", "UserGroups", fileName);
                    
                    continue;
                }



                int UGCVResult = DeleteAllUGCV( row.UserGroupName, fileName, uRL, appName,dtm);
                Debug.WriteLine("UGCVResult = " + UGCVResult);
                if (UGCVResult == -9)
                {
                    ExcelMethods.WriteToCell(i, "Status", "User Group NOT deleted - Exception while tying to remove credentials from group", "UserGroups", fileName);
                    
                    continue;
                }

                //If not 9, then must be 0 (successfully removed creds) or 1 (no creds to remove) - so proceed to removing users


                int UGCMResult = DeleteAllUGMembers( row.UserGroupName, fileName, uRL, appName,dtm);
                Debug.WriteLine("UGCMResult = " + UGCVResult);
                if (UGCMResult == -9)
                {
                    ExcelMethods.WriteToCell(i, "Status", "User Group NOT deleted - Exception while tying to remove users from group", "UserGroups", fileName);
                    
                    continue;
                }

                //If not 9, then must be 0 (successfully removed creds) or 1 (no creds to remove) - so proceed to removing users




                result = DeleteUserGroup( row.UserGroupName, row.Description, fileName, uRL, i, appName);
                if (result > 0)
                {
                    userGroupsDeleted.Add(result);
                    
                    ExcelMethods.WriteToCell(i, "Status", "User Group (and any child credential values/ user membership(s)) successfully  deleted (and audit record(s) added)", "UserGroups", fileName);
                }
                else if (result == -1)
                {
                    ExcelMethods.WriteToCell(i, "Status", "User Group NOT deleted - group does not exist (for that application)", "UserGroups", fileName);
                }
                else if (result == -9)
                {
                    ExcelMethods.WriteToCell(i, "Status", "Exception - User Group not deleted", "UserGroups", fileName);
                }




                i++;
            }
            try
            {
                if (userGroupsDeleted.Count > 0) { DataProcessingMethods.InsertAuditEntriesDeleteUserGroups(userGroupsDeleted, uRL, 7002, dtm); }
                MessageBox.Show("Upload complete. Number of User Groups (+ audit entries) successfully created = " + userGroupsDeleted.Count.ToString() + " out of " + userGroupsToLoad.Count + " attempted. See input excel file for individual upload status.", "Upload Complete"); 
                
            }
            catch
            {
               MessageBox.Show("Unknown Exception while adding Audit entries. However, the User Group records themselves were deleted. Number of deletions made = " + userGroupsDeleted.Count.ToString() + " out of " + userGroupsToLoad.Count + " attempted. See input excel file for individual upload status.", "Upload Complete - Error adding Audit Records");
            }
            progressbar1.Value = 0;
        }

        private static IEnumerable<dynamic> ExecuteQuery(string countChecksql)
        {
            conn.Open();
            var queryResult = conn.Query(countChecksql);
            conn.Close();
            return queryResult;

        }

        private static IEnumerable<T> ExecuteQuery<T>(string countChecksql)
        {
            conn.Open();
            var queryResult = conn.Query<T>(countChecksql);
            conn.Close();
            return queryResult;
            
        }

        private static void InsertAuditEntriesDeleteUserGroups(List<int> userGroupsDeleted, string uRL, int actionId, DateTime dtm)
        {
            string xmlHeader = string.Format("<audit action=\"UserGroupAction.Deleted\"><data>");
            string xmlEnd = "</data></audit>";

            string xmlValues = "";
            foreach (var userGroupID in userGroupsDeleted)
            {

                xmlValues = xmlValues + "<usergroup id=\"" + userGroupID + "\" />";

            }
            string finalXml = xmlHeader + xmlValues + xmlEnd;


            CreateAuditEntry(actionId, uRL, finalXml, dtm);

                    }



        static int DeleteUserGroup( string userGroupToDelete, string credentialDescriptionToAdd, string fileName, string uRL, int i, string appName)
        {



           

        Debug.WriteLine("Inside Delete Cred");


            if (userGroupToDelete == "")
            {


                return 2;
            }

            //First make sure that the usergroup DOES exist


            int UserGroupID = FindUserGroupByName(userGroupToDelete);
             
            if (UserGroupID < 0)  { return 1; } // THis means there is none returned

            
            

            Debug.WriteLine("In insert Cred, SQL Declarationa = ");

            string sqlDelete = String.Format(" EXEC {0}.{1} {2}", database, ConfigurationManager.AppSettings["sp_DeleteUserGroup"],UserGroupID);
            
            string sqlCombined;




            try
            {


                sqlCombined = "Begin Transaction " + sqlDelete + " Commit Transaction";

                Debug.WriteLine("about to execute Insert Cred with SQL comb = " + sqlCombined);
                
                ExecuteQuery(sqlCombined);

                return UserGroupID;

            }
            catch (Exception ex)
            {

                //Unknown exception has occurred
                return 9;

            }


        }

        private static int FindUserGroupByName(string userGroupToDelete)
        {

            var groupsReturned = userGroupTasks.SearchByNames(userGroupToDelete);

            int groupsReturnedCount = groupsReturned.Count;

            if (groupsReturnedCount == 0)
            {
                //No such group
                return -1;
            }

            return groupsReturned.First().UserGroupId;



           
        }

        internal static void InsertLAFDAUserGroups(List<DAUserGroups> lafDAUserGroupsToLoad,  string appName, string changeDescription, string fileName, ProgressBar progressbar1)
        {
            //First get the user and create the logging details for use later in audit:

            string uRL = auditText1 + currentUser + auditText2 + changeDescription;


            var appData = DataProcessingMethods.GetLAFApplications();


            var applicationSelected = DataProcessingMethods.GetApplication(appData, "LAF");


            //this.appuRL = applicationSelected.Name;
             int appID = applicationSelected.ApplicationID;


            //First get the CredentialID for the type (as per supplied description)

            progressbar1.Value = 0;
            //int credentialID = .ToInt32(credID.FirstConvert());
            int i = 0;


            //Get uRN for LAF_UserGroup Credential:
            string credTypeDesc = DA_UserGroupDescription;          
            var credData = DataProcessingMethods.GetLAFCredentials( appName);
            var credentialSelection = DataProcessingMethods.GetCredential(credData, credTypeDesc);
            var uRNID = credentialSelection.UniformResourceNameId;
            var credID = credentialSelection.CredentialID;
            var URNSelection = DataProcessingMethods.GetURNData( appName, uRNID).First();        
            var uRN = URNSelection.Urn;


            
            List<IInputData> successList = new List<IInputData>();
            DateTime dtm = DateTime.Now;
            progressbar1.Step = 1;
            progressbar1.Maximum = lafDAUserGroupsToLoad.Count();

            //Debug.WriteLine("log: Entered GetTestDataMethod")    


            //Before looping, get the Devolved Admin Credential ID which is constant (so as not to do on each iteraiton)



            var credentialTasks = Lloyds.LAF.Agent.Restricted.Factory.Resolve<ICredentialTasks>();

            

            //get Role URN...

            string credTypeDesc_Role = LAFRole_CredDescription;
            credentialSelection = DataProcessingMethods.GetCredential(credData, credTypeDesc_Role);
            var uRNIDRole = credentialSelection.UniformResourceNameId;
            var credIDRole = credentialSelection.CredentialID;
            var URNSelectionRole = DataProcessingMethods.GetURNData( appName, uRNIDRole).First();
            var uRN_Role = URNSelectionRole.Urn;


            CredentialValueSearchOptions search = new CredentialValueSearchOptions();
            search.Value = DA_CredValue;
            search.ApplicationId = appID;
            search.Urn = uRN_Role;

            var credentialValues = credentialTasks.SearchForCredentialValues(search);

            int roleCredentialValueID = credentialValues.First().CredentialValueId;


            //SHould only be 1...                                
            //int credValueID = credValues.First().CredentialValueId;

            foreach (DAUserGroups row in lafDAUserGroupsToLoad)
            {
                i++;
                
                progressbar1.PerformStep();


                //First see if LAF DA Group exists already...
                //form names..

                

                //Now insert CredentialValue (i.e. User Group)
                //First check that
                //get Credential Value from USerGroup

               string sql = "Select UserGroupID FROM " + database + ".LAF.UserGroup WHERE Name = '" + row.UserGroupName + "'";
                
               int result = ExecuteQuery(sql).Count();
                ;
                if(result == 0)
                {
                    ExcelMethods.WriteToCell(i, "Status", "LAF DA Group Creation failed - User Group does not exist.", "DAUserGroups", fileName);
                    continue;
                }
                //So it must exist...
                
                //Now form description string


                string credDescription = "LAF DA User Group Credential Value for application user group = " + row.UserGroupName + "";

                string credentialTypeDescription = "LAF_UserGroup";
                Debug.WriteLine("about to call Insert Cred");
                //And call dedicated


                var userGroupTasks = Lloyds.LAF.Agent.Restricted.Factory.Resolve<IUserGroupTask>();


                int sourceAppUserGroupID = 0;

                int DAresult = InsertLAFDaGroup( row, credDescription, fileName, uRL, i,credentialTypeDescription, appName, credentialTasks, userGroupTasks,dtm,uRN, roleCredentialValueID, out sourceAppUserGroupID, uRN_Role);



                if (DAresult > 0)
                {
                    row.UserGroupID = DAresult;
                    row.Value = sourceAppUserGroupID.ToString() ;
                    successList.Add(row);
                    ExcelMethods.WriteToCell(i, "Status", String.Format("DA User Group created (ID:{0}), Credential Value created and appropriate Credential values added successfully", DAresult), "DAUserGroups", fileName);
                    continue;
                }
                if (DAresult == -1)
                {

                    
                    ExcelMethods.WriteToCell(i, "Status", "No such user group exists", "DAUserGroups", fileName);
                    continue;
                }
                if (DAresult == -2)
                {


                    ExcelMethods.WriteToCell(i, "Status", "Devolved Admin Group already exists for this User Group with name: LAF DA_"+row.UserGroupName+"", "DAUserGroups", fileName);
                    continue;
                }









            }
            try
            {
                if (successList.Count > 0)
                {
                    DataProcessingMethods.InsertAuditEntriesUserGroups(successList, uRL, appID, 7000, dtm);
                    DataProcessingMethods.InsertAuditEntriesCredValues(successList, 9000, uRL, credID, appID, credTypeDesc, uRNID, dtm);
                }

                MessageBox.Show("Upload complete. Number of records successfully created = " + successList.Count.ToString() + " out of " + lafDAUserGroupsToLoad.Count + " attempted. See input excel file for individual upload status.", "Upload Complete");
            }
            catch
            {
                MessageBox.Show("Unknown Exception while adding Audit entries. However, the User Group records themselves were added. Number of records credential values created = " + successList.Count.ToString() + " out of " + lafDAUserGroupsToLoad.Count + " attempted. See input excel file for individual upload status.", "Upload Complete - Error adding Audit Records");
            }



            
            progressbar1.Value = 0;
        }
        

                    


private static int InsertLAFDaGroup( DAUserGroups row , string credDescription, string fileName, string uRL, int i, string credentialTypeDescription, string appName, ICredentialTasks credentialTasks,IUserGroupTask userGroupTask,DateTime dtm,string uRN,int roleCredentialValueID,out int applitionUserGroupID,string uRN_Role)

        {

            applitionUserGroupID = FindUserGroupByName(row.UserGroupName);

            
            if (applitionUserGroupID < 0) { return -1; }

     

            string groupLAFDa = DA_UserGroupName_Prefix + row.UserGroupName;
            string groupLAFDaDescription = DA_UserGroupDescription_Prefix + row.UserGroupName;
                                  
            UserGroup group = new UserGroup();

            try
            {
                credentialTasks.AddDevolvedAdminUserGroupForGivenUserGroup(groupLAFDa, groupLAFDaDescription, applitionUserGroupID, out group);
               
            }
            catch
            {
                //if here, then must have failed due to group with that name already existing.
                return -2;
            }

            //First Get credValueID for UserGroup Credential
            var credValues = GetCredentialValues(uRN, group.ApplicationId, applitionUserGroupID.ToString(), credentialTasks);
            //SHould only be 1...                                
            int credValueID = credValues.First().CredentialValueId;

            //Set Cred Value ID in output parameter
            row.CredentialValueID = credValueID;
            //And Get Cred Value Description...


            string sql = "Select Description FROM " + database + ".LAF.CredentialValue WHERE CredentialValueID = " + credValueID + "";
            
            var credValueDescription = ExecuteQuery<LAFCredentialData>(sql).First().Description;
            ;

            //set Description in output parameter
            row.Description = credValueDescription;


            //Now set UserGroupCredentialValueID For Role level credentialValueID:

            //First Get credValueID for Role Credential
            credValues = GetCredentialValues(uRN_Role, group.ApplicationId, DA_CredValue, credentialTasks);
            //SHould only be 1...                                
            int credValueRoleID = credValues.First().CredentialValueId;

            //Set Cred Value ID in output parameter
            row.CredentialValueID = credValueID;

            //Set New UserGroup name in output parameter (replace Application/ source user group name)

            row.UserGroupName = "LAF DA_" + row.UserGroupName;


            //Now get UGCVs

            int userGroupCredentialValueID = GetUGCV( group.UserGroupId, credValueID);

            int roleUserGroupCredentialValueID = GetUGCV( group.UserGroupId, credValueRoleID);

            

            //Now get UGCV for Role Credential




            if (group.UserGroupId > 0)
            {
                
                DataProcessingMethods.InsertAuditEntryUGCV(group.UserGroupId, group.ApplicationId, userGroupCredentialValueID, credValueID, uRL, 7005, dtm, group.Name);
                DataProcessingMethods.InsertAuditEntryUGCV(group.UserGroupId, group.ApplicationId, roleUserGroupCredentialValueID, roleCredentialValueID, uRL, 7005, dtm, group.Name);
            }

            return group.UserGroupId;

        }

        private static int GetUGCV( int userGroupId, int credValueID)
        {
            string sql = string.Format("Select TOP 1 UserGroupCredentialValueID FROM {0}.LAF.UserGroupCredentialValue Where UserGroupID = '{1}' AND CredentialValueID = '{2}' Order by 1 DESC", database,userGroupId, credValueID);
            
            var ugcv = ExecuteQuery<UGCVData>(sql);
            ;
            return ugcv.First().UserGroupCredentialValueID;
        }







        public static IEnumerable<LAFApplicationData> GetLAFApplications()
        {
            //Debug.WriteLine("log: Entered GetTestDataMethod");
            string sql = "Select * from "+database+".LAF.Application";
            
               var appdata = ExecuteQuery<LAFApplicationData>(sql);
            ;
            return appdata;
            //Just need to convert to list now - or leave - see Service tester notes

         
         
            }

        
   
        private static bool isUserGroupinList(List<UserGroup> userGroups, string userGroupName)
        {

            foreach (var row in userGroups)
            {
                if (row.Name == userGroupName)
                {
                    return true;
                }
                
            }
            return false;
        }
        

      
        internal static IEnumerable<LAFUniformResourceNameData> GetURNData( string appName, int uRNID)
        {
            string sql;
            
            sql = string.Format("Select * from " + database + ".LAF.UniformResourceName where UniformResourceNameID = {0}",uRNID); 

            var urnData = ExecuteQuery<LAFUniformResourceNameData>(sql);
            ;
            return urnData;
        }

        internal static IEnumerable<LAFUserData> GetUserData( string emailAddress)
        {
            string sql;

            sql = string.Format("Select * from " + database + ".LAF.[User] where EmailAddress = '{0}'", emailAddress); 


            return ExecuteQuery<LAFUserData>(sql);
        }
        internal static IEnumerable<LAFUserGroups> GetUserGroupData( string userGroup)
        {
            string sql;

            sql = string.Format("Select * from " + database + ".LAF.UserGroup where Name = '{0}'", userGroup);


            return ExecuteQuery<LAFUserGroups>(sql); ;
        }


        private static void InsertAuditEntryUGCV(int userGroupId, int appId,int userGroupCredentialValueId, int credentialValueId,string uRL, int actionId,DateTime dtm,string userGroupName)
        {
            
            
       
            
        

            string xmlHeader = string.Format("<audit action=\"UserGroupAction.CredentialAdded\"><data>");

            string xmlBody = string.Format("<usergroup id=\"{0}\" name=\"{1}\" applicationid=\"{2}\" /><usergroupcredentialvalues><usergroupcredentialvalue id=\"{3}\" credentialvalueid=\"{4}\" usergroupid=\"{5}\" />", userGroupId, userGroupName,appId, userGroupCredentialValueId, credentialValueId, userGroupId);
            string xmlEnd = "</usergroupcredentialvalues></data></audit>";

            string finalXml = xmlHeader + xmlBody + xmlEnd;

            CreateAuditEntry(actionId, uRL, finalXml,dtm);

        }





       


       private static void CreateAuditEntry(int actionId, string uRL, string xml,DateTime dtm)
       {

           
           string user =  ConfigurationManager.AppSettings["LAFDataLoaderEmail"];
           string uid = ConfigurationManager.AppSettings["LAFDataLoaderUID"];

         
               var auditServiceClient1 = new ServiceReferences.AuditService.AuditServiceClient();
               auditServiceClient1.Write(actionId, dtm, "", uRL, user, uid, xml);
         
        
           
           
       }




       internal static LAFApplicationData GetApplication(IEnumerable<LAFApplicationData> appData, string appName)
       {
           foreach (var row in appData)
           {
               if (row.Name == appName)
               {
                   return row;
               }
               
           }
           return null;
       }

       internal static LAFCredentialData GetCredential(IEnumerable<LAFCredentialData> credData, string credentialDescriptionType)
       {
           foreach (var row in credData)
           {
               if (row.Description == credentialDescriptionType)
               {
                   return row;
               }
           }
           return null;
       }

       internal static LAFUniformResourceNameData GetURN(IEnumerable<LAFUniformResourceNameData> URNData, int UniformResourceNameId)
       {
           foreach (var row in URNData)
           {
               if (row.UniformResourceNameId == UniformResourceNameId)
               {
                   return row;
               }
           }
           return null;
       }
    }
           
        }


    




