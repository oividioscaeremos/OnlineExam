using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System.Collections.Generic;

namespace OnlineSinav.Models

{
    public class Users
    {
        public virtual int id { get; set; }
        public virtual Roles Role { get; set; }
        public virtual IList<Department> depts { get; set; }
        public virtual string Name { get; set; }
        public virtual string SchoolNumber { get; set; }
        public virtual string Password { get; set; }

        public Users() {
            depts = new List<Department>();
        }

        public virtual void SetPassword(string password)
        {
            Password = BCrypt.Net.BCrypt.HashPassword(password, 13);
        }

        public virtual bool CheckPassword(string pwd)
        {
            var pass = BCrypt.Net.BCrypt.Verify(pwd, Password);
            return BCrypt.Net.BCrypt.Verify(pwd, Password);
        }
    }

    public class UsersMap : ClassMapping<Users>
    {
        public UsersMap()
        {
            Schema("onlineexam");
            Id(x => x.id, map => map.Generator(Generators.Identity));
            Property(x => x.Name, map => map.NotNullable(true));
            Property(x => x.SchoolNumber, map => { map.Column("school_number"); map.NotNullable(true); });
            Property(x => x.Password, map => map.NotNullable(true));

            ManyToOne(x => x.Role, x => {
                x.Column("rol_id");
                x.NotNullable(true);
            });

            Bag(x => x.depts, x => {
                x.Table("dept_user");
                x.Key(k => k.Column("user_id"));
            }, x => x.ManyToMany(k => k.Column("dept_id")));

        }
    }
}