CREATE TABLE RequirementTag
(
    RequirementTagId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,--主键自增
	RequirementId INT NOT NULL,--需求Id
    Tag NVARCHAR(50) NOT NULL,--标签
    CreateBy NVARCHAR(50) NOT NULL,--创建人
    CreateDate DATETIME NOT NULL DEFAULT ( GETDATE() ),--创建时间
    UpdateBy NVARCHAR(50) NOT NULL,--更新人
    UpdateDate DATETIME NOT NULL DEFAULT ( GETDATE() )--更新时间
);

GO

CREATE INDEX Ix_RequirementTag__RequirementId
ON RequirementTag (RequirementId)

CREATE INDEX Ix_RequirementTag__Tag
ON RequirementTag (Tag)

GO

EXEC sp_addextendedproperty N'MS_Description', N'主键Id，自增', 'SCHEMA', N'dbo', 'TABLE', N'RequirementTag', 'COLUMN', N'RequirementTagId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'需求Id', 'SCHEMA', N'dbo', 'TABLE', N'RequirementTag', 'COLUMN', N'RequirementId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'标签', 'SCHEMA', N'dbo', 'TABLE', N'RequirementTag', 'COLUMN', N'Tag'
GO
EXEC sp_addextendedproperty N'MS_Description', N'创建人', 'SCHEMA', N'dbo', 'TABLE', N'RequirementTag', 'COLUMN', N'CreateBy'
GO
EXEC sp_addextendedproperty N'MS_Description', N'创建时间', 'SCHEMA', N'dbo', 'TABLE', N'RequirementTag', 'COLUMN', N'CreateDate'
GO
EXEC sp_addextendedproperty N'MS_Description', N'更新人', 'SCHEMA', N'dbo', 'TABLE', N'RequirementTag', 'COLUMN', N'UpdateBy'
GO
EXEC sp_addextendedproperty N'MS_Description', N'更新时间', 'SCHEMA', N'dbo', 'TABLE', N'RequirementTag', 'COLUMN', N'UpdateDate'
GO
