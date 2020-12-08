using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.DAL.Base;
using Contracts.Domain.Base;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.App
{
    public class AppDbContext : DbContext, IBaseEntityTracker
    { 
        public DbSet<BagWithLetters> BagWithLetterses { get; set; } = default!;
        public DbSet<BagWithParcels> BagWithParcelses { get; set; } = default!;
        public DbSet<Parcel> Parcels { get; set; } = default!;
        public DbSet<Shipment> Shipments { get; set; } = default!;
        
        private readonly Dictionary<IDomainEntityId<Guid>, IDomainEntityId<Guid>> _entityTracker =
            new Dictionary<IDomainEntityId<Guid>, IDomainEntityId<Guid>>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Create unique indexes
            builder.Entity<Parcel>()
                .HasIndex(p => p.ParcelNumber)
                .IsUnique();

            builder.Entity<BagWithParcels>()
                .HasIndex(u => u.BagNumber)
                .IsUnique();
            
            builder.Entity<BagWithLetters>()
                .HasIndex(u => u.BagNumber)
                .IsUnique();
            
            builder.Entity<Shipment>()
                .HasIndex(u => u.ShipmentNumber)
                .IsUnique();

            // Disable cascade delete
            foreach (var relationship in builder.Model
                .GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            
            // Enable cascade delete on Shipment -> BagWithParcels
            builder.Entity<Shipment>()
                .HasMany(s => s.BagWithParcelses)
                .WithOne(l => l.Shipment!)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Enable cascade delete on Shipment -> BagWithLetters
            builder.Entity<Shipment>()
                .HasMany(s => s.BagWithLetterses)
                .WithOne(l => l.Shipment!)
                .OnDelete(DeleteBehavior.Cascade);

            // Enable cascade delete on BagWithParcels -> Parcel
            builder.Entity<BagWithParcels>()
                .HasMany(s => s.Parcels)
                .WithOne(l => l.BagWithParcels!)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public void AddToEntityTracker(IDomainEntityId<Guid> internalEntity, IDomainEntityId<Guid> externalEntity)
        {
            _entityTracker.Add(internalEntity, externalEntity);
        }

    }

}