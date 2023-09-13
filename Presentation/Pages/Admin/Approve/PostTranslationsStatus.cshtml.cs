using Application.Common.Models;
using Application.CQRS.Languages.Queries.GetLanguageByCode;
using Application.CQRS.Posts.Commands.ChangePostsStatuses;
using Application.CQRS.PostTranslations.Queries.GetPostTranslations;
using Application.CQRS.PostTranslations.Queries.GetPostTranslationsByLanguageId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Presentation.Pages.Admin.Approve
{
    [Authorize(Roles = "Admin")]
    public class PostTranslationsStatusModel : PageModel
    {
        private readonly IMediator _mediator;

        public PostTranslationsStatusModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<PostTranslationDto> Posts { get; set; }

        public async Task OnGetAsync()
        {
            await UpdateProperties();
        }

        public async Task OnPostChangeStatusAsync(string selectedvalues)
        {
            List<PostStatusObj> selectedPosts = JsonStringToList(selectedvalues);

            ChangePostsStatusesCommand command = new ChangePostsStatusesCommand()
            {
                ChangedPosts = selectedPosts
            };

            await _mediator.Send(command);
            await UpdateProperties();
        }

        private List<PostStatusObj> JsonStringToList(string json)
        {
            List<PostStatusObj> list = JsonConvert.DeserializeObject<List<PostStatusObj>>(json);           
            return list;
        }

        private async Task UpdateProperties()
        {
            Posts = await _mediator.Send(new GetPostTranslationsQuery());
        }
    }
}
