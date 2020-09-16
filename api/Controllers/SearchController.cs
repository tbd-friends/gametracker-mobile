﻿using System.Collections.Generic;
using System.Threading.Tasks;
using GameTrove.Api.Models;
using GameTrove.Application.Infrastructure;
using GameTrove.Application.Query;
using GameTrove.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameTrove.Api.Controllers
{
    [ApiController]
    [Route("search")]
    public class SearchController : ControllerBase
    {
        private readonly IAuthenticatedMediator _mediator;

        public SearchController(IAuthenticatedMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("games"), Authorize(Roles = "Administrator,User")]
        public async Task<IEnumerable<GameSearchViewModel>> SearchGames(SearchModel model)
        {
            return await _mediator.Send(new SearchForGames
            {
                Text = model.Text
            });
        }
    }
}
