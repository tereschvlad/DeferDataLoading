using System.Collections.Concurrent;

namespace DelayedDataLoading;

public static class DbConnectionHelper
{
    private static ConcurrentDictionary<string, string> _dbConnection = new ConcurrentDictionary<string, string>(); 

}