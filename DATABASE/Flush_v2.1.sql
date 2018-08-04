--Flush Acc_Swift Database
--Created By :Bimal Bahadur Khadka
--2015/06/24 

--**********************************************************
--Accounting
DELETE FROM Acc.tblJournalDetail;
DELETE FROM Acc.tblJournalMaster;
DBCC CHECKIDENT ('Acc.tblJournalDetail', RESEED, 0);
DBCC CHECKIDENT ('Acc.tblJournalMaster', RESEED, 0);

DELETE FROM Acc.tblBankPaymentDetails;
DELETE FROM Acc.tblBankPaymentMaster;
DBCC CHECKIDENT ('Acc.tblBankPaymentDetails', RESEED, 0);
DBCC CHECKIDENT ('Acc.tblBankPaymentMaster', RESEED, 0);

DELETE FROM Acc.tblBankReceiptDetails;
DELETE FROM Acc.tblBankReceiptMaster;
DBCC CHECKIDENT ('Acc.tblBankReceiptDetails', RESEED, 0);
DBCC CHECKIDENT ('Acc.tblBankReceiptMaster', RESEED, 0);

DELETE FROM Acc.tblCashPaymentDetails;
DELETE FROM Acc.tblCashPaymentMaster;
DBCC CHECKIDENT ('Acc.tblCashPaymentDetails', RESEED, 0);
DBCC CHECKIDENT ('Acc.tblCashPaymentMaster', RESEED, 0);

DELETE FROM Acc.tblCashReceiptDetails;
DELETE FROM Acc.tblCashReceiptMaster;
DBCC CHECKIDENT ('Acc.tblCashReceiptDetails', RESEED, 0);
DBCC CHECKIDENT ('Acc.tblCashReceiptMaster', RESEED, 0);

DELETE FROM Acc.tblContraDetails;
DELETE FROM Acc.tblContraMaster;
DBCC CHECKIDENT ('Acc.tblContraDetails', RESEED, 0);
DBCC CHECKIDENT ('Acc.tblContraMaster', RESEED, 0);

DELETE FROM Acc.tblCreditNoteDetail;
DELETE FROM Acc.tblCreditNoteMaster;
DBCC CHECKIDENT ('Acc.tblCreditNoteDetail', RESEED, 0);
DBCC CHECKIDENT ('Acc.tblCreditNoteMaster', RESEED, 0);

DELETE FROM Acc.tblDebitNoteDetail;
DELETE FROM Acc.tblDebitNoteMaster;
DBCC CHECKIDENT ('Acc.tblDebitNoteDetail', RESEED, 0);
DBCC CHECKIDENT ('Acc.tblDebitNoteMaster', RESEED, 0);

DELETE FROM Acc.tblBankReconciliationDetails;
DELETE FROM Acc.tblBankReconciliationMaster;
DBCC CHECKIDENT ('Acc.tblBankReconciliationDetails', RESEED, 0);
DBCC CHECKIDENT ('Acc.tblBankReconciliationMaster', RESEED, 0);

Delete from Acc.tblBankReconSaveStateDetails
DBCC CHECKIDENT ('Acc.tblBankReconSaveStateDetails', RESEED, 0);

Delete from Acc.tblBankReconSaveStateMaster
DBCC CHECKIDENT ('Acc.tblBankReconSaveStateMaster', RESEED, 0);

Delete from Acc.tblClosedBankReconciliation
DBCC CHECKIDENT ('Acc.tblClosedBankReconciliation', RESEED, 0);

Delete from Acc.tblChequeReceiptMaster
delete from Acc.tblChequeReceiptDetail
delete from Acc.tblChequeReceiptAccClass
DBCC CHECKIDENT ('Acc.tblChequeReceiptMaster', RESEED, 0);
DBCC CHECKIDENT ('Acc.tblChequeReceiptDetail', RESEED, 0);
DBCC CHECKIDENT ('Acc.tblChequeReceiptAccClass', RESEED, 0);

DELETE FROM Acc.tblTransactionClass;
DBCC CHECKIDENT ('Acc.tblTransactionClass', RESEED, 0);

DELETE FROM Acc.tblTransaction;
DBCC CHECKIDENT ('Acc.tblTransaction', RESEED, 0);

delete from System.tblReferenceVoucher
DBCC CHECKIDENT ('System.tblReferenceVoucher', RESEED, 0);

delete from System.tblReference
DBCC CHECKIDENT ('System.tblReference', RESEED, 0);

