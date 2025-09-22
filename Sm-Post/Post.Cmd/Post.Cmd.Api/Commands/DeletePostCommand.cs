using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands
{
    public class DeletePostCommand : BaseCommand
    {
        public string Username { get; set; }// solo chi  creato il post può eliminarlo
    }
}
