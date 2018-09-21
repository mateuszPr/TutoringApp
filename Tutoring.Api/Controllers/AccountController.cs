﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Tutoring.Infrastructure.Commands;
using Tutoring.Infrastructure.Commands.Users;
using Tutoring.Infrastructure.Services;

namespace Tutoring.Api.Controllers
{
    public class AccountController : ApiBaseController
    {
        private readonly IJwtHandler _jwtHandler;

        public AccountController(ICommandDispatcher commandDispatcher, IJwtHandler jwtHandler) 
                    : base(commandDispatcher)
        {
            _jwtHandler = jwtHandler;
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]ChangeUserPassword command)
        {
                await CommandDispatcher.DispatchAsync(command);
                return Created($"api/account/{command.Email}", null);
        }
    }
}
