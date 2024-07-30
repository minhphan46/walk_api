using AutoMapper;
using WalkProject.API.GraphQL.DTOs.Categories;
using WalkProject.API.GraphQL.DTOs.Difficulties;
using WalkProject.API.GraphQL.DTOs.Regions;
using WalkProject.API.GraphQL.DTOs.Users;
using WalkProject.API.GraphQL.DTOs.Walks;
using WalkProject.API.RestFul.DTOs.CategoryModel;
using WalkProject.API.RestFul.DTOs.DifficultyModel;
using WalkProject.API.RestFul.DTOs.RegionModel;
using WalkProject.API.RestFul.DTOs.WalkCategoryModel;
using WalkProject.API.RestFul.DTOs.WalkModel;
using WalkProject.DataModels.Entities;

namespace WalkProject.Utils
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // RestFul
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
            CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
            CreateMap<Walk, WalkDto>()
                .ForMember(x => x.Categories, opt => opt.MapFrom(x => x.WalkCategories))
                .ReverseMap();
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
            CreateMap<AddDifficultyDto, Difficulty>().ReverseMap();
            CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<AddCategoryDto, Category>().ReverseMap();
            CreateMap<WalkCategory, WalkCategoryDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ReverseMap();

            // GraphQl
            CreateMap<Category, CategoryInput>().ReverseMap();
            CreateMap<Category, CategoryResponse>().ReverseMap();
            CreateMap<Difficulty, DifficultyInput>().ReverseMap();
            CreateMap<Difficulty, DifficultyResponse>().ReverseMap();
            CreateMap<Region, RegionInput>().ReverseMap();
            CreateMap<Region, RegionResponse>().ReverseMap();
            CreateMap<Walk, WalkInput>().ReverseMap();
            CreateMap<Walk, WalkResponse>()
                .ForSourceMember(x => x.WalkCategories, opt => opt.DoNotValidate())
                .ForSourceMember(x => x.Difficulty, opt => opt.DoNotValidate())
                .ForSourceMember(x => x.Region, opt => opt.DoNotValidate())
                .ReverseMap();
            CreateMap<User, UserProfileInput>().ReverseMap();
            CreateMap<Role, UserRoleInput>().ReverseMap();
        }
    }
}
