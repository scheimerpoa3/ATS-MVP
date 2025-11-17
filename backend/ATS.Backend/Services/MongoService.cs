using ATS.Backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ATS.Backend.Services
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CandidatosCollectionName { get; set; }
    }

    public interface ICandidatoRepository
    {
        Task<IEnumerable<Candidato>> GetAllAsync();
        Task<Candidato> GetByIdAsync(string id);
        Task<Candidato> CreateAsync(Candidato candidato);
        Task UpdateAsync(string id, Candidato candidato);
        Task DeleteAsync(string id);
    }

    public class CandidatoRepository : ICandidatoRepository
    {
        private readonly IMongoCollection<Candidato> _candidatos;

        public CandidatoRepository(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var db = client.GetDatabase(settings.Value.DatabaseName);
            _candidatos = db.GetCollection<Candidato>(settings.Value.CandidatosCollectionName);
        }

        public async Task<IEnumerable<Candidato>> GetAllAsync()
        {
            return await _candidatos.Find(_ => true).ToListAsync();
        }

        public async Task<Candidato> GetByIdAsync(string id)
        {
            return await _candidatos.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Candidato> CreateAsync(Candidato candidato)
        {
            await _candidatos.InsertOneAsync(candidato);
            return candidato;
        }

        public async Task UpdateAsync(string id, Candidato candidato)
        {
            candidato.Id = id;
            await _candidatos.ReplaceOneAsync(c => c.Id == id, candidato);
        }

        public async Task DeleteAsync(string id)
        {
            await _candidatos.DeleteOneAsync(c => c.Id == id);
        }
    }
}
