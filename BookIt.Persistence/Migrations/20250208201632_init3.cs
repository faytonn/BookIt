using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookIt.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "ImagePath", "IsDeleted", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 8, 21, 16, 30, 710, DateTimeKind.Local).AddTicks(9338), "Default", "https://res.cloudinary.com/di3ourpee/image/upload/v1738786112/bookit./HitMeHardAndSoft.jpg", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Default" },
                    { 2, new DateTime(2025, 2, 8, 21, 16, 30, 710, DateTimeKind.Local).AddTicks(9459), "Default", "https://res.cloudinary.com/di3ourpee/image/upload/v1738786345/bookit./KendrickLamar.webp", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Default" },
                    { 3, new DateTime(2025, 2, 8, 21, 16, 30, 710, DateTimeKind.Local).AddTicks(9468), "Default", "https://res.cloudinary.com/di3ourpee/image/upload/v1738786457/bookit./paddington3.jpg", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Default" }
                });

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Within hours of its release, ...");

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Çıxışından bir neçə saat sonra, ...");

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Během několika hodin od vydání, ...");

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: "In a jaw-dropping turn of events, ...");

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "Şaşırtıcı hadisələrlə dolu bir gecədə, ...");

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 6,
                column: "Description",
                value: "Ve velkolepém obratu událostí, ...");

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 7,
                column: "Description",
                value: "Paddington 3 returns this summer with a delightful new adventure, ...");

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 8,
                column: "Description",
                value: "Paddington 3 bu yay sevilən ayıcığı ilə tamaşaçıları yenidən bir araya gətirən gözəl bir macəra, ...");

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 9,
                column: "Description",
                value: "Paddington 3 se toto léto vrací s kouzelným novým dobrodružstvím, ...");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Within hours of its release, Hit Me Hard and Soft surged to the top of major streaming platforms, breaking previous records for the fastest-streamed album debut. Industry insiders attribute this rapid success to Eilish’s massive global fanbase and the album's fresh take on modern pop and alternative music. “Billie has a rare ability to reinvent her sound while staying true to her artistic vision,” commented a representative from a leading music analytics firm. “The record-breaking numbers are a testament to her influence and the universal appeal of this album.”\r\n\r\n");

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Çıxışından bir neçə saat sonra, Hit Me Hard and Soft əsas axın platformlarında zirvəyə çatdı və ən sürətli axın edilən albom debütü üçün əvvəlki rekordları sarsıtdı. Sənaye mütəxəssisləri bu sürətli uğuru Eilish-in qlobal fan bazasının genişliyi və albomun müasir pop və alternativ musiqiyə gətirdiyi təzə yanaşmaya bağlayır. “Billie səsini yenidən kəşf etmək və bədii vizyonuna sadiq qalmaq bacarığı ilə nadir bir istedaddır,” – deyə şərh etdi aparıcı musiqi analitik şirkətinin nümayəndəsi. “Rekord qıran rəqəmlər onun təsir gücünün və albomun universal cazibəsinin canlı sübutudur.");

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Během několika hodin od vydání se album Hit Me Hard and Soft dostalo na vrchol předních streamovacích platforem a překonalo předchozí rekordy nejrychleji streamovaného debutu alba. Odborníci z oboru připisují tento rychlý úspěch masivní globální fanouškovské základně Eilish a svěžímu pohledu alba na moderní pop a alternativní hudbu. „Billie má vzácnou schopnost znovu objevit svůj zvuk a zároveň zůstat věrná své umělecké vizi,“ poznamenal zástupce přední hudební analytické firmy. „Rekordní čísla jsou důkazem jejího vlivu a univerzální přitažlivosti tohoto alba.");

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: "In a jaw-dropping turn of events, Kendrick Llamar has swept the Grammy Awards by winning five major categories in one night—all thanks to his explosive diss track aimed at rap superstar Drake. The track, which has taken the music world by storm, features razor-sharp lyrics and an electrifying beat that has left critics and fans in awe. Industry insiders are hailing the win as a turning point in the longstanding rivalry between the two rap titans. “Kendrick’s daring approach has redefined the boundaries of hip-hop,” commented a Grammy committee member. The diss track not only resonated powerfully with fans but also set new benchmarks for lyrical prowess and artistic innovation in the genre.");

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "Şaşırtıcı hadisələrlə dolu bir gecədə, Kendrick Llamar rap super ulduzu Drake-ə qarşı olan partlayıcı diss treki sayəsində beş əsas kateqoriyada Grammy mükafatını qazanaraq böyük uğur əldə etdi. Musiqi dünyasını əsir edən bu parça, kəskin sözləri və elektrikli ritmi ilə tənqidçiləri və pərəstişkarları valeh etdi. Sənaye mütəxəssisləri bu qalibiyyəti iki rap nəhənginin uzun müddətli rəqabətində dönüm nöqtəsi kimi qiymətləndirirlər. “Kendrick-in cəsur yanaşması hip-hop sərhədlərini yenidən müəyyənləşdirdi,” – deyə Grammy komitəsinin üzvü şərh etdi. Diss treki yalnız pərəstişkarların qəlbinə toxunmaqla qalmayıb, həm də janrda söz ustalığı və bədii innovasiya üçün yeni standartlar qoydu.");

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 6,
                column: "Description",
                value: "Ve velkolepém obratu událostí si Kendrick Llamar vybojoval Grammy Awards, když během jediné noci získal pět hlavních kategorií – a to díky svému explozivnímu disstracku, který míří na rapovou superhvězdu Drakea. Singl, který otřásl hudebním světem, se vyznačuje ostrými texty a elektrizujícím beatem, který ohromil kritiky i fanoušky. Odborníci z oboru označují tento úspěch za zlomový moment v dlouhotrvající rivalitě mezi těmito dvěma rapovými giganty. „Kendrickův odvážný přístup redefinoval hranice hip-hopu,“ komentoval člen Grammy komise. Disstrack nejenže silně rezonoval s fanoušky, ale také nastavil nové standardy pro lyrickou zdatnost a umělecké inovace v tomto žánru.");

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 7,
                column: "Description",
                value: "Paddington 3 returns this summer with a delightful new adventure that reunites fans with the lovable bear. Blending classic charm with modern twists, the film promises heartwarming moments, clever humor, and plenty of surprises for audiences of all ages.");

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 8,
                column: "Description",
                value: "Paddington 3 bu yay sevilən ayıcığı ilə tamaşaçıları yenidən bir araya gətirən gözəl bir macəra ilə geri dönür. Klassik cazibəni müasir tərzlərlə birləşdirən film, hər yaşdan izləyicilər üçün ürəkaçan anlar, ağıllı yumor və bir çox sürpriz vəd edir.");

            migrationBuilder.UpdateData(
                table: "NewsDetails",
                keyColumn: "Id",
                keyValue: 9,
                column: "Description",
                value: "Paddington 3 se toto léto vrací s kouzelným novým dobrodružstvím, které opět spojuje fanoušky s milovaným medvídkem. Spojující klasické kouzlo s moderními prvky, film slibuje dojemné chvíle, chytrý humor a spoustu překvapení pro diváky všech věkových kategorií.");
        }
    }
}
