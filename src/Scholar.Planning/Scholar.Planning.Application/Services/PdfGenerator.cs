using iTextSharp.text;
using iTextSharp.text.pdf;
using Scholar.Planning.Domain.Services;

namespace Scholar.Planning.Application.Services;

public class PdfGenerator : IPdfGenerator
{
    public byte[] GenerateFile()
    {
        using var ms = new MemoryStream();
        var doc = new Document(PageSize.A4, 50, 50, 25, 25);
        var writer = PdfWriter.GetInstance(doc, ms);

        doc.Open();

        CreateHeader(doc);
        CreateSubHeader(doc);

        doc.Close();
        writer.Close();

        return ms.ToArray();
    }

    private static void CreateHeader(IElementListener doc)
    {
        var table = new PdfPTable(3)
        {
            WidthPercentage = 100
        };
        table.SetWidths(new float[] { 1, 2, 2 });

        var img = Image.GetInstance("./Resources/logo.jpg");
        img.ScaleAbsolute(65.205f, 65.205f);
        var imageCell = new PdfPCell(img)
        {
            Border = Rectangle.NO_BORDER,
            HorizontalAlignment = Element.ALIGN_LEFT
        };
        table.AddCell(imageCell);

        var font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7);
        var text = new Phrase
        {
            new Chunk("Escola Municipal de Educação Infantil Parque do Sabiá", font), // Primeira linha
            new Chunk("\nRua Gavião, 89 - CEP 93295-605", font), // Segunda linha
            new Chunk("\nBairro Três Marias – Esteio/RS", font), // Terceira linha
            new Chunk("\nFone: (51) 2312-7272 - Celular: (51) 995190107", font) // Quarta linha
        };
        
        foreach (var chunk in text.Chunks)
        {
            chunk.setLineHeight(10f);
        }

        var textCell = new PdfPCell(text)
        {
            Border = Rectangle.NO_BORDER,
            VerticalAlignment = Element.ALIGN_MIDDLE,
            HorizontalAlignment = Element.ALIGN_LEFT
        };
        table.AddCell(textCell);

        var img2 = Image.GetInstance("./Resources/state-info.jpg");
        img2.ScaleAbsolute(184.1425f, 38.865f);
        var imageCell2 = new PdfPCell(img2)
        {
            Border = Rectangle.NO_BORDER,
            HorizontalAlignment = Element.ALIGN_LEFT
        };
        table.AddCell(imageCell2);

        doc.Add(table);
        doc.Add(SpaceParagraph());
    }

    private static void CreateSubHeader(IElementListener doc)
    {
        var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
        var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

        var table = new PdfPTable(2)
        {
            WidthPercentage = 100
        };
        table.SetWidths([2f, 1f]);

        var cell1 = new PdfPCell
        {
            Border = Rectangle.BOX,
            Padding = 5,
            Colspan = 2,
            Phrase =
            [
                new Chunk("1. PROFESSORA: ", boldFont),
                new Chunk("Marli Isotton", regularFont)
            ]
        };
        table.AddCell(cell1);

        var cell2 = new PdfPCell
        {
            Border = Rectangle.LEFT_BORDER,
            Padding = 5,
            Phrase =
            [
                new Chunk("2. TURMA: ", boldFont),
                new Chunk("BERÇÁRIO I", regularFont)
            ]
        };
        table.AddCell(cell2);

        var cell3 = new PdfPCell
        {
            Border = Rectangle.RIGHT_BORDER,
            Padding = 5,
            HorizontalAlignment = Element.ALIGN_LEFT,
            Phrase =
            [
                new Chunk("TURNO: ", boldFont),
                new Chunk("Tarde", regularFont)
            ]
        };
        table.AddCell(cell3);

        var cell4 = new PdfPCell
        {
            Border = Rectangle.BOX,
            Padding = 5,
            Colspan = 2,
            Phrase =
            [
                new Chunk("3. PERÍODO: ", boldFont),
                //Valor dinamico
                new Chunk("13H às 19H - 06 a 11 de setembro", regularFont)
            ]
        };
        table.AddCell(cell4);

        doc.Add(table);
    }
    
    private static Paragraph SpaceParagraph()
    {
        return new Paragraph
        {
            SpacingBefore = 20f
        };
    }

    // // Cria o documento e define o tamanho da página
    //     var doc = new Document(PageSize.A4, 50, 50, 25, 25);
    //     
    //     // Cria o writer que vai salvar o PDF
    //     var writer = PdfWriter.GetInstance(doc, new FileStream("test.pdf", FileMode.Create));
    //
    //     // Abre o documento para edição
    //     doc.Open();
    //
    //     // Adicionando o título do documento
    //     var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
    //     var title = new Paragraph("Escola Municipal de Educação Infantil Parque do Sabiá", titleFont)
    //         {
    //             Alignment = Element.ALIGN_CENTER
    //         };
    //     doc.Add(title);
    //
    //     // Adicionando endereço
    //     var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
    //     var address = new Paragraph("Rua Gavião, 89 - CEP 93295-605\nBairro Três Marias – Esteio/RS\nFone: (51) 2312-7272- Celular: (51) 995190107", regularFont)
    //         {
    //             Alignment = Element.ALIGN_CENTER
    //         };
    //     doc.Add(address);
    //
    //     // Adicionando detalhes da turma
    //     doc.Add(new Paragraph("\nPROFESSORA: Marli Isotton\nTURMA: BERÇÁRIO I  TURNO: Tarde\nPERÍODO: 13H às 19H      06 a 11 de setembro", regularFont));
    //
    //     // Adicionando conteúdo principal
    //     doc.Add(new Paragraph("\nRegistros planejamento anterior:", regularFont));
    //     doc.Add(new Paragraph("As atividades foram feitas na sala de aula... (conteúdo completo aqui)", regularFont));
    //
    //     // Adicionando tabelas (exemplo)
    //     var table = new PdfPTable(2)
    //     {
    //         WidthPercentage = 100
    //     }; // Cria uma tabela com 2 colunas
    //     table.AddCell("DATA");
    //     table.AddCell("ATIVIDADES");
    //
    //     table.AddCell("06/09/2024");
    //     table.AddCell("Primeiro momento: Acolhida com a música...");
    //
    //     table.AddCell("09/09/2024");
    //     table.AddCell("Primeiro momento: Acolhida com a música...");
    //
    //     doc.Add(table);
    //
    //     // Adicionando notas finais
    //     doc.Add(new Paragraph("\nObservação: A rotina descrita acima pode ser alterada...", regularFont));
    //
    //     // Fecha o documento
    //     doc.Close();
    //     writer.Close();
}