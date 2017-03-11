using Microsoft.EntityFrameworkCore;
using SkyEditor.ExtensionWebsite.Models.ExtensionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.ExtensionWebsite.Data
{
    public class ExtensionsDbContext : DbContext
    {
        public static ExtensionsDbContext Create()
        {
            return new ExtensionsDbContext(PreviousOptions);
        }

        private static DbContextOptions<ExtensionsDbContext> PreviousOptions;

        public ExtensionsDbContext(DbContextOptions<ExtensionsDbContext> options) : base(options)
        {
            PreviousOptions = options;
        }

        public DbSet<ExtensionCollection> ExtensionCollections { get; set; }
        public DbSet<Extension> Extensions { get; set; }
        public DbSet<ExtensionVersion> ExtensionVersions { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ExtensionVersion>().HasKey(nameof(ExtensionVersion.ID), nameof(ExtensionVersion.Version));
        }
    }
}
