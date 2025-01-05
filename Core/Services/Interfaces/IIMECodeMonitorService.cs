using Core.Event;

namespace Core.Services.Interfaces;
public interface IIMECodeMonitorService
{
    event OnProgramIMECodeChangedEventHandler? OnProgramIMECodeChanged;

    void ResetAndUpdateInterval();
    void Start();
    void Stop();
}