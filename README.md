SqlSugar DbFirst 帮助控制台, 用于读取数据库定义，生成实体类文件。仅支持 mysql 和 sqlserver。官方文档:  https://www.donet5.com/Home/Doc?typeId=1207

支持按命名规范修改类名、文件名、属性名： SqlServer去掉 “_” 字符。 MySQL 使用 Pascal 命名法。

支持手动输入和参数解析两种模式。

1. 手动模式可直接按命令行提示依次输入数据库连接串、输出目录、命名空间。
2. 参数解析模式支持命令行参数传递，命令行样例：  
```
SugarConsole.exe "mysql" "Server=xx;Database=db;User=root;Password=...;" "D:\output" "Foo.Bar.Models"
```