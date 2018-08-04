
create procedure [HRM].[spGetPaySlipByMonthEmp]
(
@monthID int=null,
@empId int=null
)
as
SELECT        d.paySlipId, m.monthID, m.date, d.employeeID
FROM            HRM.tblPaySlipMasterDetails d INNER JOIN
                         HRM.tblSalaryPayslipMaster m ON d.paySlipId = m.salaryPaySlipID where m.monthID=@monthID and d.employeeID=@empId;
