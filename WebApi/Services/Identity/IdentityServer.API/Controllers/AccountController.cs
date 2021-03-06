﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.API.Domains;
using IdentityServer.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserDataModel userDataModel;
        private readonly ILogger<AccountController> logger;

        public AccountController(IUserDataModel userDataModel, ILogger<AccountController> logger)
        {
            this.userDataModel = userDataModel;
            this.logger = logger;
        }

        [HttpGet("get-users")]
        [Authorize(LocalApi.PolicyName)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            try
            {
                var users = await userDataModel.GetUsersListAsync().ConfigureAwait(true);
                return Ok(users);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, GetType().Name);
                throw;
            }
        }

        [HttpPost("register-user")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Non passare valori letterali come parametri localizzati", Justification = "<In sospeso>")]
        public async Task<ActionResult<User>> RegisterUser(User user)
        {
            try
            {
                if (user == null)
                {
                    logger.LogDebug($"Response <{nameof(BadRequest)}>, given user is null");
                    return BadRequest("User not specified");
                }

                var userExist = await userDataModel.GetUserAsync(user.Username).ConfigureAwait(true);
                if (userExist == null)
                {
                    await userDataModel.InsertUserAsync(user).ConfigureAwait(true);

                    logger.LogDebug($"Response <{nameof(Ok)}>, User with username <{user.Username}> created");
                    return Ok(user);
                }

                logger.LogDebug($"Response <{nameof(Conflict)}>>, User with username <{user.Username}> already exists");
                return Conflict(user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, GetType().Name);
                throw;
            }
        }

        [HttpDelete("delete-user/{username}")]
        [Authorize(LocalApi.PolicyName)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Non passare valori letterali come parametri localizzati", Justification = "<In sospeso>")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            try
            {
                if (username == null)
                {
                    logger.LogDebug($"Response: <{nameof(BadRequest)}>, given useris null");
                    return BadRequest("User not specified");
                }

                var userExist = await userDataModel.GetUserAsync(username).ConfigureAwait(true);
                if (userExist != null)
                {
                    await userDataModel.DeleteUserAsync(username).ConfigureAwait(true);

                    logger.LogDebug($"Response: <{nameof(Ok)}>, user with username <{username}> was deleted");
                    return Ok("User deleted");
                }

                logger.LogDebug($"Response <{nameof(NotFound)}>>, User with username <{username}> not exists");
                return NotFound("User not exist");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, GetType().Name);
                throw;
            }
        }
    }
}
