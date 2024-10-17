using AutoMapper;
using PostService.DTOs;
using PostService.Interfaces;
using PostService.Models;

namespace PostService.Services
{
    public class SavedPostService : ISavedPostService
    {
        private readonly ISavedPostRepository _savedPostRepository;
        private readonly IMapper _mapper;

        public SavedPostService(ISavedPostRepository savedPostRepository, IMapper mapper)
        {
            _savedPostRepository = savedPostRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SavedPost>> GetSavedPostsByUserIdAsync(string userId)
        {
            return await _savedPostRepository.GetSavedPostsByUserIdAsync(userId);
        }

        public async Task SavePostAsync(string postId, string userId)
        {
            await _savedPostRepository.SavePostAsync(postId, userId);
        }

        public async Task UnsavePostAsync(string postId, string userId)
        {
            await _savedPostRepository.UnsavePostAsync(postId, userId);
        }

        public async Task RemoveSavedPostAsync(string userId, string postId)
        {
            await _savedPostRepository.RemoveSavedPostAsync(userId, postId);
        }
    }
}