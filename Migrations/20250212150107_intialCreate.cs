using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ISC_ELIB_SERVER.Migrations
{
    public partial class intialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "class_types",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    status = table.Column<bool>(type: "boolean", nullable: true),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "education_levels",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    status = table.Column<bool>(type: "boolean", nullable: true),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_education_levels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "entry_types",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entry_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "major",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    deleted = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_major", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "retirement",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    attachment = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    status = table.Column<bool>(type: "boolean", nullable: true),
                    leadership_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_retirement", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "score_types",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    weight = table.Column<int>(type: "integer", nullable: true),
                    qty_score_semester_1 = table.Column<int>(type: "integer", nullable: true),
                    qty_score_semester_2 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_score_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "subject_types",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    status = table.Column<bool>(type: "boolean", nullable: true),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subject_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "teacher_training_program",
                columns: table => new
                {
                    teacher_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    training_program_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("teacher_training_program_pkey", x => new { x.teacher_id, x.training_program_id });
                });

            migrationBuilder.CreateTable(
                name: "themes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_themes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "topics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    file = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topics", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "training_programs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    major_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    school_facilities_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    degree = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    training_form = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    deleted = table.Column<bool>(type: "boolean", nullable: true),
                    file_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    file_path = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_training_programs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "types",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_status",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "work_process",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    organization = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    subject_groups_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    position = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    is_current = table.Column<bool>(type: "boolean", nullable: true),
                    deleted = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_process", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "schools",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    province_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    district_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ward_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    head_office = table.Column<bool>(type: "boolean", nullable: true),
                    school_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: true),
                    fax = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    established_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    training_model = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    website_url = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    education_level_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schools", x => x.id);
                    table.ForeignKey(
                        name: "fk_schools_education_level_id",
                        column: x => x.education_level_id,
                        principalTable: "education_levels",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "role_permission",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    permission_id = table.Column<long>(type: "bigint", nullable: false),
                    role_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permission", x => x.id);
                    table.ForeignKey(
                        name: "fk_role_permission_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_role_permission_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "topics_file",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    topic_id = table.Column<long>(type: "bigint", nullable: false),
                    file_url = table.Column<string>(type: "text", nullable: true),
                    file_name = table.Column<string>(type: "text", nullable: true),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topics_file", x => x.id);
                    table.ForeignKey(
                        name: "fk_topics_file_topic_id",
                        column: x => x.topic_id,
                        principalTable: "topics",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "academic_years",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    end_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    school_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_academic_years", x => x.id);
                    table.ForeignKey(
                        name: "fk_academic_years_school_id",
                        column: x => x.school_id,
                        principalTable: "schools",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "semesters",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    end_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    academic_year_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_semesters", x => x.id);
                    table.ForeignKey(
                        name: "fk_semesters_academic_year_id",
                        column: x => x.academic_year_id,
                        principalTable: "academic_years",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "achievement",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    content = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    date_awarded = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    file = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    type_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_achievement", x => x.id);
                    table.ForeignKey(
                        name: "fk_achievement_type_id",
                        column: x => x.type_id,
                        principalTable: "types",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "answer_images_qa",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    answer_id = table.Column<long>(type: "bigint", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answer_images_qa", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "answers_qa",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    question_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    content = table.Column<string>(type: "text", nullable: true),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answers_qa", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "campuses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: true),
                    school_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campuses", x => x.id);
                    table.ForeignKey(
                        name: "fk_campuses_school_id",
                        column: x => x.school_id,
                        principalTable: "schools",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "chats",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sent_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    content = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    session_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chats", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "discussion_images",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    discussion_id = table.Column<long>(type: "bigint", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discussion_images", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "discussions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    topic_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    content = table.Column<string>(type: "text", nullable: true),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discussions", x => x.id);
                    table.ForeignKey(
                        name: "fk_discussions_topic_id",
                        column: x => x.topic_id,
                        principalTable: "topics",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "grade_levels",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grade_levels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "classes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    student_quantity = table.Column<int>(type: "integer", nullable: true),
                    subject_quantity = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    grade_level_id = table.Column<long>(type: "bigint", nullable: false),
                    academic_year_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    class_type_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.id);
                    table.ForeignKey(
                        name: "fk_classes_academic_year_id",
                        column: x => x.academic_year_id,
                        principalTable: "academic_years",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_classes_class_type_id",
                        column: x => x.class_type_id,
                        principalTable: "class_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_classes_grade_level_id",
                        column: x => x.grade_level_id,
                        principalTable: "grade_levels",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    full_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    dob = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    gender = table.Column<bool>(type: "boolean", nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    place_birth = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    nation = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    religion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    enrollment_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    role_id = table.Column<long>(type: "bigint", nullable: false),
                    academic_year_id = table.Column<long>(type: "bigint", nullable: false),
                    user_status_id = table.Column<long>(type: "bigint", nullable: false),
                    class_id = table.Column<long>(type: "bigint", nullable: false),
                    entry_type = table.Column<long>(type: "bigint", nullable: false),
                    address_full = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    province_code = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    district_code = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ward_code = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    street = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_academic_year_id",
                        column: x => x.academic_year_id,
                        principalTable: "academic_years",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_users_class_id",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_users_entry_type",
                        column: x => x.entry_type,
                        principalTable: "entry_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_users_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_users_user_status_id",
                        column: x => x.user_status_id,
                        principalTable: "user_status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "change_class",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    old_class_id = table.Column<long>(type: "bigint", nullable: false),
                    change_class_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    new_class_id = table.Column<long>(type: "bigint", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: true),
                    attachment_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    attachment_path = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    leadership_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_change_class", x => x.id);
                    table.ForeignKey(
                        name: "fk_change_class_new_class_id",
                        column: x => x.new_class_id,
                        principalTable: "classes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_change_class_old_class_id",
                        column: x => x.old_class_id,
                        principalTable: "classes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_change_class_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "exemption",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    class_id = table.Column<long>(type: "bigint", nullable: false),
                    exempted_objects = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    form_exemption = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exemption", x => x.id);
                    table.ForeignKey(
                        name: "fk_exemption_class_id",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_exemption_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    content = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    sender_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_notifications_sender_id",
                        column: x => x.sender_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_notifications_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "reserve",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    reserve_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    retention_period = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    reason = table.Column<string>(type: "text", nullable: true),
                    file = table.Column<string>(type: "text", nullable: true),
                    semester = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    class_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    semesters_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    leadership_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reserve", x => x.id);
                    table.ForeignKey(
                        name: "fk_reserve_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "student_info",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    guardian_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    guardian_phone = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    guardian_job = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    guardian_dob = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    guardian_address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    guardian_role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_info", x => x.id);
                    table.ForeignKey(
                        name: "fk_student_info_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "supports",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    content = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supports", x => x.id);
                    table.ForeignKey(
                        name: "fk_supports_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "system_settings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    captcha = table.Column<bool>(type: "boolean", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    theme_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_settings", x => x.id);
                    table.ForeignKey(
                        name: "fk_system_settings_theme_id",
                        column: x => x.theme_id,
                        principalTable: "themes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_system_settings_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "teacher_info",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cccd = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    issued_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    issued_place = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    union_member = table.Column<bool>(type: "boolean", nullable: true),
                    union_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    union_place = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    party_member = table.Column<bool>(type: "boolean", nullable: true),
                    party_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    address_full = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    province_code = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    district_code = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ward_code = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher_info", x => x.id);
                    table.ForeignKey(
                        name: "fk_teacher_info_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "transfer_school",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    transfer_school_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    transfer_to_school = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    school_address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    reason = table.Column<string>(type: "text", nullable: true),
                    attachment_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    attachment_path = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    leadership_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transfer_school", x => x.id);
                    table.ForeignKey(
                        name: "fk_transfer_school_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "resignation",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    attachment = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    status = table.Column<bool>(type: "boolean", nullable: true),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false),
                    leadership_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resignation", x => x.id);
                    table.ForeignKey(
                        name: "fk_resignation_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teacher_info",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "subject_groups",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subject_groups", x => x.id);
                    table.ForeignKey(
                        name: "fk_subject_groups_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teacher_info",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "teacher_family",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false),
                    guardian_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    guardian_phone = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: true),
                    guardian_address_detail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    guardian_address_full = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    province_code = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    district_code = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ward_code = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher_family", x => x.id);
                    table.ForeignKey(
                        name: "fk_teacher_family_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teacher_info",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "temporary_leave",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    attachment = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    status = table.Column<bool>(type: "boolean", nullable: true),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false),
                    leadership_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_temporary_leave", x => x.id);
                    table.ForeignKey(
                        name: "fk_temporary_leave_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teacher_info",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    hours_semester_1 = table.Column<int>(type: "integer", nullable: true),
                    hours_semester_2 = table.Column<int>(type: "integer", nullable: true),
                    subject_group_id = table.Column<long>(type: "bigint", nullable: false),
                    subject_type_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.id);
                    table.ForeignKey(
                        name: "fk_subjects_subject_group_id",
                        column: x => x.subject_group_id,
                        principalTable: "subject_groups",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_subjects_subject_type_id",
                        column: x => x.subject_type_id,
                        principalTable: "subject_types",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "exam_schedule",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    exam_day = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    form = table.Column<bool>(type: "boolean", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    academic_year_id = table.Column<long>(type: "bigint", nullable: false),
                    subject = table.Column<long>(type: "bigint", nullable: false),
                    semester_id = table.Column<long>(type: "bigint", nullable: false),
                    grade_levels_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_schedule", x => x.id);
                    table.ForeignKey(
                        name: "fk_exam_schedule_academic_year_id",
                        column: x => x.academic_year_id,
                        principalTable: "academic_years",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_exam_schedule_grade_levels_id",
                        column: x => x.grade_levels_id,
                        principalTable: "grade_levels",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_exam_schedule_semester_id",
                        column: x => x.semester_id,
                        principalTable: "semesters",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_exam_schedule_subject",
                        column: x => x.subject,
                        principalTable: "subjects",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "exams",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    exam_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    duration_minutes = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    file = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    semester_id = table.Column<long>(type: "bigint", nullable: false),
                    academic_year_id = table.Column<long>(type: "bigint", nullable: false),
                    grade_level_id = table.Column<long>(type: "bigint", nullable: false),
                    class_type_id = table.Column<long>(type: "bigint", nullable: false),
                    subject_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exams", x => x.id);
                    table.ForeignKey(
                        name: "fk_exams_academic_year_id",
                        column: x => x.academic_year_id,
                        principalTable: "academic_years",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_exams_class_type_id",
                        column: x => x.class_type_id,
                        principalTable: "class_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_exams_grade_level_id",
                        column: x => x.grade_level_id,
                        principalTable: "grade_levels",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_exams_semester_id",
                        column: x => x.semester_id,
                        principalTable: "semesters",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_exams_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "question_qa",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subject_id = table.Column<long>(type: "bigint", nullable: false),
                    content = table.Column<string>(type: "text", nullable: true),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_qa", x => x.id);
                    table.ForeignKey(
                        name: "fk_question_qa_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "student_scores",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    score = table.Column<double>(type: "double precision", nullable: true),
                    score_type_id = table.Column<long>(type: "bigint", nullable: false),
                    subject_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    semester_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_scores", x => x.id);
                    table.ForeignKey(
                        name: "fk_student_scores_score_type_id",
                        column: x => x.score_type_id,
                        principalTable: "score_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_student_scores_semester_id",
                        column: x => x.semester_id,
                        principalTable: "semesters",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_student_scores_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_student_scores_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "teaching_assignments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    class_id = table.Column<long>(type: "bigint", nullable: false),
                    subject_id = table.Column<long>(type: "bigint", nullable: false),
                    topics_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teaching_assignments", x => x.id);
                    table.ForeignKey(
                        name: "fk_teaching_assignments_class_id",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_teaching_assignments_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_teaching_assignments_topics_id",
                        column: x => x.topics_id,
                        principalTable: "topics",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_teaching_assignments_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tests",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    duration_time = table.Column<int>(type: "integer", nullable: true),
                    start_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    end_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    file = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    class_ids = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    file_submit = table.Column<bool>(type: "boolean", nullable: true),
                    semester_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subject_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tests", x => x.id);
                    table.ForeignKey(
                        name: "fk_tests_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tests_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "exam_schedule_class",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    class_id = table.Column<long>(type: "bigint", nullable: false),
                    example_schedule = table.Column<long>(type: "bigint", nullable: false),
                    supervisory_teacher_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_schedule_class", x => x.id);
                    table.ForeignKey(
                        name: "fk_exam_schedule_class_class_id",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_exam_schedule_class_example_schedule",
                        column: x => x.example_schedule,
                        principalTable: "exam_schedule",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_exam_schedule_class_supervisory_teacher_id",
                        column: x => x.supervisory_teacher_id,
                        principalTable: "teacher_info",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "exam_graders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    exam_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    class_ids = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_graders", x => x.id);
                    table.ForeignKey(
                        name: "fk_exam_graders_exam_id",
                        column: x => x.exam_id,
                        principalTable: "exams",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_exam_graders_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "question_images_qa",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    question_id = table.Column<long>(type: "bigint", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_images_qa", x => x.id);
                    table.ForeignKey(
                        name: "fk_question_images_qa_question_id",
                        column: x => x.question_id,
                        principalTable: "question_qa",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    duration_time = table.Column<int>(type: "integer", nullable: true),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    auto_open = table.Column<bool>(type: "boolean", nullable: true),
                    share_code_url = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    is_exam = table.Column<bool>(type: "boolean", nullable: true),
                    teaching_assignment_id = table.Column<long>(type: "bigint", nullable: false),
                    exam_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessions", x => x.id);
                    table.ForeignKey(
                        name: "fk_sessions_exam_id",
                        column: x => x.exam_id,
                        principalTable: "exams",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_sessions_teaching_assignment_id",
                        column: x => x.teaching_assignment_id,
                        principalTable: "teaching_assignments",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "test_file",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    test_id = table.Column<long>(type: "bigint", nullable: false),
                    file_url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_file", x => x.id);
                    table.ForeignKey(
                        name: "fk_test_file_test_id",
                        column: x => x.test_id,
                        principalTable: "tests",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "test_questions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    test_id = table.Column<long>(type: "bigint", nullable: false),
                    question_text = table.Column<string>(type: "text", nullable: true),
                    question_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_questions", x => x.id);
                    table.ForeignKey(
                        name: "fk_test_questions_test_id",
                        column: x => x.test_id,
                        principalTable: "tests",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tests_submissions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    test_id = table.Column<long>(type: "bigint", nullable: false),
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    submitted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    total_question = table.Column<int>(type: "integer", nullable: true),
                    correct_answers = table.Column<int>(type: "integer", nullable: true),
                    wrong_answers = table.Column<int>(type: "integer", nullable: true),
                    score = table.Column<double>(type: "double precision", nullable: true),
                    graded = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tests_submissions", x => x.id);
                    table.ForeignKey(
                        name: "fk_tests_submissions_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tests_submissions_test_id",
                        column: x => x.test_id,
                        principalTable: "tests",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "test_answers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    question_id = table.Column<long>(type: "bigint", nullable: false),
                    answer_text = table.Column<string>(type: "text", nullable: true),
                    is_correct = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_answers", x => x.id);
                    table.ForeignKey(
                        name: "fk_test_answers_question_id",
                        column: x => x.question_id,
                        principalTable: "test_questions",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tests_attachment",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    submission_id = table.Column<long>(type: "bigint", nullable: false),
                    file_url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tests_attachment", x => x.id);
                    table.ForeignKey(
                        name: "fk_tests_attachment_submission_id",
                        column: x => x.submission_id,
                        principalTable: "tests_submissions",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "test_submissions_answers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    submission_id = table.Column<long>(type: "bigint", nullable: false),
                    question_id = table.Column<long>(type: "bigint", nullable: false),
                    selected_answer_id = table.Column<long>(type: "bigint", nullable: false),
                    answer_text = table.Column<string>(type: "text", nullable: true),
                    is_correct = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_submissions_answers", x => x.id);
                    table.ForeignKey(
                        name: "fk_test_submissions_answers_question_id",
                        column: x => x.question_id,
                        principalTable: "test_questions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_test_submissions_answers_selected_answer_id",
                        column: x => x.selected_answer_id,
                        principalTable: "test_answers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_test_submissions_answers_submission_id",
                        column: x => x.submission_id,
                        principalTable: "tests_submissions",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_academic_years_school_id",
                table: "academic_years",
                column: "school_id");

            migrationBuilder.CreateIndex(
                name: "IX_achievement_type_id",
                table: "achievement",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_achievement_user_id",
                table: "achievement",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_answer_images_qa_answer_id",
                table: "answer_images_qa",
                column: "answer_id");

            migrationBuilder.CreateIndex(
                name: "IX_answers_qa_question_id",
                table: "answers_qa",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_campuses_school_id",
                table: "campuses",
                column: "school_id");

            migrationBuilder.CreateIndex(
                name: "IX_campuses_user_id",
                table: "campuses",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_change_class_new_class_id",
                table: "change_class",
                column: "new_class_id");

            migrationBuilder.CreateIndex(
                name: "IX_change_class_old_class_id",
                table: "change_class",
                column: "old_class_id");

            migrationBuilder.CreateIndex(
                name: "IX_change_class_student_id",
                table: "change_class",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_chats_session_id",
                table: "chats",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_academic_year_id",
                table: "classes",
                column: "academic_year_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_class_type_id",
                table: "classes",
                column: "class_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_grade_level_id",
                table: "classes",
                column: "grade_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_user_id",
                table: "classes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_discussion_images_discussion_id",
                table: "discussion_images",
                column: "discussion_id");

            migrationBuilder.CreateIndex(
                name: "IX_discussions_topic_id",
                table: "discussions",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "IX_discussions_user_id",
                table: "discussions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_graders_exam_id",
                table: "exam_graders",
                column: "exam_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_graders_user_id",
                table: "exam_graders",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_schedule_academic_year_id",
                table: "exam_schedule",
                column: "academic_year_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_schedule_grade_levels_id",
                table: "exam_schedule",
                column: "grade_levels_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_schedule_semester_id",
                table: "exam_schedule",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_schedule_subject",
                table: "exam_schedule",
                column: "subject");

            migrationBuilder.CreateIndex(
                name: "IX_exam_schedule_class_class_id",
                table: "exam_schedule_class",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_schedule_class_example_schedule",
                table: "exam_schedule_class",
                column: "example_schedule");

            migrationBuilder.CreateIndex(
                name: "IX_exam_schedule_class_supervisory_teacher_id",
                table: "exam_schedule_class",
                column: "supervisory_teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_exams_academic_year_id",
                table: "exams",
                column: "academic_year_id");

            migrationBuilder.CreateIndex(
                name: "IX_exams_class_type_id",
                table: "exams",
                column: "class_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_exams_grade_level_id",
                table: "exams",
                column: "grade_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_exams_semester_id",
                table: "exams",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "IX_exams_subject_id",
                table: "exams",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_exemption_class_id",
                table: "exemption",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_exemption_student_id",
                table: "exemption",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_grade_levels_teacher_id",
                table: "grade_levels",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_sender_id",
                table: "notifications",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_user_id",
                table: "notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_question_images_qa_question_id",
                table: "question_images_qa",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_question_qa_subject_id",
                table: "question_qa",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_reserve_student_id",
                table: "reserve",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_resignation_teacher_id",
                table: "resignation",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_permission_id",
                table: "role_permission",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_role_id",
                table: "role_permission",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_schools_education_level_id",
                table: "schools",
                column: "education_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_semesters_academic_year_id",
                table: "semesters",
                column: "academic_year_id");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_exam_id",
                table: "sessions",
                column: "exam_id");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_teaching_assignment_id",
                table: "sessions",
                column: "teaching_assignment_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_info_user_id",
                table: "student_info",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_scores_score_type_id",
                table: "student_scores",
                column: "score_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_scores_semester_id",
                table: "student_scores",
                column: "semester_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_scores_subject_id",
                table: "student_scores",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_scores_user_id",
                table: "student_scores",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_subject_groups_teacher_id",
                table: "subject_groups",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_subjects_subject_group_id",
                table: "subjects",
                column: "subject_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_subjects_subject_type_id",
                table: "subjects",
                column: "subject_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_supports_user_id",
                table: "supports",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_system_settings_theme_id",
                table: "system_settings",
                column: "theme_id");

            migrationBuilder.CreateIndex(
                name: "IX_system_settings_user_id",
                table: "system_settings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_family_teacher_id",
                table: "teacher_family",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_info_user_id",
                table: "teacher_info",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_teaching_assignments_class_id",
                table: "teaching_assignments",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_teaching_assignments_subject_id",
                table: "teaching_assignments",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_teaching_assignments_topics_id",
                table: "teaching_assignments",
                column: "topics_id");

            migrationBuilder.CreateIndex(
                name: "IX_teaching_assignments_user_id",
                table: "teaching_assignments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_temporary_leave_teacher_id",
                table: "temporary_leave",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_answers_question_id",
                table: "test_answers",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_file_test_id",
                table: "test_file",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_questions_test_id",
                table: "test_questions",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_submissions_answers_question_id",
                table: "test_submissions_answers",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_submissions_answers_selected_answer_id",
                table: "test_submissions_answers",
                column: "selected_answer_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_submissions_answers_submission_id",
                table: "test_submissions_answers",
                column: "submission_id");

            migrationBuilder.CreateIndex(
                name: "IX_tests_subject_id",
                table: "tests",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_tests_user_id",
                table: "tests",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tests_attachment_submission_id",
                table: "tests_attachment",
                column: "submission_id");

            migrationBuilder.CreateIndex(
                name: "IX_tests_submissions_student_id",
                table: "tests_submissions",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_tests_submissions_test_id",
                table: "tests_submissions",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "IX_topics_file_topic_id",
                table: "topics_file",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "IX_transfer_school_student_id",
                table: "transfer_school",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_academic_year_id",
                table: "users",
                column: "academic_year_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_class_id",
                table: "users",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_entry_type",
                table: "users",
                column: "entry_type");

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_user_status_id",
                table: "users",
                column: "user_status_id");

            migrationBuilder.AddForeignKey(
                name: "fk_achievement_user_id",
                table: "achievement",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_answer_images_qa_answer_id",
                table: "answer_images_qa",
                column: "answer_id",
                principalTable: "answers_qa",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_answers_qa_question_id",
                table: "answers_qa",
                column: "question_id",
                principalTable: "question_qa",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_campuses_user_id",
                table: "campuses",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_chats_session_id",
                table: "chats",
                column: "session_id",
                principalTable: "sessions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_discussion_images_discussion_id",
                table: "discussion_images",
                column: "discussion_id",
                principalTable: "discussions",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_discussions_user_id",
                table: "discussions",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_grade_levels_teacher_id",
                table: "grade_levels",
                column: "teacher_id",
                principalTable: "teacher_info",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_classes_user_id",
                table: "classes",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_academic_years_school_id",
                table: "academic_years");

            migrationBuilder.DropForeignKey(
                name: "fk_users_class_id",
                table: "users");

            migrationBuilder.DropTable(
                name: "achievement");

            migrationBuilder.DropTable(
                name: "answer_images_qa");

            migrationBuilder.DropTable(
                name: "campuses");

            migrationBuilder.DropTable(
                name: "change_class");

            migrationBuilder.DropTable(
                name: "chats");

            migrationBuilder.DropTable(
                name: "discussion_images");

            migrationBuilder.DropTable(
                name: "exam_graders");

            migrationBuilder.DropTable(
                name: "exam_schedule_class");

            migrationBuilder.DropTable(
                name: "exemption");

            migrationBuilder.DropTable(
                name: "major");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "question_images_qa");

            migrationBuilder.DropTable(
                name: "reserve");

            migrationBuilder.DropTable(
                name: "resignation");

            migrationBuilder.DropTable(
                name: "retirement");

            migrationBuilder.DropTable(
                name: "role_permission");

            migrationBuilder.DropTable(
                name: "student_info");

            migrationBuilder.DropTable(
                name: "student_scores");

            migrationBuilder.DropTable(
                name: "supports");

            migrationBuilder.DropTable(
                name: "system_settings");

            migrationBuilder.DropTable(
                name: "teacher_family");

            migrationBuilder.DropTable(
                name: "teacher_training_program");

            migrationBuilder.DropTable(
                name: "temporary_leave");

            migrationBuilder.DropTable(
                name: "test_file");

            migrationBuilder.DropTable(
                name: "test_submissions_answers");

            migrationBuilder.DropTable(
                name: "tests_attachment");

            migrationBuilder.DropTable(
                name: "topics_file");

            migrationBuilder.DropTable(
                name: "training_programs");

            migrationBuilder.DropTable(
                name: "transfer_school");

            migrationBuilder.DropTable(
                name: "work_process");

            migrationBuilder.DropTable(
                name: "types");

            migrationBuilder.DropTable(
                name: "answers_qa");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "discussions");

            migrationBuilder.DropTable(
                name: "exam_schedule");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "score_types");

            migrationBuilder.DropTable(
                name: "themes");

            migrationBuilder.DropTable(
                name: "test_answers");

            migrationBuilder.DropTable(
                name: "tests_submissions");

            migrationBuilder.DropTable(
                name: "question_qa");

            migrationBuilder.DropTable(
                name: "exams");

            migrationBuilder.DropTable(
                name: "teaching_assignments");

            migrationBuilder.DropTable(
                name: "test_questions");

            migrationBuilder.DropTable(
                name: "semesters");

            migrationBuilder.DropTable(
                name: "topics");

            migrationBuilder.DropTable(
                name: "tests");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropTable(
                name: "subject_groups");

            migrationBuilder.DropTable(
                name: "subject_types");

            migrationBuilder.DropTable(
                name: "schools");

            migrationBuilder.DropTable(
                name: "education_levels");

            migrationBuilder.DropTable(
                name: "classes");

            migrationBuilder.DropTable(
                name: "class_types");

            migrationBuilder.DropTable(
                name: "grade_levels");

            migrationBuilder.DropTable(
                name: "teacher_info");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "academic_years");

            migrationBuilder.DropTable(
                name: "entry_types");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "user_status");
        }
    }
}
