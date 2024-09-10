using iTextSharp.text;
using iTextSharp.text.pdf;
using Scholar.Planning.Domain.Dtos;
using Scholar.Planning.Domain.Services;

namespace Scholar.Planning.Application.Services;

public class PdfGenerator : IPdfGenerator
{
    private static readonly Font BoldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
    private static readonly Font RegularFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
    private static readonly Font SmallFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8);

    public byte[] GenerateFile()
    {
        using var ms = new MemoryStream();
        var doc = new Document(PageSize.A3, 50, 50, 25, 25);
        var writer = PdfWriter.GetInstance(doc, ms);

        doc.Open();

        CreateHeader(doc);
        CreateSubHeader(doc);
        CreateSection1(doc);
        CreateSection2(doc);

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
        table.SetWidths([1f, 2f, 2f]);

        var img = Image.GetInstance("./Resources/logo.jpg");
        img.ScaleAbsolute(65.205f, 65.205f);
        var imageCell = new PdfPCell(img)
        {
            Border = Rectangle.NO_BORDER,
            HorizontalAlignment = Element.ALIGN_LEFT
        };
        table.AddCell(imageCell);

        var text = new Phrase
        {
            new Chunk("Escola Municipal de Educação Infantil Parque do Sabiá", SmallFont),
            new Chunk("\nRua Gavião, 89 - CEP 93295-605", SmallFont),
            new Chunk("\nBairro Três Marias – Esteio/RS", SmallFont),
            new Chunk("\nFone: (51) 2312-7272 - Celular: (51) 995190107", SmallFont)
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
        img2.ScaleAbsolute(200.1425f, 48.865f);
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
                new Chunk("1. Professora: ", BoldFont),
                new Chunk("Marli Isotton", RegularFont)
            ]
        };
        table.AddCell(cell1);

        var cell2 = new PdfPCell
        {
            Border = Rectangle.LEFT_BORDER,
            Padding = 5,
            Phrase =
            [
                new Chunk("2. Turma: ", BoldFont),
                new Chunk("Berçário I", RegularFont)
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
                new Chunk("Turno: ", BoldFont),
                new Chunk("Tarde", RegularFont)
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
                new Chunk("3. Período: ", BoldFont),
                //Valor dinamico ajustar
                new Chunk("13H às 19H - 06 a 11 de setembro", RegularFont)
            ]
        };
        table.AddCell(cell4);
        doc.Add(table);
        doc.Add(SpaceParagraph());
    }

    private static void CreateSection1(IElementListener doc)
    {
        var table = new PdfPTable(4)
        {
            WidthPercentage = 100
        };
        table.SetWidths([1f, 1f, 1f, 1f]);

        var cell1 = new PdfPCell
        {
            Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER,
            PaddingTop = 10,
            Colspan = 4,
            HorizontalAlignment = Element.ALIGN_CENTER,
            Phrase =
            [
                new Paragraph("Registros planejamento anterior", BoldFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 10f,
                }
            ]
        };
        table.AddCell(cell1);

        var cell2 = new PdfPCell
        {
            Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
            Padding = 10,
            PaddingBottom = 30,
            Colspan = 4,
            HorizontalAlignment = Element.ALIGN_JUSTIFIED,
            VerticalAlignment = Element.ALIGN_JUSTIFIED,
            Phrase =
            [
                new Paragraph(Mock.BigExample, RegularFont)
            ]
        };
        table.AddCell(cell2);

        //dinamico
        var photos = new[] { "foto", "foto", "foto", "foto", "foto", "foto", "foto", "foto" };

        AddImages(photos, table);

        doc.Add(table);
        doc.Add(SpaceParagraph());
    }

    private static void CreateSection2(IElementListener doc)
    {
        var table = new PdfPTable(1)
        {
            WidthPercentage = 100
        };
        table.SetWidths([1f]);

        var cell1 = TitleCell("Intencionalidade Pedagógica");
        table.AddCell(cell1);

        var cell2 = ContentCell(Mock.MiddleExample);
        table.AddCell(cell2);

        var cell3 = TitleCell("Metodologia de aprendizagem");
        table.AddCell(cell3);

        var cell4 = ContentCell(Mock.MiddleExample);
        table.AddCell(cell4);

        var cell5 = TitleCell("Rotina");
        table.AddCell(cell5);

        //dinamico
        var items = new List<KeyValueDto<string, string>>
        {
            new() { Key = "13h às 19h", Value = Mock.SmallExample },
            new() { Key = "13h 30 min", Value = Mock.SmallExample },
            new() { Key = "Momento higiene", Value = Mock.SmallExample },
            new() { Key = "14h Momento do soninho", Value = Mock.SmallExample },
            new() { Key = "Atividade pedagógica", Value = Mock.SmallExample },
            new() { Key = "Momento Livre", Value = Mock.SmallExample },
            new() { Key = "16h Hora da fruta", Value = Mock.SmallExample },
            new() { Key = "Momento higiene", Value = Mock.SmallExample },
            new() { Key = "Momento do soninho", Value = Mock.SmallExample }
        };
        
        CreateTableWithUnorderedList(table, items);

        doc.Add(table);
        doc.Add(SpaceParagraph());
    }

    private static PdfPCell ContentCell(string content)
    {
        return new PdfPCell
        {
            Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
            Padding = 10,
            HorizontalAlignment = Element.ALIGN_JUSTIFIED,
            VerticalAlignment = Element.ALIGN_JUSTIFIED,
            Phrase =
            [
                new Paragraph(content, RegularFont)
            ]
        };
    }

    private static PdfPCell TitleCell(string title)
    {
        return new PdfPCell
        {
            Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER,
            PaddingTop = 10,
            PaddingBottom = 5,
            HorizontalAlignment = Element.ALIGN_CENTER,
            Phrase =
            [
                new Paragraph(title, BoldFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 10f,
                }
            ]
        };
    }

    private static void AddImages(IReadOnlyCollection<string> images, PdfPTable table)
    {
        var count = 0;
        for (var i = 0; i < images.Count; i++)
        {
            var border = count switch
            {
                0 => Rectangle.LEFT_BORDER,
                3 => Rectangle.RIGHT_BORDER,
                _ => Rectangle.NO_BORDER
            };

            if (i >= images.Count - 4)
                border |= Rectangle.BOTTOM_BORDER;

            var img = Image.GetInstance("./Resources/img1.png");
            img.ScaleAbsolute(170.205f, 170.205f);
            var imageCell = new PdfPCell(img)
            {
                Border = border,
                Padding = 2,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_CENTER,
            };

            table.AddCell(imageCell);

            if (count is 3)
                count = 0;
            else
                count++;
        }
    }

    private static void CreateTableWithUnorderedList(PdfPTable table, IEnumerable<KeyValueDto<string, string>> items)
    {
        var list = new List(List.UNORDERED);
        list.SetListSymbol("\u2022 ");

        foreach (var listItem in items.Select(item => new ListItem
                 {
                     new Chunk(item.Key + ": ", BoldFont),
                     new Chunk(item.Value, RegularFont)
                 }))
        {
            list.Add(listItem);
        }

        var cell = new PdfPCell
        {
            Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER,
            Padding = 5
        };
        cell.AddElement(list);
        table.AddCell(cell);
    }

    private static Paragraph SpaceParagraph()
    {
        return new Paragraph
        {
            SpacingBefore = 20f
        };
    }
}