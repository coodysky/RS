CREATE TABLE RequirementStatus
(
    RequirementStatusId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,--主键自增
    RequirementStatusCode NVARCHAR(20) NOT NULL,--需求状态Code【Init:已提交,ToBeApproved:待审核,Approving:审核中,Approved:已审核,Cancelled:已删除,Completed:已完成】
    RequirementStatusNameCn NVARCHAR(20) NOT NULL,--状态中文
    RequirementStatusDesc NVARCHAR(100) NOT NULL,--状态描述
    CreateBy NVARCHAR(50) NOT NULL,--创建人
    CreateDate DATETIME NOT NULL DEFAULT ( GETDATE() ),--创建时间
    UpdateBy NVARCHAR(50) NOT NULL,--更新人
    UpdateDate DATETIME NOT NULL DEFAULT ( GETDATE() )--更新时间
);

GO

CREATE INDEX Ix_RequirementStatus__RequirementStatusCode
ON RequirementStatus (RequirementStatusCode)

GO

EXEC sp_addextendedproperty N'MS_Description', N'主键Id，自增', 'SCHEMA', N'dbo', 'TABLE', N'RequirementStatus', 'COLUMN', N'RequirementStatusId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'需求状态Code【Init:已提交,ToBeApproved:待审核,Approving:审核中,Approved:已审核,Cancelled:已删除,Completed:已完成】', 'SCHEMA', N'dbo', 'TABLE', N'RequirementStatus', 'COLUMN', N'RequirementStatusCode'
GO
EXEC sp_addextendedproperty N'MS_Description', N'状态中文', 'SCHEMA', N'dbo', 'TABLE', N'RequirementStatus', 'COLUMN', N'RequirementStatusNameCn'
GO
EXEC sp_addextendedproperty N'MS_Description', N'状态描述', 'SCHEMA', N'dbo', 'TABLE', N'RequirementStatus', 'COLUMN', N'RequirementStatusDesc'
GO
EXEC sp_addextendedproperty N'MS_Description', N'创建人', 'SCHEMA', N'dbo', 'TABLE', N'RequirementStatus', 'COLUMN', N'CreateBy'
GO
EXEC sp_addextendedproperty N'MS_Description', N'创建时间', 'SCHEMA', N'dbo', 'TABLE', N'RequirementStatus', 'COLUMN', N'CreateDate'
GO
EXEC sp_addextendedproperty N'MS_Description', N'更新人', 'SCHEMA', N'dbo', 'TABLE', N'RequirementStatus', 'COLUMN', N'UpdateBy'
GO
EXEC sp_addextendedproperty N'MS_Description', N'更新时间', 'SCHEMA', N'dbo', 'TABLE', N'RequirementStatus', 'COLUMN', N'UpdateDate'
GO