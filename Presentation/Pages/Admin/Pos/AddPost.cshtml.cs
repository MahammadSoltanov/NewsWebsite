using Application.Common.Models;
using Application.CQRS.Languages.Queries.GetLanguageByCode;
using Application.CQRS.Languages.Queries.GetLanguages;
using Application.CQRS.Users.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Admin.Pos
{
    public class AddPostModel : PageModel
    {
        private readonly IMediator _mediator;

        public AddPostModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<UserDto> Users{ get; set; }
        public List<LanguageDto> Languages { get; set; }
        public LanguageDto DefaultLanguage {get; set;}
        [BindProperty]
        public string Title { get; set;}
        [BindProperty]
        public int AuthorId { get; set;}
        [BindProperty]
        public int LanguageId { get; set;}
        [BindProperty]
        public string TranslationContent { get; set;}
        public async Task OnGetAsync()
        {
            Users = await _mediator.Send(new GetUsersQuery());
            Languages = await _mediator.Send(new GetLanguagesQuery());
            DefaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
        }

        
    }
}
