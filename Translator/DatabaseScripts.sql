CREATE DATABASE TranslatedApp;
GO

USE TranslatedApp;
GO

CREATE TABLE Translations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserInput NVARCHAR(MAX) NOT NULL,
    "Language" NVARCHAR(100) NOT NULL,
    TranslatedText NVARCHAR(MAX) NOT NULL
);
GO

CREATE PROCEDURE [dbo].[SearchTranslations]
    @UserInput NVARCHAR(255) = NULL,
    @Language NVARCHAR(50) = NULL,
    @TranslatedText NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Translations
    WHERE
        (@UserInput IS NULL OR UserInput LIKE '%' + @UserInput + '%') AND
        (@Language IS NULL OR "Language" LIKE '%' + @Language + '%') AND
        (@TranslatedText IS NULL OR TranslatedText LIKE '%' + @TranslatedText + '%')
END
GO