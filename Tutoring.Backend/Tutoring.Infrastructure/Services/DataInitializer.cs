﻿using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Tutoring.Infrastructure.Services
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IUserService _userService;
        private readonly ILeaderService _leaderService;
        private readonly ICourseService _courseService;
        private readonly IParticipantService _participantService;

        private readonly ILogger<DataInitializer> _logger;

        private readonly string[] listOfSubjects = { "pierwszego", "drugiego", "trzeciego", "czwartego", "piątego"};

        public DataInitializer(IUserService userService, ILeaderService leaderService, ILogger<DataInitializer> logger,
                               ICourseService courseService, IParticipantService participantService)
        {
            _userService = userService;
            _leaderService = leaderService;
            _logger = logger;
            _courseService = courseService;
            _participantService = participantService;
        }

        public async Task SeedAsync()
        {
            var users = await _userService.BrowseAsync();
            if (users.Any())
            {
                _logger.LogTrace("Data was already initialized.");
                return;
            }
            _logger.LogTrace("Initializing data...");

            for (var i = 1; i <= 5; i++)
            {
                var userId = Guid.NewGuid();
                var courseId = Guid.NewGuid();
                var username = $"user{i}";
                await _userService.RegisterAsync(userId, $"user{i}@test.com", username, "secret", "Wroclaw", "user");
                _logger.LogTrace($"Adding user with username: '{username}'.");
                await _courseService.CreateAsync(courseId, $"Nauka angielskiego '{listOfSubjects[i-1]}'", 5, "Radlin", "Opis...", "Biologia 1", "Szkola podstawowa", "Biologia");
                _logger.LogTrace($"Adding course for: '{listOfSubjects[i-1]}'.");
                await _leaderService.CreateAsync(userId);
                _logger.LogTrace($"Adding leader for: '{username}'.");
                await _participantService.CreateAsync(userId);
                _logger.LogTrace($"Adding participant for: '{username}'.");
            }

            for (var i = 1; i <= 3; i++)
            {
                var userId = Guid.NewGuid();
                var username = $"admin{i}";
                _logger.LogTrace($"Adding admin with username: '{username}'.");
                await _userService.RegisterAsync(userId,$"admin{i}@test.com", username, "secret", "Wroclaw", "admin");
            }
            _logger.LogTrace("Data was initialized.");
        }
    }
}
