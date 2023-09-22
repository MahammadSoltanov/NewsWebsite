using Application.CQRS.Languages.Queries.GetLanguageByCode;
using MediatR;

namespace Application;

public class DefaultContainer
{
    private readonly IMediator _mediator;

    public DefaultContainer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<int> GetDefaultPostLanguageId()
    {
        var language = await _mediator.Send(new GetLanguageByCodeQuery("ENG"));
        return language.Id;
    }
}
