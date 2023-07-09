using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Hashtags.Queries.GetHashtags;

public record GetHashtagsQuery : IRequest<List<HashtagDto>>;

public class GetHashtagQueryHandler : IRequestHandler<GetHashtagsQuery, List<HashtagDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetHashtagQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<HashtagDto>> Handle(GetHashtagsQuery request, CancellationToken cancellationToken)
    {
        var hashtags = await _context.Hashtags.ToListAsync(cancellationToken);
        var hashtagDtos = _mapper.Map<List<HashtagDto>>(hashtags);
        return hashtagDtos;
    }
}

