using FluentMigrator;

namespace Unirota.Migrations;

[Migration(202408261910)]
public class Migration202408261910 : Migration
{
    public override void Up()
    {
        Alter.Table("Usuarios")
            .AddColumn("ImagemUrl").AsString().Nullable()
            .AddColumn("DataNascimento").AsDateTime2().NotNullable();
    }

    public override void Down()
    {
        Delete.Column("ImagemUrl").FromTable("Usuarios");
    }
}
