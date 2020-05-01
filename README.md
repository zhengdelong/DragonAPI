本项目使用了asp.net core 3.1,数据库 mysql 8.0
所以所需环境  .net core sdk  https://dotnet.microsoft.com/download
orm使用了 dapper+Kogel.Dapper.Extension.MySql(将linq解析成sql)，暂时只做了批量插入()，和基本得增删改查
