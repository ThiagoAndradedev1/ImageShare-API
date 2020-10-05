using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ImageUploadAPI.Data;
using ImageUploadAPI.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ImageUploadAPI.Dtos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace ImageUploadAPI.Controllers
{
    [Route("api/ImageUpload")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {

        private readonly ImageDbContext _context;
        private readonly IWebHostEnvironment _hostEnviroment;
        private readonly IMapper _mapper;

        public ImageUploadController(ImageDbContext context, IWebHostEnvironment hostEnviroment, IMapper mapper)
        {
            _context = context;
            _hostEnviroment = hostEnviroment;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageReadDto>>> GetAllPeople()
        {
            var imageItems = await _context.Images.ToListAsync();

            return Ok(_mapper.Map<IEnumerable<ImageReadDto>>(imageItems));
        }



        [HttpGet("{id}", Name = "GetImageById")]
        public async Task<ActionResult<ImageReadDto>> GetImageById(int id)
        {
            var imageItem = await _context.Images.FirstOrDefaultAsync(p => p.ImageId == id);

            if (imageItem != null)
            {
                return Ok(_mapper.Map<ImageReadDto>(imageItem));
            } else
            {
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<ActionResult<ImageModel>> UploadImage([FromForm] ImageModel imageModel)
        {
            string wwwRootPath = _hostEnviroment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            imageModel.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath + "/images/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await imageModel.ImageFile.CopyToAsync(fileStream);
            }
            await _context.Images.AddAsync(imageModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetImageById), new { id = imageModel.ImageId }, _mapper.Map<ImageReadDto>(imageModel));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteImage(int id)
        {
            var imageItem = await _context.Images.FirstOrDefaultAsync(p => p.ImageId == id);

            var imagePath = Path.Combine(_hostEnviroment.WebRootPath, "images", imageItem.ImageName);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.Images.Remove(imageItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
