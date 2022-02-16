using System.Net;
using Core.DataModels;
using Core.Dto;
using Core.Enums;
using Core.Exceptions;
using Duende.IdentityServer.Extensions;
using Infrastructure;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Services.Managers.Interfaces;
using static Services.Other.FilterUtilities;
using static Services.Other.FileService;

namespace Services.Managers.Implementations;

public interface IProjectsManager
{
    Task<List<Project>> GetProjects(ProjectsFilterDto filter);
    Task AddProjectsAsync(List<Project> dest);
}