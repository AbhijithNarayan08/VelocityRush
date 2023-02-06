using com.citruslime.lib.ui.signal;
using com.citruslime.ui;

public delegate void ShowTransmissionSignal(ShowUiElementSignal signal);
public delegate void HideTransmissionSignal(HideUiElementSignal signal);

public class SignalBus
{
    public event ShowTransmissionSignal ShowTransmissionSignalEvent;
    public event HideTransmissionSignal HideTransmissionSignalEvent;

    public void Transmission(ShowUiElementSignal signal)
    {
        ShowTransmissionSignalEvent?.Invoke(signal);
    }

    public void Transmission(HideUiElementSignal signal)
    {
         HideTransmissionSignalEvent?.Invoke(signal);
    }
}
