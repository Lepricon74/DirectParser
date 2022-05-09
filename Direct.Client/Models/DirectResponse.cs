using Direct.Client.Attributes;

namespace Direct.Client.Models
{
    public record DirectResponse<T>( [property : NotNull] T result);
}
