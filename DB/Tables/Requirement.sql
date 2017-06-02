CREATE TABLE Requirement
(
    RequirementId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,--主键自增
	CustomerId INT NOT NULL,--用户Id
    Title NVARCHAR(50) NOT NULL,--标题
    Content NVARCHAR(MAX) NOT NULL,--内容
    Address NVARCHAR(1000) NULL,--需求地址
    Longitude DECIMAL(10,7) NULL,--经度
    Latitude DECIMAL(10,7) NULL,--纬度
    ContactPhone NVARCHAR(20) NULL,--联系电话
    ContactMan NVARCHAR(50) NULL,--联系人
    RequirementStatusCode NVARCHAR(20) NOT NULL,--需求状态
    ReleaseDate DATETIME NULL,--发布时间
    CreateBy NVARCHAR(50) NOT NULL,--创建人
    CreateDate DATETIME NOT NULL DEFAULT ( GETDATE() ),--创建时间
    UpdateBy NVARCHAR(50) NOT NULL,--更新人
    UpdateDate DATETIME NOT NULL DEFAULT ( GETDATE() )--更新时间
);

GO

CREATE INDEX Ix_Requirement__Title
ON Requirement (Title)

GO

EXEC sp_addextendedproperty N'MS_Description', N'主键Id，自增', 'SCHEMA', N'dbo', 'TABLE', N'Requirement', 'COLUMN', N'RequirementId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'用户Id', 'SCHEMA', N'dbo', 'TABLE', N'Requirement', 'COLUMN', N'CustomerId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'标题', 'SCHEMA', N'dbo', 'TABLE', N'Requirement', 'COLUMN', N'Title'
GO
EXEC sp_addextendedproperty N'MS_Description', N'内容', 'SCHEMA', N'dbo', 'TABLE', N'Requirement', 'COLUMN', N'Content'
GO
EXEC sp_addextendedproperty N'MS_Description', N'需求地址', 'SCHEMA', N'dbo', 'TABLE', N'Requirement', 'COLUMN', N'Address'
GO
EXEC sp_addextendedproperty N'MS_Description', N'经度', 'SCHEMA', N'dbo', 'TABLE', N'Requirement', 'COLUMN', N'Longitude'
GO
EXEC sp_addextendedproperty N'MS_Description', N'纬度', 'SCHEMA', N'dbo', 'TABLE', N'Requirement', 'COLUMN', N'Latitude'
GO
EXEC sp_addextendedproperty N'MS_Description', N'联系电话', 'SCHEMA', N'dbo', 'TABLE', N'Requirement', 'COLUMN', N'ContactPhone'
GO
EXEC sp_addextendedproperty N'MS_Description', N'联系人', 'SCHEMA', N'dbo', 'TABLE', N'Requirement', 'COLUMN', N'ContactMan'
GO
EXEC sp_addextendedproperty N'MS_Description', N'需求状态', 'SCHEMA', N'dbo', 'TABLE', N'Requirement', 'COLUMN', N'RequirementStatusCode'
GO
EXEC sp_addextendedproperty N'MS_Description', N'发布时间', 'SCHEMA', N'dbo', 'TABLE', N'Requirement', 'COLUMN', N'ReleaseDate'
GO
EXEC sp_addextendedproperty N'MS_Description', N'创建人', 'SCHEMA', N'dbo', 'TABLE', N'Requirement', 'COLUMN', N'CreateBy'
GO
EXEC sp_addextendedproperty N'MS_Description', N'创建时间', 'SCHEMA', N'dbo', 'TABLE', N'Requirement', 'COLUMN', N'CreateDate'
GO
EXEC sp_addextendedproperty N'MS_Description', N'更新人', 'SCHEMA', N'dbo', 'TABLE', N'Requirement', 'COLUMN', N'UpdateBy'
GO
EXEC sp_addextendedproperty N'MS_Description', N'更新时间', 'SCHEMA', N'dbo', 'TABLE', N'Requirement', 'COLUMN', N'UpdateDate'
GO
