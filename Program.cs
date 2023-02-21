using System.Text;
using SqlSugar;
public class Program
{
    /// <summary>
    /// SulSugar DbFirst
    /// https://www.donet5.com/home/doc?masterId=1&typeId=1207
    /// </summary>
    /// <param name="args">数据库连接串 输出目录 命名空间</param>
    private static void Main(string[] args)
    {
        // 数据库类型
        string dbType = string.Empty;
        // 数据库连接串
        string connectionString = string.Empty;
        // 输出目录
        string outputDir = string.Empty;
        // 命名空间
        string @namespace = string.Empty;


        if (args.Length == 0)
        {
            bool confirmd = false;
            while (!confirmd)
            {
                Console.WriteLine("进入手动输入模式");
                PrintSplit();
                Console.WriteLine("请输入数据库类型(mysql/sqlserver)");
                dbType = Console.ReadLine() ?? "";
                Console.WriteLine("请输入数据库连接串");
                connectionString = Console.ReadLine() ?? "";
                Console.WriteLine("请输入输出目录");
                outputDir = Console.ReadLine() ?? "";
                Console.WriteLine("请输入命名空间");
                @namespace = Console.ReadLine() ?? "";
                PrintConfirm(dbType, connectionString, outputDir, @namespace);
                confirmd = ReadConfirm();
            }
        }
        else if(args.Length == 4)
        {
            Console.WriteLine("进入参数解析模式");
            PrintSplit();

            int i = 0;
            dbType = args[i++];
            connectionString = args[i++];
            outputDir = args[i++];
            @namespace = args[i++];

            PrintConfirm(dbType, connectionString, outputDir, @namespace);
            if (!ReadConfirm())
                return;
        }
        else
        {
            PrintHelp();
        }

        if(dbType != "mysql" && dbType != "sqlserver")
            Console.WriteLine("数据库类型不支持");

        DbType sugarDbType = DbType.MySql;

        switch(dbType)
        {
            case "mysql":
                sugarDbType =DbType.MySql; break;
            case "sqlserver":
                sugarDbType =DbType.SqlServer; break;
            default: 
                Console.WriteLine("数据库类型不支持");
                return;
        }

        var db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = connectionString,
            DbType = sugarDbType,
            IsAutoCloseConnection = true,
        });

        foreach (var item in db.DbMaintenance.GetTableInfoList())
        {
            string entityName = FormatName(item.Name, sugarDbType);
            db.MappingTables.Add(entityName, item.Name);
            foreach (var col in db.DbMaintenance.GetColumnInfosByTableName(item.Name))
            {
                db.MappingColumns.Add(FormatName(col.DbColumnName, sugarDbType), col.DbColumnName, entityName);
            }
        }
        db.Aop.OnLogExecuting = (sql, pars) =>
        {
            // 输出sql,查看执行sql 性能无影响
            Console.WriteLine(sql);
        };
        db.DbFirst.IsCreateAttribute().CreateClassFile(outputDir, @namespace);
    }

    public static string FormatName(string str, DbType dbType)
    {
        if(dbType == DbType.SqlServer)
            return str;
        return ToPascal(str);

    }
    public static string ToPascal(string str)
    {
        try
        {
            string[] split = str.Split('_');
            if (split.Length >= 1)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in split)
                {
                    char[] chars = item.ToCharArray();
                    chars[0] = char.ToUpper(chars[0]);
                    for (int i = 1; i < chars.Length; i++)
                    {
                        chars[i] = char.ToLower(chars[i]);
                    }
                    sb.Append(chars);
                    
                }
                return sb.ToString();
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"ToPascal发生异常 {ex.Message}");
        }
        return str;
    }

    public static void PrintSplit()
    {
        Console.WriteLine("----");
    }

    public static void PrintHelp()
    {
        Console.WriteLine("参数解析模式的参数样例:");
        Console.WriteLine("\"Server=...;Database=...;User=...;Password=...;SSL Mode=None;\" \"D:/dir\" \"GroupOrder.Model.Models\"");
    }

    public static void PrintConfirm(string dbType, string connectionString, string outputDir, string @namespace)
    {
        Console.WriteLine($"输入内容确认:\n数据库类型:{dbType}\n数据库连接串:{connectionString}\n输出目录:{outputDir}\n命名空间:{@namespace}\n");
    }

    public static bool ReadConfirm()
    {
        Console.WriteLine("是否确认? (y/n)");
        var confirm = Console.ReadLine();
        bool confirmd = (confirm == "y");
        return confirmd;
    }
}