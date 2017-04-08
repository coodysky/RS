CREATE TABLE ResponseRecord
(
    ResponseRecordId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,--主键自增
    RequirementId INT NOT NULL,--需求Id
    ResponserId INT NOT NULL,--响应人Id
    Content NVARCHAR(MAX) NOT NULL,--响应内容
    ContactPhone NVARCHAR(20) NOT NULL,--联系电话
    ContactMan NVARCHAR(50) NOT NULL,--联系人
    IsDeleted BIT NOT NULL,--是否已删除
    IsFinalServeRecord BIT NOT NULL,--是否最终服务记录
    CreateBy NVARCHAR(50) NOT NULL,--创建人
    CreateDate DATETIME NOT NULL DEFAULT ( GETDATE() ),--创建时间
    UpdateBy NVARCHAR(50) NOT NULL,--更新人
    UpdateDate DATETIME NOT NULL DEFAULT ( GETDATE() )--更新时间
);

GO

CREATE INDEX Ix_ResponseRecord__RequirementId
ON ResponseRecord (RequirementId)

CREATE INDEX Ix_ResponseRecord__ResponserId
ON ResponseRecord (ResponserId)

CREATE INDEX Ix_ResponseRecord__RequirementId__ResponserId
ON ResponseRecord (RequirementId,ResponserId)

GO

EXEC sp_addextendedproperty N'MS_Description', N'主键Id，自增', 'SCHEMA', N'dbo', 'TABLE', N'ResponseRecord', 'COLUMN', N'ResponseRecordId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'需求Id', 'SCHEMA', N'dbo', 'TABLE', N'ResponseRecord', 'COLUMN', N'RequirementId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'响应人Id', 'SCHEMA', N'dbo', 'TABLE', N'ResponseRecord', 'COLUMN', N'ResponserId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'响应内容', 'SCHEMA', N'dbo', 'TABLE', N'ResponseRecord', 'COLUMN', N'Content'
GO
EXEC sp_addextendedproperty N'MS_Description', N'联系电话', 'SCHEMA', N'dbo', 'TABLE', N'ResponseRecord', 'COLUMN', N'ContactPhone'
GO
EXEC sp_addextendedproperty N'MS_Description', N'联系人', 'SCHEMA', N'dbo', 'TABLE', N'ResponseRecord', 'COLUMN', N'ContactMan'
GO
EXEC sp_addextendedproperty N'MS_Description', N'是否已删除', 'SCHEMA', N'dbo', 'TABLE', N'ResponseRecord', 'COLUMN', N'IsDeleted'
GO
EXEC sp_addextendedproperty N'MS_Description', N'是否最终服务记录', 'SCHEMA', N'dbo', 'TABLE', N'ResponseRecord', 'COLUMN', N'IsFinalServeRecord'
GO
EXEC sp_addextendedproperty N'MS_Description', N'创建人', 'SCHEMA', N'dbo', 'TABLE', N'ResponseRecord', 'COLUMN', N'CreateBy'
GO
EXEC sp_addextendedproperty N'MS_Description', N'创建时间', 'SCHEMA', N'dbo', 'TABLE', N'ResponseRecord', 'COLUMN', N'CreateDate'
GO
EXEC sp_addextendedproperty N'MS_Description', N'更新人', 'SCHEMA', N'dbo', 'TABLE', N'ResponseRecord', 'COLUMN', N'UpdateBy'
GO
EXEC sp_addextendedproperty N'MS_Description', N'更新时间', 'SCHEMA', N'dbo', 'TABLE', N'ResponseRecord', 'COLUMN', N'UpdateDate'
GO