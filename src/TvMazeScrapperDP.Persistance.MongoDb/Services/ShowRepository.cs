using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TvMazeScrapperDp.Core.Services.Contracts;
using TvMazeScrapperDP.Persistance.MongoDb.Model;
using TvMazeScrapperDP.Persistance.MongoDb.Services.Contracts;

namespace TvMazeScrapperDP.Persistance.MongoDb.Services
{
    public class ShowRepository : IShowRepository
    {
        private readonly IMongoCollection<Show> _showCollection;
        private readonly IMapper _mapper;

        public ShowRepository(IShowContext showContext, IMapper mapper)
        {
            _showCollection = showContext.Show;
            _mapper = mapper;
        }

        public async Task SaveAsync(IEnumerable<TvMazeScrapperDp.Core.Models.Show> shows,
            CancellationToken cancellationToken)
        {
            await _showCollection.InsertManyAsync(shows.Select(_mapper.Map<Show>), new InsertManyOptions(), cancellationToken);
        }

        public async Task<TvMazeScrapperDp.Core.Models.Show> FindAsync(int id, CancellationToken cancellationToken)
        {
            var filterBuilder = new FilterDefinitionBuilder<Show>();
            var filter = filterBuilder.Eq(x => x.Id, id);

            var shows = await _showCollection.Find(filter).ToListAsync(cancellationToken);

            var show = shows.SingleOrDefault();

            return show != null
                ? _mapper.Map<TvMazeScrapperDp.Core.Models.Show>(show)
                : null;
        }

        public async Task<int> GetMaxIdAsync(CancellationToken cancellationToken)
        {
            return await CountAsync(cancellationToken) > 0
                ? await _showCollection.AsQueryable().MaxAsync(a => a.Id, cancellationToken)
                : 0;
        }

        public async Task<IEnumerable<TvMazeScrapperDp.Core.Models.Show>> GetPaginatedAsync(int page, int pageSize,
            CancellationToken cancellationToken)
        {
            var options = GetPaginatedOptions(page, pageSize);

            var cursor = await _showCollection.FindAsync(new BsonDocument(), options, cancellationToken);
            var shows = await cursor.ToListAsync(cancellationToken);

            return shows.Select(_mapper.Map<TvMazeScrapperDp.Core.Models.Show>);
        }

        public async Task<long> CountAsync(CancellationToken cancellationToken)
        {
            return await _showCollection.CountDocumentsAsync(FilterDefinition<Show>.Empty, null, cancellationToken);
        }

        private FindOptions<Show> GetPaginatedOptions(int page, int pageSize)
        {
            if (pageSize > 100)
            {
                throw new ArgumentException("Maximum allowed amount of documents per page is 100");
            }

            var skipElements = (page - 1) * pageSize;

            return new FindOptions<Show> {Skip = skipElements, Limit = pageSize};
        }
    }
}