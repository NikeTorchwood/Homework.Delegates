namespace Homework.Delegates.FirstExercise;

public static class EnumerableExtensions
{
    public static T GetMax<T>(this IEnumerable<T> collection, Func<T, float> convertToNumber) where T : class
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection), "Коллекция не должна быть null.");

        if (convertToNumber == null)
            throw new ArgumentNullException(nameof(convertToNumber), "Делегат не должен быть null.");

        if (!collection.Any())
            throw new InvalidOperationException("Коллекция пуста. Невозможно найти максимальный элемент.");

        T maxElement = null;
        var maxValue = float.MinValue;

        foreach (var item in collection)
        {
            var value = convertToNumber(item);
            if (!(value > maxValue)) continue;
            maxValue = value;
            maxElement = item;
        }

        return maxElement;
    }
}