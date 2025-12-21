using ContaVida.MVC.Models.Images;
using ContaVida.MVC.Models.Profile;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Backend.Infraestructure
{
    public interface IProfileService
    {
        Task SaveProfileImages(ImageListModel images);
        Task<ImageListModel> GetProfileImages();
        Task<ProfileDataModel> GetProfileData();
    }
}
