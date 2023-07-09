using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-RKJKQSG\SQLEXPRESS; Database=NewsDB; Trusted_Connection=True; Encrypt=False;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasKey(c => c.Id);
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
                .HasOne(ct => ct.Category)
                .WithMany(c => c.CategoryTranslations)
                .HasForeignKey(ct => ct.CategoryId);
            modelBuilder.Entity<CategoryTranslation>()
                .HasOne(ct => ct.Language)
                .WithMany(l => l.CategoryTranslations)
                .HasForeignKey(ct => ct.LanguageId);

            modelBuilder.Entity<Hashtag>()
                .HasKey(h => h.Id);
            modelBuilder.Entity<Hashtag>()
                .Property(c => c.Id)
                .UseIdentityColumn(1, 1);
            modelBuilder.Entity<Hashtag>()
                .Property(h => h.Title)
                .HasMaxLength(50)
                .HasColumnType("VARCHAR");

            modelBuilder.Entity<Language>()
                .HasKey(l => l.Id);
            modelBuilder.Entity<Language>()
                .Property(c => c.Id)
                .UseIdentityColumn(1, 1);
            modelBuilder.Entity<Language>()
                .Property(l => l.Code)
                .HasMaxLength(5)
                .HasColumnType("VARCHAR");

            modelBuilder.Entity<Post>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Post>()
                .Property(c => c.Id)
                .UseIdentityColumn(1, 1);
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId);
            modelBuilder.Entity<Post>()
                .Property(p => p.Title)
                .IsRequired();
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
                .HasForeignKey(ph => ph.HashtagId);
            modelBuilder.Entity<PostHashtag>()
                .HasOne(ph => ph.Post)
                .WithMany(p => p.PostHashtags)
                .HasForeignKey(ph => ph.PostId);

            modelBuilder.Entity<PostTranslation>()
                .HasKey(pt => pt.Id);
            modelBuilder.Entity<PostTranslation>()
                .Property(c => c.Id)
                .UseIdentityColumn(1, 1);
            modelBuilder.Entity<PostTranslation>()
                .HasOne(pt => pt.User)
                .WithMany(u => u.PostTranslations)
                .HasForeignKey(pt => pt.AuthorId);
            modelBuilder.Entity<PostTranslation>()
                .HasOne(pt => pt.Language)
                .WithMany(l => l.PostTranslations)
                .HasForeignKey(pt => pt.LanguageId);
            modelBuilder.Entity<PostTranslation>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTranslations)
                .HasForeignKey(pt => pt.PostId);
            modelBuilder.Entity<PostTranslation>()
                .Property(pt => pt.Content)
                .HasColumnType("text");

            modelBuilder.Entity<Role>()
                .HasKey(r => r.Id);
            modelBuilder.Entity<Role>()
                .Property(c => c.Id)
                .UseIdentityColumn(1, 1);
            modelBuilder.Entity<Role>()
                .Property(r => r.Title)
                .HasMaxLength(50)
                .HasColumnType("VARCHAR")
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<User>()
                .Property(c => c.Id)
                .UseIdentityColumn(1, 1);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
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
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostHashtag> PostHashtags { get; set; }
        public DbSet<PostTranslation> PostTranslations { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
