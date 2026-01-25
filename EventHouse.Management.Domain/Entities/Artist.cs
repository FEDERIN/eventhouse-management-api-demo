using EventHouse.Management.Domain.Enums;
using EventHouse.Management.Domain.Exceptions;
using EventHouse.ShareKernel.Entities;

namespace EventHouse.Management.Domain.Entities;

public sealed class Artist : Entity
{
    public string Name { get; private set; } = null!;
    public ArtistCategory Category { get; private set; }

    private readonly List<ArtistGenre> _genres = [];
    public IReadOnlyCollection<ArtistGenre> Genres => _genres.AsReadOnly();

    private Artist() { }

    public Artist(Guid id, string name, ArtistCategory category)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        Id = id;
        Name = name.Trim();
        Category = category;
    }

    public void Update(string name, ArtistCategory category)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        Name = name.Trim();
        Category = category;
    }

    public AddGenreOutcome AddGenre(Guid genreId, ArtistGenreStatus status, bool isPrimary = false)
    {
        if (genreId == Guid.Empty)
            throw new ArgumentException("GenreId cannot be empty.", nameof(genreId));

        if (_genres.Any(g => g.GenreId == genreId))
            return AddGenreOutcome.NoChange;

        if (isPrimary)
            UnmarkAllPrimary();

        if (!_genres.Any(g => g.IsPrimary))
            isPrimary = true;

        _genres.Add(new ArtistGenre(Id, genreId, status, isPrimary));
        return AddGenreOutcome.Added;
    }

    public void RemoveGenre(Guid genreId)
    {
        var existing = _genres.FirstOrDefault(g => g.GenreId == genreId);
        if (existing is null)
            return;

        var wasPrimary = existing.IsPrimary;

        _genres.Remove(existing);

        if (wasPrimary)
        {
            var firstActive = _genres.FirstOrDefault(g => g.Status == ArtistGenreStatus.Active);
            if (firstActive is not null)
            {
                UnmarkAllPrimary();
                firstActive.MarkAsPrimary();
            }
            else
            {
                UnmarkAllPrimary();
            }
        }
    }

    public bool SetGenreStatus(Guid genreId, ArtistGenreStatus status)
    {
        var existing = _genres.FirstOrDefault(g => g.GenreId == genreId)
            ?? throw new NotAssociatedException("Artist", "Genre", Id, genreId);

        if (existing.Status == status)
            return false;

        existing.SetStatus(status);
        return true;
    }

    public void SetPrimaryGenre(Guid genreId)
    {
        var selected = _genres.FirstOrDefault(g => g.GenreId == genreId)
             ?? throw new NotAssociatedException(
            parent: "Artist",
            child: "Genre",
            parentId: Id,
            childId: genreId);

        if (selected.IsPrimary)
            return;

        UnmarkAllPrimary();
        selected.MarkAsPrimary();
    }

    private void UnmarkAllPrimary()
    {
        foreach (var g in _genres.Where(x => x.IsPrimary))
            g.MarkAsSecondary();
    }

    public enum AddGenreOutcome
    {
        Added,
        NoChange
    }
}
