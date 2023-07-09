using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Hashtags.Queries.GetHashtagById;

public record GetHashtagByIdQuery(int Id) : IRequest<HashtagDto>;

public class GetHashtagByIdQueryHandler : IRequestHandler<GetHashtagByIdQuery, HashtagDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetHashtagByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<HashtagDto> Handle(GetHashtagByIdQuery request, CancellationToken cancellationToken)
    {
        var hashtag = await _context.Hashtags.FindAsync(new object[] { request.Id }, cancellationToken);

        if (hashtag == null)
        {
            throw new NotFoundException(nameof(Hashtag), request.Id);
        }

        var hashtagDto = _mapper.Map<HashtagDto>(hashtag);

        return hashtagDto;
    }
}
