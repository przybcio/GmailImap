using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace GmailImap.DAL
{
    public class FluentNhConfiguration
    {
        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
              .Database(
              SQLiteConfiguration.Standard.InMemory
              
              )   
              .Mappings(m=>m.AutoMappings.Add(AutoMap.AssemblyOf<Transaction>(type=> type.Name.StartsWith("Tran"))).ExportTo("Test"))
              //.ExposeConfiguration(BuildSchema)
              .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            // delete the existing db on each run
            if (File.Exists("DatabaseFileName.sdf"))
                File.Delete("DatabaseFileName.sdf");

            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config)
              .Create(false, true);
        }
    }
}
