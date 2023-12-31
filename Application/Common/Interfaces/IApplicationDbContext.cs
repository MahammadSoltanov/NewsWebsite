﻿using Application.Common.Models;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Category> Categories { get; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; }
        public DbSet<Hashtag> Hashtags { get; }
        public DbSet<Language> Languages { get; }
        public DbSet<Post> Posts { get; }
        public DbSet<PostHashtag> PostHashtags { get; }
        public DbSet<PostTranslation> PostTranslations { get; }
        public DbSet<Role> Roles { get; }
        public DbSet<User> Users { get; }
        public DbSet<ImageModel> Images{ get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
