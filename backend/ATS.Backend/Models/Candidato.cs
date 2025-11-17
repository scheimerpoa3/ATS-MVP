using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ATS.Backend.Models
{
    public class Candidato
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Nome { get; set; } = "";
        public string Email { get; set; } = "";
        public string Telefone { get; set; } = "";
        public string CargoDesejado { get; set; } = "";
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
