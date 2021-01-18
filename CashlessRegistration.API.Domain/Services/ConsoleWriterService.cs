using CashlessRegistration.API.Domain.Contracts.Services;
using System;

namespace CashlessRegistration.API.Domain.Services
{
    public class ConsoleWriterService : IWriterService
    {
        public void WriteLine(long value) => Console.WriteLine(value);
    }
}
