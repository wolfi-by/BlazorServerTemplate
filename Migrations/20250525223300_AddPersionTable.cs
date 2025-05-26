
using FluentMigrator;

// <summary>
// Migration to create the People table with Id, Name, and Email columns.
// </summary>
// <remarks>
// https://www.nuget.org/packages/FluentMigrator.DotNet.Cli
// </remarks>

[Migration(20250525223300)]
public class CreatePersonTable : Migration
{



    public override void Up()
    {
        Create.Table("People")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Name").AsString(100).NotNullable()
            .WithColumn("Email").AsString(200).Nullable();
    }

    public override void Down()
    {
        Delete.Table("People");
    }
}

