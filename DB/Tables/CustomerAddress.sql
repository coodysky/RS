CREATE TABLE CustomerAddress
(
    CustomerAddressId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,--主键自增
    CustomerId INT NOT NULL,--用户Id
    Address NVARCHAR(1000) NOT NULL,--具体地址
    Longitude NVARCHAR(20) NOT NULL,--经度
    Latitude NVARCHAR(20) NOT NULL,--纬度
    IsDefault BIT NOT NULL,--是否默认地址
    CreateBy NVARCHAR(50) NOT NULL,--创建人
    CreateDate DATETIME NOT NULL DEFAULT ( GETDATE() ),--创建时间
    UpdateBy NVARCHAR(50) NOT NULL,--更新人
    UpdateDate DATETIME NOT NULL DEFAULT ( GETDATE() )--更新时间
);

GO

CREATE INDEX Ix_CustomerAddress__CustomerId
ON CustomerAddress (CustomerId)

CREATE INDEX Ix_CustomerAddress__CustomerId__IsDefault
ON CustomerAddress (CustomerId,IsDefault)

GO

EXEC sp_addextendedproperty N'MS_Description', N'主键Id，自增', 'SCHEMA', N'dbo', 'TABLE', N'CustomerAddress', 'COLUMN', N'CustomerAddressId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'用户Id', 'SCHEMA', N'dbo', 'TABLE', N'CustomerAddress', 'COLUMN', N'CustomerId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'具体地址', 'SCHEMA', N'dbo', 'TABLE', N'CustomerAddress', 'COLUMN', N'Address'
GO
EXEC sp_addextendedproperty N'MS_Description', N'经度', 'SCHEMA', N'dbo', 'TABLE', N'CustomerAddress', 'COLUMN', N'Longitude'
GO
EXEC sp_addextendedproperty N'MS_Description', N'纬度', 'SCHEMA', N'dbo', 'TABLE', N'CustomerAddress', 'COLUMN', N'Latitude'
GO
EXEC sp_addextendedproperty N'MS_Description', N'是否默认地址', 'SCHEMA', N'dbo', 'TABLE', N'CustomerAddress', 'COLUMN', N'IsDefault'
GO
EXEC sp_addextendedproperty N'MS_Description', N'创建人', 'SCHEMA', N'dbo', 'TABLE', N'CustomerAddress', 'COLUMN', N'CreateBy'
GO
EXEC sp_addextendedproperty N'MS_Description', N'创建时间', 'SCHEMA', N'dbo', 'TABLE', N'CustomerAddress', 'COLUMN', N'CreateDate'
GO
EXEC sp_addextendedproperty N'MS_Description', N'更新人', 'SCHEMA', N'dbo', 'TABLE', N'CustomerAddress', 'COLUMN', N'UpdateBy'
GO
EXEC sp_addextendedproperty N'MS_Description', N'更新时间', 'SCHEMA', N'dbo', 'TABLE', N'CustomerAddress', 'COLUMN', N'UpdateDate'
GO