using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSinav.Models
{
    public class ExamResult
    {
        /*
          Create.Table("exam_results")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("exam_id").AsInt32().ForeignKey("Exam", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("teacher_id").AsInt32().ForeignKey("Users", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("student_id").AsInt32().ForeignKey("Users", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("students_answers").AsString(256)
                .WithColumn("correct_answers").AsString(256)
                .WithColumn("result").AsInt32();
             */
        public virtual int id { get; set; }
        public virtual Exam exam { get; set; }
        public virtual Users teacher { get; set; }
        public virtual Users student { get; set; }
        public virtual string students_answers { get; set; }
        public virtual string correct_answers { get; set; }
        public virtual int result { get; set; }
    }

    public class ExamResultMap : ClassMapping<ExamResult>
    {
        public ExamResultMap()
        {
            Schema("onlineexam");
            Table("exam_results");
            Lazy(true);
            Id(x => x.id, map => { map.Column("id"); map.Generator(Generators.Identity); });

            ManyToOne(x => x.exam, map => { map.Column("exam_id"); map.Cascade(Cascade.None); });
            ManyToOne(x => x.teacher, map => { map.Column("teacher_id"); map.Cascade(Cascade.None); });
            ManyToOne(x => x.student, map => { map.Column("student_id"); map.Cascade(Cascade.None); });

            Property(x => x.students_answers, map => { map.Column("students_answers"); map.NotNullable(true); });
            Property(x => x.correct_answers, map => { map.Column("correct_answers"); map.NotNullable(true); });
            Property(x => x.result, map => map.NotNullable(true));

        }
    }
}