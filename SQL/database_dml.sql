/*
USE JazzMetrics
GO
*/
/*
USE pri0115;
GO
*/

INSERT INTO [UserRole] ([Name], [Description]) VALUES ('super-admin', 'Administrator of the application, he has full rights - he can do everything.')
INSERT INTO [UserRole] ([Name], [Description]) VALUES ('admin', 'Administrator of projects, he can create projects, users etc.')
INSERT INTO [UserRole] ([Name], [Description]) VALUES ('user', 'Regular user, he can only display metrics.')

INSERT INTO [Language] ([ISO639_1Code], [ISO639_3Code], [Name]) VALUES ('cs', 'cze', 'Czech')
INSERT INTO [Language] ([ISO639_1Code], [ISO639_3Code], [Name]) VALUES ('en', 'eng', 'English')

INSERT INTO [Company] ([Name]) VALUES ('VŠB')

INSERT INTO [User] ([FirstName], LastName, [Username], Email, [Password], [Salt], [UserRoleID], LanguageID, UseLDAPLogin) VALUES 
    ('Franta', 'Novak', 'user', 'novak@gmail.com', 'E1-6D-06-27-BD-E5-80-61-91-A9-B0-00-87-BD-C1-97', 'ExwjSI4MXC', 3, 2, 0)
INSERT INTO [User] ([FirstName], LastName, [Username], Email, [Password], [Salt], [UserRoleID], LanguageID, UseLDAPLogin, CompanyId) VALUES 
    ('Franta', 'Novak', 'admin', 'novak@gmail.com', 'E1-6D-06-27-BD-E5-80-61-91-A9-B0-00-87-BD-C1-97', 'ExwjSI4MXC', 2, 2, 0, 1)
INSERT INTO [User] ([FirstName], LastName, [Username], Email, [Password], [Salt], [UserRoleID], LanguageID, UseLDAPLogin, CompanyId) VALUES 
    ('Franta', 'Novak', 'superadmin', 'novak@gmail.com', 'E1-6D-06-27-BD-E5-80-61-91-A9-B0-00-87-BD-C1-97', 'ExwjSI4MXC', 1, 1, 0, 1)
INSERT INTO [User] ([FirstName], LastName, [Username], Email, [Password], [Salt], [UserRoleID], LanguageID, UseLDAPLogin, CompanyId) VALUES 
    ('Michal', 'Prikryl', 'm.p.from.h@seznam.cz', 'm.p.from.h@seznam.cz', 'E1-6D-06-27-BD-E5-80-61-91-A9-B0-00-87-BD-C1-97', 'ExwjSI4MXC', 1, 1, 0, 1)

INSERT INTO [MetricType] ([Name], [Description]) VALUES ('Number', 'Metric values are numbers of value/type groups of artefact.')
INSERT INTO [MetricType] ([Name], [Description]) VALUES ('Coverage', 'Metric is percentage - coverage of some artefact with another artefact etc.')
INSERT INTO [MetricType] ([Name], [Description]) VALUES ('Coverage (test)', 'Metric is percentage - coverage of some artefact with test case or if artefact was tested etc.')

INSERT INTO [AffectedField] ([Name], [Description]) VALUES ('PM', 'This metric affected field provides project management status.')
INSERT INTO [AffectedField] ([Name], [Description]) VALUES ('Traceability', 'This metric affected field provides tracebility status.')
INSERT INTO [AffectedField] ([Name], [Description]) VALUES ('Quality', 'This metric affected field provides quality assurance status.')

INSERT INTO [AspiceVersion] ([VersionNumber], [ReleaseDate], [Description]) VALUES ('3.1', '2017-11-01', 'Automotive SPICE 3.1')

INSERT INTO [AspiceProcess] ([Shortcut], [Name], [Description], [AspiceVersionID]) VALUES ('SYS.1', 'REQUIREMENTS ELICITATION', '', 1)
INSERT INTO [AspiceProcess] ([Shortcut], [Name], [Description], [AspiceVersionID]) VALUES ('SYS.2', 'SYSTEM REQUIREMENTS ANALYSIS', '', 1)
INSERT INTO [AspiceProcess] ([Shortcut], [Name], [Description], [AspiceVersionID]) VALUES ('SYS.3', 'SYSTEM ARCHITECTURAL DESIGN', '', 1)
INSERT INTO [AspiceProcess] ([Shortcut], [Name], [Description], [AspiceVersionID]) VALUES ('SYS.4', 'SYSTEM INTERGRATION AND INTEGRAGTION TEST', '', 1)
INSERT INTO [AspiceProcess] ([Shortcut], [Name], [Description], [AspiceVersionID]) VALUES ('SYS.5', 'SYSTEM QUALIFICATION TEST', '', 1)
INSERT INTO [AspiceProcess] ([Shortcut], [Name], [Description], [AspiceVersionID]) VALUES ('SWE.1', 'SOFTWARE REQUIREMENTS ANALYSIS', '', 1)
INSERT INTO [AspiceProcess] ([Shortcut], [Name], [Description], [AspiceVersionID]) VALUES ('SWE.2', 'SOFTWARE ARCHITECTURAL DESIGN', '', 1)
INSERT INTO [AspiceProcess] ([Shortcut], [Name], [Description], [AspiceVersionID]) VALUES ('SWE.6', 'SOFTWARE QUALIFICATION TEST', '', 1)
INSERT INTO [AspiceProcess] ([Shortcut], [Name], [Description], [AspiceVersionID]) VALUES ('HW', 'HARDWARE PROCESSES', '', 1)

INSERT INTO [Setting] ([SettingScope], [SettingName], [Value]) VALUES ('EmailSetting', 'Sender', 'michal.prikryl.st2@vsb.cz')
INSERT INTO [Setting] ([SettingScope], [SettingName], [Value]) VALUES ('EmailSetting', 'Host', 'smtp.vsb.cz')
INSERT INTO [Setting] ([SettingScope], [SettingName], [Value]) VALUES ('EmailSetting', 'Port', '25')
INSERT INTO [Setting] ([SettingScope], [SettingName], [Value]) VALUES ('EmailSetting', 'Username', '')
INSERT INTO [Setting] ([SettingScope], [SettingName], [Value]) VALUES ('EmailSetting', 'Password', '')
INSERT INTO [Setting] ([SettingScope], [SettingName], [Value]) VALUES ('ErrorEmail', 'Email', 'michal.prikryl@post.cz')
INSERT INTO [Setting] ([SettingScope], [SettingName], [Value]) VALUES ('TokenExpiration', 'TokenExpirationMinutes', '1440')

INSERT INTO [Project] ([Name], [Description], [CreateDate]) VALUES ('Master thesis', 'Some type of master thesis.', GETDATE())

INSERT INTO [ProjectUser] ([UserID], [ProjectID], [JoinDate]) VALUES (2, 1, GETDATE())
INSERT INTO [ProjectUser] ([UserID], [ProjectID], [JoinDate]) VALUES (3, 1, GETDATE())
INSERT INTO [ProjectUser] ([UserID], [ProjectID], [JoinDate]) VALUES (4, 1, GETDATE())