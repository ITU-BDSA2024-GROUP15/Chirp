namespace SimpleDB;

/// <summary>
/// The IDatabaseRepository contains the base functions of reading and storing records
/// </summary>
/// <typeparam name="T">Generic type, representing a record</typeparam>
public interface IDatabaseRepository<T>
{
    /// <summary>
    /// Method for reading from the database
    /// </summary>
    /// <param name="limit">The amount wanted to be returned</param>
    /// <returns>returns an iterable collection of records</returns>
    public IEnumerable<T> Read(int? limit = null);
    
    /// <summary>
    /// Method for storing a record
    /// </summary>
    /// <param name="record">The record to be stored</param>
    public void Store(T record);
}