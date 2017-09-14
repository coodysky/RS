CREATE TABLE RequirementStatus
(
    RequirementStatusId INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT '主键自增',
    RequirementStatusCode VARCHAR(20) NOT NULL COMMENT '需求状态Code【Init:已提交,ToBeApproved:待审核,Approving:审核中,Approved:已审核,Cancelled:已删除,Completed:已完成】',
    RequirementStatusNameCn VARCHAR(20) NOT NULL COMMENT '状态中文',
    RequirementStatusDesc VARCHAR(100) NOT NULL COMMENT '状态描述',
    CreateBy VARCHAR(50) NOT NULL COMMENT '创建人',
    CreateDate BIGINT NOT NULL DEFAULT '0' COMMENT '创建时间',
    UpdateBy VARCHAR(50) NOT NULL COMMENT '更新人',
    UpdateDate BIGINT NOT NULL DEFAULT '0' COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT '需求状态表';

CREATE INDEX Ix_RequirementStatus__RequirementStatusCode
ON RequirementStatus (RequirementStatusCode);
