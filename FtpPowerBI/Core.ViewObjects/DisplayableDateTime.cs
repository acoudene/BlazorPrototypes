// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Core.ViewObjects;

public readonly record struct DisplayableDateTime(DateTimeOffset DateTimeOffset)
{
  public static implicit operator DisplayableDateTime(DateTimeOffset dateTimeOffset) => new DisplayableDateTime(dateTimeOffset.ToUniversalTime());
  public static implicit operator DisplayableDateTime(DateTime dateTime) => new DisplayableDateTime(new DateTimeOffset(dateTime.ToUniversalTime()));

  public static implicit operator DateTimeOffset(DisplayableDateTime dateTimeVo) => dateTimeVo.DateTimeOffset.ToLocalTime();
  public static implicit operator DateTime(DisplayableDateTime dateTimeVo) => dateTimeVo.DateTimeOffset.LocalDateTime;  

  public override string ToString()
  {
    return ((DateTimeOffset) this).ToString();
  }
}
