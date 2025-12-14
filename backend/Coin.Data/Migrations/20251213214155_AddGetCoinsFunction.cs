using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coin.Data.Migrations
{
    /// <inheritdoc />
    /// <summary>
    /// Миграция для добавления SQL функции GetCoins
    /// Функция агрегирует данные курсов монет по временным интервалам
    /// </summary>
    public partial class AddGetCoinsFunction : Migration
    {
        /// <inheritdoc />
        /// <summary>
        /// Создание SQL функции GetCoins в базе данных
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE FUNCTION dbo.GetCoins
                (
                    @id INT,
                    @stepTime INT
                )
                RETURNS @res TABLE
                (
                    Id INT,
                    CoinId INT,
                    [Time] DATETIME2(7),
                    VolumeTraded REAL,
                    Prices REAL
                )
                AS
                BEGIN
                    DECLARE @startDate datetime2, @endDate datetime2;

                    SELECT @startDate = MIN([Time]) FROM CoinRates
                        WHERE [CoinId] = @id

                    SELECT @endDate = MAX([Time]) FROM CoinRates
                        WHERE [CoinId] = @id

                    DECLARE @startTime datetime2 = @startDate;
                    DECLARE @endTime datetime2 = DATEADD(hour, @stepTime, @startTime);
                    DECLARE @number INT = 0;
                    DECLARE @testTime datetime2;

                    WHILE @endTime <= @endDate
                    BEGIN
                        SET @startTime = DATEADD(hour, @stepTime, @startTime);
                        SET @endTime = DATEADD(hour, @stepTime, @endTime);
                        SELECT @testTime = MIN([Time]) FROM CoinRates
                            WHERE [CoinId] = @id
                            AND [Time] BETWEEN @startTime AND @endTime

                        IF (@testTime IS NOT NULL)
                        BEGIN
                            SET @number = @number + 1;
                            INSERT INTO @res
                            SELECT
                                @number,
                                @id,
                                MIN([Time]),
                                AVG([VolumeTraded]),
                                AVG([Prices])
                            FROM CoinRates
                            WHERE [CoinId] = @id
                            AND [Time] BETWEEN @startTime AND @endTime
                        END
                    END
                    RETURN
                END
            ");
        }

        /// <inheritdoc />
        /// <summary>
        /// Удаление SQL функции GetCoins из базы данных
        /// </summary>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS dbo.GetCoins");
        }
    }
}
