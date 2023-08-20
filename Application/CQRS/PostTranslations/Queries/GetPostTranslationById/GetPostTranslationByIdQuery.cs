using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.PostTranslations.Queries.GetPostTranslationById;

public record GetPostTranslationByIdQuery(int Id) : IRequest<PostTranslationDto>
{
}

public class GetPostTranslationByIdQueryHandler : IRequestHandler<GetPostTranslationByIdQuery, PostTranslationDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPostTranslationByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PostTranslationDto> Handle(GetPostTranslationByIdQuery request, CancellationToken cancellationToken)
    {
        var postTranslation = await _context.PostTranslations.FindAsync(new object[] { request.Id }, cancellationToken);

        if(postTranslation == null)
        {
            throw new NotFoundException(nameof(PostTranslation), request.Id);
        }

        var postTranslationDto = _mapper.Map<PostTranslationDto>(postTranslation);

        return postTranslationDto;
    }
}

