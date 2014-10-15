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
              .Database(SQLiteConfiguration.Standard.UsingFile("testDb") )
              .Mappings(m=>m.AutoMappings.Add(AutoMap.AssemblyOf<Transaction>().Where(t => t.Name == typeof(Transaction).Name)))
              .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))
              .BuildSessionFactory();
        }        
    }
}
