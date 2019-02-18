/*CLEAN UP*/

USE JazzMetrics;
GO

DROP TABLE [UserProject];
DROP TABLE [User];
DROP TABLE [UserRole];
DROP TABLE [Language];
DROP TABLE [AppError];
DROP TABLE [ProjectMetricColumnValue];
DROP TABLE [ProjectMetricSnapshot];
DROP TABLE [ProjectMetric];
DROP TABLE [Project];
DROP TABLE [MetricColumn];
DROP TABLE [Metric];
DROP TABLE [MetricType];
DROP TABLE [AffectedField];
DROP TABLE [AspiceProcess];
DROP TABLE [AspiceVersion];

/*
USE [master];
GO
DROP DATABASE JazzMetrics;
GO
*/
