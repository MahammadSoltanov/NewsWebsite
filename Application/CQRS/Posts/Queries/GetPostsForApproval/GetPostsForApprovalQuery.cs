using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Posts.Queries.GetPostsForApproval;

public record GetPostsForApprovalQuery : IRequest<List<PostDto>>;


public class GetPostsForApprovalQueryHandler : IRequestHandler<GetPostsForApprovalQuery, List<PostDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPostsForApprovalQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PostDto>> Handle(GetPostsForApprovalQuery request, CancellationToken cancellationToken)
    {
        var posts = await _context.Posts.ToListAsync(cancellationToken);

        var postDtos = _mapper.Map<List<PostDto>>(posts);

        return postDtos;
    }
}
