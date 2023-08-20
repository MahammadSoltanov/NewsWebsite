using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Posts.Queries.GetPostById;

public record GetPostByIdQuery(int Id) : IRequest<PostDto>
{
}

public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPostByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PostDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _context.Posts.FindAsync(new object[] { request.Id }, cancellationToken);

        if (post == null)
        {
            throw new NotFoundException(nameof(PostTranslation), request.Id);
        }

        var postDto = _mapper.Map<PostDto>(post);

        return postDto;
    }
}



