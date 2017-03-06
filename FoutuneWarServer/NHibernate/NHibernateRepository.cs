using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoutuneWarServer.NHibernate
{
    public class NHibernateRepository<T> : IRepository<T> where T : class
    {
        protected readonly ISession session = SessionManager.Instance.session;

        public T Get(object id)
        {
            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    T returnVal = session.Get<T>(id);
                    transaction.Commit();
                    return returnVal;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public void Save(T value)
        {
            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(value);
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Update(T value)
        {
            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(value);
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Delete(T value)
        {
            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Delete(value);
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public IList<T> GetAll()
        {
            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    IList<T> returnVal = session.CreateCriteria<T>().List<T>();
                    transaction.Commit();
                    return returnVal;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }

    public interface IRepository<T>
    {
        T Get(object id);
        void Save(T value);
        void Update(T value);
        void Delete(T value);
        IList<T> GetAll();
    }
}
