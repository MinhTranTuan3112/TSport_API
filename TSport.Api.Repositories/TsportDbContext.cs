using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TSport.Api.Repositories.Entities;

namespace TSport.Api.Repositories;

public partial class TsportDbContext : DbContext
{
    public TsportDbContext()
    {
    }

    public TsportDbContext(DbContextOptions<TsportDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Season> Seasons { get; set; }

    public virtual DbSet<SeasonPlayer> SeasonPlayers { get; set; }

    public virtual DbSet<Shirt> Shirts { get; set; }

    public virtual DbSet<ShirtEdition> ShirtEditions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC0770447F11");

            entity.ToTable("Account");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Club__3214EC07A693A486");

            entity.ToTable("Club");

            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(100);

            entity.HasOne(d => d.CreatedAccount).WithMany(p => p.ClubCreatedAccounts)
                .HasForeignKey(d => d.CreatedAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Club__CreatedAcc__46E78A0C");

            entity.HasOne(d => d.ModifiedAccount).WithMany(p => p.ClubModifiedAccounts)
                .HasForeignKey(d => d.ModifiedAccountId)
                .HasConstraintName("FK__Club__ModifiedAc__47DBAE45");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Image__3214EC0710A0FCAC");

            entity.ToTable("Image");

            entity.HasOne(d => d.Shirt).WithMany(p => p.Images)
                .HasForeignKey(d => d.ShirtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Image__ShirtId__6D0D32F4");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC0728491D19");

            entity.ToTable("Order");

            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.Total).HasColumnType("money");

            entity.HasOne(d => d.CreatedAccount).WithMany(p => p.OrderCreatedAccounts)
                .HasForeignKey(d => d.CreatedAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__CreatedAc__3C69FB99");

            entity.HasOne(d => d.ModifiedAccount).WithMany(p => p.OrderModifiedAccounts)
                .HasForeignKey(d => d.ModifiedAccountId)
                .HasConstraintName("FK__Order__ModifiedA__3D5E1FD2");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ShirtId }).HasName("PK__OrderDet__63098A9E67A8899C");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.Size).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.Subtotal).HasColumnType("money");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__693CA210");

            entity.HasOne(d => d.Shirt).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ShirtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Shirt__6A30C649");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC07575C7002");

            entity.ToTable("Payment");

            entity.Property(e => e.Amount).HasColumnType("money");
            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(255);
            entity.Property(e => e.PaymentName).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(100);

            entity.HasOne(d => d.CreatedAccount).WithMany(p => p.PaymentCreatedAccounts)
                .HasForeignKey(d => d.CreatedAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__Created__4222D4EF");

            entity.HasOne(d => d.ModifiedAccount).WithMany(p => p.PaymentModifiedAccounts)
                .HasForeignKey(d => d.ModifiedAccountId)
                .HasConstraintName("FK__Payment__Modifie__4316F928");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Payment__OrderId__412EB0B6");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Player__3214EC0758A793B4");

            entity.ToTable("Player");

            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(100);

            entity.HasOne(d => d.Club).WithMany(p => p.Players)
                .HasForeignKey(d => d.ClubId)
                .HasConstraintName("FK__Player__ClubId__4D94879B");

            entity.HasOne(d => d.CreatedAccount).WithMany(p => p.PlayerCreatedAccounts)
                .HasForeignKey(d => d.CreatedAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Player__CreatedA__4BAC3F29");

            entity.HasOne(d => d.ModifiedAccount).WithMany(p => p.PlayerModifiedAccounts)
                .HasForeignKey(d => d.ModifiedAccountId)
                .HasConstraintName("FK__Player__Modified__4CA06362");
        });

        modelBuilder.Entity<Season>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Season__3214EC0709A51F33");

            entity.ToTable("Season");

            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(100);

            entity.HasOne(d => d.Club).WithMany(p => p.Seasons)
                .HasForeignKey(d => d.ClubId)
                .HasConstraintName("FK__Season__ClubId__5165187F");

            entity.HasOne(d => d.CreatedAccount).WithMany(p => p.SeasonCreatedAccounts)
                .HasForeignKey(d => d.CreatedAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Season__CreatedA__52593CB8");

            entity.HasOne(d => d.ModifiedAccount).WithMany(p => p.SeasonModifiedAccounts)
                .HasForeignKey(d => d.ModifiedAccountId)
                .HasConstraintName("FK__Season__Modified__534D60F1");
        });

        modelBuilder.Entity<SeasonPlayer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SeasonPl__3214EC072603C516");

            entity.ToTable("SeasonPlayer");

            entity.HasOne(d => d.Player).WithMany(p => p.SeasonPlayers)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SeasonPla__Playe__571DF1D5");

            entity.HasOne(d => d.Season).WithMany(p => p.SeasonPlayers)
                .HasForeignKey(d => d.SeasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SeasonPla__Seaso__5629CD9C");
        });

        modelBuilder.Entity<Shirt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shirt__3214EC076D4A401D");

            entity.ToTable("Shirt");

            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(100);

            entity.HasOne(d => d.CreatedAccount).WithMany(p => p.ShirtCreatedAccounts)
                .HasForeignKey(d => d.CreatedAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Shirt__CreatedAc__619B8048");

            entity.HasOne(d => d.ModifiedAccount).WithMany(p => p.ShirtModifiedAccounts)
                .HasForeignKey(d => d.ModifiedAccountId)
                .HasConstraintName("FK__Shirt__ModifiedA__628FA481");

            entity.HasOne(d => d.SeasonPlayer).WithMany(p => p.Shirts)
                .HasForeignKey(d => d.SeasonPlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Shirt__SeasonPla__6477ECF3");

            entity.HasOne(d => d.ShirtEdition).WithMany(p => p.Shirts)
                .HasForeignKey(d => d.ShirtEditionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Shirt__ShirtEdit__6383C8BA");
        });

        modelBuilder.Entity<ShirtEdition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ShirtEdi__3214EC078127535B");

            entity.ToTable("ShirtEdition");

            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.Color).HasMaxLength(255);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DiscountPrice).HasColumnType("money");
            entity.Property(e => e.Material).HasMaxLength(255);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Origin).HasMaxLength(255);
            entity.Property(e => e.Size).HasMaxLength(10);
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.StockPrice).HasColumnType("money");

            entity.HasOne(d => d.CreatedAccount).WithMany(p => p.ShirtEditionCreatedAccounts)
                .HasForeignKey(d => d.CreatedAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShirtEdit__Creat__5CD6CB2B");

            entity.HasOne(d => d.ModifiedAccount).WithMany(p => p.ShirtEditionModifiedAccounts)
                .HasForeignKey(d => d.ModifiedAccountId)
                .HasConstraintName("FK__ShirtEdit__Modif__5DCAEF64");

            entity.HasOne(d => d.Season).WithMany(p => p.ShirtEditions)
                .HasForeignKey(d => d.SeasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ShirtEdit__Seaso__5BE2A6F2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
