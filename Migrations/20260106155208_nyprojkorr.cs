using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CvProjekt.Migrations
{
    /// <inheritdoc />
    public partial class nyprojkorr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resumes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resumes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    ProfileVisits = table.Column<int>(type: "int", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    ResumeId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Resumes_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DegreeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    EndYear = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ResumeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Education_Resumes_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Qualifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResumeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Qualifications_Resumes_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ResumeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Works_Resumes_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Read = table.Column<bool>(type: "bit", nullable: false),
                    SenderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ToUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GithubLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMembers",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ProjectId1 = table.Column<int>(type: "int", nullable: true),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMembers", x => new { x.UserId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_ProjectMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Projects_ProjectId1",
                        column: x => x.ProjectId1,
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Resumes",
                column: "Id",
                values: new object[]
                {
                    1,
                    2,
                    3,
                    4,
                    5
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Adress", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "ImgUrl", "IsActive", "IsPrivate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileVisits", "ResumeId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "user-1", 0, "Storgatan 1", "static-concurrency-stamp-user-1", "erik@mail.com", true, "Erik", "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=200", true, false, "Svensson", false, null, "ERIK@MAIL.COM", "ERIK@MAIL.COM", "AQAAAAIAAYagAAAAELg7Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy==", null, false, 0, 1, "static-security-stamp-user-1", false, "erik@mail.com" },
                    { "user-2", 0, "Sveavägen 10", "static-concurrency-stamp-user-2", "anna@mail.com", true, "Anna", "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=200", true, false, "Lind", false, null, "ANNA@MAIL.COM", "ANNA@MAIL.COM", "AQAAAAIAAYagAAAAELg7Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy==", null, false, 0, 2, "static-security-stamp-user-2", false, "anna@mail.com" },
                    { "user-3", 0, "Hamngatan 4", "static-concurrency-stamp-user-3", "johan@mail.com", true, "Johan", "https://images.unsplash.com/photo-1560250097-0b93528c311a?w=200", true, false, "Ek", false, null, "JOHAN@MAIL.COM", "JOHAN@MAIL.COM", "AQAAAAIAAYagAAAAELg7Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy==", null, false, 0, 3, "static-security-stamp-user-3", false, "johan@mail.com" },
                    { "user-4", 0, "Skolgatan 55", "static-concurrency-stamp-user-4", "sara@mail.com", true, "Sara", "https://images.unsplash.com/photo-1573496359-0933d2768d98?w=200", true, false, "Berg", false, null, "SARA@MAIL.COM", "SARA@MAIL.COM", "AQAAAAIAAYagAAAAELg7Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy==", null, false, 0, 4, "static-security-stamp-user-4", false, "sara@mail.com" },
                    { "user-5", 0, "Studentvägen 3", "static-concurrency-stamp-user-5", "david@mail.com", true, "David", "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=200", false, false, "Nordin", false, null, "DAVID@MAIL.COM", "DAVID@MAIL.COM", "AQAAAAIAAYagAAAAELg7Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy==", null, false, 0, 5, "static-security-stamp-user-5", false, "david@mail.com" }
                });

            migrationBuilder.InsertData(
                table: "Education",
                columns: new[] { "Id", "DegreeName", "Description", "EndYear", "ResumeId", "SchoolName", "StartYear" },
                values: new object[,]
                {
                    { 1, "Civilingenjör", "Datateknik", 2020, 1, "KTH", 2015 },
                    { 2, "Frontend", "YH-utbildning", 2021, 2, "Nackademin", 2019 },
                    { 3, "Ekonomi", "Master", 2014, 3, "Handels", 2010 },
                    { 4, "PhD CS", "Forskning", 2019, 4, "MIT", 2015 },
                    { 5, "Teknik", "Student", 2024, 5, "Gymnasiet", 2021 }
                });

            migrationBuilder.InsertData(
                table: "Qualifications",
                columns: new[] { "Id", "Name", "ResumeId" },
                values: new object[,]
                {
                    { 1, "C#", 1 },
                    { 2, "SQL Server", 1 },
                    { 3, "JavaScript", 2 },
                    { 4, "React", 2 },
                    { 5, "Scrum", 3 },
                    { 6, "Python", 4 },
                    { 7, "Machine Learning", 4 },
                    { 8, "HTML", 5 }
                });

            migrationBuilder.InsertData(
                table: "Works",
                columns: new[] { "Id", "CompanyName", "Description", "EndDate", "Position", "ResumeId", "StartDate" },
                values: new object[,]
                {
                    { 1, "Volvo", "Backend C#", new DateOnly(2022, 1, 1), "Utvecklare", 1, new DateOnly(2020, 1, 1) },
                    { 2, "Spotify", "Jobbar med webbspelaren", null, "Frontend Dev", 2, new DateOnly(2021, 5, 1) },
                    { 3, "IKEA", "Ledde IT-team", new DateOnly(2023, 1, 1), "Chef", 3, new DateOnly(2018, 1, 1) },
                    { 4, "Google", "AI forskning", null, "Data Analyst", 4, new DateOnly(2019, 1, 1) }
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "Date", "FromUserId", "Read", "SenderName", "Text", "ToUserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "user-1", true, "Erik Svensson", "Tjena Anna! Snygg frontend du byggde.", "user-2" },
                    { 2, new DateTime(2024, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "user-2", false, "Anna Andersson", "Tack Erik! Behöver hjälp med API:et dock.", "user-1" },
                    { 3, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user-3", false, "Cecilia Carlsson", "Hej David, söker du jobb?", "user-5" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatorId", "Description", "GithubLink", "Language", "Title", "Year" },
                values: new object[,]
                {
                    { 1, "user-1", "Byggde en butik", "github.com/erik/shop", "C#", "E-handel", 2023 },
                    { 2, "user-2", "Min hemsida", "github.com/anna/me", "React", "Portfolio", 2024 },
                    { 3, "user-4", "AI budgetering", "github.com/sara/cash", "Python", "BudgetApp", 2022 }
                });

            migrationBuilder.InsertData(
                table: "ProjectMembers",
                columns: new[] { "ProjectId", "UserId", "ProjectId1", "UserId1" },
                values: new object[,]
                {
                    { 1, "user-1", null, null },
                    { 1, "user-2", null, null },
                    { 2, "user-2", null, null },
                    { 1, "user-3", null, null },
                    { 3, "user-4", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ResumeId",
                table: "AspNetUsers",
                column: "ResumeId",
                unique: true,
                filter: "[ResumeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Education_ResumeId",
                table: "Education",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FromUserId",
                table: "Messages",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ToUserId",
                table: "Messages",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProjectId1",
                table: "ProjectMembers",
                column: "ProjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_UserId1",
                table: "ProjectMembers",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatorId",
                table: "Projects",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Qualifications_ResumeId",
                table: "Qualifications",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_ResumeId",
                table: "Works",
                column: "ResumeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Education");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "ProjectMembers");

            migrationBuilder.DropTable(
                name: "Qualifications");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Resumes");
        }
    }
}
