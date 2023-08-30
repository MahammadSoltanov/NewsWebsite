using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Posts.Queries.GetPostsByCategoryId;

public record GetPostsByCategoryIdQuery(int CategoryId) : IRequest<List<PostDto>>;

public class GetPostsByCategoryIdQueryHandler : IRequestHandler<GetPostsByCategoryIdQuery, List<PostDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPostsByCategoryIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PostDto>> Handle(GetPostsByCategoryIdQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.Posts
            .Where(p => p.CategoryId == request.CategoryId)
            .ToListAsync(cancellationToken);

        var postDtos = _mapper.Map<List<PostDto>>(posts);

        return postDtos;
    }
}
