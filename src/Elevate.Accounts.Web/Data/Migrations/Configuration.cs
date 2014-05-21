using System.Data.Entity.Migrations;
using BrockAllen.MembershipReboot.Ef;
using MySql.Data.Entity;

namespace Elevate.Accounts.Web.Data.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<DefaultMembershipRebootDatabase>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

            var generator = new MySqlMigrationSqlGenerator();

            SetHistoryContextFactory("MySql.Data.MySqlClient", (conn, schema) => new MySqlHistoryContext(conn, schema));
            SetSqlGenerator("MySql.Data.MySqlClient", generator);
        }

        protected override void Seed(DefaultMembershipRebootDatabase context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
