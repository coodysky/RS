CREATE TABLE RequirementImage
(
    RequirementImageId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,--主键自增
    RequirementId INT NOT NULL,--需求Id
    ImageType NVARCHAR(10) NOT NULL,--图片类型
    ImageUrl NVARCHAR(1000) NOT NULL,--图片Url
    CreateBy NVARCHAR(50) NOT NULL,--创建人
    CreateDate DATETIME NOT NULL DEFAULT ( GETDATE() ),--创建时间
    UpdateBy NVARCHAR(50) NOT NULL,--更新人
    UpdateDate DATETIME NOT NULL DEFAULT ( GETDATE() )--更新时间
);

GO

CREATE INDEX Ix_RequirementImage__RequirementId
ON RequirementImage (RequirementId)

GO

EXEC sp_addextendedproperty N'MS_Description', N'主键Id，自增', 'SCHEMA', N'dbo', 'TABLE', N'RequirementImage', 'COLUMN', N'RequirementImageId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'需求Id', 'SCHEMA', N'dbo', 'TABLE', N'RequirementImage', 'COLUMN', N'RequirementId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'图片类型', 'SCHEMA', N'dbo', 'TABLE', N'RequirementImage', 'COLUMN', N'ImageType'
GO
EXEC sp_addextendedproperty N'MS_Description', N'图片Url', 'SCHEMA', N'dbo', 'TABLE', N'RequirementImage', 'COLUMN', N'ImageUrl'
GO
EXEC sp_addextendedproperty N'MS_Description', N'创建人', 'SCHEMA', N'dbo', 'TABLE', N'RequirementImage', 'COLUMN', N'CreateBy'
GO
EXEC sp_addextendedproperty N'MS_Description', N'创建时间', 'SCHEMA', N'dbo', 'TABLE', N'RequirementImage', 'COLUMN', N'CreateDate'
GO
EXEC sp_addextendedproperty N'MS_Description', N'更新人', 'SCHEMA', N'dbo', 'TABLE', N'RequirementImage', 'COLUMN', N'UpdateBy'
GO
EXEC sp_addextendedproperty N'MS_Description', N'更新时间', 'SCHEMA', N'dbo', 'TABLE', N'RequirementImage', 'COLUMN', N'UpdateDate'
GO