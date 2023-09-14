using AutoMapper;
using NotificationService.Application.Dtos;
using NotificationService.Domain;
using SharedLibrary;

namespace NotificationService.Application.Profiles;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<Email, EmailMessage>().ReverseMap();
    }
}