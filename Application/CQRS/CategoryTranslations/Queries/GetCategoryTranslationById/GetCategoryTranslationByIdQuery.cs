using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.CategoryTranslations.Queries.GetCategoryTranslationById;

public record GetCategoryTranslationByIdQuery(int Id) : IRequest<CategoryTranslationDto>
{
}

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryTranslationByIdQuery, CategoryTranslationDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context; 
        _mapper = mapper;
    }

    public async Task<CategoryTranslationDto> Handle(GetCategoryTranslationByIdQuery request, CancellationToken cancellationToken)
    {
        var translation = await _context.CategoryTranslations.FindAsync(new object[] {request.Id} ,cancellationToken);
        
        if(translation == null) 
        {
            throw new NotFoundException(nameof(CategoryTranslation), request.Id);
        }

        var translationDto = _mapper.Map<CategoryTranslationDto>(translation);

        return translationDto;
    }
}

