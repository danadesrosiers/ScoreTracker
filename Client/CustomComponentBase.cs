using Microsoft.AspNetCore.Components;

namespace ScoreTracker.Client;

public class CustomComponentBase : ComponentBase
{
    private readonly Dictionary<Func<object?>, RegisteredHandler> _handlers = new();

    protected void OnChange<T>(Func<T?> valueFinder, params Func<T?, Task>[] onChange)
    {
        var key = new Func<object?>(() => valueFinder());
        _handlers[key] = new RegisteredHandler<T>();
        foreach (var callback in onChange)
        {
            _handlers[key].Handlers.Add(type => callback((T?)type));
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        foreach (var (valueFinder, registeredHandler) in _handlers)
        {
            var currentState = JsonSerializer.Serialize(valueFinder());
            if (currentState != registeredHandler.SavedState)
            {
                registeredHandler.SavedState = currentState;
                var dataType = registeredHandler.GetType().GetGenericArguments().First();
                var previousValue = JsonSerializer.Deserialize(registeredHandler.SavedState, dataType);

                foreach (var handler in registeredHandler.Handlers)
                {
                    await handler(previousValue);
                    StateHasChanged();
                    await Task.Yield();
                }
            }
        }
    }

    private record RegisteredHandler
    {
        public ICollection<Func<object?, Task>> Handlers { get; } = new List<Func<object?, Task>>();
        public string? SavedState { get; set; }
    }

    private record RegisteredHandler<T> : RegisteredHandler
    {
        public new ICollection<Func<T?, Task>> Handlers { get; } = new List<Func<T?, Task>>();
    }
}