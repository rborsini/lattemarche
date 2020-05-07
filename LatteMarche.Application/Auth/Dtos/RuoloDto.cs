using System.Collections.Generic;

namespace LatteMarche.Application.Auth.Dtos
{
    public class RuoloDto
    {
        public long Id { get; set; }
        public string Codice { get; set; }
        public string Descrizione { get; set; }

        public List<Pagina> Pagine_MVC { get; set; }
        public List<Pagina> Pagine_API { get; set; }
        public List<UtenteDto> Utenti { get; set; }

        public RuoloDto()
        {
            this.Pagine_MVC = new List<Pagina>();
            this.Pagine_API = new List<Pagina>();
        }

        public class Pagina
        {
            public bool Enabled { get; set; }
            public string Title { get; set; }

            public List<ViewItemDto> Items { get; set; }

            public Pagina()
            {
                this.Items = new List<ViewItemDto>();
            }

            public Pagina(bool enabled, string title)
                : this()
            {
                this.Enabled = enabled;
                this.Title = title;
            }

        }

        public class ViewItemDto
        {
            public bool Enabled { get; set; }
            public string Title { get; set; }
            public string DisplayName { get; set; }

            public ViewItemDto() { }

            public ViewItemDto(bool enabled, string title, string displayName)
            {
                this.Enabled = enabled;
                this.Title = title;
                this.DisplayName = displayName;
            }

        }

    }

}
