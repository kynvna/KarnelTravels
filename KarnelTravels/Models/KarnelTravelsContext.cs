using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KarnelTravels.Models;

public partial class KarnelTravelsContext : DbContext
{
    public KarnelTravelsContext()
    {
    }

    public KarnelTravelsContext(DbContextOptions<KarnelTravelsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<HrCategory> HrCategories { get; set; }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblFeedback> TblFeedbacks { get; set; }

    public virtual DbSet<TblHotelRestaurant> TblHotelRestaurants { get; set; }

    public virtual DbSet<TblImageUrl> TblImageUrls { get; set; }

    public virtual DbSet<TblNews> TblNews { get; set; }

    public virtual DbSet<TblSpot> TblSpots { get; set; }

    public virtual DbSet<TblTourPackage> TblTourPackages { get; set; }

    public virtual DbSet<TblTouristPlace> TblTouristPlaces { get; set; }

    public virtual DbSet<TblTransportation> TblTransportations { get; set; }

    public virtual DbSet<TblTravel> TblTravels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:KarnelTravelsDatabase");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HrCategory>(entity =>
        {
            entity.HasKey(e => e.CatId).HasName("PK__HR_categ__6A1C8AFA129188E4");

            entity.ToTable("HR_category");

            entity.Property(e => e.CatName).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<TblCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__tblCusto__A4AE64D88CC5D9AB");

            entity.ToTable("tblCustomer");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(50);
        });

        modelBuilder.Entity<TblFeedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__tblFeedb__6A4BEDD6A4D66BCF");

            entity.ToTable("tblFeedbacks");

            entity.Property(e => e.FeedbackObject).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Customer).WithMany(p => p.TblFeedbacks)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__tblFeedba__Custo__5EBF139D");
        });

        modelBuilder.Entity<TblHotelRestaurant>(entity =>
        {
            entity.HasKey(e => e.HrId).HasName("PK__tblHotel__272A3F1EA06B4CDC");

            entity.ToTable("tblHotel_Restaurant");

            entity.Property(e => e.HrId).HasColumnName("HR_Id");
            entity.Property(e => e.Imglink).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Cat).WithMany(p => p.TblHotelRestaurants)
                .HasForeignKey(d => d.CatId)
                .HasConstraintName("FK__tblHotel___CatId__4BAC3F29");

            entity.HasOne(d => d.ImageLink).WithMany(p => p.TblHotelRestaurants)
                .HasForeignKey(d => d.ImageLinkId)
                .HasConstraintName("FK_tblHotel_Restaurant_tblImage_Url");
        });

        modelBuilder.Entity<TblImageUrl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblImage__3214EC0766DF4823");

            entity.ToTable("tblImage_Url");

            entity.Property(e => e.Description).HasMaxLength(10);
        });

        modelBuilder.Entity<TblNews>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__tblNews__954EBDF31E1C7C24");

            entity.ToTable("tblNews");

            entity.Property(e => e.ImageLinkIid).HasColumnName("ImageLinkIId");
            entity.Property(e => e.NewsObject).HasMaxLength(50);
            entity.Property(e => e.ObjectId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.ImageLinkI).WithMany(p => p.TblNews)
                .HasForeignKey(d => d.ImageLinkIid)
                .HasConstraintName("FK_tblNews_tblImage_Url");
        });

        modelBuilder.Entity<TblSpot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblSpots__3214EC07DCDE9EDD");

            entity.ToTable("tblSpots");

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<TblTourPackage>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PK__tblTour___322035CC5FAB8F98");

            entity.ToTable("tblTour_Packages");

            entity.Property(e => e.EndDate).HasColumnName("End_date");
            entity.Property(e => e.StartDate).HasColumnName("Start_date");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("money")
                .HasColumnName("Total_price");

            entity.HasOne(d => d.ImageLink).WithMany(p => p.TblTourPackages)
                .HasForeignKey(d => d.ImageLinkId)
                .HasConstraintName("FK_tblTour_Packages_tblImage_Url");
        });

        modelBuilder.Entity<TblTouristPlace>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblTouri__3214EC07F27F2B9F");

            entity.ToTable("tblTourist_Place");

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.ImageLink).WithMany(p => p.TblTouristPlaces)
                .HasForeignKey(d => d.ImageLinkId)
                .HasConstraintName("FK_tblTourist_Place_tblImage_Url");

            entity.HasOne(d => d.Sport).WithMany(p => p.TblTouristPlaces)
                .HasForeignKey(d => d.SportId)
                .HasConstraintName("FK_tblTourist_Place_tblSpots");
        });

        modelBuilder.Entity<TblTransportation>(entity =>
        {
            entity.HasKey(e => e.CatId).HasName("PK__tblTrans__6A1C8AFAB2E48C18");

            entity.ToTable("tblTransportation");

            entity.Property(e => e.CatName).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<TblTravel>(entity =>
        {
            entity.HasKey(e => e.TravelId).HasName("PK__tblTrave__E931523528691ABD");

            entity.ToTable("tblTravel");

            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.SpotDeparture).HasColumnName("Spot_Departure");
            entity.Property(e => e.SpotDestination).HasColumnName("Spot_Destination");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TransCategoryId).HasColumnName("Trans_CategoryId");

            entity.HasOne(d => d.ImageLink).WithMany(p => p.TblTravels)
                .HasForeignKey(d => d.ImageLinkId)
                .HasConstraintName("FK_tblTravel_tblImage_Url");

            entity.HasOne(d => d.TransCategory).WithMany(p => p.TblTravels)
                .HasForeignKey(d => d.TransCategoryId)
                .HasConstraintName("FK__tblTravel__Trans__5070F446");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
