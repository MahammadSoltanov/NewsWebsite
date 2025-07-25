using Application.Common.Models;
using Application.CQRS.Languages.Queries.GetLanguageByCode;
using Application.CQRS.Posts.Queries.GetPostById;
using Application.CQRS.PostTranslations.Commands.ChangePostTranslationsStatuses;
using Application.CQRS.PostTranslations.Queries.GetPostTranslationsForApproval;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Presentation.Constants;

namespace Presentation.Pages.Admin.Approve
{
    [Authorize(Roles = RoleAccessLevels.AdminAndModerator)]
    public class PostTranslationsStatusModel : PageModel
    {
        private readonly IMediator _mediator;

        public PostTranslationsStatusModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<PostTranslationDto> Posts { get; set; }
        public LanguageDto DefaultLanguage { get; set; }

        public async Task OnGetAsync()
        {
            await UpdateProperties();
        }

        public async Task OnPostChangeStatusAsync(string selectedvalues)
        {
            List<PostTranslationStatusObj> selectedPosts = JsonStringToList(selectedvalues);

            ChangePostTranslationsStatusesCommand command = new ChangePostTranslationsStatusesCommand()
            {
                ChangedTranslations = selectedPosts
            };

            await _mediator.Send(command);
            await UpdateProperties();
        }

        private List<PostTranslationStatusObj> JsonStringToList(string json)
        {
            List<PostTranslationStatusObj> list = JsonConvert.DeserializeObject<List<PostTranslationStatusObj>>(json);
            return list;
        }

        private async Task UpdateProperties()
        {
            Posts = await _mediator.Send(new GetPostTranslationsForApprovalQuery());
            DefaultLanguage = await _mediator.Send(new GetLanguageByCodeQuery(DefaultStrings.DefaultLanguageCode));
        }

        public async Task<string> GetPostStatus(int postId)
        {
            var post = await _mediator.Send(new GetPostByIdQuery(postId));

            return post.Status;
        }
    }
}
