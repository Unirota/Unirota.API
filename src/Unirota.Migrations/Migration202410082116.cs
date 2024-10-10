using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unirota.Migrations.Configuration;

namespace Unirota.Migrations;

[Migration(202410082116)]
public class Migration202410082116 :  Migration
{
    public override void Up()
    {
        Create.Table("Corridas")
            .EntityBase()
            .WithColumn("GrupoId").AsInt32().NotNullable().ForeignKey("Grupos", "Id")
            .WithColumn("Comeco").AsDateTime2().NotNullable()
            .WithColumn("Final").AsDateTime2().NotNullable();

    }

    public override void Down()
    {
        Delete.Table("Corridas");
    }
}
