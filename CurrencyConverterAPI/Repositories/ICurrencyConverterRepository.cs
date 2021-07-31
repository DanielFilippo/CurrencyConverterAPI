using CurrencyConverterAPI.Models;
using System.Threading.Tasks;

namespace CurrencyConverterAPI.Repositories
{
    public interface ICurrencyConverterRepository
    {
        Task<TransactionCreateResult> insertConversionTransactionAsync(TransactionCreateInputParams input);

        Task<TransactionUserResult> getAllTransactionsByUserIdAsync(int userId);
    }
}
