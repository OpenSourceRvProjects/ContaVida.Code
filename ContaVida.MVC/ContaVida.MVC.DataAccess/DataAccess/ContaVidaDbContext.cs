using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ContaVida.MVC.DataAccess.DataAccess;

public partial class ContaVidaDbContext : DbContext
{
    public ContaVidaDbContext()
    {
    }

    public ContaVidaDbContext(DbContextOptions<ContaVidaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CorrectLogin> CorrectLogins { get; set; }

    public virtual DbSet<EventCounter> EventCounters { get; set; }

    public virtual DbSet<PersonalProfile> PersonalProfiles { get; set; }

    public virtual DbSet<Relapse> Relapses { get; set; }

    public virtual DbSet<ResetLoginPassword> ResetLoginPasswords { get; set; }

    public virtual DbSet<SignUpRequest> SignUpRequests { get; set; }

    public virtual DbSet<SystemMaintenance> SystemMaintenances { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //Scaffold-DbContext "Server=.\SQLEXPRESS01;Database=NgLifeCounterDB;Trusted_Connection=True;Encrypt=False" Microsoft.EntityFrameworkCore.SqlServer -OutputDir DataAccess -Context ContaVidaDbContext -F
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CorrectLogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CorrectL__3214EC074C46D4D9");

            entity.ToTable("CorrectLogin");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IpAddress).HasColumnName("IP_Address");
            entity.Property(e => e.LoginDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.CorrectLogins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CorrectLo__UserI__4CA06362");
        });

        modelBuilder.Entity<EventCounter>(entity =>
        {
            entity.ToTable("EventCounter");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.RefreshMinutesTime).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");

            entity.HasOne(d => d.PersonalProfile).WithMany(p => p.EventCounters)
                .HasForeignKey(d => d.PersonalProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventCoun__Perso__4D94879B");

            entity.HasOne(d => d.User).WithMany(p => p.EventCounters)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventCoun__UserI__4E88ABD4");
        });

        modelBuilder.Entity<PersonalProfile>(entity =>
        {
            entity.ToTable("PersonalProfile");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CounterLimit).HasDefaultValue(100);
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.RelapseLimit).HasDefaultValue(150);

            entity.HasOne(d => d.User).WithMany(p => p.PersonalProfiles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PersonalP__UserI__4F7CD00D");
        });

        modelBuilder.Entity<Relapse>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.EventCounterId).HasColumnName("EventCounterID");
            entity.Property(e => e.PersonalProfileId).HasColumnName("PersonalProfileID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.EventCounter).WithMany(p => p.Relapses)
                .HasForeignKey(d => d.EventCounterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventCounterRelapses");

            entity.HasOne(d => d.PersonalProfile).WithMany(p => p.Relapses)
                .HasForeignKey(d => d.PersonalProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonalProfileRelapses");

            entity.HasOne(d => d.User).WithMany(p => p.Relapses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRelapses");
        });

        modelBuilder.Entity<ResetLoginPassword>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ResetLog__3214EC079562E333");

            entity.ToTable("ResetLoginPassword");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.ResetLoginPasswords)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ResetLogi__UserI__534D60F1");
        });

        modelBuilder.Entity<SignUpRequest>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Ip).HasColumnName("IP");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.SignUpRequests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SignUpReq__UserI__5441852A");
        });

        modelBuilder.Entity<SystemMaintenance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SystemMa__3214EC07AFCE362C");

            entity.ToTable("SystemMaintenance");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
