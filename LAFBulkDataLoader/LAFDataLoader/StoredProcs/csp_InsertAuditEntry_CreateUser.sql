USE [LAF]
GO
/****** Object:  StoredProcedure [LAF].[csp_InsertAuditEntry_CreateUser]    Script Date: 29/03/2017 15:54:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [LAF].[csp_InsertAuditEntry_CreateUser] @email nvarchar(200), @URL nvarchar(200)
AS

DECLARE @userId INT = (Select UserID FROM 	LAF.[User] WHERE EmailAddress = @email)
DECLARE @Uuid varchar(255) = (Select Uid FROM LAF.LAF.[User] Where UserId = @userId)
DECLARE @UpassCount INT = (Select FailedPasswordAttemptCount FROM LAF.LAF.[User] Where UserId = @userId)
DECLARE @UisLocked INT = (Select IsDisabled FROM LAF.LAF.[User] Where UserId = @userId)
DECLARE @UisActive INT = (Select IsActive FROM LAF.LAF.[User] Where UserId = @userId)

DECLARE @UisActiveText varchar(10) = CASE @UisActive WHEN 1 THEN 'true' ELSE 'false' END
DECLARE @UisLockedText varchar(10) = CASE @UisLocked WHEN 1 THEN 'true' ELSE 'false' END
DECLARE	@detail	xml  = '<audit action="UserAction.Created"><data><user id="' + CAST(@userId as VARCHAR(50)) + '" uid="' + CAST(@Uuid as VARCHAR(50)) + '" isactive="' + @UisActiveText + '" isdisabled="' + @UisLockedText + '" failedpasswordattemptcount="' + CAST(@UpassCount as VARCHAR(50)) + '" /></data></audit>'



	INSERT INTO LAF_Audit.Audit.[Entry]
	([ActionId], [Time], [URL], [Detail])  VALUES
	(1004, GETDATE(), @URL, @detail);


