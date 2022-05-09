using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieDatabase.Migrations
{
    public partial class InsertRatings9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "SQL", @"6-9-InsertRatings.sql");
            migrationBuilder.Sql(File.ReadAllText(sqlFile));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from usermovies where id >= 90000");
        }
    }
}
