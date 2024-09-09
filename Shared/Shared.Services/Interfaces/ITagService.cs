public interface ITagService
{
    Task AddTagToFileAsync(Guid fileId, string tagName);
    Task RemoveTagFromFileAsync(Guid fileId, string tagName);
    Task<IEnumerable<string>> GetTagsForFileAsync(Guid fileId);
}