DELETE FROM Acc.tblLedger WHERE BuiltIn=0;
DBCC CHECKIDENT ('Acc.tblBankPaymentDetails', RESEED, 0);

DELETE FROM Acc.tblGroup WHERE BuiltIn=0;
DBCC CHECKIDENT ('Acc.tblBankPaymentDetails', RESEED, 0);

DELETE FROM Acc.tblOpeningBalance;
DBCC CHECKIDENT ('Acc.tblGroup', RESEED, 0);

DELETE FROM Acc.tblPreYearBalance;
DBCC CHECKIDENT ('Acc.tblPreYearBalance', RESEED, 0);

delete from Acc.tblBulkPostingDetails
DBCC CHECKIDENT ('Acc.tblBankPaymentDetails', RESEED, 0);

delete from Acc.tblBulkPostingDetails
DBCC CHECKIDENT ('Acc.tblBankPaymentDetails', RESEED, 0);

Delete from Acc.tblBudgetAllocationDetail
DBCC CHECKIDENT ('Acc.tblBudgetAllocationDetail', RESEED, 0);

Delete from Acc.tblBudgetAllocationMaster
DBCC CHECKIDENT ('Acc.tblBudgetAllocationMaster', RESEED, 0);

Delete from Acc.tblBudget
DBCC CHECKIDENT ('Acc.tblBudget', RESEED, 0);

delete from Acc.tblAccClass where AccClassID > 1;

Delete from Acc.tblProject where ProjectID > 1;

--**********************************************************
--System
delete from System.tblAuditLog
DBCC CHECKIDENT ('System.tblAuditLog', RESEED, 0);

delete from System.tblduedate
DBCC CHECKIDENT ('System.tblduedate', RESEED, 0);

delete from System.tblErrorLog
DBCC CHECKIDENT ('System.tblErrorLog', RESEED, 0);

delete from System.tblReminder
DBCC CHECKIDENT ('System.tblReminder', RESEED, 0);

delete from System.tblReminderUser
DBCC CHECKIDENT ('System.tblReminderUser', RESEED, 0);

delete from System.tblRecurrence
DBCC CHECKIDENT ('System.tblRecurrence', RESEED, 0);

delete from System.tblRecurringVoucher
DBCC CHECKIDENT ('System.tblRecurringVoucher', RESEED, 0);

delete from System.tblRecurringVoucherPosting
DBCC CHECKIDENT ('System.tblRecurringVoucherPosting', RESEED, 0);


Update System.tblSeries set AutoNumber = 0;
--**********************************************************

--INVENTORY
delete from Inv.tblOpeningQuantity
DBCC CHECKIDENT ('Inv.tblOpeningQuantity', RESEED, 0);

DELETE FROM Inv.tblPurchaseOrderAccClass;
DBCC CHECKIDENT ('Inv.tblPurchaseOrderAccClass', RESEED, 0);

DELETE FROM Inv.tblPurchaseInvoiceDetails;
DELETE FROM Inv.tblPurchaseInvoiceMaster;
DBCC CHECKIDENT ('Inv.tblPurchaseInvoiceDetails', RESEED, 0);
DBCC CHECKIDENT ('Inv.tblPurchaseInvoiceMaster', RESEED, 0);

DELETE FROM Inv.tblPurchaseOrderDetails;
DELETE FROM Inv.tblPurchaseOrderMaster;
DBCC CHECKIDENT ('Inv.tblPurchaseOrderDetails', RESEED, 0);
DBCC CHECKIDENT ('Inv.tblPurchaseOrderMaster', RESEED, 0);

DELETE FROM Inv.tblPurchaseReturnDetails;
DELETE FROM Inv.tblPurchaseReturnMaster;
DBCC CHECKIDENT ('Inv.tblPurchaseReturnDetails', RESEED, 0);
DBCC CHECKIDENT ('Inv.tblPurchaseReturnMaster', RESEED, 0);

DELETE FROM Inv.tblSalesCounter;
DBCC CHECKIDENT ('Inv.tblSalesCounter', RESEED, 0);

DELETE FROM Inv.tblSalesInvoiceDetails;
DELETE FROM Inv.tblSalesInvoiceMaster;
DBCC CHECKIDENT ('Inv.tblSalesInvoiceDetails', RESEED, 0);
DBCC CHECKIDENT ('Inv.tblSalesInvoiceMaster', RESEED, 0);

