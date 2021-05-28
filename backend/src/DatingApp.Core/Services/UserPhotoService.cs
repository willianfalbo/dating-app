using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.Core.Dtos;
using DatingApp.Core.Entities;
using DatingApp.Core.Exceptions;
using DatingApp.Core.Interfaces;
using DatingApp.Core.Interfaces.Database;
using DatingApp.Core.Interfaces.Services;

namespace DatingApp.Core.Services
{
    public class UserPhotoService : IUserPhotoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageUploader _imageUploader;
        private readonly IUserService _userService;
        private readonly IClassMapper _mapper;

        public UserPhotoService(IUnitOfWork unitOfWork, IUserService userService, IImageUploader imageUploader, IClassMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _imageUploader = imageUploader ?? throw new ArgumentNullException(nameof(imageUploader));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<UserPhoto> GetMainPhotoForUser(int userId) =>
            _unitOfWork.UserPhotos.GetMainPhotoForUser(userId);

        public Task<UserPhoto> GetUserPhoto(int photoId) =>
            _unitOfWork.UserPhotos.GetUserPhoto(photoId);

        public async Task<UserPhoto> UploadUserPhoto(int userId, UserPhotoForCreationDto userDto)
        {
            var user = await _userService.GetUser(userId, true);

            var file = userDto.File;

            if (file is null || file.Length <= 0)
                throw new BadRequestException("The photo was not provided.");

            var uploadResult = await _imageUploader.UploadAsync(file);

            var photo = _mapper.To<UserPhoto>(userDto);
            photo.Url = uploadResult.Url;
            photo.PublicId = uploadResult.PublicId;

            if (!user.Photos.Any(u => u.IsMain))
                photo.IsMain = true;

            user.Photos.Add(photo);

            await _unitOfWork.CommitAsync();

            return photo;
        }

        public async Task SetMainPhoto(int userId, int userPhotoId)
        {
            var userFromRepo = await _userService.GetUser(userId, true);

            if (!userFromRepo.Photos.Any(p => p.Id == userPhotoId))
                throw new NotFoundException();

            var userPhotoFromRepo = await this.GetUserPhoto(userPhotoId);

            if (userPhotoFromRepo.IsMain)
                throw new BadRequestException("This is already the main photo.");

            var currentMainPhoto = await this.GetMainPhotoForUser(userId);

            currentMainPhoto.IsMain = false;
            userPhotoFromRepo.IsMain = true;

            await _unitOfWork.CommitAsync();
        }

        public async Task DeletePhoto(int userId, int userPhotoId)
        {
            var userFromRepo = await _userService.GetUser(userId, true);

            if (!userFromRepo.Photos.Any(p => p.Id == userPhotoId))
                throw new NotFoundException();

            var userPhotoFromRepo = await this.GetUserPhoto(userPhotoId);

            if (userPhotoFromRepo.IsMain)
                throw new BadRequestException("You cannot delete your main photo.");

            if (!string.IsNullOrWhiteSpace(userPhotoFromRepo.PublicId))
                await _imageUploader.DeleteAsync(userPhotoFromRepo.PublicId);

            _unitOfWork.UserPhotos.Remove(userPhotoFromRepo);

            await _unitOfWork.CommitAsync();
        }

        public Task<IEnumerable<object>> GetPhotosForModeration() =>
            _unitOfWork.UserPhotos.GetPhotosForModeration();

        public async Task ApprovePhoto(int photoId)
        {
            var photo = await this.GetUserPhoto(photoId);
            photo.IsApproved = true;

            await _unitOfWork.CommitAsync();
        }

        public async Task RejectPhoto(int photoId)
        {
            var photo = await this.GetUserPhoto(photoId);

            if (photo.IsMain)
                throw new BadRequestException("You cannot reject the main photo.");

            if (photo.PublicId != null)
                await _imageUploader.DeleteAsync(photo.PublicId);

            _unitOfWork.UserPhotos.Remove(photo);

            await _unitOfWork.CommitAsync();
        }
    }
}