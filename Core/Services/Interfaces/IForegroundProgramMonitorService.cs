using Core.Event;

namespace Core.Services.Interfaces;
public interface IForegroundProgramMonitorService
{
    event OnForegroundProgramChangedEventHandler? OnForegroundProgramChanged;

    void Dispose();
    void Start();
    void Stop();
}