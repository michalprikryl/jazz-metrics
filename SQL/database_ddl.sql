CREATE TABLE [User]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[FirstName] nvarchar(128) NOT NULL,
	[LastName] nvarchar(128) NOT NULL,
	[Username] nvarchar(128) NOT NULL,
	[Password] nvarchar(1024) NOT NULL,
	[Salt] nvarchar(1024) NOT NULL,
	[Email] nvarchar(128) NOT NULL,
	[UserRoleID] int NOT NULL,
	[LanguageID] int NOT NULL,
	[UseLDAPLogin] bit NOT NULL DEFAULT 0,
	[LDAP_URL] nvarchar(MAX) NULL DEFAULT '',
	[CompanyID] int NULL,
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
CREATE TABLE [ProjectUser]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[ProjectID] int NOT NULL,
	[UserID] int NOT NULL,
	[JoinDate] datetime2(3) NOT NULL,
	PRIMARY KEY ([ID])
)
GO
CREATE TABLE [Metric]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[Identificator] nvarchar(32) NOT NULL,
	[Name] nvarchar(512) NOT NULL,
	[Description] nvarchar(MAX) NOT NULL,
	[RequirementGroup] nvarchar(128) NOT NULL,
	[MetricTypeID] int NOT NULL,
	[AspiceProcessID] int NOT NULL,
	[AffectedFieldID] int NOT NULL,
	[CompanyID] int NULL,
	[Public] bit NOT NULL,
	PRIMARY KEY ([ID]) ,
	CONSTRAINT [UNIQUE_IDENTIFICATOR] UNIQUE ([Identificator] ASC, [CompanyID] ASC)
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
	[DataUsername] nvarchar(256) NOT NULL,
	[DataPassword] nvarchar(1024) NOT NULL,
	[Warning] bit NOT NULL DEFAULT 0,
	[MinimalWarningValue] numeric(18,3) NULL,
	PRIMARY KEY ([ID])
)
GO
CREATE TABLE [ProjectMetricSnapshot]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[InsertionDate] datetime2(3) NOT NULL,
	[ProjectMetricID] int NOT NULL,
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
CREATE TABLE [ProjectMetricColumnValue]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[Value] numeric(18,3) NOT NULL,
	[ProjectMetricSnapshotID] int NOT NULL,
	[MetricColumnID] int NOT NULL,
	PRIMARY KEY ([ID])
)
GO
CREATE TABLE [MetricColumn]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[Value] nvarchar(1024) NOT NULL,
	[FieldName] nvarchar(1024) NOT NULL,
	[NumberFieldName] nvarchar(1024) NOT NULL,
	[DivisorValue] nvarchar(1024) NULL,
	[DivisorFieldName] nvarchar(1024) NULL,
	[MetricID] int NOT NULL,
	PRIMARY KEY ([ID])
)
GO
CREATE TABLE [Setting]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[SettingScope] nvarchar(256) NOT NULL,
	[SettingName] nvarchar(256) NOT NULL,
	[Value] nvarchar(MAX) NOT NULL,
	PRIMARY KEY ([ID]) ,
	CONSTRAINT [UNIQUE_SETTINGSCOPE_SETTINGNAME] UNIQUE ([SettingScope] ASC, [SettingName] ASC)
)
GO
CREATE TABLE [Company]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[Name] nvarchar(512) NOT NULL,
	PRIMARY KEY ([ID])
)
GO
CREATE TABLE [ProjectMetricLog]
(
	[ID] int NOT NULL IDENTITY(1,1),
	[Message] nvarchar(MAX) NOT NULL,
	[Warning] bit NOT NULL,
	[ProjectMetricID] int NOT NULL,
	PRIMARY KEY ([ID])
)
GO

ALTER TABLE [User] ADD CONSTRAINT [FK_USER_LANGUAGE] FOREIGN KEY ([LanguageID]) REFERENCES [Language] ([ID])
GO
ALTER TABLE [User] ADD CONSTRAINT [FK_USER_ROLE] FOREIGN KEY ([UserRoleID]) REFERENCES [UserRole] ([ID])
GO
ALTER TABLE [ProjectUser] ADD CONSTRAINT [FK_PROJECTUSER_USER] FOREIGN KEY ([UserID]) REFERENCES [User] ([ID])
GO
ALTER TABLE [ProjectUser] ADD CONSTRAINT [FK_PROJECTUSER_PROJECT] FOREIGN KEY ([ProjectID]) REFERENCES [Project] ([ID])
GO
ALTER TABLE [ProjectMetric] ADD CONSTRAINT [FK_PROJECTMETRIC_PROJECT] FOREIGN KEY ([ProjectID]) REFERENCES [Project] ([ID])
GO
ALTER TABLE [ProjectMetric] ADD CONSTRAINT [FK_PROJECTMETRIC_METRIC] FOREIGN KEY ([MetricID]) REFERENCES [Metric] ([ID])
GO
ALTER TABLE [ProjectMetricSnapshot] ADD CONSTRAINT [FK_PROJECTMETRICVALUES_PROJECTMETRIC] FOREIGN KEY ([ProjectMetricID]) REFERENCES [ProjectMetric] ([ID])
GO
ALTER TABLE [Metric] ADD CONSTRAINT [FK_METRIC_METRICTYPE] FOREIGN KEY ([MetricTypeID]) REFERENCES [MetricType] ([ID])
GO
ALTER TABLE [Metric] ADD CONSTRAINT [FK_METRIC_ASPICEPROCESS] FOREIGN KEY ([AspiceProcessID]) REFERENCES [AspiceProcess] ([ID])
GO
ALTER TABLE [Metric] ADD CONSTRAINT [FK_METRIC_AFFECTEDFIELD] FOREIGN KEY ([AffectedFieldID]) REFERENCES [AffectedField] ([ID])
GO
ALTER TABLE [AspiceProcess] ADD CONSTRAINT [FK_ASPICEPROCESS_ASPICEVERSION] FOREIGN KEY ([AspiceVersionID]) REFERENCES [AspiceVersion] ([ID])
GO
ALTER TABLE [ProjectMetricColumnValue] ADD CONSTRAINT [FK_PROJECTMETRICVALUE_PROJECTMETRICSNAPSHOT] FOREIGN KEY ([ProjectMetricSnapshotID]) REFERENCES [ProjectMetricSnapshot] ([ID])
GO
ALTER TABLE [MetricColumn] ADD CONSTRAINT [FK_METRICCOLUMN_METRIC] FOREIGN KEY ([MetricID]) REFERENCES [Metric] ([ID])
GO
ALTER TABLE [ProjectMetricColumnValue] ADD CONSTRAINT [FK_PROJECTMETRICVALUE_METRICCOLUMN] FOREIGN KEY ([MetricColumnID]) REFERENCES [MetricColumn] ([ID])
GO
ALTER TABLE [User] ADD CONSTRAINT [FK_USER_COMPANY] FOREIGN KEY ([CompanyID]) REFERENCES [Company] ([ID])
GO
ALTER TABLE [Metric] ADD CONSTRAINT [FK_METRIC_COMPANY] FOREIGN KEY ([CompanyID]) REFERENCES [Company] ([ID])
GO
ALTER TABLE [ProjectMetricLog] ADD CONSTRAINT [FK_PROJECTMETRICLOG_PROJECTMETRIC] FOREIGN KEY ([ProjectMetricID]) REFERENCES [ProjectMetric] ([ID])
GO
