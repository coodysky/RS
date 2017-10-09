CREATE TABLE CustomerAddress
(
    CustomerAddressId INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT '主键自增',
    CustomerId INT NOT NULL COMMENT '用户Id',
    Address VARCHAR(1000) NOT NULL COMMENT '具体地址',
    Longitude DECIMAL(10,7) NOT NULL COMMENT '经度',
    Latitude DECIMAL(10,7) NOT NULL COMMENT '纬度',
    IsDefault BIT NOT NULL COMMENT '是否默认地址',
    CreateBy VARCHAR(50) NOT NULL COMMENT '创建人',
    CreateDate BIGINT NOT NULL DEFAULT '0' COMMENT '创建时间',
    UpdateBy VARCHAR(50) NOT NULL COMMENT '更新人',
    UpdateDate BIGINT NOT NULL DEFAULT '0' COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT '用户地址表';

CREATE INDEX Ix_CustomerAddress__CustomerId
ON CustomerAddress (CustomerId);

CREATE INDEX Ix_CustomerAddress__CustomerId__IsDefault
ON CustomerAddress (CustomerId,IsDefault);

CREATE INDEX Ix_CustomerAddress__Longitude__Latitude
ON CustomerAddress (Longitude,Latitude);