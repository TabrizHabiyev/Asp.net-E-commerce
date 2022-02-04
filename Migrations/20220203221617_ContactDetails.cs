using Microsoft.EntityFrameworkCore.Migrations;

namespace Asp.net_E_commerce.Migrations
{
    public partial class ContactDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SliderCompany",
                table: "SliderCompany");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeaturesBanners",
                table: "FeaturesBanners");

            migrationBuilder.RenameTable(
                name: "SliderCompany",
                newName: "sliderCompany");

            migrationBuilder.RenameTable(
                name: "FeaturesBanners",
                newName: "featuresBanners");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sliderCompany",
                table: "sliderCompany",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_featuresBanners",
                table: "featuresBanners",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "contactDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneMobile = table.Column<string>(nullable: true),
                    PhoneHotline = table.Column<string>(nullable: true),
                    MapUrl = table.Column<string>(nullable: true),
                    OpenClosed = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contactDetails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contactDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sliderCompany",
                table: "sliderCompany");

            migrationBuilder.DropPrimaryKey(
                name: "PK_featuresBanners",
                table: "featuresBanners");

            migrationBuilder.RenameTable(
                name: "sliderCompany",
                newName: "SliderCompany");

            migrationBuilder.RenameTable(
                name: "featuresBanners",
                newName: "FeaturesBanners");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SliderCompany",
                table: "SliderCompany",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeaturesBanners",
                table: "FeaturesBanners",
                column: "Id");
        }
    }
}
