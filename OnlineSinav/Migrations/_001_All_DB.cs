using FluentMigrator;

namespace OnlineSinav.Migrations
{
    [Migration(1)]
    public class _001_All_DB : Migration
    {
        public override void Down()
        {
            Delete.Table("dept_user");
            Delete.Table("Lessons");
            Delete.Table("exam_result");
            Delete.Table("exam_student");
            Delete.Table("exam_quest");
            Delete.Table("Questions");
            Delete.Table("Exam");
            Delete.Table("Department");
            Delete.Table("Users");
            Delete.Table("roles");
        }

        public override void Up()
        {
            Create.Table("roles")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("role_name").AsString(128);
            //----------------------------------------------------------------
            //----------------------------------------------------------------
            Create.Table("Users")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString(128)
                .WithColumn("school_number").AsString(10) // will be used for sign-in, teachers are also going to have a number of their own
                .WithColumn("rol_id").AsInt32().ForeignKey("roles", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("password").AsString(128);
            //----------------------------------------------------------------
            //----------------------------------------------------------------
            Create.Table("Department") //Department = 'd'
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("dept_name").AsString(128);
            //----------------------------------------------------------------
            //----------------------------------------------------------------
            Create.Table("Exam")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("exam_name").AsString(128)
                .WithColumn("exam_time_from").AsDate()
                .WithColumn("exam_time_to").AsDate()
                .WithColumn("exam_duration").AsString(5)
                .WithColumn("dept_id").AsInt32().ForeignKey("Department", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("teacher_id").AsInt32().ForeignKey("Users", "id").OnDelete(System.Data.Rule.Cascade);
            //----------------------------------------------------------------
            //----------------------------------------------------------------
            Create.Table("Questions")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("quest_name").AsString(128)
                .WithColumn("answer1").AsString(128)
                .WithColumn("answer2").AsString(128)
                .WithColumn("answer3").AsString(128)
                .WithColumn("answer4").AsString(128)
                .WithColumn("answer5").AsString(128)
                .WithColumn("true_answer").AsString(128)
                .WithColumn("dept_id").AsInt32();
            //----------------------------------------------------------------
            //----------------------------------------------------------------
            Create.Table("exam_quest")
                .WithColumn("exam_id").AsInt32().ForeignKey("Exam", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("quest_id").AsInt32().ForeignKey("Questions", "id").OnDelete(System.Data.Rule.Cascade);
            //----------------------------------------------------------------
            //----------------------------------------------------------------
            Create.Table("exam_student")
                .WithColumn("exam_id").AsInt32().ForeignKey("Exam", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("student_id").AsInt32().ForeignKey("Users", "id").OnDelete(System.Data.Rule.Cascade);
            //----------------------------------------------------------------
            //----------------------------------------------------------------
            Create.Table("exam_results")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("exam_id").AsInt32().ForeignKey("Exam", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("teacher_id").AsInt32().ForeignKey("Users", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("student_id").AsInt32().ForeignKey("Users", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("students_answers").AsString(256)
                .WithColumn("correct_answers").AsString(256)
                .WithColumn("result").AsInt32();
            //----------------------------------------------------------------
            //----------------------------------------------------------------
            Create.Table("Lessons")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("lesson_name").AsString(128)
                .WithColumn("dept_id").AsInt32().ForeignKey("Department", "id").OnDelete(System.Data.Rule.Cascade);
            //----------------------------------------------------------------
            //----------------------------------------------------------------
            Create.Table("dept_user")
                .WithColumn("dept_id").AsInt32().ForeignKey("Department", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("user_id").AsInt32().ForeignKey("Users", "id").OnDelete(System.Data.Rule.Cascade);
            /*
             cmd satırı:                                                                           
             migrate -a C:\Users\berka\source\repos\OnlineSinav\OnlineSinav\bin\OnlineSinav.dll -db MySql -conn "Data Source= 127.0.0.1; Database = onlineexam; uid=root; pwd=b2362123;"
             migrate -a C:\Users\Atabay\Documents\GitHub\NewOnlineExam\OnlineSinav\bin\OnlineSinav.dll -db MySql -conn "Data Source= 127.0.0.1; Database = onlineexam; uid=root; pwd=root;"
             */

            /*
             Alınan hata: !!! Client does not support authentication protocol requested by server; consider upgrading MySQL client
             Çözümü: 
             create user bbs@localhost identified by 'b2362123';
             grant all privileges on node.* to bbs@localhost;
             ALTER USER 'bbs'@localhost IDENTIFIED WITH mysql_native_password BY 'b2362123';

            root ile giriş yaparak: 
            1- onlineexam adında bir şema yaratalım
            2- GRANT ALL PRIVILEGES ON onlineexam.* TO 'bbs'@'localhost';

            Sonrasında kullanılacak CMD:
            migrate -a C:\Users\berka\source\repos\OnlineSinav\OnlineSinav\bin\OnlineSinav.dll -db MySql -conn "Data Source= 127.0.0.1; Database = onlineexam; uid=bbs; pwd=b2362123;"
             */

        }
    }
}