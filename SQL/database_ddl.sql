/*
CREATE DATABASE JazzMetrics;
GO
*/

CREATE TABLE [User]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[FirstName] nvarchar(128) NOT NULL,
	[LastName] nvarchar(128) NOT NULL,
	[Password] nvarchar(1024) NOT NULL,
	[Salt] nvarchar(1024) NOT NULL,
	[Email] nvarchar(128) NOT NULL,
	[UserRoleID] int NOT NULL,
	[LanguageID] int NOT NULL,
	[UseLDAPLogin] bit NOT NULL DEFAULT 0,
	[LDAP_URL] nvarchar(MAX) NULL DEFAULT '',
	PRIMARY KEY ([ID])
)
GO
CREATE TABLE [Language]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[Name] nvarchar(64) NOT NULL,
	[ISO639_1Code] nvarchar(2) NOT NULL,
	[ISO639_3Code] nvarchar(3) NOT NULL,
	PRIMARY KEY ([ID])
)
GO
CREATE TABLE [UserRole]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[Name] nvarchar(128) NULL,
	[Description] nvarchar(512) NOT NULL,
	PRIMARY KEY ([ID])
)
GO
CREATE TABLE [AppError]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[Time] datetime2(3) NOT NULL,
	[Module] nvarchar(128) NOT NULL,
	[Function] nvarchar(128) NOT NULL,
	[Exception] nvarchar(MAX) NOT NULL,
	[InnerException] nvarchar(MAX) NOT NULL,
	[Message] nvarchar(MAX) NOT NULL,
	[Solved] bit NOT NULL DEFAULT 0,
	[Deleted] bit NOT NULL DEFAULT 0,
	[AppInfo] nvarchar(256) NOT NULL DEFAULT '',
	PRIMARY KEY ([ID])
)
GO
CREATE TABLE [Project]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[Name] nvarchar(256) NOT NULL,
	[Description] nvarchar(MAX) NOT NULL DEFAULT '',
	[CreateDate] datetime2(3) NOT NULL,
	PRIMARY KEY ([ID])
)
GO
CREATE TABLE [UserProject]
(
	[ProjectID] int NOT NULL,
	[UserID] int NOT NULL,
	[JoinDate] datetime2(3) NOT NULL,
	PRIMARY KEY ([UserID], [ProjectID])
)
GO
CREATE TABLE [Metric]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[Identificator] nvarchar(32) NOT NULL,
	[Name] nvarchar(512) NOT NULL,
	[Description] nvarchar(MAX) NOT NULL,
	[MetricTypeID] int NOT NULL,
	[AspiceProcessID] int NOT NULL,
	[AffectedFieldID] int NOT NULL,
	PRIMARY KEY ([ID]) ,
	CONSTRAINT [UNIQUE_IDENTIFICATOR] UNIQUE ([Identificator])
)
GO
CREATE TABLE [ProjectMetric]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[ProjectID] int NOT NULL,
	[MetricID] int NOT NULL,
	[CreateDate] datetime2(3) NOT NULL,
	[LastUpdateDate] datetime2(3) NOT NULL,
	[DataURL] nvarchar(MAX) NOT NULL,
	[Warning] bit NOT NULL DEFAULT 0,
	[MinimalWarningValue] numeric(18,3) NULL,
	PRIMARY KEY ([ID])
)
GO
CREATE TABLE [ProjectMetricValue]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[InsertionDate] datetime2(3) NOT NULL,
	[ProjectMetricID] int NOT NULL,
	[FirstValue] int NULL,
	[SecondValue] int NULL,
	[Ratio] numeric(18,3) NULL,
	[Values] nvarchar(MAX) NULL,
	PRIMARY KEY ([ID])
)
GO
CREATE TABLE [MetricType]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[Name] nvarchar(256) NOT NULL,
	[Description] nvarchar(MAX) NOT NULL,
	PRIMARY KEY ([ID])
)
GO
CREATE TABLE [AspiceProcess]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[Shortcut] nvarchar(64) NOT NULL,
	[Name] nvarchar(512) NOT NULL,
	[Description] nvarchar(MAX) NOT NULL,
	[AspiceVersionID] int NOT NULL,
	PRIMARY KEY ([ID]) ,
	CONSTRAINT [UNIQUE_SHORTCUT] UNIQUE ([Shortcut] ASC)
)
GO
CREATE TABLE [AffectedField]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[Name] nvarchar(128) NOT NULL,
	[Description] nvarchar(MAX) NULL,
	PRIMARY KEY ([ID])
)
GO
CREATE TABLE [AspiceVersion]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[VersionNumber] numeric(2,1) NOT NULL,
	[ReleaseDate] date NOT NULL,
	[Description] nvarchar(MAX) NOT NULL,
	PRIMARY KEY ([ID])
)
GO

ALTER TABLE [User] ADD CONSTRAINT [FK_USER_LANGUAGE] FOREIGN KEY ([LanguageID]) REFERENCES [Language] ([ID])
GO
ALTER TABLE [User] ADD CONSTRAINT [FK_USER_ROLE] FOREIGN KEY ([UserRoleID]) REFERENCES [UserRole] ([ID])
GO
ALTER TABLE [UserProject] ADD CONSTRAINT [FK_USERPROJECT_USER] FOREIGN KEY ([UserID]) REFERENCES [User] ([ID])
GO
ALTER TABLE [UserProject] ADD CONSTRAINT [FK_USERPROJECT_PROJECT] FOREIGN KEY ([ProjectID]) REFERENCES [Project] ([ID])
GO
ALTER TABLE [ProjectMetric] ADD CONSTRAINT [FK_PROJECTMETRIC_PROJECT] FOREIGN KEY ([ProjectID]) REFERENCES [Project] ([ID])
GO
ALTER TABLE [ProjectMetric] ADD CONSTRAINT [FK_PROJECTMETRIC_METRIC] FOREIGN KEY ([MetricID]) REFERENCES [Metric] ([ID])
GO
ALTER TABLE [ProjectMetricValue] ADD CONSTRAINT [FK_PROJECTMETRICVALUES_PROJECTMETRIC] FOREIGN KEY ([ProjectMetricID]) REFERENCES [ProjectMetric] ([ID])
GO
ALTER TABLE [Metric] ADD CONSTRAINT [FK_METRIC_METRICTYPE] FOREIGN KEY ([MetricTypeID]) REFERENCES [MetricType] ([ID])
GO
ALTER TABLE [Metric] ADD CONSTRAINT [FK_METRIC_ASPICEPROCESS] FOREIGN KEY ([AspiceProcessID]) REFERENCES [AspiceProcess] ([ID])
GO
ALTER TABLE [Metric] ADD CONSTRAINT [FK_METRIC_AFFECTEDFIELD] FOREIGN KEY ([AffectedFieldID]) REFERENCES [AffectedField] ([ID])
GO
ALTER TABLE [AspiceProcess] ADD CONSTRAINT [FK_ASPICEPROCESS_ASPICEVERSION] FOREIGN KEY ([AspiceVersionID]) REFERENCES [AspiceVersion] ([ID])
GO

