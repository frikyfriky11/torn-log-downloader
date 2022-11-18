using System.ComponentModel.DataAnnotations.Schema;

namespace TornLogDownloader.Entities;

public class Run
{
  public Guid Id { get; set; }

  [Column(TypeName = "datetimeoffset(3)")]
  public DateTimeOffset Started { get; set; }

  [Column(TypeName = "datetimeoffset(3)")]
  public DateTimeOffset? Ended { get; set; }

  public bool IsSuccessful { get; set; }

  [InverseProperty(nameof(RunApiCall.Run))]
  public ICollection<RunApiCall> RunApiCalls { get; set; } = default!;
}

