using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CQRS.Hashtags.Queries.GetHashtagById;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Hashtags.Queries.GetHashtagsByPostId;

public record GetHashtagsByPostIdQuery(int PostId) : IRequest<List<HashtagDto>>;

public record GetHashtagsByPostIdQueryHandler : IRequestHandler<GetHashtagsByPostIdQuery, List<HashtagDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public GetHashtagsByPostIdQueryHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<List<HashtagDto>> Handle(GetHashtagsByPostIdQuery request, CancellationToken cancellationToken)
    {
        var postHashtagS = await _context.PostHashtags
            .Where(ph => ph.PostId == request.PostId)
            .ToListAsync(cancellationToken);

        if (postHashtagS == null || postHashtagS.Count == 0) 
        {
            return new List<HashtagDto>();
        }

        var hashtagSDto = new List<HashtagDto>();

        foreach(var postHashtag in postHashtagS)
        {
            var hashtagDto = await _mediator.Send(new GetHashtagByIdQuery(postHashtag.HashtagId));
            
            if(hashtagDto != null ) 
            {
                hashtagSDto.Add(hashtagDto);
            }            
        }
        
        return hashtagSDto;
    }
}

