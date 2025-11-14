using AutoMapper;
using Middle0.Domain.Entities;
using Middle0.Domain.Common.DTO;

namespace Middle0.Application.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			// Сопоставление из Entity → DTO
			CreateMap<Event, EventDTO>()
				.ForMember(dest => dest.UserEmail, opt => opt.Ignore()) // если нет в Entity
				.ForMember(dest => dest.UserName, opt => opt.Ignore())
				.ForMember(dest => dest.SendEmail, opt => opt.Ignore());

			// Сопоставление из DTO → Entity
			CreateMap<EventDTO, Event>();
		}
	}
}