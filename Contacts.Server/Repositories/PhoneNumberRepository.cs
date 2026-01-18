using Contacts.Server;
using Contacts.Server.Model;
using Contacts.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class PhoneNumberRepository : IPhoneNumberRepository
{
    private readonly ApplicationContext _db;

    public PhoneNumberRepository(ApplicationContext db)
    {
        _db = db;
    }

    public async Task<PhoneNumber?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.PhoneNumbers
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task AddAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken)
    {
        await _db.PhoneNumbers.AddAsync(phoneNumber, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken)
    {
        _db.PhoneNumbers.Remove(phoneNumber);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
