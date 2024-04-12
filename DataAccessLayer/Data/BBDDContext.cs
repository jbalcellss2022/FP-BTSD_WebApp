using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data;

public partial class BBDDContext : DbContext
{
    public BBDDContext()
    {
    }

    public BBDDContext(DbContextOptions<BBDDContext> options)
        : base(options)
    {
    }

    public virtual DbSet<appLogger> appLoggers { get; set; }

    public virtual DbSet<appProduct> appProducts { get; set; }

    public virtual DbSet<appUser> appUsers { get; set; }

    public virtual DbSet<appUsersRole> appUsersRoles { get; set; }

    public virtual DbSet<appUsersStat> appUsersStats { get; set; }

    public virtual DbSet<sysActionType> sysActionTypes { get; set; }

    public virtual DbSet<sysCodeType> sysCodeTypes { get; set; }

    public virtual DbSet<sysLogger> sysLoggers { get; set; }

    public virtual DbSet<sysRole> sysRoles { get; set; }

    public virtual DbSet<sysValue> sysValues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=UOCFP_qrfy;User Id=sa;Password=TzWb0q98Cw8rE9hKVdN0;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<appLogger>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK_sysCustomLog");

            entity.ToTable("appLogger");

            entity.Property(e => e.id).HasComment("Auto ID");
            entity.Property(e => e.message).HasComment("message of the warning or error");
            entity.Property(e => e.messageDetails).HasComment("Additional info");
            entity.Property(e => e.messageType)
                .HasMaxLength(15)
                .HasComment("type of the message (Warning, Info, Debug, Error...)");
            entity.Property(e => e.userId).HasComment("UserId");

            entity.HasOne(d => d.user).WithMany(p => p.appLoggers)
                .HasForeignKey(d => d.userId)
                .HasConstraintName("FK_sysCustomLog_appUsers");
        });

        modelBuilder.Entity<appProduct>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Users Products table"));

            entity.Property(e => e.id).HasComment("Auto ID");
            entity.Property(e => e.actionId)
                .HasMaxLength(15)
                .HasComment("Product ActionType");
            entity.Property(e => e.codeId)
                .HasMaxLength(15)
                .HasComment("Product CodeType");
            entity.Property(e => e.description)
                .HasMaxLength(135)
                .HasComment("Product description");
            entity.Property(e => e.reference)
                .HasMaxLength(35)
                .HasComment("Product reference");
            entity.Property(e => e.userId).HasComment("UserId");

            entity.HasOne(d => d.action).WithMany(p => p.appProducts)
                .HasForeignKey(d => d.actionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_appProducts_sysActionTypes");

            entity.HasOne(d => d.code).WithMany(p => p.appProducts)
                .HasForeignKey(d => d.codeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_appProducts_sysCodeTypes");

            entity.HasOne(d => d.user).WithMany(p => p.appProducts)
                .HasForeignKey(d => d.userId)
                .HasConstraintName("FK_appProducts_appUsers");
        });

        modelBuilder.Entity<appUser>(entity =>
        {
            entity.HasKey(e => e.userId).HasName("PK_users");

            entity.ToTable(tb => tb.HasComment("Application User table"));

            entity.Property(e => e.userId)
                .HasDefaultValueSql("(newid())")
                .HasComment("UUID unique User ID");
            entity.Property(e => e.address).HasComment("User address");
            entity.Property(e => e.comments).HasComment("Internal comments");
            entity.Property(e => e.login).HasComment("User login email");
            entity.Property(e => e.name)
                .HasMaxLength(35)
                .HasComment("User name");
            entity.Property(e => e.phone)
                .HasMaxLength(20)
                .HasComment("User phone");
            entity.Property(e => e.surname)
                .HasMaxLength(125)
                .HasComment("User surname");
        });

        modelBuilder.Entity<appUsersRole>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK_user_roles");

            entity.ToTable(tb => tb.HasComment("Applicaction Roles table"));

            entity.Property(e => e.id).HasComment("Auto ID");
            entity.Property(e => e.role)
                .HasMaxLength(35)
                .HasComment("Role code");
            entity.Property(e => e.userId).HasComment("UserId");

            entity.HasOne(d => d.roleNavigation).WithMany(p => p.appUsersRoles)
                .HasForeignKey(d => d.role)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_appUsersRoles_sysRoles");

            entity.HasOne(d => d.user).WithMany(p => p.appUsersRoles)
                .HasForeignKey(d => d.userId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usersRoles_users");
        });

        modelBuilder.Entity<appUsersStat>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK_sysUserConn");

            entity.Property(e => e.id).HasComment("Auto ID");
            entity.Property(e => e.userData).HasComment("Connection Device, City, OS...");
            entity.Property(e => e.userDateTime)
                .HasComment("DateTime of the connection")
                .HasColumnType("datetime");
            entity.Property(e => e.userIPv4)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("Connection IPv4");
            entity.Property(e => e.userIPv6)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasComment("Connection IPv6");
            entity.Property(e => e.userId).HasComment("UserId");

            entity.HasOne(d => d.user).WithMany(p => p.appUsersStats)
                .HasForeignKey(d => d.userId)
                .HasConstraintName("FK_sysUserConn_appUsers");
        });

        modelBuilder.Entity<sysActionType>(entity =>
        {
            entity.HasKey(e => e.actionId);

            entity.ToTable(tb => tb.HasComment("Product actions types availables"));

            entity.Property(e => e.actionId)
                .HasMaxLength(15)
                .HasComment("Product Code Type");
            entity.Property(e => e.description)
                .HasMaxLength(50)
                .HasComment("Action description");
        });

        modelBuilder.Entity<sysCodeType>(entity =>
        {
            entity.HasKey(e => e.codeId);

            entity.ToTable(tb => tb.HasComment("Product codes availables"));

            entity.Property(e => e.codeId)
                .HasMaxLength(15)
                .HasComment("Product Code");
            entity.Property(e => e.description)
                .HasMaxLength(50)
                .HasComment("Code description");
        });

        modelBuilder.Entity<sysLogger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__sysLogger__3214EC075E654400");

            entity.ToTable("sysLogger");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Level).HasMaxLength(10);
            entity.Property(e => e.Logger).HasMaxLength(255);
            entity.Property(e => e.Url).HasMaxLength(255);
        });

        modelBuilder.Entity<sysRole>(entity =>
        {
            entity.HasKey(e => e.role);

            entity.ToTable(tb => tb.HasComment("Roles available table"));

            entity.Property(e => e.role)
                .HasMaxLength(35)
                .HasComment("Code of the authorization role");
            entity.Property(e => e.description)
                .HasMaxLength(50)
                .HasComment("Role description");
        });

        modelBuilder.Entity<sysValue>(entity =>
        {
            entity.HasKey(e => e.setting);

            entity.ToTable(tb => tb.HasComment("Settings and Configuration values table"));

            entity.Property(e => e.setting)
                .HasMaxLength(35)
                .HasComment("Setting Key");
            entity.Property(e => e.description)
                .HasMaxLength(135)
                .HasComment("Key-Value description");
            entity.Property(e => e.value)
                .HasMaxLength(135)
                .HasComment("Seeting Value");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
