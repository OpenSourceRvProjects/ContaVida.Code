using ContaVida.MVC.Backend.Infraestructure;
using ContaVida.MVC.DataAccess.DataAccess;
using ContaVida.MVC.Models.Images;
using ContaVida.MVC.Models.Profile;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ContaVida.MVC.Backend.Services
{
    public class ProfileService : IProfileService
    {

        private IHttpContextAccessor _accessor;
        private ContaVidaDbContext _dbContext;
        public ProfileService(IHttpContextAccessor accessor, ContaVidaDbContext dbContext)
        {
            _accessor = accessor;
            _dbContext = dbContext;

        }

        public virtual async Task<ProfileDataModel> GetProfileData()
        {
            var profile = await GetUserProfile();
            return new ProfileDataModel()
            {
                Email = profile.User.Email,
                LastName1 = profile.LastName1,
                LastName2 = profile.LastName2,
                Name = profile.Name,
                Phone = profile.Pohone,
                AllowAccess = profile.User.AllowSysAdminAccess
            };
        }

        public async Task<ImageListModel> GetProfileImages()
        {
            var profile = await GetUserProfile();
            if (profile.DefaultPetPhotos == null)
                return new ImageListModel();

            return JsonSerializer.Deserialize<ImageListModel>(profile.DefaultPetPhotos);
        }

        public async Task SaveProfileImages(ImageListModel images)
        {
            var profile = await GetUserProfile();
            profile.DefaultPetPhotos = JsonSerializer.Serialize(images);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<PersonalProfile> GetUserProfile()
        {
            var currentUserID = Guid.Parse(_accessor.HttpContext.Session.GetString("userID"));
            var userProfile = await _dbContext.Users.Include(i => i.PersonalProfiles).FirstOrDefaultAsync(f => f.Id == currentUserID);
            var profile = userProfile.PersonalProfiles.FirstOrDefault();

            return profile;
        }
    }
}
