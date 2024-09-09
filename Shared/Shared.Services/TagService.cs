using Shared.Models;
using Shared.Repositories.Interfaces;

namespace Shared.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    

    public TagService(ITagRepository tagRepository, IThunderFileRepository fileRepository)
    {
        _tagRepository = tagRepository;
        
    }

    public async Task AddTagToFileAsync(Guid fileId, string tagName)
    {
        string file = null;
        if (file == null)
            throw new FileNotFoundException("File not found.");

        var tag = new Tag
        {
            FileId = fileId,
            TagName = tagName
        };

        await _tagRepository.AddAsync(tag);
    }

    public async Task RemoveTagFromFileAsync(Guid fileId, string tagName)
    {
        var tag = await _tagRepository.GetByFileIdAndTagNameAsync(fileId, tagName);
        if (tag == null)
            throw new InvalidOperationException("Tag not found.");

        await _tagRepository.DeleteAsync(tag);
    }

    public async Task<IEnumerable<string>> GetTagsForFileAsync(Guid fileId)
    {
        return await _tagRepository.GetTagsByFileIdAsync(fileId);
    }
}