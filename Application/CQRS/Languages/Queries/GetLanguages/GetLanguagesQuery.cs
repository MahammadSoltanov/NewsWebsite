using MediatR;
using Application.Common.Models;
using Application.Common.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Languages.Queries.GetLanguages;

public record GetLanguagesQuery : IRequest<List<LanguageDto>>;

public class GetLanguagesQueryHahndler : IRequestHandler<GetLanguagesQuery, List<LanguageDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLanguagesQueryHahndler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<LanguageDto>> Handle(GetLanguagesQuery request, CancellationToken cancellationToken)
    {
        var languages = await _context.Languages.ToListAsync(cancellationToken);
        var languagesDto = _mapper.Map<List<LanguageDto>>(languages);
        return languagesDto;
    }

}