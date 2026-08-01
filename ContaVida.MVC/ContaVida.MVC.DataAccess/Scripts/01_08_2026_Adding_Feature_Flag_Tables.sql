
BEGIN TRY

	BEGIN TRANSACTION
	IF OBJECT_ID('dbo.FeatureFlags', 'U') IS NULL
	BEGIN
		CREATE TABLE dbo.FeatureFlags (
			Id uniqueidentifier not null primary key,
			Name nvarchar (256) not null,
			CreationDate datetime not null,
			Description nvarchar(1024) not null,
			UserCreatedId uniqueidentifier not null  FOREIGN KEY  (UserCreatedId) REFERENCES Users(Id), 
			Value bit not null

		)
	END
    ELSE
        BEGIN
            PRINT 'FeatureFlags table already exists on dbo'
        END
    


	IF OBJECT_ID('dbo.FeatureFlagHistory', 'U') IS NULL
	BEGIN
		CREATE TABLE dbo.FeatureFlagHistory
		(
			Id uniqueidentifier primary key,
			FeatureFlagID uniqueidentifier FOREIGN KEY  (FeatureFlagID) REFERENCES dbo.FeatureFlags(Id), 
			NewValue bit not null,
			CreationDate datetime not null,
			UserChangedId uniqueidentifier not null  FOREIGN KEY  (UserChangedId) REFERENCES Users(Id), 
			EventType nvarchar(32) not null
		)
	END
    ELSE
        BEGIN
            PRINT 'FeatureFlagHistory table already exists on dbo'
        END
	

	COMMIT TRANSACTION
    PRINT 'TRANSACTION COMPLETED FOR FEATURE FLAG TABLES'


END TRY
BEGIN CATCH 
	IF @@TRANCOUNT > 0
    PRINT 'TRANSACTION FAILED FOR FEATURE FLAG TABLES, REVERTING CHANGES'
	ROLLBACK TRANSACTION
	THROW

END CATCH

GO
IF OBJECT_ID('dbo.TR_FeatureFlags_Insert', 'TR') IS NULL
BEGIN
    EXEC('
        CREATE TRIGGER dbo.TR_FeatureFlags_Insert
        ON dbo.FeatureFlags
        AFTER INSERT
        AS
        BEGIN
            SET NOCOUNT ON;

            INSERT INTO dbo.FeatureFlagHistory
            (
                Id,
                FeatureFlagID,
                NewValue,
                CreationDate,
                UserChangedId,
                EventType
            )
            SELECT
                NEWID(),
                i.Id,
                i.Value,
                GETDATE(),
                i.UserCreatedId,
                ''ADDED''
            FROM inserted AS i;
        END;
    ');
END
  ELSE
        BEGIN
            PRINT 'TR_FeatureFlags_Insert already exists on dbo'
        END

IF OBJECT_ID('dbo.TR_FeatureFlags_Update', 'TR') IS NULL
BEGIN
    EXEC('
        CREATE TRIGGER dbo.TR_FeatureFlags_Update
        ON dbo.FeatureFlags
        AFTER UPDATE
        AS
        BEGIN
            SET NOCOUNT ON;

            INSERT INTO dbo.FeatureFlagHistory
            (
                Id,
                FeatureFlagID,
                NewValue,
                CreationDate,
                UserChangedId,
                EventType
            )
            SELECT
                NEWID(),
                i.Id,
                i.Value,
                GETDATE(),
                i.UserCreatedId,
                ''MODIFIED''
            FROM inserted AS i
            INNER JOIN deleted AS d
                ON i.Id = d.Id
            WHERE i.Value <> d.Value;
        END;
    ');
END
  ELSE
        BEGIN
            PRINT 'TR_FeatureFlags_Update already exists on dbo'
        END