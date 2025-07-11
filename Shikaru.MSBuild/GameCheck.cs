using Microsoft.Build.Framework;

namespace Shikaru.MSBuild;

// TODO: Check game version
public class GameCheck : ITask
{
    public IBuildEngine BuildEngine { get; set; } = null!;
    public ITaskHost HostObject { get; set; } = null!;

    [Required] public string GamePathManaged { get; set; } = null!;

    public bool Execute()
    {
        return true;
    }
}
