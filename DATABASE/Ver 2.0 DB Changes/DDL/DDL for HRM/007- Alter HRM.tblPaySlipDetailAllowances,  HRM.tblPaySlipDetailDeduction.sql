--Created by : Sunil Shrestha; on : June 16 2016 --
--Change the name of the column and its datatype so it will be dynamic--
exec sp_rename 'HRM.tblPaySlipDetailAllowances.allowanceName', 'allowanceID','COLUMN';
exec sp_rename 'HRM.tblPaySlipDetailDeduction.deductionName', 'deductionID','COLUMN';

Alter Table HRM.tblPaySlipDetailAllowances
	Alter Column allowanceID int;

Alter Table HRM.tblPaySlipDetailDeduction
	Alter Column deductionID int;