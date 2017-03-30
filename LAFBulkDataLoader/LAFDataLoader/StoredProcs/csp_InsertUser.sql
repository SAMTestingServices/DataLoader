USE [LAF]
GO
/****** Object:  StoredProcedure [LAF].[csp_InsertUser]    Script Date: 29/03/2017 16:00:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [LAF].[csp_InsertUser]
(
	@EmailAddress varchar(200),
	@Title varchar(200),
	@FirstName varchar(200),
	@LastName varchar(200),
	@IsInsuranceProfessional int,
	@organisationTypeId int,
	@CompanyName varchar(200),
	@JobTitle varchar(200),
	@CountryOfResidenceId int,
	@Telephone varchar(20)
)
AS

insert into 
	[LAF].[User] (EmailAddress,ReceiveMarketing,isDisabled,LastActivityDate,password,LastLoginDate,LastPasswordChangedDate,LastLockoutDate,FailedPasswordAttemptCount,RegistrationDate,SupportComment,PasswordSalt,title,firstname,lastname,isinsuranceprofessional,organisationTypeId,companyName,jobtitle,countryofresidenceId,telephone)
values
	(@EmailAddress,0,0,'0001-01-01 00:00:00.0000000','X69kO4Ppv6eYkVN/6WEyFEPYuVFtW9NPfhWlLTAFvKdsakhG83f5tpavqdAl+HkOyb5w2psOAAKrwnLSMZ9iug==','0001-01-01 00:00:00.0000000',GETDATE(),'0001-01-01 00:00:00.0000000',0,GETDATE(),NULL,'jBbJJKFbViTnQhwYREg6Lg==',@Title,@FirstName,@LastName,@IsInsuranceProfessional,@organisationTypeId,@CompanyName,@JobTitle,@CountryOfResidenceId,@Telephone)
