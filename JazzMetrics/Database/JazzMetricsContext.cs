using Database.DAO;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public partial class JazzMetricsContext : DbContext
    {
        public JazzMetricsContext() { }

        public JazzMetricsContext(DbContextOptions<JazzMetricsContext> options) : base(options) { }

        public virtual DbSet<AffectedField> AffectedField { get; set; }
        public virtual DbSet<AppError> AppError { get; set; }
        public virtual DbSet<AspiceProcess> AspiceProcess { get; set; }
        public virtual DbSet<AspiceVersion> AspiceVersion { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<Metric> Metric { get; set; }
        public virtual DbSet<MetricColumn> MetricColumn { get; set; }
        public virtual DbSet<MetricType> MetricType { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectMetric> ProjectMetric { get; set; }
        public virtual DbSet<ProjectMetricColumnValue> ProjectMetricColumnValue { get; set; }
        public virtual DbSet<ProjectMetricSnapshot> ProjectMetricSnapshot { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserProject> UserProject { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseLazyLoadingProxies();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<AffectedField>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<AppError>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AppInfo)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Exception).IsRequired();

                entity.Property(e => e.Function)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.InnerException).IsRequired();

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.Module)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Time).HasColumnType("datetime2(3)");
            });

            modelBuilder.Entity<AspiceProcess>(entity =>
            {
                entity.HasIndex(e => e.Shortcut)
                    .HasName("UNIQUE_SHORTCUT")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AspiceVersionId).HasColumnName("AspiceVersionID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.Shortcut)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasOne(d => d.AspiceVersion)
                    .WithMany(p => p.AspiceProcess)
                    .HasForeignKey(d => d.AspiceVersionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ASPICEPROCESS_ASPICEVERSION");
            });

            modelBuilder.Entity<AspiceVersion>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.ReleaseDate).HasColumnType("date");

                entity.Property(e => e.VersionNumber).HasColumnType("numeric(2, 1)");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Iso6391code)
                    .IsRequired()
                    .HasColumnName("ISO639_1Code")
                    .HasMaxLength(2);

                entity.Property(e => e.Iso6393code)
                    .IsRequired()
                    .HasColumnName("ISO639_3Code")
                    .HasMaxLength(3);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<Metric>(entity =>
            {
                entity.HasIndex(e => e.Identificator)
                    .HasName("UNIQUE_IDENTIFICATOR")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AffectedFieldId).HasColumnName("AffectedFieldID");

                entity.Property(e => e.AspiceProcessId).HasColumnName("AspiceProcessID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Identificator)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.MetricTypeId).HasColumnName("MetricTypeID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.HasOne(d => d.AffectedField)
                    .WithMany(p => p.Metric)
                    .HasForeignKey(d => d.AffectedFieldId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_METRIC_AFFECTEDFIELD");

                entity.HasOne(d => d.AspiceProcess)
                    .WithMany(p => p.Metric)
                    .HasForeignKey(d => d.AspiceProcessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_METRIC_ASPICEPROCESS");

                entity.HasOne(d => d.MetricType)
                    .WithMany(p => p.Metric)
                    .HasForeignKey(d => d.MetricTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_METRIC_METRICTYPE");
            });

            modelBuilder.Entity<MetricColumn>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MetricId).HasColumnName("MetricID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.PairMetricColumnId).HasColumnName("PairMetricColumnID");

                entity.HasOne(d => d.Metric)
                    .WithMany(p => p.MetricColumn)
                    .HasForeignKey(d => d.MetricId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_METRICCOLUMN_METRIC");

                entity.HasOne(d => d.PairMetricColumn)
                    .WithMany(p => p.InversePairMetricColumn)
                    .HasForeignKey(d => d.PairMetricColumnId)
                    .HasConstraintName("FK_METRICCOLUMN_METRICCOLUMN");
            });

            modelBuilder.Entity<MetricType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime2(3)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<ProjectMetric>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime2(3)");

                entity.Property(e => e.DataPassword)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.DataUrl)
                    .IsRequired()
                    .HasColumnName("DataURL");

                entity.Property(e => e.DataUsername)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime2(3)");

                entity.Property(e => e.MetricId).HasColumnName("MetricID");

                entity.Property(e => e.MinimalWarningValue).HasColumnType("numeric(18, 3)");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.HasOne(d => d.Metric)
                    .WithMany(p => p.ProjectMetric)
                    .HasForeignKey(d => d.MetricId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROJECTMETRIC_METRIC");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectMetric)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROJECTMETRIC_PROJECT");
            });

            modelBuilder.Entity<ProjectMetricColumnValue>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MetricColumnId).HasColumnName("MetricColumnID");

                entity.Property(e => e.ProjectMetricSnapshotId).HasColumnName("ProjectMetricSnapshotID");

                entity.Property(e => e.Value).HasColumnType("numeric(18, 3)");

                entity.HasOne(d => d.MetricColumn)
                    .WithMany(p => p.ProjectMetricColumnValue)
                    .HasForeignKey(d => d.MetricColumnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROJECTMETRICVALUE_METRICCOLUMN");

                entity.HasOne(d => d.ProjectMetricSnapshot)
                    .WithMany(p => p.ProjectMetricColumnValue)
                    .HasForeignKey(d => d.ProjectMetricSnapshotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROJECTMETRICVALUE_PROJECTMETRICSNAPSHOT");
            });

            modelBuilder.Entity<ProjectMetricSnapshot>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InsertionDate).HasColumnType("datetime2(3)");

                entity.Property(e => e.ProjectMetricId).HasColumnName("ProjectMetricID");

                entity.HasOne(d => d.ProjectMetric)
                    .WithMany(p => p.ProjectMetricSnapshot)
                    .HasForeignKey(d => d.ProjectMetricId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROJECTMETRICVALUES_PROJECTMETRIC");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.LdapUrl)
                    .HasColumnName("LDAP_URL")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.UseLdaplogin).HasColumnName("UseLDAPLogin");

                entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USER_LANGUAGE");

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.UserRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USER_ROLE");
            });

            modelBuilder.Entity<UserProject>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ProjectId })
                    .HasName("PK__UserProj__00E96741A8458835");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.JoinDate).HasColumnType("datetime2(3)");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.UserProject)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERPROJECT_PROJECT");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserProject)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERPROJECT_USER");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.Name).HasMaxLength(128);
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasIndex(e => new { e.SettingScope, e.SettingName })
                    .HasName("UNIQUE_SETTINGSCOPE_SETTINGNAME")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SettingName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.SettingScope)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Value).IsRequired();
            });
        }
    }
}
