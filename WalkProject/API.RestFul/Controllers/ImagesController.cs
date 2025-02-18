﻿using Microsoft.AspNetCore.Mvc;
using WalkProject.API.RestFul.DTOs.ApiResponse;
using WalkProject.API.RestFul.DTOs.MediaModel;
using WalkProject.API.RestFul.Repositories.Interfaces;
using WalkProject.DataModels.Entities;
using Path = System.IO.Path;

namespace NZWalks.RESTful.Controllers
{
    [Route("api/media")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        // POST: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);

            if (ModelState.IsValid)
            {
                // convert DTO to Domain model
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                };


                // User repository to upload image
                await imageRepository.Upload(imageDomainModel);
                var apiResponse = new APISucessResponse(
                        statusCode: System.Net.HttpStatusCode.OK,
                        message: "Successfully uploaded image",
                        data: imageDomainModel
                    );

                return Ok(imageDomainModel);

            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file.");
            }
        }
    }
}
