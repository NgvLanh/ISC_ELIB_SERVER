using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ISC_ELIB_SERVER.Models
{
    public partial class isc_elibContext : DbContext
    {
        public isc_elibContext()
        {
        }

        public isc_elibContext(DbContextOptions<isc_elibContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AcademicYear> AcademicYears { get; set; } = null!;
        public virtual DbSet<Achievement> Achievements { get; set; } = null!;
        public virtual DbSet<AnswerImagesQa> AnswerImagesQas { get; set; } = null!;
        public virtual DbSet<AnswersQa> AnswersQas { get; set; } = null!;
        public virtual DbSet<Campus> Campuses { get; set; } = null!;
        public virtual DbSet<ChangeClass> ChangeClasses { get; set; } = null!;
        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<ClassType> ClassTypes { get; set; } = null!;
        public virtual DbSet<Discussion> Discussions { get; set; } = null!;
        public virtual DbSet<DiscussionImage> DiscussionImages { get; set; } = null!;
        public virtual DbSet<EducationLevel> EducationLevels { get; set; } = null!;
        public virtual DbSet<EntryType> EntryTypes { get; set; } = null!;
        public virtual DbSet<Exam> Exams { get; set; } = null!;
        public virtual DbSet<ExamGrader> ExamGraders { get; set; } = null!;
        public virtual DbSet<ExamSchedule> ExamSchedules { get; set; } = null!;
        public virtual DbSet<ExamScheduleClass> ExamScheduleClasses { get; set; } = null!;
        public virtual DbSet<Exemption> Exemptions { get; set; } = null!;
        public virtual DbSet<GradeLevel> GradeLevels { get; set; } = null!;
        public virtual DbSet<Major> Majors { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Permission> Permissions { get; set; } = null!;
        public virtual DbSet<QuestionImagesQa> QuestionImagesQas { get; set; } = null!;
        public virtual DbSet<QuestionQa> QuestionQas { get; set; } = null!;
        public virtual DbSet<Reserve> Reserves { get; set; } = null!;
        public virtual DbSet<Resignation> Resignations { get; set; } = null!;
        public virtual DbSet<Retirement> Retirements { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public virtual DbSet<School> Schools { get; set; } = null!;
        public virtual DbSet<ScoreType> ScoreTypes { get; set; } = null!;
        public virtual DbSet<Semester> Semesters { get; set; } = null!;
        public virtual DbSet<Session> Sessions { get; set; } = null!;
        public virtual DbSet<StudentInfo> StudentInfos { get; set; } = null!;
        public virtual DbSet<StudentScore> StudentScores { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<SubjectGroup> SubjectGroups { get; set; } = null!;
        public virtual DbSet<SubjectType> SubjectTypes { get; set; } = null!;
        public virtual DbSet<Support> Supports { get; set; } = null!;
        public virtual DbSet<SystemSetting> SystemSettings { get; set; } = null!;
        public virtual DbSet<TeacherFamily> TeacherFamilies { get; set; } = null!;
        public virtual DbSet<TeacherInfo> TeacherInfos { get; set; } = null!;
        public virtual DbSet<TeacherTrainingProgram> TeacherTrainingPrograms { get; set; } = null!;
        public virtual DbSet<TeachingAssignment> TeachingAssignments { get; set; } = null!;
        public virtual DbSet<TemporaryLeave> TemporaryLeaves { get; set; } = null!;
        public virtual DbSet<Test> Tests { get; set; } = null!;
        public virtual DbSet<TestAnswer> TestAnswers { get; set; } = null!;
        public virtual DbSet<TestFile> TestFiles { get; set; } = null!;
        public virtual DbSet<TestQuestion> TestQuestions { get; set; } = null!;
        public virtual DbSet<TestSubmissionsAnswer> TestSubmissionsAnswers { get; set; } = null!;
        public virtual DbSet<TestsAttachment> TestsAttachments { get; set; } = null!;
        public virtual DbSet<TestsSubmission> TestsSubmissions { get; set; } = null!;
        public virtual DbSet<Theme> Themes { get; set; } = null!;
        public virtual DbSet<Topic> Topics { get; set; } = null!;
        public virtual DbSet<TopicsFile> TopicsFiles { get; set; } = null!;
        public virtual DbSet<TrainingProgram> TrainingPrograms { get; set; } = null!;
        public virtual DbSet<TransferSchool> TransferSchools { get; set; } = null!;
        public virtual DbSet<Type> Types { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserStatus> UserStatuses { get; set; } = null!;
        public virtual DbSet<WorkProcess> WorkProcesses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
                if (!string.IsNullOrEmpty(connectionString))
                {
                    optionsBuilder.UseNpgsql(connectionString);
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcademicYear>(entity =>
            {
                entity.ToTable("academic_years");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EndTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("end_time");

                entity.Property(e => e.SchoolId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("school_id");


                entity.Property(e => e.Active)
                    .HasDefaultValue(true)
                    .HasColumnName("active");

                entity.Property(e => e.StartTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("start_time");

                entity.HasOne(d => d.School)
                    .WithMany(p => p.AcademicYears)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_academic_years_school_id");
            });

            modelBuilder.Entity<Achievement>(entity =>
            {
                entity.ToTable("achievement");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .HasColumnName("content");

                entity.Property(e => e.DateAwarded)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date_awarded");

                entity.Property(e => e.File)
                    .HasMaxLength(100)
                    .HasColumnName("file");

                entity.Property(e => e.TypeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("type_id");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Achievements)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_achievement_type_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Achievements)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_achievement_user_id");
            });

            modelBuilder.Entity<AnswerImagesQa>(entity =>
            {
                entity.ToTable("answer_images_qa");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AnswerId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("answer_id");

                entity.Property(e => e.ImageUrl).HasColumnName("image_url");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.AnswerImagesQas)
                    .HasForeignKey(d => d.AnswerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_answer_images_qa_answer_id");
            });

            modelBuilder.Entity<AnswersQa>(entity =>
            {
                entity.ToTable("answers_qa");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("create_at");

                entity.Property(e => e.QuestionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("question_id");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.AnswersQas)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_answers_qa_question_id");
            });

            modelBuilder.Entity<Campus>(entity =>
            {
                entity.ToTable("campuses");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasColumnName("address");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(13)
                    .HasColumnName("phone_number");

                entity.Property(e => e.SchoolId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("school_id");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");


                entity.Property(e => e.Active)
                    .HasDefaultValue(true)
                    .HasColumnName("active");

                entity.HasOne(d => d.School)
                    .WithMany(p => p.Campuses)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_campuses_school_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Campuses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_campuses_user_id");
            });

            modelBuilder.Entity<ChangeClass>(entity =>
            {
                entity.ToTable("change_class");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AttachmentName)
                    .HasMaxLength(255)
                    .HasColumnName("attachment_name");

                entity.Property(e => e.AttachmentPath)
                    .HasMaxLength(255)
                    .HasColumnName("attachment_path");

                entity.Property(e => e.ChangeClassDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("change_class_date");

                entity.Property(e => e.LeadershipId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("leadership_id");

                entity.Property(e => e.NewClassId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("new_class_id");

                entity.Property(e => e.OldClassId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("old_class_id");

                entity.Property(e => e.Reason).HasColumnName("reason");

                entity.Property(e => e.StudentId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("student_id");

                entity.HasOne(d => d.NewClass)
                    .WithMany(p => p.ChangeClassNewClasses)
                    .HasForeignKey(d => d.NewClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_change_class_new_class_id");

                entity.HasOne(d => d.OldClass)
                    .WithMany(p => p.ChangeClassOldClasses)
                    .HasForeignKey(d => d.OldClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_change_class_old_class_id");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.ChangeClasses)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_change_class_student_id");
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.ToTable("chats");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .HasColumnName("content");

                entity.Property(e => e.SentAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("sent_at");

                entity.Property(e => e.SessionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("session_id");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Chats)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_chats_session_id");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("classes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AcademicYearId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("academic_year_id");

                entity.Property(e => e.ClassTypeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("class_type_id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .HasColumnName("code");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.GradeLevelId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("grade_level_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.StudentQuantity).HasColumnName("student_quantity");

                entity.Property(e => e.SubjectQuantity).HasColumnName("subject_quantity");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.AcademicYear)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.AcademicYearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_classes_academic_year_id");

                entity.HasOne(d => d.ClassType)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.ClassTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_classes_class_type_id");

                entity.HasOne(d => d.GradeLevel)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.GradeLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_classes_grade_level_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_classes_user_id");
            });

            modelBuilder.Entity<ClassType>(entity =>
            {
                entity.ToTable("class_types");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Discussion>(entity =>
            {
                entity.ToTable("discussions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("create_at");

                entity.Property(e => e.TopicId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("topic_id");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.Discussions)
                    .HasForeignKey(d => d.TopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_discussions_topic_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Discussions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_discussions_user_id");
            });

            modelBuilder.Entity<DiscussionImage>(entity =>
            {
                entity.ToTable("discussion_images");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DiscussionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("discussion_id");

                entity.Property(e => e.ImageUrl).HasColumnName("image_url");

                entity.HasOne(d => d.Discussion)
                    .WithMany(p => p.DiscussionImages)
                    .HasForeignKey(d => d.DiscussionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_discussion_images_discussion_id");
            });

            modelBuilder.Entity<EducationLevel>(entity =>
            {
                entity.ToTable("education_levels");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<EntryType>(entity =>
            {
                entity.ToTable("entry_types");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.ToTable("exams");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AcademicYearId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("academic_year_id");

                entity.Property(e => e.ClassTypeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("class_type_id");

                entity.Property(e => e.DurationMinutes).HasColumnName("duration_minutes");

                entity.Property(e => e.ExamDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("exam_date");

                entity.Property(e => e.File)
                    .HasMaxLength(255)
                    .HasColumnName("file");

                entity.Property(e => e.GradeLevelId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("grade_level_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.SemesterId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("semester_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.SubjectId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("subject_id");

                entity.HasOne(d => d.AcademicYear)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.AcademicYearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_exams_academic_year_id");

                entity.HasOne(d => d.ClassType)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.ClassTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_exams_class_type_id");

                entity.HasOne(d => d.GradeLevel)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.GradeLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_exams_grade_level_id");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_exams_semester_id");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_exams_subject_id");
            });

            modelBuilder.Entity<ExamGrader>(entity =>
            {
                entity.ToTable("exam_graders");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClassIds)
                    .HasMaxLength(255)
                    .HasColumnName("class_ids");

                entity.Property(e => e.ExamId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("exam_id");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.ExamGraders)
                    .HasForeignKey(d => d.ExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_exam_graders_exam_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ExamGraders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_exam_graders_user_id");
            });

            modelBuilder.Entity<ExamSchedule>(entity =>
            {
                entity.ToTable("exam_schedule");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AcademicYearId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("academic_year_id");

                entity.Property(e => e.ExamDay)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("exam_day");

                entity.Property(e => e.Form).HasColumnName("form");

                entity.Property(e => e.GradeLevelsId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("grade_levels_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.SemesterId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("semester_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.Subject)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("subject");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .HasColumnName("type");

                entity.HasOne(d => d.AcademicYear)
                    .WithMany(p => p.ExamSchedules)
                    .HasForeignKey(d => d.AcademicYearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_exam_schedule_academic_year_id");

                entity.HasOne(d => d.GradeLevels)
                    .WithMany(p => p.ExamSchedules)
                    .HasForeignKey(d => d.GradeLevelsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_exam_schedule_grade_levels_id");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.ExamSchedules)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_exam_schedule_semester_id");

                entity.HasOne(d => d.SubjectNavigation)
                    .WithMany(p => p.ExamSchedules)
                    .HasForeignKey(d => d.Subject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_exam_schedule_subject");
            });

            modelBuilder.Entity<ExamScheduleClass>(entity =>
            {
                entity.ToTable("exam_schedule_class");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClassId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("class_id");

                entity.Property(e => e.ExampleSchedule)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("example_schedule");

                entity.Property(e => e.SupervisoryTeacherId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("supervisory_teacher_id");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ExamScheduleClasses)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_exam_schedule_class_class_id");

                entity.HasOne(d => d.ExampleScheduleNavigation)
                    .WithMany(p => p.ExamScheduleClasses)
                    .HasForeignKey(d => d.ExampleSchedule)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_exam_schedule_class_example_schedule");

                entity.HasOne(d => d.SupervisoryTeacher)
                    .WithMany(p => p.ExamScheduleClasses)
                    .HasForeignKey(d => d.SupervisoryTeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_exam_schedule_class_supervisory_teacher_id");
            });

            modelBuilder.Entity<Exemption>(entity =>
            {
                entity.ToTable("exemption");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClassId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("class_id");

                entity.Property(e => e.ExemptedObjects)
                    .HasMaxLength(50)
                    .HasColumnName("exempted_objects");

                entity.Property(e => e.FormExemption)
                    .HasMaxLength(50)
                    .HasColumnName("form_exemption");

                entity.Property(e => e.StudentId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("student_id");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Exemptions)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_exemption_class_id");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Exemptions)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_exemption_student_id");
            });

            modelBuilder.Entity<GradeLevel>(entity =>
            {
                entity.ToTable("grade_levels");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .HasColumnName("code");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.TeacherId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("teacher_id");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.GradeLevels)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_grade_levels_teacher_id");
            });

            modelBuilder.Entity<Major>(entity =>
            {
                entity.ToTable("major");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("notifications");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .HasColumnName("content");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("create_at");

                entity.Property(e => e.SenderId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sender_id");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.NotificationSenders)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_notifications_sender_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.NotificationUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_notifications_user_id");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("permissions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<QuestionImagesQa>(entity =>
            {
                entity.ToTable("question_images_qa");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ImageUrl).HasColumnName("image_url");

                entity.Property(e => e.QuestionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("question_id");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionImagesQas)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_question_images_qa_question_id");
            });

            modelBuilder.Entity<QuestionQa>(entity =>
            {
                entity.ToTable("question_qa");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("create_at");

                entity.Property(e => e.SubjectId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("subject_id");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.QuestionQas)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_question_qa_subject_id");
            });

            modelBuilder.Entity<Reserve>(entity =>
            {
                entity.ToTable("reserve");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClassId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("class_id");

                entity.Property(e => e.File).HasColumnName("file");

                entity.Property(e => e.LeadershipId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("leadership_id");

                entity.Property(e => e.Reason).HasColumnName("reason");

                entity.Property(e => e.ReserveDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("reserve_date");

                entity.Property(e => e.RetentionPeriod)
                    .HasMaxLength(50)
                    .HasColumnName("retention_period");

                entity.Property(e => e.Semester)
                    .HasMaxLength(50)
                    .HasColumnName("semester");

                entity.Property(e => e.SemestersId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("semesters_id");

                entity.Property(e => e.StudentId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("student_id");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Reserves)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_reserve_student_id");
            });

            modelBuilder.Entity<Resignation>(entity =>
            {
                entity.ToTable("resignation");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Attachment)
                    .HasMaxLength(255)
                    .HasColumnName("attachment");

                entity.Property(e => e.Date)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date");

                entity.Property(e => e.LeadershipId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("leadership_id");

                entity.Property(e => e.Note).HasColumnName("note");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TeacherId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("teacher_id");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Resignations)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_resignation_teacher_id");
            });

            modelBuilder.Entity<Retirement>(entity =>
            {
                entity.ToTable("retirement");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Attachment)
                    .HasMaxLength(255)
                    .HasColumnName("attachment");

                entity.Property(e => e.Date)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date");

                entity.Property(e => e.LeadershipId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("leadership_id");

                entity.Property(e => e.Note).HasColumnName("note");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TeacherId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("teacher_id");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("role_permission");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PermissionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("permission_id");

                entity.Property(e => e.RoleId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("role_id");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_role_permission_permission_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_role_permission_role_id");
            });

            modelBuilder.Entity<School>(entity =>
            {
                entity.ToTable("schools");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .HasColumnName("code");

                entity.Property(e => e.DistrictId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("district_id");

                entity.Property(e => e.EducationLevelId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("education_level_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.EstablishedDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("established_date");

                entity.Property(e => e.Fax)
                    .HasMaxLength(13)
                    .HasColumnName("fax");

                entity.Property(e => e.HeadOffice).HasColumnName("head_office");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(13)
                    .HasColumnName("phone_number");

                entity.Property(e => e.ProvinceId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("province_id");

                entity.Property(e => e.SchoolType)
                    .HasMaxLength(50)
                    .HasColumnName("school_type");

                entity.Property(e => e.TrainingModel)
                    .HasMaxLength(50)
                    .HasColumnName("training_model");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.Property(e => e.WardId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ward_id");

                entity.Property(e => e.WebsiteUrl)
                    .HasMaxLength(100)
                    .HasColumnName("website_url");

                entity.Property(e => e.Active)
                    .HasDefaultValue(true)
                    .HasColumnName("active");

                entity.HasOne(d => d.EducationLevel)
                    .WithMany(p => p.Schools)
                    .HasForeignKey(d => d.EducationLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_schools_education_level_id");
            });

            modelBuilder.Entity<ScoreType>(entity =>
            {
                entity.ToTable("score_types");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.QtyScoreSemester1).HasColumnName("qty_score_semester_1");

                entity.Property(e => e.QtyScoreSemester2).HasColumnName("qty_score_semester_2");

                entity.Property(e => e.Weight).HasColumnName("weight");
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.ToTable("semesters");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AcademicYearId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("academic_year_id");

                entity.Property(e => e.EndTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("end_time");

                entity.Property(e => e.StartTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("start_time");

                entity.HasOne(d => d.AcademicYear)
                    .WithMany(p => p.Semesters)
                    .HasForeignKey(d => d.AcademicYearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_semesters_academic_year_id");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("sessions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AutoOpen).HasColumnName("auto_open");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.DurationTime).HasColumnName("duration_time");

                entity.Property(e => e.EndDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("end_date");

                entity.Property(e => e.ExamId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("exam_id");

                entity.Property(e => e.IsExam).HasColumnName("is_exam");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .HasColumnName("password");

                entity.Property(e => e.ShareCodeUrl)
                    .HasMaxLength(100)
                    .HasColumnName("share_code_url");

                entity.Property(e => e.StartDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("start_date");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.TeachingAssignmentId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("teaching_assignment_id");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.ExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sessions_exam_id");

                entity.HasOne(d => d.TeachingAssignment)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.TeachingAssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_sessions_teaching_assignment_id");
            });

            modelBuilder.Entity<StudentInfo>(entity =>
            {
                entity.ToTable("student_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GuardianAddress)
                    .HasMaxLength(100)
                    .HasColumnName("guardian_address");

                entity.Property(e => e.GuardianDob)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("guardian_dob");

                entity.Property(e => e.GuardianJob)
                    .HasMaxLength(100)
                    .HasColumnName("guardian_job");

                entity.Property(e => e.GuardianName)
                    .HasMaxLength(100)
                    .HasColumnName("guardian_name");

                entity.Property(e => e.GuardianPhone)
                    .HasMaxLength(10)
                    .HasColumnName("guardian_phone");

                entity.Property(e => e.GuardianRole)
                    .HasMaxLength(50)
                    .HasColumnName("guardian_role");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.StudentInfos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_student_info_user_id");
            });

            modelBuilder.Entity<StudentScore>(entity =>
            {
                entity.ToTable("student_scores");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.ScoreTypeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("score_type_id");

                entity.Property(e => e.SemesterId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("semester_id");

                entity.Property(e => e.SubjectId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("subject_id");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.ScoreType)
                    .WithMany(p => p.StudentScores)
                    .HasForeignKey(d => d.ScoreTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_student_scores_score_type_id");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.StudentScores)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_student_scores_semester_id");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.StudentScores)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_student_scores_subject_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.StudentScores)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_student_scores_user_id");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("subjects");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .HasColumnName("code");

                entity.Property(e => e.HoursSemester1).HasColumnName("hours_semester_1");

                entity.Property(e => e.HoursSemester2).HasColumnName("hours_semester_2");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.SubjectGroupId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("subject_group_id");

                entity.Property(e => e.SubjectTypeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("subject_type_id");

                entity.HasOne(d => d.SubjectGroup)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.SubjectGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_subjects_subject_group_id");

                entity.HasOne(d => d.SubjectType)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.SubjectTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_subjects_subject_type_id");
            });

            modelBuilder.Entity<SubjectGroup>(entity =>
            {
                entity.ToTable("subject_groups");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.TeacherId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("teacher_id");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.SubjectGroups)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_subject_groups_teacher_id");
            });

            modelBuilder.Entity<SubjectType>(entity =>
            {
                entity.ToTable("subject_types");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Support>(entity =>
            {
                entity.ToTable("supports");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .HasColumnName("content");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("create_at");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Supports)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_supports_user_id");
            });

            modelBuilder.Entity<SystemSetting>(entity =>
            {
                entity.ToTable("system_settings");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Captcha).HasColumnName("captcha");

                entity.Property(e => e.ThemeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("theme_id");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.SystemSettings)
                    .HasForeignKey(d => d.ThemeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_system_settings_theme_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SystemSettings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_system_settings_user_id");
            });

            modelBuilder.Entity<TeacherFamily>(entity =>
            {
                entity.ToTable("teacher_family");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DistrictCode)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("district_code");

                entity.Property(e => e.GuardianAddressDetail)
                    .HasMaxLength(255)
                    .HasColumnName("guardian_address_detail");

                entity.Property(e => e.GuardianAddressFull)
                    .HasMaxLength(255)
                    .HasColumnName("guardian_address_full");

                entity.Property(e => e.GuardianName)
                    .HasMaxLength(100)
                    .HasColumnName("guardian_name");

                entity.Property(e => e.GuardianPhone)
                    .HasMaxLength(13)
                    .HasColumnName("guardian_phone");

                entity.Property(e => e.ProvinceCode)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("province_code");

                entity.Property(e => e.TeacherId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("teacher_id");

                entity.Property(e => e.WardCode)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ward_code");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.TeacherFamilies)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_teacher_family_teacher_id");
            });

            modelBuilder.Entity<TeacherInfo>(entity =>
            {
                entity.ToTable("teacher_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddressFull)
                    .HasMaxLength(50)
                    .HasColumnName("address_full");

                entity.Property(e => e.Cccd)
                    .HasMaxLength(15)
                    .HasColumnName("cccd");

                entity.Property(e => e.DistrictCode)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("district_code");

                entity.Property(e => e.IssuedDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("issued_date");

                entity.Property(e => e.IssuedPlace)
                    .HasMaxLength(50)
                    .HasColumnName("issued_place");

                entity.Property(e => e.PartyDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("party_date");

                entity.Property(e => e.PartyMember).HasColumnName("party_member");

                entity.Property(e => e.ProvinceCode)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("province_code");

                entity.Property(e => e.UnionDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("union_date");

                entity.Property(e => e.UnionMember).HasColumnName("union_member");

                entity.Property(e => e.UnionPlace)
                    .HasMaxLength(50)
                    .HasColumnName("union_place");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.Property(e => e.WardCode)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ward_code");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TeacherInfos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_teacher_info_user_id");
            });

            modelBuilder.Entity<TeacherTrainingProgram>(entity =>
            {
                entity.HasKey(e => new { e.TeacherId, e.TrainingProgramId })
                    .HasName("teacher_training_program_pkey");

                entity.ToTable("teacher_training_program");

                entity.Property(e => e.TeacherId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("teacher_id");

                entity.Property(e => e.TrainingProgramId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("training_program_id");
            });

            modelBuilder.Entity<TeachingAssignment>(entity =>
            {
                entity.ToTable("teaching_assignments");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClassId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("class_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.EndDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("end_date");

                entity.Property(e => e.StartDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("start_date");

                entity.Property(e => e.SubjectId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("subject_id");

                entity.Property(e => e.TopicsId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("topics_id");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.TeachingAssignments)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_teaching_assignments_class_id");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.TeachingAssignments)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_teaching_assignments_subject_id");

                entity.HasOne(d => d.Topics)
                    .WithMany(p => p.TeachingAssignments)
                    .HasForeignKey(d => d.TopicsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_teaching_assignments_topics_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TeachingAssignments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_teaching_assignments_user_id");
            });

            modelBuilder.Entity<TemporaryLeave>(entity =>
            {
                entity.ToTable("temporary_leave");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Attachment)
                    .HasMaxLength(255)
                    .HasColumnName("attachment");

                entity.Property(e => e.Date)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date");

                entity.Property(e => e.LeadershipId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("leadership_id");

                entity.Property(e => e.Note).HasColumnName("note");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TeacherId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("teacher_id");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.TemporaryLeaves)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_temporary_leave_teacher_id");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.ToTable("tests");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClassIds)
                    .HasMaxLength(255)
                    .HasColumnName("class_ids");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.DurationTime).HasColumnName("duration_time");

                entity.Property(e => e.EndTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("end_time");

                entity.Property(e => e.File)
                    .HasMaxLength(255)
                    .HasColumnName("file");

                entity.Property(e => e.FileSubmit).HasColumnName("file_submit");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.SemesterId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("semester_id");

                entity.Property(e => e.StartTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("start_time");

                entity.Property(e => e.SubjectId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("subject_id");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Tests)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tests_subject_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tests_user_id");
            });

            modelBuilder.Entity<TestAnswer>(entity =>
            {
                entity.ToTable("test_answers");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AnswerText).HasColumnName("answer_text");

                entity.Property(e => e.IsCorrect).HasColumnName("is_correct");

                entity.Property(e => e.QuestionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("question_id");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.TestAnswers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_test_answers_question_id");
            });

            modelBuilder.Entity<TestFile>(entity =>
            {
                entity.ToTable("test_file");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FileUrl).HasColumnName("file_url");

                entity.Property(e => e.TestId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("test_id");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.TestFiles)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_test_file_test_id");
            });

            modelBuilder.Entity<TestQuestion>(entity =>
            {
                entity.ToTable("test_questions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.QuestionText).HasColumnName("question_text");

                entity.Property(e => e.QuestionType)
                    .HasMaxLength(50)
                    .HasColumnName("question_type");

                entity.Property(e => e.TestId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("test_id");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.TestQuestions)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_test_questions_test_id");
            });

            modelBuilder.Entity<TestSubmissionsAnswer>(entity =>
            {
                entity.ToTable("test_submissions_answers");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AnswerText).HasColumnName("answer_text");

                entity.Property(e => e.IsCorrect).HasColumnName("is_correct");

                entity.Property(e => e.QuestionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("question_id");

                entity.Property(e => e.SelectedAnswerId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("selected_answer_id");

                entity.Property(e => e.SubmissionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("submission_id");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.TestSubmissionsAnswers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_test_submissions_answers_question_id");

                entity.HasOne(d => d.SelectedAnswer)
                    .WithMany(p => p.TestSubmissionsAnswers)
                    .HasForeignKey(d => d.SelectedAnswerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_test_submissions_answers_selected_answer_id");

                entity.HasOne(d => d.Submission)
                    .WithMany(p => p.TestSubmissionsAnswers)
                    .HasForeignKey(d => d.SubmissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_test_submissions_answers_submission_id");
            });

            modelBuilder.Entity<TestsAttachment>(entity =>
            {
                entity.ToTable("tests_attachment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FileUrl).HasColumnName("file_url");

                entity.Property(e => e.SubmissionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("submission_id");

                entity.HasOne(d => d.Submission)
                    .WithMany(p => p.TestsAttachments)
                    .HasForeignKey(d => d.SubmissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tests_attachment_submission_id");
            });

            modelBuilder.Entity<TestsSubmission>(entity =>
            {
                entity.ToTable("tests_submissions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CorrectAnswers).HasColumnName("correct_answers");

                entity.Property(e => e.Graded).HasColumnName("graded");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.StudentId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("student_id");

                entity.Property(e => e.SubmittedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("submitted_at");

                entity.Property(e => e.TestId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("test_id");

                entity.Property(e => e.TotalQuestion).HasColumnName("total_question");

                entity.Property(e => e.WrongAnswers).HasColumnName("wrong_answers");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TestsSubmissions)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tests_submissions_student_id");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.TestsSubmissions)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tests_submissions_test_id");
            });

            modelBuilder.Entity<Theme>(entity =>
            {
                entity.ToTable("themes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.ToTable("topics");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.EndDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("end_date");

                entity.Property(e => e.File)
                    .HasMaxLength(255)
                    .HasColumnName("file");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<TopicsFile>(entity =>
            {
                entity.ToTable("topics_file");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("create_at");

                entity.Property(e => e.FileName).HasColumnName("file_name");

                entity.Property(e => e.FileUrl).HasColumnName("file_url");

                entity.Property(e => e.TopicId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("topic_id");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.TopicsFiles)
                    .HasForeignKey(d => d.TopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_topics_file_topic_id");
            });

            modelBuilder.Entity<TrainingProgram>(entity =>
            {
                entity.ToTable("training_programs");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Degree)
                    .HasMaxLength(50)
                    .HasColumnName("degree");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EndDate).HasColumnName("end_date");

                entity.Property(e => e.FileName)
                    .HasMaxLength(250)
                    .HasColumnName("file_name");

                entity.Property(e => e.FilePath)
                    .HasMaxLength(250)
                    .HasColumnName("file_path");

                entity.Property(e => e.MajorId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("major_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.SchoolFacilitiesId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("school_facilities_id");

                entity.Property(e => e.StartDate).HasColumnName("start_date");

                entity.Property(e => e.TrainingForm)
                    .HasMaxLength(50)
                    .HasColumnName("training_form");
            });

            modelBuilder.Entity<TransferSchool>(entity =>
            {
                entity.ToTable("transfer_school");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AttachmentName)
                    .HasMaxLength(255)
                    .HasColumnName("attachment_name");

                entity.Property(e => e.AttachmentPath)
                    .HasMaxLength(255)
                    .HasColumnName("attachment_path");

                entity.Property(e => e.LeadershipId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("leadership_id");

                entity.Property(e => e.Reason).HasColumnName("reason");

                entity.Property(e => e.SchoolAddress)
                    .HasMaxLength(255)
                    .HasColumnName("school_address");

                entity.Property(e => e.StudentId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("student_id");

                entity.Property(e => e.TransferSchoolDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("transfer_school_date");

                entity.Property(e => e.TransferToSchool)
                    .HasMaxLength(255)
                    .HasColumnName("transfer_to_school");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TransferSchools)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_transfer_school_student_id");
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.ToTable("types");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AcademicYearId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("academic_year_id");

                entity.Property(e => e.AddressFull)
                    .HasMaxLength(255)
                    .HasColumnName("address_full");

                entity.Property(e => e.ClassId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("class_id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .HasColumnName("code");

                entity.Property(e => e.DistrictCode)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("district_code");

                entity.Property(e => e.Dob)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("dob");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.EnrollmentDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("enrollment_date");

                entity.Property(e => e.EntryType)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("entry_type");

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .HasColumnName("full_name");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Nation)
                    .HasMaxLength(50)
                    .HasColumnName("nation");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .HasColumnName("phone_number");

                entity.Property(e => e.PlaceBirth)
                    .HasMaxLength(255)
                    .HasColumnName("place_birth");

                entity.Property(e => e.ProvinceCode)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("province_code");

                entity.Property(e => e.Religion)
                    .HasMaxLength(50)
                    .HasColumnName("religion");

                entity.Property(e => e.RoleId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("role_id");

                entity.Property(e => e.Street)
                    .HasMaxLength(255)
                    .HasColumnName("street");

                entity.Property(e => e.UserStatusId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("user_status_id");

                entity.Property(e => e.WardCode)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ward_code");

                entity.HasOne(d => d.AcademicYear)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.AcademicYearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_users_academic_year_id");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_users_class_id");

                entity.HasOne(d => d.EntryTypeNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.EntryType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_users_entry_type");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_users_role_id");

                entity.HasOne(d => d.UserStatus)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_users_user_status_id");
            });

            modelBuilder.Entity<UserStatus>(entity =>
            {
                entity.ToTable("user_status");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<WorkProcess>(entity =>
            {
                entity.ToTable("work_process");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Deleted).HasColumnName("deleted");

                entity.Property(e => e.EndDate).HasColumnName("end_date");

                entity.Property(e => e.IsCurrent).HasColumnName("is_current");

                entity.Property(e => e.Organization)
                    .HasMaxLength(255)
                    .HasColumnName("organization");

                entity.Property(e => e.Position)
                    .HasMaxLength(50)
                    .HasColumnName("position");

                entity.Property(e => e.StartDate).HasColumnName("start_date");

                entity.Property(e => e.SubjectGroupsId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("subject_groups_id");

                entity.Property(e => e.TeacherId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("teacher_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
