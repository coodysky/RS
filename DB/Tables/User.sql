CREATE TABLE [User]
(
    UserId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,--主键自增
    UserName NVARCHAR(50) NOT NULL,--登录账号
    RealName NVARCHAR(50) NOT NULL,--真实姓名
    Password NVARCHAR(100) NOT NULL,--密码，加密
    CreateBy NVARCHAR(50) NOT NULL,--创建人
    CreateDate DATETIME NOT NULL DEFAULT ( GETDATE() ),--创建时间
    UpdateBy NVARCHAR(50) NOT NULL,--更新人
    UpdateDate DATETIME NOT NULL DEFAULT ( GETDATE() )--更新时间
);

GO

CREATE INDEX Ix_User__UserName
ON [User] (UserName)

CREATE INDEX Ix_User__RealName
ON [User] (RealName)

CREATE INDEX Ix_User__RealName__Password
ON [User] (RealName,Password)

GO

EXEC sp_addextendedproperty N'MS_Description', N'主键Id，自增', 'SCHEMA', N'dbo', 'TABLE', N'User', 'COLUMN', N'UserId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'登录账号', 'SCHEMA', N'dbo', 'TABLE', N'User', 'COLUMN', N'UserName'
GO
EXEC sp_addextendedproperty N'MS_Description', N'真实姓名', 'SCHEMA', N'dbo', 'TABLE', N'User', 'COLUMN', N'RealName'
GO
EXEC sp_addextendedproperty N'MS_Description', N'密码，加密', 'SCHEMA', N'dbo', 'TABLE', N'User', 'COLUMN', N'Password'
GO
EXEC sp_addextendedproperty N'MS_Description', N'创建人', 'SCHEMA', N'dbo', 'TABLE', N'User', 'COLUMN', N'CreateBy'
GO
EXEC sp_addextendedproperty N'MS_Description', N'创建时间', 'SCHEMA', N'dbo', 'TABLE', N'User', 'COLUMN', N'CreateDate'
GO
EXEC sp_addextendedproperty N'MS_Description', N'更新人', 'SCHEMA', N'dbo', 'TABLE', N'User', 'COLUMN', N'UpdateBy'
GO
EXEC sp_addextendedproperty N'MS_Description', N'更新时间', 'SCHEMA', N'dbo', 'TABLE', N'User', 'COLUMN', N'UpdateDate'
GO