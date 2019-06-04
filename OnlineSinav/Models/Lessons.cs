using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSinav.Models
{
    public class Lessons
    {
        public virtual int id { get; set; }
        public virtual Department Department { get; set; }
        public virtual string LessonName { get; set; }
    }

    public class LessonsMap : ClassMapping<Lessons>
    {

        public LessonsMap()
        {
            Schema("onlineexam");
            Lazy(true);
            Id(x => x.id, map => { map.Column("id"); map.Generator(Generators.Identity); });
            Property(x => x.LessonName, map => { map.Column("lesson_name"); map.NotNullable(true); });
            ManyToOne(x => x.Department, map => { map.Column("dept_id"); map.Cascade(Cascade.Remove); });
        }
    }
}