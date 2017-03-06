using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoutuneWarServer.NHibernate
{
    public class SessionManager
    {
        static SessionManager instance;
        static ISessionFactory sessionFactory;

        public static SessionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    object sync = new object();
                    lock (sync)
                    {
                        instance = new SessionManager();
                    }
                }
                return instance;
            }
        }

        public SessionManager()
        {
        }

        public ISession session
        {
            get
            {
                if (sessionFactory == null)
                {
                    object sync = new object();
                    lock (sync)
                    {
                        Configuration cfg = new Configuration();
                        cfg.Configure("NHibernate/hibernate.cfg.xml");
                        sessionFactory = Fluently.Configure(cfg).Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>()).BuildSessionFactory();
                    }
                }
                return sessionFactory.OpenSession();
            }
        }
    }
}
