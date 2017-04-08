CREATE TABLE Customer
(
    CustomerId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,--主键自增
    NickName NVARCHAR(50) NOT NULL,--昵称
    RealName NVARCHAR(50) NOT NULL,--真实姓名
    Password NVARCHAR(100) NOT NULL,--密码，加密
    MobilePhone NVARCHAR(20) NOT NULL,--注册的手机号，唯一
    Email NVARCHAR(100) NOT NULL,--电子邮箱
    CreateBy NVARCHAR(50) NOT NULL,--创建人
    CreateDate DATETIME NOT NULL DEFAULT ( GETDATE() ),--创建时间
    UpdateBy NVARCHAR(50) NOT NULL,--更新人
    UpdateDate DATETIME NOT NULL DEFAULT ( GETDATE() )--更新时间
);

GO

CREATE INDEX Ix_Customer__NickName
ON Customer (NickName)

CREATE INDEX Ix_Customer__MobilePhone
ON Customer (MobilePhone)

CREATE INDEX Ix_Customer__MobilePhone__Password
ON Customer (MobilePhone,Password)

GO

EXEC sp_addextendedproperty N'MS_Description', N'主键Id，自增', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'CustomerId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'昵称', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'NickName'
GO
EXEC sp_addextendedproperty N'MS_Description', N'真实姓名', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'RealName'
GO
EXEC sp_addextendedproperty N'MS_Description', N'密码，加密', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'Password'
GO
EXEC sp_addextendedproperty N'MS_Description', N'注册的手机号，唯一', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'MobilePhone'
GO
EXEC sp_addextendedproperty N'MS_Description', N'电子邮箱', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'Email'
GO
EXEC sp_addextendedproperty N'MS_Description', N'创建人', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'CreateBy'
GO
EXEC sp_addextendedproperty N'MS_Description', N'创建时间', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'CreateDate'
GO
EXEC sp_addextendedproperty N'MS_Description', N'更新人', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'UpdateBy'
GO
EXEC sp_addextendedproperty N'MS_Description', N'更新时间', 'SCHEMA', N'dbo', 'TABLE', N'Customer', 'COLUMN', N'UpdateDate'
GO