CREATE TABLE RequirementImage
(
    RequirementImageId INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT '主键自增',
    RequirementId INT NOT NULL COMMENT '需求Id',
    ImageType VARCHAR(10) NOT NULL COMMENT '图片类型',
    ImageUrl VARCHAR(1000) NOT NULL COMMENT '图片Url',
    CreateBy VARCHAR(50) NOT NULL COMMENT '创建人',
    CreateDate BIGINT NOT NULL DEFAULT '0' COMMENT '创建时间',
    UpdateBy VARCHAR(50) NOT NULL COMMENT '更新人',
    UpdateDate BIGINT NOT NULL DEFAULT '0' COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT '需求图片表';

CREATE INDEX Ix_RequirementImage__RequirementId
ON RequirementImage (RequirementId);
