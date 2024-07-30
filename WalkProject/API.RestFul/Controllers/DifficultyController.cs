using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WalkProject.API.RestFul.DTOs.ApiResponse;
using WalkProject.API.RestFul.DTOs.DifficultyModel;
using WalkProject.API.RestFul.Repositories.Interfaces;
using WalkProject.API.RestFul.Validators;
using WalkProject.DataModels.Entities;
using System.Net;

namespace NZWalks.RESTful.Controllers
{
    [Route("api/difficulties")]
    [ApiController]
    public class DifficultyController : ControllerBase
    {
        private readonly IDifficultyRepository difficultyRepository;
        private readonly IMapper mapper;

        public DifficultyController(IDifficultyRepository difficultyRepository, IMapper mapper)
        {
            this.difficultyRepository = difficultyRepository;
            this.mapper = mapper;
        }

        // GET: https://localhost:portnumber/api/difficulties
        [HttpGet]
        public async Task<IActionResult> GetAllDifficulties()
        {
            var difficultyDomain = await difficultyRepository.GetAllAsync();
            var apiResponse = new APISucessResponse(
                statusCode: HttpStatusCode.OK,
                message: "Successfully get all difficulties",
                data: mapper.Map<List<DifficultyDto>>(difficultyDomain)
            );
            return Ok(apiResponse);
        }

        // GET: https://localhost:portnumber/api/difficulties/{id}
        [HttpGet]
        [Route("{difficultyId:Guid}")]
        public async Task<IActionResult> GetDifficultyById([FromRoute] Guid difficultyId)
        {
            var difficultyDomain = await difficultyRepository.GetByIdAsync(difficultyId);

            if (difficultyDomain == null)
            {
                return NotFound();
            }

            // return Ok(mapper.Map<DifficultyDto>(difficultyDomain));
            var apiResponse = new APISucessResponse(
                statusCode: HttpStatusCode.OK,
                message: "Successfully get difficulty by id",
                data: mapper.Map<DifficultyDto>(difficultyDomain)
            );
            return Ok(apiResponse);
        }

        // POST: https://localhost:portnumber/api/difficulties
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateDifficulty([FromBody] AddDifficultyDto addDifficultyRequestDto)
        {
            var difficultyDomainModel = mapper.Map<Difficulty>(addDifficultyRequestDto);

            difficultyDomainModel = await difficultyRepository.CreateAsync(difficultyDomainModel);

            var difficultyDto = mapper.Map<DifficultyDto>(difficultyDomainModel);

            // return CreatedAtAction(nameof(GetDifficultyById), new { difficultyId = difficultyDto.Id }, difficultyDto);
            var apiResponse = new APISucessResponse(
                statusCode: HttpStatusCode.Created,
                message: "Successfully created difficulty",
                data: difficultyDto
            );
            return CreatedAtAction(nameof(GetDifficultyById), new { difficultyId = difficultyDto.Id }, apiResponse);
        }

        // PUT: https://localhost:portnumber/api/difficulties/{id}
        [HttpPut]
        [Route("{difficultyId:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateDifficulty([FromRoute] Guid difficultyId, [FromBody] AddDifficultyDto updateDifficultyRequestDto)
        {
            var difficultyDomainModel = mapper.Map<Difficulty>(updateDifficultyRequestDto);
            // check if the difficulty exists
            difficultyDomainModel = await difficultyRepository.UpdateAsync(difficultyId, difficultyDomainModel);

            if (difficultyDomainModel == null)
            {
                return NotFound();
            }

            var difficultyDto = mapper.Map<DifficultyDto>(difficultyDomainModel);

            var apiResponse = new APISucessResponse(
                statusCode: HttpStatusCode.OK,
                message: "Successfully updated difficulty",
                data: difficultyDto
            );

            return Ok(apiResponse);
        }

        // DELETE: https://localhost:portnumber/api/difficulties/{id}
        [HttpDelete]
        [Route("{difficultyId:Guid}")]
        public async Task<IActionResult> DeleteDifficulty([FromRoute] Guid difficultyId)
        {
            var difficultyDomainModel = await difficultyRepository.DeleteAsync(difficultyId);

            if (difficultyDomainModel == null)
            {
                return NotFound();
            }

            var difficultyDto = mapper.Map<DifficultyDto>(difficultyDomainModel);

            var apiResponse = new APISucessResponse(
                statusCode: HttpStatusCode.OK,
                message: "Successfully deleted difficulty",
                data: difficultyDto
            );

            return Ok(apiResponse);
        }
    }
}
