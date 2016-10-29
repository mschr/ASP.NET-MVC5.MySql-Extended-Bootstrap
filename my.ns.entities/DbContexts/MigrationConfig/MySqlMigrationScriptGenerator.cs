using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my.ns.entities.DbContexts.MigrationConfig
{
    public class MySqlMigrationScriptGenerator : MySqlMigrationSqlGenerator
    {
        public override IEnumerable<MigrationStatement> Generate(IEnumerable<MigrationOperation> migrationOperations, string providerManifestToken)
        {
            IEnumerable<MigrationStatement> statements = base.Generate(migrationOperations, providerManifestToken);
            foreach (MigrationStatement statement in statements) statement.Sql = StripSchema(statement.Sql).TrimEnd() + ";";
            return statements;
        }
        private string StripSchema(string name)
        {
            return System.Text.RegularExpressions.Regex.Replace(name, @"dbo\.", "",
                System.Text.RegularExpressions.RegexOptions.Compiled);
        }
    }
}
