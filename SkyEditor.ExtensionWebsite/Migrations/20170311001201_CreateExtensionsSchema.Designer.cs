using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SkyEditor.ExtensionWebsite.Data;

namespace SkyEditor.ExtensionWebsite.Migrations
{
    [DbContext(typeof(ExtensionsDbContext))]
    [Migration("20170311001201_CreateExtensionsSchema")]
    partial class CreateExtensionsSchema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("SkyEditor.ExtensionWebsite.Models.ExtensionModels.ApiKey", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CollectionID");

                    b.Property<string>("Key")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("CollectionID");

                    b.ToTable("ApiKey");
                });

            modelBuilder.Entity("SkyEditor.ExtensionWebsite.Models.ExtensionModels.Extension", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author")
                        .IsRequired();

                    b.Property<Guid>("CollectionID");

                    b.Property<string>("Description");

                    b.Property<string>("ExtensionID")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("CollectionID");

                    b.ToTable("Extension");
                });

            modelBuilder.Entity("SkyEditor.ExtensionWebsite.Models.ExtensionModels.ExtensionCollection", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid?>("ParentID");

                    b.HasKey("ID");

                    b.HasIndex("ParentID");

                    b.ToTable("ExtensionCollection");
                });

            modelBuilder.Entity("SkyEditor.ExtensionWebsite.Models.ExtensionModels.ExtensionVersion", b =>
                {
                    b.Property<Guid>("ID");

                    b.Property<string>("Version");

                    b.Property<Guid>("ExtensionID");

                    b.HasKey("ID", "Version");

                    b.HasIndex("ExtensionID");

                    b.ToTable("ExtensionVersion");
                });

            modelBuilder.Entity("SkyEditor.ExtensionWebsite.Models.ExtensionModels.ApiKey", b =>
                {
                    b.HasOne("SkyEditor.ExtensionWebsite.Models.ExtensionModels.ExtensionCollection", "Collection")
                        .WithMany()
                        .HasForeignKey("CollectionID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SkyEditor.ExtensionWebsite.Models.ExtensionModels.Extension", b =>
                {
                    b.HasOne("SkyEditor.ExtensionWebsite.Models.ExtensionModels.ExtensionCollection", "Collection")
                        .WithMany()
                        .HasForeignKey("CollectionID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SkyEditor.ExtensionWebsite.Models.ExtensionModels.ExtensionCollection", b =>
                {
                    b.HasOne("SkyEditor.ExtensionWebsite.Models.ExtensionModels.ExtensionCollection", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentID");
                });

            modelBuilder.Entity("SkyEditor.ExtensionWebsite.Models.ExtensionModels.ExtensionVersion", b =>
                {
                    b.HasOne("SkyEditor.ExtensionWebsite.Models.ExtensionModels.Extension", "Extension")
                        .WithMany("Versions")
                        .HasForeignKey("ExtensionID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
