using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.PostTranslations.Queries.GetPostTranslationsForApproval
{
    public record GetPostTranslationsForApprovalQuery : IRequest<List<PostTranslationDto>>;

    public class GetPostTranslationsForApprovalQueryHandler : IRequestHandler<GetPostTranslationsForApprovalQuery, List<PostTranslationDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPostTranslationsForApprovalQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PostTranslationDto>> Handle(GetPostTranslationsForApprovalQuery request, CancellationToken cancellationToken)
        {
            var translations= await _context.PostTranslations.ToListAsync(cancellationToken);

            var translationDtos = _mapper.Map<List<PostTranslationDto>>(translations);

            return translationDtos;
        }
    }
}
