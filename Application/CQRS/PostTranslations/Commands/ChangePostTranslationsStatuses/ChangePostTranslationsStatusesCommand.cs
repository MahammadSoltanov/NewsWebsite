using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.PostTranslations.Commands.ChangePostTranslationsStatuses;

public record ChangePostTranslationsStatusesCommand : IRequest<Unit>
{
    public List<PostTranslationStatusObj> ChangedTranslations { get; set; }
}

public class ChangePostTranslationsStatusesCommandHandler : IRequestHandler<ChangePostTranslationsStatusesCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private const string PublishedStatus = "Published";

    public ChangePostTranslationsStatusesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ChangePostTranslationsStatusesCommand request, CancellationToken cancellationToken)
    {
        var translations = await _context.PostTranslations.ToListAsync(cancellationToken);

        foreach (var changedPost in request.ChangedTranslations)
        {
            foreach (var translation in translations)
            {
                if (translation.Id == changedPost.Id)
                {
                    if(changedPost.Status == PublishedStatus)
                    {
                        translation.PublishDate = DateTime.Now;
                    }

                    translation.Status = changedPost.Status;
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

}

