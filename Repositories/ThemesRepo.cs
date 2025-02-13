using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ThemesRepo
    {
        private readonly isc_elibContext _context;
        public ThemesRepo(isc_elibContext context) {
            _context = context;
        }

        public ICollection<Theme> GetThemes()
        {
            return _context.Themes.ToList();
        }

        public Theme GetThemesById(long id)
        {
            return _context.Themes.FirstOrDefault(s => s.Id == id);
        }

        public Theme CreateThemes(Theme theme)
        {
            _context.Themes.Add(theme);
            _context.SaveChanges();
            return theme;
        }

        public Theme UpdateThemes(Theme theme)
        {
            _context.Themes.Update(theme);
            _context.SaveChanges();
            return theme;
        }

        public bool DeleteThemes(long id)
        {
            var themes = GetThemesById(id);
            if (themes != null)
            {
                _context.Themes.Remove(themes);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
