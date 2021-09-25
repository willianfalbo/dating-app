using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Core.Dtos.Photos;
using DatingApp.Core.Entities;
using DatingApp.Core.Exceptions;
using DatingApp.Core.Interfaces.Database;
using DatingApp.Core.Interfaces.Files;
using DatingApp.Core.Interfaces.Mappers;
using DatingApp.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace DatingApp.Infrastructure.Services
{
    public class PhotosService : IPhotosService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageUploader _imageUploader;
        private readonly IUsersService _usersService;
        private readonly IClassMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ISlackService _slackService;

        public PhotosService(
            IUnitOfWork unitOfWork,
            IUsersService usersService,
            IImageUploader imageUploader,
            IClassMapper mapper,
            IConfiguration configuration,
            ISlackService slackService
        )
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _imageUploader = imageUploader ?? throw new ArgumentNullException(nameof(imageUploader));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _slackService = slackService ?? throw new ArgumentNullException(nameof(slackService));
        }

        public Task<Photo> GetMainPhoto(int userId) =>
            _unitOfWork.Photos.GetMainPhoto(userId);

        public Task<Photo> GetPhoto(int photoId) =>
            _unitOfWork.Photos.GetPhoto(photoId);

        public async Task<Photo> UploadPhoto(int userId, PhotoForCreationDto userDto)
        {
            var user = await _usersService.GetUser(userId, true);

            var file = userDto.File;

            if (file is null || file.Length <= 0)
                throw new BadRequestException("The photo was not provided.");

            var uploadResult = await _imageUploader.UploadAsync(file);

            var photo = _mapper.To<Photo>(userDto);
            photo.Url = uploadResult.Url;
            photo.PublicId = uploadResult.PublicId;

            if (!user.Photos.Any(u => u.IsMain))
                photo.IsMain = true;

            user.Photos.Add(photo);

            await _unitOfWork.CommitAsync();

            return photo;
        }

        public async Task SetMainPhoto(int userId, int photoId)
        {
            var user = await _usersService.GetUser(userId, true);
            if (!user.Photos.Any(p => p.Id == photoId))
                throw new NotFoundException();

            var photo = await this.GetPhoto(photoId);
            if (photo.IsMain)
                throw new BadRequestException("This is already the main photo.");

            // unset current main photo
            var currentMainPhoto = await this.GetMainPhoto(userId);
            currentMainPhoto.IsMain = false;

            photo.IsMain = true;

            await _unitOfWork.CommitAsync();
        }

        public async Task DeletePhoto(int userId, int photoId)
        {
            var user = await _usersService.GetUser(userId, true);
            if (!user.Photos.Any(p => p.Id == photoId))
                throw new NotFoundException();

            var photo = await this.GetPhoto(photoId);
            if (photo.IsMain)
                throw new BadRequestException("You cannot delete your main photo.");

            if (!string.IsNullOrWhiteSpace(photo.PublicId))
                await _imageUploader.DeleteAsync(photo.PublicId);

            _unitOfWork.Photos.Remove(photo);

            await _unitOfWork.CommitAsync();
        }

        public Task<IEnumerable<object>> GetPhotosForModeration() =>
            _unitOfWork.Photos.GetPhotosForModeration();

        public async Task ApprovePhoto(int photoId)
        {
            var photo = await this.GetPhoto(photoId);
            photo.IsApproved = true;

            await _unitOfWork.CommitAsync();
        }

        public async Task RejectPhoto(int photoId)
        {
            var photo = await this.GetPhoto(photoId);

            if (photo.IsMain)
                throw new BadRequestException("You cannot reject the main photo.");

            if (photo.PublicId != null)
                await _imageUploader.DeleteAsync(photo.PublicId);

            _unitOfWork.Photos.Remove(photo);

            await _unitOfWork.CommitAsync();

            await _slackService.PostChatMessageAsync(
                channelName: _configuration["Slack:Channels:RejectedPhotos"],
                message: $"A photo for the user `{photo.User.UserName}` was rejected. The PublicID is `{photo.PublicId}`.",
                ignoreErrors: true
            );
        }
    }
}
