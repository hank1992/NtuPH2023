using Microsoft.EntityFrameworkCore;
using NtuPH2023.Models;

namespace NtuPH2023.Data
{
    public partial class NtuPH2023Context : DbContext
    {
        private readonly IHttpContextAccessor _httpContext;

        public NtuPH2023Context()
        {
        }

        public NtuPH2023Context(DbContextOptions<NtuPH2023Context> options, IHttpContextAccessor httpContext)
            : base(options)
        {
            _httpContext = httpContext;
        }

        public virtual DbSet<TblComment> TblComments { get; set; } = null!;
        public virtual DbSet<TblNews> TblNews { get; set; } = null!;
        public virtual DbSet<TblRelatedInfo> TblRelatedInfos { get; set; } = null!;
        public virtual DbSet<TblSetting> TblSettings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblSetting>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.ToTable("tblSetting");

                entity.Property(e => e.Uid).ValueGeneratedNever();

                entity.Property(e => e.CreatedTimestamp).HasColumnType("datetime");

                entity.Property(e => e.CreatedUser).HasMaxLength(500);
            });

            modelBuilder.Entity<TblComment>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.ToTable("tblComment");

                entity.Property(e => e.Uid).ValueGeneratedNever();

                entity.Property(e => e.CreatedTimestamp).HasColumnType("datetime");

                entity.Property(e => e.CreatedUser).HasMaxLength(500);

                entity.Property(e => e.LastModifiedTimestamp).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedUser).HasMaxLength(500);
            });

            modelBuilder.Entity<TblNews>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.ToTable("tblNews");

                entity.Property(e => e.Uid).ValueGeneratedNever();

                entity.Property(e => e.CreatedTimestamp).HasColumnType("datetime");

                entity.Property(e => e.CreatedUser).HasMaxLength(500);

                entity.Property(e => e.LastModifiedTimestamp).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedUser).HasMaxLength(500);
            });

            modelBuilder.Entity<TblRelatedInfo>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.ToTable("tblRelatedInfo");

                entity.Property(e => e.Uid).ValueGeneratedNever();

                entity.Property(e => e.CreatedTimestamp).HasColumnType("datetime");

                entity.Property(e => e.CreatedUser).HasMaxLength(500);

                entity.Property(e => e.LastModifiedTimestamp).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedUser).HasMaxLength(500);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        private void SetSystemColumn()
        {
            var now = DateTime.UtcNow;

            var user = _httpContext.HttpContext?.User?.Identity?.Name;

            user ??= "System";

            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        if (entry.Properties.Any(p => p.Metadata.Name == "LastModifiedTimestamp"))
                            entry.Property("LastModifiedTimestamp").CurrentValue = now;
                        if (entry.Properties.Any(p => p.Metadata.Name == "LastModifiedUser"))
                            entry.Property("LastModifiedUser").CurrentValue = user;
                        break;
                    case EntityState.Added:
                        if (entry.Properties.Any(p => p.Metadata.Name == "LastModifiedTimestamp"))
                            entry.Property("LastModifiedTimestamp").CurrentValue = now;
                        if (entry.Properties.Any(p => p.Metadata.Name == "LastModifiedUser") && string.IsNullOrEmpty((string)entry.Property("LastModifiedUser").CurrentValue))
                            entry.Property("LastModifiedUser").CurrentValue = user;
                        if (entry.Properties.Any(p => p.Metadata.Name == "CreatedTimestamp"))
                            entry.Property("CreatedTimestamp").CurrentValue = now;
                        if (entry.Properties.Any(p => p.Metadata.Name == "CreatedUser") && string.IsNullOrEmpty((string)entry.Property("CreatedUser").CurrentValue))
                            entry.Property("CreatedUser").CurrentValue = user;
                        if (entry.Properties.Any(p => p.Metadata.Name == "Uid"))
                            entry.Property("Uid").CurrentValue = Guid.NewGuid();
                        break;
                    default:
                        break;
                }
            }
        }

        public override int SaveChanges()
        {
            SetSystemColumn();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetSystemColumn();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
