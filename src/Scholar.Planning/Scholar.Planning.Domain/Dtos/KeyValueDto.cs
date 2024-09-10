namespace Scholar.Planning.Domain.Dtos;

public class KeyValueDto<T, Q>
{
    public required T Key { get; set; }
    public required Q Value { get; set; }
}