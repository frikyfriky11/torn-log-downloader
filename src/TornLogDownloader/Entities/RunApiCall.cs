using System.ComponentModel.DataAnnotations.Schema;

namespace TornLogDownloader.Entities;

public class RunApiCall
{
  public Guid Id { get; set; }

  [ForeignKey(nameof(Run))]
  public Guid RunId { get; set; }

  public Run Run { get; set; } = default!;

  [Column(TypeName = "datetimeoffset(3)")]
  public DateTimeOffset Started { get; set; }

  [Column(TypeName = "datetimeoffset(3)")]
  public DateTimeOffset? Ended { get; set; }

  [InverseProperty(nameof(RawLog.RunApiCall))]
  public ICollection<RawLog> RawLogs { get; set; } = default!;
}


