using FluentMigrator;
using Unirota.Migrations.Configuration;

namespace Unirota.Migrations;

[Migration(202410102021)]
public class Migration202410102021 :  Migration
{
    public override void Up()
    {
        Create.Table("Corridas")
            .EntityBase()
            .WithColumn("GrupoId").AsInt32().NotNullable().ForeignKey("Grupos", "Id")
            .WithColumn("Comeco").AsDateTime2().NotNullable()
            .WithColumn("Final").AsDateTime2().Nullable();

    }

    public override void Down()
    {
        Delete.Table("Corridas");
    }
}
