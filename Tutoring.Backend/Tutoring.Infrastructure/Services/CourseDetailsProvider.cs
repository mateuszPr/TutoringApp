﻿using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutoring.Core.Domain;
using Tutoring.Infrastructure.Dtos;

namespace Tutoring.Infrastructure.Services
{
    public class CourseDetailsProvider : ICourseDetailsProvider
    {
        private static readonly IDictionary<string, IEnumerable<FieldDetails>> _availableFieldDetails =
            new Dictionary<string, IEnumerable<FieldDetails>>
            {
                ["Matematyka"] = new List<FieldDetails>
                {
                    new FieldDetails("Analiza matematyczna", "Szkola wyzsza"),
                    new FieldDetails("Algebra liniowa", "Szkola wyzsza"),
                    new FieldDetails("Matematyka", "Szkola podstawowa"),
                    new FieldDetails("Matematyka", "Szkola srednia"),
                },

                ["Biologia"] = new List<FieldDetails>
                {
                    new FieldDetails("Biologia roslin", "Szkola wyzsza"),
                    new FieldDetails("Botanika", "Szkola wyzsza"),
                    new FieldDetails("Biologia 1", "Szkola podstawowa"),
                    new FieldDetails("Biologia 1", "Szkola wyzsza"),
                },

                ["Jezyk angielski"] = new List<FieldDetails>
                {
                    new FieldDetails("Jezyk angielski w biznesie", "Szkola wyzsza"),
                    new FieldDetails("Literatura anglojezyczna", "Szkola wyzsza"),
                    new FieldDetails("Jezyk angielski", "Szkola podstawowa"),
                    new FieldDetails("Jezyk angielski", "Szkola srednia"),
                },
            };

        private readonly static string CacheKey = "details";
        private readonly IMemoryCache _cache;

        public CourseDetailsProvider(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<IEnumerable<CourseDetailsDto>> BrowseAsync()
        {
            var fieldDetails = _cache.Get<IEnumerable<CourseDetailsDto>>(CacheKey);
            if (fieldDetails == null)
            {
                fieldDetails = await GetAllAsync();
                _cache.Set(CacheKey, fieldDetails);
            }

            return fieldDetails;
        }

        private async Task<IEnumerable<CourseDetailsDto>> GetAllAsync()
        {
            return await Task.FromResult(_availableFieldDetails.GroupBy(x => x.Key)
                .SelectMany(g => g.SelectMany(v => v.Value.Select(x => new CourseDetailsDto
                {
                    Subject = v.Key,
                    Field = x.Field,
                    Level = x.Level
                }))));
        }

        public async Task<CourseDetails> GetAsync(string subject, string field, string level)
        {
            if (!_availableFieldDetails.ContainsKey(subject))
            {
                throw new Exception($"Course subject: '{subject}' is not available.");
            }
            var fields = _availableFieldDetails[subject];

            var currentField = fields.SingleOrDefault(x => x.Field == field && x.Level == level);

            if (currentField == null)
            {
                throw new Exception($"Field: '{field}' and level: '{level}' for subject: '{subject}' is not available.");
            }

            return await Task.FromResult(CourseDetails.Create(field,level,subject));
        }

        private class FieldDetails
        {
            public string Field { get;  }
            public string Level { get;  }

            public FieldDetails(string field, string level)
            {
                Field = field;
                Level = level;
            }
        }
    }
}
