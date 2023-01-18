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
                Console.WriteLine("请输入数据库连接串");
                connectionString = Console.ReadLine() ?? "";
                Console.WriteLine("请输入输出目录");
                outputDir = Console.ReadLine() ?? "";
                Console.WriteLine("请输入命名空间");
                @namespace = Console.ReadLine() ?? "";
                PrintConfirm(connectionString, outputDir, @namespace);
                confirmd = ReadConfirm();
            }
        }
        else if(args.Length == 3)
        {
            Console.WriteLine("进入参数解析模式");
            PrintSplit();

            connectionString = args[0];
            outputDir = args[1];
            @namespace = args[2];

            PrintConfirm(connectionString, outputDir, @namespace);
            if (!ReadConfirm())
                return;
        }
        else
        {
            PrintHelp();
        }

        var db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = connectionString,
            DbType = DbType.MySql,
            IsAutoCloseConnection = true,
        });

        foreach (var item in db.DbMaintenance.GetTableInfoList())
        {
            string entityName = item.Name.ToUpper(); /*实体名大写*/
            db.MappingTables.Add(entityName, item.Name);
            foreach (var col in db.DbMaintenance.GetColumnInfosByTableName(item.Name))
            {
                db.MappingColumns.Add(col.DbColumnName.ToUpper() /*类的属性大写*/, col.DbColumnName, entityName);
            }
        }
        db.Aop.OnLogExecuting = (sql, pars) =>
        {
            // 输出sql,查看执行sql 性能无影响
            Console.WriteLine(sql);
        };
        db.DbFirst.IsCreateAttribute().CreateClassFile(outputDir, @namespace);
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

    public static void PrintConfirm(string connectionString, string outputDir, string @namespace)
    {
        Console.WriteLine($"输入内容确认:\n数据库连接串:{connectionString}\n输出目录:{outputDir}\n命名空间:{@namespace}\n");
    }

    public static bool ReadConfirm()
    {
        Console.WriteLine("是否确认? (y/n)");
        var confirm = Console.ReadLine();
        bool confirmd = (confirm == "y");
        return confirmd;
    }
}