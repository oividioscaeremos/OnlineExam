using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSinav.Models
{
    public class Roles
    {
        public Roles() { }
        public virtual int id { get; set; }
        public virtual string RoleName { get; set; }
        
    }

    public class RolesMap : ClassMapping<Roles>
    {

        public RolesMap()
        {
            Schema("onlineexam");
            Lazy(true);
            Id(x => x.id, map => { map.Column("id"); map.Generator(Generators.Identity); });
            Property(x => x.RoleName, map => { map.Column("role_name"); map.NotNullable(true); });


            
        }
    }
}