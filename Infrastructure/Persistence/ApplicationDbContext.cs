﻿using Application;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ImageModel>()
                .HasKey(i => i.Id);
            modelBuilder.Entity<ImageModel>()
                .HasIndex(i => i.Url)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Description)
                .IsUnique();
            modelBuilder.Entity<Category>()
                .Property(c => c.Id)
                .UseIdentityColumn(1, 1);
            modelBuilder.Entity<Category>()
                .Property(c => c.Description)
                .HasMaxLength(200)
                .HasColumnType("VARCHAR");

            modelBuilder.Entity<CategoryTranslation>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<CategoryTranslation>()
                .Property(c => c.Id)
                .UseIdentityColumn(1, 1);
            modelBuilder.Entity<CategoryTranslation>()
                .Property(ct => ct.Title)
                .HasMaxLength(200);
            modelBuilder.Entity<CategoryTranslation>()
                .HasIndex(ct => ct.Title)
                .IsUnique();
            modelBuilder.Entity<CategoryTranslation>()
                .HasOne(ct => ct.Category)
                .WithMany(c => c.CategoryTranslations)
                .HasForeignKey(ct => ct.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CategoryTranslation>()
                .HasOne(ct => ct.Language)
                .WithMany(l => l.CategoryTranslations)
                .HasForeignKey(ct => ct.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Hashtag>()
                .HasKey(h => h.Id);
            modelBuilder.Entity<Hashtag>()
                .Property(c => c.Id)
                .UseIdentityColumn(1, 1);
            modelBuilder.Entity<Hashtag>()
                .Property(h => h.Title)
                .HasMaxLength(50)
                .HasColumnType("VARCHAR");
            modelBuilder.Entity<Hashtag>()
                .HasIndex(h => h.Title)
                .IsUnique();

            modelBuilder.Entity<Language>()
                .HasKey(l => l.Id);
            modelBuilder.Entity<Language>()
                .Property(c => c.Id)
                .UseIdentityColumn(1, 1);
            modelBuilder.Entity<Language>()
                .Property(l => l.Code)
                .HasMaxLength(5)
                .HasColumnType("VARCHAR");
            modelBuilder.Entity<Language>()
                .HasIndex(l => l.Code)
                .IsUnique();
            modelBuilder.Entity<Language>()
                .HasIndex(l => l.Title)
                .IsUnique();

            modelBuilder.Entity<Post>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Post>()
                .Property(c => c.Id)
                .UseIdentityColumn(1, 1);
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Post>()
                .Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("VARCHAR");

            modelBuilder.Entity<PostHashtag>()
                .HasKey(ph => new { ph.PostId, ph.HashtagId });
            modelBuilder.Entity<PostHashtag>()
                .HasOne(ph => ph.Hashtag)
                .WithMany(h => h.PostHashtags)
                .HasForeignKey(ph => ph.HashtagId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PostHashtag>()
                .HasOne(ph => ph.Post)
                .WithMany(p => p.PostHashtags)
                .HasForeignKey(ph => ph.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PostHashtag>()
                .Ignore(ph => ph.Id);

            modelBuilder.Entity<PostTranslation>()
                .HasKey(pt => pt.Id);
            modelBuilder.Entity<PostTranslation>()
                .Property(c => c.Id)
                .UseIdentityColumn(1, 1);
            modelBuilder.Entity<PostTranslation>()
                .HasOne(pt => pt.User)
                .WithMany(u => u.PostTranslations)
                .HasForeignKey(pt => pt.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PostTranslation>()
                .HasOne(pt => pt.Language)
                .WithMany(l => l.PostTranslations)
                .HasForeignKey(pt => pt.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PostTranslation>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTranslations)
                .HasForeignKey(pt => pt.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PostTranslation>()
                .Property(pt => pt.Content)
                .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<Role>()
                .HasKey(r => r.Id);
            modelBuilder.Entity<Role>()
                .Property(c => c.Id)
                .UseIdentityColumn(1, 1);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<User>()
                .Property(c => c.Id)
                .UseIdentityColumn(1, 1);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .HasColumnType("VARCHAR")
                .HasMaxLength(25)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.Surname)
                .HasColumnType("VARCHAR")
                .HasMaxLength(25)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .HasColumnType("VARCHAR")
                .HasMaxLength(16)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasColumnType("VARCHAR")
                .HasMaxLength(50)
                .IsRequired();
            modelBuilder.Entity<User>()
                .HasIndex(p => p.Email)
                .IsUnique();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostHashtag> PostHashtags { get; set; }
        public DbSet<PostTranslation> PostTranslations { get; set; }
        public DbSet<ImageModel> Images { get; set; }
    }
}
