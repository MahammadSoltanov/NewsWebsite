using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Posts.Queries.GetPublishedPosts;

public record GetPublishedPostsQuery : IRequest<List<PostDto>>;

public class GetPublishedPostsQueryHandler : IRequestHandler<GetPublishedPostsQuery, List<PostDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPublishedPostsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PostDto>> Handle(GetPublishedPostsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.Posts
            .Where(p => p.Status == "Published")
            .ToListAsync(cancellationToken);
        var postDtos = _mapper.Map<List<PostDto>>(posts);

        if(postDtos == null)
        {
            return new List<PostDto>();
        }

        return postDtos;
    }
}