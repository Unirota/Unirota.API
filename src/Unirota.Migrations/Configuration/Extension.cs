using FluentMigrator.Builders.Create.Table;

namespace Unirota.Migrations.Configuration;

public static class Extensions
{
    public static ICreateTableColumnOptionOrWithColumnSyntax EntityBase(this ICreateTableWithColumnSyntax tableWithColumnSyntax)
    {
        return tableWithColumnSyntax
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("CreatedAt").AsDateTime2().NotNullable();
    }
}