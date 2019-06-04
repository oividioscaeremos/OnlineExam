using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using OnlineSinav.Models;
using System.Web;

namespace OnlineSinav
{
    public class Database
    {
        private const string SessionKey = "OnlineSinav.Database.SessionKey";
        private static ISessionFactory _sessionFactory;

        public static ISession Session
        {
            get
            {
                return (ISession)HttpContext.Current.Items[SessionKey];
            }
        }

        public static void Configure()
        {
            var config = new Configuration();

            var mapper = new ModelMapper();
            mapper.AddMapping<UsersMap>();
            mapper.AddMapping<RolesMap>();
            mapper.AddMapping<QuestionsMap>();
            mapper.AddMapping<LessonsMap>();
            mapper.AddMapping<ExamMap>();
            mapper.AddMapping<DepartmentMap>();

            config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            _sessionFactory = config.BuildSessionFactory();

        }

        public static void OpenSession()
        {
            HttpContext.Current.Items[SessionKey] = _sessionFactory.OpenSession();
        }

        public static void CloseSession()
        {

            var session = HttpContext.Current.Items[SessionKey] as ISession;
            if (session != null)
            {
                session.Close();
            }

            HttpContext.Current.Items.Remove("Session");

        }

    }
}