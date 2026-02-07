namespace MyBlazor10.Client.WorkerServices;

public class AuthoritativeEqualToCachedStampRule
{
  public bool IsSatisfied(StampInfo? authoritativeStamp, StampInfo? cachedStamp)
  {
    if (cachedStamp is null)
      // If we can't get a client stamp, assume update is available
      return false;

    if (authoritativeStamp is null)
      // If we can't get a server stamp, assume no update is available
      return true;

    if (authoritativeStamp.Identifier != cachedStamp.Identifier)
      // Detected update    
      return false;

    return true; // To avoid infinite loop
  }
}