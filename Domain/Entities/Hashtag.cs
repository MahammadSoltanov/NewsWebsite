namespace Domain.Entities
{
    public class Hashtag : BaseEntity
    {
        public string Title { get; set; }
        public ICollection<PostHashtag> PostHashtags { get; set; }
    }
}
