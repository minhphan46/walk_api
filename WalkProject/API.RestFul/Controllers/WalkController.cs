using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WalkProject.API.RestFul.DTOs.ApiResponse;
using WalkProject.API.RestFul.DTOs.WalkModel;
using WalkProject.API.RestFul.Repositories.Interfaces;
using WalkProject.API.RestFul.Validators;
using WalkProject.DataModels.Entities;
using System.Net;

namespace NZWalks.RESTful.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalkController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalkController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        // GET Walks
        // GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string filterOn, [FromQuery] string filterQuery,
            [FromQuery] string sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy,
                    isAscending ?? true, pageNumber, pageSize);

            // Map Domain Model to DTO
            var apiResponse = new APISucessResponse(
                    statusCode: HttpStatusCode.OK,
                    message: "Successfully get all walks",
                    data: mapper.Map<List<WalkDto>>(walksDomainModel)
                );

            return Ok(apiResponse);
        }

        // Get Walk By Id
        // GET: /api/Walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            var apiResponse = new APISucessResponse(
                statusCode: HttpStatusCode.OK,
                message: "Successfully get walk by id",
                data: mapper.Map<WalkDto>(walkDomainModel)
            );

            // Map Domain Model to DTO
            return Ok(apiResponse);
        }

        // CREATE Walk
        // POST: /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepository.CreateAsync(walkDomainModel, addWalkRequestDto.CategoryId);

            var apiResponse = new APISucessResponse(
                statusCode: HttpStatusCode.Created,
                message: "Successfully created walk",
                data: mapper.Map<WalkDto>(walkDomainModel)
            );

            // Map Domain model to DTO
            return Ok(apiResponse);
        }


        // Update Walk By Id
        // PUT: /api/Walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {

            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

            walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            var apiResponse = new APISucessResponse(
                statusCode: HttpStatusCode.OK,
                message: "Successfully updated walk",
                data: mapper.Map<WalkDto>(walkDomainModel)
            );

            // Map Domain Model to DTO
            return Ok(apiResponse);
        }


        // Delete a Walk By Id
        // DELETE: /api/Walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await walkRepository.DeleteAsync(id);

            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }

            var apiResponse = new APISucessResponse(
                statusCode: HttpStatusCode.OK,
                message: "Successfully deleted walk",
                data: mapper.Map<WalkDto>(deletedWalkDomainModel)
            );

            // Map Domain Model to DTO
            return Ok(apiResponse);
        }
    }
}
