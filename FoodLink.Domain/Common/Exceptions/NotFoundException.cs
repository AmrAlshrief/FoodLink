namespace FoodLink.Domain.Common.Exceptions;

public class NotFoundException(string message) : Exception(message)
{
}
