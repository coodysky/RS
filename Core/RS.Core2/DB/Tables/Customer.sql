CREATE TABLE Customer
(
    CustomerId INT NOT NULL AUTO_INCREMENT PRIMARY KEY COMMENT '主键自增',
    NickName VARCHAR(50) NOT NULL COMMENT '昵称',
    RealName VARCHAR(50) NOT NULL COMMENT '真实姓名',
    Password VARCHAR(100) NOT NULL COMMENT '密码，加密',
    MobilePhone VARCHAR(20) NOT NULL COMMENT '注册的手机号，唯一',
    Email VARCHAR(100) NOT NULL COMMENT '电子邮箱',
    CreateBy VARCHAR(50) NOT NULL COMMENT '创建人',
    CreateDate BIGINT NOT NULL DEFAULT '0' COMMENT '创建时间',
    UpdateBy VARCHAR(50) NOT NULL COMMENT '更新人',
    UpdateDate BIGINT NOT NULL DEFAULT '0' COMMENT '更新时间'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT '用户表';

CREATE INDEX Ix_Customer__NickName
ON Customer (NickName);

CREATE INDEX Ix_Customer__MobilePhone
ON Customer (MobilePhone);

CREATE INDEX Ix_Customer__MobilePhone__Password
ON Customer (MobilePhone,Password);