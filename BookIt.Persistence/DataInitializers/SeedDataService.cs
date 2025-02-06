using BookIt.Domain.Entities;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;

namespace BookIt.Persistence.DataInitializers;

public static class SeedDataService
{

    public static void AddSeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder.AddLanguages();
        modelBuilder.AddSettings();
        modelBuilder.AddNews();
    }

    public static void AddLanguages(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Language>().HasData(
            new Language { Id = 1, LangaugeName = "EN", IsoCode = "en", ImagePath = "https://res.cloudinary.com/di3ourpee/image/upload/v1738784159/bookit./UK%20Flag.svg" },
            new Language { Id = 2, LangaugeName = "AZE", IsoCode = "az", ImagePath = "https://res.cloudinary.com/di3ourpee/image/upload/v1738784159/bookit./AZE%20Flag.png" },
            new Language { Id = 3, LangaugeName = "CS", IsoCode = "cze", ImagePath = "https://res.cloudinary.com/di3ourpee/image/upload/v1738783701/bookit./CZE%20Flag.svg" }
            );
    }

    public static void AddSettings(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Setting>().HasData(
            new Setting { Id = 1, Key = "Office Address" },
            new Setting { Id = 2, Key = "SupportPhone" },
            new Setting { Id = 3, Key = "InstagramLink" },
            new Setting { Id = 4, Key = "FacebookLink" },
            new Setting { Id = 5, Key = "TwitterLink" },
            new Setting { Id = 6, Key = "YoutubeLink" },
            new Setting { Id = 7, Key = "Email" }
            );

        modelBuilder.Entity<SettingDetail>().HasData(
            new SettingDetail { Id = 1, Value = "Narimanov region, street Tabriz 41", SettingId = 1, LanguageId = 1 },
            new SettingDetail { Id = 2, Value = "Nərimanov rayonu, Təbriz küçəsi 41", SettingId = 1, LanguageId = 2 },
            new SettingDetail { Id = 3, Value = "Oblast Nərimanov, ulice Tabríz 41", SettingId = 1, LanguageId = 3 },

            new SettingDetail { Id = 4, Value = "*4141", SettingId = 2, LanguageId = 1 },
            new SettingDetail { Id = 5, Value = "*4141", SettingId = 2, LanguageId = 2 },
            new SettingDetail { Id = 6, Value = "*4141", SettingId = 2, LanguageId = 3 },

            new SettingDetail { Id = 7, Value = "https://www.instagram.com/", SettingId = 3, LanguageId = 1 },
            new SettingDetail { Id = 8, Value = "https://www.instagram.com/", SettingId = 3, LanguageId = 2 },
            new SettingDetail { Id = 9, Value = "https://www.instagram.com/", SettingId = 3, LanguageId = 3 },


            new SettingDetail { Id = 10, Value = "https://www.facebook.com/", SettingId = 4, LanguageId = 1 },
            new SettingDetail { Id = 11, Value = "https://www.facebook.com/", SettingId = 4, LanguageId = 2 },
            new SettingDetail { Id = 12, Value = "https://www.facebook.com/", SettingId = 4, LanguageId = 3 },


            new SettingDetail { Id = 13, Value = "https://x.com/", SettingId = 5, LanguageId = 1 },
            new SettingDetail { Id = 14, Value = "https://x.com/", SettingId = 5, LanguageId = 2 },
            new SettingDetail { Id = 15, Value = "https://x.com/", SettingId = 5, LanguageId = 3 },


            new SettingDetail { Id = 16, Value = "https://www.youtube.com/", SettingId = 6, LanguageId = 1 },
            new SettingDetail { Id = 17, Value = "https://www.youtube.com/", SettingId = 6, LanguageId = 2 },
            new SettingDetail { Id = 18, Value = "https://www.youtube.com/", SettingId = 6, LanguageId = 3 },


            new SettingDetail { Id = 19, Value = "bookit.aze@gmail.com", SettingId = 7, LanguageId = 1 },
            new SettingDetail { Id = 20, Value = "bookit.aze@gmail.com/", SettingId = 7, LanguageId = 2 },
            new SettingDetail { Id = 21, Value = "bookit.aze@gmail.com/", SettingId = 7, LanguageId = 3 }
            );
    }

    public static void AddNews(this ModelBuilder modelBuilder)
    {
        new News
        {
            Id = 1,
            ImagePath = "https://res.cloudinary.com/di3ourpee/image/upload/v1738786112/bookit./HitMeHardAndSoft.jpg",
            IsDeleted = false,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.MinValue,
            CreatedBy = "Default",
            UpdatedBy = "Default"
        };

        new News
        {
            Id = 2,
            ImagePath = "https://res.cloudinary.com/di3ourpee/image/upload/v1738786345/bookit./KendrickLamar.webp",
            IsDeleted = false,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.MinValue,
            CreatedBy = "Default",
            UpdatedBy = "Default"
        };

        new News
        {
            Id = 3,
            ImagePath = "https://res.cloudinary.com/di3ourpee/image/upload/v1738786457/bookit./paddington3.jpg",
            IsDeleted = false,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.MinValue,
            CreatedBy = "Default",
            UpdatedBy = "Default"
        };

        modelBuilder.Entity<NewsDetail>().HasData(
            new NewsDetail
            {
                Id = 1,
                Title = "Billie Eilish's album 'Hit Me Hard And Soft' has set a new record",
                Description = "Within hours of its release, Hit Me Hard and Soft surged to the top of major streaming platforms, breaking previous records for the fastest-streamed album debut. Industry insiders attribute this rapid success to Eilish’s massive global fanbase and the album's fresh take on modern pop and alternative music. “Billie has a rare ability to reinvent her sound while staying true to her artistic vision,” commented a representative from a leading music analytics firm. “The record-breaking numbers are a testament to her influence and the universal appeal of this album.”\r\n\r\n",
                LanguageId = 1,
                NewsId = 1
            },

            new NewsDetail
            {
                Id = 2,
                Title = "Billie Eilish-in \"Hit Me Hard And Soft\" albomu yeni rekorda imza atıb",
                Description = "Çıxışından bir neçə saat sonra, Hit Me Hard and Soft əsas axın platformlarında zirvəyə çatdı və ən sürətli axın edilən albom debütü üçün əvvəlki rekordları sarsıtdı. Sənaye mütəxəssisləri bu sürətli uğuru Eilish-in qlobal fan bazasının genişliyi və albomun müasir pop və alternativ musiqiyə gətirdiyi təzə yanaşmaya bağlayır. “Billie səsini yenidən kəşf etmək və bədii vizyonuna sadiq qalmaq bacarığı ilə nadir bir istedaddır,” – deyə şərh etdi aparıcı musiqi analitik şirkətinin nümayəndəsi. “Rekord qıran rəqəmlər onun təsir gücünün və albomun universal cazibəsinin canlı sübutudur.",
                LanguageId = 2,
                NewsId = 1
            }
            ,

            new NewsDetail
            {
                Id = 3,
                Title = "Album Billie Eilish 'Hit Me Hard And Soft' vytvořilo nový rekord",
                Description = "Během několika hodin od vydání se album Hit Me Hard and Soft dostalo na vrchol předních streamovacích platforem a překonalo předchozí rekordy nejrychleji streamovaného debutu alba. Odborníci z oboru připisují tento rychlý úspěch masivní globální fanouškovské základně Eilish a svěžímu pohledu alba na moderní pop a alternativní hudbu. „Billie má vzácnou schopnost znovu objevit svůj zvuk a zároveň zůstat věrná své umělecké vizi,“ poznamenal zástupce přední hudební analytické firmy. „Rekordní čísla jsou důkazem jejího vlivu a univerzální přitažlivosti tohoto alba.",
                LanguageId = 3,
                NewsId = 1
            },






            new NewsDetail
            {
                Id = 4,
                Title = "Kendrick Llamar Wins 5 Grammys in a Single Night with Explosive Diss Track for Drake",
                Description = "In a jaw-dropping turn of events, Kendrick Llamar has swept the Grammy Awards by winning five major categories in one night—all thanks to his explosive diss track aimed at rap superstar Drake. The track, which has taken the music world by storm, features razor-sharp lyrics and an electrifying beat that has left critics and fans in awe. Industry insiders are hailing the win as a turning point in the longstanding rivalry between the two rap titans. “Kendrick’s daring approach has redefined the boundaries of hip-hop,” commented a Grammy committee member. The diss track not only resonated powerfully with fans but also set new benchmarks for lyrical prowess and artistic innovation in the genre.",
                LanguageId = 1,
                NewsId = 2
            },

            new NewsDetail
            {
                Id = 5,
                Title = "Kendrick Llamar Drake-ə Hədəf Alan Partlayıcı Diss Treki ilə Bir Gecədə 5 Grammy Qazanır",
                Description = "Şaşırtıcı hadisələrlə dolu bir gecədə, Kendrick Llamar rap super ulduzu Drake-ə qarşı olan partlayıcı diss treki sayəsində beş əsas kateqoriyada Grammy mükafatını qazanaraq böyük uğur əldə etdi. Musiqi dünyasını əsir edən bu parça, kəskin sözləri və elektrikli ritmi ilə tənqidçiləri və pərəstişkarları valeh etdi. Sənaye mütəxəssisləri bu qalibiyyəti iki rap nəhənginin uzun müddətli rəqabətində dönüm nöqtəsi kimi qiymətləndirirlər. “Kendrick-in cəsur yanaşması hip-hop sərhədlərini yenidən müəyyənləşdirdi,” – deyə Grammy komitəsinin üzvü şərh etdi. Diss treki yalnız pərəstişkarların qəlbinə toxunmaqla qalmayıb, həm də janrda söz ustalığı və bədii innovasiya üçün yeni standartlar qoydu.",
                LanguageId = 2,
                NewsId = 2
            },

            new NewsDetail
            {
                Id = 6,
                Title = "Kendrick Llamar získal 5 Grammy jedné noci s explozivním disstrackem namířeným na Drakea",
                Description = "Ve velkolepém obratu událostí si Kendrick Llamar vybojoval Grammy Awards, když během jediné noci získal pět hlavních kategorií – a to díky svému explozivnímu disstracku, který míří na rapovou superhvězdu Drakea. Singl, který otřásl hudebním světem, se vyznačuje ostrými texty a elektrizujícím beatem, který ohromil kritiky i fanoušky. Odborníci z oboru označují tento úspěch za zlomový moment v dlouhotrvající rivalitě mezi těmito dvěma rapovými giganty. „Kendrickův odvážný přístup redefinoval hranice hip-hopu,“ komentoval člen Grammy komise. Disstrack nejenže silně rezonoval s fanoušky, ale také nastavil nové standardy pro lyrickou zdatnost a umělecké inovace v tomto žánru.",
                LanguageId = 3,
                NewsId = 2
            },





            new NewsDetail
            {
                Id = 7,
                Title = "Paddington 3: The Beloved Bear Returns for Another Heartwarming Adventure",
                Description = "Paddington 3 returns this summer with a delightful new adventure that reunites fans with the lovable bear. Blending classic charm with modern twists, the film promises heartwarming moments, clever humor, and plenty of surprises for audiences of all ages.",
                LanguageId = 1,
                NewsId = 3
            },

            new NewsDetail
            {
                Id = 8,
                Title = "Paddington 3: Sevimli Ayıcıq Yenidən Möhtəşəm Macəraya Qayıdır",
                Description = "Paddington 3 bu yay sevilən ayıcığı ilə tamaşaçıları yenidən bir araya gətirən gözəl bir macəra ilə geri dönür. Klassik cazibəni müasir tərzlərlə birləşdirən film, hər yaşdan izləyicilər üçün ürəkaçan anlar, ağıllı yumor və bir çox sürpriz vəd edir.",
                LanguageId = 2,
                NewsId = 3
            },

            new NewsDetail
            {
                Id = 9,
                Title = "Paddington 3: Oblíbený medvídek se vrací s další dojemnou dobrodružnou cestou",
                Description = "Paddington 3 se toto léto vrací s kouzelným novým dobrodružstvím, které opět spojuje fanoušky s milovaným medvídkem. Spojující klasické kouzlo s moderními prvky, film slibuje dojemné chvíle, chytrý humor a spoustu překvapení pro diváky všech věkových kategorií.",
                LanguageId = 3,
                NewsId = 3
            }

            );
    }

}
