-- Replace the username and objectId with the output from main.bicep
CREATE USER [chewie-webapp-ld2ijhpvmb34c] FROM EXTERNAL PROVIDER WITH OBJECT_ID='e37b130a-f343-49a3-a4e1-7710eef45e1f'
ALTER ROLE db_accessadmin
	ADD MEMBER [chewie-webapp-ld2ijhpvmb34c];  
GO
ALTER ROLE db_datareader
	ADD MEMBER [chewie-webapp-ld2ijhpvmb34c];  
GO
ALTER ROLE db_datawriter
	ADD MEMBER [chewie-webapp-ld2ijhpvmb34c];  
GO

-- Additional permissions needed for migrations
GRANT ALTER TO [chewie-webapp-ld2ijhpvmb34c];
GRANT CREATE TABLE TO [chewie-webapp-ld2ijhpvmb34c];
GRANT REFERENCES TO [chewie-webapp-ld2ijhpvmb34c];
GO