using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using PDFimg.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PDFimg.Helpers
{
    public static class AddImageToPdf
    {
        // Add image to pdf using the 'iText7'.
        public static void Execute(ICollection<string> pdfFiles, ICollection<DataPageModel> dataPage)
        {
            foreach (var inputPdf in pdfFiles)
            {
                if (File.Exists(inputPdf))
                {
                    // Created new folder for output PDF.
                    var pathToInputPdf = Path.GetDirectoryName(inputPdf);
                    var pathToOutputPdf = Path.Combine(pathToInputPdf!, "PDF");
                    Directory.CreateDirectory(pathToOutputPdf);

                    // Created path to output PDF.
                    var pdfName = Path.GetFileName(inputPdf);
                    var outputPdf = Path.Combine(pathToOutputPdf, pdfName);

                    // Reading the input PDF and created the output PDF.
                    PdfDocument pdfDocument = new PdfDocument(new PdfReader(inputPdf), new PdfWriter(outputPdf));

                    // Count pages on the input PDF.
                    int numberOfpages = pdfDocument.GetNumberOfPages();

                    // Iterate through the collection of data page.
                    foreach (var item in dataPage)
                    {
                        // Validate the page numbers.
                        ICollection<int> pageNumbersCollection = ValidatePageNumbers(item.PageNumbers);

                        if (File.Exists(item.PathToImage))
                        {
                            // Creating an image object.
                            Image image = new Image(ImageDataFactory.Create(item.PathToImage));

                            // Iterate through the list of page numbers.
                            foreach (var page in pageNumbersCollection)
                            {
                                if (page <= numberOfpages)
                                {
                                    // Get chosen page on output PDF ('0' defines the last page).
                                    PdfPage pdfPage = (page == 0) ? pdfDocument.GetPage(numberOfpages) : pdfDocument.GetPage(page);

                                    // Setting the position of the image.
                                    image.SetFixedPosition(pdfPage.GetPageSize().GetLeft() + item.PositionX, pdfPage.GetPageSize().GetTop() - image.GetImageHeight() - item.PositionY);

                                    // Adding image to the output PDF.
                                    new Canvas(pdfPage, pdfPage.GetPageSize()).Add(image);
                                }
                            }
                        }
                    }

                    // Close the documents.
                    pdfDocument.Close();
                }
            }
        }

        private static ICollection<int> ValidatePageNumbers(string pageNumbers)
        {
            // Array of characters that will be used as separators.
            char[] separatorsArr = new char[] { ',' };

            // Validate the input string of page numbers and create a sorted list of int.
            int page = default!;
            ICollection<int> pageNumbersCollection = pageNumbers.Split(separatorsArr)
                .Where(s => int.TryParse(s, out page))
                .Select(s => page)
                .OrderBy(s => s)
                .ToList();

            return pageNumbersCollection;
        }
    }
}
