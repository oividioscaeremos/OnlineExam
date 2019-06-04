using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace OnlineSinav.Models
{
    public class Department
    {
        public Department() { }
        public virtual int id { get; set; }
        public virtual string DeptName { get; set; }
    }

    public class DepartmentMap : ClassMapping<Department>
    {

        public DepartmentMap()
        {
            Schema("onlineexam");
            Id(x => x.id, map => { map.Column("id"); map.Generator(Generators.Assigned); });
            Property(x => x.DeptName, map => { map.Column("dept_name"); map.NotNullable(true); });
        }
    }
}