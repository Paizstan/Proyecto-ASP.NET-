using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using WebApplication1.Models;

namespace WebApplication1.Utilidades
{
    public class TableUsuario:IDocument
    {
        private List<Role> _items;

        public TableUsuario(List<Role> items)
        {
            _items = items;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(50);
                page.Content().Column(column =>
                {
                    column.Item().Text("Lista de Roles").FontSize(20).Bold();
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1);//Id
                            columns.RelativeColumn(2);//Nombre
                        });
                        table.Header(header =>
                        {
                            header.Cell().Text("ID").Bold();
                            header.Cell().Text("Nombre").Bold();

                        });
                        foreach (var item in _items)
                        {
                            table.Cell().Text(item.Id.ToString());
                            table.Cell().Text(item.Nombre);

                            static IContainer CellStyle(IContainer container) =>
                                container.PaddingVertical(5);
                        }
                    });
                });
            });
        }
    }
}
