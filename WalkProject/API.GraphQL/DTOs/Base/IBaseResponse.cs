using System.ComponentModel.DataAnnotations;

namespace WalkProject.API.GraphQL.DTOs.Base
{
    [InterfaceType("BaseResponse")]
    public interface IBaseResponse
    {
        [Key]
        public Guid Id { get; set; }
    }
}
