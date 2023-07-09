﻿namespace Domain.Entities
{
    public class Language : BaseAuditableEntity
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public ICollection<CategoryTranslation> CategoryTranslations { get; set; }
        public ICollection<PostTranslation> PostTranslations { get; set; }
    }
}
