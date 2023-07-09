namespace Domain.Entities
{
    public class Hashtag : BaseAuditableEntity
    {
        public string Title { get; set; }
        public ICollection<PostHashtag> PostHashtags { get; set; }
    }
}
