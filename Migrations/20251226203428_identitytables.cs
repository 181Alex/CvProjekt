using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CvProjekt.Migrations
{
    /// <inheritdoc />
    public partial class identitytables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Education",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Education",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Education",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Education",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Education",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Qualifications",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Qualifications",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Qualifications",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Qualifications",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Qualifications",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Qualifications",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Qualifications",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Qualifications",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Works",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Works",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Works",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Works",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-5");

            migrationBuilder.DeleteData(
                table: "Resumes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Resumes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Resumes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Resumes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Resumes",
                keyColumn: "Id",
                keyValue: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "Id", "AccessFailedCount", "Adress", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "ImgUrl", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileVisits", "ResumeId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "user-1", 0, "Storgatan 1", "static-concurrency-stamp-user-1", "erik@mail.com", true, "Erik", "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=200", true, "Svensson", false, null, "ERIK@MAIL.COM", "ERIK@MAIL.COM", "AQAAAAIAAYagAAAAELg7Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy==", null, false, 0, 1, "static-security-stamp-user-1", false, "erik@mail.com" },
                    { "user-2", 0, "Sveavägen 10", "static-concurrency-stamp-user-2", "anna@mail.com", true, "Anna", "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=200", true, "Lind", false, null, "ANNA@MAIL.COM", "ANNA@MAIL.COM", "AQAAAAIAAYagAAAAELg7Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy==", null, false, 0, 2, "static-security-stamp-user-2", false, "anna@mail.com" },
                    { "user-3", 0, "Hamngatan 4", "static-concurrency-stamp-user-3", "johan@mail.com", true, "Johan", "https://images.unsplash.com/photo-1560250097-0b93528c311a?w=200", true, "Ek", false, null, "JOHAN@MAIL.COM", "JOHAN@MAIL.COM", "AQAAAAIAAYagAAAAELg7Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy==", null, false, 0, 3, "static-security-stamp-user-3", false, "johan@mail.com" },
                    { "user-4", 0, "Skolgatan 55", "static-concurrency-stamp-user-4", "sara@mail.com", true, "Sara", "https://images.unsplash.com/photo-1573496359-0933d2768d98?w=200", true, "Berg", false, null, "SARA@MAIL.COM", "SARA@MAIL.COM", "AQAAAAIAAYagAAAAELg7Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy==", null, false, 0, 4, "static-security-stamp-user-4", false, "sara@mail.com" },
                    { "user-5", 0, "Studentvägen 3", "static-concurrency-stamp-user-5", "david@mail.com", true, "David", "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=200", false, "Nordin", false, null, "DAVID@MAIL.COM", "DAVID@MAIL.COM", "AQAAAAIAAYagAAAAELg7Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy0k9/8Q7k6Xy==", null, false, 0, 5, "static-security-stamp-user-5", false, "david@mail.com" }
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
                    { 1, "Volvo", "Backend C#", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Utvecklare", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Spotify", "Jobbar med webbspelaren", null, "Frontend Dev", 2, new DateTime(2021, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "IKEA", "Ledde IT-team", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chef", 3, new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Google", "AI forskning", null, "Data Analyst", 4, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "Date", "FromUserId", "Read", "Text", "ToUserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "user-1", true, "Tjena Anna! Snygg frontend du byggde.", "user-2" },
                    { 2, new DateTime(2024, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "user-2", false, "Tack Erik! Behöver hjälp med API:et dock.", "user-1" },
                    { 3, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user-3", false, "Hej David, söker du jobb?", "user-5" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Description", "GithubLink", "Language", "Title", "UserId", "Year" },
                values: new object[,]
                {
                    { 1, "Byggde en butik", "github.com/erik/shop", "C#", "E-handel", "user-1", 2023 },
                    { 2, "Min hemsida", "github.com/anna/me", "React", "Portfolio", "user-2", 2024 },
                    { 3, "AI budgetering", "github.com/sara/cash", "Python", "BudgetApp", "user-4", 2022 }
                });
        }
    }
}
