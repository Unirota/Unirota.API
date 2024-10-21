using FluentMigrator;

namespace Unirota.Migrations;

[Migration(202410101934)]
public class Migration202410101934:Migration
{
    public override void Up()
    {
        Alter.Table("Grupos")
            .AddColumn("Destino")
            .AsString()
            .NotNullable()
            .WithDefaultValue("");
    }

    public override void Down()
    {
        Delete.Column("Destino").FromTable("Grupos");
    }
}