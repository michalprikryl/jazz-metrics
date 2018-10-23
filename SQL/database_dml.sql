USE JazzMetrics
go

/*USE pri0115;
go*/

INSERT INTO [Role] ([Name]) VALUES ('admin')

INSERT INTO [Language] ([ISO639_1Code], [ISO639_3Code], [Name]) VALUES ('cs', 'cze', 'Czech')
INSERT INTO [Language] ([ISO639_1Code], [ISO639_3Code], [Name]) VALUES ('en', 'eng', 'English')

INSERT INTO [User] ([Name], LastName, Email, [Password], [Salt], [RoleID], LanguageID) VALUES ('Franta', 'Novak', 'novak@gmail.com', 'E1-6D-06-27-BD-E5-80-61-91-A9-B0-00-87-BD-C1-97', 'ExwjSI4MXC', 1, 1)