using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TornLogDownloader.Entities;

public class RawLog
{
  [Key]
  public Guid Id { get; set; }

  [ForeignKey(nameof(RunApiCall))]
  public Guid RunApiCallId { get; set; }

  public RunApiCall RunApiCall { get; set; } = default!;

  [Required]
  [MaxLength(20)]
  public string LogId { get; set; } = default!;

  public int? LogTypeId { get; set; }

  [MaxLength(100)]
  public string? Title { get; set; }

  public long Timestamp { get; set; }

  [Column(TypeName = "smalldatetime")]
  public DateTime Date { get; set; }

  [MaxLength(100)]
  public string? CategoryName { get; set; }

  [MaxLength(500)]
  public string? Data { get; set; }

  [MaxLength(500)]
  public string? Params { get; set; }
}

