using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace GmailImap.DAL
{
    public class FluentNhConfiguration : IWindsorInstaller
    {
        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
              .Database(SQLiteConfiguration.Standard.UsingFile("testDb") )
              .Mappings(m=>m.AutoMappings.Add(AutoMap.AssemblyOf<Transaction>().Where(t => t.Name == typeof(Transaction).Name)))
              .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))
              .BuildSessionFactory();
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(

                Component.For<ISessionFactory>().UsingFactoryMethod(CreateSessionFactory),
                Component.For<ITransactionRepository>().ImplementedBy<NhTransactionRepository>()
                );
        }
    }
}
