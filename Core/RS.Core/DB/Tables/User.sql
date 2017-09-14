CREATE TABLE User
(
    UserId INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT '主键自增',
    UserName VARCHAR(50) NOT NULL COMMENT '登录账号',
    RealName VARCHAR(50) NOT NULL COMMENT '真实姓名',
    Password VARCHAR(100) NOT NULL COMMENT '密码，加密',
    CreateBy VARCHAR(50) NOT NULL COMMENT '创建人',
    CreateDate BIGINT NOT NULL DEFAULT '0' COMMENT '创建时间',
    UpdateBy VARCHAR(50) NOT NULL COMMENT '更新人',
    UpdateDate BIGINT NOT NULL DEFAULT '0' COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT '操作人员表';

CREATE INDEX Ix_User__UserName
ON User (UserName);

CREATE INDEX Ix_User__RealName
ON User (RealName);

CREATE INDEX Ix_User__RealName__Password
ON User (RealName,Password);
