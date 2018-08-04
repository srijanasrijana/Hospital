using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogic
{
    public class UserPermission
    {
        public static bool ChkUserPermission(string code)
        {
            bool isUserPermission = false;
            int UserID = User.CurrUserID;
            Global.m_db.ClearParameter();
            Global.m_db.setCommandType(CommandType.StoredProcedure);
            Global.m_db.setCommandText("System.spCheckUserAccess");
            Global.m_db.AddParameter("@UserID", SqlDbType.Int, UserID);
            Global.m_db.AddParameter("@AccessCode", SqlDbType.NVarChar, 200, code);
            DataTable dtUserInfo = Global.m_db.GetDataTable();
            DataRow druserinfo = dtUserInfo.Rows[0];
            if (Convert.ToInt32( druserinfo["sno"].ToString())> 0)
            {
                isUserPermission = true;
            }
            else
            {
                isUserPermission = false;
            }
            return isUserPermission;
          //  return dtUserInfo;

            DataTable dtUserInfoo = User.GetUserInfo(User.CurrUserID);
            DataRow drUserInfo = dtUserInfo.Rows[0];
            DataTable dtUserAccessRoleDtl = User.GetAccessRoleDetails(Convert.ToInt32(drUserInfo["AccessRoleID"]));
            foreach (DataRow dr in dtUserAccessRoleDtl.Rows)
            {
                DataTable dtAccessInfo = User.GetAccessInfo(dr["AccessID"].ToString());
                DataRow drAccessInfo = dtAccessInfo.Rows[0];
                switch (drAccessInfo["Code"].ToString())//work according to code
                {

                    case "USER_CREATE":
                    case "USER_MODIFY":
                    case "USER_DELETE":
                    case "ACCOUNT_CREATE":
                    case "ACCOUNT_MODIFY":
                    case "ACCOUNT_DELETE":
                    case "SERVICE_CREATE":
                    case "SERVICE_MODIFY":

                    case "SERVICE_DELETE":
                    case "PRODUCT_CREATE":
                    case "PRODUCT_MODIFY":

                    case "PRODUCT_DELETE":
                    case "ACCOUNT_CLASS_CREATE":
                    case "ACCOUNT_CLASS_MODIFY":

                    case "ACCOUNT_CLASS_DELETE":
                    case "DEPOTE_CREATE":
                    case "DEPOTE_MODIFY":

                    case "DEPOTE_DELETE":
                    case "PROJECT_CREATE":
                    case "PROJECT_MODIFY":

                    case "PROJECT_DELETE":
                    case "UNIT_CONVERSION":
                    case "CASH_RECEIPT_CREATE":
                    case "CASH_RECEIPT_MODIFY":
                    case "CASH_RECEIPT_DELETE":
                    case "CASH_RECEIPT_VIEW":
                    case "PURCHASE_ORDER_CREATE":
                    case "PURCHASE_ORDER_MODIFY":

                    case "PURCHASE_ORDER_VIEW":
                    case "CHEQUE_RECEIPT_CREATE":
                    case "CHEQUE_RECEIPT_MODIFY":
                    
                    case "CHEQUE_RECEIPT_DELETE":
                    case "CHEQUE_RECEIPT_VIEW":
                    case "CASH_PAYMENT_CREATE":
                    case "CASH_PAYMENT_MODIFY":

                    case "CASH_PAYMENT_DELETE":
                    case "CASH_PAYMENT_VIEW":
                    case "BANK_RECEIPT_CREATE":
                    case "BANK_RECEIPT_MODIFY":

                    case "BANK_RECEIPT_DELETE":
                    case "BANK_RECEIPT_VIEW":
                    case "BANK_PAYMENT_CREATE":
                    case "BANK_PAYMENT_MODIFY":

                    case "BANK_PAYMENT_DELETE":
                    case "BANK_PAYMENT_VIEW":
                    case "JOURNAL_CREATE":
                    case "JOURNAL_MODIFY":

                    case "JOURNAL_DELETE":
                    case "JOURNAL_VIEW":
                    case "BANK_RECONCILATION_CREATE":
                    case "BANK_RECONCILATION_MODIFY":

                    case "BANK_RECONCILATION_DELETE":
                    case "BANK_RECONCILATION_VIEW":
                    case "SALE_INVOICE_CREATE":
                    case "SALE_INVOICE_MODIFY":

                    case "SALE_INVOICE_DELETE":
                    case "SALE_INVOICE_VIEW":
                    case "DEBIT_NOTE_CREATE":
                    case "DEBIT_NOTE_MODIFY":

                    case "DEBIT_NOTE_DELETE":
                    case "DEBIT_NOTE_VIEW":
                    case "CREDIT_NOTE_CREATE":
                    case "CREDIT_NOTE_MODIFY":

                    case "CREDIT_NOTE_DELETE":
                    case "CREDIT_NOTE_VIEW":
                    case "PURCHASE_INVOICE_CREATE":
                    case "PURCHASE_INVOICE_MODIFY":

                    case "PURCHASE_INVOICE_DELETE":
                    case "PURCHASE_INVOICE_VIEW":
                    case "CONTRA_CREATE":
                    case "CONTRA_MODIFY":

                    case "CONTRA_DELETE":
                    case "CONTRA_VIEW":
                    case "CONFIGURATION_CREATE":
                    case "CONFIGURATION_MODIFY":

                    case "CONFIGURATION_DELETE":
                    case "COMPANY_CREATE":
                    case "COMPANY_MODIFY":

                    case "COMPANY_INFO_CREATE":
                    case "COMPANY_INFO_MODIFY":

                    case "COMPANY_INFO_VIEW":
                    case "TRIAL_BALANCE":
                    case "BALANCE_SHEET":
                    case "INCOME_EXPENDITURE":
                    case "DAY_BOOKS":
                    case "DEBITNOTE_REGISTER":
                    case "CREDITNOTE_REGISTER":
                    case "STOCK_STATUS":
                    case "CASH_FLOW":
                    case "SALES_CREATE":
                    case "SALES_MODIFY":
                    
                    case "SALES_DELETE":
                    case "SETTING_CREATE":
                    case "SETTING_MODIFY":

                    case "UNIT_CREATE":
                    case "UNIT_MODIFY":

                    case "UNIT_DELETE":
                    case "ACCOUNT_LEDGER_VIEW":
                    case "PROFIT_LOSS_VIEW":
                    case "VAT_REPORT":
                    case "ACCOUNT_LEDGER":
                    case "DAMAGE_SAVE":
                    case "SETTING_VIEW":
                    case "DB_CONNECTION_SETTING":
                    case "BACKUP_RESTORE":
                    case "DATE_CONVERTER":
                    case "REMINDER_CREATE":
                    case "REMINDER_MODIFY":

                    case "REMINDER_DELETE":
                    case "REMINDER_VIEW":
                    case "ACTIVATE":
                    case "ACCESS_ROLE_CREATE":
                    case "ACCESS_ROLE_MODIFY":

                    case "ACCESS_ROLE_DELETE":
                    case "ACCESS_ROLE_VIEW":
                    case "ACCOUNT_CLASS_VIEW":
                    case "POST_DATE_TRANSACTION":
                    case "FREEZE_DATE":
                    case "SLABS_SAVE":
                    case "CHEQUE_REPORT":
                        if (code == drAccessInfo["Code"].ToString())
                        {
                            isUserPermission = true;
                        }
                        break;
                }

            }
           

        }
    }
}
