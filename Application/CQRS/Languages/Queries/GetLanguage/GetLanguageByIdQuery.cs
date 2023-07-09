using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Languages.Queries.GetLanguage;

public record GetLanguageByIdQuery : IRequest<LanguageDto>
{
    public int Id { get; init; }
}

public class GetLanguageByIdQueryHandler : IRequestHandler<GetLanguageByIdQuery, LanguageDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetLanguageByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LanguageDto> Handle(GetLanguageByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Languages.FindAsync(new object[] { request.Id }, cancellationToken);            
        if(entity == null) 
        {
            throw new NotFoundException(nameof(Language), request.Id);
        }

        var languageDto = _mapper.Map<LanguageDto>(entity);
        return languageDto;
    }
}
