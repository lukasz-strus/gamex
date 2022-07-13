﻿using Flurl.Http;
using gamexModelsDto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace gamexServices;

public interface IUserService
{
    /// <summary>
    /// Update user by Id
    /// </summary>
    /// <param name="token">JWT</param>
    /// <param name="id">User Id</param>
    /// <param name="updateUserDto">Update user data transfer object</param>
    /// <returns>Status Code</returns>
    Task<int> Update(string token, int id, UpdateUserDto updateUserDto);

    /// <summary>
    /// Get list of users
    /// </summary>
    /// <param name="token">JWT</param>
    /// <param name="query">GetAllQuery</param>
    /// <returns>GetAllResult<UserDto></returns>
    Task<GetAllResult<UserDto>> GetAll(string token, GetAllQuery query);

    /// <summary>
    /// Get user by id
    /// </summary>
    /// <param name="token">JWT</param>
    /// <param name="id">User ID</param>
    /// <returns>User</returns>
    Task<UserDto> Get(string token, int id);

    /// <summary>
    /// Delete user by id
    /// </summary>
    /// <param name="token">JWT</param>
    /// <param name="id">User Id</param>
    /// <returns>Status Code</returns>
    Task<int> Delete(string token, int id);
}

public class UserService : IUserService
{
    private readonly string _baseUrl =
        "https://gamex-api-app.azurewebsites.net/api/user";

    public async Task<int> Update(string token, int id, UpdateUserDto updateUserDto)
    {
        var response = await (_baseUrl + $"/{id}")
            .WithOAuthBearerToken(token)
            .PutJsonAsync(updateUserDto);

        return response.StatusCode;
    }

    public async Task<GetAllResult<UserDto>> GetAll(string token, GetAllQuery query)
    {
        var response = await _baseUrl
            .WithOAuthBearerToken(token)
            .SetQueryParams(new
            {
                searchPhrase = query.SearchPhrase,
                pageSize = query.PageSize,
                pageNumber = query.PageNumber,
                sortyBy = query.SortBy,
                sortDirection = query.SortDirection
            })
            .GetAsync()
            .ReceiveJson<GetAllResult<UserDto>>();

        return response;
    }

    public async Task<UserDto> Get(string token, int id)
    {
        var response = await (_baseUrl + $"/{id}")
            .WithOAuthBearerToken(token)
            .GetAsync()
            .ReceiveJson<UserDto>();

        return response;
    }

    public async Task<int> Delete(string token, int id)
    {
        var response = await (_baseUrl + $"/{id}")
            .WithOAuthBearerToken(token)
            .DeleteAsync();

        return response.StatusCode;
    }
}