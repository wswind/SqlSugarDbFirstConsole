SqlSugar DbFirst 帮助控制台, 用于通过MySQL数据库连接生成实体类文件。  
基于官方文档:  
https://www.donet5.com/home/doc?masterId=1&typeId=1207  

支持手动输入和参数解析两种模式。

1. 手动模式可直接按命令行提示依次输入数据库连接串、输出目录、命名空间。
2. 参数解析模式支持命令行参数传递，命令行样例：  
```
SugarConsole.exe "mysql" "Server=xx;Database=db;User=root;Password=...;SSL Mode=None;" "D:\output" "Foo.Bar.Models"
```
----

A helper console for SqlSugar DbFirst, based on SqlSugar official doc: 
https://www.donet5.com/home/doc?masterId=1&typeId=1207