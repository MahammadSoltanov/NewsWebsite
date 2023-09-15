using Application.Common.Models;
using Application.CQRS.Languages.Queries.GetLanguageByCode;
using Application.CQRS.Posts.Commands.ChangePostsStatuses;
using Application.CQRS.Posts.Queries.GetPosts;
using Application.CQRS.PostTranslations.Commands.ChangePostTranslationsStatuses;
using Application.CQRS.PostTranslations.Queries.GetPostTranslations;
using Application.CQRS.PostTranslations.Queries.GetPostTranslationsByLanguageId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Presentation.Pages.Admin.Approve
{
    public class PostsStatusModel : PageModel
    {
        private readonly IMediator _mediator;

        public PostsStatusModel(IMediator mediator)
        {
            _mediator = mediator;
        }


        public List<PostDto> Posts { get; set; }
        public List<PostTranslationDto> Translations { get; set; }

        public async Task OnGetAsync()
        {
            await UpdateProperties();
        }

        public async Task<IActionResult> OnPostChangeStatusAsync(string selectedvalues)
        {
            List<PostStatusObj> selectedPosts = JsonStringToList(selectedvalues);

            ChangePostsStatusesCommand command = new ChangePostsStatusesCommand()
            {
                ChangedPosts = selectedPosts
            };

            await _mediator.Send(command);
            await UpdateProperties();
            return Page();
        }

        private List<PostStatusObj> JsonStringToList(string json)
        {
            List<PostStatusObj> list = JsonConvert.DeserializeObject<List<PostStatusObj>>(json);
            return list;
        }

        private async Task UpdateProperties()
        {
            Posts = await _mediator.Send(new GetPostsQuery());
            var defaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
            Translations = await _mediator.Send(new GetPostTranslationsByLanguageIdQuery(defaultLanguage.Id));
        }
    }
}
