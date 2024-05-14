using System;
using System.Collections.Generic;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities.Data;

public partial class BBDDContext : DbContext
{
    public BBDDContext()
    {
    }

    public BBDDContext(DbContextOptions<BBDDContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppCBDynamic> AppCBDynamics { get; set; }

    public virtual DbSet<AppCBStatic> AppCBStatics { get; set; }

    public virtual DbSet<AppChat> AppChats { get; set; }

    public virtual DbSet<AppLogger> AppLoggers { get; set; }

    public virtual DbSet<AppProduct> AppProducts { get; set; }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<AppUsersRole> AppUsersRoles { get; set; }

    public virtual DbSet<AppUsersStat> AppUsersStats { get; set; }

    public virtual DbSet<SysActionType> SysActionTypes { get; set; }

    public virtual DbSet<SysCodeType> SysCodeTypes { get; set; }

    public virtual DbSet<SysLogger> SysLoggers { get; set; }

    public virtual DbSet<SysRole> SysRoles { get; set; }

    public virtual DbSet<SysValue> SysValues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppCBDynamic>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.Id });

            entity.ToTable("AppCBDynamic");

            entity.Property(e => e.UserId).HasComment("UserId");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasComment("Auto ID");
            entity.Property(e => e.CBType).HasMaxLength(15);
            entity.Property(e => e.Description)
                .HasMaxLength(135)
                .HasComment("Product description");
            entity.Property(e => e.IsoDateC).HasColumnType("datetime");
            entity.Property(e => e.IsoDateM).HasColumnType("datetime");
        });

        modelBuilder.Entity<AppCBStatic>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.Id });

            entity.ToTable("AppCBStatic");

            entity.Property(e => e.UserId).HasComment("UserId");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasComment("Auto ID");
            entity.Property(e => e.CBType).HasMaxLength(15);
            entity.Property(e => e.Description)
                .HasMaxLength(135)
                .HasComment("Product description");
            entity.Property(e => e.IsoDateC).HasColumnType("datetime");
            entity.Property(e => e.IsoDateM).HasColumnType("datetime");
        });

        modelBuilder.Entity<AppChat>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.IdxSec }).HasName("PK_AppChat");

            entity.Property(e => e.IdxSec).ValueGeneratedOnAdd();
            entity.Property(e => e.Datetime).HasColumnType("datetime");
        });

        modelBuilder.Entity<AppLogger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_sysCustomLog");

            entity.ToTable("AppLogger");

            entity.Property(e => e.Id).HasComment("Auto ID");
            entity.Property(e => e.Message).HasComment("message of the warning or error");
            entity.Property(e => e.MessageDetails).HasComment("Additional info");
            entity.Property(e => e.MessageType)
                .HasMaxLength(15)
                .HasComment("type of the message (Warning, Info, Debug, Error...)");
            entity.Property(e => e.UserId).HasComment("UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AppLoggers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_sysCustomLog_appUsers");
        });

        modelBuilder.Entity<AppProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_appProducts");

            entity.ToTable(tb => tb.HasComment("Users Products table"));

            entity.Property(e => e.Id).HasComment("Auto ID");
            entity.Property(e => e.ActionId)
                .HasMaxLength(15)
                .HasComment("Product ActionType");
            entity.Property(e => e.CodeId)
                .HasMaxLength(15)
                .HasComment("Product CodeType");
            entity.Property(e => e.Description)
                .HasMaxLength(135)
                .HasComment("Product description");
            entity.Property(e => e.Reference)
                .HasMaxLength(35)
                .HasComment("Product reference");
            entity.Property(e => e.UserId).HasComment("UserId");

            entity.HasOne(d => d.Action).WithMany(p => p.AppProducts)
                .HasForeignKey(d => d.ActionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_appProducts_sysActionTypes");

            entity.HasOne(d => d.Code).WithMany(p => p.AppProducts)
                .HasForeignKey(d => d.CodeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_appProducts_sysCodeTypes");

            entity.HasOne(d => d.User).WithMany(p => p.AppProducts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_appProducts_appUsers");
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_users");

            entity.ToTable(tb => tb.HasComment("Application User table"));

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(newid())")
                .HasComment("UUID unique User ID");
            entity.Property(e => e.Address).HasComment("User address");
            entity.Property(e => e.Comments).HasComment("Internal comments");
            entity.Property(e => e.Is2FAEnabled).HasDefaultValue(false);
            entity.Property(e => e.IsAdmin).HasDefaultValue(false);
            entity.Property(e => e.IsBlocked).HasDefaultValue(false);
            entity.Property(e => e.Login).HasComment("User login email");
            entity.Property(e => e.Name)
                .HasMaxLength(35)
                .HasComment("User name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasComment("User phone");
            entity.Property(e => e.Retries).HasDefaultValue(0);
            entity.Property(e => e.Surname)
                .HasMaxLength(125)
                .HasComment("User surname");
            entity.Property(e => e.TokenExpiresUTC).HasColumnType("datetime");
            entity.Property(e => e.TokenIsValid).HasDefaultValue(false);
            entity.Property(e => e.TokenIssuedUTC).HasColumnType("datetime");
        });

        modelBuilder.Entity<AppUsersRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_user_roles");

            entity.ToTable(tb => tb.HasComment("Applicaction Roles table"));

            entity.Property(e => e.Id).HasComment("Auto ID");
            entity.Property(e => e.Role)
                .HasMaxLength(35)
                .HasComment("Role code");
            entity.Property(e => e.UserId).HasComment("UserId");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.AppUsersRoles)
                .HasForeignKey(d => d.Role)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_appUsersRoles_sysRoles");

            entity.HasOne(d => d.User).WithMany(p => p.AppUsersRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usersRoles_users");
        });

        modelBuilder.Entity<AppUsersStat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_sysUserConn");

            entity.Property(e => e.Id).HasComment("Auto ID");
            entity.Property(e => e.IPv4)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("Connection IPv4");
            entity.Property(e => e.IPv6)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasComment("Connection IPv6");
            entity.Property(e => e.IsoDateC).HasColumnType("datetime");
            entity.Property(e => e.IsoDateM).HasColumnType("datetime");
            entity.Property(e => e.Location).HasComment("Connection Device, City, OS...");
            entity.Property(e => e.UserId).HasComment("UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AppUsersStats)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_sysUserConn_appUsers");
        });

        modelBuilder.Entity<SysActionType>(entity =>
        {
            entity.HasKey(e => e.ActionId).HasName("PK_sysActionTypes");

            entity.ToTable(tb => tb.HasComment("Product actions types availables"));

            entity.Property(e => e.ActionId)
                .HasMaxLength(15)
                .HasComment("Product Code Type");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasComment("Action description");
        });

        modelBuilder.Entity<SysCodeType>(entity =>
        {
            entity.HasKey(e => e.CodeId).HasName("PK_sysCodeTypes");

            entity.ToTable(tb => tb.HasComment("Product codes availables"));

            entity.Property(e => e.CodeId)
                .HasMaxLength(15)
                .HasComment("Product Code");
            entity.Property(e => e.Description).HasComment("Code description");
        });

        modelBuilder.Entity<SysLogger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__sysLogger__3214EC075E654400");

            entity.ToTable("SysLogger");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Level).HasMaxLength(10);
            entity.Property(e => e.Logger).HasMaxLength(255);
            entity.Property(e => e.Url).HasMaxLength(255);
        });

        modelBuilder.Entity<SysRole>(entity =>
        {
            entity.HasKey(e => e.Role).HasName("PK_sysRoles");

            entity.ToTable(tb => tb.HasComment("Roles available table"));

            entity.Property(e => e.Role)
                .HasMaxLength(35)
                .HasComment("Code of the authorization role");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasComment("Role description");
        });

        modelBuilder.Entity<SysValue>(entity =>
        {
            entity.HasKey(e => e.Setting).HasName("PK_sysValues");

            entity.ToTable(tb => tb.HasComment("Settings and Configuration values table"));

            entity.Property(e => e.Setting)
                .HasMaxLength(35)
                .HasComment("Setting Key");
            entity.Property(e => e.Description)
                .HasMaxLength(135)
                .HasComment("Key-Value description");
            entity.Property(e => e.Value)
                .HasMaxLength(135)
                .HasComment("Seeting Value");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
