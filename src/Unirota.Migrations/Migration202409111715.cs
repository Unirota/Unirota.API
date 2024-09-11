using FluentMigrator;
using Unirota.Migrations.Configuration;

namespace Unirota.Migrations;

[Migration(20240911171)]
public class Migration202409111715 : Migration
{
    public override void Up()
    {
        Create.Table("Grupos")
            .EntityBase()
            .WithColumn("Nome").AsString().NotNullable()
            .WithColumn("MotoristaId").AsInt32().NotNullable().ForeignKey("Usuarios", "Id")
            .WithColumn("PassageiroLimite").AsInt32().NotNullable()
            .WithColumn("ImagemUrl").AsString().Nullable()
            .WithColumn("HoraInicio").AsDateTime2().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Grupos");
    }
}
