CREATE TABLE RequirementTag
(
    RequirementTagId INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT '主键自增',
	RequirementId INT NOT NULL COMMENT '需求Id',
    Tag VARCHAR(50) NOT NULL COMMENT '标签',
    CreateBy VARCHAR(50) NOT NULL COMMENT '创建人',
    CreateDate BIGINT NOT NULL DEFAULT '0' COMMENT '创建时间',
    UpdateBy VARCHAR(50) NOT NULL COMMENT '更新人',
    UpdateDate BIGINT NOT NULL DEFAULT '0' COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT '需求标签表';

CREATE INDEX Ix_RequirementTag__RequirementId
ON RequirementTag (RequirementId);

CREATE INDEX Ix_RequirementTag__Tag
ON RequirementTag (Tag);

