using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalkProject.API.RestFul.DTOs.ApiResponse;
using WalkProject.API.RestFul.DTOs.RegionModel;
using WalkProject.API.RestFul.Repositories.Interfaces;
using WalkProject.API.RestFul.Validators;
using WalkProject.DataModels.Entities;
using System.Net;

namespace NZWalks.RESTful.Controllers
{
    // https://localhost:portnumber/api/regions
    [Route("api/regions")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAllRegions()
        {
            var regionsDomain = await regionRepository.GetAllAsync();

            var apiResponse = new APISucessResponse(
                    statusCode: HttpStatusCode.OK,
                    message: "Successfully get all regions",
                    data: mapper.Map<List<RegionDto>>(regionsDomain)
                );
            return Ok(apiResponse);
        }

        // GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{regionId:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid regionId)
        {
            var regionDomain = await regionRepository.GetByIdAsync(regionId);

            if (regionDomain == null)
            {
                return NotFound();
            }
            var apiResponse = new APISucessResponse(
                    statusCode: HttpStatusCode.OK,
                    message: "Successfully get region by id",
                    data: mapper.Map<RegionDto>(regionDomain)
                );
            return Ok(apiResponse);
        }

        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            var apiResponse = new APISucessResponse(
                    statusCode: HttpStatusCode.Created,
                    message: "Successfully created region",
                    data: regionDto
                );
            return CreatedAtAction(nameof(GetRegionById), new { regionId = regionDto.Id }, apiResponse);
        }

        // PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{regionId:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid regionId, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
            // check if the region exists
            regionDomainModel = await regionRepository.UpdateAsync(regionId, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            var apiResponse = new APISucessResponse(
                statusCode: HttpStatusCode.OK,
                message: "Successfully updated region",
                data: regionDto
            );

            return Ok(apiResponse);
        }

        // DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{regionId:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid regionId)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(regionId);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            var apiResponse = new APISucessResponse(
                statusCode: HttpStatusCode.OK,
                message: "Successfully deleted difficulty",
                data: regionDto
            );

            return Ok(apiResponse);
        }
    }
}
