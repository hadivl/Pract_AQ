using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Аквариум_практика
{
    public partial class AquariumContext : DbContext
    {
        public AquariumContext()
        {
        }

        public AquariumContext(DbContextOptions<AquariumContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AquaClassPlantsFish> AquaClassPlantsFish { get; set; }
        public virtual DbSet<AquariumClassification> AquariumClassification { get; set; }
        public virtual DbSet<AquariumCleaningEquipment> AquariumCleaningEquipment { get; set; }
        public virtual DbSet<AquariumFish> AquariumFish { get; set; }
        public virtual DbSet<AquariumOrder> AquariumOrder { get; set; }
        public virtual DbSet<Equipment> Equipment { get; set; }
        public virtual DbSet<FeedAndMedicines> FeedAndMedicines { get; set; }
        public virtual DbSet<FishAndFeedMedicines> FishAndFeedMedicines { get; set; }
        public virtual DbSet<InfoOrderAquariums> InfoOrderAquariums { get; set; }
        public virtual DbSet<PlantsForAquariums> PlantsForAquariums { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=MSI;Initial Catalog=Aquarium;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AquaClassPlantsFish>(entity =>
            {
                entity.HasKey(e => e.IdAquaClassPlantsAqua);

                entity.ToTable("Aqua_Class_Plants_Fish");

                entity.HasOne(d => d.IdAquaFishNavigation)
                    .WithMany(p => p.AquaClassPlantsFish)
                    .HasForeignKey(d => d.IdAquaFish)
                    .HasConstraintName("FK_Aqua_Class_Plants_Fish_Aquarium_Fish");

                entity.HasOne(d => d.IdAquariumClassNavigation)
                    .WithMany(p => p.AquaClassPlantsFish)
                    .HasForeignKey(d => d.IdAquariumClass)
                    .HasConstraintName("FK_Aqua_Class_Plants_Fish_Aquarium_Classification");

                entity.HasOne(d => d.IdAquariumOrderNavigation)
                    .WithMany(p => p.AquaClassPlantsFish)
                    .HasForeignKey(d => d.IdAquariumOrder)
                    .HasConstraintName("FK_Aqua_Class_Plants_Fish_Aquarium_Order");

                entity.HasOne(d => d.IdPlantsAquariumsNavigation)
                    .WithMany(p => p.AquaClassPlantsFish)
                    .HasForeignKey(d => d.IdPlantsAquariums)
                    .HasConstraintName("FK_Aqua_Class_Plants_Fish_Plants_For_Aquariums");
            });

            modelBuilder.Entity<AquariumClassification>(entity =>
            {
                entity.HasKey(e => e.IdAquariumClass);

                entity.ToTable("Aquarium_Classification");

                entity.HasIndex(e => new { e.Volume, e.Shape })
                    .HasName("classification_index");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Scope)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Shape)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TypeConstruction)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<AquariumCleaningEquipment>(entity =>
            {
                entity.HasKey(e => e.IdCleaningEquipment);

                entity.ToTable("Aquarium_Cleaning_Equipment");

                entity.Property(e => e.Cleaning).HasMaxLength(100);

                entity.Property(e => e.EquipCatchingFish).HasMaxLength(100);

                entity.Property(e => e.EquipPlantingFish).HasMaxLength(100);

                entity.Property(e => e.PumpingOutWater).HasMaxLength(100);

                entity.Property(e => e.WaterInlet).HasMaxLength(100);

                entity.HasOne(d => d.IdAquariumClassNavigation)
                    .WithMany(p => p.AquariumCleaningEquipment)
                    .HasForeignKey(d => d.IdAquariumClass)
                    .HasConstraintName("FK_Aquarium_Cleaning_Equipment_Aquarium_Classification");
            });

            modelBuilder.Entity<AquariumFish>(entity =>
            {
                entity.HasKey(e => e.IdAquaFish);

                entity.ToTable("Aquarium_Fish");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.pHOfTheWater)
                    .IsRequired()
                    .HasColumnName("pHOfTheWater")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.TheNeedShelters)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AquariumOrder>(entity =>
            {
                entity.HasKey(e => e.IdAquariumOrder);

                entity.ToTable("Aquarium_Order");

                entity.Property(e => e.Customer)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsFixedLength();

                entity.Property(e => e.Сashier)
                    .HasMaxLength(150)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.HasKey(e => e.IdEquipment);

                entity.Property(e => e.AdditionalEquipment)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Lighting)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.TemperatureControl)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.WaterPurification)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.IdAquariumClassNavigation)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.IdAquariumClass)
                    .HasConstraintName("FK_Equipment_Aquarium_Classification");
            });

            modelBuilder.Entity<FeedAndMedicines>(entity =>
            {
                entity.HasKey(e => e.IdFeedAndMedicines);

                entity.ToTable("Feed_And_Medicines");

                entity.Property(e => e.NameFeed)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.NameMedicine)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.TypeFeed)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.TypeMedicine)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<FishAndFeedMedicines>(entity =>
            {
                entity.HasKey(e => e.IdAquariumFeedMedicines);

                entity.ToTable("Fish_And_Feed_Medicines");

                entity.HasOne(d => d.IdAquaFishNavigation)
                    .WithMany(p => p.FishAndFeedMedicines)
                    .HasForeignKey(d => d.IdAquaFish)
                    .HasConstraintName("FK_Fish_And_Feed_Medicines_Aquarium_Fish");

                entity.HasOne(d => d.IdFeedAndMedicinesNavigation)
                    .WithMany(p => p.FishAndFeedMedicines)
                    .HasForeignKey(d => d.IdFeedAndMedicines)
                    .HasConstraintName("FK_Fish_And_Feed_Medicines_Feed_And_Medicines");
            });

            modelBuilder.Entity<InfoOrderAquariums>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("info_Order_Aquariums");

                entity.Property(e => e.Customer)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsFixedLength();

                entity.Property(e => e.Scope)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsFixedLength();
            });

            modelBuilder.Entity<PlantsForAquariums>(entity =>
            {
                entity.HasKey(e => e.IdPlantsAquariums);

                entity.ToTable("Plants_For_Aquariums");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.HasOne(d => d.IdAquaFishNavigation)
                    .WithMany(p => p.PlantsForAquariums)
                    .HasForeignKey(d => d.IdAquaFish)
                    .HasConstraintName("FK_Plants_For_Aquariums_Aquarium_Fish");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.LoginUser)
                    .IsRequired()
                    .HasMaxLength(70)
                    .IsFixedLength();

                entity.Property(e => e.NameUser)
                    .IsRequired()
                    .HasMaxLength(70)
                    .IsFixedLength();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
