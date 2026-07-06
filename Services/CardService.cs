using Microsoft.EntityFrameworkCore;
using MTGProject.ModelDto;
using MTGProject.Models;

namespace MTGProject.Services
{
    public class CardService
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;

        public CardService(IDbContextFactory<MyDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<CardDTO>> GetAllCards(int pageNumber, int pageSize)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Cards
                .Where(c => c.OriginalImageUrl != null)
                .Include(c => c.Artist)
                .Include(c => c.CardColors).ThenInclude(cc => cc.Color)
                .Include(c => c.CardTypes).ThenInclude(ct => ct.Type)
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CardDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Artist = c.Artist.FullName,
                    Image = c.OriginalImageUrl,
                    Color = c.CardColors.FirstOrDefault().Color.Name ?? "Unknown",
                    ConvertedManaCost = c.ConvertedManaCost,
                    Type = c.Type,
                    Rarity = c.RarityCode,
                    Text = c.Text,
                    Flavor = c.Flavor,
                    RarityCode = c.RarityCode,
                    SetCode = c.SetCodeNavigation.Code

                })
                .ToListAsync();
        }

        public async Task<List<CardDTO>> SearchCards(
            string searchTerm, 
            string manaCost, 
            string typeName, 
            string rarityCode, 
            string colorCode, 
            int pageSize)
        {
            using (MyDbContext localContext = _contextFactory.CreateDbContext())
            {
                string normalizedSearchTerm = NormalizeSearchTerm(searchTerm);
                
                IQueryable<Card> query = localContext.Cards
                    .Where(c => c.OriginalImageUrl != null);
                
                // Apply filters if provided
                if (!string.IsNullOrEmpty(normalizedSearchTerm))
                {
                    query = query.Where(c => c.Name.ToLower().StartsWith(normalizedSearchTerm));
                }
                
                if (!string.IsNullOrEmpty(manaCost))
                {
                    query = query.Where(c => c.ConvertedManaCost == manaCost);
                }
                
                if (!string.IsNullOrEmpty(typeName))
                {
                    query = query.Where(c => c.Type.Contains(typeName));
                }
                
                if (!string.IsNullOrEmpty(rarityCode))
                {
                    query = query.Where(c => c.RarityCode == rarityCode);
                }
                
                if (!string.IsNullOrEmpty(colorCode))
                {
                    query = query.Where(c => c.CardColors.Any(cc => cc.Color.Code == colorCode));
                }
                
                return await query
                    .Include(c => c.Artist)
                    .Include(c => c.CardColors).ThenInclude(cc => cc.Color)
                    .Include(c => c.CardTypes).ThenInclude(ct => ct.Type)
                    .OrderBy(c => c.Name)
                    .Take(pageSize)
                    .Select(c => new CardDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Artist = c.Artist.FullName,
                        Image = c.OriginalImageUrl,
                        Color = c.CardColors.FirstOrDefault().Color.Name ?? "Unknown",
                        ConvertedManaCost = c.ConvertedManaCost,
                        Type = c.Type,
                        Rarity = c.RarityCode
                    })
                    .ToListAsync();
            }
        }
        
        private string NormalizeSearchTerm(string searchTerm)
        {
            return searchTerm?.ToLower().Trim() ?? string.Empty;
        }
        
        public async Task<List<ColorDTO>> GetColors()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Colors
                .Select(c => new ColorDTO
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name
                })
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
        
        public async Task<List<RarityDTO>> GetRarities()
        { 
            using var context = _contextFactory.CreateDbContext();
            return await context.Rarities
                .Select(r => new RarityDTO
                {
                    Id = r.Id,
                    Code = r.Code,
                    Name = r.Name
                })
                .OrderBy(r => r.Name)
                .ToListAsync();
        }
        
        public async Task<List<TypeDTO>> GetTypes()
        {
                using var context = _contextFactory.CreateDbContext();
                return await context.Types
                    .Select(t => new TypeDTO
                    {
                        Id = t.Id,
                        Name = t.Name
                    })
                    .OrderBy(t => t.Name)
                    .ToListAsync();
        }
        
        
        
        public async Task<bool> AddCardToFavorites(string userId, long cardId)
        {
            using var context = _contextFactory.CreateDbContext();
            Favorite? existingFavorite = await context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.CardId == cardId);
            if (existingFavorite != null)
            {
                return false;
            }
            var favorite = new Favorite
            {
                UserId = userId,
                CardId = cardId,
            };
            context.Favorites.Add(favorite);
            await context.SaveChangesAsync();
            return true;
        }
        
        
        
        public async Task<bool> CardExistInFavorites(string userId, long cardId)
        {
            using (MyDbContext localContext = _contextFactory.CreateDbContext())
            {
                Favorite? existingFavorite = await localContext.Favorites
                    .FirstOrDefaultAsync(f => f.UserId == userId && f.CardId == cardId);
                return existingFavorite != null;
            }
          
        }
        
        
        public async Task<bool> RemoveCardFromFavorites(string userId, long cardId)
        {
            using var context = _contextFactory.CreateDbContext();
            Favorite? existingFavorite = await context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.CardId == cardId);

            if (existingFavorite == null)
            {
                return false;
            }

            context.Favorites.Remove(existingFavorite);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CardDTO>> GetUserFavoriteCards(string userId)
        {
            using (MyDbContext localContext = _contextFactory.CreateDbContext())
            {
                List<long> favoriteCardIds = await localContext.Favorites
                    .Where(f => f.UserId == userId)
                    .Select(f => f.CardId)
                    .ToListAsync();
                
                
                return await localContext.Cards
                    .Where(c => favoriteCardIds.Contains(c.Id) && c.OriginalImageUrl != null)
                    .Include(c => c.Artist)
                    .Include(c => c.CardColors).ThenInclude(cc => cc.Color)
                    .Include(c => c.CardTypes).ThenInclude(ct => ct.Type)
                    .OrderBy(c => c.Name)
                    .Select(c => new CardDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Artist = c.Artist.FullName,
                        Image = c.OriginalImageUrl,
                        Color = c.CardColors.FirstOrDefault().Color.Name ?? "Unknown",
                        ConvertedManaCost = c.ConvertedManaCost,
                        Type = c.Type,
                        Rarity = c.RarityCode,
                        Text = c.Text,
                        Flavor = c.Flavor,
                        RarityCode = c.RarityCode,
                        SetCode = c.SetCodeNavigation.Code
                    })
                    .ToListAsync();  
            }
        }
        
    }
    
}
