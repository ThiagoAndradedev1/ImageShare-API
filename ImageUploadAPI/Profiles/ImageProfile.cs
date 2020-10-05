using AutoMapper;
using ImageUploadAPI.Models;
using ImageUploadAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUploadAPI.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<ImageModel, ImageReadDto>();
        }
    }
}
