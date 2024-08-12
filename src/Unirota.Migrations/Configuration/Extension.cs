using FluentMigrator.Builders.Create.Table;

namespace Unirota.Migrations.Configuration;

public static class Extensions
{
    public static ICreateTableColumnOptionOrWithColumnSyntax EntityBase(this ICreateTableWithColumnSyntax tableWithColumnSyntax)
    {
        return tableWithColumnSyntax
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("created_at").AsDateTime().NotNullable();
    }
}