DELETE FROM Inv.tblStockTransferAccClass;
DELETE FROM Inv.tblStockTransferDetails;
DELETE FROM Inv.tblStockTransferMaster;
DBCC CHECKIDENT ('Inv.tblStockTransferAccClass', RESEED, 0);
DBCC CHECKIDENT ('Inv.tblStockTransferDetails', RESEED, 0);
DBCC CHECKIDENT ('Inv.tblStockTransferMaster', RESEED, 0);

DELETE FROM Inv.tblSalesOrderAccClass;
DBCC CHECKIDENT ('Inv.tblSalesOrderAccClass', RESEED, 0);

DELETE FROM Inv.tblSalesOrderDetails;
DELETE FROM Inv.tblSalesOrderMaster;
DBCC CHECKIDENT ('Inv.tblSalesOrderDetails', RESEED, 0);
DBCC CHECKIDENT ('Inv.tblSalesOrderMaster', RESEED, 0);

DELETE FROM Inv.tblSalesReturnDetails;
DELETE FROM Inv.tblSalesReturnMaster;
DBCC CHECKIDENT ('Inv.tblSalesReturnDetails', RESEED, 0);
DBCC CHECKIDENT ('Inv.tblSalesReturnMaster', RESEED, 0);

delete from Inv.tblInventoryTrans
DBCC CHECKIDENT ('Inv.tblInventoryTrans', RESEED, 0);

delete from Inv.tblDamageItemsAccClass
delete from Inv.tblDamageProductInvoiceDetail
delete from Inv.tblDamageProductInvoiceMaster
DBCC CHECKIDENT ('Inv.tblDamageItemsAccClass', RESEED, 0);
DBCC CHECKIDENT ('Inv.tblDamageProductInvoiceDetail', RESEED, 0);
DBCC CHECKIDENT ('Inv.tblDamageProductInvoiceMaster', RESEED, 0);

DELETE FROM Inv.tblProduct;
DBCC CHECKIDENT ('Inv.tblProduct', RESEED, 0);

--Delete users and access roles
DELETE FROM System.tblUser WHERE UserID>1;

DELETE FROM System.tblAccessRole WHERE BuiltIn=0;

--**********************************************************
-- HRM AND HOSPITAL

--HOSPITAL
DELETE FROM Hos.tblEmployeementDetails
DBCC CHECKIDENT ('Hos.tblEmployeementDetails', RESEED, 0);

DELETE FROM Hos.tblAcademicQualification
DBCC CHECKIDENT ('Hos.tblAcademicQualification', RESEED, 0);

DELETE FROM Hos.tblDoctorPhoto
DBCC CHECKIDENT ('Hos.tblDoctorPhoto', RESEED, 0);

DELETE FROM Hos.tblPatient
DBCC CHECKIDENT ('Hos.tblPatient', RESEED, 0);

DELETE FROM Hos.tblDoctor
DBCC CHECKIDENT ('Hos.tblDoctor', RESEED, 0);

--HRM

DELETE FROM HRM.tblEmployeementDetails
DBCC CHECKIDENT ('HRM.tblEmployeementDetails', RESEED, 0);

DELETE FROM HRM.tblWorkExperiences
DBCC CHECKIDENT ('HRM.tblWorkExperiences', RESEED, 0);

DELETE FROM HRM.tblAcademicQualification
DBCC CHECKIDENT ('HRM.tblAcademicQualification', RESEED, 0);

DELETE FROM HRM.tblEmployeeLoan
DBCC CHECKIDENT ('HRM.tblEmployeeLoan', RESEED, 0);

DELETE FROM HRM.tblEmployeeAdvance
DBCC CHECKIDENT ('HRM.tblEmployeeAdvance', RESEED, 0);

DELETE FROM HRM.tblEmployeePhoto
DBCC CHECKIDENT ('HRM.tblEmployeePhoto', RESEED, 0);

DELETE FROM HRM.tblEmployee
DBCC CHECKIDENT ('HRM.tblEmployee', RESEED, 0);

DELETE FROM HRm.tblPartTimeSalaryDetail
DBCC CHECKIDENT ('HRm.tblPartTimeSalaryDetail', RESEED, 0);

DELETE from HRM.tblPartTimeSalaryMaster
DBCC CHECKIDENT ('HRM.tblPartTimeSalaryMaster', RESEED, 0);

DELETE from HRM.tblSalaryPayslipMaster
DBCC CHECKIDENT ('HRM.tblSalaryPayslipMaster', RESEED, 0);