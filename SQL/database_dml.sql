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
    ('Franta', 'Novak', 'user2', 'novak@gmail.com', 'E1-6D-06-27-BD-E5-80-61-91-A9-B0-00-87-BD-C1-97', 'ExwjSI4MXC', 3, 2, 0, 1)
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
INSERT INTO [Setting] ([SettingScope], [SettingName], [Value]) VALUES ('Job', 'MetricUpdateMinutes', '1440')

INSERT INTO [Project] ([Name], [Description], [CreateDate]) VALUES ('Master thesis', 'Some type of master thesis.', GETDATE())

INSERT INTO [ProjectUser] ([UserID], [ProjectID], [JoinDate]) VALUES (2, 1, GETDATE())
INSERT INTO [ProjectUser] ([UserID], [ProjectID], [JoinDate]) VALUES (3, 1, GETDATE())
INSERT INTO [ProjectUser] ([UserID], [ProjectID], [JoinDate]) VALUES (4, 1, GETDATE())
INSERT INTO [ProjectUser] ([UserID], [ProjectID], [JoinDate]) VALUES (5, 1, GETDATE())

INSERT INTO [Metric] ([Identificator], [RequirementGroup], [Name], [Description], [MetricTypeID], [AspiceProcessID], [AffectedFieldID], [CompanyId], [Public]) VALUES 
    ('M63', 'HWRS', 'HWRS Statuses', 'Number of HWRS Requirements Statuses

Arguments:
• Project
• Collection or Module – All HWRS Modules

Purpose:
The metrics provides information how many HWRS Requirements have no status or are  in Statuses  - Under construction., Ready to review, Reviewed. Implemented, Tested', 1, 9, 1, 1, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('Under construction', 'LITERAL_NAME', 'LITERAL_NAME1', null, null, null, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('Ready to review', 'LITERAL_NAME', 'LITERAL_NAME1', null, null, null, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('Reviewed', 'LITERAL_NAME', 'LITERAL_NAME1', null, null, null, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('Implemented', 'LITERAL_NAME', 'LITERAL_NAME1', null, null, null, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('Tested', 'LITERAL_NAME', 'LITERAL_NAME1', null, null, null, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('', 'LITERAL_NAME', 'LITERAL_NAME1', null, null, null, 1)

INSERT INTO [Metric] ([Identificator], [RequirementGroup], [Name], [Description], [MetricTypeID], [AspiceProcessID], [AffectedFieldID], [CompanyId], [Public]) VALUES 
    ('M85', 'HWRS', 'HWRS to SYAR, SYRS Traceability', '--', 2, 9, 1, 1, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('*any*', 'REFERENCE_ID1', null, '*all*', '', 'Coverage', 2)

INSERT INTO [Metric] ([Identificator], [RequirementGroup], [Name], [Description], [MetricTypeID], [AspiceProcessID], [AffectedFieldID], [CompanyId], [Public]) VALUES 
    ('M59', 'HWRS', 'No. of HW Requirements reviewed', 'Number of HWRS Requirements with status Reviewed

Arguments:
• Project
• Collection or Module – All HWRS Modules

Purpose:
The metrics provides information how many HW requirements are in the status reviewed or further – how many HW requirements were reviewed.', 1, 9, 3, 1, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('*any*', 'LITERAL_NAME', 'REFERENCE_ID', null, null, null, 3)

INSERT INTO [Metric] ([Identificator], [RequirementGroup], [Name], [Description], [MetricTypeID], [AspiceProcessID], [AffectedFieldID], [CompanyId], [Public]) VALUES 
    ('M28', 'SWRS', 'No. of Software Requirements - Statuses', 'Number of SWRS with no Status or Status Under Construction, Ready to Review, Reviewed, Implemented, Tested

Arguments:
• Project
• Collection or Module – All SWRS Modules

Purpose:
Number of software  requirements – statuses shows how many software requirements are in the state of: no Status or Status Under Construction, Ready to Review, Reviewed, Implemented, Tested', 1, 6, 1, 1, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('Under Construction;Ready to Review;Reviewed;Implemented;Tested', 'LITERAL_NAME', 'LITERAL_NAME1', null, null, null, 4)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('', 'LITERAL_NAME', 'LITERAL_NAME1', null, null, null, 4)

INSERT INTO [Metric] ([Identificator], [RequirementGroup], [Name], [Description], [MetricTypeID], [AspiceProcessID], [AffectedFieldID], [CompanyId], [Public]) VALUES 
    ('M35', 'SWRS', 'No. of SW requirements reviewed', 'Number of SWRS Requirements with status Reviewed

Arguments:
• Project
• Collection or Module – All HWRS Modules

Purpose:
The metrics provides information how many SW requirements are in the status reviewed or further – how many SW requirements were reviewed.', 1, 6, 3, 1, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('*any*', 'LITERAL_NAME', 'REFERENCE_ID', null, null, null, 5)

INSERT INTO [Metric] ([Identificator], [RequirementGroup], [Name], [Description], [MetricTypeID], [AspiceProcessID], [AffectedFieldID], [CompanyId], [Public]) VALUES 
    ('M27', 'SYRS', 'No. of System Requirements - Statuses', 'Number of SYRS with no Status or Status Under Construction, Ready to Review, Reviewed, Implemented, Tested

Arguments:
• Project
• Collection or Module – All SYRS Modules

Purpose:
Number of system requirements – statuses shows how many system requirements are in the state of: no Status or Status Under Construction, Ready to Review, Reviewed, Implemented, Tested', 1, 2, 1, 1, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('Under Construction;Ready to Review;Reviewed;Implemented;Tested', 'LITERAL_NAME', 'LITERAL_NAME1', null, null, null, 6)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('', 'LITERAL_NAME', 'LITERAL_NAME1', null, null, null, 6)

INSERT INTO [Metric] ([Identificator], [RequirementGroup], [Name], [Description], [MetricTypeID], [AspiceProcessID], [AffectedFieldID], [CompanyId], [Public]) VALUES 
    ('M56', 'SYRS', 'No. of System requirements linked to System test cases', '--', 1, 1, 1, 1, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('*any*', 'REFERENCE_ID1', 'LITERAL_NAME1', null, null, null, 7)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('', 'REFERENCE_ID1', 'LITERAL_NAME1', null, null, null, 7)

INSERT INTO [Metric] ([Identificator], [RequirementGroup], [Name], [Description], [MetricTypeID], [AspiceProcessID], [AffectedFieldID], [CompanyId], [Public]) VALUES 
    ('M03', 'SYRS', 'Review Coverage: System Reqs.', 'Ratio of number of all SYRS Requirements to number of SYRS Requirements with Status Reviewed, Implemented or Tested

Arguments:
• Project
• Collection or Module – All SYRS Modules

Purpose:
Review coverage system requirements shows how many of the system requirements were reviewed.', 2, 2, 3, 1, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('Reviewed;Implemented;Tested', 'LITERAL_NAME', null, 'SYRS', 'NAME', 'Coverage', 8)

INSERT INTO [Metric] ([Identificator], [RequirementGroup], [Name], [Description], [MetricTypeID], [AspiceProcessID], [AffectedFieldID], [CompanyId], [Public]) VALUES 
    ('M06', 'SWRS', 'Review Coverage: SW Reqs.', 'Ratio of number of all SWRS Requirements to number of SWRS Requirements with Status Reviewed, Implemented or Tested

Arguments:
• Project
• Collection or Module – All SWRS Modules

Purpose:
Review coverage software requirements shows percentage how many of the software requirements were reviewed.', 2, 6, 3, 1, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('Reviewed;Implemented;Tested', 'LITERAL_NAME', null, 'SWRS', 'NAME', 'Coverage', 9)

INSERT INTO [Metric] ([Identificator], [RequirementGroup], [Name], [Description], [MetricTypeID], [AspiceProcessID], [AffectedFieldID], [CompanyId], [Public]) VALUES 
    ('M60', 'HWRS', 'Review Coverage: HW Reqs.', 'Ratio of number of all HWRS Requirements to number of HWRS Requirements with Status Reviewed, Implemented or Tested

Arguments:
• Project
• Collection or Module – All HWRS Modules

Purpose:
Review coverage software requirements shows percentage how many of the software requirements were reviewed.', 2, 9, 3, 1, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('Reviewed;Implemented;Tested', 'LITERAL_NAME', null, 'HWRS', 'NAME', 'Coverage', 10)

INSERT INTO [Metric] ([Identificator], [RequirementGroup], [Name], [Description], [MetricTypeID], [AspiceProcessID], [AffectedFieldID], [CompanyId], [Public]) VALUES 
    ('M75', 'SWRS', 'SW Integration Test Case Coverage', 'SWRS “Interface Requirements” vs SWRS Interface Requirements with Tested by filled

Arguments:
• Project
• Collection or Module – All SWRS Modules

Purpose:', 3, 7, 2, 1, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('*any*', 'REFERENCE_ID1', null, '*all*', '', 'Coverage', 11)

INSERT INTO [Metric] ([Identificator], [RequirementGroup], [Name], [Description], [MetricTypeID], [AspiceProcessID], [AffectedFieldID], [CompanyId], [Public]) VALUES 
    ('M65', 'SYAR', 'Review coverage for SYAR', 'Ratio of SYAR Requirement and Interface Requirement to number of Requirement and Interface Requirement with Status “Reviewed” and higher.

Arguments:
• Project
• Collection or Module – All SYAR Modules

Purpose:
The metrics shows coverage of Statuses “Reviewed” and higher in SYAR Requirements and Interface Requirements.', 2, 3, 3, 1, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('Reviewed;Implemented;Tested', 'LITERAL_NAME', null, '*all*', '', 'Coverage', 12)

INSERT INTO [Metric] ([Identificator], [RequirementGroup], [Name], [Description], [MetricTypeID], [AspiceProcessID], [AffectedFieldID], [CompanyId], [Public]) VALUES 
    ('M66', 'SYAR', 'SYAR Statuses', 'Number of SYAR Requirements and Interface Requirements in Statuses

Arguments:
• Project
• Collection or Module – All SYAR Modules

Purpose:
The metrics provides information how many SYAR Requirements and Interface Requirements have no status or are in Statuses - Under construction, Ready to review, Reviewed, Implemented, Tested.', 1, 3, 1, 1, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('Under Construction;Ready to Review;Reviewed;Implemented;Tested', 'LITERAL_NAME', 'LITERAL_NAME1', null, null, null, 13)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('', 'LITERAL_NAME', 'LITERAL_NAME1', null, null, null, 13)

INSERT INTO [Metric] ([Identificator], [RequirementGroup], [Name], [Description], [MetricTypeID], [AspiceProcessID], [AffectedFieldID], [CompanyId], [Public]) VALUES 
    ('M78', 'SYAR', 'SYAR to SYRS Traceability', 'Percentage of total SYAR Requirement versus Requirements that have an uplink to SYRS

Arguments:
• Project
• Collection or Module – All SYAS Modules
• Type: Requirement
• Variant (M78V)

Purpose:
The metrics shows percentage of total SYAR Requirement versus Requirements that have an uplink to SYRS.', 2, 3, 2, 1, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('*any*', 'REFERENCE_ID1', null, '*all*', '', 'Coverage', 14)

INSERT INTO [Metric] ([Identificator], [RequirementGroup], [Name], [Description], [MetricTypeID], [AspiceProcessID], [AffectedFieldID], [CompanyId], [Public]) VALUES 
    ('M26', 'SYRS', 'System Requirements Test Case Coverage', '--', 2, 2, 3, 1, 1)
INSERT INTO [dbo].[MetricColumn] ([Value], [FieldName], [NumberFieldName], [DivisorValue], [DivisorFieldName], [CoverageName], [MetricID]) VALUES
    ('*any*', 'REFERENCE_ID1', null, '*all*', '', 'Coverage', 15)
