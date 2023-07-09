﻿namespace Domain.Entities
{
    public class Category : BaseAuditableEntity
    {
        public string Description { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<CategoryTranslation> CategoryTranslations { get; set; }
    }
}
