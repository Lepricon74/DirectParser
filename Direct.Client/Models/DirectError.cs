using Direct.Client.Attributes;

namespace Direct.Client.Models
{
    internal record DirectError<T>([property : NotNull] T error);
}
