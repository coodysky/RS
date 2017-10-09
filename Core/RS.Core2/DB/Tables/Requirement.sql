CREATE TABLE Requirement
(
    RequirementId INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT '主键自增',
	CustomerId INT NOT NULL COMMENT '用户Id',
    Title VARCHAR(50) NOT NULL COMMENT '标题',
    Content VARCHAR(4000) NOT NULL COMMENT '内容',
	Price DECIMAL(18,3) NULL COMMENT '价格',
    Address VARCHAR(1000) NULL COMMENT '需求地址',
    Longitude DECIMAL(10,7) NULL COMMENT '经度',
    Latitude DECIMAL(10,7) NULL COMMENT '纬度',
    ContactPhone VARCHAR(20) NULL COMMENT '联系电话',
    ContactMan VARCHAR(50) NULL COMMENT '联系人',
    RequirementStatusCode VARCHAR(20) NOT NULL COMMENT '需求状态',
    ReleaseDate BIGINT NULL DEFAULT '0' COMMENT '发布时间',
    CreateBy VARCHAR(50) NOT NULL COMMENT '创建人',
    CreateDate BIGINT NOT NULL DEFAULT '0' COMMENT '创建时间',
    UpdateBy VARCHAR(50) NOT NULL COMMENT '更新人',
    UpdateDate BIGINT NOT NULL DEFAULT '0' COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT '需求表';

CREATE INDEX Ix_Requirement__Title
ON Requirement (Title);

CREATE INDEX Ix_Requirement__Longitude__Latitude
ON Requirement (Longitude,Latitude);

