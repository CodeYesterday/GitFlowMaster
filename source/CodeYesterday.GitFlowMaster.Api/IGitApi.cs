namespace CodeYesterday.GitFlowMaster.Api;

[PublicAPI]
public interface IGitApi
{
    Task FetchAsync(bool prune = false, bool pruneTags = false);

    Task PullAsync();

    Task PushAsync();

    Task GetBranchListAsync();
}
