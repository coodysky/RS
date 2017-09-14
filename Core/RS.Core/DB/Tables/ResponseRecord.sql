CREATE TABLE ResponseRecord
(
    ResponseRecordId INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT '主键自增',
    RequirementId INT NOT NULL COMMENT '需求Id',
    ResponserId INT NOT NULL COMMENT '响应人Id',
	Title VARCHAR(50) NOT NULL COMMENT '标题',
	Price DECIMAL(18,3) NULL COMMENT '金额',
    Content VARCHAR(4000) NOT NULL COMMENT '响应内容',
    ContactPhone VARCHAR(20) NOT NULL COMMENT '联系电话',
    ContactMan VARCHAR(50) NOT NULL COMMENT '联系人',
    IsDeleted BIT NOT NULL COMMENT '是否已删除',
    IsFinalServeRecord BIT NOT NULL COMMENT '是否最终服务记录',
    CreateBy VARCHAR(50) NOT NULL COMMENT '创建人',
    CreateDate BIGINT NOT NULL DEFAULT '0' COMMENT '创建时间',
    UpdateBy VARCHAR(50) NOT NULL COMMENT '更新人',
    UpdateDate BIGINT NOT NULL DEFAULT '0' COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT '需求响应表';

CREATE INDEX Ix_ResponseRecord__RequirementId
ON ResponseRecord (RequirementId);

CREATE INDEX Ix_ResponseRecord__ResponserId
ON ResponseRecord (ResponserId);

CREATE INDEX Ix_ResponseRecord__RequirementId__ResponserId
ON ResponseRecord (RequirementId,ResponserId);