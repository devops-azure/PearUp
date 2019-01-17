using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PearUp.Repository.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "InterestsHiLo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "UserHiLo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Interests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    InterestDescription = table.Column<string>(maxLength: 512, nullable: true),
                    InterestName = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Exception = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    StackTrace = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    BucketList = table.Column<string>(maxLength: 140, nullable: false),
                    Email = table.Column<string>(maxLength: 512, nullable: true),
                    FullName = table.Column<string>(maxLength: 512, nullable: false),
                    FunAndInterestingThings = table.Column<string>(maxLength: 140, nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false),
                    Profession = table.Column<string>(maxLength: 100, nullable: false),
                    School = table.Column<string>(maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    LookingFor = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: false),
                    PasswordSalt = table.Column<byte[]>(nullable: false),
                    Distance = table.Column<int>(nullable: false),
                    MaxAge = table.Column<int>(nullable: false),
                    MinAge = table.Column<int>(nullable: false),
                    CountryCode = table.Column<string>(maxLength: 3, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInterest",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    InterestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInterest", x => new { x.UserId, x.InterestId });
                    table.ForeignKey(
                        name: "FK_UserInterest_Interests_InterestId",
                        column: x => x.InterestId,
                        principalTable: "Interests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInterest_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInterest_InterestId",
                table: "UserInterest",
                column: "InterestId");

            migrationBuilder.AddUniqueConstraint(
                name: "IX_User_PhoneNumber",
                table: "User",
                column: "PhoneNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "UserInterest");

            migrationBuilder.DropTable(
                name: "Interests");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropSequence(
                name: "InterestsHiLo");

            migrationBuilder.DropSequence(
                name: "UserHiLo");
        }
    }
}
