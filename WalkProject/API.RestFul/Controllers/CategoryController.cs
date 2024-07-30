using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WalkProject.API.RestFul.Repositories.Interfaces;
using WalkProject.API.RestFul.DTOs.ApiResponse;
using WalkProject.API.RestFul.DTOs.CategoryModel;
using WalkProject.API.RestFul.Validators;
using WalkProject.DataModels.Entities;

namespace NZWalks.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        // GET: https://localhost:portnumber/api/categories
        [HttpGet]
        public async Task<IActionResult> GetAllCatergories()
        {
            var categoryDomain = await categoryRepository.GetAllAsync();
            var apiResponse = new APISucessResponse(
                statusCode: HttpStatusCode.OK,
                message: "Successfully get all categories",
                data: mapper.Map<List<CategoryDto>>(categoryDomain)
            );
            return Ok(apiResponse);
        }

        // GET: https://localhost:portnumber/api/categories/{id}
        [HttpGet]
        [Route("{categoryId:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid categoryId)
        {
            var categoryDomain = await categoryRepository.GetByIdAsync(categoryId);

            if (categoryDomain == null)
            {
                return NotFound();
            }

            // return Ok(mapper.Map<CategoryDto>(categoryDomain));
            var apiResponse = new APISucessResponse(
                statusCode: HttpStatusCode.OK,
                message: "Successfully get category by id",
                data: mapper.Map<CategoryDto>(categoryDomain)
            );
            return Ok(apiResponse);
        }

        // POST: https://localhost:portnumber/api/categories
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateCategory([FromBody] AddCategoryDto addCategoryRequestDto)
        {
            var categoryDomainModel = mapper.Map<Category>(addCategoryRequestDto);

            categoryDomainModel = await categoryRepository.CreateAsync(categoryDomainModel);

            var categoryDto = mapper.Map<CategoryDto>(categoryDomainModel);

            // return CreatedAtAction(nameof(GetCategoryById), new { categoryId = categoryDto.Id }, categoryDto);
            var apiResponse = new APISucessResponse(
                statusCode: HttpStatusCode.Created,
                message: "Successfully created category",
                data: categoryDto
            );
            return CreatedAtAction(nameof(GetCategoryById), new { categoryId = categoryDto.Id }, apiResponse);
        }

        // PUT: https://localhost:portnumber/api/categories/{id}
        [HttpPut]
        [Route("{categoryId:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid categoryId, [FromBody] AddCategoryDto updateCategoryRequestDto)
        {
            var categoryDomainModel = mapper.Map<Category>(updateCategoryRequestDto);
            // check if the category exists
            categoryDomainModel = await categoryRepository.UpdateAsync(categoryId, categoryDomainModel);

            if (categoryDomainModel == null)
            {
                return NotFound();
            }

            var categoryDto = mapper.Map<CategoryDto>(categoryDomainModel);

            var apiResponse = new APISucessResponse(
                statusCode: HttpStatusCode.OK,
                message: "Successfully updated category",
                data: categoryDto
            );

            return Ok(apiResponse);
        }

        // DELETE: https://localhost:portnumber/api/categories/{id}
        [HttpDelete]
        [Route("{categoryId:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid categoryId)
        {
            var categoryDomainModel = await categoryRepository.DeleteAsync(categoryId);

            if (categoryDomainModel == null)
            {
                return NotFound();
            }

            var categoryDto = mapper.Map<CategoryDto>(categoryDomainModel);

            var apiResponse = new APISucessResponse(
                statusCode: HttpStatusCode.OK,
                message: "Successfully deleted category",
                data: categoryDto
            );

            return Ok(apiResponse);
        }
    }
}
