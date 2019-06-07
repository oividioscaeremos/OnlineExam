using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSinav.Models
{
    public class Questions
    {
        public Questions() { }
        public virtual int id { get; set; }
        public virtual string QuestName { get; set; }
        public virtual string Answer1 { get; set; }
        public virtual string Answer2 { get; set; }
        public virtual string Answer3 { get; set; }
        public virtual string Answer4 { get; set; }
        public virtual string Answer5 { get; set; }
        public virtual string TrueAnswer { get; set; }
        public virtual Department dept_id { get; set; }

    }

    public class QuestionsMap : ClassMapping<Questions>
    {

        public QuestionsMap()
        {
            Schema("onlineexam");
            Lazy(true);
            Id(x => x.id, map => { map.Column("id"); map.Generator(Generators.Identity); });

            Property(x => x.QuestName, map => { map.Column("quest_name"); map.NotNullable(true); });

            Property(x => x.Answer1, map => map.NotNullable(true));
            Property(x => x.Answer2, map => map.NotNullable(true));
            Property(x => x.Answer3, map => map.NotNullable(true));
            Property(x => x.Answer4, map => map.NotNullable(true));
            Property(x => x.Answer5, map => map.NotNullable(true));

            Property(x => x.TrueAnswer, map => { map.Column("true_answer"); map.NotNullable(true); });
            ManyToOne(x => x.dept_id, map => { map.Column("dept_id"); map.Cascade(Cascade.Remove); });
        }
    }
}