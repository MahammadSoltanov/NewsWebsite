using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events.PostTranslations;
using MediatR;

namespace Application.CQRS.PostTranslations.Commands.CreatePostTranslation;

public record CreatePostTranslationCommand : IRequest<int>
{
    public int AuthorId { get; set; }
    public int LanguageId { get; set; }
    public int PostId { get; set; }
    public string Title { get; set; }
    public string? Content { get; set; }
}

public class CreatePostTranslationCommandHandler : IRequestHandler<CreatePostTranslationCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreatePostTranslationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePostTranslationCommand request, CancellationToken cancellationToken)
    {
        var entity = new PostTranslation()
        {
            AuthorId = request.AuthorId,
            LanguageId = request.LanguageId,
            PostId = request.PostId,
            Title = request.Title,
            Content = request.Content
        };

        entity.AddDomainEvent(new PostTranslationCreatedEvent(entity));

        _context.PostTranslations.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}