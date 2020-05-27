﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameTrove.Application.Query.Handlers
{
    public class RetrieveGameByCodeHandler : IRequestHandler<RetrieveGameByCode, GameViewModel>
    {
        private readonly GameTrackerContext _context;

        public RetrieveGameByCodeHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<GameViewModel> Handle(RetrieveGameByCode request, CancellationToken cancellationToken)
        {
            var game = await (from pg in _context.Games
                              join p in _context.Platforms on pg.PlatformId equals p.Id
                              join t in _context.Titles on pg.TitleId equals t.Id
                              where pg.Code == request.Code
                              select new
                              {
                                  Id = pg.Id,
                                  Name = t.Name,
                                  Description = t.Subtitle,
                                  Registered = pg.Registered,
                                  Code = pg.Code,
                                  Platform = p.Name
                              }).SingleOrDefaultAsync(cancellationToken: cancellationToken);

            return game != null ? new GameViewModel
            {
                Id = game.Id,
                Code = game.Code,
                Description = game.Description,
                Name = game.Name,
                Registered = game.Registered,
                Platform = game.Platform
            } : null;
        }
    }
}