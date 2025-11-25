using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GymManagementAPI.Models;

public class Payment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("memberId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string MemberId { get; set; } = string.Empty;

    [BsonElement("amount")]
    public decimal Amount { get; set; }

    [BsonElement("paymentDate")]
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    [BsonElement("paymentMethod")]
    public string PaymentMethod { get; set; } = string.Empty;

    [BsonElement("status")]
    public string Status { get; set; } = "completed";

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Mongoose version field - ignore this
    [BsonElement("__v")]
    [BsonIgnoreIfNull]
    public int? Version { get; set; }

    // Populated field
    [BsonIgnore]
    public Member? Member { get; set; }
}
