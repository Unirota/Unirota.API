using FluentMigrator;
using Unirota.Migrations.Configuration;

namespace Unirota.Migrations;

[Migration(202409192012)]
public class Migration202409192012 : Migration
{
    public override void Up()
    {
        Create.Table("Veiculos")
           .EntityBase()
           .WithColumn("Placa").AsString().NotNullable()
           .WithColumn("MotoristaId").AsInt32().NotNullable().ForeignKey("Usuarios", "Id")
           .WithColumn("Cor").AsString().NotNullable()
           .WithColumn("Carroceria").AsString().NotNullable()
           .WithColumn("Descricao").AsString().Nullable();
    }

    public override void Down()
    {
        Delete.Table("Veiculos");
    }
}
