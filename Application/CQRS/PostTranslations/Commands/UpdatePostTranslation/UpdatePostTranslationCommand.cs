using Application.Common.Interfaces;
using Application.CQRS.PostTranslations.Commands.CreatePostTranslation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.PostTranslations.Commands.UpdatePostTranslation;

public record UpdatePostTranslationCommand : IRequest<string>
{
    public int PostId { get; set; }
    public int LanguageId { get; set; }
    public int AuthorId {get; set; }
    public string Title { get; set; }
    public string TranslationContent { get; set; }

}

public class UpdatePostTranslationCommandHandler : IRequestHandler<UpdatePostTranslationCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public UpdatePostTranslationCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<string> Handle(UpdatePostTranslationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PostTranslations.FirstOrDefaultAsync(
            pt => pt.PostId == request.PostId && pt.LanguageId == request.LanguageId, 
            cancellationToken);

        if(entity == null) 
        {
            var command = new CreatePostTranslationCommand()
            {
                PostId = request.PostId,
                LanguageId = request.LanguageId,
                Title = request.Title,
                AuthorId = request.AuthorId,
                Content = request.TranslationContent
            };

            int id = await _mediator.Send(command);
            string message = $"Post translation with Id = {id} was successfully created";
            return message;
        }

        else
        {
            entity.AuthorId = request.AuthorId;
            entity.Title = request.Title != null ? request.Title : entity.Title;
            entity.Content = request.TranslationContent != null ? request.TranslationContent : entity.Content;
            
            await _context.SaveChangesAsync(cancellationToken);
            
            string message = $"Post translation with Id = {entity.Id} was successfully updated";
            return message;
        }

    }
}
