namespace MyBlazor10.Client.WorkerServices;

public interface IStampChangeNotifier
{
  bool Notify(StampInfo? previous, StampInfo current);
}